#include <SparkFunMPU9250-DMP.h>

// TODO Compass gives wrong values for some reason

class DriftCorrection {
  private:
    float drift = 0;
    float correctedYaw = 0;
    float headingOffset = 0;

    int ringBufferSize = 15;
    int ringBufferIndex = 0;
    float *ringBuffer;

    bool calibrated = false;
    float calibAccum = 0;
    int calibSamples = 0;

  public:
    DriftCorrection() : ringBuffer(new float[ringBufferSize]) {

    }

    void finishCalibration() {
      headingOffset = calibAccum / float(calibSamples);
      calibrated = true;

      Serial.print("Compass calibration finished with offset=");
      Serial.println(headingOffset);
      Serial.println("mx, my, mz");
    }

    void update(MPU9250_DMP* imu, float yaw) {
      imu->updateCompass();
      imu->computeCompassHeading();

      float head = atan2(imu->my, imu->mx) * 180 / PI;

      if (!calibrated) {
        calibAccum += head;
        calibSamples++;
        return;
      }

      float absoluteHeading = head - headingOffset;
      pushRingBuffer(absoluteHeading);

      //Serial.println(absoluteHeading);

      /*Serial.print(imu->mx);
        Serial.print(", ");
        Serial.print(imu->my);
        Serial.print(", ");
        Serial.print(imu->mz);
        Serial.print(", ");
        Serial.println(head);*/
      float currentDrift = yaw - absoluteHeading;
      if (ringBufferIndex == 0)
        drift = computeRingBufferAverage();

      this->correctedYaw = yaw;
    }

    float getCorrectedYaw() {
      return correctedYaw;
    }

  private:
    void pushRingBuffer(float value) {
      ringBuffer[ringBufferIndex] = value;
      ringBufferIndex ++;
      if (ringBufferIndex >= ringBufferSize)
        ringBufferIndex = 0;
    }

    float computeRingBufferAverage() {
      float val = 0;
      for (int i = 0; i < ringBufferSize; i++)
        val += ringBuffer[i];
      val /= ringBufferSize;
      return val;
    }
};
