//
// Created by twome on 08/04/2020.
//

#include "WatchdogDriver.h"

using namespace vr;

bool WatchdogDriver::exiting = false;

vr::EVRInitError WatchdogDriver::Init(vr::IVRDriverContext *pDriverContext) {
    VR_INIT_WATCHDOG_DRIVER_CONTEXT(pDriverContext);

    watchdogThread = new std::thread(&WatchdogThreadFunction);
    if (!watchdogThread)
        return VRInitError_Driver_Failed;

    return VRInitError_None;
}

void WatchdogDriver::Cleanup() {
    exiting = true;
    if (watchdogThread) {
        watchdogThread->join();
        delete watchdogThread;
        watchdogThread = nullptr;
    }
}

void WatchdogDriver::WatchdogThreadFunction() {
    while (!exiting) {
        std::this_thread::sleep_for(std::chrono::seconds(5));
        vr::VRWatchdogHost()->WatchdogWakeUp(vr::TrackedDeviceClass_HMD);
    }
}
