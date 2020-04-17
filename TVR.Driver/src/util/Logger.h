//
// Created by twome on 07/04/2020.
//

#ifndef TVRDRV_LOGGER_H
#define TVRDRV_LOGGER_H

#include <string>
#include <utility>
#include <iostream>

namespace log {

    class Logger {
    private:
        std::string prefix;

        static Logger nullLog;

    public:

        explicit Logger(std::string prefix) : prefix(std::move(prefix)) {}

        template<typename obj>
        Logger &operator<<(const obj &val) {
            if (prefix.length() == 0)
                std::cout << val;
            else
                std::cout << "[" << prefix << "] " << val;
            return nullLog;
        }
    };


    static Logger info("INFO");
    static Logger err("ERROR");
    static Logger warn("WARN");
    Logger Logger::nullLog("");
    static std::string endl = "\n";

}


#endif //TVRDRV_LOGGER_H
