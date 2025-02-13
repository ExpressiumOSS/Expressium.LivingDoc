namespace Expressium.CucumberMessages
{
    public class Feature
    {
        public Location location { get; set; }
        public Tag[] tags { get; set; }
        public string language { get; set; }
        public string keyword { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Child[] children { get; set; }
    }
}
