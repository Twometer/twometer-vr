//
// Created by Twometer on 8 Dec 2020.
//

#ifndef TVR_DRIVER_TRACKERDRIVER_H
#define TVR_DRIVER_TRACKERDRIVER_H

#include <openvr_driver.h>
#include "../Model/TrackerInfo.h"
#include "../Model/TrackerButton.h"

class TrackerDriver : public vr::ITrackedDeviceServerDriver {
private:
    uint32_t objectId{0};
    TrackerInfo *tracker;

    vr::VRInputComponentHandle_t buttonA{};
    vr::VRInputComponentHandle_t buttonB{};
    vr::VRInputComponentHandle_t buttonUp{};
    vr::VRInputComponentHandle_t buttonLeft{};
    vr::VRInputComponentHandle_t buttonRight{};
    vr::VRInputComponentHandle_t buttonDown{};
    vr::VRInputComponentHandle_t buttonMenu{};
    vr::VRInputComponentHandle_t buttonTrigger{};

public:
    explicit TrackerDriver(TrackerInfo *tracker);

    vr::EVRInitError Activate(uint32_t objectId) override;

    vr::DriverPose_t GetPose() override;

    void Deactivate() override {};

    void EnterStandby() override {};

    void *GetComponent(const char *pchComponentNameAndVersion) override { return nullptr; };

    void DebugRequest(const char *pchRequest, char *pchResponseBuffer, uint32_t unResponseBufferSize) override {};

    void Update();

    void SetTracker(TrackerInfo *tracker);

private:
    int32_t GetTrackerRole();

    bool IsButtonPressed(TrackerButton button);
};

#endif //TVR_DRIVER_TRACKERDRIVER_H
