#pragma once
#define CONF_FILE "app_config.conf"

typedef enum AppMode {
	APP_MODE_BLACKLIST = 0,
	APP_MODE_WHITELIST = 1
} AppMode;

typedef struct AppConfig {
	AppMode mode;

	int vendor_id;	//not used right now
	int product_id;	//not used right now
};