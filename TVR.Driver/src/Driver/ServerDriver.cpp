//
// Created by Twometer on 8 Nov 2020.
//

#include "ServerDriver.h"
#include "TrackerDriver.h"

using namespace vr;

vr::EVRInitError ServerDriver::Init(vr::IVRDriverContext *pDriverContext) {
    VR_INIT_SERVER_DRIVER_CONTEXT(pDriverContext);

    streamClient = new StreamClient();

    streamClient->SetAddTrackerCallback([this](TrackerInfo *tracker) {
        // Only register a new tracker if it does not already exist
        if (knownTrackers.find(tracker->modelNo) == knownTrackers.end()) {
            auto driver = new TrackerDriver(tracker);
            auto deviceClass = GetDeviceClass(tracker);
            VRServerDriverHost()->TrackedDeviceAdded(tracker->modelNo.c_str(), deviceClass, driver);
            knownTrackers.insert(tracker->modelNo);
        }

        // Always set to connected
        tracker->connected = true;

        VRDriverLog()->Log("New tracker connected");
    });

    streamClient->SetRemoveTrackerCallback([](TrackerInfo *tracker) {
        tracker->connected = false;
        VRDriverLog()->Log("Tracker disconnected");
    });

    VRDriverLog()->Log("Server driver initialized");

    return VRInitError_None;
}

void ServerDriver::Cleanup() {
    streamClient->Close();
    delete streamClient;
}

vr::ETrackedDeviceClass ServerDriver::GetDeviceClass(TrackerInfo *tracker) {
    switch (tracker->trackerClass) {
        case TrackerClass::Controller:
            return vr::TrackedDeviceClass_Controller;
        case TrackerClass::BodyTracker:
        case TrackerClass::Generic:
            return vr::TrackedDeviceClass_GenericTracker;
        default:
            return vr::TrackedDeviceClass_Invalid;
    }
}
