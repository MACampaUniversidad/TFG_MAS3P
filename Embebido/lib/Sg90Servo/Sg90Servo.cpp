/*
#
# SG90Servo - Miguel A. Diaz de la Campa Ortega. 2021
# this library is under the MIT license.
#
*/

#include "Sg90Servo.h"
#include "Arduino.h"

//######## public ###########

Sg90Servo::Sg90Servo(int pin, int maxPulse){
    
    _servoPulseMax=maxPulse;
    _pin= pin;
    //establecer ciclo y pinMode(pin, mode);
        /*
        *PWM CLK frequency default is 12M;
        *PWM CLK frequency value:
        *PWM_CLK_FREQ_48M
        *PWM_CLK_FREQ_24M
        *PWM_CLK_FREQ_16M
        *PWM_CLK_FREQ_12M
        *PWM_CLK_FREQ_8M
        *PWM_CLK_FREQ_6M
        *PWM_CLK_FREQ_4M
        *PWM_CLK_FREQ_3M
        *PWM_CLK_FREQ_2M
        *PWM_CLK_FREQ_1M
    */
    setPWM_Frequency(PWM_CLK_FREQ_1M);
    setPWM_ComparePeriod(0x4E20); 
    pinMode(_pin,OUTPUT);
}

void Sg90Servo::MoveTo(int degrees){
  analogWrite(_pin,map(degrees,0,180,0,_servoPulseMax));
 
}
//Macro to 0 degrees.
void Sg90Servo::MoveHome(){
  MoveTo(0);
}

//######## private ###########

int Sg90Servo::map(long x,long in_min,long in_max,long out_min,long out_max) {
     return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
}

