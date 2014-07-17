using System;
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace BrewMatic3000
{
    public delegate void MyEventHandler();

    public class NavigateButton
    {
        private enum NavButtonStates
        {
            Idle,
            Pressed,
            Warned
        }

        private NavButtonStates _navButtonState = NavButtonStates.Idle;

        private readonly InterruptPort _navButton;

        private const int TimeoutDisplayWarning = 1000;

        private const int TimeoutConfirmed = 1000;

        private DateTime _timeButtonPressed = DateTime.MinValue;


        public event MyEventHandler KeyPressShort;

        public event MyEventHandler KeyPressLongWarning;

        public event MyEventHandler KeyPressLongCancelled;

        public event MyEventHandler KeyPressLong;

        public NavigateButton(InterruptPort navButton)
        {
            _navButton = navButton;
            _navButton.OnInterrupt += navButton_OnInterrupt;
            var worker = new Thread(
            DoWork
            );
            worker.Start();
        }

        private void navButton_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            if (data2 > 0 && _navButtonState == NavButtonStates.Idle)
            {
                _timeButtonPressed = DateTime.Now;
                _navButtonState = NavButtonStates.Pressed;
            }
            else if (data2 == 0 && _navButtonState == NavButtonStates.Warned)
            {
                KeyPressLongCancelled();
                _navButtonState = NavButtonStates.Idle;
            }
            else if (data2 == 0 && _navButtonState == NavButtonStates.Pressed)
            {
                KeyPressShort();
                _navButtonState = NavButtonStates.Idle;
            }
        }

        private void DoWork()
        {
            while (true)
            {
                if (_navButtonState != NavButtonStates.Idle)
                {
                    var tsPressed = DateTime.Now.Subtract(_timeButtonPressed);
                    var buttonWasPressedMillis = tsPressed.Milliseconds + (1000 * tsPressed.Seconds) + (1000 * 60 * tsPressed.Minutes);
                    if (_navButtonState == NavButtonStates.Pressed && buttonWasPressedMillis > TimeoutDisplayWarning)
                    {
                        _navButtonState = NavButtonStates.Warned;
                        KeyPressLongWarning();
                    }
                    else if (_navButtonState == NavButtonStates.Warned && buttonWasPressedMillis > (TimeoutDisplayWarning + TimeoutConfirmed))
                    {
                        _navButtonState = NavButtonStates.Idle;
                        KeyPressLong();
                    }
                }
                Thread.Sleep(200);
            }
        }


    }


}
