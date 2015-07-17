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
    public class Herd
    {
        ModuleGranger ParentModule;
        SQLiteDB DB;

        string herdID;
        public string HerdID
        {
            get { return herdID; }
            set
            {
                herdID = value;
                DBTableName = BuildTableName(herdID);
            }
        }
        string DBTableName;
        public Dictionary<string, Horse> Horses = new Dictionary<string, Horse>();

        public Herd(SQLiteDB db, string herdID, ModuleGranger parentModule)
        {
            this.DB = db;
            this.ParentModule = parentModule;
            this.HerdID = herdID;
            //DBTableName = BuildTableName(HerdID);
            db.CreateTable(DBTableName, Horse.DBFieldNames, true);
            PopulateFromDB(db);
        }

        string BuildTableName(string herdid)
        {
            return "Granger_Herd_" + herdid;
        }

        public string GetTableName()
        {
            return DBTableName;
        }

        public void Save(SQLiteDB db)
        {
            db.ClearTable(DBTableName);
            foreach (Horse horse in Horses.Values)
            {
                horse.Save(db, DBTableName);
            }
        }

        public void PopulateFromDB(SQLiteDB db)
        {
            DataTable dt = db.GetDataTable(DBTableName);
            foreach (DataRow dtrow in dt.Rows)
            {
                Horse horse = Horse.ConstructHorse(dtrow);
                Horses[horse.Name] = horse;
            }
        }

        public void Delete(SQLiteDB db)
        {
            db.DropTable(DBTableName);
            Horses.Clear();
        }

        internal bool HorseExists(Horse horse)
        {
            if (Horses.Keys.Contains(horse.Name)) return true;
            else return false;
        }

        internal bool UpdateHorse(Horse horse)
        {
            Horse savedhorse = Horses[horse.Name];
            //verify genders
            if (savedhorse.IsMale != horse.IsMale)
            {
                ParentModule.NotifyUser("FAILED to update horse '" + horse.Name + "' in '" + this.HerdID + "' herd, reason: different gender!", delay: 5000);
                return false;
            }
            //verify mother and father
            if (savedhorse.Mother != horse.Mother && savedhorse.Mother != "" && horse.Mother != ""
                || savedhorse.Father != horse.Father && savedhorse.Father != "" && horse.Father != "")
            {
                ParentModule.NotifyUser("FAILED to update horse '" + horse.Name + "' in '" + this.HerdID + "' herd, reason: parents do not match!", delay: 5000);
                return false;
            }
            //verify traits
            int lowestAHSkill = savedhorse.TraitsInspectedAtSkill > horse.TraitsInspectedAtSkill ? horse.TraitsInspectedAtSkill : savedhorse.TraitsInspectedAtSkill;
            IEnumerable<HorseTrait> comparableTraits = HorseTraitEX.EnumToAHSkillMap.Where(w => w.Value <= lowestAHSkill).Select(w => w.Key);
            foreach (var trait in comparableTraits)
            {
                if (horse.Traits.Contains(trait) != savedhorse.Traits.Contains(trait))
                {
                    ParentModule.NotifyUser("FAILED to update horse '" + horse.Name + "' in '" + this.HerdID + "' herd, reason: traits do not match!", delay: 5000);
                    return false;
                }
            }
            //apply changes
            savedhorse.Father = horse.Father;
            savedhorse.Mother = horse.Mother;
            savedhorse.TakenCareOfBy = horse.TakenCareOfBy;
            savedhorse.PregnantTo = horse.PregnantTo;
            //update traits based on animal husbandry skill
            if (savedhorse.TraitsInspectedAtSkill > horse.TraitsInspectedAtSkill)
            {
                savedhorse.TraitsInspectedAtSkill = horse.TraitsInspectedAtSkill;
                savedhorse.Traits = horse.Traits;
            }
            ParentModule.NotifyUser("Updated horse '" + horse.Name + "' in '" + this.HerdID + "' herd");
            return true;
        }

        internal void AddHorse(Horse horse)
        {
            try
            {
                Horses.Add(horse.Name, horse);
                ParentModule.NotifyUser("Added new horse '" + horse.Name + "' to '" + this.HerdID + "' herd");
            }
            catch (Exception _e)
            {
                Logger.WriteLine("! Granger: error while trying to add horse " + horse.Name + " to herd " + this.HerdID);
                Logger.LogException(_e);
            }
        }

        #region deprec
        internal void SortByName()
        {
            List<KeyValuePair<string, Horse>> list = Horses.ToList();
            list.Sort((x, y) => x.Key.CompareTo(y.Key));
            Horses = list.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        internal void SortByValue()
        {
            List<KeyValuePair<string, Horse>> list = Horses.ToList();
            list.Sort((x, y) => TraitValues.CalculateHorseValue(x.Value).CompareTo(TraitValues.CalculateHorseValue(y.Value)));
            Horses = list.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        internal void SortByMother()
        {
            List<KeyValuePair<string, Horse>> list = Horses.ToList();
            list.Sort((x, y) => x.Value.Mother.CompareTo(y.Value.Mother));
            Horses = list.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        internal void SortByFather()
        {
            List<KeyValuePair<string, Horse>> list = Horses.ToList();
            list.Sort((x, y) => x.Value.Father.CompareTo(y.Value.Father));
            Horses = list.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        internal void SortByPregnant()
        {
            List<KeyValuePair<string, Horse>> list = Horses.ToList();
            list.Sort((x, y) => x.Value.PregnantTo.CompareTo(y.Value.PregnantTo));
            Horses = list.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        internal void SortByGender()
        {
            List<KeyValuePair<string, Horse>> list = Horses.ToList();
            list.Sort((x, y) => x.Value.IsMale.CompareTo(y.Value.IsMale));
            Horses = list.ToDictionary(pair => pair.Key, pair => pair.Value);
        }
        #endregion

        internal void SortBy(HorseSortingOption horseSortingOption)
        {
            List<KeyValuePair<string, Horse>> list = Horses.ToList();
            switch (horseSortingOption)
            {
                case HorseSortingOption.Name:
                    list.Sort((x, y) => x.Key.CompareTo(y.Key));
                    break;
                case HorseSortingOption.Gender:
                    list.Sort((x, y) => x.Value.IsMale.CompareTo(y.Value.IsMale));
                    break;
                case HorseSortingOption.Father:
                    list.Sort((x, y) => x.Value.Father.CompareTo(y.Value.Father));
                    break;
                case HorseSortingOption.Mother:
                    list.Sort((x, y) => x.Value.Mother.CompareTo(y.Value.Mother));
                    break;
                case HorseSortingOption.Value:
                    list.Sort((x, y) => TraitValues.CalculateHorseValue(x.Value).CompareTo(TraitValues.CalculateHorseValue(y.Value)));
                    list.Reverse();
                    break;
                case HorseSortingOption.Potential:
                    list.Sort((x, y) => TraitValues.CalculateHorsePotentialValue(x.Value).CompareTo(TraitValues.CalculateHorsePotentialValue(y.Value)));
                    list.Reverse();
                    break;
                case HorseSortingOption.Breeded:
                    list.Sort((x, y) => x.Value.NotInMood.CompareTo(y.Value.NotInMood));
                    break;
                case HorseSortingOption.Groomed:
                    list.Sort((x, y) => x.Value.GroomedOn.CompareTo(y.Value.GroomedOn));
                    break;
                case HorseSortingOption.Pregnant:
                    list.Sort((x, y) => x.Value.PregnantTo.CompareTo(y.Value.PregnantTo));
                    break;
                case HorseSortingOption.Comment:
                    list.Sort((x, y) => x.Value.Comments.CompareTo(y.Value.Comments));
                    break;
                default:
                    break;
            }
            Horses = list.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        internal bool Rename(string newherdid)
        {
            if (RenameDBTable(newherdid))
            {
                this.HerdID = newherdid;
                return true;
            }
            else return false;
        }

        internal void IncludeAnotherHerd(Herd oldherd)
        {
            foreach (var horse in oldherd.Horses)
                this.Horses.Add(horse.Key, horse.Value);
        }

        internal bool DisposeDBTable()
        {
            if (DB.DropTable(this.GetTableName()))
            {
                return true;
            }
            else return false;
        }

        private bool RenameDBTable(string newherdid)
        {
            Exception exc;
            if (DB.RenameTable(this.GetTableName(), BuildTableName(newherdid), out exc)) return true;
            else
            {
                if (exc.Message.Contains("no such table")) return true;
                else return false;
            }
        }

        internal void WipeDB(SQLiteDB db)
        {
            db.DropTable(DBTableName);
        }
    }
}