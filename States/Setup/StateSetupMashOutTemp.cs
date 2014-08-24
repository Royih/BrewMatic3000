using BrewMatic3000.Extensions;

namespace BrewMatic3000.States.Setup
{
    public class StateSetupMashOutTemp : State
    {

        private const float MinTemp = 76.0f;

        private const float MaxTemp = 90.0f;

        public StateSetupMashOutTemp(BrewData brewData)
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
                            "Mesh out temp", 
                            "", 
                            "Current: " + BrewData.MashOutTemperature.DisplayTemperature()
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
            BrewData.MashOutTemperature += 0.1f;
            if (BrewData.MashOutTemperature > MaxTemp)
            {
                BrewData.MashOutTemperature = MinTemp;
            }
        }

        public override void KeyPressPreviousShort()
        {
            BrewData.MashOutTemperature -= 0.1f;
            if (BrewData.MashOutTemperature < MinTemp)
            {
                BrewData.MashOutTemperature = MaxTemp;
            }
        }

        public override void KeyPressNextLong()
        {
            RiseStateChangedEvent(new StateSetup(BrewData, new[] { "", "Saved", "", "" }, 2));
        }

    }
}
