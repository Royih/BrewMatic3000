using BrewMatic3000.Extensions;

namespace BrewMatic3000.States.Setup
{
    public class StateSetupMashTemp : State
    {

        private const float MinTemp = 60.0f;

        private const float MaxTemp = 75.0f;

        public StateSetupMashTemp(BrewData brewData)
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
                            "Mash temp", 
                            "", 
                            "Current: " + BrewData.MashTemperature.DisplayTemperature()
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
            BrewData.MashTemperature += 0.1f;
            if (BrewData.MashTemperature > MaxTemp)
            {
                BrewData.MashTemperature = MinTemp;
            }
        }

        public override void KeyPressPreviousShort()
        {
            BrewData.MashTemperature -= 0.1f;
            if (BrewData.MashTemperature < MinTemp)
            {
                BrewData.MashTemperature = MaxTemp;
            }
        }

        public override void KeyPressNextLong()
        {
            RiseStateChangedEvent(new StateSetup(BrewData, new[] { "", "Saved", "", "" }, 3));
        }

    }
}
