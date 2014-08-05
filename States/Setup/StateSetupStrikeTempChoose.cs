
using BrewMatic3000.Extensions;

namespace BrewMatic3000.States.Setup
{
    public class StateSetupStrikeTempChoose : State
    {
        private const float MinTemp = 70.0f;

        private const float MaxTemp = 80.0f;

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
            WriteToLcd("Set Strike Temp", "Current: " + BrewData.StrikeTemperature.ToString("f1").PadLeft(4) + "*C");
        }

        public override void OnKeyPressShort()
        {
            BrewData.StrikeTemperature += 0.1f;
            if (BrewData.StrikeTemperature > MaxTemp)
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
            RiseStateChangedEvent((new State1Initial(BrewData)));
        }

        public override string[] GetNewStateIndication(int secondsLeft)
        {
            return null;
        }

    }
}
