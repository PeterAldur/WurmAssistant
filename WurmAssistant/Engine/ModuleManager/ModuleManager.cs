using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WurmAssistant
{
    using Granger;

    class ModuleManager
    {
        string PlayerName;
        string WurmLogsDir;

        public static ModuleLogSearcher moduleLogSearcher;
        public ModuleClearTimestamps moduleClearTimestamps;
        public static ModuleTimingAssist moduleTimerAssist;
        public ModuleSoundNotify moduleSoundNotify;
        public ModuleTimers moduleTimers;
        public static ModuleCalendar moduleCalendar;
        public static ModuleGranger moduleGranger;

        List<Module> ModuleList = new List<Module>();

        public ModuleManager(string playerName)
        {
            this.PlayerName = playerName;
            this.WurmLogsDir = WurmPaths.GetLogsDirForPlayer(playerName);
            if (WurmLogsDir == null) throw new Exception("!!! Failed to start ModuleManager for: " + PlayerName);
            else Initialize();
        }

        void Initialize()
        {
            if (moduleLogSearcher == null)
            {
                moduleLogSearcher = new ModuleLogSearcher(WurmLogsDir);
                ModuleList.Add(moduleLogSearcher);
            }
            if (moduleClearTimestamps == null)
            {
                moduleClearTimestamps = new ModuleClearTimestamps();
                ModuleList.Add(moduleClearTimestamps);
            }
            if (moduleTimerAssist == null)
            {
                moduleTimerAssist = new ModuleTimingAssist(PlayerName);
                ModuleList.Add(moduleTimerAssist);
            }
            if (moduleSoundNotify == null)
            {
                moduleSoundNotify = new ModuleSoundNotify(PlayerName);
                ModuleList.Add(moduleSoundNotify);
            }
            if (moduleTimers == null)
            {
                moduleTimers = new ModuleTimers(PlayerName);
                ModuleList.Add(moduleTimers);
            }
            if (moduleCalendar == null)
            {
                moduleCalendar = new ModuleCalendar();
                ModuleList.Add(moduleCalendar);
            }
            if (moduleGranger == null)
            {
                moduleGranger = new ModuleGranger(PlayerName);
                ModuleList.Add(moduleGranger);
            }
        }

        public void OnEngineKilled()
        {
            moduleClearTimestamps = null;
            moduleTimerAssist = null;
            moduleSoundNotify = null;
            moduleTimers = null;
            moduleLogSearcher = null;
            moduleCalendar = null;
            moduleGranger = null;
        }

        internal void BeforeHandleLogs(bool sleepMode)
        {
            foreach (Module module in ModuleList)
            {
                module.BeforeHandleLogs(sleepMode);
            }
        }

        internal void HandleNewLogEvents(NewLogEntries newLogEntries)
        {
            if (newLogEntries.AllEntries.Count > 0)
            {
                foreach (NewLogEntriesContainer container in newLogEntries.AllEntries)
                {
                    foreach (Module module in ModuleList)
                    {
                        module.HandleNewLogEvents(container.Entries, container.Log);
                    }
                }
            }
        }

        internal void AfterHandleLogs(bool sleepMode)
        {
            foreach (Module module in ModuleList)
            {
                module.AfterHandleLogs(sleepMode);
            }
        }

        internal void TryToSaveSettings()
        {
            foreach (Module module in ModuleList)
            {
                module.TryToSaveSettings();
            }
        }

        internal void OnAppClosing()
        {
            foreach (Module module in ModuleList)
            {
                module.ForceSaveSettings();
                module.OnAppClosing();
            }
        }

        internal void OnPollingTick(bool sleepMode)
        {
            foreach (Module module in ModuleList)
            {
                module.OnPollingTick(sleepMode);
            }
        }

        internal void OnEngineWakeUp()
        {
            foreach (Module module in ModuleList)
            {
                if (module.GetType() == typeof(ModuleCalendar)) ((ModuleCalendar)module).OnEngineWakeUp();
            }
        }

        internal void ToggleLogSearcherUI()
        {
            if (moduleLogSearcher != null) moduleLogSearcher.ToggleUI();
        }

        internal void ToggleCalendarUI()
        {
            if (moduleCalendar != null) moduleCalendar.ToggleUI();
        }

        internal void ToggleGrangerUI()
        {
            if (moduleGranger != null) moduleGranger.ToggleUI();
        }

        internal WurmAssistant.DynaMenuItemData GetTimersMenuItem()
        {
            if (moduleTimers != null)
            {
                WurmAssistant.DynaMenuItemData menuItemData = new WurmAssistant.DynaMenuItemData(
                    PlayerName,
                    new WurmAssistant.ToggleUIDelegate(moduleTimers.ToggleUI));
                return menuItemData;
            }
            else return null;
        }

        internal WurmAssistant.DynaMenuItemData GetSoundNotifyMenuItem()
        {
            if (moduleSoundNotify != null)
            {
                WurmAssistant.DynaMenuItemData menuItemData = new WurmAssistant.DynaMenuItemData(
                    PlayerName,
                    new WurmAssistant.ToggleUIDelegate(moduleSoundNotify.ToggleUI));
                return menuItemData;
            }
            else return null;
        }
    }
}
