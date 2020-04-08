//
// Created by twome on 06/04/2020.
//

#ifndef TVRDRV_CONTROLLERSTATE_H
#define TVRDRV_CONTROLLERSTATE_H

struct ControllerState {
public:
    static ControllerState invalid;

    uint8_t controllerId;
    float posX;
    float posY;
    float posZ;
    float rotX;
    float rotY;

    ControllerState(uint8_t controllerId, float posX, float posY, float posZ, float rotX, float rotY) : controllerId(
            controllerId), posX(posX), posY(posY), posZ(posZ), rotX(rotX), rotY(rotY) {}

    bool IsValid() {
        return controllerId != 255;
    }
};

ControllerState ControllerState::invalid(255, 0, 0, 0, 0, 0);

#endif //TVRDRV_CONTROLLERSTATE_H
