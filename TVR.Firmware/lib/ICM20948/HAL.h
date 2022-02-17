#ifndef _ICM_HAL_H
#define _ICM_HAL_H

#include <Wire.h>

static int idd_io_hal_init_i2c(void) {
    Wire.setClock(400000);
    return 0;
}

static int idd_io_hal_read_reg(uint8_t reg, uint8_t *rbuffer, uint32_t rlen) {
    Wire.beginTransmission(I2C_ADDR);
    Wire.write(reg);
    Wire.endTransmission(false);
    Wire.requestFrom(I2C_ADDR, rlen);
    for (unsigned char i = 0; i < rlen; i++) {
        rbuffer[i] = Wire.read();
    }
    return 0;
}

static int idd_io_hal_write_reg(uint8_t reg, const uint8_t *wbuffer, uint32_t wlen) {
    Wire.beginTransmission(I2C_ADDR);
    Wire.write(reg);
    for (unsigned char i = 0; i < wlen; i++) {
        Wire.write(wbuffer[i]);
    }
    Wire.endTransmission(true);
    return 0;
}

extern "C" {
void inv_icm20948_sleep_us(int us) {
    delayMicroseconds(us);
}

uint64_t inv_icm20948_get_time_us(void) {
    return micros();
}
}

#endif