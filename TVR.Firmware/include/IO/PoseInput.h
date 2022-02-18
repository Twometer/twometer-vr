#ifndef TVR_POSE_INPUT
#define TVR_POSE_INPUT

#include <ICM20948.h>

#include "../Utils/Constants.h"
#include "../Utils/Logger.h"
#include "../Utils/Vector.h"
#include "Hardware.h"

class PoseInput {
   private:
    ICM20948 icm{};

   public:
    void begin() {
        Wire.begin(PIN_SDA, PIN_SCL);

#if SENSOR_USE_COMPASS
        icm.setFusionMode(FUSION_9_AXIS);
#else
        icm.setFusionMode(FUSION_6_AXIS);
#endif

        int code = icm.begin();
        switch (code) {
        case ICM_SUCCESS:
            Logger::info("IMU initialized successfully.");
            break;
        case ICM_BAD_WHOAMI:
            Logger::crash("IMU init failed: Bad WHOAMI");
            break;
        case ICM_DMP_ERROR:
            Logger::crash("IMU init failed: Failed to flash DMP");
            break;
        case ICM_MAG_ERROR:
            Logger::crash("IMU init failed: Failed to find compass");
            break;
        case ICM_SERIAL_ERROR:
            Logger::crash("IMU init failed: Serial connection error");
            break;
        case ICM_SETUP_ERROR:
            Logger::crash("IMU init failed: Setup failed");
            break;
        }

        icm.setHighPowerMode(true);
#if SENSOR_USE_COMPASS
        icm.startSensor(INV_SENSOR_TYPE_ROTATION_VECTOR, SENSOR_PERIOD_US);
#else
        icm.startSensor(INV_SENSOR_TYPE_GAME_ROTATION_VECTOR, SENSOR_PERIOD_US);
#endif
    }

    void update() {
        icm.update();
    }

    bool available() {
        return icm.available();
    }

    void clearAvailable() {
        return icm.clearAvailable();
    }

    vec4 getPose() {
        return {icm.x(), icm.y(), icm.z(), icm.w()};
    }

    CalibrationData getCalibData() {
        return icm.getCalibrationData();
    }
};

#endif