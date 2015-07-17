using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace WurmAssistant
{
    public class AC_Settings_deprec
    {
        //settings container (string dict)
        Dictionary<string, string> SettingsDict = new Dictionary<string, string>();

        //loc for the app settings xml file
        string SettingsPath;

        public AC_Settings_deprec(string fileNameAndPath)
        {
            SettingsPath = fileNameAndPath;
            LoadFromFile();
        }

        //get setting public mtd
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

        //set setting public mtd
        public void Set(string setting_name_key, string setting_new_value, bool immediate_save_setting = true)
        {
            if (setting_name_key != null && setting_name_key.Trim() != ""
                && setting_new_value != null && setting_new_value.Trim() != "")
            {
                SettingsDict[setting_name_key] = setting_new_value;
                if (immediate_save_setting) SaveToFile();
            }
        }

        public void Set(string setting_name_key, bool setting_new_value, bool immediate_save_setting = true)
        {
            Set(setting_name_key, setting_new_value.ToString(), immediate_save_setting);
        }

        public void Set(string setting_name_key, int setting_new_value, bool immediate_save_setting = true)
        {
            Set(setting_name_key, setting_new_value.ToString(), immediate_save_setting);
        }

        public void Set(string setting_name_key, float setting_new_value, bool immediate_save_setting = true)
        {
            Set(setting_name_key, setting_new_value.ToString(), immediate_save_setting);
        }

        public void Set(string setting_name_key, double setting_new_value, bool immediate_save_setting = true)
        {
            Set(setting_name_key, setting_new_value.ToString(), immediate_save_setting);
        }

        //save settings to file prv method
        public void SaveToFile()
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
        //load settings from file prv method
        public void LoadFromFile()
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

        public int Count()
        {
            return SettingsDict.Count;
        }
    }
}
