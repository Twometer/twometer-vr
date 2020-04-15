//
// Created by twome on 08/04/2020.
//

#ifndef TVRDRV_CONTROLLERDRIVER_H
#define TVRDRV_CONTROLLERDRIVER_H


#include <openvr_driver.h>
#include "../net/StreamClient.h"
#include "../model/Button.h"

#define TRACKER_LEFT  0
#define TRACKER_RIGHT 1

#define UNIVERSE_ID 42

class ControllerDriver : public vr::ITrackedDeviceServerDriver {
private:
    int trackerId = -1;
    std::string serialNumber;
    uint32_t objectId = vr::k_unTrackedDeviceIndexInvalid;

    vr::PropertyContainerHandle_t propertyContainer = vr::k_ulInvalidPropertyContainer;

    ControllerState controllerState;

    vr::VRInputComponentHandle_t buttonA{};
    vr::VRInputComponentHandle_t buttonB{};

    int32_t GetTrackerRole();

    static vr::HmdQuaternion_t ToQuaternion(float yaw, float pitch, float roll);

public:
    ControllerDriver(int trackerId, std::string serialNumber);

    vr::EVRInitError Activate(uint32_t unObjectId) override;

    void Deactivate() override;

    void EnterStandby() override;

    void *GetComponent(const char *pchComponentNameAndVersion) override;

    void DebugRequest(const char *pchRequest, char *pchResponseBuffer, uint32_t unResponseBufferSize) override;

    vr::DriverPose_t GetPose() override;

    std::string GetSerialNumber();

    void SetControllerState(ControllerState newState);
};


#endif //TVRDRV_CONTROLLERDRIVER_H
