/*
  Based on:

  MPU6050 Basic Example with IMU
  by: Kris Winer
  date: May 10, 2014
  license: Beerware - Use this code however you'd like. If you
  find it useful you can buy me a beer some time.

  Modified by Twometer to work as a library with the ESP8266
*/

#include "MPU6050.h"


bool MPU6050::begin() {
  Wire.begin(4, 5);

  uint8_t c = readByte(MPU6050_ADDRESS, WHO_AM_I_MPU6050);  // Read WHO_AM_I register for MPU-6050
  delay(1000);

  if (c == 0x68)
  {
    MPU6050SelfTest(SelfTest);
    if (SelfTest[0] < 1.0f && SelfTest[1] < 1.0f && SelfTest[2] < 1.0f && SelfTest[3] < 1.0f && SelfTest[4] < 1.0f && SelfTest[5] < 1.0f) {
      delay(1000);
      calibrateMPU6050(gyroBias, accelBias);
      delay(1000);
      initMPU6050();
      return true;
    }
  }
  return false;
}

void MPU6050::update() {
  if (readByte(MPU6050_ADDRESS, INT_STATUS) & 0x01) { // check if data ready interrupt
    readAccelData(accelCount);  // Read the x/y/z adc values
    getAres();

    // Now we'll calculate the accleration value into actual g's
    ax = (float)accelCount[0] * aRes; // get actual g value, this depends on scale being set
    ay = (float)accelCount[1] * aRes;
    az = (float)accelCount[2] * aRes;

    readGyroData(gyroCount);  // Read the x/y/z adc values
    getGres();

    // Calculate the gyro value into actual degrees per second
    gx = (float)gyroCount[0] * gRes; // get actual gyro value, this depends on scale being set
    gy = (float)gyroCount[1] * gRes;
    gz = (float)gyroCount[2] * gRes;

    tempCount = readTempData();  // Read the x/y/z adc values
    temperature = ((float) tempCount) / 340. + 36.53; // Temperature in degrees Centigrade
  }

  Now = micros();
  deltat = ((Now - lastUpdate) / 1000000.0f); // set integration time by time elapsed since last filter update
  lastUpdate = Now;
  //    if(lastUpdate - firstUpdate > 10000000uL) {
  //      beta = 0.041; // decrease filter gain after stabilized
  //      zeta = 0.015; // increase gyro bias drift gain after stabilized
  //    }
  // Pass gyro rate as rad/s
  MadgwickQuaternionUpdate(ax, ay, az, gx * PI / 180.0f, gy * PI / 180.0f, gz * PI / 180.0f);
  _yaw   = atan2(2.0f * (q[1] * q[2] + q[0] * q[3]), q[0] * q[0] + q[1] * q[1] - q[2] * q[2] - q[3] * q[3]);
  _pitch = -asin(2.0f * (q[1] * q[3] - q[0] * q[2]));
  _roll  = atan2(2.0f * (q[0] * q[1] + q[2] * q[3]), q[0] * q[0] - q[1] * q[1] - q[2] * q[2] + q[3] * q[3]);
  _pitch *= 180.0f / PI;
  _yaw   *= 180.0f / PI;
  _roll  *= 180.0f / PI;

  count = millis();
}
