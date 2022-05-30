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

#define MAP_VALUE(A,B,C) (double)((double)A / B * C);


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


//0x8e -> Mouse Buttons disabled, touchpad enabled.
//0x85 -> Mouse Buttons work, touchpad work.
//0x83 -> Mouse Buttons work, touchpad work.
//0x81 -> All Disabled

typedef struct Haptic {
	uint8_t		packet_type = 0x8f;
	uint8_t		len = 0x07;
	uint8_t		position = 1;
	uint16_t	amplitude;
	uint16_t	period;
	uint16_t	cunt;
} Haptic;

bool sdc_set_lizard_mode(bool enabled)
{
	if (!enabled)
	{
		uint8_t data[64] = { 0x87, 0x03, 0x08, 0x07 };

		if (hid_request(data, -64) == NULL) {
			return false;
		}
	}
	else 
	{
		uint8_t data[64] = { 0x85, 0x00 };

		if (hid_request(data, -64) == NULL) {
			return false;
		}

		data[0] = 0x8e;

		if (hid_request(data, -64) == NULL) {
			return false;
		}
	}
	return true;
}

bool sdc_set_haptic(uint8_t amount)
{

	uint8_t data[64] = { 0x8f, 0x07, 0x0 }; // Command to set lizard mode
	Haptic haptic;


	if (amount > 0)
	{
		amount = MAP_VALUE(amount, 0xff, 0x85);
		haptic.amplitude = 0x0000 + 0x85 - amount + 0x10;
		haptic.period = 0x0005;
		haptic.cunt = 1;
	}


	memcpy(&data, &haptic, sizeof(Haptic));

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