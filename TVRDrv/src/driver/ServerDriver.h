//
// Created by twome on 08/04/2020.
//

#ifndef TVRDRV_SERVERDRIVER_H
#define TVRDRV_SERVERDRIVER_H

#include <openvr_driver.h>
#include "ControllerDriver.h"

#if defined(_WIN32)
#define HMD_DLL_EXPORT extern "C" __declspec( dllexport )
#define HMD_DLL_IMPORT extern "C" __declspec( dllimport )
#elif defined(__GNUC__) || defined(COMPILER_GCC) || defined(__APPLE__)
#define HMD_DLL_EXPORT extern "C" __attribute__((visibility("default")))
#define HMD_DLL_IMPORT extern "C"
#else
#error "Unsupported Platform."
#endif

class ServerDriver : public vr::IServerTrackedDeviceProvider {
private:
    ControllerDriver *controllerDriver = nullptr;

public:
    vr::EVRInitError Init(vr::IVRDriverContext *pDriverContext) override;

    void Cleanup() override;

    const char *const *GetInterfaceVersions() override { return vr::k_InterfaceVersions; }

    void RunFrame() override;

    bool ShouldBlockStandbyMode() override { return false; }

    void EnterStandby() override {}

    void LeaveStandby() override {}
};


#endif //TVRDRV_SERVERDRIVER_H
