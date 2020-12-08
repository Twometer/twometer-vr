#include "Driver/ServerDriver.h"
#include "Driver/WatchdogDriver.h"
#include "Utils/OpenVrExports.h"

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