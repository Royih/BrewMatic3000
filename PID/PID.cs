using System;
using BrewMatic3000.Interfaces;

namespace BrewMatic3000.PID
{
    public delegate float GetFloat();
    public delegate void SetFloat(float value);

    //Source: http://www.codeproject.com/Articles/49548/Industrial-NET-PID-Controllers
    //PID tuning help: http://en.wikipedia.org/wiki/PID_controller
    public class PID : IPID
    {

        //Gains
        private readonly float Kp;
        private readonly float Ki;
        private readonly float Kd;


        //Max/Min Calculation
        private const float PvMax = 100;
        private const float PvMin = 0;
        private const float OutMax = 100;
        private const float OutMin = 0;

        //Running Values
        private DateTime _lastUpdate = DateTime.MinValue;
        private float _lastPv;
        private float _errSum;

        //Logging of values
        private LogValue[] _logValues;
        private TimeSpan _logInterval = new TimeSpan(0, 0, 15);
        private DateTime _nextLog = DateTime.MinValue;

        public PID(float kp, float ki, float kd)
        {
            _logValues = new LogValue[0];
            Kp = kp;
            Ki = ki;
            Kd = kd;
        }

        public float GetValue(float currentTemperature, float preferredTemperature)
        {
            var pv = currentTemperature;
            var sp = preferredTemperature;

            //We need to scale the pv to +/- 100%, but first clamp it
            pv = Clamp(pv, PvMin, PvMax);
            pv = ScaleValue(pv, PvMin, PvMax, 0, 1.0f); //ScaleValue(pv, PvMin, PvMax, -1.0f, 1.0f);

            //We also need to scale the setpoint
            sp = Clamp(sp, PvMin, PvMax);
            sp = ScaleValue(sp, PvMin, PvMax, 0, 1.0f); //ScaleValue(sp, PvMin, PvMax, -1.0f, 1.0f);

            //Now the error is in percent...
            float err = sp - pv;

            var pTerm = err * Kp;
            var iTerm = 0.0f;
            var dTerm = 0.0f;

            var partialSum = 0.0f;
            var nowTime = DateTime.Now;

            if (_lastUpdate != DateTime.MinValue)
            {
                var dT = nowTime - _lastUpdate;

                float dTSecs = dT.Seconds + (dT.Milliseconds / 1000f);

                //Compute the integral if we have to...
                if (pv >= PvMin && pv <= PvMax)
                {
                    partialSum = _errSum + (dTSecs * err);
                    iTerm = Ki * partialSum;
                }

                if (dTSecs != 0.0f)
                    dTerm = Kd * (pv - _lastPv) / dTSecs;
            }

            _lastUpdate = nowTime;
            _errSum = partialSum;
            _lastPv = pv;

            //Now we have to scale the output value to match the requested scale
            var outReal = pTerm + iTerm + dTerm;

            outReal = Clamp(outReal, 0, 1.0f); //Clamp(outReal, -1.0f, 1.0f);
            outReal = ScaleValue(outReal, 0, 1.0f, OutMin, OutMax); //ScaleValue(outReal, -1.0f, 1.0f, OutMin, OutMax);

            //This is to prevent further heating if the setpoint is reached. Even though this is probably due to improper tuning of the PID
            /*if (currentTemperature >= preferredTemperature)
            {
                outReal = 0;
            }*/

            //log this adjustment
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


            //Write it out to the world
            return outReal;
        }


        private float ScaleValue(float value, float valuemin,
           float valuemax, float scalemin, float scalemax)
        {
            float vPerc = (value - valuemin) / (valuemax - valuemin);
            float bigSpan = vPerc * (scalemax - scalemin);

            float retVal = scalemin + bigSpan;

            return retVal;
        }

        private float Clamp(float value, float min, float max)
        {
            if (value > max)
                return max;
            if (value < min)
                return min;
            return value;
        }



        public LogValue[] GetLogValues()
        {
            return _logValues;
        }
    }
}
