namespace WebApp.Model.BrewGuide
{
    public class DataCaptureValueDto
    {
        public int Id { get; set; }
        public int BrewStepId {get; set;}
        public string Label { get; set; }
        public string ValueAsString { get; set; }
        public string ValueType { get; set; }
        public string Units { get; set; }
        public bool Optional { get; set; }
    }
}