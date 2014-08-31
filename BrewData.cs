
using System;
using BrewMatic3000.RealHW;

namespace BrewMatic3000
{
    public class BrewData
    {
        public float StrikeTemperature { get; set; }

        public float MashTemperature { get; set; }

        public float MashOutTemperature { get; set; }

        public int MashTime { get; set; }

        public float SpargeTemperature { get; set; }

        public PT100Reader TempReader1 { get; set; }

        public PT100Reader TempReader2 { get; set; }

        public HeatElement3000W Heater1 { get; set; }

        public HeatElement3000W Heater2 { get; set; }

        public PID.PID MashPID;
        public PID.PID SpargePID;

        public float MashPIDKp = 25.5f; // Increase if slow or not reaching set value
        public float MashPIDKi = 1.9f; // Decrease to avoid overshoot.
        public float MashPIDKd = 1.0f;

        public float SpargePIDKp = 23.3f; // Increase if slow or not reaching set value
        public float SpargePIDKi = 1.9f; // Decrease to avoid overshoot
        public float SpargePIDKd = 1.0f;

        public DateTime BrewWarmupStart = DateTime.MinValue;
        public DateTime BrewAddGrainStart = DateTime.MinValue;
        public DateTime BrewMashStart = DateTime.MinValue;
        public DateTime BrewMashOutStart = DateTime.MinValue;
        public DateTime BrewSpargeStart = DateTime.MinValue;
        public DateTime BrewSpargeEnd = DateTime.MinValue;

        public int EstimatedMashWarmupMinutes = 90;
        public int EstimatedSpargeWarmupMinutes = 60;
        public DateTime MashStartTime = DateTime.MinValue;

        public DS3231 TimeChip { get; private set; }

        public BrewData(PT100Reader tempReader1, PT100Reader tempReader2, HeatElement3000W heater1, HeatElement3000W heater2)
        {
            StrikeTemperature = 73.6f;
            MashTemperature = 67.0f;
            MashOutTemperature = 78.0f; //minimum = 76 grader
            SpargeTemperature = 75.6f;
            MashTime = 60; //minutes
            TempReader1 = tempReader1;
            TempReader2 = tempReader2;
            Heater1 = heater1;
            Heater2 = heater2;

            MashPID = new PID.PID(MashPIDKp, MashPIDKi, MashPIDKd, StrikeTemperature, tempReader1, Heater1);
            SpargePID = new PID.PID(SpargePIDKp, SpargePIDKi, SpargePIDKd, SpargePIDKd, tempReader2, Heater2);


            //Load the actual time and date from the time-chip
            TimeChip = new DS3231();

            var currentTime = TimeChip.GetDateTime();

            var dt = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, currentTime.Minute, currentTime.Second);

            Microsoft.SPOT.Hardware.Utility.SetLocalTime(dt);
            MashStartTime = DateTime.Now.AddHours(1).AddMinutes(40);
        }

    }
}
