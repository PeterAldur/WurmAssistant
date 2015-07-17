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
    public class TraitValues
    {
        public static Dictionary<HorseTrait, float> TraitToValueMap = new Dictionary<HorseTrait, float>();
        static string DBTableName = "Granger_TraitValues";
        static string[] DBFieldNames = { "trait", "value" };

        public TraitValues(SQLiteDB db)
        {
            GrangerDebug.Log("GRANGERINIT: building default trait list");
            TraitToValueMap.Clear();
            TraitToValueMap.Add(HorseTrait.FightFiercely, 0F);
            TraitToValueMap.Add(HorseTrait.FleeterMovement, 40F);
            TraitToValueMap.Add(HorseTrait.ToughBugger, 0F);
            TraitToValueMap.Add(HorseTrait.StrongBody, 30F);
            TraitToValueMap.Add(HorseTrait.LightningMovement, 45F);
            TraitToValueMap.Add(HorseTrait.CarryMoreThanAverage, 30F);
            TraitToValueMap.Add(HorseTrait.VeryStrongLegs, 40F);
            TraitToValueMap.Add(HorseTrait.KeenSenses, 0F);
            TraitToValueMap.Add(HorseTrait.MalformedHindlegs, -30F);
            TraitToValueMap.Add(HorseTrait.LegsOfDifferentLength, -45F);
            TraitToValueMap.Add(HorseTrait.OverlyAggressive, 0F);
            TraitToValueMap.Add(HorseTrait.VeryUnmotivated, -35F);
            TraitToValueMap.Add(HorseTrait.UnusuallyStrongWilled, 0F);
            TraitToValueMap.Add(HorseTrait.HasSomeIllness, -100F);
            TraitToValueMap.Add(HorseTrait.ConstantlyHungry, -5F);
            TraitToValueMap.Add(HorseTrait.FeebleAndUnhealthy, -5F);
            TraitToValueMap.Add(HorseTrait.StrongAndHealthy, 5F);
            TraitToValueMap.Add(HorseTrait.CertainSpark, 0F);

            GrangerDebug.Log("GRANGERINIT: creating trait db table if not exists");
            db.CreateTable(DBTableName, DBFieldNames, true);
            GrangerDebug.Log("GRANGERINIT: attempting to load trait values");
            Load(db);
        }

        public void Update(string traitname, decimal value)
        {
            TraitToValueMap[HorseTraitEX.NameToEnumMap[traitname]] = (float)value;
        }

        public void Save(SQLiteDB db)
        {
            try
            {
                db.BeginTrans();
                foreach (var keyval in TraitToValueMap)
                {
                    List<DBField> fields = new List<DBField>();
                    fields.Add(new DBField("trait", keyval.Key.ToString()));
                    fields.Add(new DBField("value", keyval.Value.ToString()));
                    if (!db.Update(DBTableName, fields, "trait='" + keyval.Key.ToString() + "'"))
                        db.Insert(DBTableName, fields);
                }
                db.CommitTrans();
            }
            catch (Exception _e)
            {
                db.RollbackTrans();
                Logger.WriteLine("! Rollback while saving trait values to DB");
                Logger.LogException(_e);
            }
        }

        public void Load(SQLiteDB db)
        {
            DataTable dt = db.GetDataTable(DBTableName);
            if (dt.Rows.Count > 0) GrangerDebug.Log("GRANGERINIT: applying saved trait values");
            foreach (DataRow drow in dt.Rows)
            {
                TraitToValueMap[(HorseTrait)Enum.Parse(typeof(HorseTrait), drow["trait"].ToString())] = float.Parse(drow["value"].ToString());
            }
        }

        public static float CalculateHorseValue(Horse horse)
        {
            float result = 0F;
            foreach (HorseTrait trait in horse.Traits)
            {
                result += TraitToValueMap[trait];
            }
            return result;
        }

        public static float CalculateHorsePotentialValue(Horse horse)
        {
            float solidvalue = CalculateHorseValue(horse);
            float potentialplus, potentialminus;
            CalculateHorsePotentialValues(horse, out potentialplus, out potentialminus);
            return solidvalue + potentialminus + potentialplus;
        }

        static void CalculateHorsePotentialValues(Horse horse, out float potentialplus, out float potentialminus)
        {
            potentialplus = 0F;
            potentialminus = 0F;
            foreach (var keyval in HorseTraitEX.EnumToAHSkillMap)
            {
                if (!horse.Traits.Contains(keyval.Key))
                {
                    if (horse.TraitsInspectedAtSkill < keyval.Value)
                    {
                        float potvalue;
                        TraitToValueMap.TryGetValue(keyval.Key, out potvalue);
                        if (potvalue > 0F)
                        {
                            potentialplus += potvalue;
                        }
                        else
                        {
                            potentialminus += potvalue;
                        }
                    }
                }
            }
        }

        internal static string CalculatePotentialHorseValueForDisplay(Horse horse)
        {
            float potentialplus, potentialminus;
            CalculateHorsePotentialValues(horse, out potentialplus, out potentialminus);
            return "(+" + potentialplus.ToString() + ") (" + potentialminus.ToString() + ")";
        }

        internal static bool HasAnyNegatives(Horse horse)
        {
            foreach (HorseTrait trait in horse.Traits)
            {
                if (TraitToValueMap[trait] < 0F) return true;
            }
            return false;
        }

        internal static void getMaxAndMinValues(out float maxVal, out float minVal)
        {
            maxVal = minVal = 0F;
            foreach (float val in TraitToValueMap.Values)
            {
                if (val > maxVal) maxVal = val;
                if (val < minVal) minVal = val;
            }
        }

        internal static float GetHighestCombinedPositiveTraitValue(float DuplicateGoodFactor)
        {
            float returnTotal = 0F;
            foreach (float value in TraitValues.TraitToValueMap.Values)
            {
                if (value > 0F) returnTotal += value + value * DuplicateGoodFactor;
            }
            return returnTotal;
        }

        internal static void WipeDB(SQLiteDB db)
        {
            db.DropTable(DBTableName);
        }
    }
}