namespace BrewMatic3000.States.Setup
{
    public class StateSetupHeat1Effect : State
    {

        public StateSetupHeat1Effect(BrewData brewData)
            : base(brewData)
        {
        }

        public override void Start()
        {
            WriteDefaultText();
        }

        private void WriteDefaultText()
        {
            WriteToLcd("Set % Heater 1", "Current: " + BrewData.Heater1.GetCurrentValue() + "%");
        }

        public override void OnKeyPressShort()
        {
            RiseStateChangedEvent(typeof(StateSetupHeat2Effect));
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
            RiseStateChangedEvent(typeof(StateSetupHeat1EffectChoose));
        }

    }
}
