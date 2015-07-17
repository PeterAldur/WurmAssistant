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

    public class HerdList
    {
        ModuleGranger ParentModule;
        SQLiteDB DB;

        Dictionary<string, Herd> Herds = new Dictionary<string, Herd>();
        static string DBTableName = "Granger_HerdTableNames";
        static string[] DBFieldNames = { "herdid", "tablename" };

        AC_SettingsDB ModuleSettings;

        Herd currentHerd = null;
        public Herd CurrentHerd
        {
            get { return currentHerd; }
        }
        Herd newHorseTargetHerd = null;
        public Herd NewHorseTargetHerd
        {
            get { return newHorseTargetHerd; }
        }

        public bool TryFindHorse(string name, out Horse horse, bool fixCase = false)
        {
            try
            {
                if (fixCase)
                {
                    char firstletter = name[0];
                    firstletter = Char.ToUpperInvariant(firstletter);
                    name = firstletter + name.Substring(1, name.Length - 1);
                }
                foreach (var herd in Herds)
                {
                    if (herd.Value.Horses.TryGetValue(name, out horse))
                    {
                        return true;
                    }
                }
                horse = null;
                return false;
            }
            catch
            {
                horse = null;
                return false;
            }
        }

        public HerdList(AC_SettingsDB moduleSettings, SQLiteDB db, ModuleGranger parentModule)
        {
            this.DB = db;
            this.ParentModule = parentModule;
            this.ModuleSettings = moduleSettings;
            db.CreateTable(DBTableName, DBFieldNames, true);
            GrangerDebug.Log("GRANGERINIT: loading herds");
            Load(db);

            string savedHerdID = ModuleSettings.GetNullableStr("CurrentHerd", null);
            string savedNewHorseTargetHerd = ModuleSettings.GetNullableStr("NewHorseTargetHerd", null);
            SetSelectedHerds(savedHerdID, savedNewHorseTargetHerd);
        }

        public void SetSelectedHerds(string newCurrentHerdID = null, string newHorseTargetHerdID = null)
        {
            if (newCurrentHerdID != null)
            {
                try
                {
                    //try to set new herd to provided value
                    SetCurrentHerd(Herds[newCurrentHerdID].HerdID);
                }
                catch
                {
                    //select default if failed
                    SelectDefaultAsCurrentHerd();
                }
            }
            else
            {
                //check if this herd is valid
                if (!(CurrentHerd != null && Herds.ContainsKey(CurrentHerd.HerdID)))
                {
                    if (Herds.Count > 0)
                    {
                        SetCurrentHerd(Herds.Keys.First()); //select any that exists
                    }
                    else
                    {
                        SelectDefaultAsCurrentHerd(); //select default if empty (will create if not exists)
                    }
                }
            }

            if (newHorseTargetHerdID != null)
            {
                try
                {
                    SetNewHorseTargetHerd(Herds[newHorseTargetHerdID].HerdID);
                }
                catch
                {
                    SelectDefaultAsTargetHerd();
                }
            }
            else
            {
                if (newHorseTargetHerd == null || !Herds.ContainsKey(newHorseTargetHerd.HerdID))
                    SelectDefaultAsTargetHerd();
            }
        }

        void SelectDefaultAsCurrentHerd()
        {
            try
            {
                SetCurrentHerd(Herds["Default"].HerdID);
            }
            catch
            {
                CreateDefaultHerd();
                SetCurrentHerd(Herds["Default"].HerdID);
            }
        }

        void SelectDefaultAsTargetHerd()
        {
            try
            {
                SetNewHorseTargetHerd(Herds["Default"].HerdID);
            }
            catch
            {
                CreateDefaultHerd();
                SetNewHorseTargetHerd(Herds["Default"].HerdID);
            }
        }

        void CreateDefaultHerd()
        {
            Herd herd = new Herd(DB, "Default", ParentModule);
            Herds.Add(herd.HerdID, herd);
            currentHerd = herd;
        }

        void SetCurrentHerd(string id)
        {
            currentHerd = Herds[id];
            ModuleSettings.SetNullableStr("CurrentHerd", currentHerd.HerdID);
        }

        void SetNewHorseTargetHerd(string id)
        {
            newHorseTargetHerd = Herds[id];
            ModuleSettings.SetNullableStr("NewHorseTargetHerd", newHorseTargetHerd.HerdID);
        }

        public string[] GetHerdIDs()
        {
            return Herds.Keys.ToArray();
        }

        public void Save(SQLiteDB db)
        {
            try
            {
                db.BeginTrans();
                db.ClearTable(DBTableName);
                foreach (var keyval in Herds)
                {
                    List<DBField> fields = new List<DBField>();
                    fields.Add(new DBField("herdid", keyval.Key));
                    fields.Add(new DBField("tablename", keyval.Value.GetTableName()));
                    //if (!db.Update(DBTableName, fields, "herdid='" + keyval.Key + "'"))
                    db.Insert(DBTableName, fields);
                    keyval.Value.Save(db);
                }
                db.CommitTrans();
            }
            catch (Exception _e)
            {
                db.RollbackTrans();
                Logger.WriteLine("! Rollback while saving herds list to DB");
                Logger.LogException(_e);
            }
        }

        public void Load(SQLiteDB db)
        {
            DataTable dt = db.GetDataTable(DBTableName);
            foreach (DataRow dtrow in dt.Rows)
            {
                Herd herd = new Herd(db, dtrow["herdid"].ToString(), ParentModule);
                Herds.Add(dtrow["herdid"].ToString(), herd);
                GrangerDebug.Log("GRANGERINIT: loaded herd: "+herd.HerdID);
            }
        }

        internal bool HandleNewHorse(Horse horse)
        {
            GrangerDebug.Log("HERD: attempting to add/update new horse");
            // check if horse exists in any herd
            foreach (Herd herd in Herds.Values)
            {
                // if yes, update data about this horse
                if (herd.HorseExists(horse))
                {
                    GrangerDebug.Log("HERD: found existing horse in herd "+herd.HerdID+", applying");
                    herd.UpdateHorse(horse);
                    GrangerDebug.Log("applied");
                    Save(DB);
                    return true;
                }
            }
            // if no, add this horse to current new target, if set
            if (newHorseTargetHerd != null)
            {
                GrangerDebug.Log("HERD: adding new horse");
                newHorseTargetHerd.AddHorse(horse);
                GrangerDebug.Log("added");
                Save(DB);
                return true;
            }
            GrangerDebug.Log("HERD: failed to add new horse, target herd not set?");
            return false;
        }

        internal bool CreateNewHerd(string newherdid)
        {
            try
            {
                Herds.Add(newherdid, new Herd(DB, newherdid, ParentModule));
                Save(DB);
                return true;
            }
            catch (Exception _e)
            {
                Logger.WriteLine("! Granger: Error while creating new herd: " + newherdid);
                Logger.LogException(_e);
                return false;
            }
        }

        internal bool RenameHerd(string oldherdid, string newherdid)
        {
            try
            {
                Herd herd = Herds[oldherdid];
                if (herd.Rename(newherdid))
                {
                    Herds.Remove(oldherdid);
                    Herds.Add(newherdid, herd);
                    SetSelectedHerds(newCurrentHerdID: newherdid);
                    Save(DB);
                    return true;
                }
                else return false;
            }
            catch (Exception _e)
            {
                Logger.WriteLine("! Granger: Error while renaming herd: " + oldherdid + " to: " + newherdid);
                Logger.LogException(_e);
                return false;
            }
        }

        internal bool MergeHerds(string oldherdid, string targetherdid)
        {
            try
            {
                Herd oldherd = Herds[oldherdid];
                Herd targetherd = Herds[targetherdid];
                if (oldherd.DisposeDBTable())
                {
                    targetherd.IncludeAnotherHerd(oldherd);
                    Herds.Remove(oldherd.HerdID);
                    SetSelectedHerds(newCurrentHerdID: targetherd.HerdID);
                    Save(DB);
                    return true;
                }
                else return false;
            }
            catch (Exception _e)
            {
                Logger.WriteLine("! Granger: Error while merging herd: " + oldherdid + " into: " + targetherdid);
                Logger.LogException(_e);
                return false;
            }
        }

        internal bool DeleteHerd(Herd herd)
        {
            try
            {
                if (herd.DisposeDBTable())
                {
                    Herds.Remove(herd.HerdID);
                    SetSelectedHerds();
                    Save(DB);
                    return true;
                }
                else return false;
            }
            catch (Exception _e)
            {
                Logger.WriteLine("! Granger: Error while deleting herd: " + herd.HerdID);
                Logger.LogException(_e);
                return false;
            }
        }

        internal string[] GetListOfAllHorseNames()
        {
            List<string> allHorseNames = new List<string>();
            foreach (var herd in Herds)
            {
                allHorseNames.AddRange(herd.Value.Horses.Keys.ToArray());
            }
            allHorseNames.Sort();
            return allHorseNames.ToArray();
        }

        internal bool HorseExists(string name)
        {
            Horse horse;
            if (TryFindHorse(name, out horse, fixCase: true)) return true;
            else return false;
        }

        public bool RenameHorse(Horse horse, string newName)
        {
            if (CurrentHerd.HorseExists(horse))
            {
                if (CurrentHerd.Horses.Remove(horse.Name))
                {
                    string oldname = horse.Name;
                    horse.Name = newName;
                    try
                    {
                        CurrentHerd.Horses.Add(horse.Name, horse);
                        Save(DB);
                        return true;
                    }
                    catch
                    {
                        horse.Name = oldname;
                        CurrentHerd.Horses.Add(horse.Name, horse);
                    }
                }
            }
            return false;
        }

        internal bool MoveHorse(Horse horse, string oldHerd, string newHerd)
        {
            if (CurrentHerd.HerdID != oldHerd)
            {
                Logger.WriteLine("Granger: move horse error: current herd is not the source herd!");
                return false;
            }
            Herd newHerdObj;
            if (!Herds.TryGetValue(newHerd, out newHerdObj))
            {
                Logger.WriteLine("Granger: move horse error: destination herd does not exist?");
                return false;
            }
            else
            {
                if (CurrentHerd.Horses.Remove(horse.Name))
                {
                    newHerdObj.Horses.Add(horse.Name, horse);
                    Save(DB);
                    return true;
                }
            }
            return false;
        }

        internal void WipeDB(SQLiteDB db)
        {
            foreach (var herd in Herds)
            {
                herd.Value.WipeDB(db);
            }
            db.DropTable(DBTableName);
        }
    }
}