//
// Created by Twometer on 9 Nov 2020.
//

#ifndef TVR_DRIVER_TRACKERSTATE_H
#define TVR_DRIVER_TRACKERSTATE_H

#include <cstdint>
#include "Vectors.h"

struct TrackerState {
    uint8_t trackerId;
    uint16_t buttons;
    vec3 position;
    vec4 rotation;
};

#endif //TVR_DRIVER_TRACKERSTATE_H
