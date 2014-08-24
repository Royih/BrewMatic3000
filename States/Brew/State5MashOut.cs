using System;
using BrewMatic3000.Extensions;

namespace BrewMatic3000.States.Brew
{
    public class State5Mashout : State
    {

        public State5Mashout(BrewData brewData, string[] initialMessage = null, int initialScreen = 0)
            : base(brewData, initialMessage, initialScreen)
        {

        }

        public enum Screens
        {
            Default,
            MashOutComplete,
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

                        var preferredMashOutTemp = BrewData.MashOutTemperature;
                        var preferredSpargeTemp = BrewData.SpargeTemperature;


                        var pidOutputMash = BrewData.MashPID.GetValue(currentTemp1, preferredMashOutTemp);
                        BrewData.Heater1.SetValue(pidOutputMash);

                        var pidOutputSparge = BrewData.SpargePID.GetValue(currentTemp2, preferredSpargeTemp);
                        BrewData.Heater2.SetValue(pidOutputSparge);

                        var longWarningNext = "Mash out complete";

                        var strLine1 = "= Brew: Mash out =";
                        var strLine2 = "";
                        var strLine3 = "Tg:" + preferredMashOutTemp.ToString("f1").PadLeft(4) + " Ac:" + currentTemp1.ToString("f1").PadLeft(4);
                        var strLine4 = "H:" + (int)BrewData.Heater1.GetCurrentValue() + "%";

                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 }, longWarningNext);
                    }
                case (int)Screens.MashOutComplete:
                    {
                        var strLine1 = "= Brew: Mash out =";
                        var strLine2 = "Mash out complete";
                        var strLine3 = "";
                        var strLine4 = "";

                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 }, strLine2);
                    }
                case (int)Screens.AbortBrew:
                    {
                        var strLine1 = "= Brew: Mash out =";
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
                RiseStateChangedEvent(new State6Sparge(BrewData));
            }
            if (GetCurrentScreenNumber == (int)Screens.MashOutComplete)
            {
                RiseStateChangedEvent(new State6Sparge(BrewData));
            }
        }



        protected override void StartExtra()
        {
            BrewData.BrewMashOutStart = DateTime.Now;
        }



    }
}
