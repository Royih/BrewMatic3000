using System;
using BrewMatic3000.Extensions;

namespace BrewMatic3000.States.Brew
{
    public class State6Sparge : State
    {
        public State6Sparge(BrewData brewData, string[] initialMessage = null, int initialScreen = 0)
            : base(brewData, initialMessage, initialScreen)
        {

        }

        public enum Screens
        {
            Default,
            SpargeComplete,
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
                        var longWarningNext = "..sparge complete";

                        var strLine1 = "= Brew: Sparge =";
                        var strLine2 = "";
                        var strLine3 = "Timer:" + DateTime.Now.Subtract(BrewData.BrewSpargeStart).Display();
                        var strLine4 = "";

                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 }, longWarningNext);
                    }
                case (int)Screens.SpargeComplete:
                    {
                        var strLine1 = "= Brew: Sparge =";
                        var strLine2 = "Sparge complete";
                        var strLine3 = "";
                        var strLine4 = "";

                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 }, strLine2);
                    }
                case (int)Screens.AbortBrew:
                    {
                        var strLine1 = "= Brew: Sparge =";
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
                RiseStateChangedEvent(new State7Boil(BrewData));
            }
            if (GetCurrentScreenNumber == (int)Screens.SpargeComplete)
            {
                RiseStateChangedEvent(new State7Boil(BrewData));
            }
            if (GetCurrentScreenNumber == (int)Screens.AbortBrew)
            {
                RiseStateChangedEvent(new StateDashboard(BrewData, new[] { "Brew aborted" }));
            }
        }



        protected override void StartExtra()
        {
            BrewData.BrewSpargeStart = DateTime.Now;
            BrewData.MashPID.Stop();
            BrewData.SpargePID.Stop();
        }

    }
}
