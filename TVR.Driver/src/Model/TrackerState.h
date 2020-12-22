//
// Created by Twometer on 9 Nov 2020.
//

#ifndef TVR_DRIVER_TRACKERSTATE_H
#define TVR_DRIVER_TRACKERSTATE_H

#include <cstdint>
#include "Vectors.h"

struct TrackerState {
    uint8_t trackerId{0};
    uint16_t buttons{0};
    vec3 position{0, 0, 0};
    vec4 rotation{0, 0, 0, 1};
    bool inRange{false};
};

#endif //TVR_DRIVER_TRACKERSTATE_H
