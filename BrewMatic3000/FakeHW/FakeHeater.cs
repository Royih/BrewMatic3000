using System;
using BrewMatic3000.Interfaces;

namespace BrewMatic3000.FakeHW
{
    public class FakeHeater : IHeatDevice
    {
        private float _currentPercentage = 10;
        private const float MaxWatts = 3500.0f;

        public FakeHeater()
        {
            _currentPercentage = 10;
        }

        public float GetCurrentWatts()
        {
            return ((MaxWatts * _currentPercentage) / 100);
        }

        public void SetValue(float percentage)
        {
            _currentPercentage = percentage;
        }

        public float GetCurrentValue()
        {
            return _currentPercentage;
        }


        public void Start()
        {
            
        }
    }
}
