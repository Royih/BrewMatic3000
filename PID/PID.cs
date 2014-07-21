using System;
using BrewMatic3000.Interfaces;

namespace BrewMatic3000.PID
{
    public delegate float GetFloat();
    public delegate void SetFloat(float value);

    // Source: http://playground.arduino.cc/Main/BarebonesPIDForEspresso#pid
    // Credits and lots of gratitude to Tim Hirzel for sharing his code (December 2007). 
    // Tim Hirzels Arduino code was ported to .Net Microframework by Roy Ingar Hansen (July 2014)
    // PID tuning help: http://en.wikipedia.org/wiki/PID_controller
    public class PID : IPID
    {
        private const float WindupGuardGain = 1.0f;

        //Gains
        private readonly float _kp;
        private readonly float _ki;
        private readonly float _kd;


        private float _pTerm;
        private float _iTerm;
        private float _dTerm;
        private float _iState;
        private float _lastTemp;


        //Logging of values
        private LogValue[] _logValues;
        private TimeSpan _logInterval = new TimeSpan(0, 0, 15);
        private DateTime _nextLog = DateTime.MinValue;

        public PID(float kp, float ki, float kd)
        {
            _logValues = new LogValue[0];
            _kp = kp;
            _ki = ki;
            _kd = kd;
        }

        public float GetValue(float currentTemperature, float preferredTemperature)
        {
            var pv = currentTemperature;
            var sp = preferredTemperature;

            // these local variables can be factored out if memory is an issue, 
            // but they make it more readable
            double result;
            float error;
            float windupGaurd;

            // determine how badly we are doing
            error = sp - pv;

            // the pTerm is the view from now, the pgain judges 
            // how much we care about error we are this instant.
            _pTerm = _kp * error;

            // iState keeps changing over time; it's 
            // overall "performance" over time, or accumulated error
            _iState += error;

            // to prevent the iTerm getting huge despite lots of 
            //  error, we use a "windup guard" 
            // (this happens when the machine is first turned on and
            // it cant help be cold despite its best efforts)

            // not necessary, but this makes windup guard values 
            // relative to the current iGain
            windupGaurd = WindupGuardGain / _ki;

            if (_iState > windupGaurd)
                _iState = windupGaurd;
            else if (_iState < -windupGaurd)
                _iState = -windupGaurd;
            _iTerm = _ki * _iState;

            // the dTerm, the difference between the temperature now
            //  and our last reading, indicated the "speed," 
            // how quickly the temp is changing. (aka. Differential)
            _dTerm = (_kd * (pv - _lastTemp));

            // now that we've use lastTemp, put the current temp in
            // our pocket until for the next round
            _lastTemp = pv;

            // the magic feedback bit
            var outReal = _pTerm + _iTerm - _dTerm;
            if (outReal > 100)
                outReal = 100;
            if (outReal < 0)
                outReal = 0;

            LogValue(outReal, pv);

            //Write it out to the world
            return outReal;
        }

        public void LogValue(float outReal, float currentTemperature)
        {
            //Log this adjustment
            if (_nextLog == DateTime.MinValue || _nextLog < DateTime.Now)
            {
                var nuArray = new LogValue[_logValues.Length + 1];
                Array.Copy(_logValues, nuArray, _logValues.Length);
                _logValues = nuArray;
                _logValues[_logValues.Length - 1] = new LogValue()
                {
                    Effect = outReal,
                    Temperature = currentTemperature,
                    TimeStamp = DateTime.Now
                };
                _nextLog = DateTime.Now.Add(_logInterval);
            }

        }

        public LogValue[] GetLogValues()
        {
            return _logValues;
        }
    }
}
