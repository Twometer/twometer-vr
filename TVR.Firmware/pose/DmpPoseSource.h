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

  float yawAccum = 0.0f;
  float yawOffset = 0.0f;

  float pitchAccum = 0.0f;
  float pitchOffset = 0.0f;

  float rollAccum = 0.0f;
  float rollOffset = 0.0f;

  int samples = 0;

public:

    void begin() override {
      if (imu.begin(PIN_SDA, PIN_SCL) == INV_SUCCESS) {
        imu.dmpBegin(DMP_FEATURE_6X_LP_QUAT | DMP_FEATURE_GYRO_CAL, UPDATE_RATE);
      }
    }

    void calibrateAccelGyro() override {

    }

    void calibrateMagnetometer() override {

    }

    void calculateOffsets() override {
      uint32_t begin = millis();
      uint32_t elapsed = 0;
      while ((elapsed = millis() - begin) < WARMUP_TIME + CALIB_DURATION) {
        if (update() && elapsed > WARMUP_TIME) {
          yawAccum += imu.yaw;
          pitchAccum += imu.pitch;
          rollAccum += imu.roll;
          samples++;
        }
        delay(5);
      }

      float samplesF = float(samples);
      yawOffset = yawAccum / samplesF;
      pitchOffset = pitchAccum / samplesF;
      rollOffset = rollAccum / samplesF;
    }

    bool update() override {
      if (imu.fifoAvailable() && imu.dmpUpdateFifo() == INV_SUCCESS) {
          imu.computeEulerAngles();
          return true;
      }
      return false;
    }

    float getYaw() override {
      return imu.yaw - yawOffset;
    }

    float getPitch() override {
      return imu.pitch - pitchOffset;
    }

    float getRoll() override {
      return imu.roll - rollOffset;
    }

    bool requiresCalibration() override {
      return false;
    }

};

#endif TVR_DMPPOSESOURCE
