using BrewMatic3000.States.Setup;

namespace BrewMatic3000.States
{
    public class State1Initial : State
    {
        public State1Initial(BrewData brewData = null) : base(brewData) { }


        private void ApplyDefaultScreen()
        {
            WriteToLcd("S:" + BrewData.StrikeTemperature + "*C T:" + BrewData.MashTemperature + "*C", "Mash:" + BrewData.MashTime + "min");
        }

        public override void OnKeyPressLongWarning()
        {
            WriteToLcd("..hold to start", "==>[-");
        }

        public override void OnKeyPressLong()
        {
            RiseStateChangedEvent(typeof(State2StrikeWarmup));
        }

        public override void OnKeyPressLongCancelled()
        {
            ApplyDefaultScreen();
        }


        public override void Start()
        {
            ApplyDefaultScreen();
        }

        public override void OnKeyPressShort()
        {
            RiseStateChangedEvent(typeof(StateSetupStrikeTemp));
        }

    }
}
