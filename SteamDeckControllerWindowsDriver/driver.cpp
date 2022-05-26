#define WIN32_LEAN_AND_MEAN
#ifdef DEBUG

//Enable to see hid input data as hex in console
//#define DEBUG_HID_INPUT

#endif // DEBUG

#ifdef WIN32_LEAN_AND_MEAN
#include <windows.h>
#endif

// standard libraries
#include <iostream>
#include <stdio.h>
#include <stdlib.h>
#include <chrono>
#include <vector>
#include <fstream>
#include <sstream>
#include <string>
#include <filesystem>
#include <direct.h>

// internal 
#define LOG_TAG "driver_main"
#include "logging.h"
#include "hid_interface.h"
#include "tools.h"
#include "driver_mapper.h"
#include "vigem_virtual_device.h"
#include "processes.h"
#include "config.h"

//Steam Deck Config
#include "steam_deck_config.h"
#include "steam_deck_hid_commands.h"
#include "steam_deck_controller_interface.h"

#define EXIT_ERROR(code) prepare_exit(); return -code;

#define TIME_MILLIS() (long)(time(nullptr) * 1000);

AppConfig config;

std::vector<std::string> blacklisted_processes = std::vector<std::string>();
std::vector<std::string> whitelisted_processes = std::vector<std::string>();
bool emulating = true;
bool _lst_emulating = true;
long last_process_check;
long last_conf_load;

void prepare_exit()
{
	LOG_INFO("Closing hid device.");
	hid_close_device();
	LOG_INFO("Closing vigmem virtual controller device.");
	vigem_close_device();
}

void signal_received(int signum) {
	const char* signal = "UNKNOWN";

	switch (signum)
	{
	case 1:
		signal = "SIGABRT";
	case 2:
		signal = "SIGFPE";
	case 3:
		signal = "SIGILL";
	case 4:
		signal = "SIGINT";
	case 5:
		signal = "SIGSEGV";
	case 6:
		signal = "SIGTERM";
	}


	LOG_INFO("Interrupt signal %s received.", signal);

	// cleanup and close up stuff here  
	// terminate program  

	prepare_exit();

	exit(signum);
}

void driver_mainloop()
{
	uint8_t data[64];
	int length;
	while (length = hid_read_timeout(data, sizeof(data), 0) > 0)
	{
#ifdef DEBUG_HID_INPUT
		char hex_str[64 * 2 + 1];
		tool_bytes2hex(data, hex_str, length);
		LOG_DEBUG("Data read from hid device: %s", hex_str);
#endif
		map_driver_hid_input(data, length);

	}
}

void check_should_emulate()
{
	last_process_check = TIME_MILLIS();

	if (config.mode == APP_MODE_WHITELIST)
	{
		bool whitelisted = false;
		for (auto it = std::begin(whitelisted_processes); it != std::end(whitelisted_processes) && !whitelisted; ++it) {
			whitelisted = sys_check_process_running((*it).c_str());
		}
		emulating = whitelisted;
	}
	else
	{
		bool blacklisted = false;
		for (auto it = std::begin(blacklisted_processes); it != std::end(blacklisted_processes) && !blacklisted; ++it) {
			blacklisted = sys_check_process_running((*it).c_str());
		}
		emulating = !blacklisted;
	}
}

void handle_emulating_changed()
{
	_lst_emulating = emulating;

	if (!emulating)
	{
		LOG_INFO("Stopping gamecontroller emulation");
		if (!vigem_reset_device())
		{
			LOG_ERROR("Failed to reset virtual controller state.");
		}
	}
	else
	{
		LOG_INFO("Starting gamecontroller emulation");
	}
}

void process_config(std::string key, std::string value)
{
	if (key == "blacklist")
	{
		blacklisted_processes.push_back(value);
		return;
	}

	if (key == "whitelist")
	{
		whitelisted_processes.push_back(value);
		return;
	}

	if (key == "mode")
	{
		if (value == "whitelist")
		{
			config.mode = APP_MODE_WHITELIST;
		}
		else
		{
			config.mode = APP_MODE_BLACKLIST;
		}
		return;
	}

	LOG_WARN("Unknown config key: %s=%s", key.c_str(), value.c_str());
}

bool load_conf()
{
	last_conf_load = TIME_MILLIS();

	LOG_INFO("Loading config");

	std::ifstream file(CONF_FILE);
	if (file.is_open()) {

		blacklisted_processes.clear();
		whitelisted_processes.clear();

		std::string line;
		while (std::getline(file, line))
		{
			//Ignore comments in conf file
			if (line.c_str()[0] == '#')
				continue;

			//Ignore empty lines
			if (line.length() == 0)
				continue;


			LOG_DEBUG("%s", line.c_str());
			std::istringstream is_line(line);
			std::string key;
			if (std::getline(is_line, key, '='))
			{
				std::string value;
				if (std::getline(is_line, value))
					process_config(key, value);
			}
		}
		return true;
	} 
	else
	{
		LOG_ERROR("Unable to open config file: %s", CONF_FILE);
		return false;
	}
}

int main()
{
	if (!load_conf())
	{
		EXIT_ERROR(0x1f);
	}

	if (!hid_open_device(VENDOR_ID, PRODUCT_ID))
	{
		// The hid device of the decks controller could not be opened. 
		// We cannot continue without data, therefore exit with error 0x01.
		// Solution is most likely to close steam.

		LOG_ERROR("Could not open hid device! Check if steam is running! If so, close it.");
		EXIT_ERROR(0x01);
	}

	LOG_INFO("Successfully opened device.");

	//Lets try to get the serial number of the steam deck controller.
	char serial[MAX_SERIAL_LEN];

	if (!sdc_get_serial(serial))
	{
		LOG_ERROR("Failed to retrieve serial number from device");
		EXIT_ERROR(0x02);
	}

	LOG_INFO("Serial of connected controller: %s", serial);

	//Lets try to disable the lizard mode of the controller (mouse and keyboard input instead of controller controls)

	if (!sdc_set_lizard_mode(false))
	{
		LOG_ERROR("Failed to disable lizard mode");
		EXIT_ERROR(0x03);
	}

	//Now clear any controller mappings

	if (!sdc_clear_mappings())
	{
		LOG_ERROR("Failed to clear mappings");
		EXIT_ERROR(0x04);
	}

	//Lets try to configure the controller mode
	// idle_timeout: 10 minutes
	// gyro_enabled: true
	if (!sdc_configure(10 * 60, true))
	{
		LOG_ERROR("Failed to configure controller");
		EXIT_ERROR(0x05);
	}

	//The steam deck controller has been set up. Now we can connect to ViGEm and create a virtual controller
	if (!vigem_open_device())
	{
		LOG_ERROR("Failed to open and create ViGEm device");
		EXIT_ERROR(0x06);

	}

	//Everything looks good. Now we can try to grab some input and pass it to the mapper.
	LOG_INFO("Driver is successfully started. Happy gaming!");

	check_should_emulate();

	while (true)
	{
		long start = TIME_MILLIS();
		if (emulating)
			driver_mainloop();
		long end = TIME_MILLIS();
		if (end - start < 10)
			Sleep((10 - (end - start)));

		if (end - last_process_check > 5000)
			check_should_emulate();

		if (end - last_conf_load > 15000)
			load_conf();

		if (_lst_emulating != emulating)
		{
			handle_emulating_changed();
		}
	}


	LOG_ERROR("Somehow we exited an infinite loop. Exiting now gracefully.");

	prepare_exit();
	return 0;
}