//
// Created by twome on 08/04/2020.
//

#ifndef TVRDRV_WATCHDOGDRIVER_H
#define TVRDRV_WATCHDOGDRIVER_H

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


#endif //TVRDRV_WATCHDOGDRIVER_H
