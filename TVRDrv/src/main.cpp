#include <iostream>
#include "net/StreamClient.h"
#include "driver/SteamVRDriver.h"

int main() {
    std::cout << "Hello, Virtual World!" << std::endl;

    StreamClient streamClient([](const DataPacket &packet) {
        std::cout << "Received data with " << packet.controllerStates.size() << " controller state frames" << std::endl;
    });
    streamClient.Connect();

    return 0;
}

HMD_DLL_EXPORT void *HmdDriverFactory(const char *pInterfaceName, int *pReturnCode) {
    if (strcmp(vr::IServerTrackedDeviceProvider_Version, pInterfaceName) == 0) {
        // return serverDriver
    }
    if (strcmp(vr::IVRWatchdogProvider_Version, pInterfaceName) == 0) {
        // return watchdogDriver
    }

    if (pReturnCode)
        *pReturnCode = vr::VRInitError_Init_InterfaceNotFound;
    return nullptr;
}