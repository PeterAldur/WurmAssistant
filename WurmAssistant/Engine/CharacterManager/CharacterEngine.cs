using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;

namespace WurmAssistant
{
    class CharacterEngine
    {
        string PlayerName;

        LogsManager LogMan;
        ModuleManager ModuleMan;

        public CharacterEngine(string playerName)
        {
            this.PlayerName = playerName;

            LogMan = new LogsManager(PlayerName);
            ModuleMan = new ModuleManager(PlayerName);
        }

        public void ManagePMLogs()
        {
            LogMan.ManagePMLogs();
        }

        public void Update(bool sleepMode)
        {
            if (!sleepMode)
            {
                ManagePMLogs();
            }

            ModuleMan.BeforeHandleLogs(sleepMode);

            if (!sleepMode)
            {
                ModuleMan.HandleNewLogEvents(LogMan.UpdateAndGetNewEvents());
            }

            ModuleMan.AfterHandleLogs(sleepMode);
        }

        public void UpdateIfDisplayLogEntries()
        {
            LogMan.UpdateIfDisplayLogEntries();
        }

        public void TryToSaveModuleSettings()
        {
            ModuleMan.TryToSaveSettings();
        }

        public void AppSettingsChanged()
        {
            UpdateIfDisplayLogEntries();
        }

        public void OnAppClosing()
        {
            ModuleMan.OnAppClosing();
        }

        public void UpdateOnPollingLoop(bool sleepMode)
        {
            ModuleMan.OnPollingTick(sleepMode);
        }

        internal void OnEngineWakeUp()
        {
            ModuleMan.OnEngineWakeUp();
        }

        public void OnEngineKilled()
        {
            ModuleMan.OnEngineKilled();
        }

        internal void ToggleLogSearcherUI()
        {
            ModuleMan.ToggleLogSearcherUI();
        }

        internal void ToggleCalendarUI()
        {
            ModuleMan.ToggleCalendarUI();
        }

        internal void ToggleGrangerUI()
        {
            ModuleMan.ToggleGrangerUI();
        }

        internal WurmAssistant.DynaMenuItemData GetTimersMenuItem()
        {
            return ModuleMan.GetTimersMenuItem();
        }

        internal WurmAssistant.DynaMenuItemData GetSoundNotifyMenuItem()
        {
            return ModuleMan.GetSoundNotifyMenuItem();
        }
    }
}
