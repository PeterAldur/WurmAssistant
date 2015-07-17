using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace WurmAssistant
{
    public enum SearchTypes { RegexEscapedCaseIns, RegexCustom }

    public class SearchTypesEX
    {
        static Dictionary<string, SearchTypes> NameToEnumMap = new Dictionary<string, SearchTypes>();
        static Dictionary<SearchTypes, string> EnumToNameMap = new Dictionary<SearchTypes, string>();

        static SearchTypesEX()
        {
			{
                NameToEnumMap.Add("Match (default, case insensitive)", SearchTypes.RegexEscapedCaseIns);
                EnumToNameMap.Add(SearchTypes.RegexEscapedCaseIns, "Match (default, case insensitive)");

                NameToEnumMap.Add("Custom regular expression", SearchTypes.RegexCustom);
                EnumToNameMap.Add(SearchTypes.RegexCustom, "Custom regular expression");
			}
        }

        public static bool doesTypeExist(string par)
        {
            return NameToEnumMap.ContainsKey(par);
        }

        public static string GetNameForSearchType(SearchTypes type)
        {
            return EnumToNameMap[type]; 
        }

        public static SearchTypes GetSearchTypeForName(string name)
        {
            return NameToEnumMap[name];
        }

        public static string[] GetAllNames()
        {
            return NameToEnumMap.Keys.ToArray();
        }

        public static SearchTypes[] GetAllSearchTypes()
        {
            return EnumToNameMap.Keys.ToArray();
        }
    }

    /// <summary>
    /// Threading sucks
    /// </summary>
    public class LogSearchManager
    {
        SQLiteDB SearchDB;
        //string WurmLogsPath; //unused??
        Dictionary<string, LogFileSearcherV2> SearchersDict = new Dictionary<string, LogFileSearcherV2>();
        bool Aborting = false;
        public volatile bool incorrectLogsDir = false;

        public Thread ThreadWorker;
        static LogSearchData ResultBuffer;

        Queue<string> LoggerMessageQueue = new Queue<string>();

        public LogSearchManager(string wurmlogspath)
        {
            //this.WurmLogsPath = wurmlogspath;
        }

        //public void UpdateWurmLogsPath(string path)
        //{
        //    if (WurmLogsPath != path)
        //    {
        //        WurmLogsPath = path;
        //    }
        //}

        public void Initialize()
        {
            ThreadWorker = new Thread(TrdStart_Initialize);
            ThreadWorker.Priority = ThreadPriority.BelowNormal;
            ThreadWorker.Start();
        }

        public void TrdStart_Initialize()
        {
            try
            {
                SearchDB = new SQLiteDB(WurmAssistant.PersistentDataDir + "LogSearcher.s3db");
                //first try to get log dir from system registry
                string path = Path.Combine(WurmPaths.WurmDir, @"players\");
                //if (path == null)
                //{
                //    //if that fails, try to obtain it from player chosen log file dir
                //    //going from \wurm\players\name\logs to \wurm\players
                //    path = GeneralHelper.GetPathToDirectoryAbove(WurmLogsPath);
                //    path = GeneralHelper.GetPathToDirectoryAbove(path);
                //}
                //else
                //{
                //    path += @"\players";
                //}

                if (path != null)
                {
                    string[] dirs;
                    try
                    {
                        dirs = Directory.GetDirectories(path);
                    }
                    catch
                    {
                        dirs = new string[0];
                    }
                    TSafeLogger.WriteLine("LogSearcher: Preparing cache, this may take a while...");
                    foreach (string dir in dirs)
                    {
                        string dirpath = dir;
                        //Logger.WriteLine("Found dir: " + dirpath);
                        string playername = dirpath.Substring(dirpath.LastIndexOf(@"\") + 1, dirpath.Length - dirpath.LastIndexOf(@"\") - 1);
                        dirpath += @"\logs";
                        //Logger.WriteLine("About to save dir: " + dirpath + " (player name: ["+playername+"])");
                        SearchersDict.Add(playername, new LogFileSearcherV2(dirpath, SearchDB));
                    }
                    foreach (var dict in SearchersDict)
                    {
                        if (dict.Value.incorrectLogsDir)
                        {
                            this.incorrectLogsDir = true;
                        }
                        else
                            this.incorrectLogsDir = false;
                    }
                    if (!this.incorrectLogsDir)
                    {
                        if (SearchersDict.Count == 0)
                        {
                            this.incorrectLogsDir = true;

                        }
                        else this.incorrectLogsDir = false;
                    }
                    if (this.incorrectLogsDir)
                    {
                        DisplayLogErrorOnNoCachedLogs();
                    }
                    TSafeLogger.WriteLine("LogSearcher: Caching finished");
                    if (isForceRecaching)
                    {
                        if (ForceRecachingOwner != null)
                        {
                            ForceRecachingOwner.BeginInvoke(new FormLogSearcher.OnRecacheCompleteCallback(ForceRecachingOwner.InvokeOnRecacheComplete));
                            isForceRecaching = false;
                            ForceRecachingOwner = null;
                        }
                    }
                }
                else
                {
                    //if all fails, throw an error into logger
                    TSafeLogger.WriteLine("!! LogSearcher error: could not establish a valid path to Wurm logs directories");
                }
            }
            catch (Exception _e)
            {
                TSafeLogger.WriteLine("!!! LogSearcher: Unexpected exception at TrdStart_Initialize");
                TSafeLogger.DisplayExceptionData(_e);
                TSafeLogger.CriticalException(_e);
            }
        }

        void DisplayLogErrorOnNoCachedLogs()
        {
            TSafeLogger.WriteLine("!! LogSearcher: No logs cached for at least one dir");
        }

        public bool UpdateCache()
        {
            if (ThreadWorker.ThreadState == ThreadState.Stopped)
            {
                ThreadWorker = new Thread(TrdStart_Update);
                ThreadWorker.Priority = ThreadPriority.BelowNormal;
                ThreadWorker.Start();
                return true;
            }
            else return false;
        }

        public void TrdStart_Update()
        {
            try
            {
                foreach (var keyvalue in SearchersDict)
                {
                    keyvalue.Value.UpdateCache();
                }
            }
            catch (Exception _e)
            {
                TSafeLogger.WriteLine("!!! LogSearcher: Unexpected exception at TrdStart_Update");
                TSafeLogger.DisplayExceptionData(_e);
                TSafeLogger.CriticalException(_e);
            }
        }

        public void AddSearcher(string logfilepath, string player)
        {
            SearchersDict.Add(player, new LogFileSearcherV2(logfilepath, SearchDB));
        }

        public void Clean()
        {
            this.Aborting = true;
            if (SearchersDict != null)
            {
                foreach (var keyvalue in SearchersDict)
                {
                    keyvalue.Value.Abort();
                }
            }
            if (ThreadWorker != null) ThreadWorker.Abort();
        }

        public bool PerformSearch(LogSearchData logSearchData)
        {
            // check if thread is available
            if (ThreadWorker.ThreadState == ThreadState.Stopped)
            {
                // start the search in new thread, supplying the container
                ThreadWorker = new Thread(TrdStart_PerformSearch);
                ThreadWorker.Priority = ThreadPriority.BelowNormal;
                ThreadWorker.Start(logSearchData);
                return true;
            }
            else return false;
        }

        private void TrdStart_PerformSearch(object par_logSearchData)
        {
            try
            {
                // unbox the container
                LogSearchData logSearchData = (LogSearchData)par_logSearchData;

                // get the correct log searcher based on player
                LogFileSearcherV2 logsearcher;
                if (SearchersDict.TryGetValue(logSearchData.SearchCriteria.Player, out logsearcher))
                {
                    //get the searcher to provide a string list of all required entries
                    //send the container to get results, then retrieve the container
                    logSearchData = logsearcher.GetFilteredSearchList(
                        logSearchData.SearchCriteria.GameLogType,
                        logSearchData.SearchCriteria.TimeFrom,
                        logSearchData.SearchCriteria.TimeTo,
                        logSearchData);
                }
                //callback and return search results in a thread safe way
                if (logSearchData.CallerControl != null)
                {
                    if (logSearchData.CallerControl.GetType() == typeof(FormLogSearcher))
                    {
                        FormLogSearcher ui = (FormLogSearcher)logSearchData.CallerControl;
                        logSearchData.AllLinesArray = logSearchData.AllLines.ToArray();
                        try
                        {
                            ui.BeginInvoke(new FormLogSearcher.DisplaySearchResultsCallback(ui.DisplaySearchResults), new object[] { logSearchData });
                        }
                        catch (Exception _e)
                        {
                            TSafeLogger.WriteLine("!!! LogSearcher: error while trying to invoke FormLogSearcher, TrdStart_PerformSearch");
                            TSafeLogger.DisplayExceptionData(_e);
                        }
                    }
                    else if (logSearchData.CallerControl.GetType() == typeof(FormTimers))
                    {
                        FormTimers ui = (FormTimers)logSearchData.CallerControl;
                        try
                        {
                            ui.BeginInvoke(new FormLogSearcher.DisplaySearchResultsCallback(ui.HandleSearchCallback), new object[] { logSearchData });
                        }
                        catch (Exception _e)
                        {
                            TSafeLogger.WriteLine("!!! LogSearcher: error while trying to invoke FormTimers, TrdStart_PerformSearch");
                            TSafeLogger.DisplayExceptionData(_e);
                        }
                    }
                    else if (logSearchData.CallerControl.GetType() == typeof(Granger.FormHorseManager))
                    {
                        Granger.FormHorseManager ui = (Granger.FormHorseManager)logSearchData.CallerControl;
                        try
                        {
                            ui.BeginInvoke(new FormLogSearcher.DisplaySearchResultsCallback(ui.HandleSearchCallback), new object[] { logSearchData });
                        }
                        catch (Exception _e)
                        {
                            TSafeLogger.WriteLine("!!! LogSearcher: error while trying to invoke FormHorseManager, TrdStart_PerformSearch");
                            TSafeLogger.DisplayExceptionData(_e);
                        }
                    }
                    else if (logSearchData.CallerControl.GetType() == typeof(WurmAssistant))
                    {
                        WurmAssistant ui = (WurmAssistant)logSearchData.CallerControl;
                        try
                        {
                            if (logSearchData.CallbackID == LogSearchDataIDs.TimingAssistUptimeSearch)
                            {
                                ui.BeginInvoke(new WurmAssistant.TimingAssistUptimeSearchCallback(ui.InvokeTimingAssistUptimeSearch), new object[] { logSearchData });
                            }
                            else if (logSearchData.CallbackID == LogSearchDataIDs.TimingAssistDateTimeSearch)
                            {
                                ui.BeginInvoke(new WurmAssistant.TimingAssistDateTimeSearchCallback(ui.InvokeTimingAssistDateTimeSearch), new object[] { logSearchData });
                            }
                        }
                        catch (Exception _e)
                        {
                            TSafeLogger.WriteLine("!!! LogSearcher: error while trying to invoke WurmAssistant, TrdStart_PerformSearch");
                            TSafeLogger.DisplayExceptionData(_e);
                        }
                    }
                    else
                    {
                        //add result to the buffer
                        ResultBuffer = logSearchData;
                    }
                }
                else
                {
                    ResultBuffer = logSearchData;
                }
            }
            catch (Exception _e)
            {
                TSafeLogger.WriteLine("!!! LogSearcher: Unexpected exception at TrdStart_PerformSearch");
                TSafeLogger.DisplayExceptionData(_e);
                TSafeLogger.CriticalException(_e);
            }
        }

        public LogSearchData GetResultFromBuffer()
        {
            if (ResultBuffer != null)
            {
                LogSearchData result = ResultBuffer;
                ResultBuffer = null;
                return result;
            }
            else return null;
        }

        public static bool TryParseDateTimeFromSearchResultLine(string line, out DateTime datetime)
        {
            try
            {
                datetime = new DateTime(
                    Convert.ToInt32(line.Substring(1, 4)),
                    Convert.ToInt32(line.Substring(6, 2)),
                    Convert.ToInt32(line.Substring(9, 2)),
                    Convert.ToInt32(line.Substring(14, 2)),
                    Convert.ToInt32(line.Substring(17, 2)),
                    Convert.ToInt32(line.Substring(20, 2))
                    );
                return true;
            }
            catch
            {
                datetime = new DateTime(0);
                return false;
            }
        }

        void ClearResultBuffer()
        {
            ResultBuffer = null;
        }

        bool isForceRecaching = false;
        FormLogSearcher ForceRecachingOwner = null;

        /// <summary>
        /// This will rebuild entire log cache
        /// </summary>
        /// <param name="control">calling Control or any of it's inheritors</param>
        /// <param name="internalCall">will not do any BeginInvoke callbacks</param>
        /// <returns></returns>
        public bool ForceRecache(FormLogSearcher control, bool internalCall)
        {
            if (ThreadWorker != null && ThreadWorker.ThreadState == ThreadState.Stopped)
            {
                if (!internalCall) isForceRecaching = true;
                if (!internalCall) ForceRecachingOwner = control;
                SearchDB.ClearDB();
                SearchersDict.Clear();
                Initialize();
                return true;
            }
            else return false;
        }

        Queue<LogSearchData> SearchQueue = new Queue<LogSearchData>();

        public void EnqueueSearch(LogSearchData logsearchdata)
        {
            SearchQueue.Enqueue(logsearchdata);
        }

        public void UpdateSearchQueueHandler()
        {
            if (SearchQueue.Count > 0)
            {
                if (PerformSearch(SearchQueue.Peek()))
                {
                    SearchQueue.Dequeue();
                }
            }
        }
    }
}
