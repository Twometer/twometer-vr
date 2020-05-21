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
      if (imu.fifoAvailable() && imu.dmpUpdateFifo() == INV_SUCCESS)
      {
        Serial.print(getQx()); Serial.print(", ");
        Serial.print(getQy()); Serial.print(", ");
        Serial.print(getQz()); Serial.print(", ");
        Serial.println(getQw());
        return true;
      }
      return false;
    }

    float getQx() override {
      return calcQuat(imu.qx, 30);
    }

    float getQy() override {
      return calcQuat(imu.qy, 30);
    }

    float getQz() override {
      return calcQuat(imu.qz, 30);
    }

    float getQw() override {
      return calcQuat(imu.qw, 30);
    }

    bool requiresCalibration() override {
      return false;
    }

    float calcQuat(long number, uint8_t q) {
      unsigned long mask = 0;
    	for (int i=0; i<q; i++)
    	{
    		mask |= (1<<i);
    	}
    	return (number >> q) + ((number & mask) / (float) (2<<(q-1)));
    }
};

#endif TVR_DMPPOSESOURCE
