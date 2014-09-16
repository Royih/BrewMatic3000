
namespace BrewMatic3000.States.Setup
{
    public class StateSetupEstimatedSpargeWarmupTime : State
    {

        private const int MinTime = 30;

        private const int MaxTime = 100;

        public StateSetupEstimatedSpargeWarmupTime(BrewData brewData)
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
                            "Est. Spg. warmup tm", 
                            "", 
                            "Current: " + BrewData.Config.EstimatedSpargeWarmupMinutes +"min"
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
            BrewData.Config.EstimatedSpargeWarmupMinutes += 1;
            if (BrewData.Config.EstimatedSpargeWarmupMinutes > MaxTime)
            {
                BrewData.Config.EstimatedSpargeWarmupMinutes = MinTime;
            }
        }

        public override void KeyPressPreviousShort()
        {
            BrewData.Config.EstimatedSpargeWarmupMinutes -= 1;
            if (BrewData.Config.EstimatedSpargeWarmupMinutes < MinTime)
            {
                BrewData.Config.EstimatedSpargeWarmupMinutes = MaxTime;
            }
        }

        public override void KeyPressNextLong()
        {
            RiseStateChangedEvent(new StateSetup(BrewData, new[] { "", "Saved", "", "" }, (int)StateSetup.Screens.EstimatedSpargeWarmupTime));
        }

    }
}
