#ifndef TVR_DMPPOSESOURCE
#define TVR_DMPPOSESOURCE

#include <SparkFunMPU9250-DMP.h>
#include "IPoseSource.h"

#define PIN_SDA 4
#define PIN_SCL 5

#define WARMUP_TIME 15000       // Wait a few seconds for the chip to settle down, then start calibration
#define CALIB_DURATION 3000     // Data collection should last 3 seconds. Then, we calculate the offsets.

#define UPDATE_RATE 90

// #define USE_COMPASS

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

#ifdef USE_COMPASS
  float head0;
  Timer yawResetTimer;
#endif

public:

    void begin() override {
      Wire.setClock(400000L);
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
#ifdef USE_COMPASS
        if (imu.dataReady()) {
          imu.update(UPDATE_COMPASS);
          imu.computeCompassHeading();
        }
#endif
        delay(5);
      }

      float samplesF = float(samples);
      yawOffset = yawAccum / samplesF;
      pitchOffset = pitchAccum / samplesF;
      rollOffset = rollAccum / samplesF;

      Serial.print("Yaw offset: "); Serial.println(yawOffset);
      Serial.print("Pitch offset: "); Serial.println(pitchOffset);
      Serial.print("Roll offset: "); Serial.println(rollOffset);

#ifdef USE_COMPASS
      head0 = imu.heading;
      Serial.print("head0: "); Serial.println(head0);
#endif
    }

    bool update() override {
#ifdef USE_COMPASS
      if (imu.dataReady()) {
        imu.update(UPDATE_COMPASS);
        imu.computeCompassHeading();
        if (abs(imu.heading - head0) < 0.25 && yawResetTimer.elapsed(15000)) {
          resetYaw();
          yawResetTimer.reset();
        }
      }
#endif

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

private:
    void resetYaw() {
      yawOffset = imu.yaw;
    }

};

#endif TVR_DMPPOSESOURCE
