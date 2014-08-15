using BrewMatic3000.Extensions;
using BrewMatic3000.RealHW;
using BrewMatic3000.States;
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace BrewMatic3000
{
    public class BrewGuide
    {

        private LiquidCrystal_I2C Lcd { get; set; }

        private NavigateButton NavButtonManager { get; set; }

        private PT100Reader TempReader1 { get; set; }

        private PT100Reader TempReader2 { get; set; }

        private OutputPort PortHeater1 { get; set; }

        private HeatElement3000W Heater1 { get; set; }

        private OutputPort PortHeater2 { get; set; }

        private HeatElement3000W Heater2 { get; set; }

        private State _currentState;

        public BrewGuide(InterruptPort pushButton, LiquidCrystal_I2C lcd, PT100Reader tempReader1, PT100Reader tempReader2, OutputPort portHeater1, OutputPort portHeater2)
        {
            Lcd = lcd;

            NavButtonManager = new NavigateButton(pushButton);

            TempReader1 = tempReader1;
            TempReader2 = tempReader2;

            PortHeater1 = portHeater1;
            PortHeater2 = portHeater2;
        }

        public void Initialize()
        {
            var blinker = new LedBlinker();
            blinker.Start();

            Heater1 = new HeatElement3000W(PortHeater1);
            Heater1.Start();

            Heater2 = new HeatElement3000W(PortHeater2);
            Heater2.Start();


        }

        private void ApplyState(State state)
        {
            NavButtonManager.KeyPressLong -= NavButton_KeyPressLong;
            NavButtonManager.KeyPressLongWarning -= NavButton_KeyPressLongWarning;
            NavButtonManager.KeyPressLongCancelled -= NavButton_KeyPressLongCancelled;
            NavButtonManager.KeyPressShort -= NavButton_KeyPressShort;
            if (_currentState != state)
            {
                //abort/dispose currently running job
                if (_currentState != null)
                    _currentState.Dispose();

                //update with new job
                _currentState = state;
                _currentState.DisplayContentChanged += _currentState_DisplayContentChanged;
                _currentState.StateChanged += _currentState_StateChanged;

                //start new job
                _currentState.Start();

                //DisplayLcdContent(state.DisplayUi);
            }
            NavButtonManager.KeyPressLong += NavButton_KeyPressLong;
            NavButtonManager.KeyPressLongWarning += NavButton_KeyPressLongWarning;
            NavButtonManager.KeyPressLongCancelled += NavButton_KeyPressLongCancelled;
            NavButtonManager.KeyPressShort += NavButton_KeyPressShort;

        }

        private void DisplayLcdContent(string[] contentToDisplay)
        {
            lock (Lcd)
            {
                for (byte i = 0; i < contentToDisplay.Length; i++)
                {
                    var line = contentToDisplay[i];
                    if (line.Length < Lcd.Columns)
                    {
                        //add spaces to the right
                        line = line.PadRight(Lcd.Columns);
                    }
                    //Update entire display content (since no content exists yet)
                    Lcd.setCursor(0, i);
                    Lcd.write(line);

                }
            }

        }

        public void Run()
        {
            NavButtonManager.KeyPressShort += NavButton_KeyPressShort;
            NavButtonManager.KeyPressLongWarning += NavButton_KeyPressLongWarning;
            NavButtonManager.KeyPressLong += NavButton_KeyPressLong;
            NavButtonManager.KeyPressLongCancelled += NavButton_KeyPressLongCancelled;

            var brewData = new BrewData(TempReader1, TempReader2, Heater1, Heater2);
            ApplyState(new StateDashboard(brewData));

            while (true)
            {
                Thread.Sleep(Timeout.Infinite);
            }
            // ReSharper disable FunctionNeverReturns
        }
        // ReSharper restore FunctionNeverReturns

        void NavButton_KeyPressLongCancelled()
        {
            _currentState.OnKeyPressLongCancelled();
        }

        void NavButton_KeyPressLongWarning()
        {
            _currentState.OnKeyPressLongWarning();
        }

        void NavButton_KeyPressShort()
        {
            _currentState.OnKeyPressShort();
        }

        void NavButton_KeyPressLong()
        {
            _currentState.OnKeyPressLong();
        }

        void _currentState_DisplayContentChanged()
        {
            DisplayLcdContent(_currentState.CurrentLcdContent);
        }

        void _currentState_StateChanged(State newState)
        {
            ApplyState(newState);
        }

    }


}
