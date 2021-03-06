using BrewMatic3000.Extensions;

namespace BrewMatic3000.States.Setup
{
    public class StateSetupHeat1Effect : State
    {
        public StateSetupHeat1Effect(BrewData brewData)
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
                            "Heat effect 1", 
                            "", 
                            "Current: " + BrewData.Heater1.GetCurrentValue().DisplayHeat()
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
            if (BrewData.Heater1.GetCurrentValue() == 100)
            {
                BrewData.Heater1.SetValue(0);
            }
            else
            {
                BrewData.Heater1.SetValue(BrewData.Heater1.GetCurrentValue() + 10);
            }
        }

        public override void KeyPressPreviousShort()
        {
            if (BrewData.Heater1.GetCurrentValue() == 0)
            {
                BrewData.Heater1.SetValue(100);
            }
            else
            {
                BrewData.Heater1.SetValue(BrewData.Heater1.GetCurrentValue() - 10);
            }
        }

        public override void KeyPressNextLong()
        {
            RiseStateChangedEvent(new StateSetup(BrewData, new[] { "", "Saved", "", "" }));
        }

    }
}
