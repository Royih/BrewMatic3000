using System;
using BrewMatic3000.Extensions;
using BrewMatic3000.States.Setup;

namespace BrewMatic3000.States.Brew
{
    public class State2Warmup : State
    {

        private float _maxTemp1;
        private float _maxTemp2;

        private DateTime _startPIDMash = DateTime.MinValue;
        private DateTime _startPIDSparge = DateTime.MinValue;

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
                        var strLine3 = "";
                        var strLine4 = "";

                        if (BrewData.MashPID.Started())
                        {
                            strLine3 = GetLineString(currentTemp1, BrewData.Config.StrikeTemperature, BrewData.Heater1.GetCurrentValue(), "Ms");
                        }
                        else
                        {
                            strLine3 = "Ms: " + _startPIDMash.Subtract(DateTime.Now).Display();
                        }

                        if (BrewData.SpargePID.Started())
                        {
                            strLine4 = GetLineString(currentTemp2, BrewData.Config.SpargeTemperature,
                                BrewData.Heater2.GetCurrentValue(), "Sp");
                        }
                        else
                        {
                            strLine4 = "Sp: " + _startPIDSparge.Subtract(DateTime.Now).Display();
                        }

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
                        var strLine2 = "Max 1: " + _maxTemp1.DisplayTemperature();
                        var strLine3 = "Max 2: " + _maxTemp2.DisplayTemperature();
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
            if (GetCurrentScreenNumber == (int)Screens.AbortBrew)
            {
                RiseStateChangedEvent(new StateDashboard(BrewData, new[] { "Brew aborted" }));
            }
        }


        protected override void StartExtra()
        {
            BrewData.BrewWarmupStart = DateTime.Now;
            _startPIDMash = BrewData.MashStartTime.AddMinutes((-1) * BrewData.Config.EstimatedMashWarmupMinutes);
            _startPIDSparge = BrewData.MashStartTime.AddMinutes((-1) * BrewData.Config.EstimatedSpargeWarmupMinutes);
        }

        protected override void DoWorkExtra()
        {
            if (!BrewData.MashPID.Started() && DateTime.Now > _startPIDMash)
            {
                BrewData.MashPID.Start(BrewData.Config.StrikeTemperature);
            }
            else if (BrewData.MashPID.Started())
            {
                var currentTemp1 = BrewData.TempReader1.GetValue();
                if (_maxTemp1 < currentTemp1)
                {
                    _maxTemp1 = currentTemp1;
                }
            }

            if (!BrewData.SpargePID.Started() && DateTime.Now > _startPIDSparge)
            {
                BrewData.SpargePID.Start(BrewData.Config.SpargeTemperature);
            }
            else if (BrewData.SpargePID.Started())
            {
                var currentTemp2 = BrewData.TempReader2.GetValue();

                if (_maxTemp2 < currentTemp2)
                {
                    _maxTemp2 = currentTemp2;
                }
            }


        }

        private string GetLineString(float currentTemp, float desiredTemp, float watt, string prefix)
        {
            var currentTempString = currentTemp.ToString("f1").PadLeft(4);
            var desiredTempString = desiredTemp.ToString("f1").PadLeft(4);
            return prefix + ":" + currentTempString.PadLeft(5) + "/" + desiredTempString + " H:" + watt.DisplayHeat(); //Ms: 58.9/68.0 W:100%"
        }

    }
}
