#ifndef ROTATION_SENSOR_H
#define ROTATION_SENSOR_H

#define MPU6050_DMP_FIFO_RATE_DIVISOR 0x03
#include "I2Cdev.h"
#include "MPU6050_6Axis_MotionApps20.h"
#include "Wire.h"

class RotationSensor {
  private:
    MPU6050 mpu;
    uint16_t packetSize = 0;
    uint16_t fifoCount = 0;
    uint8_t fifoBuffer[64];
    Quaternion q{};
    VectorFloat gravity{};
    float ypr[3];

  public:
    void begin() {
      Wire.begin(4, 5);
      Wire.setClock(400000);

      mpu.initialize();
      mpu.dmpInitialize();

      mpu.setXGyroOffset(220);
      mpu.setYGyroOffset(76);
      mpu.setZGyroOffset(-85);
      mpu.setZAccelOffset(1788);
      mpu.CalibrateAccel(6);
      mpu.CalibrateGyro(6);
      mpu.PrintActiveOffsets();

      mpu.setDMPEnabled(true);
      packetSize = mpu.dmpGetFIFOPacketSize();
      fifoCount = mpu.getFIFOCount();
    }
    void update() {
      while (fifoCount < packetSize) {
        fifoCount = mpu.getFIFOCount();
      }

      if (fifoCount == 1024) {
        mpu.resetFIFO();
        Serial.println(F("FIFO overflow!"));
      } else {
        if (fifoCount % packetSize != 0) {
          mpu.resetFIFO();
        }
        else {
          while (fifoCount >= packetSize) {
            mpu.getFIFOBytes(fifoBuffer, packetSize);
            fifoCount -= packetSize;
          }

          mpu.dmpGetQuaternion(&q, fifoBuffer);
          mpu.dmpGetGravity(&gravity, &q);
          mpu.dmpGetYawPitchRoll(ypr, &q, &gravity);
        }
      }
    }

    float getYaw() {
      return ypr[0] * 180 / PI;
    }

    float getPitch() {
      return ypr[1] * 180 / PI;
    }

    float getRoll() {
      return ypr[2] * 180 / PI;
    }
};

#endif
