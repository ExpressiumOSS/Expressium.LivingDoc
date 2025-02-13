namespace Expressium.CucumberMessages
{
    public class Background
    {
        public Location location { get; set; }
        public string keyword { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Step[] steps { get; set; }
        public string id { get; set; }
    }
}
