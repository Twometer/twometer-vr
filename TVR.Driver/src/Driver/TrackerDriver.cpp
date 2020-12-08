//
// Created by Twometer on 08/12/2020.
//

#include "TrackerDriver.h"

#include <utility>

using namespace vr;

constexpr uint64_t UNIVERSE_ID = 2688095746;

TrackerDriver::TrackerDriver(std::string modelNumber, TrackerState *state) : modelNumber(std::move(modelNumber)),
                                                                             state(state) {}

EVRInitError TrackerDriver::Activate(uint32_t unObjectId) {
    PropertyContainerHandle_t propertyContainer = VRProperties()->TrackedDeviceToPropertyContainer(unObjectId);
    VRProperties()->SetStringProperty(propertyContainer, Prop_ModelNumber_String, modelNumber.c_str());
    VRProperties()->SetStringProperty(propertyContainer, Prop_RenderModelName_String, "Twometer VR Tracker");
    VRProperties()->SetUint64Property(propertyContainer, Prop_CurrentUniverseId_Uint64, UNIVERSE_ID);

    return VRInitError_None;
}

DriverPose_t TrackerDriver::GetPose() {
    DriverPose_t pose{};

    return pose;
}


