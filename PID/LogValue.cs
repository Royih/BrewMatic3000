using System;
using Microsoft.SPOT;

namespace BrewMatic3000.PID
{
    public class LogValue
    {
        public DateTime TimeStamp { get; set; }
        public double Temperature { get; set; }
        public double Effect { get; set; }
    }
}
