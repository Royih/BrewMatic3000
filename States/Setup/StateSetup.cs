
using System;
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
            MashStartTime,
            EstimatedMashWarmupTime,
            EstimatedSpargeWarmupTime,
            Time,
            SaveConfig,
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
                        var line4 = "Current: " + BrewData.Config.MashOutTemperature.DisplayTemperature();
                        return new Screen(screenNumber, new[] { line1, line2, line3, line4 }, line3);
                    }
                case (int)Screens.MashTemp:
                    {
                        var line1 = "=  Setup  =";
                        var line2 = "";
                        var line3 = "Mash temp";
                        var line4 = "Current: " + BrewData.Config.MashTemperature.DisplayTemperature();
                        return new Screen(screenNumber, new[] { line1, line2, line3, line4 }, line3);
                    }
                case (int)Screens.MashTime:
                    {
                        var line1 = "=  Setup  =";
                        var line2 = "";
                        var line3 = "Mash time";
                        var line4 = "Current: " + BrewData.Config.MashTime + "min";
                        return new Screen(screenNumber, new[] { line1, line2, line3, line4 }, line3);
                    }
                case (int)Screens.SpargeTemp:
                    {
                        var line1 = "=  Setup  =";
                        var line2 = "";
                        var line3 = "Sparge temp";
                        var line4 = "Current: " + BrewData.Config.SpargeTemperature.DisplayTemperature();
                        return new Screen(screenNumber, new[] { line1, line2, line3, line4 }, line3);
                    }
                case (int)Screens.StrikeTemp:
                    {
                        var line1 = "=  Setup  =";
                        var line2 = "";
                        var line3 = "Strike temp";
                        var line4 = "Current: " + BrewData.Config.StrikeTemperature.DisplayTemperature();
                        return new Screen(screenNumber, new[] { line1, line2, line3, line4 }, line3);
                    }
                case (int)Screens.MashStartTime:
                    {
                        var line1 = "=  Setup  =";
                        var line2 = "";
                        var line3 = "Mash start time";
                        var line4 = BrewData.MashStartTime.ToString("yyyy MMM dd HH:mm:ss");
                        return new Screen(screenNumber, new[] { line1, line2, line3, line4 }, line3);
                    }
                case (int)Screens.EstimatedMashWarmupTime:
                    {
                        var line1 = "=  Setup  =";
                        var line2 = "";
                        var line3 = "Est. Msh. warmup tm";
                        var line4 = "Current: " + BrewData.Config.EstimatedMashWarmupMinutes;
                        return new Screen(screenNumber, new[] { line1, line2, line3, line4 }, line3);
                    }
                case (int)Screens.EstimatedSpargeWarmupTime:
                    {
                        var line1 = "=  Setup  =";
                        var line2 = "";
                        var line3 = "Est. Spg. warmup tm";
                        var line4 = "Current: " + BrewData.Config.EstimatedSpargeWarmupMinutes;
                        return new Screen(screenNumber, new[] { line1, line2, line3, line4 }, line3);
                    }
                case (int)Screens.Time:
                    {
                        var line1 = "=  Setup  =";
                        var line2 = "";
                        var line3 = "Time and date";
                        var line4 = DateTime.Now.ToString("yyyy MMM dd HH:mm:ss");
                        return new Screen(screenNumber, new[] { line1, line2, line3, line4 }, line3);
                    }
                case (int)Screens.SaveConfig:
                    {
                        var line1 = "=  Setup  =";
                        var line2 = "";
                        var line3 = "Save config";
                        var line4 = "";
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

        protected override void StartExtra()
        {
            if (GetCurrentScreenNumber == (int)Screens.Time)
            {
                BrewData.TimeChip.SetDateTime(DateTime.Now);
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
            if (GetCurrentScreenNumber == (int)Screens.MashStartTime)
            {
                RiseStateChangedEvent(new StateSetupMashStartTime(BrewData));
            }
            if (GetCurrentScreenNumber == (int)Screens.EstimatedMashWarmupTime)
            {
                RiseStateChangedEvent(new StateSetupEstimatedMashWarmupTime(BrewData));
            }
            if (GetCurrentScreenNumber == (int)Screens.EstimatedSpargeWarmupTime)
            {
                RiseStateChangedEvent(new StateSetupEstimatedSpargeWarmupTime(BrewData));
            }
            if (GetCurrentScreenNumber == (int)Screens.Time)
            {
                RiseStateChangedEvent(new StateSetupTime(BrewData));
            }
            if (GetCurrentScreenNumber == (int)Screens.SaveConfig)
            {
                BrewData.Config.SaveConfig();
                RiseStateChangedEvent(new StateDashboard(BrewData));
            }
            if (GetCurrentScreenNumber == (int)Screens.Return)
            {
                RiseStateChangedEvent(new StateDashboard(BrewData));
            }

        }

    }
}
