/*
#
# 77HCx4051 Analogic & Digital mux 8 channels. - Miguel A. Diaz de la Campa Ortega. 2021
# this library distributed under the MIT license.
#
*/
#include "74HCx4051AD.h"

//######## public ###########

 Mux75HCx4051::Mux75HCx4051(Mux75HCx4051::EOperationMode opMode, 
                                                    uint8_t excitatorPin, 
                                                    uint8_t ioPin, 
                                                    uint8_t s0,
                                                    uint8_t s1,
                                                    uint8_t s2){
    
    _excitatorPin= excitatorPin;
    _operationMode=opMode; 
    _ioPin= ioPin;
    _s0 = s0;
    _s1 = s1;
    _s2 = s2;
    SwitchOperationMode(opMode);
}

uint16_t Mux75HCx4051::AnalogRead( Mux75HCx4051::EHCx4051Pins  pin)
{
    selectPin(pin);
    return analogRead(_ioPin);
}

int Mux75HCx4051::DigitalRead( Mux75HCx4051::EHCx4051Pins  pin)
{
    selectPin(pin);
    return digitalRead(_ioPin);
}

void Mux75HCx4051::DigitalWrite( Mux75HCx4051::EHCx4051Pins  pin, u_short value)
{
    selectPin(pin);
}

void Mux75HCx4051::AnalogicWrite( Mux75HCx4051::EHCx4051Pins pin, long value)
{
    selectPin(pin);
}

void Mux75HCx4051::SwitchOperationMode(Mux75HCx4051::EOperationMode opMode)
{
    int mode;
    switch (opMode)
    {
        case Multiplexor:
           pinMode(_ioPin, INPUT_PULLUP); //avoid floating pins
            break;
        case Demultiplexor:
             pinMode(_ioPin, OUTPUT);
            break;
        default:
            break;
    }
    //Control pins.
    pinMode(_s0 , OUTPUT);
    pinMode(_s1 , OUTPUT);
    pinMode(_s2 , OUTPUT);
    pinMode(_excitatorPin, OUTPUT);
}

Mux75HCx4051::EOperationMode Mux75HCx4051::GetOperationMode()
{
    return _operationMode;
}

void Mux75HCx4051::Switch(Mux75HCx4051::EEstate state)
{
    switch (state)
    {
        case  Enabled:
            digitalWrite(_excitatorPin, LOW);
            _eState=LOW;
            break;
        case  Disabled:
            digitalWrite(_excitatorPin, HIGH);
            _eState=HIGH;
            break;
        default:
            break;
    }
}

int Mux75HCx4051::GetState()
{
    return _eState;
}

//######## private ###########

void  Mux75HCx4051::selectPin(Mux75HCx4051::EHCx4051Pins pin)
{
   std::bitset<3> bitField = (uint8_t) pin;

        /*digitalWrite(_s0, bitField[0]);
        digitalWrite(_s1, bitField[1]);
        digitalWrite(_s2, bitField[2]);*/
        //
        writeDigitalBit(_s0, bitField[2]);
        writeDigitalBit(_s1, bitField[1]);
        writeDigitalBit(_s2, bitField[0]);
          Serial.print( "pin:" );
          Serial.print( pin );
          Serial.print( "->" );
          Serial.print( bitField[2] );
          Serial.print( bitField[1] );
          Serial.print( bitField[0] );
          Serial.println("");   
}
void Mux75HCx4051::writeDigitalBit(uint8_t pin, uint8_t value)
{
 if (value <1) {  
     digitalWrite(pin,LOW);
     } 
     else 
     {
        digitalWrite(pin,HIGH);
     }

}
