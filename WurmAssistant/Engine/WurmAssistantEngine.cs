using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace WurmAssistant
{
    /// <summary>
    /// Deprecated old Wurm Assistant Engine.
    /// </summary>
    class WurmAssistantEngine
    {
        // path to wurm logs directory
        string WurmLogsDir;
        WurmAssistant ParentForm;
        // stores count of all gamelogstate at last update, currently used only to recheck for new PM logs
        int lastLogCount = 0;

        string WurmPlayer = "undefined";

        // true if wurm not running
        public bool SleepMode = false;

        // all log wrappers
        public GameLogState EventLogState;
        public GameLogState CombatLogState;
        public GameLogState AllianceLogState;
        public GameLogState CA_HELPLogState;
        public GameLogState FreedomLogState;
        public GameLogState FriendsLogState;
        public GameLogState GLFreedomLogState;
        public GameLogState LocalLogState;
        public GameLogState MGMTLogState;
        public GameLogState SkillsLogState;
        public GameLogState TeamLogState;
        public GameLogState VillageLogState;

        // all modules
        public ModuleClearTimestamps moduleClearTimestamps;
        public ModuleTimingAssist moduleTimerAssist;
        public ModuleSoundNotify moduleSoundNotify;
        public ModuleHorseDump moduleHorseDump;
        public ModuleTimers moduleTimers;
        public ModuleLogSearcher moduleLogSearcher;
        public ModuleCalendar moduleCalendar;

        // used to pass new log messages from log wrappers to modules
        List<string> NewLogEntries;

        // Lists of references to all active log wrappers and modules, used for iteration
        List<GameLogState> GenericLogsList  = new List<GameLogState>();
        List<GameLogState> PMLogsList = new List<GameLogState>();
        List<GameLogState> CombinedLogsList = new List<GameLogState>();
        List<Module> ModuleList = new List<Module>();

        /// <summary>
        /// constructs a new instance of Wurm Assistant Engine
        /// </summary>
        /// <param name="wurmloglocation">path to the Wurm Online logs folder</param>
        public WurmAssistantEngine(WurmAssistant parentForm, string wurmloglocation)
        {
            this.ParentForm = parentForm;
            this.WurmLogsDir = wurmloglocation;
            WurmPlayer = GeneralHelper.GetPreviousFolderNameFromPath(wurmloglocation);
            InitializeMain(wurmloglocation);
        }

        void InitializeMain(string wurmloglocation)
        {
            Logger.WriteLine("> Initializing wrappers for Wurm log files ");
            // init all the log wrappers except PM and add them to Generic list
            EventLogState = new GameLogState(wurmloglocation, GameLogTypes.Event, ParentForm.DailyLoggingMode);
            GenericLogsList.Add(EventLogState);
            CombatLogState = new GameLogState(wurmloglocation, GameLogTypes.Combat, ParentForm.DailyLoggingMode);
            GenericLogsList.Add(CombatLogState);
            AllianceLogState = new GameLogState(wurmloglocation, GameLogTypes.Alliance, ParentForm.DailyLoggingMode);
            GenericLogsList.Add(AllianceLogState);
            CA_HELPLogState = new GameLogState(wurmloglocation, GameLogTypes.CA_HELP, ParentForm.DailyLoggingMode);
            GenericLogsList.Add(CA_HELPLogState);
            FreedomLogState = new GameLogState(wurmloglocation, GameLogTypes.Freedom, ParentForm.DailyLoggingMode);
            GenericLogsList.Add(FreedomLogState);
            FriendsLogState = new GameLogState(wurmloglocation, GameLogTypes.Friends, ParentForm.DailyLoggingMode);
            GenericLogsList.Add(FriendsLogState);
            GLFreedomLogState = new GameLogState(wurmloglocation, GameLogTypes.GLFreedom, ParentForm.DailyLoggingMode);
            GenericLogsList.Add(GLFreedomLogState);
            LocalLogState = new GameLogState(wurmloglocation, GameLogTypes.Local, ParentForm.DailyLoggingMode);
            GenericLogsList.Add(LocalLogState);
            MGMTLogState = new GameLogState(wurmloglocation, GameLogTypes.MGMT, ParentForm.DailyLoggingMode);
            GenericLogsList.Add(MGMTLogState);
            SkillsLogState = new GameLogState(wurmloglocation, GameLogTypes.Skills, ParentForm.DailyLoggingMode);
            GenericLogsList.Add(SkillsLogState);
            TeamLogState = new GameLogState(wurmloglocation, GameLogTypes.Team, ParentForm.DailyLoggingMode);
            GenericLogsList.Add(TeamLogState);
            VillageLogState = new GameLogState(wurmloglocation, GameLogTypes.Village, ParentForm.DailyLoggingMode);
            GenericLogsList.Add(VillageLogState);

            // init all PM logs and add them to PM list, then combine Generic and PM lists to Combined list
            ManagePMLogs();

            int numOfLogFilesFound = 0; 
            foreach (var log in CombinedLogsList)
            {
                if (log.LogTextFileExists) numOfLogFilesFound++;
            }
            if (numOfLogFilesFound == 0)
            {
                Logger.WriteLine("! WARNING: No log files acquired at the specified location:");
                Logger.WriteLine("! "+wurmloglocation);
                //Logger.WriteLine("! Search pattern: "+ CombinedLogsList[0].debugShowLogPattern());
                Logger.WriteLine("! Possible causes:");
                Logger.WriteLine("! 1. Wurm not launched today, files will be auto-acquired when they are created by the game");
                Logger.WriteLine("! 2. Program set to incorrect logging mode (daily/weekly)");
                Logger.WriteLine("! 3. Chosen path to log file directory is incorrect");
                Logger.WriteLine("! 4. You just found a bug, congrats! Please report");
            }
            else
            {
                Logger.WriteLine("> Tracking " + numOfLogFilesFound + " logs at specified location, all new logs will be auto-acquired");
            }

            // init all modules and add them to Module list
            Logger.WriteLine("> Initializing modules");
            moduleLogSearcher = new ModuleLogSearcher(WurmLogsDir);
            ModuleList.Add(moduleLogSearcher);
            moduleClearTimestamps = new ModuleClearTimestamps();
            ModuleList.Add(moduleClearTimestamps); //all modules that require log timestamps should be added BEFORE this one
            moduleTimerAssist = new ModuleTimingAssist(WurmPlayer);
            ModuleList.Add(moduleTimerAssist); //this should always be before new modules
            moduleSoundNotify = new ModuleSoundNotify(WurmPlayer);
            ModuleList.Add(moduleSoundNotify);
            moduleTimers = new ModuleTimers(WurmPlayer);
            ModuleList.Add(moduleTimers);
            moduleCalendar = new ModuleCalendar();
            ModuleList.Add(moduleCalendar);

            //moduleHorseDump = new ModuleHorseDump();
            //ModuleList.Add(moduleHorseDump);
        }

        void ManagePMLogs()
        {
            string[] AllLogFiles = Directory.GetFiles(WurmLogsDir);
            if (AllLogFiles.Length != lastLogCount)
            {
                PMLogsList.Clear();
                foreach (string file in AllLogFiles)
                {
                    string workstring = Path.GetFileNameWithoutExtension(file);
                    if (workstring.StartsWith("PM", StringComparison.Ordinal))
                    {
                        //note: because this was added later on top of design, it does same file name parsing as gamelogstate
                        //      any changes here should be reflected there and vice versa
                        if (workstring.EndsWith(DateTime.Now.ToString("yyyy-MM-dd")))
                        {
                            string playername = workstring.Remove(0,4);
                            playername = playername.Remove(playername.IndexOf('.'));
                            GameLogState newlog = new GameLogState(WurmLogsDir, GameLogTypes.PM, ParentForm.DailyLoggingMode, playername);
                            PMLogsList.Add(newlog);
                        }
                        else if (workstring.EndsWith(DateTime.Now.ToString("yyyy-MM")))
                        {
                            string playername = workstring.Remove(0, 4);
                            playername = playername.Remove(playername.IndexOf('.'));
                            GameLogState newlog = new GameLogState(WurmLogsDir, GameLogTypes.PM, ParentForm.DailyLoggingMode, playername);
                            PMLogsList.Add(newlog);
                        }
                    }
                }
                CombinedLogsList.Clear();
                CombinedLogsList.AddRange(GenericLogsList);
                CombinedLogsList.AddRange(PMLogsList);
                lastLogCount = AllLogFiles.Length;
                UpdateIfDisplayLogEntries();
            }
        }

        /// <summary>
        /// Retrieves new log messages from log wrappers and forwards to all modules
        /// </summary>
        public void Update()
        {
            if (!SleepMode)
            {
                ManagePMLogs();
            }

            foreach (Module module in ModuleList)
            {
                module.BeforeHandleLogs(SleepMode);
            }

            if (!SleepMode)
            {
                foreach (GameLogState log in CombinedLogsList)
                {
                    NewLogEntries = log.UpdateAndGetNewEvents();

                    if (NewLogEntries != null)
                    {
                        foreach (Module module in ModuleList)
                        {
                            module.HandleNewLogEvents(NewLogEntries, log);
                        }
                    }
                }
            }

            foreach (Module module in ModuleList)
            {
                module.AfterHandleLogs(SleepMode);
            }
        }

        void UpdateIfDisplayLogEntries()
        {
            foreach (var log in CombinedLogsList)
            {
                log.displayEvents = ParentForm.DisplayAllLogEvents;
            }
        }

        public void TryToSaveModuleSettings()
        {
            foreach (Module module in ModuleList)
            {
                module.TryToSaveSettings();
            }
        }

        public void AppSettingsChanged()
        {
            UpdateIfDisplayLogEntries();
        }

        internal void OnAppClosing()
        {
            foreach (Module module in ModuleList)
            {
                module.ForceSaveSettings();
                module.OnAppClosing();
            }
        }

        public void UpdateOnPollingLoop()
        {
            foreach (Module module in ModuleList)
            {
                module.OnPollingTick(SleepMode);
            }
        }

        internal void OnEngineWakeUp()
        {
            moduleCalendar.OnEngineWakeUp();
        }
    }
}
