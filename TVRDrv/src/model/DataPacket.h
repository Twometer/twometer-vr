//
// Created by twome on 06/04/2020.
//

#ifndef TVRDRV_DATAPACKET_H
#define TVRDRV_DATAPACKET_H

#include <vector>
#include "ControllerState.h"
#include "ButtonPress.h"

struct DataPacket {
public:
    std::vector<ControllerState> controllerStates;

    std::vector<ButtonPress> buttonPresses;

};

#endif //TVRDRV_DATAPACKET_H
