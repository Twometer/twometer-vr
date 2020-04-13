//
// Created by twome on 08/04/2020.
//

#ifndef TVRDRV_CONTROLLERDRIVER_H
#define TVRDRV_CONTROLLERDRIVER_H


#include <openvr_driver.h>
#include "../net/StreamClient.h"

#define TRACKER_LEFT  0
#define TRACKER_RIGHT 1

#define UNIVERSE_ID 42

class ControllerDriver : public vr::ITrackedDeviceServerDriver {
private:
    StreamClient *streamClient = nullptr;

    std::string serialNumber;

    vr::PropertyContainerHandle_t propertyContainer = vr::k_ulInvalidPropertyContainer;

    int trackerId = -1;

    uint32_t objectId = vr::k_unTrackedDeviceIndexInvalid;

    vr::VRInputComponentHandle_t buttonA;
    vr::VRInputComponentHandle_t buttonB;

    ControllerState controllerState;

    int32_t GetTrackerRole();

    vr::HmdQuaternion_t ToQuaternion(float yaw_z, float pitch_y, float roll_x);

public:
    ControllerDriver(int trackerId, StreamClient *streamClient, std::string serialNumber);

    vr::EVRInitError Activate(uint32_t unObjectId) override;

    void Deactivate() override;

    void EnterStandby() override;

    void *GetComponent(const char *pchComponentNameAndVersion) override;

    void DebugRequest(const char *pchRequest, char *pchResponseBuffer, uint32_t unResponseBufferSize) override;

    vr::DriverPose_t GetPose() override;

    std::string GetSerialNumber();

    void RunFrame();

    void ProcessEvent(vr::VREvent_t event);

    void SetControllerState(ControllerState controllerState);
};


#endif //TVRDRV_CONTROLLERDRIVER_H
