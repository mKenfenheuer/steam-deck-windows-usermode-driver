#pragma once
#include <stdint.h>
#include "steam_deck_hid_commands.h"

void handle_button_actions(SDCInput* input);
void map_driver_hid_input(SDCInput* input);
