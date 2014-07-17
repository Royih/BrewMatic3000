
namespace BrewMatic3000.States.Setup
{
    public class StateSetupStrikeTempChoose : State
    {
        private const int MinTemp = 55;

        private const int MaxTemp = 95;

        public StateSetupStrikeTempChoose(BrewData brewData)
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
            if (++BrewData.StrikeTemperature > MaxTemp)
            {
                BrewData.StrikeTemperature = MinTemp;
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
