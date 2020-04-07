// Describes a debounced button on an input pin

class Button {
  private:
    int pin;

    int buttonState;
    int lastButtonState = HIGH;
    unsigned long lastDebounceTime = 0;  // the last time the pin was toggled
    unsigned long debounceDelay = 50;

    bool pressed;
    unsigned long pressTime = -1;
  public:
    Button(int pin) {
      this->pin = pin;
    }

    bool isHeld() {
      return pressed && (millis() - pressTime) > 1500;
    }

    bool isPressed() {
      bool ok = false;

      int reading = digitalRead(pin);
      if (reading != lastButtonState) {
        // reset the debouncing timer
        lastDebounceTime = millis();
      }

      if ((millis() - lastDebounceTime) > debounceDelay) {
        // whatever the reading is at, it's been there for longer than the debounce
        // delay, so take it as the actual current state:

        // if the button state has changed:
        if (reading != buttonState) {
          buttonState = reading;

          // only ok if the new button state is LOW
          if (buttonState == LOW) {
            ok = true;
            pressed = true;
            pressTime = millis();
          } else {
            pressed = false;
            pressTime = -1;
          }
        }
      }
      lastButtonState = reading;

      return ok;
    }


};
