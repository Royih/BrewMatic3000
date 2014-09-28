
using System;
using System.IO;
using Microsoft.SPOT;

namespace BrewMatic3000
{
    public class Config
    {
        public float StrikeTemperature = 73.6f;

        public float MashTemperature = 67.0f;

        public float MashOutTemperature = 78.0f;

        public int MashTime = 60; //Minutes

        public float SpargeTemperature = 75.6f;

        public float MashPIDKp = 25.5f; // Increase if slow or not reaching set value

        public float MashPIDKi = 1.9f; // Decrease to avoid overshoot.

        public float MashPIDKd = 1.0f;

        public float SpargePIDKp = 23.3f; // Increase if slow or not reaching set value

        public float SpargePIDKi = 1.9f; // Decrease to avoid overshoot

        public float SpargePIDKd = 1.0f;

        public int EstimatedMashWarmupMinutes = 90;

        public int EstimatedSpargeWarmupMinutes = 60;

        public int TempLoggerLogEveryNSeconds = 10;

        private const string ConfigfileName = "config.txt";

        private string ConfigFile
        {
            get
            {
                return Path.Combine("\\sd\\BrewMatic3000", ConfigfileName);
            }
        }

        private StreamReader ConfigFileReader
        {
            get
            {
                var myFile = new FileInfo(ConfigFile);
                if (myFile.Exists)
                    return new StreamReader(myFile.FullName);
                return null;
            }
        }

        private StreamWriter ConfigFileWriter
        {
            get
            {

                var myFile = new FileInfo(ConfigFile);
                if (!myFile.Exists)
                    myFile.Create();
                return new StreamWriter(myFile.FullName, false);

            }
        }


        public Config()
        {
            try
            {
                using (var file = ConfigFileReader)
                {
                    if (file == null)
                        return;
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        var value = line.Split(':');
                        if (value.Length == 2)
                        {
                            ApplyConfigSetting(value[0], value[1]);
                        }
                    }
                    file.Close();
                }
            }
            catch (IOException ex)
            {
                Debug.Print("Error loading config file: " + ex.Message);
            }

        }

        public void SaveConfig()
        {
            try
            {
                using (var file = ConfigFileWriter)
                {
                    file.WriteLine("StrikeTemperature:" + StrikeTemperature);
                    file.WriteLine("MashTemperature:" + MashTemperature);
                    file.WriteLine("MashOutTemperature:" + MashOutTemperature);
                    file.WriteLine("MashTime:" + MashTime);
                    file.WriteLine("SpargeTemperature:" + SpargeTemperature);
                    file.WriteLine("MashPIDKp:" + MashPIDKp);
                    file.WriteLine("MashPIDKi:" + MashPIDKi);
                    file.WriteLine("MashPIDKd:" + MashPIDKd);
                    file.WriteLine("SpargePIDKp:" + SpargePIDKp);
                    file.WriteLine("SpargePIDKi:" + SpargePIDKi);
                    file.WriteLine("SpargePIDKd:" + SpargePIDKd);
                    file.WriteLine("EstimatedMashWarmupMinutes:" + EstimatedMashWarmupMinutes);
                    file.WriteLine("EstimatedSpargeWarmupMinutes:" + EstimatedSpargeWarmupMinutes);
                    file.WriteLine("TempLoggerLogEveryNSeconds:" + TempLoggerLogEveryNSeconds);
                    file.Close();
                }
            }
            catch (IOException ex)
            {
                Debug.Print("Error saving config file: " + ex.Message);
            }
        }

        private void ApplyConfigSetting(string configName, string valueAsString)
        {
            switch (configName)
            {
                case "StrikeTemperature":
                    {
                        StrikeTemperature = ApplyFloat(valueAsString, StrikeTemperature);
                        break;
                    }
                case "MashTemperature":
                    {
                        MashTemperature = ApplyFloat(valueAsString, MashTemperature);
                        break;
                    }
                case "MashOutTemperature":
                    {
                        MashOutTemperature = ApplyFloat(valueAsString, MashOutTemperature);
                        break;
                    }
                case "MashTime":
                    {
                        MashTime = ApplyInt(valueAsString, MashTime);
                        break;
                    }
                case "SpargeTemperature":
                    {
                        SpargeTemperature = ApplyFloat(valueAsString, SpargeTemperature);
                        break;
                    }
                case "MashPIDKp":
                    {
                        MashPIDKp = ApplyFloat(valueAsString, MashPIDKp);
                        break;
                    }
                case "MashPIDKi":
                    {
                        MashPIDKi = ApplyFloat(valueAsString, MashPIDKi);
                        break;
                    }
                case "MashPIDKd":
                    {
                        MashPIDKd = ApplyFloat(valueAsString, MashPIDKd);
                        break;
                    }
                case "SpargePIDKp":
                    {
                        SpargePIDKp = ApplyFloat(valueAsString, SpargePIDKp);
                        break;
                    }
                case "SpargePIDKi":
                    {
                        SpargePIDKi = ApplyFloat(valueAsString, SpargePIDKi);
                        break;
                    }
                case "SpargePIDKd":
                    {
                        SpargePIDKd = ApplyFloat(valueAsString, SpargePIDKd);
                        break;
                    }
                case "EstimatedMashWarmupMinutes":
                    {
                        EstimatedMashWarmupMinutes = ApplyInt(valueAsString, EstimatedMashWarmupMinutes);
                        break;
                    }
                case "EstimatedSpargeWarmupMinutes":
                    {
                        EstimatedSpargeWarmupMinutes = ApplyInt(valueAsString, EstimatedSpargeWarmupMinutes);
                        break;
                    }
                case "TempLoggerLogEveryNSeconds":
                    {
                        TempLoggerLogEveryNSeconds = ApplyInt(valueAsString, TempLoggerLogEveryNSeconds);
                        break;
                    }
            }
        }

        private float ApplyFloat(string valueAsString, float defaultValue)
        {
            double valueAsDouble;
            if (double.TryParse(valueAsString, out valueAsDouble))
            {
                return (float)valueAsDouble;
            }
            return defaultValue;
        }

        private int ApplyInt(string valueAsString, int defaultValue)
        {
            try
            {
                return int.Parse(valueAsString);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }
}
