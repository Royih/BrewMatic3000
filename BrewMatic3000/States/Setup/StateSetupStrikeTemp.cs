using BrewMatic3000.Extensions;

namespace BrewMatic3000.States.Setup
{
    public class StateSetupStrikeTemp : State
    {

        private const float MinTemp = 50.0f;

        private const float MaxTemp = 85.0f;

        public StateSetupStrikeTemp(BrewData brewData)
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
                            "Strike temp", 
                            "", 
                            "Current: " + BrewData.Config.StrikeTemperature.DisplayTemperature()
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
            BrewData.Config.StrikeTemperature += 0.1f;
            if (BrewData.Config.StrikeTemperature > MaxTemp)
            {
                BrewData.Config.StrikeTemperature = MinTemp;
            }
        }

        public override void KeyPressPreviousShort()
        {
            BrewData.Config.StrikeTemperature -= 0.1f;
            if (BrewData.Config.StrikeTemperature < MinTemp)
            {
                BrewData.Config.StrikeTemperature = MaxTemp;
            }
        }

        public override void KeyPressNextLong()
        {
            RiseStateChangedEvent(new StateSetup(BrewData, new[] { "", "Saved", "", "" }, (int)StateSetup.Screens.StrikeTemp));
        }

    }
}
