//
// Created by Twometer on 8 Dec 2020.
//

#include "TrackerDriver.h"
#include "../Utils/Constants.h"


using namespace vr;

TrackerDriver::TrackerDriver(TrackerInfo *tracker) : tracker(tracker) {}

EVRInitError TrackerDriver::Activate(uint32_t unObjectId) {
    PropertyContainerHandle_t propertyContainer = VRProperties()->TrackedDeviceToPropertyContainer(unObjectId);
    VRProperties()->SetStringProperty(propertyContainer, Prop_ModelNumber_String, "TVRCTRLV2");
    VRProperties()->SetStringProperty(propertyContainer, Prop_SerialNumber_String, tracker->modelNo.c_str());
    VRProperties()->SetStringProperty(propertyContainer, Prop_RenderModelName_String, "Twometer VR Tracker");
    VRProperties()->SetUint64Property(propertyContainer, Prop_CurrentUniverseId_Uint64, UNIVERSE_ID);
    VRProperties()->SetBoolProperty(propertyContainer, Prop_IsOnDesktop_Bool, false);
    VRProperties()->SetInt32Property(propertyContainer, Prop_ControllerRoleHint_Int32, GetTrackerRole());
    VRProperties()->SetStringProperty(propertyContainer, Prop_InputProfilePath_String,
                                      "{tvr}/input/twometer_vr_profile.json");

    return VRInitError_None;
}

DriverPose_t TrackerDriver::GetPose() {
    DriverPose_t pose{};

    pose.result = TrackingResult_Running_OK;
    pose.poseIsValid = tracker->connected;
    pose.deviceIsConnected = tracker->connected;
    pose.shouldApplyHeadModel = false;
    pose.willDriftInYaw = false;

    pose.qDriverFromHeadRotation.w = pose.qWorldFromDriverRotation.w = 1.0f;

    TrackerState &state = tracker->trackerState;
    pose.vecPosition[0] = state.position.x;
    pose.vecPosition[1] = state.position.y;
    pose.vecPosition[2] = state.position.z;

    pose.qRotation = {state.rotation.x, state.rotation.y, state.rotation.z, state.rotation.w};

    return pose;
}

int32_t TrackerDriver::GetTrackerRole() {
    if (tracker->trackerClass != TrackerClass::Controller)
        return TrackedControllerRole_OptOut;

    switch (tracker->trackerColor) {
        case TrackerColor::Red:
            return TrackedControllerRole_LeftHand;
        case TrackerColor::Blue:
            return TrackedControllerRole_RightHand;
        default:
            return TrackedControllerRole_Invalid;
    }
}



