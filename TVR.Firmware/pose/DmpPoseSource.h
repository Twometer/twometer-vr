#ifndef TVR_DMPPOSESOURCE
#define TVR_DMPPOSESOURCE

#include <SparkFunMPU9250-DMP.h>
#include "IPoseSource.h"

#define PIN_SDA 4
#define PIN_SCL 5

#define WARMUP_TIME 15000       // Wait a few seconds for the chip to settle down, then start calibration
#define CALIB_DURATION 3000     // Data collection should last 3 seconds. Then, we calculate the offsets.

#define UPDATE_RATE 90

/**
 * Pose source that uses the MPU's DMP chip
 * to do 6-axis sensor fusion
 */
class DmpPoseSource : public IPoseSource {
private:
  MPU9250_DMP imu{};

public:
    void begin() override {
      Wire.setClock(400000L);
      if (imu.begin(PIN_SDA, PIN_SCL) == INV_SUCCESS) {
        imu.dmpBegin(DMP_FEATURE_6X_LP_QUAT | DMP_FEATURE_GYRO_CAL, UPDATE_RATE);
      }
    }

    // The DMP does this stuff for us
    void calibrateAccelGyro() override {
    }

    void calibrateMagnetometer() override {
    }

    void calculateOffsets() override {
    }

    bool update() override {
      return imu.fifoAvailable() && imu.dmpUpdateFifo() == INV_SUCCESS;
    }

    float getQx() override {
      return imu.qx;
    }

    float getQy() override {
      return imu.qy;
    }

    float getQz() override {
      return imu.qz;
    }

    float getQw() override {
      return imu.qw;
    }

    bool requiresCalibration() override {
      return false;
    }

};

#endif TVR_DMPPOSESOURCE
