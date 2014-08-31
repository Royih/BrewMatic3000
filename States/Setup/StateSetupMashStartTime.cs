using System;

namespace BrewMatic3000.States.Setup
{
    public class StateSetupMashStartTime : State
    {

        private DateTime _tmpDateTime = DateTime.MinValue;

        private Mode _currentMode = Mode.Year;

        private const int MinYear = 2013;
        private const int MaxYear = 2100;
        private const int MinMonth = 1;
        private const int MaxMonth = 12;
        private const int MinDay = 1;
        private const int MaxDay = 31;
        private const int MinHour = 0;
        private const int MaxHour = 23;
        private const int MinMinute = 0;
        private const int MaxMinute = 59;

        public StateSetupMashStartTime(BrewData brewData, string[] initialMessage = null, int initialScreen = 0)
            : base(brewData, initialMessage, initialScreen)
        {
        }

        public enum Screens
        {
            Default
        }

        private enum Mode
        {
            Year,
            Month,
            Day,
            Hour,
            Minute,
            Second
        }

        private string GetName(Mode mode)
        {
            switch (mode)
            {
                case Mode.Year:
                    return "Year";
                case Mode.Month:
                    return "Month";
                case Mode.Day:
                    return "Day";
                case Mode.Hour:
                    return "Hour";
                case Mode.Minute:
                    return "Minute";
                default:
                    return "Second";
            }
        }

        public override int GetNumberOfScreens()
        {
            return (int)Screens.Default;
        }

        public override Screen GetScreen(int screenNumber)
        {
            switch (screenNumber)
            {
                case (int)Screens.Default:
                    {
                        var line1 = "=Setup Ms. St. Time=";
                        var line2 = "";
                        var line3 = GetName(_currentMode);
                        var line4 = _tmpDateTime.ToString("yyyy MMM dd HH:mm:ss");
                        return new Screen(screenNumber, new[] { line1, line2, line3, line4 }, GetName(_currentMode));
                    }
                default:
                    {
                        return GetScreenError(screenNumber);
                    }
            }
        }

        public override void KeyPressNextShort()
        {
            switch (_currentMode)
            {
                case Mode.Year:
                    {
                        if (_tmpDateTime.Year + 1 <= MaxYear)
                        {
                            _tmpDateTime = new DateTime(_tmpDateTime.Year + 1, _tmpDateTime.Month, _tmpDateTime.Day, _tmpDateTime.Hour, _tmpDateTime.Minute, _tmpDateTime.Second);
                        }
                        else
                        {
                            _tmpDateTime = new DateTime(MinYear, _tmpDateTime.Month, _tmpDateTime.Day, _tmpDateTime.Hour, _tmpDateTime.Minute, _tmpDateTime.Second);
                        }
                        break;
                    }
                case Mode.Month:
                    {
                        if (_tmpDateTime.Month + 1 <= MaxMonth)
                        {
                            _tmpDateTime = new DateTime(_tmpDateTime.Year, _tmpDateTime.Month + 1, _tmpDateTime.Day, _tmpDateTime.Hour, _tmpDateTime.Minute, _tmpDateTime.Second);
                        }
                        else
                        {
                            _tmpDateTime = new DateTime(_tmpDateTime.Year, MinMonth, _tmpDateTime.Day, _tmpDateTime.Hour, _tmpDateTime.Minute, _tmpDateTime.Second);
                        }
                        break;
                    }
                case Mode.Day:
                    {
                        if (_tmpDateTime.Day + 1 <= MaxDay)
                        {
                            _tmpDateTime = new DateTime(_tmpDateTime.Year, _tmpDateTime.Month, _tmpDateTime.Day + 1, _tmpDateTime.Hour, _tmpDateTime.Minute, _tmpDateTime.Second);
                        }
                        else
                        {
                            _tmpDateTime = new DateTime(_tmpDateTime.Year, _tmpDateTime.Month, MinDay, _tmpDateTime.Hour, _tmpDateTime.Minute, _tmpDateTime.Second);
                        }
                        break;
                    }
                case Mode.Hour:
                    {
                        if (_tmpDateTime.Hour + 1 <= MaxHour)
                        {
                            _tmpDateTime = new DateTime(_tmpDateTime.Year, _tmpDateTime.Month, _tmpDateTime.Day, _tmpDateTime.Hour + 1, _tmpDateTime.Minute, _tmpDateTime.Second);
                        }
                        else
                        {
                            _tmpDateTime = new DateTime(_tmpDateTime.Year, _tmpDateTime.Month, _tmpDateTime.Day, MinHour, _tmpDateTime.Minute, _tmpDateTime.Second);
                        }
                        break;
                    }
                case Mode.Minute:
                    {
                        if (_tmpDateTime.Minute + 1 <= MaxMinute)
                        {
                            _tmpDateTime = new DateTime(_tmpDateTime.Year, _tmpDateTime.Month, _tmpDateTime.Day, _tmpDateTime.Hour, _tmpDateTime.Minute + 1, _tmpDateTime.Second);
                        }
                        else
                        {
                            _tmpDateTime = new DateTime(_tmpDateTime.Year, MaxMonth, _tmpDateTime.Day, _tmpDateTime.Hour, MinMinute, _tmpDateTime.Second);
                        }
                        break;
                    }
            }
        }

        public override void KeyPressPreviousShort()
        {
            switch (_currentMode)
            {
                case Mode.Year:
                    {
                        if (_tmpDateTime.Year - 1 >= MinYear)
                        {
                            _tmpDateTime = new DateTime(_tmpDateTime.Year - 1, _tmpDateTime.Month, _tmpDateTime.Day, _tmpDateTime.Hour, _tmpDateTime.Minute, 0);
                        }
                        else
                        {
                            _tmpDateTime = new DateTime(MaxYear, _tmpDateTime.Month, _tmpDateTime.Day, _tmpDateTime.Hour, _tmpDateTime.Minute, 0);
                        }
                        break;
                    }
                case Mode.Month:
                    {
                        if (_tmpDateTime.Month - 1 >= MinMonth)
                        {
                            _tmpDateTime = new DateTime(_tmpDateTime.Year, _tmpDateTime.Month - 1, _tmpDateTime.Day, _tmpDateTime.Hour, _tmpDateTime.Minute, 0);
                        }
                        else
                        {
                            _tmpDateTime = new DateTime(_tmpDateTime.Year, MaxMonth, _tmpDateTime.Day, _tmpDateTime.Hour, _tmpDateTime.Minute, 0);
                        }
                        break;
                    }
                case Mode.Day:
                    {
                        if (_tmpDateTime.Day - 1 >= MinDay)
                        {
                            _tmpDateTime = new DateTime(_tmpDateTime.Year, _tmpDateTime.Month, _tmpDateTime.Day - 1, _tmpDateTime.Hour, _tmpDateTime.Minute, 0);
                        }
                        else
                        {
                            _tmpDateTime = new DateTime(_tmpDateTime.Year, _tmpDateTime.Month, MaxDay, _tmpDateTime.Hour, _tmpDateTime.Minute, 0);
                        }
                        break;
                    }
                case Mode.Hour:
                    {
                        if (_tmpDateTime.Hour - 1 >= MinHour)
                        {
                            _tmpDateTime = new DateTime(_tmpDateTime.Year, _tmpDateTime.Month, _tmpDateTime.Day, _tmpDateTime.Hour - 1, _tmpDateTime.Minute, 0);
                        }
                        else
                        {
                            _tmpDateTime = new DateTime(_tmpDateTime.Year, _tmpDateTime.Month, _tmpDateTime.Day, MaxHour, _tmpDateTime.Minute, 0);
                        }
                        break;
                    }
                case Mode.Minute:
                    {
                        if (_tmpDateTime.Minute - 1 >= MinMinute)
                        {
                            _tmpDateTime = new DateTime(_tmpDateTime.Year, _tmpDateTime.Month, _tmpDateTime.Day, _tmpDateTime.Hour, _tmpDateTime.Minute - 1, 0);
                        }
                        else
                        {
                            _tmpDateTime = new DateTime(_tmpDateTime.Year, _tmpDateTime.Month, _tmpDateTime.Day, _tmpDateTime.Hour, MaxMinute, 0);
                        }
                        break;
                    }
            }
        }

        public override void KeyPressNextLong()
        {
            _currentMode++;
            if (_currentMode == Mode.Second)
            {
                BrewData.MashStartTime = _tmpDateTime;
                RiseStateChangedEvent(new StateSetup(BrewData, null, (int)StateSetup.Screens.MashStartTime));
            }
        }

        protected override void StartExtra()
        {
            _tmpDateTime = BrewData.MashStartTime;
        }


    }
}
