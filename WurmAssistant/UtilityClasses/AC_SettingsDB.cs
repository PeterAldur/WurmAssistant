using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.Xml;
using System.IO;
using System.Diagnostics;

namespace WurmAssistant
{
    public class AC_SettingsDB
    {
        //settings container (string dict)
        Dictionary<string, string> SettingsDict = new Dictionary<string, string>();
        SQLiteDB Database;
        bool SettingsChanged = false;

        //old way
        string SettingsPath;
        bool DBSettingsFailed = false;

        string DBTableName;

        /// <summary>
        /// Initializes new settings handler with specified table name
        /// </summary>
        /// <param name="tableName">_settings string will be appended to this table name</param>
        /// <param name="database">reference to this app database handler</param>
        /// <param name="fileNameAndPath">(deprecated) a path to settings xml file used in older versions</param>
        public AC_SettingsDB(string tableName, SQLiteDB database, string fileNameAndPath = null)
        {
            SettingsPath = fileNameAndPath;
            Database = database;
            tableName = tableName.Trim();
            DBTableName = tableName + "_settings";
            InitTable();
            if (OldXMLSettingsExist())
            {
                if (!MoveXMLSettingsToDB())
                {
                    DBSettingsFailed = true;
                }
                else
                {
                    Logger.WriteLine("Note: Settings have been moved to internal database.");
                }
            }
            LoadFromDB();
        }

        void InitTable()
        {
            string[] fields = { "name PRIMARY KEY NOT NULL", "value" };

            if (Database.CreateTable(DBTableName, fields, true))
            {
                Debug.WriteLine("Failed to create table: " + DBTableName);
            }
        }

        bool OldXMLSettingsExist()
        {
            return File.Exists(SettingsPath);
        }

        bool MoveXMLSettingsToDB()
        {
            try
            {
                LoadFromXML();
                File.Delete(SettingsPath);
                SaveToDB();
                return true;
            }
            catch (Exception _e)
            {
                Logger.WriteLine("! Error while moving XML settings to Database, settings path:" + SettingsPath);
                Logger.LogException(_e);
                return false;
            }
        }

        public string Get(string name_key, string defaultvalue)
        {
            try
            {
                return SettingsDict[name_key];
            }
            catch
            {
                Set(name_key, defaultvalue);
                return defaultvalue;
            }
        }

        public bool Get(string name_key, bool defaultvalue)
        {
            return Convert.ToBoolean(Get(name_key, defaultvalue.ToString()));
        }

        public int Get(string name_key, int defaultvalue)
        {
            return Convert.ToInt32(Get(name_key, defaultvalue.ToString()));
        }

        public double Get(string name_key, double defaultvalue)
        {
            return Convert.ToDouble(Get(name_key, defaultvalue.ToString()));
        }

        public float Get(string name_key, float defaultvalue)
        {
            return Convert.ToSingle(Get(name_key, defaultvalue.ToString()));
        }

        public DateTime Get(string name_key, DateTime defaultvalue)
        {
            return Convert.ToDateTime(Get(name_key, defaultvalue.ToString()));
        }

        public TimeSpan Get(string name_key, TimeSpan defaultvalue)
        {
            return TimeSpan.Parse((Get(name_key, defaultvalue.ToString())));
        }

        public void Set(string setting_name_key, string setting_new_value, bool immediate_save_setting = false)
        {
            if (setting_name_key != null && setting_name_key.Trim() != ""
                && setting_new_value != null && setting_new_value.Trim() != "")
            {
                SettingsDict[setting_name_key] = setting_new_value;
                if (immediate_save_setting) SaveToDB();
                else SettingsChanged = true;
            }
        }

        public void Set(string setting_name_key, bool setting_new_value, bool immediate_save_setting = false)
        {
            Set(setting_name_key, setting_new_value.ToString(), immediate_save_setting);
        }

        public void Set(string setting_name_key, int setting_new_value, bool immediate_save_setting = false)
        {
            Set(setting_name_key, setting_new_value.ToString(), immediate_save_setting);
        }

        public void Set(string setting_name_key, float setting_new_value, bool immediate_save_setting = false)
        {
            Set(setting_name_key, setting_new_value.ToString(), immediate_save_setting);
        }

        public void Set(string setting_name_key, double setting_new_value, bool immediate_save_setting = false)
        {
            Set(setting_name_key, setting_new_value.ToString(), immediate_save_setting);
        }

        public void Set(string setting_name_key, DateTime setting_new_value, bool immediate_save_setting = false)
        {
            Set(setting_name_key, setting_new_value.ToString(), immediate_save_setting);
        }

        public void Set(string setting_name_key, TimeSpan setting_new_value, bool immediate_save_setting = false)
        {
            Set(setting_name_key, setting_new_value.ToString(), immediate_save_setting);
        }

        /// <summary>
        /// First array element cant be "NULL", array elements can't contain ";", emptry string elements are also returned
        /// </summary>
        /// <param name="setting_name_key"></param>
        /// <param name="setting_new_value"></param>
        /// <param name="immediate_save_setting"></param>
        public void SetStrArray(string setting_name_key, string[] setting_new_value, bool immediate_save_setting = false)
        {
            Set(setting_name_key, ConvertArrayToString(setting_new_value), immediate_save_setting);
        }

        /// <summary>
        /// Default value should be NULL, returns null if saved array was empty or null
        /// </summary>
        /// <param name="name_key"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public string[] GetStrArray(string name_key, string[] defaultvalue) //def is null
        {
            string defaultvalueStr = ConvertArrayToString(defaultvalue);
            return ConvertStringtoArray(Get(name_key, defaultvalueStr));
        }

        string[] ConvertStringtoArray(string str)
        {
            if (str == "NULL")
            {
                return null;
            }
            else
            {
                return str.Split(new string[] { ";" }, StringSplitOptions.None);
            }
        }

        string ConvertArrayToString(string[] strArray)
        {
            string result;
            if (strArray == null || strArray.Length == 0)
            {
                result = "NULL";
            }
            else
            {
                result = "";
                foreach (string str in strArray)
                {
                    result += str + ";";
                }
                result = result.Remove(result.Length - 1, 1);
            }
            return result;
        }

        /// <summary>
        /// can set nulls, sets the null as "NULL"
        /// </summary>
        /// <param name="setting_name_key"></param>
        /// <param name="setting_new_value"></param>
        /// <param name="immediate_save_setting"></param>
        public void SetNullableStr(string setting_name_key, string setting_new_value, bool immediate_save_setting = false)
        {
            if (setting_new_value == null) Set(setting_name_key, "NULL", immediate_save_setting);
            else Set(setting_name_key, setting_new_value, immediate_save_setting);
        }

        /// <summary>
        /// can return nulls
        /// </summary>
        /// <param name="name_key"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public string GetNullableStr(string name_key, string defaultvalue)
        {
            string retrStr = Get(name_key, defaultvalue == null ? "NULL" : defaultvalue);
            return retrStr == "NULL" ? null : retrStr;
        }

        public void SaveIfChanged()
        {
            if (SettingsChanged)
            {
                SaveToDB();
                SettingsChanged = false;
            }
        }

        public void SaveToDB()
        {
            if (DBSettingsFailed)
            {
                SaveToXML();
            }
            else
            {
                try
                {
                    Database.BeginTrans();
                    Database.ClearTable(DBTableName);
                    foreach (var keyvalue in SettingsDict)
                    {
                        List<DBField> fields = new List<DBField>();
                        fields.Add(new DBField("name", keyvalue.Key));
                        fields.Add(new DBField("value", keyvalue.Value));
                        Database.Insert(DBTableName, fields);
                    }
                    Database.CommitTrans();
                }
                catch (Exception _e)
                {
                    Database.RollbackTrans(_e);
                    Logger.WriteLine("!!! Error: Could not save settings to DB: " + DBTableName);
                }
            }
        }

        public void LoadFromDB()
        {
            if (DBSettingsFailed)
            {
                LoadFromXML();
            }
            else
            {
                try
                {
                    DataTable querydata = Database.GetDataTable(DBTableName);
                    if (querydata != null && querydata.Rows.Count > 0)
                    {
                        SettingsDict.Clear();
                        foreach (DataRow row in querydata.Rows)
                        {
                            SettingsDict.Add(row["name"].ToString(), row["value"].ToString());
                        }
                    }
                }
                catch (Exception _e)
                {
                    Logger.WriteLine("!!! Error: Could not read settings from DB table: " + DBTableName);
                    Logger.LogException(_e);
                }
            }
        }

        public int Count()
        {
            return SettingsDict.Count;
        }

        #region deprec XML saveload methods maintaned for compatibility

        private void SaveToXML()
        {
            try
            {
                using (XmlWriter writer = XmlWriter.Create(SettingsPath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("StringDictionary");
                    foreach (KeyValuePair<string, string> valuepair in SettingsDict)
                    {
                        writer.WriteStartElement("KeyValuePair");
                        writer.WriteElementString("Key", valuepair.Key);
                        writer.WriteElementString("Value", valuepair.Value);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            catch (Exception _e)
            {
                Logger.WriteLine("!!! Error: Could not save settings to file: " + SettingsPath);
                Logger.LogException(_e);
            }
        }

        private void LoadFromXML()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    SettingsDict.Clear();
                    using (XmlReader reader = XmlReader.Create(SettingsPath))
                    {
                        while (reader.Read())
                        {
                            if (reader.IsStartElement())
                            {
                                if (reader.Name == "KeyValuePair")
                                {
                                    //this needs some love, breaks if key/value is empty
                                    reader.Read();
                                    reader.Read();
                                    string key = reader.Value.ToString();
                                    reader.Read();
                                    reader.Read();
                                    reader.Read();
                                    string value = reader.Value.ToString();
                                    SettingsDict.Add(key, value);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception _e)
            {
                Logger.WriteLine("!!! Error: Could not load settings from file: " + SettingsPath);
                Logger.LogException(_e);
            }
        }

        #endregion

        internal void Clear()
        {
            Database.DropTable(DBTableName);
        }
    }
}
