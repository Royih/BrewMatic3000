using System;

namespace WebApp.Model.BrewLogs
{
    public class BrewLogsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float BatchSize { get; set; }
        public DateTime Initiated { get; set; }
        public DateTime BrewDate { get; set; }
        public string CurrentStep { get; set; }
    }
}
