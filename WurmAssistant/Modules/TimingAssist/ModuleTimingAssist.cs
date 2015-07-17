using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.ComponentModel;

namespace WurmAssistant
{
    /// <summary>
    /// Internal engine module (used by other modules). 
    ///  Warning: This class is very clunky
    /// </summary>
    public class ModuleTimingAssist : Module
    {
        #region WURM UPTIME

        public static DateTime currentTime = DateTime.Now;
        public static TimeSpan timePassed = new TimeSpan(0);
        public static int millisecondsPassed = 0;
        public static DateTime ServerUpSince = new DateTime(0);
        public static ModuleTimingAssist ZeroRef;
        string WurmPlayer;
        bool isUptimeLogSearchScheduled = false;
        DateTime lastLiveLogTimeUpdate = new DateTime(0);

        void InitUptimeSettings()
        {
            ServerUpSince = ModuleSettings.Get("ServerUpSince", DateTime.Now);
        }

        public void ProcessServerUptimeInfoFromHistoryEvent(string logevent, DateTime lineDateTime)
        {
            TimeSpan timespan = GetTimeSpanServerUpSince(logevent);

            Debug.WriteLine(timespan.ToString());

            ServerUpSince = lineDateTime - timespan;
            ModuleSettings.Set("ServerUpSince", ServerUpSince);

            Debug.WriteLine("parsed history DT value: " + ServerUpSince);
        }

        void ProcessServerUptimeInfo(string logevent)
        {
            TimeSpan timespan = GetTimeSpanServerUpSince(logevent);

            Debug.WriteLine(timespan.ToString());

            ServerUpSince = DateTime.Now - timespan;
            ModuleSettings.Set("ServerUpSince", ServerUpSince);

            Debug.WriteLine("parsed DT value: " + ServerUpSince);
        }

        TimeSpan GetTimeSpanServerUpSince(string logevent)
        {
            //EX:   The server has been up 1 days, 14 hours and 43 minutes.
            Match matchdays = Regex.Match(logevent, @"\d\d* days");
            Match matchhours = Regex.Match(logevent, @"\d\d* hours");
            Match matchminutes = Regex.Match(logevent, @"\d\d* minutes");
            Match matchseconds = Regex.Match(logevent, @"\d\d* seconds");

            int days = GeneralHelper.MatchToInt32(matchdays);
            int hours = GeneralHelper.MatchToInt32(matchhours);
            int minutes = GeneralHelper.MatchToInt32(matchminutes);
            int seconds = GeneralHelper.MatchToInt32(matchseconds);

            Debug.WriteLine("[" + days + "][" + hours + "][" + minutes + "][" + seconds + "]");

            return new TimeSpan(days, hours, minutes, 0);
        }

        bool BeginSearchHistoryForUptime()
        {
            //prepare LogSearchData
            LogSearchData logsearchdata = new LogSearchData();
            logsearchdata.CallerControl = WurmAssistant.ZeroRef;
            logsearchdata.CallbackID = LogSearchDataIDs.TimingAssistUptimeSearch;
            logsearchdata.SearchCriteria = new LogSearchData.SearchData(
                WurmPlayer,
                GameLogTypes.Event,
                DateTime.Now - TimeSpan.FromDays(4),
                DateTime.Now,
                "",
                SearchTypes.RegexEscapedCaseIns);
            if (ModuleLogSearcher.LogSearchMan != null)
            {
                if (ModuleLogSearcher.LogSearchMan.PerformSearch(logsearchdata))
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public void EndSearchHistoryForUptime(LogSearchData logsearchdata)
        {
            string lastUptimeCheckLine = null;
            foreach (string line in logsearchdata.AllLines)
            {
                if (line.Contains("The server has been up"))
                {
                    lastUptimeCheckLine = line;

                }
            }
            if (lastUptimeCheckLine != null)
            {
                DateTime lineDateTime;
                if (LogSearchManager.TryParseDateTimeFromSearchResultLine(lastUptimeCheckLine, out lineDateTime))
                {
                    //prefer uptime data from live log reader
                    if (lastLiveLogTimeUpdate < lineDateTime)
                    {
                        ProcessServerUptimeInfoFromHistoryEvent(lastUptimeCheckLine, lineDateTime);
                    }
                }
            }
        }

        #endregion

        #region WURM DATE AND TIME

        public static class WurmCalendarData
        {
            static Dictionary<int, string> WurmDayNumToNameMap = new Dictionary<int, string>();
            static Dictionary<int, string> WurmMonthNumToNameMap = new Dictionary<int, string>();
            static Dictionary<string, int> WurmDayNameToNumMap = new Dictionary<string, int>();
            static Dictionary<string, int> WurmMonthNameToNumMap = new Dictionary<string, int>();

            public static readonly string[] WurmDaysNames = { "Ant", "Luck", "Wurm", "Wrath", "Tears", "Sleep", "Awakening" };
            public static readonly string[] WurmMonthsNames = { "Diamond", "Saw", "Digging", "Leaf", "Bear", "Snake", "White Shark", "Fire", "Raven", "Dancer", "Omen", "Silent" };

            static WurmCalendarData()
            {
                int counter = 0;
                foreach (string day in WurmDaysNames)
                {
                    WurmDayNumToNameMap.Add(counter + 1, WurmDaysNames[counter]);
                    WurmDayNameToNumMap.Add(WurmDaysNames[counter], counter + 1);
                    counter++;
                }

                counter = 0;
                foreach (string month in WurmMonthsNames)
                {
                    WurmMonthNumToNameMap.Add(counter + 1, WurmMonthsNames[counter]);
                    WurmMonthNameToNumMap.Add(WurmMonthsNames[counter], counter + 1);
                    counter++;
                }
            }

            public static int GetDayNumForName(string name)
            {
                int result;
                if (WurmDayNameToNumMap.TryGetValue(name, out result)) return result;
                else return -1;
            }

            public static string GetDayNameForNum(int num)
            {
                string result;
                if (WurmDayNumToNameMap.TryGetValue(num, out result)) return result;
                else return null;
            }

            public static int GetMonthNumForName(string name)
            {
                int result;
                if (WurmMonthNameToNumMap.TryGetValue(name, out result)) return result;
                else
                {
                    if (name.StartsWith("White", StringComparison.Ordinal))
                    {
                        if (WurmMonthNameToNumMap.TryGetValue("White Shark", out result))
                        {
                            return result;
                        }
                        else return -1;
                    }
                    else return -1;
                }
            }

            public static string GetMonthNameForNum(int num)
            {
                string result;
                if (WurmMonthNumToNameMap.TryGetValue(num, out result)) return result;
                else return null;
            }
        }

        public struct WurmDateTime
        {
            public DateTime AcquiredOn;
            public TimeSpan TimeOfDay;
            public int DayInYear;
            public int Year;
            public TimeSpan TotalTimeOfWurm;

            public void AdjustTimeDifference(DateTime currentTime)
            {
                TimeSpan tsdiff = currentTime - AcquiredOn;
                TimeSpan tsWurmTime = new TimeSpan(DayInYear + Year * 336, TimeOfDay.Hours, TimeOfDay.Minutes, TimeOfDay.Seconds);
                tsWurmTime += tsdiff;
                TotalTimeOfWurm = tsWurmTime;
                Year = tsWurmTime.Days / 336;
                DayInYear = tsWurmTime.Days % 336;
                TimeOfDay = new TimeSpan(tsWurmTime.Hours, tsWurmTime.Minutes, tsWurmTime.Seconds);
                AcquiredOn = currentTime;
            }
        }

        public static class CurrentWurmDateTime
        {
            static bool DateParseError = false;

            //this data is updated directly from logs/events and kept intact in between to preserve accuracy
            static int _Year;
            static int _DayInYear;
            static TimeSpan _TimeOfDay;
            static DateTime _UpdatedOnRealDate;

            //this is the actual regularly updated output, every time using above base values to preserve accuracy
            public static int Year;
            public static int DayInYear;
            public static TimeSpan TimeOfDay;
            public static TimeSpan TimeAndDayOfYear;
            const double OffsetWurmSecondsPerRealHour = 0D;

            public static WurmDateTime ParseLogLineToWDT(string logline)
            {
                WurmDateTime wdt = new WurmDateTime();
                //time
                Match wurmTime = Regex.Match(logline, @" \d\d:\d\d:\d\d ");
                wdt.TimeOfDay = new TimeSpan(
                    Convert.ToInt32(wurmTime.Value.Substring(1, 2)),
                    Convert.ToInt32(wurmTime.Value.Substring(4, 2)),
                    Convert.ToInt32(wurmTime.Value.Substring(7, 2)));
                //day
                foreach (string name in WurmCalendarData.WurmDaysNames)
                {
                    if (Regex.IsMatch(logline, name))
                    {
                        wdt.DayInYear = WurmCalendarData.GetDayNumForName(name);
                        break;
                    }
                }
                //week
                Match wurmWeek = Regex.Match(logline, @"week \d");
                wdt.DayInYear = wdt.DayInYear + (Convert.ToInt32(wurmWeek.Value.Substring(5)) - 1) * 7;
                //month(starfall)
                foreach (string name in WurmCalendarData.WurmMonthsNames)
                {
                    if (Regex.IsMatch(logline, name))
                    {
                        wdt.DayInYear = wdt.DayInYear + (WurmCalendarData.GetMonthNumForName(name) - 1) * 28;
                        break;
                    }
                }
                //year
                Match wurmYear = Regex.Match(logline, @"in the year of \d+");
                wdt.Year = Convert.ToInt32(wurmYear.Value.Substring(15));

                return wdt;
            }

            //[01:09:06] It is 02:53:19 on day of Sleep in week 4 of the starfall of the Saw in the year of 1028.
            static public void SetCurrentWurmDateTime(string logline, DateTime updateRealDateTime)
            {
                try
                {
                    WurmDateTime wdt = ParseLogLineToWDT(logline);
                    _TimeOfDay = wdt.TimeOfDay;
                    _DayInYear = wdt.DayInYear;
                    _Year = wdt.Year;

                    _UpdatedOnRealDate = updateRealDateTime;
                }
                catch (Exception _e)
                {
                    DateParseError = true;
                    Logger.WriteLine("!! TimingAssist: ERROR while parsing Wurm DateTime!");
                    Logger.LogException(_e);
                }
            }

            static void UpdateWurmDate(TimeSpan updateValue)
            {
                DayInYear = _DayInYear;
                TimeOfDay = _TimeOfDay;
                Year = _Year;

                int offsetSeconds = (int)(OffsetWurmSecondsPerRealHour * (DateTime.Now - _UpdatedOnRealDate).TotalHours);

                DayInYear = DayInYear + updateValue.Days;
                TimeOfDay += new TimeSpan(updateValue.Hours, updateValue.Minutes, updateValue.Seconds);
                TimeOfDay += TimeSpan.FromSeconds(offsetSeconds);
                if (TimeOfDay.Days > 0)
                {
                    DayInYear += TimeOfDay.Days;
                    TimeOfDay = new TimeSpan(TimeOfDay.Hours, TimeOfDay.Minutes, TimeOfDay.Seconds);
                }
                if (DayInYear > 336)
                {
                    int AddYears = DayInYear / 336;
                    Year += AddYears;
                    DayInYear -= 336 * AddYears;
                }

                TimeAndDayOfYear = new TimeSpan(DayInYear, TimeOfDay.Hours, TimeOfDay.Minutes, TimeOfDay.Seconds);
            }

            public static void UpdateWurmDateOnTimerLoop()
            {
                DateTime timeNow = DateTime.Now;
                TimeSpan wurmTimeSinceLastUpdate = timeNow - CurrentWurmDateTime._UpdatedOnRealDate;
                wurmTimeSinceLastUpdate = new TimeSpan(wurmTimeSinceLastUpdate.Ticks * RealTimeToWurmTimeFactor);
                CurrentWurmDateTime.UpdateWurmDate(wurmTimeSinceLastUpdate);
            }

            public static void SetWurmDateTimeFromSettings(int year, int dayinyear, long timespantotalseconds, DateTime lastupdated)
            {
                _Year = year;
                _DayInYear = dayinyear;
                _TimeOfDay = TimeSpan.FromSeconds(timespantotalseconds);
                _UpdatedOnRealDate = lastupdated;
                UpdateWurmDateOnTimerLoop();
            }

            public static void GetDataForSavingWurmDateTime(out int year, out int dayinyear, out int timespantotalseconds, out DateTime lastupdated)
            {
                year = _Year;
                dayinyear = _DayInYear;
                timespantotalseconds = (int)_TimeOfDay.TotalSeconds;
                lastupdated = _UpdatedOnRealDate;
            }

            public static void SetDataFromCombinedTimeSpan(TimeSpan combinedTimeSpan, DateTime acquiredDT)
            {
                _Year = combinedTimeSpan.Days / 336;
                _DayInYear = combinedTimeSpan.Days % 336;
                _TimeOfDay = new TimeSpan(combinedTimeSpan.Hours, combinedTimeSpan.Minutes, combinedTimeSpan.Seconds);
                _UpdatedOnRealDate = acquiredDT;
                UpdateWurmDateOnTimerLoop();
            }
        }

        const int RealTimeToWurmTimeFactor = 8;
        bool isDateTimeLogSearchScheduled = false;
        const int UpdateWurmDateTimeDefaultIntervalInSeconds = 1000;
        int UpdateWurmDateTimeCounter = UpdateWurmDateTimeDefaultIntervalInSeconds;

        void InitDateTimeSettings()
        {
            CurrentWurmDateTime.SetWurmDateTimeFromSettings(
                ModuleSettings.Get("WurmDateYear_" + WurmPlayer, 0),
                ModuleSettings.Get("WurmDateDayInYear_" + WurmPlayer, 0),
                ModuleSettings.Get("WurmDateTimeOfDay_" + WurmPlayer, 0),
                ModuleSettings.Get("WurmDateTimeLastUpdated_" + WurmPlayer, new DateTime(0)));
        }

        void SaveDateTimeToSettings()
        {
            int year, dayInYear, timeOfDayTotalSeconds;
            DateTime lastUpdated;
            CurrentWurmDateTime.GetDataForSavingWurmDateTime(
                out year, 
                out dayInYear, 
                out timeOfDayTotalSeconds,
                out lastUpdated);
            ModuleSettings.Set("WurmDateYear_" + WurmPlayer, year);
            ModuleSettings.Set("WurmDateDayInYear_" + WurmPlayer, dayInYear);
            ModuleSettings.Set("WurmDateTimeOfDay_" + WurmPlayer, timeOfDayTotalSeconds);
            ModuleSettings.Set("WurmDateTimeLastUpdated_" + WurmPlayer, lastUpdated);
        }

        bool debug_BeginSearchHistoryForDateTime_shown = false;

        bool BeginSearchHistoryForDateTime()
        {
            if (!debug_BeginSearchHistoryForDateTime_shown)
            {
                debug_BeginSearchHistoryForDateTime_shown = true;
                Logger.WriteLine("Debug: begin history search for wurm date");
            }
            //prepare LogSearchData
            LogSearchData logsearchdata = new LogSearchData();
            logsearchdata.CallerControl = WurmAssistant.ZeroRef;
            logsearchdata.CallbackID = LogSearchDataIDs.TimingAssistDateTimeSearch;
            logsearchdata.SearchCriteria = new LogSearchData.SearchData(
                WurmPlayer,
                GameLogTypes.Event,
                DateTime.Now - TimeSpan.FromDays(7),
                DateTime.Now,
                "",
                SearchTypes.RegexEscapedCaseIns);
            if (ModuleLogSearcher.LogSearchMan != null)
            {
                if (ModuleLogSearcher.LogSearchMan.PerformSearch(logsearchdata))
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public void EndSearchHistoryForDateTime(LogSearchData logsearchdata)
        {
            Logger.WriteLine("Debug: end history search for wurm date");
            string lastDateLine = null;
            string lastServerNameForPlayer = null;
            foreach (string line in logsearchdata.AllLines)
            {
                if (line.Contains("of the starfall of"))
                {
                    lastDateLine = line;
                }
                //102 other players are online. You are on Pristine (653 totally in Wurm).
                if (line.Contains("other players are online"))
                {
                    Match match;
                    match = Regex.Match(line, @"\d+ other players are online. You are on (.+) \(");
                    if (match.Success)
                    {
                        lastServerNameForPlayer = match.Groups[1].Value;
                    }
                    else
                    {
                        Logger.WriteLine("! TimingAssist: Error while trying to recognize server welcome message: " + line);
                    }
                }
            }
            if (lastDateLine != null)
            {
                DateTime lineDateTime;
                if (LogSearchManager.TryParseDateTimeFromSearchResultLine(lastDateLine, out lineDateTime))
                {
                    if (lineDateTime > lastLiveLogTimeUpdate)
                    {
                        CurrentWurmDateTime.SetCurrentWurmDateTime(lastDateLine, lineDateTime);
                        SaveDateTimeToSettings();
                    }
                }
            }
            if (lastServerNameForPlayer != null)
            {
                Logger.WriteLine("Debug: scheduling http lookup for server: "+lastServerNameForPlayer);
                CurrentServerName = lastServerNameForPlayer;
                HttpExtractorScheduled = true;
            }
        }
        
        #endregion

        #region HTTP EXTRACTOR CODE

        bool HttpExtractorScheduled = false;
        string CurrentServerName;

        public class TimingAssistWorkerBasket
        {
            public class ServerHttpInformation
            {
                bool _isAccurate = false;
                public bool isAccurate
                {
                    get { return _isAccurate; }
                }
                public DateTime TimeParsed;
                public DateTime HeaderLastUpdated;
                public string Name;
                public string strUptime;
                public string strWurmTime;

                public bool AnyError = false;

                public void CheckAccurate()
                {
                    try
                    {
                        if (HeaderLastUpdated > TimeParsed - TimeSpan.FromMinutes(10) && HeaderLastUpdated < TimeParsed + TimeSpan.FromMinutes(10))
                        {
                            _isAccurate = true;
                        }
                    }
                    catch
                    {
                        TSafeLogger.WriteLine("! TimingAssist: HttpInfo error while checking accuracy");
                    }
                }
            }

            public static string[] ServerLinks = {
                        "http://jenn001.game.wurmonline.com/battles/stats.html",
                        "http://freedom001.game.wurmonline.com/battles/stats.html",
                        "http://freedom002.game.wurmonline.com/battles/stats.html",
                        "http://freedom003.game.wurmonline.com/battles/stats.html",
                        "http://freedom004.game.wurmonline.com/battles/stats.html",
                        "http://wild001.game.wurmonline.com/battles/stats.html",
                        "http://wild001.game.wurmonline.com:8080/battles/stats.html",
                        "http://freedom001.game.wurmonline.com:8080/battles/stats.html",
                        "http://freedom002.game.wurmonline.com:8080/battles/stats.html",
                        "http://freedom003.game.wurmonline.com:8080/battles/stats.html",
                        "http://freedom002.game.wurmonline.com:8081/battles/stats.html",
                        "http://freedom002.game.wurmonline.com:8082/battles/stats.html"};

            public Dictionary<string, ServerHttpInformation> ServerNameToInformationMap = new Dictionary<string, ServerHttpInformation>();
        }

        TimingAssistWorkerBasket HttpServerData;

        bool BeginHttpExtractor()
        {
            if (WurmAssistant.ZeroRef.BeginHttpExtractor(new TimingAssistWorkerBasket()))
            {
                Logger.WriteLine("Debug: begin wurm server feed lookup for uptime and wurm date");
                return true;
            }
            else
            {
                Logger.WriteLine("Debug: attempted to begin wurm server feed lookup, but worker was busy");
                return false;
            }
        }

        public void HttpExtractorWorkerToDo(object sender, DoWorkEventArgs e)
        {
            TimingAssistWorkerBasket basket = e.Argument as TimingAssistWorkerBasket;
            TSafeLogger.WriteLine("Debug: http worker start");
            try
            {
                WebClient webclient = new WebClient();

                foreach (string link in TimingAssistWorkerBasket.ServerLinks)
                {
                    TimingAssistWorkerBasket.ServerHttpInformation info = new TimingAssistWorkerBasket.ServerHttpInformation();
                    try
                    {
                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(link);
                        HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                        info.HeaderLastUpdated = res.LastModified;
                        info.TimeParsed = DateTime.Now;
                        info.CheckAccurate();

                        List<string> httpLines = new List<string>();

                        using (Stream stream = webclient.OpenRead(link))
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                bool serverNameRead = false;
                                bool uptimeRead = false;
                                bool wurmTimeRead = false;
                                string line;
                                while ((line = reader.ReadLine()) != null)
                                {
                                    if (serverNameRead)
                                    {
                                        Match match = Regex.Match(line, @">.+<");
                                        info.Name = match.Value.Substring(1, match.Value.Length - 2);
                                        serverNameRead = false;
                                    }
                                    else if (uptimeRead)
                                    {
                                        Match match = Regex.Match(line, @">.+<");
                                        info.strUptime = match.Value.Substring(1, match.Value.Length - 2);
                                        uptimeRead = false;
                                    }
                                    else if (wurmTimeRead)
                                    {
                                        Match match = Regex.Match(line, @">.+<");
                                        info.strWurmTime = match.Value.Substring(1, match.Value.Length - 2);
                                        wurmTimeRead = false;
                                    }

                                    httpLines.Add(line);
                                    if (Regex.IsMatch(line, "Server name"))
                                    {
                                        serverNameRead = true;
                                    }
                                    else if (Regex.IsMatch(line, "Uptime"))
                                    {
                                        uptimeRead = true;
                                    }
                                    else if (Regex.IsMatch(line, "Wurm Time"))
                                    {
                                        wurmTimeRead = true;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception _e)
                    {
                        info.AnyError = true;
                        TSafeLogger.WriteLine("! TimingAssist: There was an exception when accessing / parsing http data for " + link);
                        TSafeLogger.DisplayExceptionData(_e);
                    }
                    if (!info.AnyError) basket.ServerNameToInformationMap.Add(info.Name, info);
                }
            }
            catch (Exception _e)
            {
                TSafeLogger.WriteLine("! TimingAssist: Exception while initializing WebClient");
                TSafeLogger.DisplayExceptionData(_e);
            }
            TSafeLogger.WriteLine("Debug: http worker finished");

            e.Result = basket;
        }

        public void EndHttpExtractor(object arg_basket)
        {
            Logger.WriteLine("Debug: finalizing http worker");
            HttpServerData = arg_basket as TimingAssistWorkerBasket;
            if (CurrentServerName != null)
            {
                Logger.WriteLine("Debug: current server name is: "+CurrentServerName);
                if (HttpServerData.ServerNameToInformationMap.Count > 0)
                {
                    Logger.WriteLine("Debug: local time is: " + DateTime.Now);
                    Logger.WriteLine("Debug: data extracted for servers:");
                    foreach (var keyval in HttpServerData.ServerNameToInformationMap)
                    {
                        Logger.WriteLine("Debug: "+keyval.Key+" ("+keyval.Value.strWurmTime+") ["+ keyval.Value.HeaderLastUpdated+"]");
                    }

                    TimingAssistWorkerBasket.ServerHttpInformation info;
                    if (HttpServerData.ServerNameToInformationMap.TryGetValue(CurrentServerName, out info))
                    {
                        Logger.WriteLine("Debug: matched server to data");
                        if (info.isAccurate)
                        {
                            Logger.WriteLine("Debug: data is accurate");

                            TimeSpan timespan = GetTimeSpanServerUpSince(info.strUptime);
                            DateTime httpSrvUpSince = info.HeaderLastUpdated - timespan;
                            if (ServerUpSince < httpSrvUpSince - TimeSpan.FromMinutes(10) || ServerUpSince > httpSrvUpSince + TimeSpan.FromMinutes(10))
                            {
                                ServerUpSince = httpSrvUpSince;
                                Logger.WriteLine("TimingAssist: http data uptime check: updated due to incorrect value");
                            }
                            else Logger.WriteLine("TimingAssist: http data uptime check: passed");

                            WurmDateTime wdt = CurrentWurmDateTime.ParseLogLineToWDT(info.strWurmTime);
                            wdt.AcquiredOn = info.HeaderLastUpdated;
                            wdt.AdjustTimeDifference(DateTime.Now);
                            TimeSpan totalCurrentTimeOfWurm = TimeSpan.FromDays(CurrentWurmDateTime.Year * 336) + CurrentWurmDateTime.TimeAndDayOfYear;
                            if (totalCurrentTimeOfWurm < wdt.TotalTimeOfWurm - TimeSpan.FromMinutes(10) || totalCurrentTimeOfWurm > wdt.TotalTimeOfWurm + TimeSpan.FromMinutes(10))
                            {
                                CurrentWurmDateTime.SetDataFromCombinedTimeSpan(wdt.TotalTimeOfWurm, wdt.AcquiredOn);
                                Logger.WriteLine("TimingAssist: http data wurm date check: updated due to incorrect value");
                                // set it
                            }
                            else Logger.WriteLine("TimingAssist: http data wurm date check: passed");
                        }
                        else
                        {
                            Logger.WriteLine("! TimingAssist: http data was not accurate and was discarded");
                        }
                    }
                    else Logger.WriteLine("! TimingAssist: found no http data for this server: " + CurrentServerName != null ? CurrentServerName : "server name is null");
                }
                else Logger.WriteLine("! TimingAssist: no http data extracted");
            }
            else Logger.WriteLine("! TimingAssist: server name was null, this is probably a bug");
        }

        #endregion

        public ModuleTimingAssist(string wurmPlayer)
            : base("TimerAssist")
        {
            ZeroRef = this;
            this.WurmPlayer = wurmPlayer;
            InitUptimeSettings();
            InitDateTimeSettings();
            Debug.WriteLine("loaded DT value:" + ServerUpSince);
            DateTime _timenow = DateTime.Now;
            isUptimeLogSearchScheduled = true;
            isDateTimeLogSearchScheduled = true;
        }

        public override void OnPollingTick(bool engineInSleepMode)
        {
            //retry until successful
            if (HttpExtractorScheduled) 
                if (BeginHttpExtractor())
                    HttpExtractorScheduled = false;
            if (isUptimeLogSearchScheduled)
            {
                if (BeginSearchHistoryForUptime()) isUptimeLogSearchScheduled = false;
            }

            if (isDateTimeLogSearchScheduled)
            {
                if (BeginSearchHistoryForDateTime()) isDateTimeLogSearchScheduled = false;
            }
        }

        public override void BeforeHandleLogs(bool engineInSleepMode)
        {
            DateTime _timenow = DateTime.Now;
            timePassed = _timenow - currentTime;
            currentTime = _timenow;
            millisecondsPassed = (int)(timePassed.Ticks / 10000);

            UpdateWurmDateTimeCounter -= WurmAssistant.ZeroRef.TimerTickRate;
            if (UpdateWurmDateTimeCounter < 0)
            {
                CurrentWurmDateTime.UpdateWurmDateOnTimerLoop();
                UpdateWurmDateTimeCounter = UpdateWurmDateTimeDefaultIntervalInSeconds;

                //Debug.WriteLine("!!!        Wurm date time: " + CurrentWurmDateTime.TimeOfDay.ToString() +
                //    " day in year: " + CurrentWurmDateTime.DayInYear.ToString() +
                //    " year: " + CurrentWurmDateTime.Year.ToString());
            }

        }

        public override void HandleNewLogEvents(List<string> newLogEvents, GameLogState log)
        {
            if (log.GetLogType() == GameLogTypes.Event)
            {
                foreach (string logevent in newLogEvents)
                {
                    if (logevent.StartsWith("The server has been up", StringComparison.Ordinal))
                    {
                        lastLiveLogTimeUpdate = DateTime.Now;
                        ProcessServerUptimeInfo(logevent);
                    }
                    else if (logevent.StartsWith("It is", StringComparison.Ordinal))
                    {
                        if (logevent.Contains("of the starfall of the"))
                        {
                            CurrentWurmDateTime.SetCurrentWurmDateTime(logevent, DateTime.Now);
                            SaveDateTimeToSettings();
                        }
                    }
                    //Welcome back, Aldur! The Exodus server has been waiting for you.
                    else if (logevent.StartsWith("Welcome back", StringComparison.Ordinal))
                    {
                        if (Regex.IsMatch(logevent, @"The .+ server has been waiting for you"))
                        {
                            try
                            {
                                Match match = Regex.Match(logevent, @"The .+ server");
                                CurrentServerName = match.Value.Substring(4, match.Value.Length - 11);
                                Logger.WriteLine("Debug: scheduling http lookup for server: "+CurrentServerName);
                                HttpExtractorScheduled = true;
                            }
                            catch
                            {
                                Logger.WriteLine("! TimingAssist: Error while trying start http update, log line: " + logevent);
                            }
                        }
                    }
                }
            }
        }
    }
}
