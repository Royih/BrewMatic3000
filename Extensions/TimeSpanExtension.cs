
using System;

namespace BrewMatic3000.Extensions
{
    public static class TimeSpanExtension
    {
        public static string Display(this TimeSpan input)
        {
            return input.ToString().Substring(0, 8);
        }
    }
}
