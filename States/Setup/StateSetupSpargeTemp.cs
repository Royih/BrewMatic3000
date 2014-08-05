using BrewMatic3000.Extensions;

namespace BrewMatic3000.States.Setup
{
    public class StateSetupSpargeTemp : State
    {

        public StateSetupSpargeTemp(BrewData brewData)
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
            RiseStateChangedEvent(new StateSetupHeat1Effect(BrewData));
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
            RiseStateChangedEvent(new StateSetupSpargeTempChoose(BrewData));
        }

        public override string[] GetNewStateIndication(int secondsLeft)
        {
            return null;
        }

    }
}
