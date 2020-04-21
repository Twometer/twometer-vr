#include <SparkFunMPU9250-DMP.h>

#define PIN_SDA 4
#define PIN_SCL 5

#define WARMUP_TIME 15000        // Wait 5 seconds for the chip to settle down, then start calibration
#define CALIB_DURATION 2750     // Data collection should last 2.75 seconds. Then, we calculate the offsets.

class MPUSensor {
  private:
    MPU9250_DMP imu{};

    unsigned long calibrationStarted = 0;
    bool calibrated = false;

    float yawOffset  = 0;
    float pitchOffset = 0;
    float rollOffset = 0;
    int samples = 0;

  public:
    bool begin() {
      if (imu.begin(PIN_SDA, PIN_SCL) == INV_SUCCESS) {
        // Initialize the DMP with 6-Axis Gyro and 60 Hz update rate
        imu.dmpBegin(DMP_FEATURE_6X_LP_QUAT | DMP_FEATURE_GYRO_CAL, 60);
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

    void beginCalibration() {
      calibrationStarted = millis();
    }

    void update() {
      imu.computeEulerAngles();
      if (!calibrated) {
        calibrationUpdate();
      }
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
      } else if (elapsed > WARMUP_TIME) {
        yawOffset += imu.yaw;
        pitchOffset += imu.pitch;
        rollOffset += imu.roll;

        samples++;
      }
    }

    bool isCalibrated() {
      return calibrated;
    }

    float getYaw() {
      return imu.yaw - yawOffset;
    }

    float getPitch() {
      return imu.pitch - pitchOffset;
    }

    float getRoll() {
      return imu.roll - rollOffset;
    }

};
