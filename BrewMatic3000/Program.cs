using BrewMatic3000.RealHW;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace BrewMatic3000
{
    public class Program
    {
        public static void Main_Old()
        {
            var pushButtonPrev = new InputPort(Pins.GPIO_PIN_D6, true, Port.ResistorMode.PullDown);
            var pushButtonNext = new InputPort(Pins.GPIO_PIN_D5, true, Port.ResistorMode.PullDown);

            var tempInput1 = new SecretLabs.NETMF.Hardware.AnalogInput(Pins.GPIO_PIN_A5);
            var tempInput2 = new SecretLabs.NETMF.Hardware.AnalogInput(Pins.GPIO_PIN_A4);

            var portHeater1 = new OutputPort(Pins.GPIO_PIN_D8, false);
            var portHeater2 = new OutputPort(Pins.GPIO_PIN_D9, false);


            var lcd = new LiquidCrystal_I2C(0x27, 20, 4);
            lcd.setBacklight(true);


            var navButtons = new NavigateButtons(pushButtonPrev, pushButtonNext);

            var program = new BrewGuide(navButtons, lcd, new PT100Reader(tempInput1), new PT100Reader(tempInput2), portHeater1, portHeater2);
            program.Initialize();
            program.Run();

        }

        public static void Main()
        {
            var pushButtonPrev = new InputPort(Pins.GPIO_PIN_D6, true, Port.ResistorMode.PullDown);
            var pushButtonNext = new InputPort(Pins.GPIO_PIN_D5, true, Port.ResistorMode.PullDown);

            var tempInput1 = new SecretLabs.NETMF.Hardware.AnalogInput(Pins.GPIO_PIN_A5);
            var tempInput2 = new SecretLabs.NETMF.Hardware.AnalogInput(Pins.GPIO_PIN_A4);

            var portHeater1 = new OutputPort(Pins.GPIO_PIN_D8, false);
            var portHeater2 = new OutputPort(Pins.GPIO_PIN_D9, false);


            var lcd = new LiquidCrystal_I2C(0x27, 20, 4);
            lcd.setBacklight(true);


            var navButtons = new NavigateButtons(pushButtonPrev, pushButtonNext);

            var program = new SlaveMode(navButtons, lcd, new PT100Reader(tempInput1), new PT100Reader(tempInput2), portHeater1, portHeater2);
            program.Initialize();
            program.Run();

        }

    }
}
