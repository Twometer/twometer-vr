#ifndef TVR_BUTTON
#define TVR_BUTTON

class Button {
private:
  int pin;

  int buttonState = HIGH;
  int lastButtonState = HIGH;

  unsigned long lastBounceTime = 0;  // the last time the pin was toggled
  unsigned long debounceDelay = 50;

public:
  Button(int pin) : pin(pin) {
  }

  bool isPressed() {
    int reading = digitalRead(pin);
    if (reading != lastButtonState) {
      // Reset the debouncing timer
      lastBounceTime = millis();
    }

    if ((millis() - lastBounceTime) > debounceDelay) {
      // whatever the reading is at, it's been there for longer than the debounce
      // delay, so take it as the actual current state:
      buttonState = reading;
    }

    lastButtonState = reading;
    return buttonState == LOW;
  }
};

#endif // TVR_BUTTON
