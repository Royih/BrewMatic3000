using System;

namespace WebApp.Model.BrewGuide
{
    public class StepDto
    {
        public int Order { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? CompleteTime { get; set; }
        public float GetTargetMashTemp { get; set; }
        public float GetTargetSpargeTemp { get; set; }
        public string CompleteButtonText { get; set; }
        public string Instructions { get; set; }

    }
}