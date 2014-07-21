
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

        public float MashPIDKp = 4.0f; 
        public float MashPIDKi = 0.02f; // Decrease to avoid overshoot
        public float MashPIDKd = 1.0f; 

        public float SpargePIDKp = 4.0f;
        public float SpargePIDKi = 0.08f; // Decrease to avoid overshoot
        public float SpargePIDKd = 1.0f;


        public BrewData(PT100Reader tempReader1, PT100Reader tempReader2, HeatElement3000W heater1, HeatElement3000W heater2)
        {
            StrikeTemperature = 50;
            MashTemperature = 66;
            SpargeWaterTemperature = 86;
            MashTime = 65;
            TempReader1 = tempReader1;
            TempReader2 = tempReader2;
            Heater1 = heater1;
            Heater2 = heater2;
        }

    }
}
