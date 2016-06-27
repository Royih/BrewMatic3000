
using System;

namespace WebApp.Model.BrewGuide
{
    public class BrewLogHistoryDto
    {
        public string Name { get; set; }
        public DateTime Started { get; set; }
        public DateTime? Completed { get; set; }

        public string TimeUsed
        {
            get
            {
                if (!Completed.HasValue)
                {
                    return "...";
                }
                return Completed.Value.Subtract(Started).ToString("g").Substring(0,7);
            }
        }
    }
}
