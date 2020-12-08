//
// Created by Twometer on 8 Nov 2020.
//

#include "ServerDriver.h"

using namespace vr;

vr::EVRInitError ServerDriver::Init(vr::IVRDriverContext *pDriverContext) {
    return VRInitError_None;
}

void ServerDriver::Cleanup() {
    streamClient.close();
}