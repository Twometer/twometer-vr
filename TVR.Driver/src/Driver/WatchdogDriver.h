//
// Created by Twometer on 8 Nov 2020.
//

#ifndef TVR_DRIVER_WATCHDOGDRIVER_H
#define TVR_DRIVER_WATCHDOGDRIVER_H

#include <thread>
#include <openvr_driver.h>

class WatchdogDriver : public vr::IVRWatchdogProvider {
private:
    std::thread *watchdogThread = nullptr;

    static bool exiting;

public:
    vr::EVRInitError Init(vr::IVRDriverContext *pDriverContext) override;

    void Cleanup() override;

    static void WatchdogThreadFunction();
};


#endif //TVR_DRIVER_WATCHDOGDRIVER_H
