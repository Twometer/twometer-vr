#ifndef TVR_SERIAL_NO
#define TVR_SERIAL_NO

#include "Constants.h"

class SerialNo {
   public:
    static String get() {
        String serialNo = MANUFACTURER_ID;
        serialNo.concat(String(ESP.getChipId(), HEX));
        serialNo.toUpperCase();
        return serialNo;
    }
};

#endif