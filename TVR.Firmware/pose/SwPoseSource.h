#ifndef TVR_SWPOSESOURCE
#define TVR_SWPOSESOURCE

#include <MPU9250.h>
#include "IPoseSource.h"

#define PIN_SDA 4
#define PIN_SCL 5

#define UPDATE_RATE  90   // Hz
#define UPDATE_DELAY (1.0 / UPDATE_RATE)

#define MAGNETIC_DECLINATION 2.85 // Deg

#define WARMUP_TIME 15000        // Wait a few seconds for the values to settle down, then start data collection
#define CALIB_DURATION 2750     // Data collection should last 2.75 seconds. Then, we calculate the offsets.

/**
 * Pose source that uses the Madgwick algorithm
 * to produce 9-axis sensor fusion data
 */
class SwPoseSource : public IPoseSource {
  private:
    MPU9250 mpu;
    Storage storage;
    uint32_t lastUpdate = 0;

    float yawAccum = 0.0f;
    float yawOffset = 0.0f;

    float pitchAccum = 0.0f;
    float pitchOffset = 0.0f;

    float rollAccum = 0.0f;
    float rollOffset = 0.0f;

    int samples = 0;

    Timer updateTimer;

  public:

    void begin() override {
      Wire.begin(PIN_SDA, PIN_SCL);
      Wire.setClock(400000L);
      mpu.setup();
    }

    void calibrateAccelGyro() override {
      mpu.calibrateAccelGyro();
    }

    void calibrateMagnetometer() override {
      mpu.setMagneticDeclination(MAGNETIC_DECLINATION);
      mpu.calibrateMag();
    }

    void calculateOffsets() override {
      uint32_t begin = millis();
      uint32_t elapsed = 0;
      while ((elapsed = millis() - begin) < WARMUP_TIME + CALIB_DURATION) {
        mpu.update();
        if (elapsed > WARMUP_TIME) {
          yawAccum += mpu.getYaw();
          pitchAccum += mpu.getPitch();
          rollAccum += mpu.getRoll();
          samples++;
        }
        delay(50);
      }

      float samplesF = float(samples);
      yawOffset = yawAccum / samplesF;
      pitchOffset = pitchAccum / samplesF;
      rollOffset = rollAccum / samplesF;

      Serial.print("Yaw offset: "); Serial.println(yawOffset);
      Serial.print("Pitch offset: "); Serial.println(pitchOffset);
      Serial.print("Roll offset: "); Serial.println(rollOffset);
    }

    bool update() override {
      mpu.update();
      return true;
    }

    float getYaw() override {
      return mpu.getYaw() - yawOffset;
    }

    float getPitch() override {
      return mpu.getPitch() - pitchOffset;
    }

    float getRoll() override {
      return mpu.getRoll() - rollOffset;
    }

    bool requiresCalibration() override {
      return true; // !storage.hasData()
    }

};

#endif // TVR_SWPOSESOURCE
