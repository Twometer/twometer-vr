#include <SparkFunMPU9250-DMP.h>

#define PIN_SDA 4
#define PIN_SCL 5

class MPUSensor {
  private:
    MPU9250_DMP imu;

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
          imu.computeEulerAngles();
          return true;
        }
      }
      return false;
    }

    float getYaw() {
      return imu.yaw;
    }

    float getRoll() {
      return imu.roll;
    }

    float getPitch() {
      return imu.pitch;
    }
};
