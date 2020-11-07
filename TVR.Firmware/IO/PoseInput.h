#ifndef TVR_POSE_INPUT
#define TVR_POSE_INPUT

#include <ICM-20948.h>
#include "Hardware.h"
#include "../Utils/Logger.h"
#include "../Utils/Constants.h"

class PoseInput
{
private:
    ICM20948 icm{};

public:
    void begin()
    {
        Wire.begin(PIN_SDA, PIN_SCL);

        int code = icm.begin();
        switch (code)
        {
        case ERR_OK:
            Logger::info("IMU initialized successfully.");
            break;
        case ERR_BAD_WHOAMI:
            Logger::crash("IMU init failed: Bad WHOAMI");
            break;
        case ERR_DMP_FAILED:
            Logger::crash("IMU init failed: Failed to flash DMP");
            break;
        case ERR_AUX_FAILED:
            Logger::crash("IMU init failed: Failed to find compass");
            break;
        }

        icm.enableSensor(INV_SENSOR_TYPE_GYROSCOPE);
        icm.enableSensor(INV_SENSOR_TYPE_ACCELEROMETER);
        icm.enableSensor(INV_SENSOR_TYPE_MAGNETOMETER);
        icm.enableSensor(INV_SENSOR_TYPE_ROTATION_VECTOR);
        icm.setSampleRate(INV_SENSOR_TYPE_ROTATION_VECTOR, SENSOR_RATE);
    }

    void update()
    {
        icm.update();
    }

    bool available()
    {
        return icm.available();
    }

    void clearAvailable()
    {
        return icm.clearAvailable();
    }

    vec4 &getPose()
    {
        return icm.getRotation();
    }
};

#endif