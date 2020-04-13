//
// Created by twome on 08/04/2020.
//

#include "ControllerDriver.h"

#include <utility>
#include <cmath>

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

    // No transform due to HMD pose
    pose.qDriverFromHeadRotation.w = 1.f;
    pose.qDriverFromHeadRotation.x = 0.0f;
    pose.qDriverFromHeadRotation.y = 0.0f;
    pose.qDriverFromHeadRotation.z = 0.0f;
    pose.qWorldFromDriverRotation.w = 1.f;
    pose.qWorldFromDriverRotation.x = 0.f;
    pose.qWorldFromDriverRotation.y = 0.f;
    pose.qWorldFromDriverRotation.z = 0.f;
    pose.vecDriverFromHeadTranslation[0] = 0.0f;
    pose.vecDriverFromHeadTranslation[1] = 0.0f;
    pose.vecDriverFromHeadTranslation[2] = 0.0f;

    // Apply controller position and rotation
    pose.vecWorldFromDriverTranslation[0] = controllerState.posX;
    pose.vecWorldFromDriverTranslation[1] = controllerState.posY;
    pose.vecWorldFromDriverTranslation[2] = controllerState.posZ;
    pose.qWorldFromDriverRotation = ToQuaternion(controllerState.rotZ, controllerState.rotY, controllerState.rotX);
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
    vr::VRServerDriverHost()->TrackedDevicePoseUpdated(objectId, GetPose(), sizeof(vr::DriverPose_t));
}

vr::HmdQuaternion_t ControllerDriver::ToQuaternion(float yaw, float pitch, float roll) {
    // Abbreviations for the various angular functions
    double cy = cos(yaw * 0.5);
    double sy = sin(yaw * 0.5);
    double cp = cos(pitch * 0.5);
    double sp = sin(pitch * 0.5);
    double cr = cos(roll * 0.5);
    double sr = sin(roll * 0.5);

    HmdQuaternion_t q{};
    q.w = cr * cp * cy + sr * sp * sy;
    q.x = sr * cp * cy - cr * sp * sy;
    q.y = cr * sp * cy + sr * cp * sy;
    q.z = cr * cp * sy - sr * sp * cy;

    return q;
}
