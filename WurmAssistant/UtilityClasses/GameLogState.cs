using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WurmAssistant
{
    /// <summary>
    /// Enums for various log types in Wurm. Note: all PM logs have the same type
    /// </summary>
    public enum GameLogTypes
    {
        Combat, Event, Friends, Local, Skills, Alliance,
        CA_HELP, Freedom, GLFreedom, MGMT, PM, Team, Village,
        Unknown
    };

    /// <summary>
    /// provides mapping for string name to enum and reverse version for easy lookups
    /// </summary>
    public static class GameLogTypesEX
    {
        static Dictionary<string, GameLogTypes> NameToEnumMap = new Dictionary<string, GameLogTypes>();
        static Dictionary<GameLogTypes, string> EnumToNameMap = new Dictionary<GameLogTypes, string>();

        static GameLogTypesEX()
        {
			{
                NameToEnumMap.Add("_Combat", GameLogTypes.Combat);
                EnumToNameMap.Add(GameLogTypes.Combat, "_Combat");

                NameToEnumMap.Add("_Event", GameLogTypes.Event);
                EnumToNameMap.Add(GameLogTypes.Event, "_Event");

                NameToEnumMap.Add("_Friends", GameLogTypes.Friends);
                EnumToNameMap.Add(GameLogTypes.Friends, "_Friends");

                NameToEnumMap.Add("_Local", GameLogTypes.Local);
                EnumToNameMap.Add(GameLogTypes.Local, "_Local");

                NameToEnumMap.Add("_Skills", GameLogTypes.Skills);
                EnumToNameMap.Add(GameLogTypes.Skills, "_Skills");

                NameToEnumMap.Add("Alliance", GameLogTypes.Alliance);
                EnumToNameMap.Add(GameLogTypes.Alliance, "Alliance");

                NameToEnumMap.Add("CA_HELP", GameLogTypes.CA_HELP);
                EnumToNameMap.Add(GameLogTypes.CA_HELP, "CA_HELP");

                NameToEnumMap.Add("Freedom", GameLogTypes.Freedom);
                EnumToNameMap.Add(GameLogTypes.Freedom, "Freedom");

                NameToEnumMap.Add("GL-Freedom", GameLogTypes.GLFreedom);
                EnumToNameMap.Add(GameLogTypes.GLFreedom, "GL-Freedom");

                NameToEnumMap.Add("MGMT", GameLogTypes.MGMT);
                EnumToNameMap.Add(GameLogTypes.MGMT, "MGMT");

                NameToEnumMap.Add("PM", GameLogTypes.PM);
                EnumToNameMap.Add(GameLogTypes.PM, "PM");

                NameToEnumMap.Add("Team", GameLogTypes.Team);
                EnumToNameMap.Add(GameLogTypes.Team, "Team");

                NameToEnumMap.Add("Village", GameLogTypes.Village);
                EnumToNameMap.Add(GameLogTypes.Village, "Village");
			}
        }

        public static bool doesTypeExist(string par)
        {
            return NameToEnumMap.ContainsKey(par);
        }

        public static string GetNameForLogType(GameLogTypes type)
        {
            return EnumToNameMap[type]; 
        }

        public static GameLogTypes GetLogTypeForName(string name)
        {
            return NameToEnumMap[name];
        }

        public static string[] GetAllNames()
        {
            return NameToEnumMap.Keys.ToArray();
        }

        public static GameLogTypes[] GetAllLogTypes()
        {
            return EnumToNameMap.Keys.ToArray();
        }
    }

    /// <summary>
    /// Wrapper around single Wurm Online log file. 
    /// Auto aquires file when possible and auto reaquires proper file at midnight change.
    /// </summary>
    public class GameLogState
    {
        // path to log file
        string logAddress;
        // path to log directory
        string logDirPath;
        // type of this log
        GameLogTypes LogType;
        bool DailyLoggingMode;
        // text file wrapper associated with this log wrapper
        TextFileObject LogFile;
        // list of all new lines in log, compared to previous snapshot
        List<string> newLinesInLog = new List<string>();
        // used to avoid parsing lines already inside log file at engine initialization
        bool isInitialized = false;
        // displays all events in program log as they appear in log
        public bool displayEvents = false;
        // time when this log was aquired, used to track midnight change
        DateTime logAquiredOnDate;
        // name of the PM sender, used to build proper file path for this wrapper
        string PM_name = "";
        // holds log file name string
        string LogFileName;
        
        // true if underlying text file was accessible on last update try
        public bool LogTextFileExists
        {
            get { return LogFile.FileExists; }
        }

        string LogPatternToSearchFor;

        /// <summary>
        /// constructs new log wrapper, name required ONLY for PM logs
        /// </summary>
        /// <param name="wurmLogDirAddress">path to the Wurm Online Logs folder for chosen player account</param>
        /// <param name="logtype">type of this log, determines what files will be searched for aquiring</param>
        /// <param name="name">name of the PM player, if this is PM log wrapper</param>
        public GameLogState(string wurmLogDirAddress, GameLogTypes logtype, bool dailyLoggingMode, string name = "") 
        {
            this.LogType = logtype;
            this.DailyLoggingMode = dailyLoggingMode;
            if (this.LogType == GameLogTypes.PM)
            {
                this.PM_name = "__"+name;
            }
            this.logDirPath = wurmLogDirAddress;
            InitLogState();
        }

        void InitLogState()
        {
            isInitialized = false;
            SetPathToLogFile(this.logDirPath);
            this.LogFile = new TextFileObject(this.logAddress, true, false, false, true, true, true, true);
            this.UpdateAndGetNewEvents();
        }

        void SetPathToLogFile(string wurmLogDirAddress)
        {
            //get current date
            DateTime DateNow = DateTime.Now;
            logAquiredOnDate = DateNow;

            //note: because pm logs handling was added to wurmassistantengine, on top of original design,
            //      any changes here should be reflected there and vice versa
            string logDateFormat;
            if (DailyLoggingMode) logDateFormat = DateNow.ToString("yyyy-MM-dd");
            else logDateFormat = DateNow.ToString("yyyy-MM");

            LogPatternToSearchFor = GetLogStringForType() + PM_name + "." + logDateFormat + ".txt";
            System.Diagnostics.Debug.WriteLine(LogPatternToSearchFor);
            LogFileName = LogPatternToSearchFor;
            logAddress = wurmLogDirAddress + @"\" + LogPatternToSearchFor;
            System.Diagnostics.Debug.WriteLine(logAddress);
        }

        public string debugShowLogPattern()
        {
            return LogPatternToSearchFor;
        }

        /// <summary>
        /// Returns this log type
        /// </summary>
        /// <returns>Enum GameLogTypes</returns>
        public GameLogTypes GetLogType()
        {
            return LogType;
        }

        public string GetLogName()
        {
            return LogFileName;
        }

        string GetLogStringForType()
        {
            System.Diagnostics.Debug.WriteLine(LogType);
            return GameLogTypesEX.GetNameForLogType(LogType);
        }

        /// <summary>
        /// Updates the snapshot of log file contents, retrieves and returns list of new lines in the log since previous update
        /// </summary>
        /// <returns>null if no new lines</returns>
        public List<string> UpdateAndGetNewEvents()
        {
            handleDayChange();
            LogFile.Update();

            if (LogFile.FileExists)
            {
                string nextLogLine;
                newLinesInLog.Clear();
                bool newLogData = false;

                while ((nextLogLine = LogFile.ReadNextLineOffset(0)) != null)
                {
                    if (nextLogLine != null)
                    {
                        newLinesInLog.Add(nextLogLine);
                        newLogData = true;
                    }
                }

                if (!isInitialized)
                {
                    isInitialized = true;
                    return null;
                }

                else if (newLogData == true)
                {
                    if (displayEvents)
                    {
                        foreach (string line in newLinesInLog)
                        {
                            Logger.WriteLine(GetLogStringForType() + ": " + line);
                        }
                    }
                    return newLinesInLog;
                }
                else return null;
            }
            else return null;
        }

        void handleDayChange()
        {
            if (DateTime.Now.Day != logAquiredOnDate.Day)
            {
                InitLogState();
            }
        }
    }
}
