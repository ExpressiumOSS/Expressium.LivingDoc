using System;

namespace Expressium.TestExecution.Cucumber
{
    public class CucumberProject
    {
        public Feature[] objects { get; set; }
    }

    public class Feature
    {
        public string uri { get; set; }
        public string id { get; set; }
        public string keyword { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Scenario[] elements { get; set; }
    }

    public class Scenario
    {
        public string id { get; set; }
        public string keyword { get; set; }
        public string name { get; set; }
        public int line { get; set; }
        public string type { get; set; }
        public Step[] steps { get; set; }
    }

    public class Step
    {
        public string keyword { get; set; }
        public string name { get; set; }
        public int line { get; set; }
        public Result result { get; set; }
    }

    public class Result
    {
        public string status { get; set; }
        public int duration { get; set; }
        public string error_message { get; set; }
    }
}

