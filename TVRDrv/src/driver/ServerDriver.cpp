//
// Created by twome on 08/04/2020.
//

#include "ServerDriver.h"

using namespace vr;

void ServerDriver::Cleanup() {
    delete controllerDriver;
}

vr::EVRInitError ServerDriver::Init(vr::IVRDriverContext *pDriverContext) {
    VR_INIT_SERVER_DRIVER_CONTEXT(pDriverContext);

    controllerDriver = new ControllerDriver();
    VRServerDriverHost()->TrackedDeviceAdded(controllerDriver->GetSerialNumber().c_str(),
                                             vr::TrackedDeviceClass_Controller, controllerDriver);

    return VRInitError_None;
}

void ServerDriver::RunFrame() {
    if (controllerDriver)
        controllerDriver->RunFrame();

    VREvent_t vrEvent{};
    while (VRServerDriverHost()->PollNextEvent(&vrEvent, sizeof(vrEvent))) {
        if (controllerDriver)
            controllerDriver->ProcessEvent(vrEvent);
    }
}
