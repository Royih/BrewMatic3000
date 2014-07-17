
namespace BrewMatic3000.States.Setup
{
    public class StateSetupMashTimeChoose : State
    {
        private const int MinTime = 50;

        private const int MaxTime = 95;

        public StateSetupMashTimeChoose(BrewData brewData)
            : base(brewData)
        {

        }

        public override void Start()
        {
            WriteDefaultText();
        }

        private void WriteDefaultText()
        {
            WriteToLcd("Set Mash Time", "Current: " + BrewData.MashTime + "min");
        }

        public override void OnKeyPressShort()
        {
            if (++BrewData.MashTime > MaxTime)
            {
                BrewData.MashTime = MinTime;
            }
            WriteDefaultText();

        }

        public override void OnKeyPressLongWarning()
        {
            WriteToLcd("..hold to save");
        }

        public override void OnKeyPressLongCancelled()
        {
            WriteDefaultText();
        }

        public override void OnKeyPressLong()
        {
            RiseStateChangedEvent(typeof(State1Initial));
        }

    }
}
