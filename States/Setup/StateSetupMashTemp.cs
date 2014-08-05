using BrewMatic3000.Extensions;

namespace BrewMatic3000.States.Setup
{
    public class StateSetupMashTemp : State
    {

        public StateSetupMashTemp(BrewData brewData)
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
            RiseStateChangedEvent(new StateSetupMashOutTemp(BrewData));
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
            RiseStateChangedEvent(new StateSetupMashTempChoose(BrewData));
        }

        public override string[] GetNewStateIndication(int secondsLeft)
        {
            return null;
        }
    }
}
