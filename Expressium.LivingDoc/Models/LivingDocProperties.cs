using System;

namespace Expressium.LivingDoc.Models
{
    public class LivingDocProperties
    {
        public Property[] properties { get; set; }

        public class Property
        {
            public string name { get; set; }
            public string value { get; set; }
        }

    }
}
