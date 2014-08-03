
using BrewMatic3000.RealHW;
using Microsoft.SPOT.Hardware;

namespace BrewMatic3000
{
    public class BrewData
    {
        public float StrikeTemperature { get; set; }

        public float MashTemperature { get; set; }

        public int MashTime { get; set; }

        public float SpargeWaterTemperature { get; set; }

        public PT100Reader TempReader1 { get; set; }

        public PT100Reader TempReader2 { get; set; }

        public HeatElement3000W Heater1 { get; set; }

        public HeatElement3000W Heater2 { get; set; }

        public PID.PID MashPID;
        public PID.PID SpargePID;

        public float MashPIDKp = 25.5f; //Previous: 18.5
        public float MashPIDKi = 1.9f; // Decrease to avoid overshoot.
        public float MashPIDKd = 1.0f;

        public float SpargePIDKp = 23.3f; //Previous 16.3
        public float SpargePIDKi = 1.9f; // Decrease to avoid overshoot
        public float SpargePIDKd = 1.0f;


        public BrewData(PT100Reader tempReader1, PT100Reader tempReader2, HeatElement3000W heater1, HeatElement3000W heater2)
        {
            StrikeTemperature = 73.2f;
            MashTemperature = 67;
            SpargeWaterTemperature = 75.6f;
            MashTime = 60;
            TempReader1 = tempReader1;
            TempReader2 = tempReader2;
            Heater1 = heater1;
            Heater2 = heater2;

            MashPID = new PID.PID(MashPIDKp, MashPIDKi, MashPIDKd);
            SpargePID = new PID.PID(SpargePIDKp, SpargePIDKi, SpargePIDKd);
        }

    }
}
