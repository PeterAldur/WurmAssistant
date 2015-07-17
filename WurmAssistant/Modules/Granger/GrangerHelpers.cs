using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WurmAssistant.Granger
{
    public static class GrangerHelpers
    {
        public static string[] HorseAges = 
        {
            "Young", "Adolescent", "Mature", "Aged", "Old", "Venerable"
        };

        public static string[] WildCreatureNames = 
        {
            "Horse", "Hell horse"
        };

        public static string[] OtherNamePrefixes =
        {
            "fat", "starving", "diseased"
        };

        public static string[] AllNamePrefixes;

        static GrangerHelpers()
        {
            List<string> allnameprefixesBuilder = new List<string>();
            allnameprefixesBuilder.AddRange(HorseAges);
            allnameprefixesBuilder.AddRange(OtherNamePrefixes);
            AllNamePrefixes = allnameprefixesBuilder.ToArray<string>();
        }

        public static string RemoveAllPrefixes(string horseName)
        {
            foreach (string prefix in GrangerHelpers.AllNamePrefixes)
            {
                horseName = horseName.Replace(prefix, "");
            }
            horseName = horseName.Trim();
            return horseName;
        }
    }
}
