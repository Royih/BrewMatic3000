using System;
using System.Threading;
using BrewMatic3000.Extensions;

namespace BrewMatic3000.States
{
    public class State4Mash : State
    {
        private Thread _worker;

        private bool _abort;

        private bool _mainDisplayVisible = true;

        private DateTime _mashComplete;

        private NavigateAction[] Actions
        {
            get
            {
                return new[]
                {
                    new NavigateAction("Mash complete?", "","..start mashout?", new State5Mashout(BrewData)),
                    new NavigateAction("Abort brew?", "","..hold to abort", new State1Initial(BrewData))
                };
            }
        }

        public State4Mash(BrewData brewData)
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
                WriteToLcd("..mash complete?");
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
                RiseStateChangedEvent(new State5Mashout(BrewData));
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
            BrewData.BrewMashStart = DateTime.Now;
            _mashComplete = DateTime.Now.AddMinutes(BrewData.MashTime);
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
            while (!_abort && _mashComplete > DateTime.Now)
            {
                var ts = _mashComplete.Subtract(DateTime.Now);

                var currentTemp1 = BrewData.TempReader1.GetValue();
                var currentTemp2 = BrewData.TempReader2.GetValue();

                var preferredMashTemp = BrewData.MashTemperature;
                var preferredSpargeTemp = BrewData.SpargeTemperature;


                var pidOutputMash = BrewData.MashPID.GetValue(currentTemp1, preferredMashTemp);
                BrewData.Heater1.SetValue(pidOutputMash);

                var pidOutputSparge = BrewData.SpargePID.GetValue(currentTemp2, preferredSpargeTemp);
                BrewData.Heater2.SetValue(pidOutputSparge);

                if (_mainDisplayVisible)
                {
                    WriteToLcd("Mash:   " + ts,
                               "Tg:" + preferredMashTemp.ToString("f1").PadLeft(4) + " Ac:" + currentTemp1.ToString("f1").PadLeft(4));
                }

                Thread.Sleep(1000);
            }
            if (!_abort)
            {
                RiseStateChangedEvent(new State5Mashout(BrewData));
            }
        }

        public override string[] GetNewStateIndication(int secondsLeft)
        {
            return new[] { "Start mashing..", "In " + secondsLeft + " seconds" };
        }
    }
}
