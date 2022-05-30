#define WIN32_LEAN_AND_MEAN

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
#include "config.h"
#include "processes.h"

using namespace std;



EmulatedAxis parse_em_axis(string value)
{
	if (value == "LeftStickX") return EmAx_LeftStickX;
	if (value == "LeftStickY") return EmAx_LeftStickY;
	if (value == "RightStickX") return EmAx_RightStickX;
	if (value == "RightStickY") return EmAx_RightStickY;
	if (value == "LT") return EmAx_LT;
	if (value == "RT") return EmAx_RT;
	return EmAx_None;
}

HardwareButton parse_hw_button(string value)
{
	if (value == "BtnX") return HwBtn_X;
	if (value == "BtnY") return HwBtn_Y;
	if (value == "BtnA") return HwBtn_A;
	if (value == "BtnB") return HwBtn_B;
	if (value == "BtnMenu") return HwBtn_Menu;
	if (value == "BtnOptions") return HwBtn_Options;
	if (value == "BtnSteam") return HwBtn_Steam;
	if (value == "BtnQuickAccess") return HwBtn_QuickAccess;
	if (value == "BtnDpadUp") return HwBtn_DpadUp;
	if (value == "BtnDpadLeft") return HwBtn_DpadLeft;
	if (value == "BtnDpadRight") return HwBtn_DpadRight;
	if (value == "BtnDpadDown") return HwBtn_DpadDown;
	if (value == "BtnL1") return HwBtn_L1;
	if (value == "BtnR1") return HwBtn_R1;
	if (value == "BtnL2") return HwBtn_L2;
	if (value == "BtnR2") return HwBtn_R2;
	if (value == "BtnL4") return HwBtn_L4;
	if (value == "BtnR4") return HwBtn_R4;
	if (value == "BtnL5") return HwBtn_L5;
	if (value == "BtnR5") return HwBtn_R5;
	if (value == "BtnRPadPress") return HwBtn_RPadPress;
	if (value == "BtnLPadPress") return HwBtn_LPadPress;
	if (value == "BtnRPadTouch") return HwBtn_RPadTouch;
	if (value == "BtnLPadTouch") return HwBtn_LPadTouch;
	if (value == "BtnRStickPress") return HwBtn_RStickPress;
	if (value == "BtnLStickPress") return HwBtn_LStickPress;
	if (value == "BtnRStickTouch") return HwBtn_RStickTouch;
	if (value == "BtnLStickTouch") return HwBtn_LStickTouch;
	return HwBtn_None;
}

EmulatedButton parse_em_button(string value)
{
	if (value == "BtnX") return EmBtn_X;
	if (value == "BtnY") return EmBtn_Y;
	if (value == "BtnA") return EmBtn_A;
	if (value == "BtnB") return EmBtn_B;
	if (value == "BtnRB") return EmBtn_RB;
	if (value == "BtnLB") return EmBtn_LB;
	if (value == "BtnLS") return EmBtn_LS;
	if (value == "BtnRS") return EmBtn_RS;
	if (value == "BtnBack") return EmBtn_Back;
	if (value == "BtnStart") return EmBtn_Start;
	if (value == "BtnGuide") return EmBtn_Guide;
	if (value == "BtnDpadUp") return EmBtn_DpadUp;
	if (value == "BtnDpadDown") return EmBtn_DpadDown;
	if (value == "BtnDpadLeft") return EmBtn_DpadLeft;
	if (value == "BtnDpadRight") return EmBtn_DpadRight;
	return EmBtn_None;
}

ControllerConfig* get_active_config(AppConfig* config)
{
	vector<string> processes = vector<string>();
	sys_get_process_list(&processes);
	for (auto it = begin(config->pre_exe_controller_configs); it != end(config->pre_exe_controller_configs); ++it) {
		if (process_list_contains(&processes, &((*it).executable)))
		{
			return &(*it);
		}
	}
	return &config->default_controller_config;
}

void process_profile_config(string key, string value, ControllerConfig* config)
{
	if (key == "DisableLizardMode")
	{
		config->disable_lizard_mode = value == "true";
		return;
	}
}

void process_actions_config(string key, string value, AppConfig* config)
{
	if (key == "OpenWindowsGameBar")
	{
		config->button_actions.OpenWindowsGameBar = parse_hw_button(value);
		return;
	}
}

void process_general_config(string key, string value, AppConfig* config)
{
	if (key == "blacklist")
	{
		config->blacklisted_processes.push_back(value);
		return;
	}

	if (key == "whitelist")
	{
		config->whitelisted_processes.push_back(value);
		return;
	}

	if (key == "mode")
	{
		if (value == "whitelist")
		{
			config->mode = APP_MODE_WHITELIST;
		}
		else
		{
			config->mode = APP_MODE_BLACKLIST;
		}
		return;
	}

	LOG_WARN("Unknown config key: %s=%s", key.c_str(), value.c_str());
}

void split_string(vector<string>* strings, string value, char delim)
{
	string line = "";
	stringstream strstream(value);
	while (getline(strstream, line, delim))
	{
		if (line.length() > 1)
			strings->push_back(line);
	}
}

ConfigKeyValue get_key_value(string value)
{
	ConfigKeyValue val = ConfigKeyValue();
	int index = value.find_first_of('=');
	if (index > 0)
	{
		val.key = value.substr(0, index);
		val.value = value.substr(index + 1);
		return val;
	}
	else
	{
		LOG_ERROR("Could not parse config key value! %s", value.c_str());
		return val;
	}
}

void parse_axis_config(EmulatedAxisConfig* conf, string value)
{
	string axis = value;
	string activation_button = "None";
	bool inverted = false;
	if ((int)value.find_first_of(',') > 0)
	{
		axis = value.substr(0, value.find_first_of(','));
		string left = value.substr(axis.length() + 1);
		vector<string> properties = vector<string>();
		split_string(&properties, left, ',');

		for (auto it = begin(properties);
			it != end(properties); ++it) {
			string value = *it;
			ConfigKeyValue keyVal = get_key_value(value);

			if (keyVal.key == "activate")
			{
				activation_button = keyVal.value;
				continue;
			}

			if (keyVal.key == "inverted")
			{
				inverted = keyVal.value == "true";
				continue;
			}

			LOG_WARN("Unknown config key %s in axis config. %s", keyVal.key.c_str(), value.c_str());
		}
	}

	conf->inverted = inverted;
	conf->activation_button = parse_hw_button(activation_button);
	conf->emulated_axis = parse_em_axis(axis);
}

//Key is hw axis, value is format [EmAxis],[activation_button=(hwButton)],[inverted=true/false]
void process_axes_config(string key, string value, ControllerConfig* config)
{
	EmulatedAxisConfig axisConf = EmulatedAxisConfig();
	parse_axis_config(&axisConf, value);
	if (key == "LeftStickX") config->axis_mapping.LeftStickX = axisConf;
	if (key == "LeftStickY") config->axis_mapping.LeftStickY = axisConf;
	if (key == "RightStickX") config->axis_mapping.RightStickX = axisConf;
	if (key == "RightStickY") config->axis_mapping.RightStickY = axisConf;
	if (key == "LeftPadX") config->axis_mapping.LeftPadX = axisConf;
	if (key == "LeftPadY") config->axis_mapping.LeftPadY = axisConf;
	if (key == "RightPadX") config->axis_mapping.RightPadX = axisConf;
	if (key == "RightPadY") config->axis_mapping.RightPadY = axisConf;
	if (key == "RightPadPressure") config->axis_mapping.RightPadPressure = axisConf;
	if (key == "LeftPadPressure") config->axis_mapping.LeftPadPressure = axisConf;
	if (key == "L2") config->axis_mapping.L2 = axisConf;
	if (key == "R2") config->axis_mapping.R2 = axisConf;
	if (key == "GyroAccelX") config->axis_mapping.GyroAccelX = axisConf;
	if (key == "GyroAccelY") config->axis_mapping.GyroAccelY = axisConf;
	if (key == "GyroAccelZ") config->axis_mapping.GyroAccelZ = axisConf;
	if (key == "GyroRoll") config->axis_mapping.GyroRoll = axisConf;
	if (key == "GyroPitch") config->axis_mapping.GyroPitch = axisConf;
	if (key == "GyroYaw") config->axis_mapping.GyroYaw = axisConf;
}

void process_buttons_config(string key, string value, ControllerConfig* config)
{
	EmulatedButton emBtn = parse_em_button(value);
	if (key == "BtnX") config->button_mapping.BtnX = emBtn;
	if (key == "BtnY") config->button_mapping.BtnY = emBtn;
	if (key == "BtnA") config->button_mapping.BtnA = emBtn;
	if (key == "BtnB") config->button_mapping.BtnB = emBtn;
	if (key == "BtnMenu") config->button_mapping.BtnMenu = emBtn;
	if (key == "BtnOptions") config->button_mapping.BtnOptions = emBtn;
	if (key == "BtnSteam") config->button_mapping.BtnSteam = emBtn;
	if (key == "BtnQuickAccess") config->button_mapping.BtnQuickAccess = emBtn;
	if (key == "BtnDpadUp") config->button_mapping.BtnDpadUp = emBtn;
	if (key == "BtnDpadLeft") config->button_mapping.BtnDpadLeft = emBtn;
	if (key == "BtnDpadRight") config->button_mapping.BtnDpadRight = emBtn;
	if (key == "BtnDpadDown") config->button_mapping.BtnDpadDown = emBtn;
	if (key == "BtnL1") config->button_mapping.BtnL1 = emBtn;
	if (key == "BtnR1") config->button_mapping.BtnR1 = emBtn;
	if (key == "BtnL2") config->button_mapping.BtnL2 = emBtn;
	if (key == "BtnR2") config->button_mapping.BtnR2 = emBtn;
	if (key == "BtnL4") config->button_mapping.BtnL4 = emBtn;
	if (key == "BtnR4") config->button_mapping.BtnR4 = emBtn;
	if (key == "BtnL5") config->button_mapping.BtnL5 = emBtn;
	if (key == "BtnR5") config->button_mapping.BtnR5 = emBtn;
	if (key == "BtnRPadPress") config->button_mapping.BtnRPadPress = emBtn;
	if (key == "BtnLPadPress") config->button_mapping.BtnLPadPress = emBtn;
	if (key == "BtnRPadTouch") config->button_mapping.BtnRPadTouch = emBtn;
	if (key == "BtnLPadTouch") config->button_mapping.BtnLPadTouch = emBtn;
	if (key == "BtnRStickPress") config->button_mapping.BtnRStickPress = emBtn;
	if (key == "BtnLStickPress") config->button_mapping.BtnLStickPress = emBtn;
	if (key == "BtnRStickTouch") config->button_mapping.BtnRStickTouch = emBtn;
	if (key == "BtnLStickTouch") config->button_mapping.BtnLStickTouch = emBtn;
}

ControllerConfig* get_controller_config(AppConfig* config, string executable)
{
	for (auto it = begin(config->pre_exe_controller_configs); it != end(config->pre_exe_controller_configs); ++it) {
		if ((*it).executable == executable)
			return &(*it);
	}
	return nullptr;
}

bool load_conf(AppConfig* config)
{
	LOG_INFO("Loading config");

	ifstream file(CONF_FILE);
	if (file.is_open()) {

		config->blacklisted_processes.clear();
		config->whitelisted_processes.clear();

		string line;
		string section = "general";
		string executable = "";
		int line_number = 1;
		while (getline(file, line))
		{
			//Ignore comments in conf file
			if (line.c_str()[0] == '#')
				continue;

			//Ignore empty lines
			if (line.length() == 0)
				continue;

			LOG_DEBUG("%s", line.c_str());

			// Process sections
			if (line.c_str()[0] == '[')
			{
				int length = line.length() - 2;
				int index = line.find_first_of(',');
				if ((int)line.find_first_of(',') > 0)
				{
					length = line.find_first_of(',') - 1;
					executable = line.substr(line.find_first_of(',') + 1, line.length() - line.find_first_of(',') - 2);
				}
				else
				{
					executable = "";
				}
				section = line.substr(1, length);
				continue;
			}

			if ((int)line.find_first_of('=') > 0)
			{
				string key = line.substr(0, line.find_first_of('='));
				string value = line.substr(line.find_first_of('=') + 1);

				if (section == "general")
				{
					process_general_config(key, value, config);
					continue;
				}

				ControllerConfig* controller_config = &(config->default_controller_config);
				if (executable.length() > 0)
					controller_config = get_controller_config(config, executable);

				if (controller_config == nullptr)
				{
					ControllerConfig cc = ControllerConfig();
					cc.executable = executable;
					config->pre_exe_controller_configs.push_back(cc);
					controller_config = get_controller_config(config, executable);
				}

				if (section == "axes")
				{
					process_axes_config(key, value, controller_config);
					continue;
				}

				if (section == "buttons")
				{
					process_buttons_config(key, value, controller_config);
					continue;
				}

				if (section == "actions")
				{
					process_actions_config(key, value, config);
				}

				if (section == "profile")
				{
					process_profile_config(key, value, controller_config);
				}
			}
			else
			{
				LOG_WARN("Malformed Config. Expected \"=\" in line %d.", line_number);
			}
			line_number++;
		}
		return true;
	}
	else
	{
		LOG_ERROR("Unable to open config file: %s", CONF_FILE);
		return false;
	}
}

bool process_list_contains(vector<string>* processes, string* name)
{
	for (auto it = begin(*processes); it != end(*processes); ++it) {
		if (*it == *name)
			return true;
	}
	return false;
}

bool check_should_emulate(AppConfig* config)
{
	vector<string> processes = vector<string>();
	sys_get_process_list(&processes);

	if (config->mode == APP_MODE_WHITELIST)
	{
		for (auto it = begin(config->whitelisted_processes); it != end(config->whitelisted_processes); ++it) {
			if (process_list_contains(&processes, &(*it)))
				return true;
		}
		return false;
	}
	else
	{
		for (auto it = begin(config->blacklisted_processes); it != end(config->blacklisted_processes); ++it) {
			if (process_list_contains(&processes, &(*it)))
				return false;
		}
		return true;
	}
}