namespace BrewMatic3000.States.Setup
{
    public class StateSetupMashTime : State
    {

        public StateSetupMashTime(BrewData brewData)
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
            RiseStateChangedEvent(new StateSetupSpargeTemp(BrewData));
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
            RiseStateChangedEvent(new StateSetupMashTimeChoose(BrewData));
        }

        public override string[] GetNewStateIndication(int secondsLeft)
        {
            return null;
        }

    }
}
