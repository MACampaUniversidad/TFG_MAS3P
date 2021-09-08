#include <Wire.h>  
#include <Adafruit_Sensor.h>
#include <Adafruit_BME280.h>
#include "cubecell_SH1107Wire.h"
#include "Arduino.h"
#include <Sg90digitalServo.h> //C:\Users\mdiaz\OneDrive\Documentos\Arduino\libraries\Sg90Servo
#include <74HCx4051AD.h> //C:\Users\mdiaz\OneDrive\Documentos\Arduino\libraries\71HCx4051Multiplexor

//IO PIN DEFINITIONS
uint8_t MuxExcitator=GPIO8;
uint8_t MuxIOPin = ADC3;


Adafruit_BME280 bme; // I2C (sda/scl) Barométrico. 
SH1107Wire  display(0x3c, 500000, I2C_NUM_0,GEOMETRY_128_64,GPIO10); // addr , freq , i2c group , resolution , rst

Sg90digitalServo servoTorre(PWM1);
Sg90digitalServo servoBase(PWM2);

Mux75HCx4051 MUX(Mux75HCx4051::Multiplexor, MuxExcitator , MuxIOPin , GPIO12, GPIO13, GPIO14);//E,IO, s0,s1,s2

//verificacion calibrado de servos
const bool ENABLE_SERVO_TEST = false;
const bool ENABLE_MUX_TEST = true;

int AsixX=0;
int AsixY=0;

void VextON(void)
{
  pinMode(Vext,OUTPUT);
  digitalWrite(Vext, LOW);
}

void VextOFF(void) //Vext default OFF
{
  pinMode(Vext,OUTPUT);
  digitalWrite(Vext, HIGH);
}

void setup() {

   VextON();
  
  Serial.begin(9600); 
  display.init();
  display.clear();
  display.display();
  display.setContrast(255);
  display.clear();
  display.setLogBuffer(15, 60);
  display.setFont(ArialMT_Plain_10);
  display.setTextAlignment(TEXT_ALIGN_CENTER);

  bool communication = bme.begin(0x76);
   
  if (!communication) {
    
    display.println("No encuentro el sensor BME280");;
    display.print("SensorID was: 0x");
    display.println(bme.sensorID(), 16);
    display.println("ID of 0xFF probably means a bad address\n");
    display.drawLogBuffer(0, 0);
    display.display();
    delay(10000);
    display.clear();
    
  } else {
    
    display.println("Inicializando..\n");
    display.drawLogBuffer(0, 0);
    display.display();
    delay(100);
    display.clear();
  }
  if (ENABLE_MUX_TEST) {
    inicializarMux();
  }
  pinMode(GPIO5, OUTPUT);
  
}

void inicializarMux()
{
   MUX.Switch(Mux75HCx4051::Enabled);
}
 
void loop() {

  if (ENABLE_SERVO_TEST) cicloServo();
  if (ENABLE_MUX_TEST) {
    testMUX();
  } else {
    display.clear();
    display.setLogBuffer(15, 60);
    //display.print("ID Sensor = ");
    //display.println(bme.sensorID(), 16);
    display.print("x");display.print(AsixX);display.print("y");display.print(AsixY);
    display.println();
    display.print("Temperatura = ");
    display.print(bme.readTemperature());
    display.println(" *C");
   /* display.print("Presion = ");
    display.print(bme.readPressure() / 100.0F);
    display.println(" hPa");*/

   display.println();
 

  //display.print("Humedad = ");
  //display.print(bme.readHumidity());
  //display.println(" %\n");
  //display.print("Suelo = ");
  //display.print(analogRead(ADC3));
  display.drawLogBuffer(0, 0);
  display.display();
  display.clear(); 
 

  //buscarLuz();
  }
}
void testMUX()
{
      MUX.Switch(Mux75HCx4051::Enabled);
     //analogic reads for the 8 mux channels
     uint16_t analogReads[] =  {
        MUX.AnalogRead(Mux75HCx4051::Y0),
        MUX.AnalogRead(Mux75HCx4051::Y1),
        MUX.AnalogRead(Mux75HCx4051::Y2),
        MUX.AnalogRead(Mux75HCx4051::Y3),
        MUX.AnalogRead(Mux75HCx4051::Y4),
        MUX.AnalogRead(Mux75HCx4051::Y5),
        MUX.AnalogRead(Mux75HCx4051::Y6),
        MUX.AnalogRead(Mux75HCx4051::Y7)
      };
     
    display.clear();
    display.setLogBuffer(15, 60);
    display.println("mux test");  
    display.print("status [e=1//d=2] = "); display.println(MUX.GetState());
    display.print("mode [m=1/dM=2] = "); display.println(MUX.GetOperationMode());
    //read the 8 channels
    for (int i=0;i<4;i++){
      //try refactoring this by using std::format (check size increase in case of linking std if not linked yet)
        display.print("Y"); display.print(i);display.print("=");display.print(analogReads[i]);
    }
     display.println();  
     for (int i=4;i<=7;i++){
      //try refactoring this by using std::format (check size increase in case of linking std if not linked yet)
        display.print("Y"); display.print(i);display.print("=");display.print(analogReads[i]);
    }                
     display.println();           
    
  display.drawLogBuffer(0, 0);
  display.display();
 
  display.clear();  
}

void buscarLuz()
{
  bool left,right,up,down;
  
   /* left = digitalRead(ldrIzdo)==1;
    right = digitalRead(ldrDrcho)==1;
    up = digitalRead(ldrSuperior)==1;
    down = digitalRead(ldrInferior)==1;

      if (left && !right) {}
        if (!left && right) {}

        if(up && !down) {
          if (AsixY>0) {
            AsixY--;
        //    moverServoPanel(AsixY);
            }
        }
        if(!up && down){    
          if (AsixY<180) {
            AsixY++;
           // moverServoPanel(AsixY);
          }
        }
        if(!up && !down){    
          if (AsixY=0) {
          //  moverServoPanel(AsixY);
          }
        }
        */
}
void cicloServo()
{
  
      servoBase.MoveTo(0);
      servoTorre.MoveTo(0);
    
 delay(1500);
  servoBase.MoveTo(45);
 servoTorre.MoveTo(45);

    
 
 delay(1500);
  servoBase.MoveTo(90);
 servoTorre.MoveTo(90);

 
 delay(1500);
  servoBase.MoveTo(135);
 servoTorre.MoveTo(135);

 
 delay(1500);
  servoBase.MoveTo(180);
 servoTorre.MoveTo(180);

 delay(1500); 
 /*
 for (int n=0;n<=180; n++) 
 {
  servoBase.StepLeft(); //cada ciclo tiene 20.000 ms con lo que 200 serían 180 
  servoTorre.StepLeft();
    display.print("current angles : Base: ");display.print(servoBase.CurrentAngle);
    display.println("Torre: ");display.print(servoTorre.CurrentAngle);
    display.println("");
    display.drawLogBuffer(0, 0);
    display.display();
    display.clear(); 
 

 }
 for (int n=0;n<=180; n++) 
 {
  servoBase.StepRight();
  servoTorre.StepRight();
    display.print("current angles : Base: ");display.print(servoBase.CurrentAngle);
    display.println("Torre: ");display.print(servoTorre.CurrentAngle);
    display.println("");
    display.drawLogBuffer(0, 0);
    display.display();
    display.clear(); 
 }
*/

}

 
