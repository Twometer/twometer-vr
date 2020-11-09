//
// Created by Twometer on 8 Nov 2020.
//

#include "TrackerProvider.h"

using namespace vr;

vr::EVRInitError TrackerProvider::Init(vr::IVRDriverContext *pDriverContext) {
    return VRInitError_None;
}

void TrackerProvider::Cleanup() {
    streamClient.close();
}