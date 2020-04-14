#ifndef PROFILER_H
#define PROFILER_H

int profiler_begin_time = 0;

void profiler_begin() {
  profiler_begin_time = millis();
}

void profiler_end(String desc) {
  int duration = millis() - profiler_begin_time;
  profiler_begin_time = 0;
  Serial.print("Task '");
  Serial.print(desc);
  Serial.print("' took ");
  Serial.print(duration);
  Serial.println(" ms.");
}

#endif
