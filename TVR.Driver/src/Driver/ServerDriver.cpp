//
// Created by Twometer on 8 Nov 2020.
//

#include "ServerDriver.h"

using namespace vr;

vr::EVRInitError ServerDriver::Init(vr::IVRDriverContext *pDriverContext) {
    streamClient = new StreamClient();

    streamClient->SetAddTrackerCallback([](TrackerInfo *tracker) {

    });

    streamClient->SetRemoveTrackerCallback([](TrackerInfo *tracker) {

    });

    return VRInitError_None;
}

void ServerDriver::Cleanup() {
    streamClient->Close();
    delete streamClient;
}