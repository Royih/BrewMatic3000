using System;
using BrewMatic3000.Extensions;

namespace BrewMatic3000.States.Brew
{
    public class State2Warmup : State
    {

        private float _maxTemp1;
        private float _maxTemp2;

        public State2Warmup(BrewData brewData, string[] initialMessage = null, int initialScreen = 0)
            : base(brewData, initialMessage, initialScreen)
        {

        }


        public enum Screens
        {
            Default,
            AddGrain,
            MinMax,
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

                        var strLine1 = "= Brew: Warmup =";
                        var strLine2 = "";
                        var strLine3 = GetLineString(currentTemp1, BrewData.StrikeTemperature, BrewData.Heater1.GetCurrentValue(), "Ms");
                        var strLine4 = GetLineString(currentTemp2, BrewData.SpargeTemperature, BrewData.Heater2.GetCurrentValue(), "Sp");


                        var longWarningNext = "Add grain";

                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 }, longWarningNext);
                    }
                case (int)Screens.AddGrain:
                    {
                        var strLine1 = "= Brew: Warmup =";
                        var strLine2 = "Add grain";
                        var strLine3 = "";
                        var strLine4 = "";

                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 }, strLine2);
                    }
                case (int)Screens.MinMax:
                    {
                        var strLine1 = "= Brew: Warmup =";
                        var strLine2 = "Min: " + _maxTemp1.DisplayTemperature();
                        var strLine3 = "Max: " + _maxTemp2.DisplayTemperature();
                        var strLine4 = "";

                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 }, strLine2);
                    }
                case (int)Screens.AbortBrew:
                    {
                        var strLine1 = "= Brew: Warmup =";
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
                RiseStateChangedEvent(new State3MashAddGrain(BrewData));
            }
            if (GetCurrentScreenNumber == (int)Screens.AddGrain)
            {
                RiseStateChangedEvent(new State3MashAddGrain(BrewData));
            }
        }


        protected override void StartExtra()
        {
            BrewData.BrewWarmupStart = DateTime.Now;
        }

        protected override void DoWorkExtra()
        {
            var currentTemp1 = BrewData.TempReader1.GetValue();
            var currentTemp2 = BrewData.TempReader2.GetValue();

            //keep the highest temp (for pid tuning purposes)
            if (_maxTemp1 < currentTemp1)
            {
                _maxTemp1 = currentTemp1;
            }
            if (_maxTemp2 < currentTemp2)
            {
                _maxTemp2 = currentTemp2;
            }

            var pidOutputMash = BrewData.MashPID.GetValue(currentTemp1, BrewData.StrikeTemperature);
            BrewData.Heater1.SetValue(pidOutputMash);

            var pidOutputSparge = BrewData.SpargePID.GetValue(currentTemp2, BrewData.SpargeTemperature);
            BrewData.Heater2.SetValue(pidOutputSparge);
        }

        private string GetLineString(float currentTemp, float desiredTemp, float watt, string prefix)
        {
            var currentTempString = currentTemp.ToString("f1").PadLeft(4);
            var desiredTempString = desiredTemp.ToString("f1").PadLeft(4);
            return prefix + ":" + currentTempString.PadLeft(5) + "/" + desiredTempString + " H:" + watt.DisplayHeat(); //Ms: 58.9/68.0 W:100%"
        }

    }
}
