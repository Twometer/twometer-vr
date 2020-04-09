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
    VRProperties()->SetUint64Property(propertyContainer, Prop_CurrentUniverseId_Uint64, UNIVERSE_ID);
    VRProperties()->SetBoolProperty(propertyContainer, Prop_IsOnDesktop_Bool, false);
    VRProperties()->SetInt32Property(propertyContainer, Prop_ControllerRoleHint_Int32, GetTrackerRole());
    VRProperties()->SetStringProperty(propertyContainer, Prop_InputProfilePath_String,
                                      "{tvr}/input/twometer_vr_profile.json");

    VRDriverInput()->CreateBooleanComponent(propertyContainer, "/input/a/click", &buttonA);
    VRDriverInput()->CreateBooleanComponent(propertyContainer, "/input/b/click", &buttonB);

    return VRInitError_None;
}

void ControllerDriver::Deactivate() {
    objectId = k_unTrackedDeviceIndexInvalid;
}

void ControllerDriver::EnterStandby() {

}

void *ControllerDriver::GetComponent(const char *pchComponentNameAndVersion) {
    return nullptr;
}

void ControllerDriver::DebugRequest(const char *pchRequest, char *pchResponseBuffer, uint32_t unResponseBufferSize) {

}

vr::DriverPose_t ControllerDriver::GetPose() {
    DriverPose_t pose = {0};
    pose.poseIsValid = controllerState.IsValid();
    pose.result = !controllerState.IsValid() ? TrackingResult_Calibrating_OutOfRange : TrackingResult_Running_OK;
    pose.deviceIsConnected = true;
    pose.shouldApplyHeadModel = false;
    pose.willDriftInYaw = false;

    // TODO: Translate absolute controller position to HMD-Space

    // Quaternions
    //pose.qWorldFromDriverRotation = HmdQuaternion_Init( 1, 0, 0, 0 );
    //pose.qDriverFromHeadRotation = HmdQuaternion_Init( 1, 0, 0, 0 );

    return pose;
}

std::string ControllerDriver::GetSerialNumber() {
    return serialNumber;
}

void ControllerDriver::RunFrame() {
    // Update buttons here

    // vr::VRDriverInput()->UpdateBooleanComponent( m_compA, (0x8000 & GetAsyncKeyState( 'A' )) != 0, 0 );
}

void ControllerDriver::ProcessEvent(vr::VREvent_t event) {

}

ControllerDriver::ControllerDriver(int trackerId, StreamClient *streamClient, std::string serialNumber)
        : trackerId(trackerId), streamClient(streamClient), serialNumber(std::move(serialNumber)),
          controllerState(ControllerState::invalid) {
}

int32_t ControllerDriver::GetTrackerRole() {
    if (trackerId == TRACKER_LEFT)
        return TrackedControllerRole_LeftHand;
    else if (trackerId == TRACKER_RIGHT)
        return TrackedControllerRole_RightHand;
    else
        return TrackedControllerRole_Invalid;
}

void ControllerDriver::SetControllerState(ControllerState controllerState) {
    this->controllerState = controllerState;
}
