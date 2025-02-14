using Expressium.TestExecution;
using System;
using System.Collections.Generic;

namespace Expressium.TestExecutionReport.Extensions
{
    public static class TestExecutionFolderExtension
    {
        public static List<string> GetListOfFolders(this LivingDocProject project)
        {
            var listOfFolders = new List<string>();

            foreach (var feature in project.Features)
            {
                if (!listOfFolders.Contains(feature.Uri))
                    listOfFolders.Add(feature.Uri);
            }

            return listOfFolders;
        }

        public static FolderNode BuildTree(List<string> tokens)
        {
            var root = new FolderNode("Root");

            foreach (var token in tokens)
            {
                var parts = token.Split('/');
                var current = root;

                foreach (var part in parts)
                {
                    if (!current.Children.ContainsKey(part))
                        current.Children[part] = new FolderNode(part);

                    current = current.Children[part];
                }
            }

            return root;
        }
    }

    public class FolderNode
    {
        public string Name { get; set; }
        public Dictionary<string, FolderNode> Children { get; set; } = new();

        public FolderNode(string name)
        {
            Name = name;
        }
    }
}
