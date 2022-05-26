#define WIN32_LEAN_AND_MEAN

#define LOG_TAG "driver_mapper"
#include "logging.h"
#include "tools.h"

#include <stdint.h>
#include "steam_deck_controller_interface.h"
#include "steam_deck_hid_commands.h"
#include "vigem_virtual_device.h"
#include "steam_deck_config.h"

uint8_t interrupt_count = 0;

#define MAP_VALUE(A,B,C) (double)((double)A / B * C);

void map_driver_hid_input(uint8_t* data, int length)
{
	SDCInput *input = (SDCInput*) data;
	XUSB_REPORT report;

	//Map trigger values from 0 ... 0x7FFF (Deck input) to 0 ... 255 for XBox Controller input
	report.bLeftTrigger = (BYTE)MAP_VALUE(input->ltrig, MAX_TRIGGER_VALUE, 255);
	report.bRightTrigger = (BYTE)MAP_VALUE(input->rtrig, MAX_TRIGGER_VALUE, 255);

	report.bLeftTrigger = (BYTE)MAP_VALUE(input->ltrig, MAX_TRIGGER_VALUE, 255);
	report.bRightTrigger = (BYTE)MAP_VALUE(input->rtrig, MAX_TRIGGER_VALUE, 255);

	//Select left touchpad as left stick if touched and left stick is not touched
	if ((input->buttons1 & BTN_LPAD_TOUCH) 
		&& !(input->buttons4 & BTN_LSTICK_TOUCH))
	{
		memcpy_s(&report.sThumbLX, sizeof(SHORT), &input->lpad_x, sizeof(SHORT));
		memcpy_s(&report.sThumbLY, sizeof(SHORT), &input->lpad_y, sizeof(SHORT));
	}
	else
	{
		memcpy_s(&report.sThumbLX, sizeof(SHORT), &input->lthumb_x, sizeof(SHORT));
		memcpy_s(&report.sThumbLY, sizeof(SHORT), &input->lthumb_y, sizeof(SHORT));
	}

	//Select right touchpad as right stick if touched and right stick is not touched
	if ((input->buttons1 & BTN_RPAD_TOUCH)
		&& !(input->buttons4 & BTN_RSTICK_TOUCH))
	{
		memcpy_s(&report.sThumbRX, sizeof(SHORT), &input->rpad_x, sizeof(SHORT));
		memcpy_s(&report.sThumbRY, sizeof(SHORT), &input->rpad_y, sizeof(SHORT));
	}
	else
	{
		memcpy_s(&report.sThumbRX, sizeof(SHORT), &input->rthumb_x, sizeof(SHORT));
		memcpy_s(&report.sThumbRY, sizeof(SHORT), &input->rthumb_y, sizeof(SHORT));
	}

#ifdef DEBUG_BUTTON_PRESS
	if (input->buttons0 & BTN_A)
		LOG_DEBUG("Button A is pressed!");

	if (input->buttons0 & BTN_X)
		LOG_DEBUG("Button X is pressed!");

	if (input->buttons0 & BTN_B)
		LOG_DEBUG("Button B is pressed!");

	if (input->buttons0 & BTN_Y)
		LOG_DEBUG("Button Y is pressed!");

	if (input->buttons0 & BTN_L1)
		LOG_DEBUG("Button L1 is pressed!");

	if (input->buttons0 & BTN_L2)
		LOG_DEBUG("Button L2 is pressed!");

	if (input->buttons0 & BTN_R1)
		LOG_DEBUG("Button R1 is pressed!");

	if (input->buttons0 & BTN_R2)
		LOG_DEBUG("Button R2 is pressed!");

	if (input->buttons1 & BTN_LSTICK_PRESS)
		LOG_DEBUG("Button LSTICK_PRESS is pressed!");

	if (input->buttons1 & BTN_LPAD_TOUCH)
		LOG_DEBUG("Button LPAD_TOUCH is pressed!");

	if (input->buttons1 & BTN_LPAD_PRESS)
		LOG_DEBUG("Button LPAD_PRESS is pressed!");

	if (input->buttons1 & BTN_RPAD_TOUCH)
		LOG_DEBUG("Button RPAD_TOUCH is pressed!");

	if (input->buttons1 & BTN_RPAD_PRESS)
		LOG_DEBUG("Button RPAD_PRESS is pressed!");

	if (input->buttons1 & BTN_R5)
		LOG_DEBUG("Button R5 is pressed!");

	if (input->buttons0 & BTN_L5)
		LOG_DEBUG("Button L5 is pressed!");

	if (input->buttons0 & BTN_OPTIONS)
		LOG_DEBUG("Button OPTIONS is pressed!");

	if (input->buttons0 & BTN_STEAM)
		LOG_DEBUG("Button STEAM is pressed!");

	if (input->buttons0 & BTN_MENU)
		LOG_DEBUG("Button MENU is pressed!");

	if (input->buttons0 & BTN_DPAD_DOWN)
		LOG_DEBUG("Button DPAD_DOWN is pressed!");

	if (input->buttons0 & BTN_DPAD_LEFT)
		LOG_DEBUG("Button DPAD_LEFT is pressed!");

	if (input->buttons0 & BTN_DPAD_RIGHT)
		LOG_DEBUG("Button DPAD_RIGHT is pressed!");

	if (input->buttons0 & BTN_DPAD_UP)
		LOG_DEBUG("Button DPAD_UP is pressed!");

	if (input->buttons2 & BTN_RSTICK_PRESS)
		LOG_DEBUG("Button RSTICK_PRESS is pressed!");

	if (input->buttons4 & BTN_LSTICK_TOUCH)
		LOG_DEBUG("Button LSTICK_TOUCH is pressed!");

	if (input->buttons4 & BTN_RSTICK_TOUCH)
		LOG_DEBUG("Button RSTICK_TOUCH is pressed!");

	if (input->buttons4 & BTN_R4)
		LOG_DEBUG("Button R4 is pressed!");

	if (input->buttons4 & BTN_L4)
		LOG_DEBUG("Button L4 is pressed!");

	if (input->buttons5 & BTN_QUICK_ACCESS)
		LOG_DEBUG("Button QUICK_ACCESS is pressed!");
#endif

	report.wButtons = 0x0;

	if (input->buttons0 & BTN_A)
		report.wButtons |= XUSB_GAMEPAD_A;

	if (input->buttons0 & BTN_X)
		report.wButtons |= XUSB_GAMEPAD_X;

	if (input->buttons0 & BTN_B)
		report.wButtons |= XUSB_GAMEPAD_B;

	if (input->buttons0 & BTN_Y)
		report.wButtons |= XUSB_GAMEPAD_Y;

	if (input->buttons0 & BTN_L1)
		report.wButtons |= XUSB_GAMEPAD_LEFT_SHOULDER;

	//if (input->buttons0 & BTN_L2)
	//	report.wButtons |= XUSB_GAMEPAD_A;

	if (input->buttons0 & BTN_R1)
		report.wButtons |= XUSB_GAMEPAD_RIGHT_SHOULDER;

	//if (input->buttons0 & BTN_R2)
	//	report.wButtons |= XUSB_GAMEPAD_A;

	if (input->buttons1 & BTN_LSTICK_PRESS)
		report.wButtons |= XUSB_GAMEPAD_LEFT_THUMB;

	//if (input->buttons1 & BTN_LPAD_TOUCH)
	//	report.wButtons |= XUSB_GAMEPAD_A;

	if (input->buttons1 & BTN_LPAD_PRESS)
		report.wButtons |= XUSB_GAMEPAD_LEFT_THUMB;

	//if (input->buttons1 & BTN_RPAD_TOUCH)
	//	report.wButtons |= XUSB_GAMEPAD_A;

	if (input->buttons1 & BTN_RPAD_PRESS)
		report.wButtons |= XUSB_GAMEPAD_RIGHT_THUMB;

	if (input->buttons1 & BTN_R5)
		report.wButtons |= XUSB_GAMEPAD_Y;

	if (input->buttons0 & BTN_L5)
		report.wButtons |= XUSB_GAMEPAD_X;

	if (input->buttons0 & BTN_OPTIONS)
		report.wButtons |= XUSB_GAMEPAD_START;

	if (input->buttons0 & BTN_STEAM)
		report.wButtons |= XUSB_GAMEPAD_GUIDE;

	if (input->buttons0 & BTN_MENU)
		report.wButtons |= XUSB_GAMEPAD_BACK;

	if (input->buttons0 & BTN_DPAD_DOWN)
		report.wButtons |= XUSB_GAMEPAD_DPAD_DOWN;

	if (input->buttons0 & BTN_DPAD_LEFT)
		report.wButtons |= XUSB_GAMEPAD_DPAD_LEFT;

	if (input->buttons0 & BTN_DPAD_RIGHT)
		report.wButtons |= XUSB_GAMEPAD_DPAD_RIGHT;

	if (input->buttons0 & BTN_DPAD_UP)
		report.wButtons |= XUSB_GAMEPAD_DPAD_UP;

	if (input->buttons2 & BTN_RSTICK_PRESS)
		report.wButtons |= XUSB_GAMEPAD_RIGHT_THUMB;

	//if (input->buttons4 & BTN_LSTICK_TOUCH)
	//	report.wButtons |= XUSB_GAMEPAD_A;

	//if (input->buttons4 & BTN_RSTICK_TOUCH)
	//	report.wButtons |= XUSB_GAMEPAD_A;

	if (input->buttons4 & BTN_R4)
		report.wButtons |= XUSB_GAMEPAD_B;

	if (input->buttons4 & BTN_L4)
		report.wButtons |= XUSB_GAMEPAD_A;

	//if (input->buttons5 & BTN_QUICK_ACCESS)
	//	report.wButtons |= XUSB_GAMEPAD_A;



	//Now update the virtual controller status
	vigem_update_device(report);

	// We need to disable the lizard mode periodically.
	// TODO: Nice detection whether we are ingame, so we can still use the lizard mode on desktop.
	// For now we will just get rid of it forever.

	if (interrupt_count % DISABLE_LIZARD_MODE_EVERY_X_INT == 0)
	{
		sdc_set_lizard_mode(false);
	}
	interrupt_count++;
}
