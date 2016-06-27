using System;

namespace WebApp.Model.BrewGuide
{
    public class StepDto
    {
        public int Order { get; set; }
        public string Name { get; set; }
        public DateTime? StartTime { get; set; }
        public bool ShowTimer { get; set; }
        public float GetTargetMashTemp { get; set; }
        public float GetTargetSpargeTemp { get; set; }
        public string CompleteButtonText { get; set; }

    }
}