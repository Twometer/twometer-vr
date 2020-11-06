#ifndef TVR_BUTTON_INPUT
#define TVR_BUTTON_INPUT

#include <stdint.h>
#include "Hardware.h"

class ButtonInput
{
public:
    void setup();
    void update();
    uint16_t getStates();
};

#endif