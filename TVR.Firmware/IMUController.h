#include <MPU9250.h>

#define PIN_SDA 4
#define PIN_SCL 5

#define UPDATE_RATE  90   // Hz
#define UPDATE_DELAY (1.0 / UPDATE_RATE)

#define MAGNETIC_DECLINATION 2.85 // Deg

#define WARMUP_TIME 9000        // Wait a few seconds for the values to settle down, then start data collection
#define CALIB_DURATION 2750     // Data collection should last 2.75 seconds. Then, we calculate the offsets.

class IMUController {
  private:
    MPU9250 mpu;
    uint32_t lastUpdate = 0;

    float yawAccum = 0.0f;
    float yawOffset = 0.0f;

    float pitchAccum = 0.0f;
    float pitchOffset = 0.0f;

    float rollAccum = 0.0f;
    float rollOffset = 0.0f;

    int samples = 0;
  public: 
    void begin() {
      Wire.begin(PIN_SDA, PIN_SCL);
      mpu.setup();
    }

    void calibrateAccelGyro() {
      mpu.calibrateAccelGyro();
    }

    void calibrateMagnetometer() {
      mpu.calibrateMag();
      mpu.setMagneticDeclination(MAGNETIC_DECLINATION);
    }

    void calcOffsets() {
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
    }

    bool update() {
      if (millis() - lastUpdate > UPDATE_DELAY) {
        mpu.update();
        lastUpdate = millis();
        return true;
      }
      return false;
    }

    MPU9250* getMpu() {
      return &mpu;
    }

    float getYaw() {
      return mpu.getYaw() - yawOffset;
    }

    float getPitch() {
      return mpu.getPitch() - pitchOffset;
    }

    float getRoll() {
      return mpu.getRoll() - rollOffset;
    }

};
