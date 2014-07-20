
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

        public float MashPIDKp = 5.5f; // Decrease to make the slowdown start earlier (to stop overshoot)
        public float MashPIDKi = 0.1f; //
        public float MashPIDKd = 10.0f; //Increase to decrease overshoot??

        public float SpargePIDKp = 3.5f; // Decrease to make the slowdown start earlier (to stop overshoot)
        public float SpargePIDKi = 0.1f; //
        public float SpargePIDKd = -105.0f; //Increase to decrease overshoot??


        public BrewData(PT100Reader tempReader1, PT100Reader tempReader2, HeatElement3000W heater1, HeatElement3000W heater2)
        {
            StrikeTemperature = 70;
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
