using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WurmAssistant
{
    class LogsManager
    {
        GameLogState EventLogState;
        GameLogState CombatLogState;
        GameLogState AllianceLogState;
        GameLogState CA_HELPLogState;
        GameLogState FreedomLogState;
        GameLogState FriendsLogState;
        GameLogState GLFreedomLogState;
        GameLogState LocalLogState;
        GameLogState MGMTLogState;
        GameLogState SkillsLogState;
        GameLogState TeamLogState;
        GameLogState VillageLogState;

        string PlayerName;
        string WurmLogsDir;

        List<GameLogState> GenericLogsList = new List<GameLogState>();
        List<GameLogState> PMLogsList = new List<GameLogState>();
        List<GameLogState> CombinedLogsList = new List<GameLogState>();

        int lastLogCount = 0;

        public LogsManager(string playerName)
        {
            this.PlayerName = playerName;
            this.WurmLogsDir = WurmPaths.GetLogsDirForPlayer(playerName);
            if (WurmLogsDir == null) throw new Exception("!!! Failed to start LogsManager for: " + PlayerName);
            else Initialize();
        }

        void Initialize()
        {
            Logger.WriteLine("> Initializing wrappers for Wurm log files ");
            // init all the log wrappers except PM and add them to Generic list
            EventLogState = new GameLogState(WurmLogsDir, GameLogTypes.Event, WurmAssistant.ZeroRef.DailyLoggingMode);
            GenericLogsList.Add(EventLogState);
            CombatLogState = new GameLogState(WurmLogsDir, GameLogTypes.Combat, WurmAssistant.ZeroRef.DailyLoggingMode);
            GenericLogsList.Add(CombatLogState);
            AllianceLogState = new GameLogState(WurmLogsDir, GameLogTypes.Alliance, WurmAssistant.ZeroRef.DailyLoggingMode);
            GenericLogsList.Add(AllianceLogState);
            CA_HELPLogState = new GameLogState(WurmLogsDir, GameLogTypes.CA_HELP, WurmAssistant.ZeroRef.DailyLoggingMode);
            GenericLogsList.Add(CA_HELPLogState);
            FreedomLogState = new GameLogState(WurmLogsDir, GameLogTypes.Freedom, WurmAssistant.ZeroRef.DailyLoggingMode);
            GenericLogsList.Add(FreedomLogState);
            FriendsLogState = new GameLogState(WurmLogsDir, GameLogTypes.Friends, WurmAssistant.ZeroRef.DailyLoggingMode);
            GenericLogsList.Add(FriendsLogState);
            GLFreedomLogState = new GameLogState(WurmLogsDir, GameLogTypes.GLFreedom, WurmAssistant.ZeroRef.DailyLoggingMode);
            GenericLogsList.Add(GLFreedomLogState);
            LocalLogState = new GameLogState(WurmLogsDir, GameLogTypes.Local, WurmAssistant.ZeroRef.DailyLoggingMode);
            GenericLogsList.Add(LocalLogState);
            MGMTLogState = new GameLogState(WurmLogsDir, GameLogTypes.MGMT, WurmAssistant.ZeroRef.DailyLoggingMode);
            GenericLogsList.Add(MGMTLogState);
            SkillsLogState = new GameLogState(WurmLogsDir, GameLogTypes.Skills, WurmAssistant.ZeroRef.DailyLoggingMode);
            GenericLogsList.Add(SkillsLogState);
            TeamLogState = new GameLogState(WurmLogsDir, GameLogTypes.Team, WurmAssistant.ZeroRef.DailyLoggingMode);
            GenericLogsList.Add(TeamLogState);
            VillageLogState = new GameLogState(WurmLogsDir, GameLogTypes.Village, WurmAssistant.ZeroRef.DailyLoggingMode);
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
                Logger.WriteLine("! LogsManager: No log files acquired for " + PlayerName +", path: " + WurmLogsDir);
                Logger.WriteLine("! IF you have run Wurm Client today for the above character, please make sure the program \"logging mode\" is set correctly.");
                Logger.WriteLine("! You can find this setting in tracking settings window (Engine menu), top left corner. It needs to be set to same mode as the game itself.");
                Logger.WriteLine("! If the above is set correctly, you may have found a bug, please report this. Some of the program features will not work for you until this is resolved.");
                //Logger.WriteLine("! Search pattern: "+ CombinedLogsList[0].debugShowLogPattern());
            }
            else
            {
                Logger.WriteLine("> Tracking " + numOfLogFilesFound + " logs for " + PlayerName);
            }
        }

        public void ManagePMLogs()
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
                        //filename parsing also in gamelogstate
                        if (workstring.EndsWith(DateTime.Now.ToString("yyyy-MM-dd")))
                        {
                            string playername = workstring.Remove(0, 4);
                            playername = playername.Remove(playername.IndexOf('.'));
                            GameLogState newlog = new GameLogState(WurmLogsDir, GameLogTypes.PM, WurmAssistant.ZeroRef.DailyLoggingMode, playername);
                            PMLogsList.Add(newlog);
                        }
                        else if (workstring.EndsWith(DateTime.Now.ToString("yyyy-MM")))
                        {
                            string playername = workstring.Remove(0, 4);
                            playername = playername.Remove(playername.IndexOf('.'));
                            GameLogState newlog = new GameLogState(WurmLogsDir, GameLogTypes.PM, WurmAssistant.ZeroRef.DailyLoggingMode, playername);
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

        public void UpdateIfDisplayLogEntries()
        {
            foreach (var log in CombinedLogsList)
            {
                log.displayEvents = WurmAssistant.ZeroRef.DisplayAllLogEvents;
            }
        }

        public NewLogEntries UpdateAndGetNewEvents()
        {
            NewLogEntries result = new NewLogEntries();
            foreach (GameLogState log in CombinedLogsList)
            {
                List<string> newentries = log.UpdateAndGetNewEvents();

                if (newentries != null)
                {
                    NewLogEntriesContainer logentries = new NewLogEntriesContainer();
                    logentries.Entries = newentries;
                    logentries.Log = log;
                    result.AllEntries.Add(logentries);
                }
            }
            return result;
        }
    }

    class NewLogEntries
    {
        public List<NewLogEntriesContainer> AllEntries = new List<NewLogEntriesContainer>();
    }

    struct NewLogEntriesContainer
    {
        public GameLogState Log;
        public List<string> Entries;
    }
}
