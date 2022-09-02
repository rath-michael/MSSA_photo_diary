using System;
using System.Collections.Generic;
using System.Linq;

namespace Week8Project
{
    internal static class TagConverter
    {
        // Convert single string to list of strings
        public static List<string> StringToTags(string input)
        {
            List<string> tags = input.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            return tags;
        }
        // Convert list of tags to single string
        public static string TagsToString(IEnumerable<ImageTag> tags)
        {
            List<string> sList = new List<string>();
            foreach (var item in tags)
            {
                sList.Add(item.Tag);
            }

            string s = string.Join(", ", sList);
            return s;
        }
    }
}
