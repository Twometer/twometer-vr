//
// Created by twome on 06/04/2020.
//

#ifndef TVRDRV_BUTTONPRESS_H
#define TVRDRV_BUTTONPRESS_H

struct ButtonPress {
public:
    uint8_t controllerId;
    uint8_t buttonId;

    ButtonPress(uint8_t controllerId, uint8_t buttonId) : controllerId(controllerId), buttonId(buttonId) {}

};

#endif //TVRDRV_BUTTONPRESS_H
