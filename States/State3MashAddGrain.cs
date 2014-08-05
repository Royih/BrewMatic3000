
using System;
using System.Threading;
using BrewMatic3000.Extensions;
using BrewMatic3000.FakeHW;
using BrewMatic3000.Interfaces;

namespace BrewMatic3000.States
{
    public class State3MashAddGrain : State
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
                    new NavigateAction("Start mashing?", "","..hold to start", new State4Mash(BrewData)),
                    new NavigateAction("Abort brew?", "","..hold to abort", new State1Initial(BrewData))
                };
            }
        }

        public State3MashAddGrain(BrewData brewData)
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
                WriteToLcd("..start mashing?");
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
                RiseStateChangedEvent(new State4Mash(BrewData));
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
            BrewData.BrewAddGrainStart = DateTime.Now;
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

                var preferredMashTemp = BrewData.MashTemperature;
                var preferredSpargeTemp = BrewData.SpargeTemperature;

                var pidOutputMash = BrewData.MashPID.GetValue(currentTemp1, preferredMashTemp);
                BrewData.Heater1.SetValue(pidOutputMash);

                var pidOutputSparge = BrewData.SpargePID.GetValue(currentTemp2, BrewData.SpargeTemperature);
                BrewData.Heater2.SetValue(pidOutputSparge);


                if (_mainDisplayVisible)
                {
                    var line1String = GetLineString(currentTemp1, preferredMashTemp, BrewData.Heater1.GetCurrentValue());
                    var line2String = GetLineString(currentTemp2, preferredSpargeTemp, BrewData.Heater2.GetCurrentValue());
                    WriteToLcd(line1String, line2String);
                }

                Thread.Sleep(1000);
            }
        }
        private string GetLineString(float currentTemp, float desiredTemp, float watt)
        {
            var currentTempString = currentTemp.ToString("f1").PadLeft(4);
            var desiredTempString = desiredTemp.ToString("f1").PadLeft(4);
            return currentTempString + "|" + desiredTempString + "|W:" + (int)watt + "%"; //58.9|68.0|W:100%"
        }

        public override string[] GetNewStateIndication(int secondsLeft)
        {
            return new[] { "Add grain..", "In " + secondsLeft + " seconds" };
        }
    }
}
