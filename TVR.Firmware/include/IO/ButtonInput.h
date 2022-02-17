#ifndef TVR_BUTTON_INPUT
#define TVR_BUTTON_INPUT

#include <stdint.h>

#include "Hardware.h"

class ButtonInput {
   private:
    uint8_t shiftedIn;

   public:
    void begin() {
        pinMode(PIN_SHLD, OUTPUT);
        pinMode(PIN_CE, OUTPUT);
        pinMode(PIN_CLK, OUTPUT);
        pinMode(PIN_DATA, INPUT);

        digitalWrite(PIN_CLK, HIGH);
        digitalWrite(PIN_SHLD, HIGH);
    }

    void update() {
        digitalWrite(PIN_SHLD, LOW);
        delayMicroseconds(5);  // Requires a delay here according to the datasheet timing diagram
        digitalWrite(PIN_SHLD, HIGH);
        delayMicroseconds(5);

        digitalWrite(PIN_CLK, HIGH);
        digitalWrite(PIN_CE, LOW);  // Enable the clock

        shiftedIn = ~shiftIn(PIN_DATA, PIN_CLK, MSBFIRST);
        digitalWrite(PIN_CE, HIGH);
    }

    uint16_t getStates() {
        return (uint16_t)shiftedIn;
    }
};

#endif