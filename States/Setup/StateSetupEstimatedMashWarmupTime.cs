using BrewMatic3000.Extensions;

namespace BrewMatic3000.States.Setup
{
    public class StateSetupEstimatedMashWarmupTime : State
    {

        private const int MinTime = 30;

        private const int MaxTime = 100;

        public StateSetupEstimatedMashWarmupTime(BrewData brewData)
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
                            "Est. Msh. warmup tm", 
                            "", 
                            "Current: " + BrewData.EstimatedMashWarmupMinutes +"min"
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
            BrewData.EstimatedMashWarmupMinutes += 1;
            if (BrewData.EstimatedMashWarmupMinutes > MaxTime)
            {
                BrewData.EstimatedMashWarmupMinutes = MinTime;
            }
        }

        public override void KeyPressPreviousShort()
        {
            BrewData.EstimatedMashWarmupMinutes -= 1;
            if (BrewData.EstimatedMashWarmupMinutes < MinTime)
            {
                BrewData.EstimatedMashWarmupMinutes = MaxTime;
            }
        }

        public override void KeyPressNextLong()
        {
            RiseStateChangedEvent(new StateSetup(BrewData, new[] { "", "Saved", "", "" }, (int)StateSetup.Screens.EstimatedMashWarmupTime));
        }

    }
}
