
using BrewMatic3000.Interfaces;
using Microsoft.SPOT;

namespace BrewMatic3000.RealHW
{
    /// <summary>
    /// Feautures implemented: 
    /// * F1: Return the avarage value based on the last N measurments
    /// * F2: Ignore measurments that differs from a specified threshold N number of times before it is accepted
    /// </summary>
    public class PT100Reader : ITempReader
    {
        private readonly SecretLabs.NETMF.Hardware.AnalogInput _analogInput;

        private const int ReistorOhms = 163;
        private const int MaximumValue = 1023;
        private const float AnalogReference = 3.3f;
        private const float MinimumCurrentValue = 0.004f;

        //F1: Variables used to return an average value based on the last N measurements
        private const int NumberOfMeasurementsToKeep = 5; //The number of measurments to keep. An average of these measurments will be returned
        private readonly float[] _measurements = new float[NumberOfMeasurementsToKeep]; //An array of the last measurements. 
        
        //F2: Variables used to ignore measurments out
        private float _lastMeasure; // Holds the last valid measurement
        private const float IgnoreThreshold = 0.4f; //The maximum allowed diff to be allowed compared to the last measurement
        private const int NumberOfTimesToIgnoreValuesOutsideThreshold = 3; //Measurments outside allowed threshold will be ignored this number of times before made valid
        private int _ignoredValuesOutsideThresholdCounter; //A counter to keep 

        public PT100Reader(SecretLabs.NETMF.Hardware.AnalogInput analogInput)
        {
            _analogInput = analogInput;
        }

        /// <summary>
        /// Will return a value between 0 and 100 (degreen celcius)
        /// 4mA = 0*C
        /// 20mA = 100*C
        /// 162 ohm resistors gives: 
        /// 0*C = 162 * 4e-3 = 0,648V
        /// 100*C = 162 * 20e-3 = 3.24V
        /// </summary>
        /// <returns></returns>
        private float ReadNewValue()
        {
            var myVoltage = (float)_analogInput.Read() / MaximumValue * AnalogReference;
            var myCurrent = (myVoltage / ReistorOhms);
            var myTemp = (myCurrent - MinimumCurrentValue) * 6250;
            return myTemp;
        }

        /// <summary>
        /// Take a new measurement. The value returned should be an average of the last n measurements
        /// </summary>
        /// <returns></returns>
        public float GetValue()
        {
            var newValue = ReadNewValue();

            if (_lastMeasure > 0)
            {
                var diff = newValue > _lastMeasure ? newValue - _lastMeasure : _lastMeasure - newValue;

                if (diff > IgnoreThreshold && _ignoredValuesOutsideThresholdCounter < NumberOfTimesToIgnoreValuesOutsideThreshold)
                {
                    _ignoredValuesOutsideThresholdCounter++;
                    Debug.Print("Ignored value \"" + newValue.ToString("f1") + "\" outside threshold. Diff = \"" + diff + "\". Ignore-Counter = " + _ignoredValuesOutsideThresholdCounter);
                    return _lastMeasure;
                }
                _ignoredValuesOutsideThresholdCounter = 0;

            }


            //First copy any value one place up the chain
            for (var i = _measurements.Length - 2; i >= 0; i--)
            {
                _measurements[i + 1] = _measurements[i];
            }
            //Place a new measurement at the beginning of the array
            _measurements[0] = newValue;

            _lastMeasure = CalculateAverageValue();
            return _lastMeasure;
        }

        private float CalculateAverageValue()
        {
            var valuesFound = 0;
            float sumOfValues = 0;
            foreach (var value in _measurements)
            {
                if (value > 0)
                {
                    valuesFound++;
                    sumOfValues += value;
                }
            }
            if (valuesFound > 0)
                return sumOfValues / valuesFound;
            return 0;
        }

        public void SetValue(float value)
        {
            throw new System.NotImplementedException("This is a real temp-reader. The value cannot be set. That would be cheating.");
        }
    }
}
