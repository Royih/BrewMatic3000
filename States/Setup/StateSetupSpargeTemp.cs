using BrewMatic3000.Extensions;

namespace BrewMatic3000.States.Setup
{
    public class StateSetupSpargeTemp : State
    {

        private const float MinTemp = 70.0f;

        private const float MaxTemp = 90.0f;

        public StateSetupSpargeTemp(BrewData brewData)
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
                            "Sparge temp", 
                            "", 
                            "Current: " + BrewData.Config.SpargeTemperature.DisplayTemperature()
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
            BrewData.Config.SpargeTemperature += 0.1f;
            if (BrewData.Config.SpargeTemperature > MaxTemp)
            {
                BrewData.Config.SpargeTemperature = MinTemp;
            }
        }

        public override void KeyPressPreviousShort()
        {
            BrewData.Config.SpargeTemperature -= 0.1f;
            if (BrewData.Config.SpargeTemperature < MinTemp)
            {
                BrewData.Config.SpargeTemperature = MaxTemp;
            }
        }

        public override void KeyPressNextLong()
        {
            RiseStateChangedEvent(new StateSetup(BrewData, new[] { "", "Saved", "", "" }, (int)StateSetup.Screens.SpargeTemp));
        }

    }
}
