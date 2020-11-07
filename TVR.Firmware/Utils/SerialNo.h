#ifndef TVR_SERIAL_NO
#define TVR_SERIAL_NO

class SerialNo
{
public:
    static String get()
    {
        String serialNo = "TVR";
        serialNo.concat(String(ESP.getChipId(), HEX));
        serialNo.toUpperCase();
        return serialNo;
    }
};

#endif