#include "IPoseSource.h"

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
