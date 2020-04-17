//
// Created by twome on 08/04/2020.
//

#include <thread>
#include "ServerDriver.h"

using namespace vr;

void ServerDriver::Cleanup() {
    for (auto drv : *controllerDrivers)
        delete drv;
    delete controllerDrivers;
}

vr::EVRInitError ServerDriver::Init(vr::IVRDriverContext *pDriverContext) {
    VR_INIT_SERVER_DRIVER_CONTEXT(pDriverContext);

    streamClient = new StreamClient([this](const DataPacket &dataPacket) {
        for (const ControllerState &state : dataPacket.controllerStates) {
            ControllerDriver *driver = (*controllerDrivers)[state.controllerId];
            driver->SetControllerState(state);
        }
    });
    if (!streamClient->Connect())
        return VRInitError_Init_WebServerFailed;

    streamThread = new std::thread([&] { streamClient->ReceiveLoop(); });

    controllerDrivers = new std::vector<ControllerDriver *>();
    controllerDrivers->push_back(new ControllerDriver(TRACKER_LEFT, "TVRXTL_0001"));
    controllerDrivers->push_back(new ControllerDriver(TRACKER_RIGHT, "TVRXTL_0002"));

    for (auto drv : *controllerDrivers)
        VRServerDriverHost()->TrackedDeviceAdded(drv->GetSerialNumber().c_str(), vr::TrackedDeviceClass_Controller, drv);

    return VRInitError_None;
}

void ServerDriver::RunFrame() {
}