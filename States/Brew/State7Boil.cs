using System;

namespace BrewMatic3000.States.Brew
{
    public class State7Boil : State
    {
        public State7Boil(BrewData brewData, string[] initialMessage = null, int initialScreen = 0)
            : base(brewData, initialMessage, initialScreen)
        {

        }

        public enum Screens
        {
            Default,
            ShowLog,
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
                case (int)Screens.Default:
                    {
                        var strLine1 = "= Brew: Boiling =";
                        var strLine2 = "My wrk is done:)";
                        var strLine3 = "";
                        var strLine4 = "";
                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 });
                    }
                case (int)Screens.ShowLog:
                    {
                        var strLine1 = "= Brew: Boiling =";
                        var strLine2 = "Show brew log";
                        var strLine3 = "";
                        var strLine4 = "";

                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 }, strLine2);
                    }
                case (int)Screens.Return:
                    {
                        var strLine1 = "= Brew: Boiling =";
                        var strLine2 = "Return to dashboard";
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
            if (GetCurrentScreenNumber == (int)Screens.ShowLog)
            {
                RiseStateChangedEvent(new StateShowLog(BrewData));
            }
            if (GetCurrentScreenNumber == (int)Screens.Return)
            {
                RiseStateChangedEvent(new StateDashboard(BrewData));
            }
        }

        protected override void StartExtra()
        {
            BrewData.BrewSpargeEnd = DateTime.Now;
            BrewData.MashPID.Stop();
            BrewData.SpargePID.Stop();
            BrewData.LogBrewEventToFile("Ready for boil");
        }

    }
}
