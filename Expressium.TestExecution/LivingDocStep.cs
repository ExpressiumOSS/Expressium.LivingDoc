using System;

namespace Expressium.TestExecution
{
    public class LivingDocStep
    {
        public string Id { get; set; }
        public string Keyword { get; set; }
        public string Name { get; set; }
        //public int Line { get; set; }
        public string Status { get; set; }
        public long Duration { get; set; }
        public string Message { get; set; }

        public LivingDocDataTable DataTable { get; set; }

        public LivingDocStep()
        {
            DataTable = new LivingDocDataTable();
        }

        public bool IsPassed()
        {
            return Status.IsPassed();
        }

        public bool IsFailed()
        {
            return Status.IsFailed();
        }

        public bool IsPending()
        {
            return Status.IsPending();
        }       

        public bool IsIncomplete()
        {
            return Status.IsIncomplete();
        }

        public bool IsUndefined()
        {
            return Status.IsUndefined();
        }

        public bool IsAmbiguous()
        {
            return Status.IsAmbiguous();
        }

        public bool IsSkipped()
        {
            return Status.IsSkipped();
        }

        public string GetStatus()
        {
            return Status.GetStatus();
        }
    }
}
