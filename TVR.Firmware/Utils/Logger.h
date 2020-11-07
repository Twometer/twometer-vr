#ifndef TVR_LOGGER
#define TVR_LOGGER

class Logger
{
private:
    static void write(String prefix, String message)
    {
        Serial.print("[");
        Serial.print(prefix);
        Serial.print("] ");
        Serial.println(message);
    }

public:
    static void info(String message)
    {
        write("INFO", message);
    }

    static void warn(String message)
    {
        write("WARN", message);
    }

    static void error(String message)
    {
        write("ERROR", message);
    }

    static void crash(String message) 
    {
        error("Failed to initialize UDP client");
        delay(5000);
        ESP.restart();
    }
};

#endif