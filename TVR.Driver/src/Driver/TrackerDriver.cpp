//
// Created by Twometer on 08/12/2020.
//

#include "TrackerDriver.h"
#include "../Utils/Constants.h"


using namespace vr;

TrackerDriver::TrackerDriver(TrackerInfo *tracker) : tracker(tracker) {}

EVRInitError TrackerDriver::Activate(uint32_t unObjectId) {
    PropertyContainerHandle_t propertyContainer = VRProperties()->TrackedDeviceToPropertyContainer(unObjectId);
    VRProperties()->SetStringProperty(propertyContainer, Prop_ModelNumber_String, tracker->modelNo.c_str());
    VRProperties()->SetStringProperty(propertyContainer, Prop_RenderModelName_String, "Twometer VR Tracker");
    VRProperties()->SetUint64Property(propertyContainer, Prop_CurrentUniverseId_Uint64, UNIVERSE_ID);
    VRProperties()->SetBoolProperty(propertyContainer, Prop_IsOnDesktop_Bool, false);

    return VRInitError_None;
}

DriverPose_t TrackerDriver::GetPose() {
    DriverPose_t pose{};

    pose.result = TrackingResult_Running_OK;
    pose.poseIsValid = true;
    pose.deviceIsConnected = true;
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



