using BrewMatic3000.Extensions;

namespace BrewMatic3000.States.Setup
{
    public class StateSetupMashTempChoose : State
    {
        private const float MinTemp = 60.0f;

        private const float MaxTemp = 75.0f;

        public StateSetupMashTempChoose(BrewData brewData)
            : base(brewData)
        {

        }

        public override void Start()
        {
            WriteDefaultText();
        }

        private void WriteDefaultText()
        {
            WriteToLcd("Set Mash Temp", "Current: " + BrewData.MashTemperature.ToString("f1").PadLeft(4) + "*C");
        }

        public override void OnKeyPressShort()
        {
            BrewData.MashTemperature += 0.1f;
            if (BrewData.MashTemperature > MaxTemp)
            {
                BrewData.MashTemperature = MinTemp;
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
            RiseStateChangedEvent(new State1Initial(BrewData));
        }

        public override string[] GetNewStateIndication(int secondsLeft)
        {
            return null;
        }

    }
}
