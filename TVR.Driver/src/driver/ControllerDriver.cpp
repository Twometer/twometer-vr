//
// Created by twome on 08/04/2020.
//

#include "ControllerDriver.h"

#include <cmath>

#define deg2rad(angleDegrees) ((angleDegrees) * M_PI / 180.0)

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

vr::DriverPose_t ControllerDriver::GetPose() {
    DriverPose_t pose = {0};
    pose.poseIsValid = controllerState.IsValid();
    pose.result = !controllerState.IsValid() ? TrackingResult_Calibrating_OutOfRange : TrackingResult_Running_OK;
    pose.deviceIsConnected = true;
    pose.shouldApplyHeadModel = false;
    pose.willDriftInYaw = false;

    // No transform due to HMD pose
    pose.qDriverFromHeadRotation.w = pose.qWorldFromDriverRotation.w = 1.f;

    // Apply the positional data
    pose.vecPosition[0] = controllerState.posX;
    pose.vecPosition[1] = controllerState.posY;
    pose.vecPosition[2] = controllerState.posZ;

    pose.qRotation = vrmath::quaternionFromYawPitchRoll(deg2rad(controllerState.yaw), deg2rad(controllerState.pitch), deg2rad(controllerState.roll));

    return pose;
}

std::string ControllerDriver::GetSerialNumber() {
    return serialNumber;
}

ControllerDriver::ControllerDriver(int trackerId, std::string serialNumber)
        : trackerId(trackerId), serialNumber(std::move(serialNumber)),
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

void ControllerDriver::SetControllerState(ControllerState newState) {
    controllerState = std::move(newState);
    vr::VRServerDriverHost()->TrackedDevicePoseUpdated(objectId, GetPose(), sizeof(vr::DriverPose_t));

    vr::VRDriverInput()->UpdateBooleanComponent(buttonA, controllerState.buttons[Button::A], 0);
    vr::VRDriverInput()->UpdateBooleanComponent(buttonB, controllerState.buttons[Button::B], 0);
}

// Unused methods //
void ControllerDriver::EnterStandby() {
}

void *ControllerDriver::GetComponent(const char *pchComponentNameAndVersion) {
    return nullptr;
}

void ControllerDriver::DebugRequest(const char *pchRequest, char *pchResponseBuffer, uint32_t unResponseBufferSize) {
}