//
// Created by Twometer on 8 Dec 2020.
//

#ifndef TVR_DRIVER_TRACKERDRIVER_H
#define TVR_DRIVER_TRACKERDRIVER_H

#include <openvr_driver.h>
#include "../Model/TrackerInfo.h"

class TrackerDriver : public vr::ITrackedDeviceServerDriver {
private:
    TrackerInfo *tracker;

public:
    explicit TrackerDriver(TrackerInfo *tracker);

    vr::EVRInitError Activate(uint32_t unObjectId) override;

    vr::DriverPose_t GetPose() override;

    void Deactivate() override {};

    void EnterStandby() override {};

    void *GetComponent(const char *pchComponentNameAndVersion) override { return nullptr; };

    void DebugRequest(const char *pchRequest, char *pchResponseBuffer, uint32_t unResponseBufferSize) override {};

private:
    int32_t GetTrackerRole();
};

#endif //TVR_DRIVER_TRACKERDRIVER_H
