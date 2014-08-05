namespace BrewMatic3000.States.Setup
{
    public class StateSetupHeat2Effect : State
    {

        public StateSetupHeat2Effect(BrewData brewData)
            : base(brewData)
        {
        }

        public override void Start()
        {
            WriteDefaultText();
        }

        private void WriteDefaultText()
        {
            WriteToLcd("Set % Heater 2", "Current: " + BrewData.Heater2.GetCurrentValue() + "%");
        }

        public override void OnKeyPressShort()
        {
            if (BrewData.Heater1.GetCurrentValue() > 0 || BrewData.Heater2.GetCurrentValue() > 0)
            {
                RiseStateChangedEvent(new StateTurnOffHeat(BrewData));
            }
            else
            {
                RiseStateChangedEvent(new StateDashboard(BrewData));
            }


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
            RiseStateChangedEvent(new StateSetupHeat2EffectChoose(BrewData));
        }

        public override string[] GetNewStateIndication(int secondsLeft)
        {
            return null;
        }
    }
}
