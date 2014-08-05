using BrewMatic3000.Extensions;

namespace BrewMatic3000.States.Setup
{
    public class StateSetupMashOutTempChoose : State
    {
        private const float MinTemp = 76.0f;

        private const float MaxTemp = 90.0f;

        public StateSetupMashOutTempChoose(BrewData brewData)
            : base(brewData)
        {

        }

        public override void Start()
        {
            WriteDefaultText();
        }

        private void WriteDefaultText()
        {
            WriteToLcd("Set Mash Temp", "Current: " + BrewData.MashOutTemperature.ToString("f1").PadLeft(4) + "*C");
        }

        public override void OnKeyPressShort()
        {
            BrewData.MashOutTemperature += 0.1f;
            if (BrewData.MashOutTemperature > MaxTemp)
            {
                BrewData.MashOutTemperature = MinTemp;
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
