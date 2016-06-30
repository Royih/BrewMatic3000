using System;

namespace WebApp.Model.BrewGuide
{
    public class SetupDto
    {
        public string Name { get; set; }
        public float MashTemp { get; set; }
        public float StrikeTemp { get; set; }
        public float SpargeTemp { get; set; }
        public float MashOutTemp { get; set; }
        public int MashTimeInMinutes { get; set; }
        public int BoilTimeInMinutes { get; set; }
        public int BatchSize { get; set; }
        public float MashWaterAmount { get; set; }
        public float SpargeWaterAmount { get; set; }
    }
}