using BrewMatic3000.Extensions;

namespace BrewMatic3000.States.Setup
{
    public class StateSetupHeat2Effect : State
    {
        public StateSetupHeat2Effect(BrewData brewData)
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
                            "Heat effect 2", 
                            "", 
                            "Current: " + BrewData.Heater2.GetCurrentValue().DisplayHeat()
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
            if (BrewData.Heater2.GetCurrentValue().Equals(100f))
            {
                BrewData.Heater2.SetValue(0);
            }
            else
            {
                BrewData.Heater2.SetValue(BrewData.Heater2.GetCurrentValue() + 10);
            }
        }

        public override void KeyPressPreviousShort()
        {
            if (BrewData.Heater2.GetCurrentValue().Equals(0f))
            {
                BrewData.Heater2.SetValue(100);
            }
            else
            {
                BrewData.Heater2.SetValue(BrewData.Heater2.GetCurrentValue() - 10);
            }
        }

        public override void KeyPressNextLong()
        {
            RiseStateChangedEvent(new StateSetup(BrewData, new[] { "", "Saved", "", "" }, 1));
        }

    }
}
