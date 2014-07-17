using System.Text;
using System.Threading;

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
                    var strLine1 = "T1:" + BrewData.TempReader.GetValue().ToString("f1") + " T2:" + "0.0";
                    var strLine2 = "H1:" + PadLeft(BrewData.Heater1.GetCurrentValue(), 3) + "% H2:" + PadLeft(BrewData.Heater2.GetCurrentValue(), 3) + "%";
                    WriteToLcd(strLine1, strLine2);
                }
                Thread.Sleep(1000);
            }
        }

        private string PadLeft(float input, int length)
        {
            var returnValue = input.ToString();
            while (returnValue.Length < length)
            {
                returnValue = " " + returnValue;
            }
            return returnValue;
        }

    }
}
