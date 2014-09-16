namespace BrewMatic3000.States.Setup
{
    public class StateSetupMashTime : State
    {

        private const int MinTemp = 50;

        private const int MaxTemp = 95;

        public StateSetupMashTime(BrewData brewData)
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
                            "Mash time", 
                            "", 
                            "Current: " + BrewData.Config.MashTime +"min"
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
            BrewData.Config.MashTime += 1;
            if (BrewData.Config.MashTime > MaxTemp)
            {
                BrewData.Config.MashTime = MinTemp;
            }
        }

        public override void KeyPressPreviousShort()
        {
            BrewData.Config.MashTime -= 1;
            if (BrewData.Config.MashTime < MinTemp)
            {
                BrewData.Config.MashTime = MaxTemp;
            }
        }

        public override void KeyPressNextLong()
        {
            RiseStateChangedEvent(new StateSetup(BrewData, new[] { "", "Saved", "", "" }, (int)StateSetup.Screens.MashTime));
        }

    }
}
