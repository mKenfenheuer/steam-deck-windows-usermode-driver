#pragma once

#include <stdio.h>
#include <stdarg.h>

#define LOG_TAG_LEN 16
#define LOG_LEVEL 0

#define str(a) #a
#define xstr(a) str(a)

static const char* levels = "DIWE";
static char tag_padded[LOG_TAG_LEN + 1];

typedef enum {
	LLVL_DEBUG = 0,
	LLVL_INFO = 1,
	LLVL_WARN = 2,
	LLVL_ERROR = 3,
} LogLevel;

void _log(const char* tag, LogLevel lvl, const char* fmt, ...);

#ifdef LOG_TAG
#define LOG_DEBUG(fmt, ...)		_log(LOG_TAG, LLVL_DEBUG,	fmt, ##__VA_ARGS__ )
#define LOG_WARN(fmt, ...)		_log(LOG_TAG, LLVL_WARN,	fmt, ##__VA_ARGS__ )
#define LOG_INFO(fmt, ...)		_log(LOG_TAG, LLVL_INFO,	fmt, ##__VA_ARGS__ )
#define LOG_ERROR(fmt, ...)		_log(LOG_TAG, LLVL_ERROR,	fmt, ##__VA_ARGS__ )
#endif
