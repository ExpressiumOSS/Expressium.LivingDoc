using System;

namespace Expressium.TestExecution.Cucumber
{
    public class CucumberProject
    {
        public Feature[] objects { get; set; }
    }

    public class Feature
    {
        public string Id { get; set; }
        public Tag[] Tags { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Keyword { get; set; }
        public int Line { get; set; }
        public string Uri { get; set; }
        public Element[] Elements { get; set; }
    }

    public class Tag
    {
        public string Name { get; set; }
        public int Line { get; set; }
    }

    public class Element
    {
        public string Id { get; set; }
        public Tag[] Tags { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Keyword { get; set; }
        public int Line { get; set; }
        public string Type { get; set; }
        public Step[] Steps { get; set; }
        public After1[] After { get; set; }
        public Before1[] Before { get; set; }
    }

    public class Step
    {
        public Result Result { get; set; }
        public string Name { get; set; }
        public string Keyword { get; set; }
        public int Line { get; set; }
        public Match match { get; set; }
        public Embedding[] embeddings { get; set; }
        public object[] comments { get; set; }
        public Doc_String doc_string { get; set; }
        public Row[] Rows { get; set; }
        public int[] matchedColumns { get; set; }
        public Argument1[] arguments { get; set; }
        public Before[] before { get; set; }
        public After[] after { get; set; }
        public object[] output { get; set; }
    }

    public class Result
    {
        public string Status { get; set; }
        public long Duration { get; set; }
        public string Error_message { get; set; }
    }

    public class Match
    {
        public string location { get; set; }
        public Argument[] arguments { get; set; }
    }

    public class Argument
    {
        public string val { get; set; }
        public int offset { get; set; }
    }

    public class Doc_String
    {
        public string content_type { get; set; }
        public int line { get; set; }
        public string value { get; set; }
    }

    public class Embedding
    {
        public string mime_type { get; set; }
        public string data { get; set; }
        public Media media { get; set; }
        public string name { get; set; }
    }

    public class Media
    {
        public string type { get; set; }
    }

    public class Row
    {
        public string[] Cells { get; set; }
        public int Line { get; set; }
    }

    public class Argument1
    {
        public Row1[] rows { get; set; }
    }

    public class Row1
    {
        public string[] cells { get; set; }
    }

    public class Before
    {
        public Embedding1[] embeddings { get; set; }
        public Result1 result { get; set; }
    }

    public class Result1
    {
        public int duration { get; set; }
        public string status { get; set; }
    }

    public class Embedding1
    {
        public string mime_type { get; set; }
        public string data { get; set; }
    }

    public class After
    {
        public Result2 result { get; set; }
        public Match1 match { get; set; }
    }

    public class Result2
    {
        public int duration { get; set; }
        public string status { get; set; }
    }

    public class Match1
    {
        public string location { get; set; }
    }

    public class After1
    {
        public Result Result { get; set; }
        public Match2 match { get; set; }
        public Embedding2[] embeddings { get; set; }
    }

    public class Match2
    {
        public string location { get; set; }
    }

    public class Embedding2
    {
        public string mime_type { get; set; }
        public string data { get; set; }
    }

    public class Before1
    {
        public string[] output { get; set; }
        public Result Result { get; set; }
        public Match3 match { get; set; }
    }

    public class Match3
    {
        public string location { get; set; }
    }
}
