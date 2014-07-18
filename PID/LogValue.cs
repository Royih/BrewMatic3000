using System;
using Microsoft.SPOT;

namespace BrewMatic3000.PID
{
    public class LogValue
    {
        public DateTime TimeStamp { get; set; }
        public float Temperature { get; set; }
        public float Effect { get; set; }
    }
}
