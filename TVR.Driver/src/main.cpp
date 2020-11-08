#include <iostream>
#include "Driver/TrackerDriver.h"
#include "Driver/WatchdogDriver.h"
#include "Utils/OpenVrExports.h"

TrackerDriver trackerDriver;
WatchdogDriver watchdogDriver;

HMD_DLL_EXPORT void *HmdDriverFactory(const char *pInterfaceName, int *pReturnCode) {
    if (strcmp(vr::IServerTrackedDeviceProvider_Version, pInterfaceName) == 0) {
        return &trackerDriver;
    }
    if (strcmp(vr::IVRWatchdogProvider_Version, pInterfaceName) == 0) {
        return &watchdogDriver;
    }

    if (pReturnCode)
        *pReturnCode = vr::VRInitError_Init_InterfaceNotFound;
    return nullptr;
}