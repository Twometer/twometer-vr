//
// Created by twome on 06/04/2020.
//

#ifndef TVRDRV_CONTROLLERSTATE_H
#define TVRDRV_CONTROLLERSTATE_H

#include <cstdint>

struct ControllerState {
public:
    static ControllerState invalid;

    uint8_t controllerId;
    float posX;
    float posY;
    float posZ;
    float rotX;
    float rotY;
    float rotZ;

    ControllerState(uint8_t controllerId, float posX, float posY, float posZ, float rotX, float rotY, float rotZ) : controllerId(
            controllerId), posX(posX), posY(posY), posZ(posZ), rotX(rotX), rotY(rotY), rotZ(rotZ) {}

    bool IsValid() {
        return controllerId != 255;
    }
};

#endif //TVRDRV_CONTROLLERSTATE_H
