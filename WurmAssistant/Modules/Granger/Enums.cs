using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WurmAssistant.Granger
{
    public enum HorseTrait
    {
        FightFiercely,
        FleeterMovement,
        ToughBugger,
        StrongBody,
        LightningMovement,
        CarryMoreThanAverage,
        VeryStrongLegs,
        KeenSenses,
        MalformedHindlegs,
        LegsOfDifferentLength,
        OverlyAggressive,
        VeryUnmotivated,
        UnusuallyStrongWilled,
        HasSomeIllness,
        ConstantlyHungry,
        FeebleAndUnhealthy,
        StrongAndHealthy,
        CertainSpark
    }

    public enum HorseSortingOption
    {
        Name, Gender, Father, Mother, Value, Potential, Breeded, Groomed, Pregnant,
        Comment
    }

    public enum HerdOperationType
    {
        New, Rename, Merge, Select
    }

    public enum HorseOperationType
    {
        New, Edit, View
    }

    static class HorseTraitEX
    {
        const int TRAIT_LINE_RECOGNITION_CHAR_COUNT = 3;

        public static Dictionary<string, HorseTrait> NameToEnumMap = new Dictionary<string, HorseTrait>();
        public static Dictionary<HorseTrait, string> EnumToNameMap = new Dictionary<HorseTrait, string>();
        public static Dictionary<HorseTrait, int> EnumToAHSkillMap = new Dictionary<HorseTrait, int>();

        static HashSet<string> TraitLineRecognitionSet = new HashSet<string>();

        static HorseTraitEX()
        {
            NameToEnumMap.Add("It will fight fiercely", HorseTrait.FightFiercely);
            NameToEnumMap.Add("It has fleeter movement than normal", HorseTrait.FleeterMovement);
            NameToEnumMap.Add("It is a tough bugger", HorseTrait.ToughBugger);
            NameToEnumMap.Add("It has a strong body", HorseTrait.StrongBody);
            NameToEnumMap.Add("It has lightning movement", HorseTrait.LightningMovement);
            NameToEnumMap.Add("It can carry more than average", HorseTrait.CarryMoreThanAverage);
            NameToEnumMap.Add("It has very strong leg muscles", HorseTrait.VeryStrongLegs);
            NameToEnumMap.Add("It has keen senses", HorseTrait.KeenSenses);
            NameToEnumMap.Add("It has malformed hindlegs", HorseTrait.MalformedHindlegs);
            NameToEnumMap.Add("The legs are of different length", HorseTrait.LegsOfDifferentLength);
            NameToEnumMap.Add("It seems overly aggressive", HorseTrait.OverlyAggressive);
            NameToEnumMap.Add("It looks very unmotivated", HorseTrait.VeryUnmotivated);
            NameToEnumMap.Add("It is unusually strong willed", HorseTrait.UnusuallyStrongWilled);
            NameToEnumMap.Add("It has some illness", HorseTrait.HasSomeIllness);
            NameToEnumMap.Add("It looks constantly hungry", HorseTrait.ConstantlyHungry);
            NameToEnumMap.Add("It looks feeble and unhealthy", HorseTrait.FeebleAndUnhealthy);
            NameToEnumMap.Add("It looks unusually strong and healthy", HorseTrait.StrongAndHealthy);
            NameToEnumMap.Add("It has a certain spark in its eyes", HorseTrait.CertainSpark);

            foreach (var keyval in NameToEnumMap)
            {
                EnumToNameMap.Add(keyval.Value, keyval.Key);
                TraitLineRecognitionSet.Add(keyval.Key.Substring(0, TRAIT_LINE_RECOGNITION_CHAR_COUNT));
            }

            EnumToAHSkillMap.Add(HorseTrait.FightFiercely, 20);
            EnumToAHSkillMap.Add(HorseTrait.FleeterMovement, 21);
            EnumToAHSkillMap.Add(HorseTrait.ToughBugger, 22);
            EnumToAHSkillMap.Add(HorseTrait.StrongBody, 23);
            EnumToAHSkillMap.Add(HorseTrait.LightningMovement, 24);
            EnumToAHSkillMap.Add(HorseTrait.CarryMoreThanAverage, 25);
            EnumToAHSkillMap.Add(HorseTrait.VeryStrongLegs, 26);
            EnumToAHSkillMap.Add(HorseTrait.KeenSenses, 27);
            EnumToAHSkillMap.Add(HorseTrait.MalformedHindlegs, 28);
            EnumToAHSkillMap.Add(HorseTrait.LegsOfDifferentLength, 29);
            EnumToAHSkillMap.Add(HorseTrait.OverlyAggressive, 30);
            EnumToAHSkillMap.Add(HorseTrait.VeryUnmotivated, 31);
            EnumToAHSkillMap.Add(HorseTrait.UnusuallyStrongWilled, 32);
            EnumToAHSkillMap.Add(HorseTrait.HasSomeIllness, 33);
            EnumToAHSkillMap.Add(HorseTrait.ConstantlyHungry, 34);
            EnumToAHSkillMap.Add(HorseTrait.FeebleAndUnhealthy, 39);
            EnumToAHSkillMap.Add(HorseTrait.StrongAndHealthy, 40);
            EnumToAHSkillMap.Add(HorseTrait.CertainSpark, 41);
        }

        internal static HorseTrait[] ExtractTraitsFromLine(string line)
        {
            List<HorseTrait> traits = new List<HorseTrait>();
            foreach (var keyval in NameToEnumMap)
            {
                if (line.Contains(keyval.Key))
                {
                    traits.Add(keyval.Value);
                }
            }
            return traits.ToArray<HorseTrait>();
        }

        internal static bool CanThisBeTraitLine(string line)
        {
            return TraitLineRecognitionSet.Contains(line.Substring(0, TRAIT_LINE_RECOGNITION_CHAR_COUNT));
        }
    }
}