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
#include <thread>
#include <functional>

// internal 
#define LOG_TAG "driver_main"
#include "logging.h"
#include "hid_interface.h"
#include "tools.h"
#include "driver_mapper.h"
#include "vigem_virtual_device.h"
#include "processes.h"
#include "config.h"
#include "emulated_keyboard.h"

//Steam Deck Config
#include "steam_deck_config.h"
#include "steam_deck_hid_commands.h"
#include "steam_deck_controller_interface.h"

#define EXIT_ERROR(code) prepare_exit(); return -code;

using std::chrono::duration_cast;
using std::chrono::milliseconds;
using std::chrono::seconds;
using std::chrono::system_clock;

#define TIME_MILLIS() duration_cast<milliseconds>(system_clock::now().time_since_epoch()).count()

AppConfig config;
ControllerConfig* active_controller;

bool emulating = true;
bool _lst_emulating = true;
unsigned long long last_process_check;
unsigned long long last_conf_load;

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

unsigned long long _last_lizard_mode_update = TIME_MILLIS();
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

		SDCInput* input = (SDCInput*)data;

		if (emulating)
			map_driver_hid_input(input);

		if (TIME_MILLIS() - _last_lizard_mode_update > 900)
		{
			sdc_set_lizard_mode(!active_controller->disable_lizard_mode);
			_last_lizard_mode_update = TIME_MILLIS();
		}

		handle_button_actions(input);
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

void manager_thread()
{
	unsigned long last_conf_load = TIME_MILLIS();
	unsigned long last_em_check = TIME_MILLIS();

	while (true)
	{
		unsigned long time = TIME_MILLIS();
		if (time - last_conf_load > 1000)
		{
			load_conf(&config);
			last_conf_load = TIME_MILLIS();
		}

		if (time - last_em_check > 500)
		{
			emulating = check_should_emulate(&config);
			active_controller = get_active_config(&config);
			last_em_check = TIME_MILLIS();
		}

		if (_lst_emulating != emulating)
			handle_emulating_changed();

		Sleep(100);
	}
}

void vigem_raiseEvent(PVIGEM_CLIENT client, PVIGEM_TARGET target,
	uint8_t largeMotor, uint8_t smallMotor, uint8_t ledNumber,
	LPVOID userData)
{
	LOG_INFO("LM: %d, SM: %d", largeMotor, smallMotor);
	uint8_t value = max(largeMotor, smallMotor);
	sdc_set_haptic(value);
}


int main()
{
	if (!load_conf(&config))
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

	vigem_register_notifications(&vigem_raiseEvent);

	//Everything looks good. Now we can try to grab some input and pass it to the mapper.
	LOG_INFO("Driver is successfully started. Happy gaming!");

	emulating = check_should_emulate(&config);
	active_controller = get_active_config(&config);

	auto threadObj = thread(&manager_thread);
	threadObj.detach();

	while (true)
	{
		long start = TIME_MILLIS();
		driver_mainloop();
		long end = TIME_MILLIS();

		if (end - start < 5)
			Sleep((5 - (end - start)));
	}


	LOG_ERROR("Somehow we exited an infinite loop. Exiting now gracefully.");

	prepare_exit();
	return 0;
}