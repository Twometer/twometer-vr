//
// Created by Twometer on 8 Nov 2020.
//

#ifndef TVR_DRIVER_SERVERDRIVER_H
#define TVR_DRIVER_SERVERDRIVER_H

#include <unordered_set>
#include <openvr_driver.h>
#include "../Net/StreamClient.h"

class ServerDriver : public vr::IServerTrackedDeviceProvider {
private:
    StreamClient *streamClient{};

    std::unordered_set<std::string> knownTrackers;

public:
    vr::EVRInitError Init(vr::IVRDriverContext *pDriverContext) override;

    void Cleanup() override;

    const char *const *GetInterfaceVersions() override { return vr::k_InterfaceVersions; }

    void RunFrame() override {};

    bool ShouldBlockStandbyMode() override { return false; }

    void EnterStandby() override {}

    void LeaveStandby() override {}

private:
    static vr::ETrackedDeviceClass GetDeviceClass(TrackerInfo *tracker);
};


#endif //TVR_DRIVER_SERVERDRIVER_H
