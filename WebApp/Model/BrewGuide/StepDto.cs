using System;

namespace WebApp.Model.BrewGuide
{
    public class StepDto
    {
        public int Order { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? CompleteTime { get; set; }
        public float TargetMashTemp { get; set; }
        public float TargetSpargeTemp { get; set; }
        public string CompleteButtonText { get; set; }
        public string Instructions { get; set; }

    }
}