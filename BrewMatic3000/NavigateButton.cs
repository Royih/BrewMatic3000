using System;
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace BrewMatic3000
{
    public enum NavButtonStates
    {
        Idle,
        Pressed,
        Warned,
        PressedLong
    }

    public class NavigateButton
    {

        private NavButtonStates _prevButtonState = NavButtonStates.Idle;
        private DateTime _buttonTimeout = DateTime.MinValue;

        private const int TimeoutDisplayWarning = 500;

        private const int TimeoutConfirmed = 500;

        private readonly InputPort _navButton;


        public NavigateButton(InputPort navButton)
        {
            _navButton = navButton;
        }

        public bool EventShort { get; private set; }

        public bool EventLongWarning { get; private set; }

        public bool EventLongCancelled { get; private set; }

        public bool EventLong { get; private set; }

        public void Read()
        {
            EventShort = false;
            EventLongWarning = false;
            EventLongCancelled = false;
            EventLong = false;
            lock (_navButton)
            {
                if (_navButton.Read() && _prevButtonState == NavButtonStates.Idle)
                {
                    _buttonTimeout = DateTime.Now.AddMilliseconds(TimeoutDisplayWarning);
                    _prevButtonState = NavButtonStates.Pressed;
                }
                else
                {
                    if (_prevButtonState == NavButtonStates.Pressed && !_navButton.Read())
                    {
                        _prevButtonState = NavButtonStates.Idle;
                        EventShort = true;
                    }
                    else if (_prevButtonState == NavButtonStates.Pressed && _navButton.Read() && _buttonTimeout < DateTime.Now)
                    {
                        _buttonTimeout = DateTime.Now.AddMilliseconds(TimeoutConfirmed);
                        _prevButtonState = NavButtonStates.Warned;
                        EventLongWarning = true;
                    }
                    else if (_prevButtonState == NavButtonStates.Warned && !_navButton.Read())
                    {
                        _buttonTimeout = DateTime.MinValue;
                        _prevButtonState = NavButtonStates.Idle;
                        EventLongCancelled = true;
                    }
                    else if (_prevButtonState == NavButtonStates.Warned && _navButton.Read() && _buttonTimeout < DateTime.Now)
                    {
                        _buttonTimeout = DateTime.MinValue;
                        _prevButtonState = NavButtonStates.PressedLong;
                        EventLong = true;
                    }
                    else if (!_navButton.Read() && _prevButtonState != NavButtonStates.Idle)
                    {
                        _buttonTimeout = DateTime.MinValue;
                        _prevButtonState = NavButtonStates.Idle;
                    }
                }
                Thread.Sleep(5);
            }
        }
    }
}
