namespace Expressium.CucumberMessages
{
    public class Scenario
    {
        public Location location { get; set; }
        public Tag[] tags { get; set; }
        public string keyword { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Step[] steps { get; set; }
        public Example[] examples { get; set; }
        public string id { get; set; }
    }
}
