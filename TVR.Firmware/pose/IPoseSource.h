#ifndef TVR_IPOSESOURCE
#define TVR_IPOSESOURCE

/**
 * Base class for all pose sources
 */
class IPoseSource {
  public:
    virtual void begin() = 0;

    virtual void calibrateAccelGyro() = 0;

    virtual void calibrateMagnetometer() = 0;

    virtual void calculateOffsets() = 0;

    virtual bool update();

    virtual float getYaw() = 0;
    virtual float getPitch() = 0;
    virtual float getRoll() = 0;

    virtual bool requiresCalibration() = 0;
};

#endif // TVR_IPOSESOURCE
