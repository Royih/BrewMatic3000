
namespace BrewMatic3000.States.Setup
{
    public class StateSetupHeat1EffectChoose : State
    {
        public StateSetupHeat1EffectChoose(BrewData brewData)
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
            if (BrewData.Heater1.GetCurrentValue() == 100)
            {
                BrewData.Heater1.SetValue(0);
            }
            else
            {
                BrewData.Heater1.SetValue(BrewData.Heater1.GetCurrentValue() + 10);
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
            RiseStateChangedEvent(new StateDashboard(BrewData));
        }

        public override string[] GetNewStateIndication(int secondsLeft)
        {
            return null;
        }

    }
}
