using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace WurmAssistant
{
    public enum SearchStatus { Busy, Searching, None}

    //modify the form to hide instead of kill, create at constr
    public class ModuleLogSearcher : Module
    {
        FormLogSearcher LogSearcherUI;

        string LogFilesPath;
        public static LogSearchManager LogSearchMan;

        LogSearchData ScheduledLogSearchData;
        public bool isSearchScheduled = false;
        public bool isRecacheScheduled = false;
        bool isInternalRecacheScheduled = false;

        int CacheUpdateCounter = 1000;
        bool SleepMode = false;

        public bool isAppClosing = false;

        public ModuleLogSearcher(string logFilesPath)
            : base("LogSearcher")
        {
            this.LogFilesPath = logFilesPath;
            InitUI();

            if (LogSearchMan == null)
            {
                
                LogSearchMan = new LogSearchManager(LogFilesPath);
                LogSearchMan.Initialize();
            }
            else
            {
                LogSearchMan.UpdateCache();
            }
        }

        void InitUI()
        {
            LogSearcherUI = new FormLogSearcher(this);
        }

        public void ToggleUI()
        {
            if (LogSearcherUI.Visible)
            {
                LogSearcherUI.Hide();
            }
            else
            {
                LogSearcherUI.Show();
                LogSearcherUI.RestoreFromMin();
            }
        }

        public override void OnAppClosing()
        {
            isAppClosing = true;
            //kill db connections and kill threads
            LogSearchMan.Clean();
        }

        public override void BeforeHandleLogs(bool engineInSleepMode)
        {
            if (!SleepMode)
            {
                CacheUpdateCounter -= WurmAssistant.ZeroRef.TimerTickRate;
                if (CacheUpdateCounter < 0) CacheUpdateCounter = 0;
            }
        }

        public override void AfterHandleLogs(bool engineInSleepMode)
        {
            //try to update cache if counter 0
            //will retry until succesfull, then reset counter
            //will check and set sleep mode AFTER performing at least one update, to ensure everything is saved!
            if (CacheUpdateCounter == 0)
            {
                if (LogSearchMan.incorrectLogsDir)
                {
                    isInternalRecacheScheduled = true;
                    Logger.WriteLine("LogSearcher: Attempting to recache log data due to previous errors");
                    CacheUpdateCounter = 10000;
                }
                else
                {
                    if (LogSearchMan.UpdateCache())
                    {
                        CacheUpdateCounter = 30000;
                        if (engineInSleepMode) SleepMode = true;
                    }
                }
            }
        }
 
        public void PerformSearch(LogSearchData logsearchdata)
        {
            // try to perform search using supplied container
            // schedule for retry if worker is busy, until successful
            if (!LogSearchMan.PerformSearch(logsearchdata))
            {
                ScheduledLogSearchData = logsearchdata;
                isSearchScheduled = true;
                LogSearcherUI.UpdateUIAboutScheduledSearch(SearchStatus.Busy);
            }
            else
            {
                isSearchScheduled = false;
                LogSearcherUI.UpdateUIAboutScheduledSearch(SearchStatus.Searching);
            }
        }

        public override void OnPollingTick(bool engineInSleepMode)
        {
            if (LogSearchMan != null)
                LogSearchMan.UpdateSearchQueueHandler();

            //100ms
            // if scheduled, try to perform every tick
            if (isSearchScheduled)
            {
                PerformSearch(ScheduledLogSearchData);
            }

            if (isRecacheScheduled)
            {
                ForceRecache();
            }

            if (isInternalRecacheScheduled)
            {
                ForceRecache(true);
            }
        }

        // this should be in general helper!
        public List<string> GetAllPlayerNames()
        {
            List<string> allPlayers = new List<string>();

            string path = GeneralHelper.GetPathToDirectoryAbove(LogFilesPath);
            path = GeneralHelper.GetPathToDirectoryAbove(path);
            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
            {
                string playername = dir.Substring(dir.LastIndexOf(@"\") + 1, dir.Length - dir.LastIndexOf(@"\") - 1);
                allPlayers.Add(playername);
            }

            return allPlayers;
        }

        // this should be in general helper!
        public string GetCurrentPlayer()
        {
            string player = GeneralHelper.GetPreviousFolderNameFromPath(LogFilesPath);
            return player;
        }

        public void ForceRecache(bool internalRecache = false)
        {
            if (LogSearchMan != null && LogSearchMan.ForceRecache(LogSearcherUI, internalRecache))
            {
                isRecacheScheduled = false;
                if (internalRecache) isInternalRecacheScheduled = false;
            }
            else
            {
                isRecacheScheduled = true;
                if (internalRecache) isInternalRecacheScheduled = true;
            }
        }
    }
}
