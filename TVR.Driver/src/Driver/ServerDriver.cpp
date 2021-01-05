//
// Created by Twometer on 8 Nov 2020.
//

#include "ServerDriver.h"

using namespace vr;

vr::EVRInitError ServerDriver::Init(vr::IVRDriverContext *pDriverContext) {
    VR_INIT_SERVER_DRIVER_CONTEXT(pDriverContext);

    streamClient = new StreamClient();

    streamClient->SetAddTrackerCallback([this](TrackerInfo *tracker) {
        if (knownDrivers.find(tracker->serialNo) == knownDrivers.end()) {
            // Only register a new driver if it does not already exist
            auto driver = new TrackerDriver(tracker);
            auto deviceClass = GetDeviceClass(tracker);
            VRServerDriverHost()->TrackedDeviceAdded(tracker->serialNo.c_str(), deviceClass, driver);

            tracker->context = driver;
            knownDrivers[tracker->serialNo] = driver;
        } else {
            // If it exists, reuse it for the new tracker info
            auto driver = knownDrivers[tracker->serialNo];
            tracker->context = driver;
            driver->SetTracker(tracker);
        }

        // Always set connected
        tracker->connected = true;

        VRDriverLog()->Log("New tracker connected");
    });

    streamClient->SetUpdateTrackerCallback([](TrackerInfo *tracker) {
        auto driver = (TrackerDriver *) tracker->context;
        if (driver != nullptr)
            driver->Update();
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
