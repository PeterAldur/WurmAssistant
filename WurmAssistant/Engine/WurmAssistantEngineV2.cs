using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace WurmAssistant
{
    /// <summary>
    /// Wurm Assistant Engine. Handles all the logs and modules, provides all new log messages to all listed modules.
    /// </summary>
    class WurmAssistantEngineV2
    {
        string[] TrackedGameCharacters;

        public bool SleepMode = false;

        List<CharacterEngine> CharEngines = new List<CharacterEngine>();

        /// <summary>
        /// constructs a new instance of Wurm Assistant Engine
        /// </summary>
        /// <param name="wurmloglocation">path to the Wurm Online logs folder</param>
        public WurmAssistantEngineV2(string[] charactersArray)
        {
            this.TrackedGameCharacters = charactersArray;

            if (TrackedGameCharacters == null)
            {
                throw new Exception("Engine Error: no characters tracked, aborting engine startup");
            }
            else
            {
                try
                {
                    foreach (string character in TrackedGameCharacters)
                    {
                        CharEngines.Add(new CharacterEngine(character));
                    }
                }
                catch (Exception _e)
                {
                    Logger.WriteLine("! Error while starting engines");
                    Logger.LogException(_e);
                    throw;
                }
            }
        }

        /// <summary>
        /// Retrieves new log messages from log wrappers and forwards to all modules
        /// </summary>
        public void Update()
        {
            foreach (CharacterEngine engine in CharEngines)
            {
                engine.Update(SleepMode);
            }
        }

        void UpdateIfDisplayLogEntries()
        {
            foreach (CharacterEngine engine in CharEngines)
            {
                engine.UpdateIfDisplayLogEntries();
            }
        }

        public void TryToSaveModuleSettings()
        {
            foreach (CharacterEngine engine in CharEngines)
            {
                engine.TryToSaveModuleSettings();
            }
        }

        public void AppSettingsChanged()
        {
            foreach (CharacterEngine engine in CharEngines)
            {
                engine.UpdateIfDisplayLogEntries();
            }
        }

        internal void OnAppClosing()
        {
            foreach (CharacterEngine engine in CharEngines)
            {
                engine.OnAppClosing();
            }
        }

        public void UpdateOnPollingLoop()
        {
            foreach (CharacterEngine engine in CharEngines)
            {
                engine.UpdateOnPollingLoop(SleepMode);
            }
        }

        internal void OnEngineWakeUp()
        {
            foreach (CharacterEngine engine in CharEngines)
            {
                engine.OnEngineWakeUp();
            }
        }

        public void KillAllModules() //this has to be called when engine is killed, to free static modules as well
        {
            foreach (CharacterEngine engine in CharEngines)
            {
                engine.OnEngineKilled();
            }
        }

        internal void ToggleLogSearcherUI()
        {
            try
            {
                CharEngines[0].ToggleLogSearcherUI();
            }
            catch (Exception _e)
            {
                Logger.WriteLine("! Error while attempting to open LogSearcher");
                Logger.LogException(_e);
            }
        }

        internal void ToggleCalendarUI()
        {
            try
            {
                CharEngines[0].ToggleCalendarUI();
            }
            catch (Exception _e)
            {
                Logger.WriteLine("! Error while attempting to open LogSearcher");
                Logger.LogException(_e);
            }
        }

        internal void ToggleGrangerUI()
        {
            try
            {
                CharEngines[0].ToggleGrangerUI();
            }
            catch (Exception _e)
            {
                Logger.WriteLine("! Error while attempting to open LogSearcher");
                Logger.LogException(_e);
            }
        }

        internal List<WurmAssistant.DynaMenuItemData> GetTimersMenuItems()
        {
            List<WurmAssistant.DynaMenuItemData> menuItemData = new List<WurmAssistant.DynaMenuItemData>();
            foreach (CharacterEngine engine in CharEngines)
            {
                menuItemData.Add(engine.GetTimersMenuItem());
            }
            return menuItemData;
        }

        internal List<WurmAssistant.DynaMenuItemData> GetSoundNotifyMenuItems()
        {
            List<WurmAssistant.DynaMenuItemData> menuItemData = new List<WurmAssistant.DynaMenuItemData>();
            foreach (CharacterEngine engine in CharEngines)
            {
                menuItemData.Add(engine.GetSoundNotifyMenuItem());
            }
            return menuItemData;
        }
    }
}
