/*
#
# Sg90digitalServo - Miguel A. Diaz de la Campa Ortega. 2021
# this library distributed under the MIT license.
#
*/


#include "Sg90DigitalServo.h"
#include "Arduino.h"

//######## public ###########

Sg90digitalServo::Sg90digitalServo(int pin ){
    
    _pin= pin;

    pinMode(_pin,OUTPUT);
     _currentPulse =  Pulse0; 
    
}

void Sg90digitalServo::MoveTo(int degrees){
 
 int possition= CurrentAngle=map( degrees,
                   0,  180,
                   Pulse0, Pulse180);

  if (possition < _currentPulse) {
    //the possition may not be 100% = to the current pulse because of the step size. 
    while (possition < _currentPulse)
          StepLeft();
    return;      
  } 
  if (possition > _currentPulse) {
   //the possition may not be 100% = to the current pulse because of the step size. 
    while (possition > _currentPulse)
          StepRight();
    return;      
  }
  
}

void Sg90digitalServo::StepRight(){
 
    int step = this->Step;
     _currentPulse += step;

    if ( _currentPulse >  Pulse180) 
      _currentPulse=Pulse180; //2.2 *1000 ->180 grados

    writePulse();
}

void Sg90digitalServo::StepLeft(){

    int step = this->Step * -1;    
     _currentPulse += step;

    if ( _currentPulse <  Pulse0) 
      _currentPulse=  Pulse0; //0.05 x1000 microsegundos -> 0 grados
 
   writePulse();
}

void Sg90digitalServo::writePulse(){
  
  updateAngle();

  digitalWrite( _pin, HIGH );
  delayMicroseconds( _currentPulse );

  digitalWrite( _pin, LOW );
  delayMicroseconds( PulsePeriod - _currentPulse );

}


//######## private ###########

int Sg90digitalServo::map(long x,long in_min,long in_max,long out_min,long out_max) {
     return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
}

void Sg90digitalServo::updateAngle(){

 CurrentAngle=map( _currentPulse,
                      Pulse0,  Pulse180,
                      0, 180);
}

