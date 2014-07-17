using BrewMatic3000.RealHW;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using Kobush.NETMF.Hardware.LCD;

namespace BrewMatic3000
{
    public class Program
    {
        public static void Main()
        {
            var pushButton = new InterruptPort(Pins.GPIO_PIN_D6, true, ResistorModes.PullDown, Port.InterruptMode.InterruptEdgeBoth);

            var tempInput = new SecretLabs.NETMF.Hardware.AnalogInput(Pins.GPIO_PIN_A5);

            var portHeater1 = new OutputPort(Pins.GPIO_PIN_D8, false);
            var portHeater2 = new OutputPort(Pins.GPIO_PIN_D9, false);

            // create the transfer provider
            var lcdProvider = new GpioLiquidCrystalTransferProvider(Pins.GPIO_PIN_D0, Pins.GPIO_PIN_D1, Pins.GPIO_PIN_D2, Pins.GPIO_PIN_D3, Pins.GPIO_PIN_D4, Pins.GPIO_PIN_D5);

            // create the LCD interface
            var lcd = new LiquidCrystal(lcdProvider);

            var program = new BrewGuide(pushButton, lcd, new PT100Reader(tempInput), portHeater1, portHeater2);
            program.Initialize();
            program.Run();
        }



    }
}
