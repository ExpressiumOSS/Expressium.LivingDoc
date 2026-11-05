using System.Collections.Generic;

namespace Expressium.LivingDoc.Models
{
    public class LivingDocProperties
    {
        public List<LivingDocPropertySection> Sections { get; set; }

        public LivingDocProperties()
        {
            Sections = new List<LivingDocPropertySection>();
        }
    }

    public class LivingDocPropertySection
    {
        public string Name { get; set; }

        public List<LivingDocProperty> Properties { get; set; }

        public LivingDocPropertySection()
        {
            Properties = new List<LivingDocProperty>();
        }
    }

    public class LivingDocProperty
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}