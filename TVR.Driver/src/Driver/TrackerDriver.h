//
// Created by Twometer on 08/12/2020.
//

#ifndef TVR_DRIVER_TRACKERDRIVER_H
#define TVR_DRIVER_TRACKERDRIVER_H

#include <openvr_driver.h>
#include "../Model/TrackerState.h"

class TrackerDriver : public vr::ITrackedDeviceServerDriver {
private:
    std::string modelNumber;

    TrackerState *state;

public:
    TrackerDriver(std::string modelNumber, TrackerState *state);

    vr::EVRInitError Activate(uint32_t unObjectId) override;

    vr::DriverPose_t GetPose() override;

};


#endif //TVR_DRIVER_TRACKERDRIVER_H
