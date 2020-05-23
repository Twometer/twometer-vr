#ifndef TVR_TIMER
#define TVR_TIMER

class Timer {
private:
  uint32_t lastTick = 0;
  uint32_t timeout = 0;

public:
  Timer(uint32_t tps) : timeout(1000.0 / tps) {
  }

  bool elapsed() {
    return millis() - lastTick > timeout;
  }

  void reset() {
    lastTick = millis();
  }
};

#endif // TVR_TIMER
