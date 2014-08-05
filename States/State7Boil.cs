using System;
using System.Threading;
using BrewMatic3000.Extensions;

namespace BrewMatic3000.States
{
    public class State7Boil : State
    {
        private Thread _worker;

        private bool _abort;

        private bool _mainDisplayVisible = true;

        private NavigateAction[] Actions
        {
            get
            {
                return new[]
                {
                    new NavigateAction("Output log?", "","..hold to output", new StateOutputLog(BrewData)),
                    new NavigateAction("Reset brew?", "","..hold to reset", new State1Initial(BrewData))
                };
            }
        }

        public State7Boil(BrewData brewData)
            : base(brewData)
        {
            
        }

        public override void OnKeyPressLongWarning()
        {
            _mainDisplayVisible = false;
            var action = GetSelectedAction(Actions);
            if (action != null)
            {
                WriteToLcd(action.Warning);
            }
            else
            {
                WriteToLcd("..reset brew?");
            }
        }

        public override void OnKeyPressLongCancelled()
        {
            _mainDisplayVisible = true;
        }

        public override void OnKeyPressLong()
        {
            var action = GetSelectedAction(Actions);
            if (action != null)
            {
                RiseStateChangedEvent(action.NextState);
            }
        }

        public override void OnKeyPressShort()
        {
            var action = ToggleActions(Actions);
            if (action != null)
            {
                _mainDisplayVisible = false;
                WriteToLcd(action);
            }
            else
            {
                _mainDisplayVisible = true;
            }
        }

        public override void Start()
        {
            ShowStateName(); //Display info about this new state in n seconds
            BrewData.BrewSpargeEnd = DateTime.Now;
            _worker = new Thread(
              DoWork
              ) { Priority = ThreadPriority.Normal };
            _worker.Start();
        }

        public override void Dispose()
        {
            _abort = true;
        }

        private void DoWork()
        {
            while (!_abort)
            {
                if (_mainDisplayVisible)
                {
                    WriteToLcd("Boiling.", "My wrk is done:)");
                }
                Thread.Sleep(1000);
            }
        }

        public override string[] GetNewStateIndication(int secondsLeft)
        {
            return new[] { "Begin boil..", "In " + secondsLeft + " seconds" };
        }
    }
}
