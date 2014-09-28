
using System;
using System.IO;
using BrewMatic3000.Extensions;
using BrewMatic3000.Interfaces;
using BrewMatic3000.RealHW;
using Microsoft.SPOT;

namespace BrewMatic3000
{
    public class BrewData
    {
        public Config Config;

        public PT100Reader TempReader1 { get; set; }

        public PT100Reader TempReader2 { get; set; }

        public HeatElement3000W Heater1 { get; set; }

        public HeatElement3000W Heater2 { get; set; }

        public PID.PID MashPID;
        public PID.PID SpargePID;

        public DateTime BrewWarmupStart = DateTime.MinValue;
        public DateTime BrewAddGrainStart = DateTime.MinValue;
        public DateTime BrewMashStart = DateTime.MinValue;
        public DateTime BrewMashOutStart = DateTime.MinValue;
        public DateTime BrewSpargeStart = DateTime.MinValue;
        public DateTime BrewSpargeEnd = DateTime.MinValue;
        public DateTime MashStartTime = DateTime.MinValue;

        private DirectoryInfo _logDirectory;
        private FileInfo _logFile;

        private DirectoryInfo LogDirectory
        {
            get
            {
                if (_logDirectory == null)
                {
                    _logDirectory = new DirectoryInfo("\\sd\\BrewMatic3000\\Brew\\");
                }
                return _logDirectory;
            }
        }
        public FileInfo LogFile
        {
            get
            {
                if (!LogDirectory.Exists)
                {
                    LogDirectory.Create();
                }
                if (_logFile == null)
                {
                    _logFile = new FileInfo(Path.Combine(LogDirectory.FullName, "brew_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm") + ".txt"));
                }
                return _logFile;
            }
        }

        public void ResetBrewLog()
        {
            _logFile = null;
        }

        public void LogBrewEventToFile(string eventToLog)
        {
            try
            {
                using (var wr = new StreamWriter(LogFile.FullName, true))
                {
                    wr.WriteLine(DateTime.Now.Display() + ":\t" + eventToLog);
                    wr.Close();
                }
            }
            catch (Exception)
            {
                Debug.Print("Error logging value to file.");
            }
        }



        public DS3231 TimeChip { get; private set; }

        public BrewData(PT100Reader tempReader1, PT100Reader tempReader2, HeatElement3000W heater1, HeatElement3000W heater2)
        {
            Config = new Config();
            Config.SaveConfig();

            TempReader1 = tempReader1;
            TempReader2 = tempReader2;
            Heater1 = heater1;
            Heater2 = heater2;

            MashPID = new PID.PID(Config.MashPIDKp, Config.MashPIDKi, Config.MashPIDKd, Config.StrikeTemperature, tempReader1, Heater1);
            SpargePID = new PID.PID(Config.SpargePIDKp, Config.SpargePIDKi, Config.SpargePIDKd, Config.SpargePIDKd, tempReader2, Heater2);


            //Load the actual time and date from the time-chip
            TimeChip = new DS3231();

            var currentTime = TimeChip.GetDateTime();

            var dt = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, currentTime.Minute, currentTime.Second);

            Microsoft.SPOT.Hardware.Utility.SetLocalTime(dt);
            MashStartTime = DateTime.Now.AddHours(1).AddMinutes(40);
        }

    }
}
