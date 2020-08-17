#ifndef TVR_TIMER
#define TVR_TIMER

class Timer {
private:
  uint32_t lastTick = 0;
  uint32_t timeout = 0;

public:
  Timer(uint32_t tps) : timeout(1000000.0 / tps) {
  }

  bool elapsed() {
    return micros() - lastTick > timeout;
  }

  void reset() {
    lastTick = micros();
  }
};

#endif // TVR_TIMER
