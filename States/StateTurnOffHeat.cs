using BrewMatic3000.Extensions;

namespace BrewMatic3000.States
{
    public class StateTurnOffHeat : State
    {

        public StateTurnOffHeat(BrewData brewData)
            : base(brewData)
        {
        }

        public override void Start()
        {
            WriteDefaultText();
        }

        private void WriteDefaultText()
        {
            WriteToLcd("Turn off heat", "H1: " + BrewData.Heater1.GetCurrentValue().DisplayHeat() + " H2: " + BrewData.Heater2.GetCurrentValue().DisplayHeat());
        }

        public override void OnKeyPressShort()
        {
            RiseStateChangedEvent(new StateDashboard(BrewData));
        }

        public override void OnKeyPressLongWarning()
        {
            WriteToLcd("..hold to turn off heat");
        }

        public override void OnKeyPressLongCancelled()
        {
            WriteDefaultText();
        }

        public override void OnKeyPressLong()
        {
            BrewData.Heater1.SetValue(0);
            BrewData.Heater2.SetValue(0);
            RiseStateChangedEvent(new StateDashboard(BrewData));
        }

        public override string[] GetNewStateIndication(int secondsLeft)
        {
            return null;
        }
    }
}
