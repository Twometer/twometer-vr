//
// Created by Twometer on 08/12/2020.
//

#ifndef TVR_DRIVER_TRACKERDRIVER_H
#define TVR_DRIVER_TRACKERDRIVER_H

#include <openvr_driver.h>
#include "../Model/TrackerInfo.h"

class TrackerDriver : public vr::ITrackedDeviceServerDriver {
private:
    TrackerInfo *tracker;

public:
    TrackerDriver(TrackerInfo *tracker);

    vr::EVRInitError Activate(uint32_t unObjectId) override;

    vr::DriverPose_t GetPose() override;

};


#endif //TVR_DRIVER_TRACKERDRIVER_H
