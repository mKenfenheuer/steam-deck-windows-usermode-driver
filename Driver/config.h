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
#include <string>
#include <filesystem>
#include <direct.h>

// internal 
#define LOG_TAG "driver_main"
#include "logging.h"
#include "processes.h"

using namespace std;

#define CONF_FILE "app_config.conf"

typedef enum AppMode {
	APP_MODE_BLACKLIST = 0,
	APP_MODE_WHITELIST = 1
} AppMode;

typedef enum HardwareButton
{
	HwBtn_None = 0,
	HwBtn_X = 1,
	HwBtn_Y = 2,
	HwBtn_A = 3,
	HwBtn_B = 4,
	HwBtn_Menu = 5,
	HwBtn_Options = 6,
	HwBtn_Steam = 7,
	HwBtn_QuickAccess = 8,
	HwBtn_DpadUp = 9,
	HwBtn_DpadLeft = 10,
	HwBtn_DpadRight = 11,
	HwBtn_DpadDown = 12,
	HwBtn_L1 = 13,
	HwBtn_R1 = 14,
	HwBtn_L2 = 15,
	HwBtn_R2 = 16,
	HwBtn_L4 = 17,
	HwBtn_R4 = 18,
	HwBtn_L5 = 19,
	HwBtn_R5 = 20,
	HwBtn_RPadPress = 21,
	HwBtn_LPadPress = 22,
	HwBtn_RPadTouch = 23,
	HwBtn_LPadTouch = 24,
	HwBtn_RStickPress = 25,
	HwBtn_LStickPress = 26,
	HwBtn_RStickTouch = 27,
	HwBtn_LStickTouch = 28,
} HardwareButton;

typedef enum EmulatedButton
{
	EmBtn_None = 0,
	EmBtn_X = 1,
	EmBtn_Y = 2,
	EmBtn_A = 3,
	EmBtn_B = 4,
	EmBtn_RB = 5,
	EmBtn_LB = 6,
	EmBtn_LS = 7,
	EmBtn_RS = 8,
	EmBtn_Back = 9,
	EmBtn_Start = 10,
	EmBtn_Guide = 11,
	EmBtn_DpadUp = 12,
	EmBtn_DpadDown = 13,
	EmBtn_DpadLeft = 14,
	EmBtn_DpadRight = 15,
} EmulatedButton;

typedef struct ButtonMapping {
	EmulatedButton BtnX;
	EmulatedButton BtnY;
	EmulatedButton BtnA;
	EmulatedButton BtnB;
	EmulatedButton BtnMenu;
	EmulatedButton BtnOptions;
	EmulatedButton BtnSteam;
	EmulatedButton BtnQuickAccess;
	EmulatedButton BtnDpadUp;
	EmulatedButton BtnDpadLeft;
	EmulatedButton BtnDpadRight;
	EmulatedButton BtnDpadDown;
	EmulatedButton BtnL1;
	EmulatedButton BtnR1;
	EmulatedButton BtnL2;
	EmulatedButton BtnR2;
	EmulatedButton BtnL4;
	EmulatedButton BtnR4;
	EmulatedButton BtnL5;
	EmulatedButton BtnR5;
	EmulatedButton BtnRPadPress;
	EmulatedButton BtnLPadPress;
	EmulatedButton BtnRPadTouch;
	EmulatedButton BtnLPadTouch;
	EmulatedButton BtnRStickPress;
	EmulatedButton BtnLStickPress;
	EmulatedButton BtnRStickTouch;
	EmulatedButton BtnLStickTouch;
} ButtonMapping;

typedef struct ButtonActions {
	HardwareButton OpenWindowsGameBar = HwBtn_Steam;
} ButtonActions;

typedef enum EmulatedAxis
{
	EmAx_None = 0,
	EmAx_LeftStickX = 1,
	EmAx_LeftStickY = 2,
	EmAx_RightStickX = 3,
	EmAx_RightStickY = 4,
	EmAx_LT = 5,
	EmAx_RT = 6,
} EmulatedAxis;

typedef struct EmulatedAxisConfig
{
	EmulatedAxis emulated_axis = EmAx_None;
	HardwareButton activation_button = HwBtn_None;
	bool inverted;
};

typedef struct AxisMapping {
	EmulatedAxisConfig LeftStickX;
	EmulatedAxisConfig LeftStickY;
	EmulatedAxisConfig RightStickX;
	EmulatedAxisConfig RightStickY;
	EmulatedAxisConfig LeftPadX;
	EmulatedAxisConfig LeftPadY;
	EmulatedAxisConfig RightPadX;
	EmulatedAxisConfig RightPadY;
	EmulatedAxisConfig RightPadPressure;
	EmulatedAxisConfig LeftPadPressure;
	EmulatedAxisConfig L2;
	EmulatedAxisConfig R2;
	EmulatedAxisConfig GyroAccelX;
	EmulatedAxisConfig GyroAccelY;
	EmulatedAxisConfig GyroAccelZ;
	EmulatedAxisConfig GyroRoll;
	EmulatedAxisConfig GyroPitch;
	EmulatedAxisConfig GyroYaw;
};

typedef struct ConfigKeyValue {
	string key;
	string value;
};

typedef struct ControllerConfig {
	bool disable_lizard_mode = false;
	string executable;
	ButtonMapping button_mapping;
	AxisMapping axis_mapping;
};

typedef struct AppConfig {
	AppMode mode;
	ButtonActions button_actions;
	ControllerConfig default_controller_config;
	std::vector<ControllerConfig> pre_exe_controller_configs = std::vector<ControllerConfig>();
	std::vector<std::string> blacklisted_processes = std::vector<std::string>();
	std::vector<std::string> whitelisted_processes = std::vector<std::string>();
};

ControllerConfig* get_active_config(AppConfig *config);
void process_general_config(string key, string value, AppConfig* config);
void process_buttons_config(string key, string value, ControllerConfig* config);
void process_axes_config(string key, string value, ControllerConfig* config);
bool load_conf(AppConfig* config);
bool process_list_contains(vector<string>* processes, string* name);
bool check_should_emulate(AppConfig* config);