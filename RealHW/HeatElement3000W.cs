
using System.Threading;
using BrewMatic3000.Interfaces;
using Microsoft.SPOT.Hardware;

namespace BrewMatic3000.RealHW
{
    public class HeatElement3000W : IHeatDevice
    {
        private readonly OutputPort _portHeater;

        private float _currentPercentage;

        public HeatElement3000W(OutputPort portHeater)
        {
            _portHeater = portHeater;
        }

        public float GetCurrentWatts()
        {
            return ((3000 * _currentPercentage) / 100);
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
            var worker = new Thread(
                AdjustHeat
                ) { Priority = ThreadPriority.Normal };
            worker.Start();
        }

        private void AdjustHeat()
        {
            while (true)
            {
                for (var i = 0; i < 100; i++)
                {
                    _portHeater.Write(i < _currentPercentage);
                    Thread.Sleep(1);
                }

            }
        }
    }
}
