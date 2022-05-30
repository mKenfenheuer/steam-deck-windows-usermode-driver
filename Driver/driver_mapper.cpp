#define WIN32_LEAN_AND_MEAN

#define LOG_TAG "driver_mapper"
#include "logging.h"
#include "tools.h"
#include "config.h"

#include <stdint.h>
#include "steam_deck_controller_interface.h"
#include "steam_deck_hid_commands.h"
#include "vigem_virtual_device.h"
#include "steam_deck_config.h"
#include "emulated_keyboard.h"

extern ControllerConfig* active_controller;
extern AppConfig config;

#define MAP_VALUE(A,B,C) (double)((double)A / B * C);

XUSB_BUTTON map_report_button(EmulatedButton value)
{
	if (value == EmBtn_X) return XUSB_GAMEPAD_X;
	if (value == EmBtn_Y) return XUSB_GAMEPAD_Y;
	if (value == EmBtn_A) return XUSB_GAMEPAD_A;
	if (value == EmBtn_B) return XUSB_GAMEPAD_B;
	if (value == EmBtn_RB) return XUSB_GAMEPAD_RIGHT_SHOULDER;
	if (value == EmBtn_LB) return XUSB_GAMEPAD_LEFT_SHOULDER;
	if (value == EmBtn_LS) return XUSB_GAMEPAD_LEFT_THUMB;
	if (value == EmBtn_RS) return XUSB_GAMEPAD_RIGHT_THUMB;
	if (value == EmBtn_Back) return XUSB_GAMEPAD_BACK;
	if (value == EmBtn_Start) return XUSB_GAMEPAD_START;
	if (value == EmBtn_Guide) return XUSB_GAMEPAD_GUIDE;
	if (value == EmBtn_DpadUp) return XUSB_GAMEPAD_DPAD_UP;
	if (value == EmBtn_DpadDown) return XUSB_GAMEPAD_DPAD_DOWN;
	if (value == EmBtn_DpadLeft) return XUSB_GAMEPAD_DPAD_LEFT;
	if (value == EmBtn_DpadRight) return XUSB_GAMEPAD_DPAD_RIGHT;
	return (XUSB_BUTTON)0x0000;
}

bool is_button_pressed(SDCInput* input, HardwareButton hwButton)
{
	if (input->buttons0 & BTN_A && hwButton == HwBtn_A) return true;
	if (input->buttons0 & BTN_X && hwButton == HwBtn_X) return true;
	if (input->buttons0 & BTN_B && hwButton == HwBtn_B) return true;
	if (input->buttons0 & BTN_Y && hwButton == HwBtn_Y) return true;
	if (input->buttons0 & BTN_L1 && hwButton == HwBtn_L1) return true;
	if (input->buttons0 & BTN_L2 && hwButton == HwBtn_L2) return true;
	if (input->buttons0 & BTN_R1 && hwButton == HwBtn_R1) return true;
	if (input->buttons0 & BTN_R2 && hwButton == HwBtn_R2) return true;
	if (input->buttons1 & BTN_LSTICK_PRESS && hwButton == HwBtn_LStickPress) return true;
	if (input->buttons1 & BTN_LPAD_TOUCH && hwButton == HwBtn_LPadTouch) return true;
	if (input->buttons1 & BTN_LPAD_PRESS && hwButton == HwBtn_LPadPress) return true;
	if (input->buttons1 & BTN_RPAD_TOUCH && hwButton == HwBtn_RPadTouch) return true;
	if (input->buttons1 & BTN_RPAD_PRESS && hwButton == HwBtn_RPadPress) return true;
	if (input->buttons1 & BTN_R5 && hwButton == HwBtn_R5) return true;
	if (input->buttons0 & BTN_L5 && hwButton == HwBtn_L5) return true;
	if (input->buttons0 & BTN_OPTIONS && hwButton == HwBtn_Options) return true;
	if (input->buttons0 & BTN_STEAM && hwButton == HwBtn_Steam) return true;
	if (input->buttons0 & BTN_MENU && hwButton == HwBtn_Menu) return true;
	if (input->buttons0 & BTN_DPAD_DOWN && hwButton == HwBtn_DpadDown) return true;
	if (input->buttons0 & BTN_DPAD_LEFT && hwButton == HwBtn_DpadLeft) return true;
	if (input->buttons0 & BTN_DPAD_RIGHT && hwButton == HwBtn_DpadRight) return true;
	if (input->buttons0 & BTN_DPAD_UP && hwButton == HwBtn_DpadUp) return true;
	if (input->buttons2 & BTN_RSTICK_PRESS && hwButton == HwBtn_RStickPress) return true;
	if (input->buttons4 & BTN_LSTICK_TOUCH && hwButton == HwBtn_LStickTouch) return true;
	if (input->buttons4 & BTN_RSTICK_TOUCH && hwButton == HwBtn_RStickTouch) return true;
	if (input->buttons4 & BTN_R4 && hwButton == HwBtn_R4) return true;
	if (input->buttons4 & BTN_L4 && hwButton == HwBtn_L4) return true;
	if (input->buttons5 & BTN_QUICK_ACCESS && hwButton == HwBtn_QuickAccess) return true;
}

SDCInput _lst_input;
void handle_button_actions(SDCInput* input) {
	if (is_button_pressed(input, config.button_actions.OpenWindowsGameBar)
		&& !is_button_pressed(&_lst_input, config.button_actions.OpenWindowsGameBar))
	{
		send_key(VK_BACK, true);
		send_key('g');
		Sleep(10);
		send_key(VK_BACK, false);
	}
	memcpy(&_lst_input, input, sizeof(SDCInput));
}


void map_hardware_axis(int16_t* value, SDCInput* input, XUSB_REPORT* report, EmulatedAxisConfig* config)
{
	//return if activation button is not null and is not pressed
	if (config->activation_button != HwBtn_None && !is_button_pressed(input, config->activation_button))
		return;

	//Invert if nescessary
	uint16_t correct_val = *value;
	if (config->inverted)
		correct_val = UINT16_MAX - correct_val;

	switch (config->emulated_axis)
	{
	case EmAx_LeftStickX:
		//Just do memcpy as the two are the same size/bit representation
		memcpy_s(&report->sThumbLX, sizeof(SHORT), &correct_val, sizeof(SHORT));
		break;
	case EmAx_RightStickX:
		//Just do memcpy as the two are the same size/bit representation
		memcpy_s(&report->sThumbRX, sizeof(SHORT), &correct_val, sizeof(SHORT));
		break;
	case EmAx_LeftStickY:
		//Just do memcpy as the two are the same size/bit representation
		memcpy_s(&report->sThumbLY, sizeof(SHORT), &correct_val, sizeof(SHORT));
		break;
	case EmAx_RightStickY:
		//Just do memcpy as the two are the same size/bit representation
		memcpy_s(&report->sThumbRY, sizeof(SHORT), &correct_val, sizeof(SHORT));
		break;
	case EmAx_LT:
		//Map trigger values from 0 ... 0x7FFF (Deck input) to 0 ... 255 for XBox Controller input
		report->bLeftTrigger = (BYTE)MAP_VALUE(correct_val, MAX_TRIGGER_VALUE, 255);
		break;
	case EmAx_RT:
		//Map trigger values from 0 ... 0x7FFF (Deck input) to 0 ... 255 for XBox Controller input
		report->bRightTrigger = (BYTE)MAP_VALUE(correct_val, MAX_TRIGGER_VALUE, 255);
		break;
	}
}

void map_driver_hid_input(SDCInput* input)
{
	XUSB_REPORT report = XUSB_REPORT();

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
	map_hardware_axis(&input->accel_x, input, &report, &active_controller->axis_mapping.GyroAccelX);
	map_hardware_axis(&input->accel_y, input, &report, &active_controller->axis_mapping.GyroAccelY);
	map_hardware_axis(&input->accel_z, input, &report, &active_controller->axis_mapping.GyroAccelZ);
	map_hardware_axis(&input->gyaw, input, &report, &active_controller->axis_mapping.GyroYaw);
	map_hardware_axis(&input->groll, input, &report, &active_controller->axis_mapping.GyroRoll);
	map_hardware_axis(&input->gpitch, input, &report, &active_controller->axis_mapping.GyroPitch);
	map_hardware_axis(&input->lpad_x, input, &report, &active_controller->axis_mapping.LeftPadX);
	map_hardware_axis(&input->lpad_y, input, &report, &active_controller->axis_mapping.LeftPadY);
	map_hardware_axis(&input->lpad_pressure, input, &report, &active_controller->axis_mapping.LeftPadPressure);
	map_hardware_axis(&input->rpad_x, input, &report, &active_controller->axis_mapping.RightPadX);
	map_hardware_axis(&input->rpad_y, input, &report, &active_controller->axis_mapping.RightPadY);
	map_hardware_axis(&input->rpad_pressure, input, &report, &active_controller->axis_mapping.RightPadPressure);
	map_hardware_axis(&input->lthumb_x, input, &report, &active_controller->axis_mapping.LeftStickX);
	map_hardware_axis(&input->lthumb_y, input, &report, &active_controller->axis_mapping.LeftStickY);
	map_hardware_axis(&input->rthumb_x, input, &report, &active_controller->axis_mapping.RightStickX);
	map_hardware_axis(&input->rthumb_y, input, &report, &active_controller->axis_mapping.RightStickY);
	map_hardware_axis(&input->ltrig, input, &report, &active_controller->axis_mapping.L2);
	map_hardware_axis(&input->rtrig, input, &report, &active_controller->axis_mapping.R2);

	report.wButtons = 0x0;

	if (input->buttons0 & BTN_A)
		report.wButtons |= map_report_button(active_controller->button_mapping.BtnA);

	if (input->buttons0 & BTN_X)
		report.wButtons |= map_report_button(active_controller->button_mapping.BtnX);

	if (input->buttons0 & BTN_B)
		report.wButtons |= map_report_button(active_controller->button_mapping.BtnB);

	if (input->buttons0 & BTN_Y)
		report.wButtons |= map_report_button(active_controller->button_mapping.BtnY);

	if (input->buttons0 & BTN_L1)
		report.wButtons |= map_report_button(active_controller->button_mapping.BtnL1);

	if (input->buttons0 & BTN_L2)
		report.wButtons |= map_report_button(active_controller->button_mapping.BtnL2);

	if (input->buttons0 & BTN_R1)
		report.wButtons |= map_report_button(active_controller->button_mapping.BtnR1);

	if (input->buttons0 & BTN_R2)
		report.wButtons |= map_report_button(active_controller->button_mapping.BtnR2);

	if (input->buttons1 & BTN_LSTICK_PRESS)
		report.wButtons |= map_report_button(active_controller->button_mapping.BtnLStickPress);

	if (input->buttons1 & BTN_LPAD_TOUCH)
		report.wButtons |= map_report_button(active_controller->button_mapping.BtnLPadTouch);

	if (input->buttons1 & BTN_LPAD_PRESS)
		report.wButtons |= map_report_button(active_controller->button_mapping.BtnLPadPress);

	if (input->buttons1 & BTN_RPAD_TOUCH)
		report.wButtons |= map_report_button(active_controller->button_mapping.BtnRPadTouch);

	if (input->buttons1 & BTN_RPAD_PRESS)
		report.wButtons |= map_report_button(active_controller->button_mapping.BtnRPadPress);

	if (input->buttons1 & BTN_R5)
		report.wButtons |= map_report_button(active_controller->button_mapping.BtnR5);

	if (input->buttons0 & BTN_L5)
		report.wButtons |= map_report_button(active_controller->button_mapping.BtnL5);

	if (input->buttons0 & BTN_OPTIONS)
		report.wButtons |= map_report_button(active_controller->button_mapping.BtnOptions);

	if (input->buttons0 & BTN_STEAM)
		report.wButtons |= map_report_button(active_controller->button_mapping.BtnSteam);

	if (input->buttons0 & BTN_MENU)
		report.wButtons |= map_report_button(active_controller->button_mapping.BtnMenu);

	if (input->buttons0 & BTN_DPAD_DOWN)
		report.wButtons |= map_report_button(active_controller->button_mapping.BtnDpadDown);

	if (input->buttons0 & BTN_DPAD_LEFT)
		report.wButtons |= map_report_button(active_controller->button_mapping.BtnDpadLeft);

	if (input->buttons0 & BTN_DPAD_RIGHT)
		report.wButtons |= map_report_button(active_controller->button_mapping.BtnDpadRight);

	if (input->buttons0 & BTN_DPAD_UP)
		report.wButtons |= map_report_button(active_controller->button_mapping.BtnDpadUp);

	if (input->buttons2 & BTN_RSTICK_PRESS)
		report.wButtons |= map_report_button(active_controller->button_mapping.BtnRStickPress);

	if (input->buttons4 & BTN_LSTICK_TOUCH)
		report.wButtons |= map_report_button(active_controller->button_mapping.BtnLStickTouch);

	if (input->buttons4 & BTN_RSTICK_TOUCH)
		report.wButtons |= map_report_button(active_controller->button_mapping.BtnRStickTouch);
	
	if (input->buttons4 & BTN_R4)
		report.wButtons |= map_report_button(active_controller->button_mapping.BtnR4);

	if (input->buttons4 & BTN_L4)
		report.wButtons |= map_report_button(active_controller->button_mapping.BtnL4);

	if (input->buttons5 & BTN_QUICK_ACCESS)
		report.wButtons |= map_report_button(active_controller->button_mapping.BtnQuickAccess);



	//Now update the virtual controller status
	vigem_update_device(report);
}
