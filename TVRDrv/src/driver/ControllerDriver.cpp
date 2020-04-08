//
// Created by twome on 08/04/2020.
//

#include "ControllerDriver.h"

#include <utility>

using namespace vr; 

vr::EVRInitError ControllerDriver::Activate(uint32_t unObjectId) {
    this->objectId = unObjectId;
    this->propertyContainer = VRProperties()->TrackedDeviceToPropertyContainer(unObjectId);

    VRProperties()->SetStringProperty(propertyContainer, Prop_ModelNumber_String, "TwometerVRCtrl");
    VRProperties()->SetStringProperty(propertyContainer, Prop_RenderModelName_String, "Twometer VR Controller");
    VRProperties()->SetUint64Property(propertyContainer, Prop_CurrentUniverseId_Uint64, 2);
    VRProperties()->SetBoolProperty(propertyContainer, Prop_IsOnDesktop_Bool, false);

    return VRInitError_None;
}

void ControllerDriver::Deactivate() {

}

void ControllerDriver::EnterStandby() {

}

void *ControllerDriver::GetComponent(const char *pchComponentNameAndVersion) {
    return nullptr;
}

void ControllerDriver::DebugRequest(const char *pchRequest, char *pchResponseBuffer, uint32_t unResponseBufferSize) {

}

vr::DriverPose_t ControllerDriver::GetPose() {
    return vr::DriverPose_t();
}

std::string ControllerDriver::GetSerialNumber() {
    return serialNumber;
}

void ControllerDriver::RunFrame() {

}

void ControllerDriver::ProcessEvent(vr::VREvent_t event) {

}

ControllerDriver::ControllerDriver(StreamClient *streamClient, std::string serialNumber) : streamClient(streamClient),
                                                                                           serialNumber(std::move(serialNumber)) {
}
