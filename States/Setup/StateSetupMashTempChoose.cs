namespace BrewMatic3000.States.Setup
{
    public class StateSetupMashTempChoose : State
    {
        private const int MinTemp = 60;

        private const int MaxTemp = 75;

        public StateSetupMashTempChoose(BrewData brewData)
            : base(brewData)
        {

        }

        public override void Start()
        {
            WriteDefaultText();
        }

        private void WriteDefaultText()
        {
            WriteToLcd("Set Mash Temp", "Current: " + BrewData.MashTemperature + "*C");
        }

        public override void OnKeyPressShort()
        {
            if (++BrewData.MashTemperature > MaxTemp)
            {
                BrewData.MashTemperature = MinTemp;
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
