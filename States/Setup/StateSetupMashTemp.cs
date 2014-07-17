namespace BrewMatic3000.States.Setup
{
    public class StateSetupMashTemp : State
    {
        
        public StateSetupMashTemp(BrewData brewData) : base(brewData)
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
            RiseStateChangedEvent(typeof(StateSetupMashTime));
        }

        public override void OnKeyPressLongWarning()
        {
            WriteToLcd("..hold to change");
        }

        public override void OnKeyPressLongCancelled()
        {
            WriteDefaultText();
        }

        public override void OnKeyPressLong()
        {
            RiseStateChangedEvent(typeof(StateSetupMashTempChoose));
        }

    }
}
