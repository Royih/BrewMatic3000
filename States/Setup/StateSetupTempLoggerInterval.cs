
namespace BrewMatic3000.States.Setup
{
    public class StateSetupTempLoggerInterval : State
    {

        private const int MinTime = 10;

        private const int MaxTime = 1000;

        public StateSetupTempLoggerInterval(BrewData brewData)
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
                            "TempLogger interval", 
                            "", 
                            "Current: " + BrewData.Config.TempLoggerLogEveryNSeconds +" sec"
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
            BrewData.Config.TempLoggerLogEveryNSeconds += 10;
            if (BrewData.Config.TempLoggerLogEveryNSeconds > MaxTime)
            {
                BrewData.Config.TempLoggerLogEveryNSeconds = MinTime;
            }
        }

        public override void KeyPressPreviousShort()
        {
            BrewData.Config.TempLoggerLogEveryNSeconds -= 10;
            if (BrewData.Config.TempLoggerLogEveryNSeconds < MinTime)
            {
                BrewData.Config.TempLoggerLogEveryNSeconds = MaxTime;
            }
        }

        public override void KeyPressNextLong()
        {
            RiseStateChangedEvent(new StateSetup(BrewData, new[] { "", "Saved", "", "" }, (int)StateSetup.Screens.TempLoggerLogEveryNSeconds));
        }

    }
}
