#include <math.h>
//#include <time.h>

#define MAX 255 //The number of baseline measurements
#define SENSITIVITY 100 //The number of running measurements
#define THRESHOLD 10 //The cutoff for number of std's from the mean to record a measurement


enum BeamState {idle, crossed};
BeamState _beamState = idle;

int _inputPin = 3; // Input pin for the photoresistor
int _tarePin = 4;  // Input pin for the Tare button

bool _baseLineTaken = false;

float _baseStd = 0;
float _baseAvg = 0;

float _runningStd = 0;
float _runningAvg = 0;


time_t _startTime; //Start time for the lap
time_t _endTime; //End time for the lap
time_t _lapTime;


void setup() {
  pinMode(_inputPin, INPUT);
  pinMode(_tarePin, INPUT);
}

void Tare() {
  //Calculate a new baseline Std and Avg
  float measurements[MAX];
  float sum = 0;
  float std = 0;
  float avg = 0;
  float squareSumDiff = 0;
  for (int i = 0; i < MAX; ++i) {
    measurements[i] = digitalRead(_inputPin);
    sum += measurements[i];
  }
  avg = sum / MAX;
  _baseAvg = avg;

  for (int i = 0; i < MAX; ++i) {
    squareSumDiff += pow(measurements[i] - avg, 2);
  }
  std = sqrt(squareSumDiff / (MAX - 1));
  _baseStd = std;
  
  _baseLineTaken = true;
}

// Take some running measurements, calculate the avg and std  
// Number of measurements = SENSITIVITY
void Measure() {
  float measurements[SENSITIVITY];
  float std = 0;
  float avg = 0;
  float sum = 0;
  float squareSumDiff = 0;
  for (int i = 0 ; i < SENSITIVITY; ++i) {
    measurements[i] = digitalRead(_inputPin);
    sum += measurements[i];
  }
  avg = sum / SENSITIVITY;
  _runningAvg = avg;

  for (int i = 0; i < SENSITIVITY; ++i) {
    squareSumDiff += pow(measurements[i] - avg, 2);
  }
  std = sqrt(squareSumDiff / (SENSITIVITY - 1));
  
}

// Compare the running measurements to the baseline measurements
// Record a laptime if the detection passes a validation test. 
// is a valid one
void Compare() {
  if (_runningStd >= THRESHOLD*_baseStd && _beamState != crossed) {
    _beamState = crossed; 

    _endTime = now();
    _lapTime = _endTime - _startTime;
    int j = 0;
    char buff[32];
    j = sprintf(buff, "The lap time is: %d.%d.%d", minute(_lapTime), seconds(_lapTime), milli(_lapTime)); 
    
  }
  else if (_runningStd < THRESHOLD & _baseStd && _beamState != idle) {
    _beamState = idle;
    _startTime = now();
  }
  else {
    //The object is in the way. Do nothing. 
  }
}



// Determine if the lap time is worth keeping
bool LapValid () {
 
}


void loop() {
  // put your main code here, to run repeatedly:
  if (!_baseLineTaken || digitalRead(_tarePin)) {
    Tare();    
  }
  Measure();
  Compare();
}
