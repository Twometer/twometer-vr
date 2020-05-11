#ifndef TVR_TIMER
#define TVR_TIMER

class Timer {
private:
  uint32_t lastTick = 0;

public:
  bool elapsed(int timeout) {
    return millis() - lastTick > timeout;
  }

  void reset() {
    lastTick = millis();
  }

};

#endif // TVR_TIMER
