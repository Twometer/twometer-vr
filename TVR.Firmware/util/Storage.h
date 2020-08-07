#ifndef TVR_STORAGE
#define TVR_STORAGE

#include <EEPROM.h>
#include <MPU9250.h>

#define EEPROM_SIZE  64
#define EEPROM_MAGIC 42

class Storage {
  public:
    Storage() {
      EEPROM.begin(EEPROM_SIZE);
    }

    bool hasData() {
      byte magic = 0;
      EEPROM.get(0, magic);
      return magic == EEPROM_MAGIC;
    }

    void storeCalibrationData(MPU9250* mpu) {
      Serial.println("Storing calibration data");
      int idx = 0;
      write_eeprom(idx, (byte)EEPROM_MAGIC);

      // Write accelerometer biases
      write_eeprom(idx, mpu->getAccBias(0));
      write_eeprom(idx, mpu->getAccBias(1));
      write_eeprom(idx, mpu->getAccBias(2));

      // Write gyroscope biases
      write_eeprom(idx, mpu->getGyroBias(0));
      write_eeprom(idx, mpu->getGyroBias(1));
      write_eeprom(idx, mpu->getGyroBias(2));

      // Write magnetometer biases
      write_eeprom(idx, mpu->getMagBias(0));
      write_eeprom(idx, mpu->getMagBias(1));
      write_eeprom(idx, mpu->getMagBias(2));

      // Write magnetometer scales
      write_eeprom(idx, mpu->getMagScale(0));
      write_eeprom(idx, mpu->getMagScale(1));
      write_eeprom(idx, mpu->getMagScale(2));

      EEPROM.commit();
    }

    void loadCalibrationData(MPU9250* mpu) {
      Serial.println("Loading calib data");
      int idx = 0;
      byte magic = read_eeprom<byte>(idx);
      if (magic != EEPROM_MAGIC)
        return;

      mpu->setAccBias(0, read_eeprom<float>(idx));
      mpu->setAccBias(1, read_eeprom<float>(idx));
      mpu->setAccBias(2, read_eeprom<float>(idx));

      mpu->setGyroBias(0, read_eeprom<float>(idx));
      mpu->setGyroBias(1, read_eeprom<float>(idx));
      mpu->setGyroBias(2, read_eeprom<float>(idx));

      mpu->setMagBias(0, read_eeprom<float>(idx));
      mpu->setMagBias(1, read_eeprom<float>(idx));
      mpu->setMagBias(2, read_eeprom<float>(idx));

      mpu->setMagScale(0, read_eeprom<float>(idx));
      mpu->setMagScale(1, read_eeprom<float>(idx));
      mpu->setMagScale(2, read_eeprom<float>(idx));

      mpu->printCalibration();
    }

    void clear() {
      for (int i = 0; i < EEPROM_SIZE; i++) {
        EEPROM.write(i, 0);
      }
      EEPROM.commit();
    }

  private:
    template<typename T>
    void write_eeprom(int& idx, T data) {
      EEPROM.put(idx, data);
      idx += sizeof(T);
    }

    template<typename T>
    T read_eeprom(int& idx) {
      T t;
      EEPROM.get(idx, t);
      idx += sizeof(T);
      return t;
    }
};

#endif TVR_STORAGE
