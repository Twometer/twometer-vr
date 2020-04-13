#include <iostream>
#include "net/StreamClient.h"
#include "driver/ServerDriver.h"
#include "driver/WatchdogDriver.h"

ServerDriver serverDriver;
WatchdogDriver watchdogDriver;

HMD_DLL_EXPORT void *HmdDriverFactory(const char *pInterfaceName, int *pReturnCode) {
    if (strcmp(vr::IServerTrackedDeviceProvider_Version, pInterfaceName) == 0) {
        return &serverDriver;
    }
    if (strcmp(vr::IVRWatchdogProvider_Version, pInterfaceName) == 0) {
        return &watchdogDriver;
    }

    if (pReturnCode)
        *pReturnCode = vr::VRInitError_Init_InterfaceNotFound;
    return nullptr;
}