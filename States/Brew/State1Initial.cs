using System;
using BrewMatic3000.Extensions;

namespace BrewMatic3000.States.Brew
{
    public class State1Initial : State
    {
        public State1Initial(BrewData brewData, string[] initialMessage = null, int initialScreen = 0)
            : base(brewData, initialMessage, initialScreen)
        {

        }

        public enum Screens
        {
            Default,
            BeginWarmup,
            AbortBrew
        }

        public override int GetNumberOfScreens()
        {
            return (int)Screens.AbortBrew;
        }


        public override Screen GetScreen(int screenNumber)
        {
            switch (screenNumber)
            {
                case (int)Screens.Default:
                    {
                        var strLine1 = "=  Start new brew  =";
                        var strLine2 = "St: " + BrewData.MashStartTime.DisplayShort();
                        var strLine3 = "Str:" + BrewData.Config.StrikeTemperature.DisplayTemperature() + " Sp:" + BrewData.Config.SpargeTemperature.DisplayTemperature(); //St:70.5|Sp:12.2
                        var strLine4 = "Ms:" + BrewData.Config.MashTemperature.DisplayTemperature() + "  Tm:" + BrewData.Config.MashTime + "min"; //Ms:65.1|Tm:60 

                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 }, "Begin warmup");
                    }
                case (int)Screens.BeginWarmup:
                    {
                        var strLine1 = "=  Start new brew  =";
                        var strLine2 = "Begin warmup";
                        var strLine3 = "";
                        var strLine4 = "";

                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 }, strLine2);
                    }
                case (int)Screens.AbortBrew:
                    {
                        var strLine1 = "=  Start new brew  =";
                        var strLine2 = "Abort brew";
                        var strLine3 = "";
                        var strLine4 = "";

                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 }, strLine2);
                    }
                default:
                    {
                        return GetScreenError(screenNumber);
                    }
            }
        }


        public override void KeyPressNextLong()
        {
            if (GetCurrentScreenNumber == (int)Screens.Default)
            {
                RiseStateChangedEvent(new State2Warmup(BrewData));
            }
            if (GetCurrentScreenNumber == (int)Screens.BeginWarmup)
            {
                RiseStateChangedEvent(new State2Warmup(BrewData));
            }
            if (GetCurrentScreenNumber == (int)Screens.AbortBrew)
            {
                RiseStateChangedEvent(new StateDashboard(BrewData, new[] { "Brew aborted" }));
            }
        }


        protected override void StartExtra()
        {
            BrewData.BrewWarmupStart = DateTime.MinValue;
            BrewData.BrewAddGrainStart = DateTime.MinValue;
            BrewData.BrewMashStart = DateTime.MinValue;
            BrewData.BrewMashOutStart = DateTime.MinValue;
            BrewData.BrewSpargeStart = DateTime.MinValue;
            BrewData.BrewSpargeEnd = DateTime.MinValue;

            BrewData.Heater1.SetValue(0);
            BrewData.Heater2.SetValue(0);
        }


    }
}
