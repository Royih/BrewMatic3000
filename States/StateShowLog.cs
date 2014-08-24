using BrewMatic3000.Extensions;

namespace BrewMatic3000.States
{
    public class StateShowLog : State
    {

        public StateShowLog(BrewData brewData, string[] initialMessage = null, int initialScreen = 0)
            : base(brewData, initialMessage, initialScreen)
        {

        }


        public enum Screens
        {
            Screen1,
            Screen2,
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
                case (int)Screens.Screen1:
                    {
                        var strLine1 = "=Brew Log p1 =";
                        var strLine2 = "Warmup:     " +BrewData.BrewAddGrainStart.Subtract(BrewData.BrewWarmupStart).Display();
                        var strLine3 = "Add Grain:  " + BrewData.BrewMashStart.Subtract(BrewData.BrewAddGrainStart).Display();
                        var strLine4 = "Mashing:    " + BrewData.BrewMashOutStart.Subtract(BrewData.BrewMashStart).Display();
                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 });
                    }
                case (int)Screens.Screen2:
                    {
                        var strLine1 = "=Brew Log p2 =";
                        var strLine2 = "Mash out:   " + BrewData.BrewSpargeStart.Subtract(BrewData.BrewMashOutStart).Display();
                        var strLine3 = "Sparge:     " + BrewData.BrewSpargeEnd.Subtract(BrewData.BrewSpargeStart).Display();
                        var strLine4 = "Total:      " + BrewData.BrewSpargeEnd.Subtract(BrewData.BrewWarmupStart).Display();
                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 });
                    }
                case (int)Screens.Return:
                    {
                        var line1 = "=  Brew Log  =";
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
            if (GetCurrentScreenNumber == (int)Screens.Return)
            {
                RiseStateChangedEvent(new StateDashboard(BrewData));
            }
        }

    }
}
