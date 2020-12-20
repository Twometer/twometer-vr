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
                                      "{tvr}/input/tvr_handed_profile.json");

    VRDriverInput()->CreateBooleanComponent(propertyContainer, "/input/a/click", &buttonA);
    VRDriverInput()->CreateBooleanComponent(propertyContainer, "/input/b/click", &buttonB);
    VRDriverInput()->CreateBooleanComponent(propertyContainer, "/input/up/click", &buttonUp);
    VRDriverInput()->CreateBooleanComponent(propertyContainer, "/input/left/click", &buttonLeft);
    VRDriverInput()->CreateBooleanComponent(propertyContainer, "/input/right/click", &buttonRight);
    VRDriverInput()->CreateBooleanComponent(propertyContainer, "/input/down/click", &buttonDown);
    VRDriverInput()->CreateBooleanComponent(propertyContainer, "/input/menu/click", &buttonMenu);
    VRDriverInput()->CreateBooleanComponent(propertyContainer, "/input/trigger/click", &buttonTrigger);

    return VRInitError_None;
}

DriverPose_t TrackerDriver::GetPose() {
    UpdateButtons();

    DriverPose_t pose{};

    pose.result = TrackingResult_Running_OK;
    pose.poseIsValid = tracker->connected;
    pose.deviceIsConnected = tracker->connected;
    pose.shouldApplyHeadModel = false;
    pose.willDriftInYaw = false;

    pose.qDriverFromHeadRotation.w = pose.qWorldFromDriverRotation.w = 1.0f; // No rotation relative to head or world

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

void TrackerDriver::UpdateButtons() {
    VRDriverInput()->UpdateBooleanComponent(buttonA, IsButtonPressed(TrackerButton::A), 0);
    VRDriverInput()->UpdateBooleanComponent(buttonB, IsButtonPressed(TrackerButton::B), 0);
    VRDriverInput()->UpdateBooleanComponent(buttonUp, IsButtonPressed(TrackerButton::Up), 0);
    VRDriverInput()->UpdateBooleanComponent(buttonLeft, IsButtonPressed(TrackerButton::Left), 0);
    VRDriverInput()->UpdateBooleanComponent(buttonRight, IsButtonPressed(TrackerButton::Right), 0);
    VRDriverInput()->UpdateBooleanComponent(buttonDown, IsButtonPressed(TrackerButton::Down), 0);
    VRDriverInput()->UpdateBooleanComponent(buttonMenu, IsButtonPressed(TrackerButton::Menu), 0);
    VRDriverInput()->UpdateBooleanComponent(buttonTrigger, IsButtonPressed(TrackerButton::Trigger), 0);
}

bool TrackerDriver::IsButtonPressed(TrackerButton button) {
    return (tracker->trackerState.buttons & static_cast<uint16_t> (button)) != 0;
}



