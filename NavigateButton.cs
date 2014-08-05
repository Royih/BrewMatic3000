using System;
using System.Threading;
using Microsoft.SPOT;
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

        private bool _triggerKeyPressShort = false;

        private bool _triggerKeyPressLongCancelled = false;

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
            Debug.Print("Button pressed. Data2: " + data2);
            if (data2 > 0 && _navButtonState == NavButtonStates.Idle)
            {
                _timeButtonPressed = DateTime.Now;
                _navButtonState = NavButtonStates.Pressed;
            }
            else if (data2 == 0 && _navButtonState == NavButtonStates.Warned)
            {
                _triggerKeyPressLongCancelled = true;
                _navButtonState = NavButtonStates.Idle;
            }
            else if (data2 == 0 && _navButtonState == NavButtonStates.Pressed)
            {
                _navButtonState = NavButtonStates.Idle;
                _triggerKeyPressShort = true;
            }
        }

        private void DoWork()
        {
            while (true)
            {
                if (_triggerKeyPressLongCancelled)
                {
                    KeyPressLongCancelled();
                    Debug.Print("Navigate Button: Keypress Long Cancelled event");
                }
                else if (_triggerKeyPressShort)
                {
                    KeyPressShort();
                    Debug.Print("Navigate Button: Keypress Short event");
                }
                else if (_navButtonState != NavButtonStates.Idle)
                {
                    var tsPressed = DateTime.Now.Subtract(_timeButtonPressed);
                    var buttonWasPressedMillis = tsPressed.Milliseconds + (1000 * tsPressed.Seconds) + (1000 * 60 * tsPressed.Minutes);
                    if (_navButtonState == NavButtonStates.Pressed && buttonWasPressedMillis > TimeoutDisplayWarning)
                    {
                        if (KeyPressLongWarning != null)
                        {
                            _navButtonState = NavButtonStates.Warned;
                            KeyPressLongWarning();
                            Debug.Print("Navigate Button: Keypress Long Warning event");
                        }
                    }
                    else if (_navButtonState == NavButtonStates.Warned && buttonWasPressedMillis > (TimeoutDisplayWarning + TimeoutConfirmed))
                    {
                        if (KeyPressLong != null)
                        {
                            _navButtonState = NavButtonStates.Idle;
                            KeyPressLong();
                            Debug.Print("Navigate Button: Keypress Long event");
                        }
                    }
                }

                _triggerKeyPressLongCancelled = false;
                _triggerKeyPressShort = false;

                Thread.Sleep(500);


            }
        }


    }


}
