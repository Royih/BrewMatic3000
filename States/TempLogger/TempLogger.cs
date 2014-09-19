using System;
using System.IO;
using BrewMatic3000.Extensions;
using Microsoft.SPOT;

namespace BrewMatic3000.States.TempLogger
{
    public class TempLogger : State
    {

        private DateTime _logStarted = DateTime.MinValue;
        private DateTime _lastLog = DateTime.MinValue;
        private float _minTemp1;
        private float _maxTemp1;
        private float _minTemp2;
        private float _maxTemp2;
        private int _numberOfLogs;
        private int _logEveryNSeconds = 10;


        private DirectoryInfo _logDirectory;
        private FileInfo _logFile;

        private DirectoryInfo LogDirectory
        {
            get
            {
                if (_logDirectory == null)
                {
                    _logDirectory = new DirectoryInfo("\\sd\\TempLogger\\");
                }
                return _logDirectory;
            }
        }
        private FileInfo LogFile
        {
            get
            {
                if (!LogDirectory.Exists)
                {
                    LogDirectory.Create();
                }
                if (_logFile == null)
                {
                    _logFile = new FileInfo(Path.Combine(LogDirectory.FullName, "tempLogger_" + _logStarted.ToString("yyyy_MM_dd_hh_mm") + ".txt"));
                }
                return _logFile;
            }
        }

        public TempLogger(BrewData brewData, string[] initialMessage = null, int initialScreen = 0)
            : base(brewData, initialMessage, initialScreen)
        {

        }

        public enum Screens
        {
            Default,
            MinMax,
            NumberOfLogs,
            StopLogging
        }

        public override int GetNumberOfScreens()
        {
            return (int)Screens.StopLogging;
        }


        public override Screen GetScreen(int screenNumber)
        {
            switch (screenNumber)
            {
                case (int)Screens.Default:
                    {
                        var strLine1 = "=  Temp logging  =";
                        var strLine2 = "Tm: " + DateTime.Now.Subtract(_logStarted).Display();
                        var strLine3 = "Temp 1: " + BrewData.TempReader1.GetValue().DisplayTemperature();
                        var strLine4 = "Temp 2: " + BrewData.TempReader2.GetValue().DisplayTemperature();

                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 });
                    }
                case (int)Screens.MinMax:
                    {
                        var strLine1 = "=  Temp logging  =";
                        var strLine2 = "   Min   |Max";
                        var strLine3 = "1: " + _minTemp1.DisplayTemperature() + "|" + _maxTemp1.DisplayTemperature();
                        var strLine4 = "2: " + _minTemp2.DisplayTemperature() + "|" + _maxTemp2.DisplayTemperature();

                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 });
                    }
                case (int)Screens.NumberOfLogs:
                    {
                        var strLine1 = "=  Temp logging  =";
                        var strLine2 = "Log every: " + _logEveryNSeconds + " sec.";
                        var strLine3 = "# logs:" + _numberOfLogs;
                        var strLine4 = "";

                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 });
                    }
                case (int)Screens.StopLogging:
                    {
                        var strLine1 = "=  Temp logging  =";
                        var strLine2 = "Stop logging";
                        var strLine3 = "";
                        var strLine4 = "";

                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 }, strLine2);
                    }
                default:
                    {
                        return GetScreenError(screenNumber);
                    }
            }
        }

        protected override void DoWorkExtra()
        {
            if (DateTime.Now.AddSeconds((-1) * _logEveryNSeconds) > _lastLog)
            {
                //Its time to log again
                _lastLog = DateTime.Now;
                _numberOfLogs++;

                var temp1 = BrewData.TempReader1.GetValue();
                var temp2 = BrewData.TempReader2.GetValue();

                EvalutateNewTemp(ref _minTemp1, ref _maxTemp1, temp1);
                EvalutateNewTemp(ref _minTemp2, ref _maxTemp2, temp2);

                LogToFile(temp1, temp2);
            }
        }

        private void EvalutateNewTemp(ref float currentMinValue, ref float currentMaxValue, float temparature)
        {
            if (currentMinValue > temparature)
                currentMinValue = temparature;

            if (currentMaxValue < temparature)
                currentMaxValue = temparature;
        }

        public override void KeyPressNextLong()
        {
            if (GetCurrentScreenNumber == (int)Screens.StopLogging)
            {
                RiseStateChangedEvent(new StateDashboard(BrewData, new[] { "Temp logging stopped" }));
            }
        }


        protected override void StartExtra()
        {
            BrewData.Heater1.SetValue(0);
            BrewData.Heater2.SetValue(0);

            _logEveryNSeconds = BrewData.Config.TempLoggerLogEveryNSeconds;

            _logStarted = DateTime.Now;
            _lastLog = DateTime.Now;
            _minTemp1 = BrewData.TempReader1.GetValue();
            _maxTemp1 = _minTemp1;
            _minTemp2 = BrewData.TempReader2.GetValue();
            _maxTemp2 = _minTemp2;
            _numberOfLogs = 1;

            //Create new directory for log
            LogToFile(_minTemp1, _minTemp2, true);
        }

        private void LogToFile(float tempValue1, float tempValue2, bool writeHeader = false)
        {
            try
            {
                using (var wr = new StreamWriter(LogFile.FullName, true))
                {
                    if (writeHeader)
                    {
                        wr.WriteLine("TimeStamp\tValue1\tValue2");
                    }
                    wr.WriteLine(DateTime.Now + "\t" + tempValue1 + "\t" + tempValue2);
                    wr.Close();
                }
            }
            catch (Exception)
            {
                Debug.Print("Error logging value to file.");
            }
        }


    }
}
