
namespace BrewMatic3000.States.Setup
{
    public class StateSetupHeat2EffectChoose : State
    {
        public StateSetupHeat2EffectChoose(BrewData brewData)
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
            if (BrewData.Heater2.GetCurrentValue() == 100)
            {
                BrewData.Heater2.SetValue(0);
            }
            else
            {
                BrewData.Heater2.SetValue(BrewData.Heater2.GetCurrentValue() + 10);
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
            RiseStateChangedEvent(typeof(StateDashboard));
        }

    }
}
