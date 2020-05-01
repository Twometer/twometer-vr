#include <SparkFunMPU9250-DMP.h>

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
    }

    void update(MPU9250_DMP* imu, float yaw) {
      imu->updateCompass();
      imu->computeCompassHeading();

      if (!calibrated) {
        calibAccum += imu->heading;
        calibSamples++;
        return;
      }

      float absoluteHeading = imu->heading - headingOffset;

      float currentDrift = yaw - absoluteHeading;
      pushRingBuffer(currentDrift);

      if (ringBufferIndex == 0)
        drift = computeRingBufferAverage();

      this->correctedYaw = yaw - drift;
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
