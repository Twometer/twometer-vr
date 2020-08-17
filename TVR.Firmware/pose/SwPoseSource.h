#ifndef TVR_SWPOSESOURCE
#define TVR_SWPOSESOURCE

#include <MPU9250.h>
#include "IPoseSource.h"

#define UPDATE_RATE 1000  // Hz
#define ERROR_RATE 25.0f  // deg/s

/**
 * Pose source that uses the Madgwick algorithm
 * to produce 9-axis sensor fusion data
 */
class SwPoseSource : public IPoseSource {
private:
  MPU9250 mpu;
  Storage storage;

  Timer updateTimer = Timer(UPDATE_RATE);

public:
  void begin() override {
    Wire.begin(PIN_SDA, PIN_SCL);
    Wire.setClock(400000L);

    mpu.setGyroMeasurementError(ERROR_RATE);
    mpu.setAhrs(false);
    mpu.setup();

    if (storage.hasData())
      storage.loadCalibrationData(&mpu);
  }

  void calibrateAccelGyro() override {
    mpu.calibrateAccelGyro();
  }

  void calibrateMagnetometer() override {
    mpu.calibrateMag();

    storage.storeCalibrationData(&mpu);
  }

  void calculateOffsets() override {
  }

  bool update() override {
    if (updateTimer.elapsed()) {
      mpu.update();
      updateTimer.reset();
      return true;
    }
    return false;
  }

  // Out of order because the coordinate system
  // is different from what the server expects.
  float getQx() override {
    return -mpu.getQuaternion(1);
  }

  float getQy() override {
    return mpu.getQuaternion(0);
  }

  float getQz() override {
    return mpu.getQuaternion(2);
  }

  float getQw() override {
    return mpu.getQuaternion(3);
  }

  bool requiresCalibration() override {
    return !storage.hasData();
  }
};

#endif // TVR_SWPOSESOURCE
