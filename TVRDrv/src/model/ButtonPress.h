//
// Created by twome on 06/04/2020.
//

#ifndef TVRDRV_BUTTONPRESS_H
#define TVRDRV_BUTTONPRESS_H

struct ButtonPress {
public:
    int controllerId;
    int buttonId;

    ButtonPress(int controllerId, int buttonId) : controllerId(controllerId), buttonId(buttonId) {}

};

#endif //TVRDRV_BUTTONPRESS_H
