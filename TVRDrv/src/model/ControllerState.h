//
// Created by twome on 06/04/2020.
//

#ifndef TVRDRV_CONTROLLERSTATE_H
#define TVRDRV_CONTROLLERSTATE_H

struct ControllerState {
public:
    int controllerId;
    float posX;
    float posY;
    float posZ;
    float rotX;
    float rotY;

    ControllerState(int controllerId, float posX, float posY, float posZ, float rotX, float rotY) : controllerId(
            controllerId), posX(posX), posY(posY), posZ(posZ), rotX(rotX), rotY(rotY) {}

};

#endif //TVRDRV_CONTROLLERSTATE_H
