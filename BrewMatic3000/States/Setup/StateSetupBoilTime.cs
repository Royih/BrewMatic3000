namespace BrewMatic3000.States.Setup
{
    public class StateSetupBoilTime : State
    {

        private const int MinTime = 60;

        private const int MaxTime = 90;

        public StateSetupBoilTime(BrewData brewData)
            : base(brewData)
        {

        }

        public enum Screens
        {
            Default,
        }

        public override int GetNumberOfScreens()
        {
            return (int)Screens.Default;
        }

        public override Screen GetScreen(int screenNumber)
        {
            switch (screenNumber)
            {
                case (int)Screens.Default:
                    {
                        return new Screen(screenNumber, new[]
                        {
                            "=  Setup  =", 
                            "Boil time", 
                            "", 
                            "Current: " + BrewData.Config.BoilTime +"min"
                        }, "Save");
                    }
                default:
                    {
                        return GetScreenError(screenNumber);
                    }
            }
        }

        public override void KeyPressNextShort()
        {
            BrewData.Config.BoilTime += 1;
            if (BrewData.Config.BoilTime > MaxTime)
            {
                BrewData.Config.BoilTime = MinTime;
            }
        }

        public override void KeyPressPreviousShort()
        {
            BrewData.Config.BoilTime -= 1;
            if (BrewData.Config.BoilTime < MinTime)
            {
                BrewData.Config.BoilTime = MaxTime;
            }
        }

        public override void KeyPressNextLong()
        {
            RiseStateChangedEvent(new StateSetup(BrewData, new[] { "", "Saved", "", "" }, (int)StateSetup.Screens.BoilTime));
        }

    }
}
