#define WIN32_LEAN_AND_MEAN
#define _CRT_SECURE_NO_WARNINGS


#ifdef WIN32_LEAN_AND_MEAN
#include <windows.h>
#endif

// standard libraries
#include <iostream>
#include <Xinput.h>
#include <stdio.h>
#include <stdlib.h>

// internal 
#define LOG_TAG "sd_controller"
#include "logging.h"
#include "hid_interface.h"

//Steam Deck Config
#include "steam_deck_config.h"
#include "steam_deck_hid_commands.h"


bool sdc_get_serial(char* buffer)
{
	uint8_t data[64] = { COMMAND_REQUEST_SERIAL_NUMBER }; // Command to grab the 
	uint8_t* response;
	response = hid_request(data, -64);

	if (response == NULL) {
		return false;
	}

	strncpy(buffer, (const char*)&data[3], MAX_SERIAL_LEN);

	if (strlen(buffer) == 0)
		return false;

	return true;
}


bool sdc_set_lizard_mode(bool enabled)
{
	uint8_t data[64] = { COMMAND_SET_LIZARD_MODE, (uint8_t)(enabled ? 0x01 : 0x00) }; // Command to set lizard mode

	if (hid_request(data, -64) == NULL) {
		return false;
	}

	return true;
}

bool sdc_configure(uint16_t idle_timeout, bool gyro_enabled)
{
	uint8_t gyro_and_timeout[64] = {
		// Header
		PT_CONFIGURE, PL_CONFIGURE, CT_CONFIGURE,
		// Idle timeout
		(uint8_t)(idle_timeout & 0xFF), (uint8_t)((idle_timeout & 0xFF00) >> 8),
		// unknown1
		0x18, 0x00, 0x00, 0x31, 0x02, 0x00, 0x08, 0x07, 0x00, 0x07, 0x07, 0x00, 0x30,
		// Gyros
		(uint8_t)(gyro_enabled ? 0x1c : 0),
		// unknown2:
		0x00, 0x2e,
	};
	if (hid_request(gyro_and_timeout, -64) == NULL)
		goto configure_fail;
	return true;
configure_fail:
	return false;
}

bool sdc_clear_mappings() {
	uint8_t data[64] = { PT_CLEAR_MAPPINGS, 0x01 };
	if (hid_request(data, -64) == NULL) {
		return false;
	}
	return true;
}


bool sdc_read_controller_data(SDCInput* input)
{
	return true;
}