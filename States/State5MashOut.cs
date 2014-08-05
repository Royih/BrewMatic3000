using System;
using System.Threading;
using BrewMatic3000.Extensions;

namespace BrewMatic3000.States
{
    public class State5Mashout : State
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
                    new NavigateAction("Mash out complt?", "","..completed?", new State6Sparge(BrewData)),
                    new NavigateAction("Abort brew?", "","..hold to abort", new State1Initial(BrewData))
                };
            }
        }

        public State5Mashout(BrewData brewData)
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
                WriteToLcd("..mash out complt?");
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
                RiseStateChangedEvent(new State6Sparge(BrewData));
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
            BrewData.BrewMashOutStart = DateTime.Now;
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
                    WriteToLcd("Mash out| W:" + (int)BrewData.Heater1.GetCurrentValue() + "%",
                               "Tg:" + preferredMashTemp.ToString("f1").PadLeft(4) + " Ac:" + currentTemp1.ToString("f1").PadLeft(4));
                }

                Thread.Sleep(1000);
            }
        }

        public override string[] GetNewStateIndication(int secondsLeft)
        {
            return new[] { "Start mash out..", "In " + secondsLeft + " seconds" };
        }
    }
}
