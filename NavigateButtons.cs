using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace BrewMatic3000
{
    public delegate void MyEventHandler();

    public class NavigateButtons : IDisposable
    {
        private bool _abort;

        private readonly NavigateButton _prevButton;

        private readonly NavigateButton _nextButton;

        public event MyEventHandler KeyPressPreviousShort;
        public event MyEventHandler KeyPressPreviousLongWarning;
        public event MyEventHandler KeyPressPreviousLongWarningCancelled;
        public event MyEventHandler KeyPressPreviousLong;


        public event MyEventHandler KeyPressNextShort;
        public event MyEventHandler KeyPressNextLongWarning;
        public event MyEventHandler KeyPressNextLongWarningCancelled;
        public event MyEventHandler KeyPressNextLong;

        public NavigateButtons(InputPort navButtonPrevious, InputPort navButtonNext)
        {
            _prevButton = new NavigateButton(navButtonPrevious);
            _nextButton = new NavigateButton(navButtonNext);
            var worker = new Thread(
            DoWork
            );
            worker.Start();
        }

        private void DoWork()
        {
            while (!_abort)
            {
                _prevButton.Read();
                _nextButton.Read();

                if (_prevButton.EventShort)
                {
                    KeyPressPreviousShort();
                    Debug.Print("Nav Button: Keypress Prev -> Short event");
                }

                if (_prevButton.EventLongWarning)
                {
                    KeyPressPreviousLongWarning();
                    Debug.Print("Nav Button: Keypress Prev -> Long warning event");
                }

                if (_prevButton.EventLongCancelled)
                {
                    KeyPressPreviousLongWarningCancelled();
                    Debug.Print("Nav Button: Keypress Prev -> Long warning cancelled event");
                }

                if (_prevButton.EventLong)
                {
                    KeyPressPreviousLong();
                    Debug.Print("Nav Button: Keypress Prev -> Long event");
                }


                if (_nextButton.EventShort)
                {
                    KeyPressNextShort();
                    Debug.Print("Nav Button: Keypress Next -> Short event");
                }

                if (_nextButton.EventLongWarning)
                {
                    KeyPressNextLongWarning();
                    Debug.Print("Nav Button: Keypress Next -> Long warning event");
                }

                if (_nextButton.EventLongCancelled)
                {
                    KeyPressNextLongWarningCancelled();
                    Debug.Print("Nav Button: Keypress Next -> Long warning cancelled event");
                }

                if (_nextButton.EventLong)
                {
                    KeyPressNextLong();
                    Debug.Print("Nav Button: Keypress Next -> Long event");
                }


                Thread.Sleep(5);
            }
        }



        public void Dispose()
        {
            _abort = true;
        }
    }


}
