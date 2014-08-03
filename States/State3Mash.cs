using System;
using System.Threading;
using BrewMatic3000.Extensions;

namespace BrewMatic3000.States
{
    public class State3Mash : State
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
                    new NavigateAction("Mash compelete", "","..completed?", typeof(State4MashComplete)),
                    new NavigateAction("Abort brew", "","..hold to abort", typeof(State1Initial))
                };
            }
        }

        public State3Mash(BrewData brewData)
            : base(brewData)
        {
            _mashComplete = DateTime.Now.AddMinutes(BrewData.MashTime);
        }

        public override void OnKeyPressLongWarning()
        {
            _mainDisplayVisible = false;
            var action = GetSelectedAction(Actions);
            if (action != null)
            {
                WriteToLcd(action.Warning);
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
                RiseStateChangedEvent(action.StateType);
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
                var preferredSpargeTemp = BrewData.SpargeWaterTemperature;


                var pidOutputMash = BrewData.MashPID.GetValue(currentTemp1, preferredMashTemp);
                BrewData.Heater1.SetValue(pidOutputMash);

                var pidOutputSparge = BrewData.SpargePID.GetValue(currentTemp2, preferredSpargeTemp);
                BrewData.Heater2.SetValue(pidOutputSparge);

                if (_mainDisplayVisible)
                {
                    WriteToLcd(" Mash:  " + ts,
                               "Tg:" + preferredMashTemp + " Ac:" + currentTemp1.ToString("f1").PadLeft(4));
                }

                Thread.Sleep(1000);
            }
            RiseStateChangedEvent(typeof(State4MashComplete));
        }
    }
}
