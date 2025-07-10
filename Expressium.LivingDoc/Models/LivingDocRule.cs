using System.Collections.Generic;

namespace Expressium.LivingDoc.Models
{
    public class LivingDocRule
    {
        public string Id { get; set; }
        public List<string> Tags { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Keyword { get; set; }

        public LivingDocRule()
        {
            Tags = new List<string>();
        }

        public string GetTags()
        {
            return string.Join(" ", Tags);
        }
    }
}
