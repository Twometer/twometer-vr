#ifndef TVR_DMPPOSESOURCE
#define TVR_DMPPOSESOURCE

#include <SparkFunMPU9250-DMP.h>
#include "IPoseSource.h"

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
      return imu.fifoAvailable() && imu.dmpUpdateFifo() == INV_SUCCESS;
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
    	for (int i=0; i < q; i++) {
    		mask |= (1 << i);
    	}
    	return (number >> q) + ((number & mask) / (float) (2 << (q - 1)));
    }
};

#endif TVR_DMPPOSESOURCE
