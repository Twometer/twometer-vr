//
// Created by twome on 06/04/2020.
//

#ifndef TVRDRV_CONTROLLERSTATE_H
#define TVRDRV_CONTROLLERSTATE_H

#include "Button.h"
#include <cstdint>
#include <map>

struct ControllerState {
public:
    static ControllerState invalid;

    uint8_t controllerId;
    float posX;
    float posY;
    float posZ;
    float roll;
    float pitch;
    float yaw;

    std::map<Button, bool> buttons;

    ControllerState(uint8_t controllerId, float posX, float posY, float posZ, float yaw, float pitch, float roll) : controllerId(
            controllerId), posX(posX), posY(posY), posZ(posZ), yaw(yaw), pitch(pitch), roll(roll) {
        buttons[Button::A] = false;
        buttons[Button::B] = false;
    }

    bool IsValid() {
        return controllerId != 255;
    }
};

#endif //TVRDRV_CONTROLLERSTATE_H
