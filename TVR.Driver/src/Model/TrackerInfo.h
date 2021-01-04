//
// Created by Twometer on 9 Dec 2020.
//

#ifndef TVR_DRIVER_TRACKERINFO_H
#define TVR_DRIVER_TRACKERINFO_H

#include <string>
#include "TrackerClass.h"
#include "TrackerColor.h"
#include "TrackerState.h"

struct TrackerInfo {
    std::string serialNo{};
    TrackerClass trackerClass{};
    TrackerColor trackerColor{};
    TrackerState trackerState{};
    bool connected{};
    void *context{};
};

#endif //TVR_DRIVER_TRACKERINFO_H
