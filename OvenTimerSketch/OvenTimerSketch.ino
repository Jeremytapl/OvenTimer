#include "Countimer.h"
#include <LiquidCrystal.h>

//LCD pin to Arduino
const int pin_RS = 8;
const int pin_EN = 9;
const int pin_d4 = 4;
const int pin_d5 = 5;
const int pin_d6 = 6;
const int pin_d7 = 7;
const int pin_BL = 10;

Countimer timer;
LiquidCrystal lcd( pin_RS,  pin_EN,  pin_d4,  pin_d5,  pin_d6,  pin_d7);

void setup()
{
  Serial.begin(9600);
  lcd.begin(16, 2);

  initLCD();
  initTimer(10, 1000);
}

void loop()
{
  // Run timer
  timer.run();

  listenForButtons();
  listenForCommands();    
}

void initLCD() {
  printLCD("Press a button", true);
}

void initTimer(int durationSeconds, int intervalMilliSeconds) {
 // onComplete is called when the timer duration runs out
 timer.setCounter(0, 0, durationSeconds, timer.COUNT_DOWN, onComplete);

 // refreshClock is called everytime the duration of intervalMilliSeconds has passed (e.g. every 1000ms)
 timer.setInterval(refreshClock, intervalMilliSeconds);
}

void listenForButtons() {
  int sensorValue = analogRead(0);
  
  if(sensorValue > 60 && sensorValue < 800) {
    timer.start();
    printLCD("Transmitting...", true);
    
    Serial.println(sensorValue, DEC);
    delay(100);
  }
}

void listenForCommands() {  
  if(Serial.available() > 0)  
  {          
    char receiveVal = Serial.read(); 
    
    if(receiveVal == '1') {   
      // Stop timer indicating a successful message was received
      timer.stop();
      printLCD("Transmitted.", true);
      
      delay(2000);      
      initLCD();   
    }      
  } 
}

void onComplete() {
  // If we get this far it means we didn't get a response from our app in time
  printLCD("Timed out.", true);

  delay(2000);      
  initLCD(); 
}

void printLCD(String text, bool clear) {
  if(clear) {
    lcd.clear();
  }
  
  lcd.print(text);
}

void refreshClock() {  
  if(timer.isCounterCompleted()) {    
    Serial.println(timer.isCounterCompleted());
    lcd.clear();
  }
}
