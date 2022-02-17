#ifndef _ICM_20948_H
#define _ICM_20948_H

#include <Wire.h>

#ifndef I2C_ADDR
#define I2C_ADDR 0x69
#endif

#include "HAL.h"
#include "Invn/Devices/DeviceIcm20948.h"
#include "Invn/EmbUtils/DataConverter.h"
#include "Invn/EmbUtils/Message.h"

#define EXPECTED_WHOAMI 0xEA

#define ICM_SUCCESS      0  /* init successful */
#define ICM_BAD_WHOAMI   -1 /* device reported wrong whoami */
#define ICM_MAG_ERROR    -2 /* compass sensor not found */
#define ICM_SERIAL_ERROR -3 /* serial connection failed */
#define ICM_SETUP_ERROR  -4 /* failed to setup device */
#define ICM_DMP_ERROR    -5 /* failed to upload dmp fw */

static const uint8_t dmp3_image[] = {
#include "Invn/Images/icm20948_img.dmp3a.h"
};

enum fusion_mode_t {
    FUSION_6_AXIS,
    FUSION_9_AXIS
};

class ICM20948 {
   private:
    inv_device_icm20948_t device_icm20948;
    inv_device_t *device;

    inv_host_serif_t serif_instance{};
    inv_sensor_listener_t sensor_listener{};

    fusion_mode_t fusion_mode = FUSION_9_AXIS;

    volatile bool has_data = false;
    volatile float quat[4] = {1, 0, 0, 0};

    static void sensor_event_cb(const inv_sensor_event_t *event, void *context) {
        ICM20948 *icm = (ICM20948 *)context;

        if (event->status == INV_SENSOR_STATUS_DATA_UPDATED) {
            int sensor = -1;
            if (icm->fusion_mode == FUSION_6_AXIS) {
                sensor = INV_SENSOR_TYPE_GAME_ROTATION_VECTOR;
            } else {
                sensor = INV_SENSOR_TYPE_ROTATION_VECTOR;
            }

            if (INV_SENSOR_ID_TO_TYPE(event->sensor) == sensor) {
                icm->quat[0] = event->data.quaternion.quat[0];
                icm->quat[1] = event->data.quaternion.quat[1];
                icm->quat[2] = event->data.quaternion.quat[2];
                icm->quat[3] = event->data.quaternion.quat[3];
                icm->has_data = true;
            }
        }
    }

   public:
    void setFusionMode(fusion_mode_t mode) {
        fusion_mode = mode;
    }

    int begin() {
        serif_instance = {
            idd_io_hal_init_i2c,
            0,
            idd_io_hal_read_reg,
            idd_io_hal_write_reg,
            0,
            32, /* max read transact size */
            32, /* max write transact size */
            INV_HOST_SERIF_TYPE_I2C,
        };

        sensor_listener = {
            sensor_event_cb, /* callback func */
            this             /* context */
        };

        if (inv_host_serif_open(&serif_instance))
            return ICM_SERIAL_ERROR;

        inv_device_icm20948_init(&device_icm20948, &serif_instance, &sensor_listener, dmp3_image, sizeof(dmp3_image));
        device = inv_device_icm20948_get_base(&device_icm20948);

        delay(1000);

        uint8_t whoami = 0xff;
        inv_device_whoami(device, &whoami);
        if (whoami != EXPECTED_WHOAMI)
            return ICM_BAD_WHOAMI;

        if (inv_device_setup(device))
            return ICM_SETUP_ERROR;

        if (inv_device_load(device, NULL, dmp3_image, sizeof(dmp3_image), true /* verify */, NULL))
            return ICM_DMP_ERROR;

        if (fusion_mode == FUSION_9_AXIS && inv_device_ping_sensor(device, INV_SENSOR_TYPE_UNCAL_MAGNETOMETER))
            return ICM_MAG_ERROR;

        return ICM_SUCCESS;
    }

    void startSensor(int type, int period_us) {
        inv_device_set_sensor_period_us(device, type, period_us);
        inv_device_start_sensor(device, type);
    }

    void setHighPowerMode(bool highPower) {
        uint8_t data = highPower ? 1 : 0;
        inv_device_set_sensor_config(device, 0, INV_SENSOR_CONFIG_POWER_MODE, &data, 1);
    }

    void update() {
        inv_device_poll(device);
    }

    bool available() {
        return has_data;
    }

    void clearAvailable() {
        has_data = false;
    }

    float x() {
        return quat[0];
    }

    float y() {
        return quat[1];
    }

    float z() {
        return quat[2];
    }

    float w() {
        return quat[3];
    }
};

#endif