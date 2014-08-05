using System;
using BrewMatic3000.Extensions;
using BrewMatic3000.States.Setup;

namespace BrewMatic3000.States
{
    public class State1Initial : State
    {
        public State1Initial(BrewData brewData = null) : base(brewData) { }


        private void ApplyDefaultScreen()
        {
            var line1String = "St:" + BrewData.StrikeTemperature.ToString("f1").PadLeft(4) + "|Sp:" + BrewData.SpargeTemperature.ToString("f1").PadLeft(4); //St:70.5|Sp:12.2
            var line2String = "Ms:" + BrewData.MashTemperature.ToString("f1").PadLeft(4) + "|Tm:" + BrewData.MashTime; //Ms:65.1|Tm:60 
            WriteToLcd(line1String, line2String);
        }

        public override void OnKeyPressLongWarning()
        {
            WriteToLcd("..hold to start", "==>[-");
        }

        public override void OnKeyPressLong()
        {
            RiseStateChangedEvent(new State2Warmup(BrewData));
        }

        public override void OnKeyPressLongCancelled()
        {
            ApplyDefaultScreen();
        }


        public override void Start()
        {
            BrewData.BrewWarmupStart = DateTime.MinValue;
            BrewData.BrewAddGrainStart = DateTime.MinValue;
            BrewData.BrewMashStart = DateTime.MinValue;
            BrewData.BrewMashOutStart = DateTime.MinValue;
            BrewData.BrewSpargeStart = DateTime.MinValue;
            BrewData.BrewSpargeEnd = DateTime.MinValue;
            ApplyDefaultScreen();
        }

        public override void OnKeyPressShort()
        {
            RiseStateChangedEvent(new StateSetupStrikeTemp(BrewData));
        }

        public override string[] GetNewStateIndication(int secondsLeft)
        {
            return null;
        }
    }
}
