#include <functional>
#include <SparkFunMPU9250-DMP.h>
#include "DriftCorrection.h"

#define PIN_SDA 4
#define PIN_SCL 5

#define WARMUP_TIME 15000       // Wait a few seconds for the chip to settle down, then start calibration
#define CALIB_DURATION 2750     // Data collection should last 2.75 seconds. Then, we calculate the offsets.

#define UPDATE_RATE 90

// #define USE_DRIFT_CORRECTION  // Drift correction does not work ATM due to compass not doing what it is supposed to

class MPUSensor {
  private:
    MPU9250_DMP imu{};
#ifdef USE_DRIFT_CORRECTION
    DriftCorrection driftCorrection;
#endif

    unsigned long calibrationStarted = 0;
    bool calibrated = false;

    float yawOffset  = 0;
    float pitchOffset = 0;
    float rollOffset = 0;
    int samples = 0;

    std::function<void()> calibrationCompleted;

  public:
    bool begin() {
      if (imu.begin(PIN_SDA, PIN_SCL) == INV_SUCCESS) {
        imu.setAccelFSR(4);
        imu.dmpBegin(DMP_FEATURE_6X_LP_QUAT | DMP_FEATURE_GYRO_CAL | DMP_FEATURE_SEND_CAL_GYRO, UPDATE_RATE);
        return true;
      }
      return false;
    }

    bool hasData() {
      if (imu.fifoAvailable()) {
        if (imu.dmpUpdateFifo() == INV_SUCCESS) {
          return true;
        }
      }
      return false;
    }

    void beginCalibration(std::function<void()> calibrationCompleted) {
      calibrationStarted = millis();
      this->calibrationCompleted = std::move(calibrationCompleted);
    }

    void update() {
      imu.computeEulerAngles();
      if (!calibrated) {
        calibrationUpdate();
      }
#ifdef USE_DRIFT_CORRECTION
      else {
        driftCorrection.update(&imu, imu.yaw - yawOffset);
      }
#endif
    }

    void calibrationUpdate() {
      unsigned long elapsed = millis() - calibrationStarted;

      if (elapsed > WARMUP_TIME + CALIB_DURATION) {
        float samplesF = float(samples);

        yawOffset /= samplesF;
        pitchOffset /= samplesF;
        rollOffset /= samplesF;

        calibrated = true;
        Serial.print("Sensor calibration completed with ");
        Serial.print(samples);
        Serial.println(" data samples.");

        Serial.print("Offsets: Yaw=");
        Serial.print(yawOffset);
        Serial.print(", Pitch=");
        Serial.print(pitchOffset);
        Serial.print(", Roll=");
        Serial.println(rollOffset);
#ifdef USE_DRIFT_CORRECTION
        driftCorrection.finishCalibration();
#endif
        calibrationCompleted();
      } else if (elapsed > WARMUP_TIME) {
        yawOffset += imu.yaw;
        pitchOffset += imu.pitch;
        rollOffset += imu.roll;
        samples++;

#ifdef USE_DRIFT_CORRECTION
        driftCorrection.update(&imu, imu.yaw - yawOffset);
#endif
      }
    }

    bool isCalibrated() {
      return calibrated;
    }

    float getYaw() {
#ifdef USE_DRIFT_CORRECTION
      return driftCorrection.getCorrectedYaw();
#else
      return imu.yaw - yawOffset;
#endif
    }

    float getPitch() {
      return imu.pitch - pitchOffset;
    }

    float getRoll() {
      return imu.roll - rollOffset;
    }

};
