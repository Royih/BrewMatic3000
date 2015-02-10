using System;
using BrewMatic3000.Extensions;
using BrewMatic3000.States.Brew;
using BrewMatic3000.States.Setup;

namespace BrewMatic3000.States
{
    public class StateDashboard : State
    {
        public StateDashboard(BrewData brewData, string[] initialMessage = null, int initialScreen = 0)
            : base(brewData, initialMessage, initialScreen)
        {

        }

        public enum Screens
        {
            Default,
            Heater,
            StartBrew,
            TurnOffHeat,
            TempLogger,
            BrewLog,
            Setup
        }

        private const string TurnOffHeatWarning = "Turn off heaters?";

        public override int GetNumberOfScreens()
        {
            return (int)Screens.Setup;
        }

        public override Screen GetScreen(int screenNumber)
        {
            switch (screenNumber)
            {
                case (int)Screens.Default:
                    {
                        var strLine1 = "=  BrewMatic 3000  =";
                        var strLine2 = "";
                        var strLine3 = "T1:" + BrewData.TempReader1.GetValue().DisplayTemperature().PadRight(8) + "T2:" + BrewData.TempReader2.GetValue().DisplayTemperature();
                        var strLine4 = DateTime.Now.Display();
                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 });
                    }
                case (int)Screens.Heater:
                    {
                        var strLine1 = "=  BrewMatic 3000  =";
                        var strLine2 = "";
                        var strLine3 = "H1:" + BrewData.Heater1.GetCurrentValue().DisplayHeat().PadRight(8) + "H2:" + BrewData.Heater2.GetCurrentValue().DisplayHeat();
                        var strLine4 = "";
                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 }, TurnOffHeatWarning);
                    }
                case (int)Screens.StartBrew:
                    {
                        var strLine1 = "=  Start new brew  =";
                        var strLine2 = "St: " + BrewData.MashStartTime.DisplayShort();
                        var strLine3 = "Str:" + BrewData.Config.StrikeTemperature.DisplayTemperature() + " Sp:" + BrewData.Config.SpargeTemperature.DisplayTemperature(); //St:70.5|Sp:12.2
                        var strLine4 = "Ms:" + BrewData.Config.MashTemperature.DisplayTemperature() + "  Tm:" + BrewData.Config.MashTime + "min"; //Ms:65.1|Tm:60 

                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 }, "Start new brew");
                    }
                case (int)Screens.TurnOffHeat:
                    {
                        var strLine1 = "=  BrewMatic 3000  =";
                        var strLine2 = "";
                        var strLine3 = "Turn off heat";
                        var strLine4 = "";
                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 }, strLine3);
                    }
                case (int)Screens.TempLogger:
                    {
                        var strLine1 = "=  BrewMatic 3000  =";
                        var strLine2 = "";
                        var strLine3 = "Temp logger";
                        var strLine4 = "";
                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 }, strLine3);
                    }
                case (int)Screens.BrewLog:
                    {
                        var strLine1 = "=  BrewMatic 3000  =";
                        var strLine2 = "";
                        var strLine3 = "Show Brew log";
                        var strLine4 = "";
                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 }, strLine3);
                    }
                case (int)Screens.Setup:
                    {
                        var strLine1 = "=  BrewMatic 3000  =";
                        var strLine2 = "";
                        var strLine3 = "Setup";
                        var strLine4 = "";
                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 }, strLine3);
                    }
                default:
                    {
                        return GetScreenError(screenNumber);
                    }
            }
        }

        protected override void DoWorkExtra()
        {

        }

        protected override void StartExtra()
        {
            BrewData.MashPID.Stop();
            BrewData.SpargePID.Stop();
        }

        public override void KeyPressNextLong()
        {
            if (GetCurrentScreenNumber == (int)Screens.Heater)
            {
                BrewData.Heater1.SetValue(0);
                BrewData.Heater2.SetValue(0);
            }
            if (GetCurrentScreenNumber == (int)Screens.StartBrew)
            {
                RiseStateChangedEvent(new State1Initial(BrewData));
            }
            if (GetCurrentScreenNumber == (int)Screens.TurnOffHeat)
            {
                BrewData.Heater1.SetValue(0);
                BrewData.Heater2.SetValue(0);
                SetScreen((int)Screens.Heater);
            }
            if (GetCurrentScreenNumber == (int)Screens.TempLogger)
            {
                RiseStateChangedEvent(new TempLogger.TempLogger(BrewData));
            }
            if (GetCurrentScreenNumber == (int)Screens.Setup)
            {
                RiseStateChangedEvent(new StateSetup(BrewData));
            }
            if (GetCurrentScreenNumber == (int)Screens.BrewLog)
            {
                RiseStateChangedEvent(new StateShowLog(BrewData));
            }

        }





    }
}
