using System;
using System.Threading;

namespace BrewMatic3000.States
{
    public class State6Sparge : State
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
                    new NavigateAction("Sparge complt?", "","..completed?", new State7Boil(BrewData)),
                    new NavigateAction("Abort brew?", "","..hold to abort", new State1Initial(BrewData))
                };
            }
        }

        public State6Sparge(BrewData brewData)
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
                WriteToLcd("..sparge complt?");
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
            else
            {
                RiseStateChangedEvent(new State7Boil(BrewData));
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
            BrewData.BrewSpargeStart = DateTime.Now;
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
            BrewData.Heater1.SetValue(0);
            BrewData.Heater2.SetValue(0);
            while (!_abort)
            {
                var currentTemp1 = BrewData.TempReader1.GetValue();
                var currentTemp2 = BrewData.TempReader2.GetValue();

                var preferredMashTemp = BrewData.MashOutTemperature;
                var preferredSpargeTemp = BrewData.SpargeTemperature;


                var pidOutputMash = BrewData.MashPID.GetValue(currentTemp1, preferredMashTemp);
                BrewData.Heater1.SetValue(pidOutputMash);

                var pidOutputSparge = BrewData.SpargePID.GetValue(currentTemp2, preferredSpargeTemp);
                BrewData.Heater2.SetValue(pidOutputSparge);

                if (_mainDisplayVisible)
                {
                    WriteToLcd("Sparge..",
                               "Timer:  " + DateTime.Now.Subtract(BrewData.BrewSpargeStart));
                }

                Thread.Sleep(1000);
            }
        }

        public override string[] GetNewStateIndication(int secondsLeft)
        {
            return new[] { "Start sparge..", "In " + secondsLeft + " seconds" };
        }
    }
}
