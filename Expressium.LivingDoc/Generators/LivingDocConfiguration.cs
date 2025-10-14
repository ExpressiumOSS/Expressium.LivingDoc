using System;

namespace Expressium.LivingDoc.Generators
{
    public class LivingDocConfiguration
    {
        public bool Overview { get; set; }
        public bool FeaturesListView { get; set; }
        public bool ScenariosListView { get; set; }
        public bool StepsListView { get; set; }
        public bool EditorView { get; set; }

        public LivingDocConfiguration()
        {
            Overview = true;
            FeaturesListView = true;
            ScenariosListView = true;
            StepsListView = true;
            EditorView = false;
        }
    }
}
