
using System.Threading;
using BrewMatic3000.Extensions;
using BrewMatic3000.Interfaces;
using Microsoft.SPOT;

namespace BrewMatic3000.States
{
    public class State2Warmup : State
    {
        private readonly ITempReader _tempReader1;
        private readonly ITempReader _tempReader2;

        private readonly IHeatDevice _heater1;
        private readonly IHeatDevice _heater2;


        private readonly PID.PID _mashPID;
        private readonly PID.PID _spargePID;

        private float _maxTemp1;
        private float _maxTemp2;

        private Thread _worker;

        private bool _abort;

        private bool _mainDisplayVisible = true;

        private NavigateAction[] Actions
        {
            get
            {
                return new[]
                {
                    new NavigateAction("Abort brew", "","..hold to abort", typeof(State1Initial))
                };
            }
        }

        public State2Warmup(BrewData brewData)
            : base(brewData)
        {
            _maxTemp1 = 0;
            _maxTemp2 = 0;

            _tempReader1 = brewData.TempReader1;
            _tempReader2 = brewData.TempReader2;
            _heater1 = brewData.Heater1;
            _heater2 = brewData.Heater2;

            _mashPID = new PID.PID(brewData.MashPIDKp, brewData.MashPIDKi, brewData.MashPIDKd);
            _spargePID = new PID.PID(brewData.SpargePIDKp, brewData.SpargePIDKi, brewData.SpargePIDKd);
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
                WriteToLcd(".start mashing?");
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
            else
            {
                RiseStateChangedEvent(typeof(State3Mash));
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

                //Output all logValues to standard output
                var logValues = _mashPID.GetLogValues();
                if (logValues != null)
                {
                    foreach (var logValue in logValues)
                    {
                        Debug.Print(logValue.TimeStamp + ";" + logValue.Temperature + ";" + logValue.Effect);
                    }
                }
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
            while (!_abort)
            {
                var currentTemp1 = _tempReader1.GetValue();
                var currentTemp2 = _tempReader2.GetValue();

                //keep the highest temp (for pid tuning purposes)
                if (_maxTemp1 < currentTemp1)
                {
                    _maxTemp1 = currentTemp1;
                }
                if (_maxTemp2 < currentTemp2)
                {
                    _maxTemp2 = currentTemp2;
                }

                var pidOutputMash = _mashPID.GetValue(currentTemp1, BrewData.StrikeTemperature);
                _heater1.SetValue(pidOutputMash);

                /*var pidOutputSparge = _spargePID.GetValue(currentTemp2, BrewData.SpargeWaterTemperature);
                _heater2.SetValue(pidOutputSparge);*/

                if (_mainDisplayVisible)
                {
                    var line1String = GetLineString(currentTemp1, BrewData.StrikeTemperature, _heater1.GetCurrentValue());
                    var line2String = GetLineString(currentTemp2, BrewData.SpargeWaterTemperature, _heater2.GetCurrentValue());
                    WriteToLcd(line1String, line2String);
                }
                Thread.Sleep(1500);
            }
        }

        private string GetLineString(float currentTemp, float desiredTemp, float watt)
        {
            var currentTempString = currentTemp.ToString("f1").PadLeft(4);
            var desiredTempString = desiredTemp.ToString("f1").PadLeft(4);
            return currentTempString + "|" + desiredTempString + "|W:" + (int)watt + "%"; //58.9|68.0|W:100%"
        }
    }
}
