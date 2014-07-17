namespace BrewMatic3000.States.Setup
{
    public class StateSetupStrikeTemp : State
    {

        public StateSetupStrikeTemp(BrewData brewData)
            : base(brewData)
        {
        }

        public override void Start()
        {
            WriteDefaultText();
        }

        private void WriteDefaultText()
        {
            WriteToLcd("Set Strike Temp", "Current: " + BrewData.StrikeTemperature + "*C");
        }

        public override void OnKeyPressShort()
        {
            RiseStateChangedEvent(typeof(StateSetupMashTemp));
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
            RiseStateChangedEvent(typeof(StateSetupStrikeTempChoose));
        }

    }
}
