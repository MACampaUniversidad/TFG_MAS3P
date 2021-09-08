/*
#
# Sg90digitalServo - Miguel A. Diaz de la Campa Ortega. 2021
# this library distributed under the MIT license.
#
*/

#ifndef Sg90digitalServo_h
#define Sg90digitalServo_h
#include "Arduino.h"

    class Sg90digitalServo
    {
    public:

      Sg90digitalServo(int pin);
      int CurrentAngle;
      int Pulse180=2200; //this is the 180 degrees pulse (~2ms in a 20ms pwm period)
      int Pulse0=500; //this is the 0 degrees pulse in ms. 
      int PulsePeriod=25000; //This is the whole interval range (usually for sg90 it's 20ms or 25ms,25ms seems to be smoother)
      int Step = 10; // 0.01 x100 ms step , 10 ms ~ 1 degree .
      void MoveTo(int degrees); //0 - 180 degrees.
      void StepRight();
      void StepLeft();

    private:
      int _pin;
      int _currentPulse;
      int map(long x, long in_min, long in_max, long out_min, long out_max);
      void writePulse();
      void updateAngle();
    };

#endif