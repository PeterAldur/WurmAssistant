using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace WurmAssistant
{
    /// <summary>
    /// Base for all modules. Provides virtual methods for handling new log messages, pre- and post-handling, creating module and managing settings.
    /// Settings are saved in individual module folder named as module name provided in constructor, in XML format.
    /// By default: settings loaded at module init and saved at app exit. Can be force saved/loaded through base methods.
    /// All settings keys are as string. Convert ToString() on saving and to appropriate type on load. 
    /// Settings are stored in dictionary, to avoid inefficiency load settings to local variables.
    /// </summary>
    public class Module
    {
        private string _moduleFolderPath;

        /// <summary>
        /// Path to directory storing module data, name of bottom dir identical to module name
        /// </summary>
        protected string ModuleFolderPath
        {
            get 
            {
                return _moduleFolderPath; 
            }
            private set
            {
                if (!value.EndsWith(@"\")) value += @"\";
                _moduleFolderPath = value;
            }
        }

        private string _moduleName = "UnknownModule";

        /// <summary>
        /// Name of this module, used also as bottom dir name 
        /// </summary>
        protected string ModuleName
        {
            get { return _moduleName; }
        }

        private Dictionary<string, string> LocalModuleSettings = new Dictionary<string, string>();

        protected AC_SettingsDB ModuleSettings;

        /// <summary>
        /// Constructs new module using specified name as default module directory
        /// </summary>
        /// <param name="moduleName">provide path-friendly module name (avoid non-letters)</param>
        protected Module(string moduleName)
        {
            if (moduleName != null && moduleName != "")
                this._moduleName = moduleName;
            else Logger.WriteLine(@"! WARNING: A module name was not specified in constructor, Modules\UnknownModule directory will be used until this is fixed");

            Logger.WriteLine("Initializing module: " + this.ModuleName);
            this.ModuleFolderPath = @"Modules\" + this.ModuleName;

            ModuleFolderPath = Path.Combine(WurmAssistant.PersistentDataDir, this.ModuleFolderPath);

            if (!Directory.Exists(ModuleFolderPath)) { Directory.CreateDirectory(ModuleFolderPath); }

            string moduleSettingsOldXMLfilePath = Path.Combine(ModuleFolderPath, Path.GetFileNameWithoutExtension(ModuleName) + ".xml");
            ModuleSettings = new AC_SettingsDB(ModuleName, WurmAssistant.Database, moduleSettingsOldXMLfilePath);

            try
            {
                LoadSettings();
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("Note: No settings file found for module: "+ModuleFolderPath);
            }
        }

        ///// <summary>
        ///// Sets specified setting key with specified new value. Will throw exception if key does not exist.
        ///// By def, settings are saved at app exit.
        ///// </summary>
        ///// <param name="setting_name_key">Name of setting, case sensitive</param>
        ///// <param name="setting_new_value">New value for the setting, use Convert correct for nonstrings</param>
        ///// <param name="immediate_save_setting">true saves to file immediatelly, false at default</param>
        //protected void SetSetting(string setting_name_key, string setting_new_value, bool immediate_save_setting = false)
        //{
        //    Module_Settings.SetSetting(setting_name_key, setting_new_value, immediate_save_setting);
        //}

        ///// <summary>
        ///// Retrieves value for the specified setting. Throws exception if setting key not found.
        ///// By def, settings are loaded at module init (before derived module constructor)
        ///// </summary>
        ///// <param name="name_key">Name of the setting, case sensitive</param>
        ///// <returns>Value of the setting as string</returns>
        //protected string GetSetting(string name_key, string defaultvalue)
        //{
        //    return Module_Settings.GetSetting(name_key, defaultvalue);
        //}

        /// <summary>
        /// Override this to do any regular updates necessary in this module. Executes once per timer tick, always before HandleNewLogEvents.
        /// </summary>
        virtual public void BeforeHandleLogs(bool engineInSleepMode)
        {
        }

        /// <summary>
        /// Override this to handle new log events, this method executes once per every log that has new message, per timer tick
        /// </summary>
        /// <param name="newLogEvents">List of all new wurm log messages, one event per line</param>
        /// <param name="log">Reference to the log handler that provided these messages</param>
        virtual public void HandleNewLogEvents(List<string> newLogEvents, GameLogState log) 
        { 
        }

        /// <summary>
        /// Override this to do anything after HandleNewLogEvents.
        /// </summary>
        virtual public void AfterHandleLogs(bool engineInSleepMode)
        {
        }

        public void TryToSaveSettings()
        {
            ModuleSettings.SaveIfChanged();
        }

        /// <summary>
        /// Saves the dictionary containing all settings to xml file. Set the settings before calling this method.
        /// Def: autosaved on app close
        /// </summary>
        public void ForceSaveSettings()
        {
            if (ModuleFolderPath == null) throw new Exception("ModuleFolderName not specified");

            ModuleSettings.SaveToDB();
        }

        /// <summary>
        /// Loads all module settings from xml file, use explicitly to force reload.
        /// Def: autoloaded at base init
        /// </summary>
        protected void LoadSettings()
        {
            if (ModuleFolderPath == null) throw new Exception("ModuleFolderName not specified");

            ModuleSettings.LoadFromDB();
        }

        virtual public void OnAppClosing()
        {
        }

        virtual public void OnPollingTick(bool engineInSleepMode)
        {
        }
    }
}
