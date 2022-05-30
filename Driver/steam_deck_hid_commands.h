#pragma once

#define COMMAND_REQUEST_SERIAL_NUMBER 0xAE, 0x15, 0x01
#define COMMAND_SET_LIZARD_MODE 0x81

//Enums from https://github.com/kozec/sc-controller
typedef enum {
	PT_INPUT = 0x01,
	PT_HOTPLUG = 0x03,
	PT_IDLE = 0x04,
	PT_OFF = 0x9f,
	PT_AUDIO = 0xb6,
	PT_CLEAR_MAPPINGS = 0x81,
	PT_CONFIGURE = 0x87,
	PT_LED = 0x87,
	PT_CALIBRATE_JOYSTICK = 0xbf,
	PT_CALIBRATE_TRACKPAD = 0xa7,
	PT_SET_AUDIO_INDICES = 0xc1,
	PT_LIZARD_BUTTONS = 0x85,
	PT_LIZARD_MOUSE = 0x8e,
	PT_FEEDBACK = 0x8f,
	PT_RESET = 0x95,
	PT_GET_SERIAL = 0xAE,
} SDCPacketType;

typedef enum {
	PL_LED = 0x03,
	PL_OFF = 0x04,
	PL_FEEDBACK = 0x07,
	PL_CONFIGURE = 0x15,
	PL_CONFIGURE_BT = 0x0f,
	PL_GET_SERIAL = 0x15,
} SDCPacketLength;

typedef enum {
	CT_LED = 0x2d,
	CT_CONFIGURE = 0x32,
	CONFIGURE_BT = 0x18,
} SDCConfigType;

typedef struct SDInput {
	uint8_t			ptype;  		//0x00
	uint8_t			_a1[3]; 		//0x01 
	uint32_t		seq;			//0x03 
	uint16_t		buttons0;		//0x09 
	uint8_t			buttons1;		//0x0A
	uint8_t			buttons2;		//0x0C
	uint8_t			buttons3;		//0x0D
	uint8_t			buttons4;		//0x0E
	uint8_t			buttons5;		//0x0E
	int16_t			lpad_x;			//0x10
	int16_t			lpad_y;			//0x12
	int16_t			rpad_x;			//0x13
	int16_t			rpad_y;			//0x16
	int16_t			accel_x;		//0x18
	int16_t			accel_y;		//0x1A
	int16_t			accel_z;		//0x1C
	int16_t			gpitch;			//0x1E
	int16_t			gyaw;			//0x20
	int16_t			groll;			//0x22
	int16_t			q1;				//0x24
	int16_t			q2;				//0x26
	int16_t			q3;				//0x28
	int16_t			q4;				//0x2A
	int16_t			ltrig;			//0x2C
	int16_t			rtrig;			//0x2E
	int16_t			lthumb_x;		//0x30
	int16_t			lthumb_y;		//0x32
	int16_t			rthumb_x;		//0x34
	int16_t			rthumb_y;		//0x36
	int16_t			lpad_pressure;	//0x38
	int16_t			rpad_pressure;	//0x3A
	// uint8_t		_a4[16];
} SDCInput;

typedef enum SDCButton0 {
	BTN_L5				= 0b1000000000000000,
	BTN_OPTIONS			= 0b0100000000000000,
	BTN_STEAM			= 0b0010000000000000,
	BTN_MENU			= 0b0001000000000000,
	BTN_DPAD_DOWN		= 0b0000100000000000,
	BTN_DPAD_LEFT		= 0b0000010000000000,
	BTN_DPAD_RIGHT		= 0b0000001000000000,
	BTN_DPAD_UP			= 0b0000000100000000,
	BTN_A				= 0b0000000010000000,
	BTN_X				= 0b0000000001000000,
	BTN_B				= 0b0000000000100000,
	BTN_Y				= 0b0000000000010000,
	BTN_L1				= 0b0000000000001000,
	BTN_R1				= 0b0000000000000100,
	BTN_L2				= 0b0000000000000010,
	BTN_R2              = 0b0000000000000001,
} SDCButton0;


typedef enum SDCButton1 {
	BTN_LSTICK_PRESS	= 0b01000000,
	BTN_LPAD_TOUCH		= 0b00001000,
	BTN_LPAD_PRESS		= 0b00000010,
	BTN_RPAD_TOUCH		= 0b00010000,
	BTN_RPAD_PRESS		= 0b00000100,
	BTN_R5				= 0b00000001,
} SDCButton1;

typedef enum SDCButton2 {
	BTN_RSTICK_PRESS	= 0b00000100,
} SDCButton2;

typedef enum SDCButton4 {
	BTN_LSTICK_TOUCH	= 0b01000000,
	BTN_RSTICK_TOUCH	= 0b10000000,
	BTN_R4				= 0b00000100,
	BTN_L4				= 0b00000010,
} SDCButton4;

typedef enum SDCButton5 {
	BTN_QUICK_ACCESS	= 0b00000100,
} SDCButton5;

