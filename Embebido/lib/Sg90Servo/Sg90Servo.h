/*
#
# SG90Servo - Miguel A. Diaz de la Campa Ortega. 2021
# this library is under the MIT license.
#
*/

#ifndef Sg90Servo_h
#define Sg90Servo_h
#include "Arduino.h"

    class Sg90Servo
    {
    public:
      Sg90Servo(int pin, int maxPulse);
      void MoveTo(int degrees);
      void MoveHome();

    private:
      int _servoPulseMax;
      int _pin;
      int map(long x, long in_min, long in_max, long out_min, long out_max);
    };

#endif