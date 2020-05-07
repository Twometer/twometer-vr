#include "IPoseSource.h"

// Pose source that uses the Madgwick algorithm
// to produce 9-axis sensor fusion data
class SwPoseSource : public IPoseSource {

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
