using System.Threading;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace BrewMatic3000
{
    public class LedBlinker
    {
        private bool _currentStatus;

        private readonly OutputPort _led = new OutputPort(Pins.ONBOARD_LED, false);

        public LedBlinker()
        {
            _currentStatus = false;
        }

        public void Start()
        {
            var worker = new Thread(
                ToggleStatus
                ) { Priority = ThreadPriority.Normal };
            worker.Start();
        }

        private void ToggleStatus()
        {
            while (true)
            {
                _currentStatus = !_currentStatus;
                _led.Write(_currentStatus);
                Thread.Sleep(1000);
            }
        }
    }
}
