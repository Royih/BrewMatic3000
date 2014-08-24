using System;
using BrewMatic3000.Extensions;

namespace BrewMatic3000.States.Brew
{
    public class State3MashAddGrain : State
    {


        public State3MashAddGrain(BrewData brewData, string[] initialMessage = null, int initialScreen = 0)
            : base(brewData, initialMessage, initialScreen)
        {

        }

        public enum Screens
        {
            Default,
            StartMashing,
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
                        var currentTemp1 = BrewData.TempReader1.GetValue();
                        var currentTemp2 = BrewData.TempReader2.GetValue();

                        var preferredMashTemp = BrewData.MashTemperature;
                        var preferredSpargeTemp = BrewData.SpargeTemperature;

                        var pidOutputMash = BrewData.MashPID.GetValue(currentTemp1, preferredMashTemp);
                        BrewData.Heater1.SetValue(pidOutputMash);

                        var pidOutputSparge = BrewData.SpargePID.GetValue(currentTemp2, BrewData.SpargeTemperature);
                        BrewData.Heater2.SetValue(pidOutputSparge);

                        var strLine1 = "= Brew: Add grain  =";
                        var strLine2 = "";
                        var strLine3 = GetLineString(currentTemp1, preferredMashTemp, BrewData.Heater1.GetCurrentValue(), "Ms");
                        var strLine4 = GetLineString(currentTemp2, preferredSpargeTemp, BrewData.Heater2.GetCurrentValue(), "Sp");

                        var longWarningNext = "Start mashing";

                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 }, longWarningNext);
                    }
                case (int)Screens.StartMashing:
                    {
                        var strLine1 = "= Brew: Add grain  =";
                        var strLine2 = "Start mashing";
                        var strLine3 = "";
                        var strLine4 = "";

                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 }, strLine2);
                    }
                case (int)Screens.AbortBrew:
                    {
                        var strLine1 = "= Brew: Add grain  =";
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
                RiseStateChangedEvent(new State4Mash(BrewData));
            }
            if (GetCurrentScreenNumber == (int)Screens.StartMashing)
            {
                RiseStateChangedEvent(new State4Mash(BrewData));
            }
        }

        protected override void StartExtra()
        {
            BrewData.BrewAddGrainStart = DateTime.Now;
        }


        private string GetLineString(float currentTemp, float desiredTemp, float watt, string prefix)
        {
            var currentTempString = currentTemp.ToString("f1").PadLeft(4);
            var desiredTempString = desiredTemp.ToString("f1").PadLeft(4);
            return prefix + ":" + currentTempString.PadLeft(5) + "/" + desiredTempString + " H:" + watt.DisplayHeat(); //Ms: 58.9/68.0 W:100%"
        }

    }
}
