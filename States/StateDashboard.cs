using System.Text;
using System.Threading;
using BrewMatic3000.Extensions;

namespace BrewMatic3000.States
{
    public class StateDashboard : State
    {
        private Thread _worker;

        private bool _abort;

        private bool _mainDisplayVisible = true;

        public StateDashboard(BrewData brewData)
            : base(brewData)
        {

        }

        public override void OnKeyPressLongWarning()
        {
            _mainDisplayVisible = false;
            WriteToLcd("..turn off heat");
        }

        public override void OnKeyPressLongCancelled()
        {
            _mainDisplayVisible = true;
        }

        public override void OnKeyPressLong()
        {
            BrewData.Heater1.SetValue(0);
            BrewData.Heater2.SetValue(0);
            _mainDisplayVisible = true;
        }

        public override void OnKeyPressShort()
        {
            RiseStateChangedEvent(typeof(State1Initial));
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
                if (_mainDisplayVisible)
                {
                    var strLine1 = "T1:" + BrewData.TempReader1.GetValue().ToString("f1").PadLeft(4) + " T2:" + BrewData.TempReader2.GetValue().ToString("f1").PadLeft(4);
                    var strLine2 = "H1:" + BrewData.Heater1.GetCurrentValue().DisplayHeat() + " H2:" + BrewData.Heater2.GetCurrentValue().DisplayHeat();
                    WriteToLcd(strLine1, strLine2);
                }
                Thread.Sleep(1000);
            }
        }



    }
}
