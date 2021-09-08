/*
#
# 77HCx4051 Analogic & Digital mux 8 channels. - Miguel A. Diaz de la Campa Ortega. 2021
# this library is distributed under the MIT license.
#
*/

#ifndef HCx4051AD_h
#define HCx4051AD_h
#include "Arduino.h"
#include <bitset>

    class Mux75HCx4051
    {
    public:
         
      enum EOperationMode: uint8_t { Multiplexor=1, Demultiplexor=2};
      enum EHCx4051Pins: uint8_t {Y0=0,Y1=1,Y2=2,Y3=3,Y4=4,Y5=5,Y6=6,Y7=7};
      enum EEstate:uint8_t {Enabled=1, Disabled=2 };

      /*
      * opMode operateMode sets the mox as multiplexor / demultiplexor
      * excitatorPin : also know as E tuns on or off the MOX, the 77HCx4051 is on when excitator is set to low.
      * ipPin is the output imput pin. 
      * s0, s1, s2 are the selection pins. Take into account this are DIGITAL pins.
      */
      Mux75HCx4051(Mux75HCx4051::EOperationMode opMode, uint8_t excitatorPin, uint8_t ioPin, uint8_t s0, uint8_t s1, uint8_t s2);

      uint16_t AnalogRead(Mux75HCx4051::EHCx4051Pins pin);
      int  DigitalRead(Mux75HCx4051::EHCx4051Pins pin);
      void DigitalWrite(Mux75HCx4051::EHCx4051Pins pin, u_short value);
      void AnalogicWrite(Mux75HCx4051::EHCx4051Pins pin, long value);
      void SwitchOperationMode(Mux75HCx4051::EOperationMode opMode);
      void Switch(Mux75HCx4051::EEstate state);
      Mux75HCx4051::EOperationMode GetOperationMode();
      int GetState(); 

    private:
       
      Mux75HCx4051::EOperationMode _operationMode;
      uint8_t _excitatorPin, _ioPin, _s0, _s1, _s2, _eState=0;
      void selectPin(Mux75HCx4051::EHCx4051Pins pin);
      void writeDigitalBit(uint8_t pin, uint8_t value);
    };

#endif