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

// internal 
#define LOG_TAG "driver_main"
#include "logging.h"
#include "hid_interface.h"
#include "tools.h"
#include "driver_mapper.h"
#include "vigem_virtual_device.h"

//Steam Deck Config
#include "steam_deck_config.h"
#include "steam_deck_hid_commands.h"
#include "steam_deck_controller_interface.h"
#include <chrono>

#define EXIT_ERROR(code) prepare_exit(); return -code;

#define TIME_MILLIS() (long)time(nullptr);

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

int main()
{
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

	while (true)
	{
		long start = TIME_MILLIS();
		driver_mainloop();
		long end = TIME_MILLIS();
		if (end - start < 10)
			Sleep((10 - (end - start)));
	}


	LOG_ERROR("Somehow we exited an infinite loop. Exiting now gracefully.");

	prepare_exit();
	return 0;
}