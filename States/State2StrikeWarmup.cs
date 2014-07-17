
using System.Threading;
using BrewMatic3000.Interfaces;

namespace BrewMatic3000.States
{
    public class State2StrikeWarmup : State
    {
        private readonly ITempReader _tempReader;
        private readonly IHeatDevice _heater1;
        //private readonly FakeEnvironment _fakeEnvironment;
        private readonly PID _tempController;

        private float _maxTemp = 0f;

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

        public State2StrikeWarmup(BrewData brewData)
            : base(brewData)
        {
            _maxTemp = 0;

            _tempReader = brewData.TempReader;
            //_heater = new FakeHeater(10);
            _heater1 = brewData.Heater1;
            _tempController = new PID();

            //_fakeEnvironment = new FakeEnvironment(_tempReader);
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
                var currentTemp = _tempReader.GetValue();

                //keep the highest temp (for pid tuning purposes)
                if (_maxTemp < currentTemp)
                {
                    _maxTemp = currentTemp;
                }

                var pidOutput = _tempController.GetValue(currentTemp, BrewData.StrikeTemperature);
                _heater1.SetValue(pidOutput);

                if (_mainDisplayVisible)
                {
                    var currentTempString = currentTemp.ToString("f1");
                    var desiredTempString = BrewData.StrikeTemperature.ToString("f1");

                    var line1String = currentTempString + "|" + desiredTempString + "|W:" + (int)_heater1.GetCurrentValue() + "%"; //58.9|68.0|W:100%"

                    var line2String = "Max:" + _maxTemp.ToString("f1");
                    WriteToLcd(line1String, line2String);
                }
                Thread.Sleep(750);
            }
        }
    }
}
