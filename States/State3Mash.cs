
using System;
using System.Threading;
using BrewMatic3000.FakeHW;
using BrewMatic3000.Interfaces;

namespace BrewMatic3000.States
{
    public class State3Mash : State
    {
        private readonly ITempReader _tempReader;

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
                    new NavigateAction("Abort brew", "","..hold to abort", typeof(State1Initial))
                };
            }
        }

        public State3Mash(BrewData brewData)
            : base(brewData)
        {
            _tempReader = new FakeTempReader();
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

                if (_mainDisplayVisible)
                {
                    WriteToLcd(" Mash:  " + ts.ToString(),
                               "Tg:" + BrewData.MashTemperature + " Ac:" + _tempReader.GetValue().ToString("f1"));
                }

                Thread.Sleep(500);
            }
            RiseStateChangedEvent(typeof(State4MashComplete));
        }
    }
}
