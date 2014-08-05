
using BrewMatic3000.Extensions;

namespace BrewMatic3000.States.Setup
{
    public class StateSetupSpargeTempChoose : State
    {
        private const float MinTemp = 70.0f;

        private const float MaxTemp = 90.0f;

        public StateSetupSpargeTempChoose(BrewData brewData)
            : base(brewData)
        {

        }

        public override void Start()
        {
            WriteDefaultText();
        }

        private void WriteDefaultText()
        {
            WriteToLcd("Set Sparge Temp", "Current: " + BrewData.SpargeTemperature.ToString("f1").PadLeft(4) + "*C");
        }

        public override void OnKeyPressShort()
        {
            BrewData.SpargeTemperature += 0.1f;
            if (BrewData.SpargeTemperature > MaxTemp)
            {
                BrewData.SpargeTemperature = MinTemp;
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
