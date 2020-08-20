#ifndef TVR_RAWSOURCE
#define TVR_RAWSOURCE

#include <MPU9250.h>

class RawSource {
private:
  MPU9250 mpu;
  Storage storage;

public:
  float ax;
  float ay;
  float az;
  float gx;
  float gy;
  float gz;
  float mx;
  float my;
  float mz;

  void begin() {
    Wire.begin(PIN_SDA, PIN_SCL);
    Wire.setClock(400000L);

    mpu.setup();

    if (storage.hasData())
      storage.loadCalibrationData(&mpu);
  }

  void calibrateAccelGyro() {
    mpu.calibrateAccelGyro();
  }

  void calibrateMagnetometer() {
    mpu.calibrateMag();
    storage.storeCalibrationData(&mpu);
  }

  bool update() {
    if (mpu.readData()) {
      ax = -mpu.getAcc(0);
      ay = mpu.getAcc(1);
      az = mpu.getAcc(2);

      gx = mpu.getGyro(0);
      gy = -mpu.getGyro(1);
      gz = -mpu.getGyro(2);

      mx = mpu.getMag(1);
      my = -mpu.getMag(0);
      mz = mpu.getMag(2);
      return true;
    }
    return false;
  }

  bool requiresCalibration() {
    return !storage.hasData();
  }

};

#endif
