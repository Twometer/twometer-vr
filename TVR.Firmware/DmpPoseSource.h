#include <SparkFunMPU9250-DMP.h>
#include "IPoseSource.h"

#define PIN_SDA 4
#define PIN_SCL 5

#define WARMUP_TIME 15000       // Wait a few seconds for the chip to settle down, then start calibration
#define CALIB_DURATION 2750     // Data collection should last 2.75 seconds. Then, we calculate the offsets.

#define UPDATE_RATE 90

// Pose source that uses the MPU's DMP chip
// to do 6-axis sensor fusion
class DmpPoseSource : public IPoseSource {

    void begin() override {

    }

    void calibrateAccelGyro() override {

    }

    void calibrateMagnetometer() override {

    }

    void calculateOffsets() override {

    }

    void update() override {

    }

    float getYaw() override {
      return 0;
    }

    float getPitch() override {
      return 0;
    }

    float getRoll() override {
      return 0;
    }

};
