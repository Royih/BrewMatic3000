using System;
using BrewMatic3000.Extensions;

namespace BrewMatic3000.States.Brew
{
    public class State4Mash : State
    {
        private DateTime _mashComplete;

        public State4Mash(BrewData brewData, string[] initialMessage = null, int initialScreen = 0)
            : base(brewData, initialMessage, initialScreen)
        {

        }

        public enum Screens
        {
            Default,
            MashComplete,
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
                        
                        var longWarningNext = "Mash complete";

                        var strLine1 = "= Brew: Mash =";
                        var strLine2 = "";
                        var strLine3 = "Timer: " + _mashComplete.Subtract(DateTime.Now).Display();
                        var strLine4 = "Tg:" + BrewData.MashPID.GetPreferredTemperature.ToString("f1").PadLeft(4) + " Ac:" + currentTemp1.ToString("f1").PadLeft(4);
                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 }, longWarningNext);
                    }
                case (int)Screens.MashComplete:
                    {
                        var strLine1 = "= Brew: Mash =";
                        var strLine2 = "Mash complete";
                        var strLine3 = "";
                        var strLine4 = "";

                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 }, strLine2);
                    }
                case (int)Screens.AbortBrew:
                    {
                        var strLine1 = "= Brew: Mash =";
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
                RiseStateChangedEvent(new State5Mashout(BrewData));
            }
            if (GetCurrentScreenNumber == (int)Screens.MashComplete)
            {
                RiseStateChangedEvent(new State5Mashout(BrewData));
            }
        }


        protected override void StartExtra()
        {
            BrewData.BrewMashStart = DateTime.Now;
            _mashComplete = DateTime.Now.AddMinutes(BrewData.MashTime);
            BrewData.MashPID.Start(BrewData.MashTemperature);
            BrewData.SpargePID.Start(BrewData.SpargeTemperature);
        }

    }
}
