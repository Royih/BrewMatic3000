using System;
using BrewMatic3000.Extensions;

namespace BrewMatic3000.States.Brew
{
    public class State7Boil : State
    {
        
        private DateTime _boilComplete;

        public State7Boil(BrewData brewData, string[] initialMessage = null, int initialScreen = 0)
            : base(brewData, initialMessage, initialScreen)
        {

        }

        public enum Screens
        {
            Default,
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
                        var strLine2 = "";
                        var strLine3 = "Timer: " + _boilComplete.Subtract(DateTime.Now).Display();
                        var strLine4 = "";
                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 });
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
            if (GetCurrentScreenNumber == (int)Screens.Default)
            {
                RiseStateChangedEvent(new State8Cooldown(BrewData));
            }
            if (GetCurrentScreenNumber == (int)Screens.Return)
            {
                RiseStateChangedEvent(new StateDashboard(BrewData));
            }
        }

        protected override void StartExtra()
        {
            BrewData.BrewBoilStart = DateTime.Now;
            _boilComplete = DateTime.Now.AddMinutes(BrewData.Config.BoilTime);
            BrewData.MashPID.Stop();
            BrewData.SpargePID.Stop();
            BrewData.Heater1.SetValue(0);
            BrewData.Heater2.SetValue(0);
            BrewData.LogBrewEventToFile("Start boiling");
        }

    }
}
