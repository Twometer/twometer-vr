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
    float qx;
    float qy;
    float qz;
    float qw;

    std::map<Button, bool> buttons;

    ControllerState(uint8_t controllerId, float posX, float posY, float posZ, float qx, float qy, float qz, float qw) : controllerId(
            controllerId), posX(posX), posY(posY), posZ(posZ), qx(qx), qy(qy), qz(qz), qw(qw) {
        buttons[Button::A] = false;
        buttons[Button::B] = false;
    }

    bool IsValid() {
        return controllerId != 255;
    }
};

#endif //TVRDRV_CONTROLLERSTATE_H
