using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace WurmAssistant.Granger
{
    public class Horse
    {
        public string Name = "";
        public string Father = "";
        public string Mother = "";
        public List<HorseTrait> Traits = new List<HorseTrait>();
        public DateTime NotInMood = new DateTime(0);
        public DateTime PregnantTo = new DateTime(0);
        public DateTime GroomedOn = new DateTime(0);
        public string PairedWith = "";
        public bool IsMale = true;
        public string TakenCareOfBy = "";
        public int TraitsInspectedAtSkill = 0;
        public bool maybeDied = false;
        public bool maybePregnant = false;
        public string Comments = "";

        public static string[] DBFieldNames = new string[]
            {
                "name", "father", "mother", "traits", "notinmood", "pregnantto", "groomedon", "pairedwith", "ismale", "caredby", "inspectskill", "maybedied", "maybepregnant", "comments"
            };

        public static Horse ConstructHorse(DataRow initdatarow = null)
        {
            Horse horse = new Horse();
            if (initdatarow != null)
            {
                horse.InitFromDTrow(initdatarow);
            }
            return horse;
        }

        public void Save(SQLiteDB db, string dbtablename)
        {
            //if (!db.Update(dbtablename, GetDBSaveData(), GetUpdateWhereClause()))
            db.Insert(dbtablename, GetDBSaveData());
        }

        List<DBField> GetDBSaveData()
        {
            List<DBField> output = new List<DBField>();
            output.Add(new DBField("name", Name));
            output.Add(new DBField("father", Father));
            output.Add(new DBField("mother", Mother));
            string traitsoutput = "";
            foreach (var item in Traits)
            {
                traitsoutput += item.ToString() + ";";
            }
            if (traitsoutput != "") traitsoutput = traitsoutput.Substring(0, traitsoutput.Length - 1);
            output.Add(new DBField("traits", traitsoutput));
            output.Add(new DBField("notinmood", NotInMood.ToString()));
            output.Add(new DBField("pregnantto", PregnantTo.ToString()));
            output.Add(new DBField("groomedon", GroomedOn.ToString()));
            output.Add(new DBField("pairedwith", PairedWith));
            output.Add(new DBField("ismale", IsMale.ToString()));
            output.Add(new DBField("caredby", TakenCareOfBy));
            output.Add(new DBField("inspectskill", TraitsInspectedAtSkill.ToString()));
            output.Add(new DBField("maybedied", maybeDied.ToString()));
            output.Add(new DBField("maybepregnant", maybePregnant.ToString()));
            output.Add(new DBField("comments", Comments.ToString()));
            return output;
        }

        string GetUpdateWhereClause()
        {
            return "name='" + Name + "'";
        }

        void InitFromDTrow(DataRow datarow)
        {
            Name = datarow["name"].ToString();
            Father = datarow["father"].ToString();
            Mother = datarow["mother"].ToString();
            string[] traitsSTR = datarow["traits"].ToString().Split(new string[] { ";" }, StringSplitOptions.None);
            Traits.Clear();
            for (int i = 0; i < traitsSTR.Length; i++)
            {
                try
                {
                    Traits.Add((HorseTrait)Enum.Parse(typeof(HorseTrait), traitsSTR[i]));
                }
                catch (Exception _e)
                {
                    if (!(_e is IndexOutOfRangeException) && traitsSTR[i] == "")
                    {
                        // horse with no traits, shouldn't happen very often if at all
                    }
                    else
                    {
                        Logger.WriteLine("! Granger: error while parsing trait for horse '" + Name + "' trait raw data: " + traitsSTR[i]);
                        Logger.LogException(_e);
                    }
                }
            }
            NotInMood = DateTime.Parse(datarow["notinmood"].ToString());
            PregnantTo = DateTime.Parse(datarow["pregnantto"].ToString());
            GroomedOn = DateTime.Parse(datarow["groomedon"].ToString());
            PairedWith = datarow["pairedwith"].ToString();
            IsMale = Boolean.Parse(datarow["ismale"].ToString());
            TakenCareOfBy = datarow["caredby"].ToString();
            TraitsInspectedAtSkill = Int32.Parse(datarow["inspectskill"].ToString());
            maybeDied = Boolean.Parse(datarow["maybedied"].ToString());
            maybePregnant = Boolean.Parse(datarow["maybepregnant"].ToString());
            Comments = datarow["comments"].ToString();
        }

        public void SetTraitsFromLogLine(string line)
        {
            HorseTrait[] traits = HorseTraitEX.ExtractTraitsFromLine(line);
            GrangerDebug.Log("extracting traits");
            foreach (var trait in traits)
            {
                if (!Traits.Contains(trait))
                {
                    Traits.Add(trait);
                    int thisTraitAHSkill = HorseTraitEX.EnumToAHSkillMap[trait];
                    if (TraitsInspectedAtSkill < thisTraitAHSkill)
                    {
                        TraitsInspectedAtSkill = thisTraitAHSkill;
                        GrangerDebug.Log("trait was above detected AH skill: " + trait.ToString());
                    }
                    GrangerDebug.Log("matched trait: "+trait.ToString());
                }
            }
        }

        internal bool IsInbreed(Horse otherHorse)
        {
            if (Name == otherHorse.Mother 
                || Name == otherHorse.Father
                || Mother == otherHorse.Name
                || Father == otherHorse.Name
                || (Mother != "" && Mother == otherHorse.Mother) 
                || (Father != "" && Father == otherHorse.Father)) 
                return true;
            else return false;
        }

        internal bool IsNotInMood
        {
            get
            {
                if (NotInMood > DateTime.Now - TimeSpan.FromHours(1D)) return true;
                else return false;
            }
        }

        internal bool IsPregnant
        {
            get
            {
                if (PregnantTo > DateTime.Now || maybePregnant) return true;
                else return false;
            }
        }

        //duplicate factor note: if trait is duplicated, it's value is multiplied by this factor
        //goodfactor means applied only against good traits (value > 0), badfactor for bad (value < 0)

        internal float GetBreedingValueForUniqueGoodTraits(Horse otherHorse)
        {
            return GetBreedingValueForTraits(otherHorse, 0.0F);
        }

        internal float GetBreedingValueForAllGoodTraits(Horse otherHorse, float DuplicateGoodFactor)
        {
            return GetBreedingValueForTraits(otherHorse, DuplicateGoodFactor);
        }

        internal float GetBreedingValueForAllBadTraits(Horse otherHorse, float DuplicateBadFactor)
        {
            return GetBreedingValueForTraits(otherHorse, DuplicateBadFactor, true);
        }

        internal float GetPotentialBreedingValue(Horse otherHorse, float DuplicateGoodFactor, float DuplicateBadFactor)
        {
            HashSet<HorseTrait> InvisibibleTraits = new HashSet<HorseTrait>();
            float totalVal = 0F;
            foreach (var keyval in TraitValues.TraitToValueMap)
            {
                float ahskill = HorseTraitEX.EnumToAHSkillMap[keyval.Key];
                //for current horse
                if (ahskill > otherHorse.TraitsInspectedAtSkill)
                {
                    if (InvisibibleTraits.Add(keyval.Key))
                    {
                        totalVal += keyval.Value;
                    }
                    else
                    {
                        totalVal += keyval.Value * ((keyval.Value > 0F) ? DuplicateGoodFactor : DuplicateBadFactor);
                    }
                }
                //for other horse
                if (ahskill > TraitsInspectedAtSkill)
                {
                    if (InvisibibleTraits.Add(keyval.Key))
                    {
                        totalVal += keyval.Value;
                    }
                    else
                    {
                        totalVal += keyval.Value * ((keyval.Value > 0F) ? DuplicateGoodFactor : DuplicateBadFactor);
                    }
                }
            }
            return totalVal;
        }

        internal float GetBreedingValueForTraits(Horse curHorse, float duplicateFactor, bool negativeTraits = false)
        {
            HashSet<HorseTrait> UniqueTraits = new HashSet<HorseTrait>();
            float totalVal = 0F;
            foreach (HorseTrait trait in Traits)
            {
                float value = TraitValues.TraitToValueMap[trait];
                if (negativeTraits ? value < 0F : value > 0F)
                    if (UniqueTraits.Add(trait)) totalVal += value;
                    else totalVal += value * duplicateFactor;
            }
            foreach (HorseTrait trait in curHorse.Traits)
            {
                float value = TraitValues.TraitToValueMap[trait];
                if (negativeTraits ? value < 0F : value > 0F)
                    if (UniqueTraits.Add(trait)) totalVal += value;
                    else totalVal += value * duplicateFactor;
            }
            return totalVal;
        }
    }
}
