#include <SparkFunMPU9250-DMP.h>

class DriftCorrection {
  private:
    float offset;
    float correctedYaw;

    float *ringBuffer;
    int ringBufferSize;
    int ringBufferIndex;
  public:
    DriftCorrection() : ringBufferSize(15), ringBufferIndex(0), ringBuffer(new float[ringBufferSize]) {

    }

    void update(MPU9250_DMP* imu, float yaw) {
      imu->updateCompass();
      imu->computeCompassHeading();

      float absoluteHeading = imu->heading;
      // TODO remove heading offset

      float drift = absoluteHeading - yaw;
      pushRingBuffer(drift);

      if (ringBufferIndex == 0)
        offset = computeRingBufferAverage();

      this->correctedYaw = yaw - offset;
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
      for(int i = 0; i < ringBufferSize; i++)
        val += ringBuffer[i];
      val /= ringBufferSize;
      return val;
    }
};
