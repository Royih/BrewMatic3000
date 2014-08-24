﻿
using BrewMatic3000.Extensions;

namespace BrewMatic3000.States.Setup
{
    public class StateSetup : State
    {
        public StateSetup(BrewData brewData, string[] initialMessage = null, int initialScreen = 0)
            : base(brewData, initialMessage, initialScreen)
        {
        }

        public enum Screens
        {
            HeatEffect1,
            HeatEffect2,
            MashOutTemp,
            MashTemp,
            MashTime,
            SpargeTemp,
            StrikeTemp,
            Return
        }

        public override int GetNumberOfScreens()
        {
            return (int)Screens.Return;
        }

        public override Screen GetScreen(int screenNumber)
        {
            switch (screenNumber)
            {
                case (int)Screens.HeatEffect1:
                    {
                        var line1 = "=  Setup  =";
                        var line2 = "";
                        var line3 = "Heat effect 1";
                        var line4 = "Current: " + BrewData.Heater1.GetCurrentValue().DisplayHeat();
                        return new Screen(screenNumber, new[] { line1, line2, line3, line4 }, line3);
                    }
                case (int)Screens.HeatEffect2:
                    {
                        var line1 = "=  Setup  =";
                        var line2 = "";
                        var line3 = "Heat effect 2";
                        var line4 = "Current: " + BrewData.Heater2.GetCurrentValue().DisplayHeat();
                        return new Screen(screenNumber, new[] { line1, line2, line3, line4 }, line3);
                    }
                case (int)Screens.MashOutTemp:
                    {
                        var line1 = "=  Setup  =";
                        var line2 = "";
                        var line3 = "Mash out temp";
                        var line4 = "Current: " + BrewData.MashOutTemperature.DisplayTemperature();
                        return new Screen(screenNumber, new[] { line1, line2, line3, line4 }, line3);
                    }
                case (int)Screens.MashTemp:
                    {
                        var line1 = "=  Setup  =";
                        var line2 = "";
                        var line3 = "Mash temp";
                        var line4 = "Current: " + BrewData.MashTemperature.DisplayTemperature();
                        return new Screen(screenNumber, new[] { line1, line2, line3, line4 }, line3);
                    }
                case (int)Screens.MashTime:
                    {
                        var line1 = "=  Setup  =";
                        var line2 = "";
                        var line3 = "Mash time";
                        var line4 = "Current: " + BrewData.MashTime + "min";
                        return new Screen(screenNumber, new[] { line1, line2, line3, line4 }, line3);
                    }
                case (int)Screens.SpargeTemp:
                    {
                        var line1 = "=  Setup  =";
                        var line2 = "";
                        var line3 = "Sparge temp";
                        var line4 = "Current: " + BrewData.SpargeTemperature.DisplayTemperature();
                        return new Screen(screenNumber, new[] { line1, line2, line3, line4 }, line3);
                    }
                case (int)Screens.StrikeTemp:
                    {
                        var line1 = "=  Setup  =";
                        var line2 = "";
                        var line3 = "Strike temp";
                        var line4 = "Current: " + BrewData.StrikeTemperature.DisplayTemperature();
                        return new Screen(screenNumber, new[] { line1, line2, line3, line4 }, line3);
                    }
                case (int)Screens.Return:
                    {
                        var line1 = "=  Setup  =";
                        var line2 = "";
                        var line3 = "Return to dashboard";
                        var line4 = "";
                        return new Screen(screenNumber, new[] { line1, line2, line3, line4 }, "");
                    }

                default:
                    {
                        return GetScreenError(screenNumber);
                    }
            }
        }

        public override void KeyPressNextLong()
        {
            if (GetCurrentScreenNumber == (int)Screens.HeatEffect1)
            {
                RiseStateChangedEvent(new StateSetupHeat1Effect(BrewData));
            }
            if (GetCurrentScreenNumber == (int)Screens.HeatEffect2)
            {
                RiseStateChangedEvent(new StateSetupHeat2Effect(BrewData));
            }
            if (GetCurrentScreenNumber == (int)Screens.MashOutTemp)
            {
                RiseStateChangedEvent(new StateSetupMashOutTemp(BrewData));
            }
            if (GetCurrentScreenNumber == (int)Screens.MashTemp)
            {
                RiseStateChangedEvent(new StateSetupMashTemp(BrewData));
            }
            if (GetCurrentScreenNumber == (int)Screens.MashTime)
            {
                RiseStateChangedEvent(new StateSetupMashTime(BrewData));
            }
            if (GetCurrentScreenNumber == (int)Screens.SpargeTemp)
            {
                RiseStateChangedEvent(new StateSetupSpargeTemp(BrewData));
            }
            if (GetCurrentScreenNumber == (int)Screens.StrikeTemp)
            {
                RiseStateChangedEvent(new StateSetupStrikeTemp(BrewData));
            }
            if (GetCurrentScreenNumber == (int)Screens.Return)
            {
                RiseStateChangedEvent(new StateDashboard(BrewData));
            }
        }

    }
}
