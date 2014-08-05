using BrewMatic3000.Extensions;

namespace BrewMatic3000.States.Setup
{
    public class StateSetupMashOutTemp : State
    {

        public StateSetupMashOutTemp(BrewData brewData)
            : base(brewData)
        {
        }

        public override void Start()
        {
            WriteDefaultText();
        }

        private void WriteDefaultText()
        {
            WriteToLcd("Set Mash out Temp", "Current: " + BrewData.MashOutTemperature.ToString("f1").PadLeft(4) + "*C");
        }

        public override void OnKeyPressShort()
        {
            RiseStateChangedEvent(new StateSetupMashTime(BrewData));
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
            RiseStateChangedEvent(new StateSetupMashOutTempChoose(BrewData));
        }

        public override string[] GetNewStateIndication(int secondsLeft)
        {
            return null;
        }
    }
}
