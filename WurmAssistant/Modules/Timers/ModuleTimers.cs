using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Diagnostics;

namespace WurmAssistant
{
    public class ModuleTimers : Module
    {
        public class PriesthoodTimer
        {
            AC_SettingsDB ModuleSettings;
            string PlayerName;
            LogSearchManager LogSearchMan;
            ModuleTimers ParentModule;

            //helpers
            public static TimeSpan SermonPreacherCooldown = new TimeSpan(3, 0, 0);
            public static TimeSpan PrayCooldown = new TimeSpan(0, 20, 0);
            public static TimeSpan AlignmentCooldown = new TimeSpan(0, 30, 0);

            class PrayHistoryEntry : IComparable<PrayHistoryEntry>
            {
                public PrayHistoryEntryTypes EntryType;
                public DateTime EntryDateTime;
                public bool Valid = false;

                public PrayHistoryEntry(PrayHistoryEntryTypes type, DateTime date)
                {
                    this.EntryType = type;
                    this.EntryDateTime = date;
                }

                public int CompareTo(PrayHistoryEntry dtlm)
                {
                    return this.EntryDateTime.CompareTo(dtlm.EntryDateTime);
                }
            }
            enum PrayHistoryEntryTypes { Prayed, SermonMightyPleased, FaithGainBelow120, FaithGain120orMore }

            class AlignmentHistoryEntry : IComparable<AlignmentHistoryEntry>
            {
                public DateTime EntryDateTime;
                public bool AlwaysValid = false;
                public bool Valid = false;
                public string Reason;
                public bool ComesFromLiveLogs = false;

                public AlignmentHistoryEntry(DateTime date, bool alwaysValid = false, string reason = null, bool comesfromlivelogs = false)
                {
                    this.EntryDateTime = date;
                    this.AlwaysValid = alwaysValid;
                    this.Reason = reason;
                    this.ComesFromLiveLogs = comesfromlivelogs;
                }

                public int CompareTo(AlignmentHistoryEntry dtlm)
                {
                    return this.EntryDateTime.CompareTo(dtlm.EntryDateTime);
                }
            }

            public enum WurmReligions { Vynora, Magranon, Fo, Libila, None };

            public static class AlignmentVerifier
            {
                public static bool CheckConditions(string line, bool isWhiteLight, WurmReligions religion)
                {
                    //Holding a sermon (+/- 1)
                    if (line.Contains("You finish this sermon"))
                    {
                        return true;
                    }
                    //Listening to a sermon (up to +/- 4 (depends on preaching success))
                    if (line.Contains("finishes the sermon by asking you"))
                    {
                        return true;
                    }
                    //Converting someone to your religion (+/- 1)
                    //Sacrificing items in an altar (at least worth 50c get price) (+/- 1)

                    if (isWhiteLight)
                    {
                        //Burying a human corpse (+ 2)
                        if (line.Contains("You bury"))
                        {
                            if (Regex.IsMatch(line, @"You bury the corpse of \w+"))
                            {
                                if (!Regex.IsMatch(line, @"You bury the corpse of \w+ \w+"))
                                {
                                    return true;
                                }
                            }
                            if (line.Contains("tower guar")) return true;
                        }
                        //Healing someone else (+ 1)
                        if (line.Contains("You treat the wound") || line.Contains("You bandage the wound"))
                        {
                            return true;
                        }
                        //Casting Bless on players (seems random). (+ 1)
                        //Praying at the White Light on the Wild server. (+ 3)
                        //Killing of a blacklighter (+ 5) no way to tell light = NOT POSSIBLE
                    }
                    else if (!isWhiteLight)
                    {
                        //Butchering a human corpse (- 1)
                        if (line.Contains("You butcher"))
                        {
                            if (Regex.IsMatch(line, @"You butcher the corpse of \w+"))
                            {
                                if (!Regex.IsMatch(line, @"You butcher the corpse of \w+ \w+"))
                                {
                                    return true;
                                }
                            }
                            if (line.Contains("tower guar")) return true;
                        }
                        //Successful Lockpicking (-5)
                        //Praying at the Black Light on the Wild server. (- 3)
                        //Desecrating an altar (- 2)
                        //Killing of a whitelighter (- 5) no way to tell light = NOT POSSIBLE
                    }

                    if (religion == WurmReligions.Fo)
                    {
                        //Listening to a confession (+/- 1)
                        if (line.Contains("You decide that a good penance is for"))
                        {
                            return true;
                        }
                        //Confessing to a priest (+/- 5)
                        if (line.Contains("thinks for a while and asks you"))
                        {
                            return true;
                        }
                        //Fo special: plant a sprout or a flower (+ 1)
                        if (line.Contains("You plant the sprout"))
                        {
                            return true;
                        }

                        if (line.Contains("You plant the flowers"))
                        {
                            return true;
                        }
                    }
                    else if (religion == WurmReligions.Vynora)
                    {
                        //Listening to a confession (+/- 1)
                        if (line.Contains("You decide that a good penance is for"))
                        {
                            return true;
                        }
                        //Confessing to a priest (+/- 5)
                        if (line.Contains("thinks for a while and asks you"))
                        {
                            return true;
                        }
                        //Vynora special: cutting down an old, very old or overaged tree (+ 1) NOTE: doesn't care about the age
                        if (line.Contains("You cut down the"))
                        {
                            return true;
                        }
                        //Vynora special: working on walls (+ 0.5) (what walls? fences? house walls? roofs floors?)
                    }
                    else if (religion == WurmReligions.Magranon)
                    {
                        //Listening to a confession (+/- 1)
                        if (line.Contains("You decide that a good penance is for"))
                        {
                            return true;
                        }
                        //Confessing to a priest (+/- 5)
                        if (line.Contains("thinks for a while and asks you"))
                        {
                            return true;
                        }
                        //Magranon special: mine (+ 0.5)
                        if (line.Contains("You mine some"))
                        {
                            return true;
                        }
                        //Magranon special: kill a creature (+ 0.5)
                        if (line.Contains("is dead. R.I.P."))
                        {
                            if (!line.Contains("tower guard"))
                            {
                                return true;
                            }
                        }
                    }
                    else if (religion == WurmReligions.Libila)
                    {
                        //Listening to a confession (+/- 1)
                        if (line.Contains("You decide that you can probably fool"))
                        {
                            return true;
                        }
                        //Confessing to a priest (+/- 5)
                        if (line.Contains("scorns you and tells you to give"))
                        {
                            return true;
                        }
                        //Libila special: kill a creature (- 0.5)
                        if (line.Contains("is dead. R.I.P."))
                        {
                            if (!line.Contains("tower guard"))
                            {
                                return true;
                            }
                        }
                    }

                    return false;
                }
            }

            List<PrayHistoryEntry> PrayerHistory = new List<PrayHistoryEntry>();
            List<AlignmentHistoryEntry> AlignmentHistory = new List<AlignmentHistoryEntry>();

            DateTime NextPrayDate = DateTime.Now;
            DateTime DateOfNextSermon = new DateTime(0);
            DateTime DateOfNextAlignment = new DateTime(0);

            bool NextPrayDateUnknown = true;
            DateTime CooldownResetSince = DateTime.Now;
            TimeSpan TimeOnThisCooldownReset = new TimeSpan(0);
            bool isPrayCountMax = false;

            // favor
            float FavorLevel = 0F;

            // threading control
            bool ScheduleUpdateCache = false;
            bool isPrayHistoryUpdateScheduled = false;
            bool isPrayHistoryUpdated = false;
            bool isFaithSkillUpdateScheduled = false;
            bool isFaithSkillUpdated = false;
            bool isSermonLookupScheduled = false;
            bool isSermonLookupUpdated = false;
            bool isAlignmentLookupScheduled = false;
            bool isAlignmentLookupUpdated = false;
            bool isCacheUpdated = false;

            //debug note: ENABLED saving
            #region MODULE SETTINGS

            float _faithSkill = 0;
            float FaithSkill
            {
                get { return _faithSkill; }
                set
                {
                    _faithSkill = value;
                    ModuleSettings.Set("FaithSkill_" + PlayerName, value);
                    FaithSkillLastUpdateDate = DateTime.Now;
                }
            }

            DateTime _faithSkillLastUpdateDate = new DateTime(0);
            DateTime FaithSkillLastUpdateDate
            {
                get { return _faithSkillLastUpdateDate; }
                set
                {
                    _faithSkillLastUpdateDate = value;
                    ModuleSettings.Set("FaithSkillLastUpdateDate_" + PlayerName, value);
                }
            }

            //prayer notifies
            string _prayerReadySound = "none";
            public string PrayerReadySound
            {
                get { return _prayerReadySound; }
                set
                {
                    _prayerReadySound = value;
                    ModuleSettings.Set("PrayerReadySound_" + PlayerName, value);
                }
            }

            bool _trayPrayerNotify = false;
            public bool TrayPrayerNotify
            {
                get { return _trayPrayerNotify; }
                set
                {
                    _trayPrayerNotify = value;
                    ModuleSettings.Set("TrayPrayerNotify_" + PlayerName, value);
                }
            }

            //favor notifies
            string _favorReadySound = "none";
            public string FavorReadySound
            {
                get { return _favorReadySound; }
                set
                {
                    _favorReadySound = value;
                    ModuleSettings.Set("FavorReadySound_" + PlayerName, value);
                }
            }

            bool _trayFavorNotify = false;
            public bool TrayFavorNotify
            {
                get { return _trayFavorNotify; }
                set
                {
                    _trayFavorNotify = value;
                    ModuleSettings.Set("TrayFavorNotify_" + PlayerName, value);
                }
            }

            int _favorNotifyLevel = -1;
            /// <summary>
            /// values: -1 == favor max; 0-100 (and more) static value
            /// </summary>
            public int FavorNotifyLevel
            {
                get { return _favorNotifyLevel; }
                set
                {
                    _favorNotifyLevel = value;
                    ModuleSettings.Set("FavorNotifyLevel_" + PlayerName, value);
                }
            }

            //sermon notifies
            string _sermonReadySound = "none";
            public string SermonReadySound
            {
                get { return _sermonReadySound; }
                set
                {
                    _sermonReadySound = value;
                    ModuleSettings.Set("SermonReadySound_" + PlayerName, value);
                }
            }

            bool _traySermonNotify = false;
            public bool TraySermonNotify
            {
                get { return _traySermonNotify; }
                set
                {
                    _traySermonNotify = value;
                    ModuleSettings.Set("TraySermonNotify_" + PlayerName, value);
                }
            }

            //alignment notifies
            string _alignmentReadySound = "none";
            public string AlignmentReadySound
            {
                get { return _alignmentReadySound; }
                set
                {
                    _alignmentReadySound = value;
                    ModuleSettings.Set("AlignmentReadySound_" + PlayerName, value);
                }
            }

            bool _trayAlignmentNotify = false;
            public bool TrayAlignmentNotify
            {
                get { return _trayAlignmentNotify; }
                set
                {
                    _trayAlignmentNotify = value;
                    ModuleSettings.Set("TrayAlignmentNotify_" + PlayerName, value);
                }
            }

            bool _priesthoodTimerEnabled = false;
            public bool PriesthoodTimerEnabled
            {
                get { return _priesthoodTimerEnabled; }
                set
                {
                    _priesthoodTimerEnabled = value;
                    ModuleSettings.Set("PriesthoodTimerEnabled_" + PlayerName, value);
                    TryToStartModule();
                }
            }

            bool _isWhiteLighter = true;
            public bool IsWhiteLighter
            {
                get { return _isWhiteLighter; }
                set
                {
                    _isWhiteLighter = value;
                    ModuleSettings.Set("IsWhiteLighter_" + PlayerName, value);
                    TryToStartModule();
                }
            }

            WurmReligions _playerReligion = WurmReligions.None;
            public WurmReligions PlayerReligion
            {
                get { return _playerReligion; }
                set
                {
                    _playerReligion = value;
                    ModuleSettings.Set("PlayerReligion_" + PlayerName, value.ToString());
                    TryToStartModule();
                }
            }

            void InitSettings()
            {
                _priesthoodTimerEnabled = ModuleSettings.Get("PriesthoodTimerEnabled_" + PlayerName, _priesthoodTimerEnabled);

                _faithSkill = ModuleSettings.Get("FaithSkill_" + PlayerName, _faithSkill);
                _faithSkillLastUpdateDate = ModuleSettings.Get("FaithSkillLastUpdateDate_" + PlayerName, _faithSkillLastUpdateDate);

                _favorReadySound = ModuleSettings.Get("FavorReadySound_" + PlayerName, _favorReadySound);
                _trayFavorNotify = ModuleSettings.Get("TrayFavorNotify_" + PlayerName, _trayFavorNotify);
                _favorNotifyLevel = ModuleSettings.Get("FavorNotifyLevel_" + PlayerName, _favorNotifyLevel);

                _prayerReadySound = ModuleSettings.Get("PrayerReadySound_" + PlayerName, _prayerReadySound);
                _trayPrayerNotify = ModuleSettings.Get("TrayPrayerNotify_" + PlayerName, _trayPrayerNotify);

                _sermonReadySound = ModuleSettings.Get("SermonReadySound_" + PlayerName, _sermonReadySound);
                _traySermonNotify = ModuleSettings.Get("TraySermonNotify_" + PlayerName, _traySermonNotify);

                _alignmentReadySound = ModuleSettings.Get("AlignmentReadySound_" + PlayerName, _alignmentReadySound);
                _trayAlignmentNotify = ModuleSettings.Get("TrayAlignmentNotify_" + PlayerName, _trayAlignmentNotify);

                _isWhiteLighter = ModuleSettings.Get("IsWhiteLighter_" + PlayerName, _isWhiteLighter);
                string playerReligionSTR = ModuleSettings.Get("PlayerReligion_" + PlayerName, _playerReligion.ToString());
                _playerReligion = (WurmReligions)Enum.Parse(typeof(WurmReligions), playerReligionSTR);
            }

            #endregion

            public PriesthoodTimer(ModuleTimers parentModule, string playername, AC_SettingsDB modulesettings)
            {
                this.ParentModule = parentModule;
                this.PlayerName = playername;
                this.ModuleSettings = modulesettings;
                InitSettings();
                LogSearchMan = ModuleLogSearcher.LogSearchMan;

                TryToStartModule();
            }

            void TryToStartModule()
            {
                ScheduleUpdateCache = false;
                if (PriesthoodTimerEnabled)
                {
                    isPrayHistoryUpdateScheduled = false;
                    isPrayHistoryUpdated = false;
                    isFaithSkillUpdateScheduled = false;
                    isFaithSkillUpdated = false;
                    isSermonLookupScheduled = false;
                    isSermonLookupUpdated = false;
                    isAlignmentLookupScheduled = false;
                    isAlignmentLookupUpdated = false;
                    isCacheUpdated = false;
                    PrayerHistory.Clear();
                    AlignmentHistory.Clear();
                    isPrayCountMax = false;
                    ScheduleUpdateCache = true;
                }
            }

            #region PREPARE CACHE (initial, using LogSearchManager)

            internal void OnPollingTick()
            {
                if (ScheduleUpdateCache)
                {
                    if (UpdateCache())
                    {
                        ScheduleUpdateCache = false;
                    }
                }
            }

            bool UpdateCache()
            {
                if (!isFaithSkillUpdateScheduled)
                {
                    if (BeginUpdateFaithSkillCache())
                    {
                        isFaithSkillUpdateScheduled = true;
                    }
                }
                if (!isPrayHistoryUpdateScheduled)
                {
                    if (BeginUpdatePrayHistoryCache())
                    {
                        isPrayHistoryUpdateScheduled = true;
                    }
                }
                if (!isSermonLookupScheduled)
                {
                    if (BeginSermonLookup())
                    {
                        isSermonLookupScheduled = true;
                    }
                }
                if (!isAlignmentLookupScheduled)
                {
                    if (BeginAlignmentLookup())
                    {
                        isAlignmentLookupScheduled = true;
                    }
                }

                if (isFaithSkillUpdateScheduled
                    && isPrayHistoryUpdateScheduled
                    && isSermonLookupScheduled
                    && isAlignmentLookupScheduled)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            bool BeginUpdateFaithSkillCache()
            {
                if (LogSearchMan != null)
                {
                    DateTime checkFromThisDate = FaithSkillLastUpdateDate;
                    DateTime minimumUpdateDate = DateTime.Now - TimeSpan.FromDays(7);
                    if (FaithSkillLastUpdateDate > minimumUpdateDate)
                    {
                        checkFromThisDate = minimumUpdateDate;
                    }

                    LogSearchData logsearchdata = new LogSearchData();
                    logsearchdata.BuildSearchData(
                        PlayerName,
                        GameLogTypes.Skills,
                        checkFromThisDate,
                        DateTime.Now,
                        "",
                        SearchTypes.RegexEscapedCaseIns);
                    logsearchdata.CallerControl = ParentModule.TimersUI;
                    logsearchdata.CallbackID = LogSearchDataIDs.TimersFaithSkill;

                    if (LogSearchMan.PerformSearch(logsearchdata))
                    {
                        return true;
                    }
                    else return false;
                }
                else return false;
            }

            void EndUpdateFaithSkillCache(LogSearchData logsearchdata) //called back
            {
                //DebugDump.DumpToTextFile("skill.txt", logsearchdata.AllLines);
                //do smtg with results
                FaithSkillLastUpdateDate = DateTime.Now;
                float mostRecentFaithSkill = -1;
                foreach (string line in logsearchdata.AllLines)
                {
                    if (line.Contains("Faith increased"))
                    {
                        float faithskill = PriestTimer_ExtractSkillLEVELFromLine(line);
                        if (faithskill > 0)
                        {
                            mostRecentFaithSkill = faithskill;
                        }
                        float faithskillgain = PriestTimer_ExtractSkillGAINfromLine(line);
                        if (faithskillgain > 0)
                        {
                            DateTime datetime;
                            if (LogSearchManager.TryParseDateTimeFromSearchResultLine(line, out datetime))
                            {
                                if (faithskillgain >= 0.120F)
                                {
                                    PrayerHistory.Add(new PrayHistoryEntry(PrayHistoryEntryTypes.FaithGain120orMore, datetime));
                                }
                                else
                                {
                                    PrayerHistory.Add(new PrayHistoryEntry(PrayHistoryEntryTypes.FaithGainBelow120, datetime));
                                }
                            }
                        }
                    }
                    else if (line.Contains("Faith decreased"))
                    {
                        float faithskill = PriestTimer_ExtractSkillLEVELFromLine(line);
                        if (faithskill > 0)
                        {
                            mostRecentFaithSkill = faithskill;
                        }
                    }

                    if (line.Contains("Favor increased"))
                    {
                        FavorLevel = PriestTimer_ExtractSkillLEVELFromLine(line);
                    }

                    //[2013-01-13] [15:26:22] Alignment increased by 2,00 to 63,176
                    if (line.Contains("Alignment increased"))
                    {
                        DateTime datetime;
                        if (LogSearchManager.TryParseDateTimeFromSearchResultLine(line, out datetime))
                        {
                            AlignmentHistory.Add(new AlignmentHistoryEntry(datetime, true));
                        }
                    }
                }
                if (mostRecentFaithSkill > 0)
                {
                    Logger.WriteLine("Timers: Determined current faith for player " + PlayerName + " to be " + mostRecentFaithSkill);
                    this.FaithSkill = mostRecentFaithSkill;
                }
            }

            bool BeginUpdatePrayHistoryCache()
            {
                if (LogSearchMan != null)
                {
                    LogSearchData logsearchdata = new LogSearchData();
                    logsearchdata.BuildSearchData(
                        PlayerName,
                        GameLogTypes.Event,
                        DateTime.Now - TimeSpan.FromDays(2),
                        DateTime.Now,
                        "",
                        SearchTypes.RegexEscapedCaseIns);
                    logsearchdata.CallerControl = ParentModule.TimersUI;
                    logsearchdata.CallbackID = LogSearchDataIDs.TimersPrayHistory;

                    if (LogSearchMan.PerformSearch(logsearchdata))
                    {
                        return true;
                    }
                    else return false;
                }
                else return false;
            }

            void EndUpdatePrayHistoryCache(LogSearchData logsearchdata) //called back
            {
                //DebugDump.DumpToTextFile("history.txt", logsearchdata.AllLines);
                //do smtg with results
                foreach (string line in logsearchdata.AllLines)
                {
                    if (line.Contains("You finish your prayer"))
                    {
                        DateTime datetime;
                        if (LogSearchManager.TryParseDateTimeFromSearchResultLine(line, out datetime))
                        {
                            PrayerHistory.Add(new PrayHistoryEntry(PrayHistoryEntryTypes.Prayed, datetime));
                        }
                        else
                        {
                            Logger.WriteLine("! Timers: error while parsing date in EndUpdatePrayHistoryCache");
                        }
                    }
                    else if (line.Contains("is mighty pleased with you"))
                    {
                        DateTime datetime;
                        if (LogSearchManager.TryParseDateTimeFromSearchResultLine(line, out datetime))
                        {
                            PrayerHistory.Add(new PrayHistoryEntry(PrayHistoryEntryTypes.SermonMightyPleased, datetime));
                        }
                        else
                        {
                            Logger.WriteLine("! Timers: error while parsing date in EndUpdatePrayHistoryCache");
                        }
                    }
                }

                //List<string> dumphistory = new List<string>();
                //foreach (MeditHistoryEntry entry in MeditHistory)
                //{
                //    dumphistory.Add(entry.EntryDateTime.ToString() + ", " + entry.EntryType.ToString());
                //}
                //DebugDump.DumpToTextFile("parsedhistory.txt", dumphistory);
            }

            bool BeginSermonLookup()
            {
                if (LogSearchMan != null)
                {
                    TimeSpan timeSinceLastCheckup = TimeSpan.FromDays(2);

                    LogSearchData logsearchdata = new LogSearchData();
                    logsearchdata.BuildSearchData(
                        PlayerName,
                        GameLogTypes.Event,
                        DateTime.Now - timeSinceLastCheckup,
                        DateTime.Now,
                        "",
                        SearchTypes.RegexEscapedCaseIns);
                    logsearchdata.CallerControl = ParentModule.TimersUI;
                    logsearchdata.CallbackID = LogSearchDataIDs.TimersSermonLookup;

                    if (LogSearchMan.PerformSearch(logsearchdata))
                    {
                        return true;
                    }
                    else return false;
                }
                else return false;
            }

            void EndSermonLookup(LogSearchData logsearchdata)
            {
                string lastSermonLine = null;
                foreach (string line in logsearchdata.AllLines)
                {
                    if (line.Contains("You finish this sermon"))
                    {
                        lastSermonLine = line;
                    }
                }
                if (lastSermonLine != null)
                {
                    UpdateDateOfNextSermon(lastSermonLine, false);
                }
            }

            void UpdateDateOfNextSermon(string line, bool liveLogs)
            {
                DateTime dateOfThisLine;
                if (liveLogs)
                {
                    dateOfThisLine = DateTime.Now;
                    DateOfNextSermon = dateOfThisLine + SermonPreacherCooldown;
                }
                else
                {
                    if (LogSearchManager.TryParseDateTimeFromSearchResultLine(line, out dateOfThisLine))
                    {
                        DateOfNextSermon = dateOfThisLine + SermonPreacherCooldown;
                    }
                    else
                    {
                        //do nothing, whatever happened
                        Logger.WriteLine("!! Timers: Pray: Sermon lookup: parse error");
                    }
                }

                if (DateOfNextSermon > CooldownResetSince + TimeSpan.FromDays(1))
                {
                    DateOfNextSermon = CooldownResetSince + TimeSpan.FromDays(1);
                }
            }

            bool BeginAlignmentLookup()
            {
                if (LogSearchMan != null)
                {
                    TimeSpan timeSinceLastCheckup = TimeSpan.FromDays(2);

                    LogSearchData logsearchdata = new LogSearchData();
                    logsearchdata.BuildSearchData(
                        PlayerName,
                        GameLogTypes.Event,
                        DateTime.Now - timeSinceLastCheckup,
                        DateTime.Now,
                        "",
                        SearchTypes.RegexEscapedCaseIns);
                    logsearchdata.CallerControl = ParentModule.TimersUI;
                    logsearchdata.CallbackID = LogSearchDataIDs.TimersAlignmentLookup;

                    if (LogSearchMan.PerformSearch(logsearchdata))
                    {
                        return true;
                    }
                    else return false;
                }
                else return false;
            }

            void EndAlignmentLookup(LogSearchData logsearchdata)
            {
                foreach (string line in logsearchdata.AllLines)
                {
                    if (AlignmentVerifier.CheckConditions(line, IsWhiteLighter, PlayerReligion))
                    {
                        DateTime datetime;
                        if (LogSearchManager.TryParseDateTimeFromSearchResultLine(line, out datetime))
                        {
                            AlignmentHistory.Add(new AlignmentHistoryEntry(datetime, reason: line));
                        }
                        else
                        {
                            Logger.WriteLine("! Timers: error while parsing date in EndAlignmentLookup");
                        }
                    }
                }
            }

            public void CallbackHandler(LogSearchData logsearchdata)
            {
                if (logsearchdata.CallbackID == LogSearchDataIDs.TimersFaithSkill)
                {
                    EndUpdateFaithSkillCache(logsearchdata);
                    isFaithSkillUpdated = true;
                }
                else if (logsearchdata.CallbackID == LogSearchDataIDs.TimersPrayHistory)
                {
                    EndUpdatePrayHistoryCache(logsearchdata);
                    isPrayHistoryUpdated = true;
                }
                else if (logsearchdata.CallbackID == LogSearchDataIDs.TimersSermonLookup)
                {
                    EndSermonLookup(logsearchdata);
                    isSermonLookupUpdated = true;
                }
                else if (logsearchdata.CallbackID == LogSearchDataIDs.TimersAlignmentLookup)
                {
                    EndAlignmentLookup(logsearchdata);
                    isAlignmentLookupUpdated = true;
                }
                else
                {
                    Debug.WriteLine("! ModuleTimers: Priesthood: Wrong callback id: " + logsearchdata.CallbackID.ToString());
                }

                if (isFaithSkillUpdated && isPrayHistoryUpdated && isSermonLookupUpdated && isAlignmentLookupUpdated)
                {
                    isCacheUpdated = true;
                    UpdateDateOfLastCooldownReset();
                    RevalidateFaithHistory();
                    UpdateNextPrayerDate();
                    RevalidateAlignmentHistory();
                    UpdateNextAlignmentDate();
                    ParentModule.UpdateUI(UpdateTimersUITypes.Priesthood);
                }
            }

            #endregion

            #region UPDATE CACHE

            void UpdateDateOfLastCooldownReset()
            {
                DateTime currentTime = DateTime.Now;
                DateTime cooldownResetDate = currentTime;

                TimeSpan timeSinceLastServerReset = currentTime - ModuleTimingAssist.ServerUpSince;
                TimeSpan daysSinceLastServerReset = new TimeSpan(timeSinceLastServerReset.Days, 0, 0, 0);
                timeSinceLastServerReset = timeSinceLastServerReset.Subtract(daysSinceLastServerReset);

                cooldownResetDate = currentTime - timeSinceLastServerReset;
                this.CooldownResetSince = cooldownResetDate;
            }

            void RevalidateFaithHistory()
            {
                //sort the history based on entry datetimes
                PrayerHistory.Sort();

                DateTime lastValidEntry = new DateTime(0);
                PrayHistoryEntry lastMayorSkillGain = null,
                    //lastMinorSkillGain = null, //useless because very small ticks will never be logged regardless of setting
                    lastMightyPleased = null,
                    lastPrayer = null;
                TimeSpan currentPrayCooldownTimeSpan = PrayCooldown;
                int validPrayerCount = 0;
                this.isPrayCountMax = false;
                for (int i = 0; i < PrayerHistory.Count; i++)
                {
                    PrayHistoryEntry entry = PrayerHistory[i];
                    entry.Valid = false;

                    if (entry.EntryDateTime > CooldownResetSince)
                    {
                        if (entry.EntryType == PrayHistoryEntryTypes.Prayed) lastPrayer = entry;
                        //else if (entry.EntryType == PrayHistoryEntryTypes.FaithGainBelow120) lastMinorSkillGain = entry;
                        else if (entry.EntryType == PrayHistoryEntryTypes.SermonMightyPleased) lastMightyPleased = entry;
                        else if (entry.EntryType == PrayHistoryEntryTypes.FaithGain120orMore) lastMayorSkillGain = entry;

                        //on sermon event, check if recently there was big faith skill gain, if yes reset prayers
                        if (entry.EntryType == PrayHistoryEntryTypes.SermonMightyPleased)
                        {
                            if (lastMayorSkillGain != null
                                && lastMayorSkillGain.EntryDateTime > entry.EntryDateTime - TimeSpan.FromSeconds(15))
                            {
                                validPrayerCount = 0;
                                this.isPrayCountMax = false;
                            }
                        }
                        //on big faith skill gain, check if recently there was a sermon event, if yes reset prayers
                        else if (entry.EntryType == PrayHistoryEntryTypes.FaithGain120orMore)
                        {
                            if (lastMightyPleased != null
                                && lastMightyPleased.EntryDateTime > entry.EntryDateTime - TimeSpan.FromSeconds(15))
                            {
                                validPrayerCount = 0;
                                this.isPrayCountMax = false;
                            }
                        }
                        //on prayed, if prayer cap not reached, check if it's later than last valid prayer + cooldown, if yes, validate
                        else if (!this.isPrayCountMax
                            && entry.EntryType == PrayHistoryEntryTypes.Prayed
                            && entry.EntryDateTime > lastValidEntry + currentPrayCooldownTimeSpan)
                        {
                            entry.Valid = true;
                            validPrayerCount++;
                            lastValidEntry = entry.EntryDateTime;
                        }

                        //if prayer cap reached, set flag
                        if (validPrayerCount >= 5)
                        {
                            this.isPrayCountMax = true;
                        }
                    }
                }

                // old algorithm

                ////validate entries
                //foreach (PrayHistoryEntry entry in PrayerHistory)
                //{
                //    entry.Valid = false;
                //    //all entries are default invalid
                //    //discard any entry prior to cooldown reset
                //    if (entry.EntryDateTime > CooldownResetSince)
                //    {
                //        if (entry.EntryType == PrayHistoryEntryTypes.SermonMightyPleased)
                //        {
                //            //apply longer cooldown from this point
                //            validPrayerCount = 0;
                //            this.isPrayCountMax = false;
                //        }

                //        //if entry date is later, than last valid + cooldown period
                //        if (!this.isPrayCountMax
                //            && entry.EntryType == PrayHistoryEntryTypes.Prayed
                //            && entry.EntryDateTime > lastValidEntry + currentPrayCooldownTimeSpan)
                //        {
                //            entry.Valid = true;
                //            validPrayerCount++;
                //            lastValidEntry = entry.EntryDateTime;
                //        }

                //        if (validPrayerCount >= 5)
                //        {
                //            this.isPrayCountMax = true;
                //        }
                //    }
                //}

                //debug
                //List<string> dumphistory = new List<string>();
                //foreach (MeditHistoryEntry entry in MeditHistory)
                //{
                //    dumphistory.Add(entry.EntryDateTime.ToString() + ", " + entry.EntryType.ToString() + ", " + entry.Valid.ToString());
                //}
                //DebugDump.DumpToTextFile("meditvalidatedlist.txt", dumphistory);
            }

            void RevalidateAlignmentHistory()
            {
                AlignmentHistory.Sort();

                DateTime lastValidEntry = new DateTime(0);
                //validate entries
                foreach (AlignmentHistoryEntry entry in AlignmentHistory)
                {
                    entry.Valid = false;
                    //all entries are default invalid
                    //discard any entry prior to cooldown reset
                    if (entry.EntryDateTime > CooldownResetSince)
                    {
                        if (entry.AlwaysValid)
                        {
                            entry.Valid = true;
                            lastValidEntry = entry.EntryDateTime;
                        }
                        //if entry date is later, than last valid + cooldown period
                        else if (entry.EntryDateTime > lastValidEntry + AlignmentCooldown)
                        {
                            entry.Valid = true;
                            //lastValidEntry = entry.EntryDateTime;  //this will never be accurate enough to qualify for validity
                        }
                    }
                }
            }

            void UpdateNextPrayerDate()
            {
                if (isPrayCountMax)
                {
                    NextPrayDate = CooldownResetSince + TimeSpan.FromDays(1);
                }
                else
                {
                    NextPrayDate = FindLastValidPrayerInHistory() + PrayCooldown;
                }

                if (NextPrayDate > CooldownResetSince + TimeSpan.FromDays(1))
                {
                    NextPrayDate = CooldownResetSince + TimeSpan.FromDays(1);
                }
            }

            void UpdateNextAlignmentDate()
            {
                DateOfNextAlignment = FindLastValidAlignmentInHistory() + AlignmentCooldown;

                if (DateOfNextAlignment > CooldownResetSince + TimeSpan.FromDays(1))
                {
                    DateOfNextAlignment = CooldownResetSince + TimeSpan.FromDays(1);
                }
            }

            float PriestTimer_ExtractSkillLEVELFromLine(string line)
            {
                return ModuleTimers.ExtractSkillLEVELFromLine(line);
                //float faithskill = -1;
                //Match match = Regex.Match(line, @"to \d+\,\d+");
                //if (!match.Success)
                //{
                //    line = line.Replace(".", ",");
                //    match = Regex.Match(line, @"to \d+\,\d+");
                //}
                //if (!match.Success) match = Regex.Match(line, @"to \d+");

                //if (!match.Success)
                //{
                //    Logger.WriteLine("! Timers: processed skill line failed to match at ExtractSkillLEVELFromLine, line: " + line);
                //    return -1;
                //}
                //else if (float.TryParse(match.Value.Substring(3), out faithskill))
                //{
                //    return faithskill;
                //}
                //else return -1;
            }

            float PriestTimer_ExtractSkillGAINfromLine(string line)
            {
                return ModuleTimers.ExtractSkillGAINFromLine(line);
                //float faithskillgain = -1;
                //Match match = Regex.Match(line, @"by \d+\,\d+");
                //if (!match.Success)
                //{
                //    line = line.Replace(".", ",");
                //    match = Regex.Match(line, @"by \d+\,\d+");
                //}
                //if (!match.Success) match = Regex.Match(line, @"by \d+");

                //if (!match.Success)
                //{
                //    Logger.WriteLine("! Timers: processed skill line failed to match at ExtractSkillGAINfromLine, line: " + line);
                //    return -1;
                //}
                //else if (float.TryParse(match.Value.Substring(3), out faithskillgain))
                //{
                //    return faithskillgain;
                //}
                //else return -1;
            }

            public void CheckIfCooldownReset()
            {
                if (isCacheUpdated)
                {
                    TimeOnThisCooldownReset = DateTime.Now - CooldownResetSince;
                    if (TimeOnThisCooldownReset > TimeSpan.FromDays(1))
                    {
                        UpdateDateOfLastCooldownReset();
                        RevalidateFaithHistory();
                        UpdateNextPrayerDate();
                        RevalidateAlignmentHistory();
                        UpdateNextAlignmentDate();
                        ParentModule.UpdateUI(UpdateTimersUITypes.Priesthood);
                    }
                }
            }

            #endregion

            public void BeforeHandleLogs(bool engineInSleepMode)
            {
                CheckIfCooldownReset();
            }

            public void HandleNewLogEvent(List<string> newLogEvents, GameLogState log)
            {
                if (PriesthoodTimerEnabled)
                {
                    foreach (string line in newLogEvents)
                    {
                        if (log.GetLogType() == GameLogTypes.Event)
                        {
                            if (line.StartsWith("You finish your prayer", StringComparison.Ordinal))
                            {
                                PrayerHistory.Add(new PrayHistoryEntry(PrayHistoryEntryTypes.Prayed, DateTime.Now));
                                UpdateDateOfLastCooldownReset();
                                RevalidateFaithHistory();
                                UpdateNextPrayerDate();
                                ParentModule.UpdateUI(UpdateTimersUITypes.Priesthood);
                                ParentModule.TimersUI.PrayerJustHappened();
                            }
                            else if (line.StartsWith("The server has been up", StringComparison.Ordinal)) //"The server has been up 14 hours and 22 minutes."
                            {
                                UpdateDateOfLastCooldownReset();
                                RevalidateFaithHistory();
                                UpdateNextPrayerDate();
                                ParentModule.UpdateUI(UpdateTimersUITypes.Priesthood);
                            }
                            else if (line.Contains("is mighty pleased with you"))
                            {
                                PrayerHistory.Add(new PrayHistoryEntry(PrayHistoryEntryTypes.SermonMightyPleased, DateTime.Now));
                                UpdateDateOfLastCooldownReset();
                                RevalidateFaithHistory();
                                UpdateNextPrayerDate();
                            }
                            else if (line.StartsWith("You finish this sermon", StringComparison.Ordinal))
                            {
                                UpdateDateOfNextSermon(line, true);
                                ParentModule.TimersUI.SermonJustHappened();
                            }

                            if (AlignmentVerifier.CheckConditions(line, IsWhiteLighter, PlayerReligion))
                            {
                                AlignmentHistory.Add(new AlignmentHistoryEntry(DateTime.Now, reason: line, comesfromlivelogs: true));
                                UpdateDateOfLastCooldownReset();
                                RevalidateAlignmentHistory();
                                UpdateNextAlignmentDate();
                                ParentModule.UpdateUI(UpdateTimersUITypes.Priesthood);
                                ParentModule.TimersUI.AlignmentGainJustHappened();
                            }
                        }
                        else if (log.GetLogType() == GameLogTypes.Skills)
                        {
                            if (line.StartsWith("Alignment increased", StringComparison.Ordinal))
                            {
                                AlignmentHistory.Add(new AlignmentHistoryEntry(DateTime.Now, true));
                                UpdateDateOfLastCooldownReset();
                                RevalidateAlignmentHistory();
                                UpdateNextAlignmentDate();
                                ParentModule.UpdateUI(UpdateTimersUITypes.Priesthood);
                                ParentModule.TimersUI.AlignmentGainJustHappened();
                            }
                            // "[02:03:41] Faith increased by 0,124 to 27,020"
                            if (line.StartsWith("Faith increased", StringComparison.Ordinal) || line.StartsWith("Faith decreased", StringComparison.Ordinal))
                            {
                                float faithskillgain = PriestTimer_ExtractSkillGAINfromLine(line);
                                if (faithskillgain > 0)
                                {
                                    if (faithskillgain >= 0.120F)
                                    {
                                        PrayerHistory.Add(new PrayHistoryEntry(PrayHistoryEntryTypes.FaithGain120orMore, DateTime.Now));
                                    }
                                    else
                                    {
                                        PrayerHistory.Add(new PrayHistoryEntry(PrayHistoryEntryTypes.FaithGainBelow120, DateTime.Now));
                                    }
                                }
                                float extractedFaithSkill = PriestTimer_ExtractSkillLEVELFromLine(line);
                                if (extractedFaithSkill > 0)
                                {
                                    this.FaithSkill = extractedFaithSkill;
                                    Logger.WriteLine("Timers: updated faith for " + PlayerName + " to " + FaithSkill);
                                    ParentModule.UpdateUI(UpdateTimersUITypes.Priesthood);
                                }
                            }

                            if (line.StartsWith("Favor increased"))
                            {
                                FavorLevel = PriestTimer_ExtractSkillLEVELFromLine(line);
                                ParentModule.UpdateUI(UpdateTimersUITypes.Priesthood);
                            }
                        }
                    }
                }
            }

            DateTime FindLastValidPrayerInHistory()
            {
                if (PrayerHistory.Count > 0)
                {
                    for (int i = PrayerHistory.Count - 1; i >= 0; i--)
                    {
                        if (PrayerHistory[i].EntryType == PrayHistoryEntryTypes.Prayed)
                        {
                            if (PrayerHistory[i].Valid) return PrayerHistory[i].EntryDateTime;
                        }
                    }
                }
                return new DateTime(0);
            }

            DateTime FindLastValidAlignmentInHistory()
            {
                if (AlignmentHistory.Count > 0)
                {
                    for (int i = AlignmentHistory.Count - 1; i >= 0; i--)
                    {
                        if (AlignmentHistory[i].Valid) return AlignmentHistory[i].EntryDateTime;
                    }
                }
                return new DateTime(0);
            }

            #region PUBLIC ACCESS

            public DateTime GetNextPrayerDate()
            {
                return NextPrayDate;
            }

            public float GetFaithSkill()
            {
                return FaithSkill;
            }

            public DateTime GetNextSermonDate()
            {
                return DateOfNextSermon;
            }

            internal DateTime GetNextAlignmentDate()
            {
                return DateOfNextAlignment;
            }

            public bool AdjustFaithSkill(float value)
            {
                if (value >= 0 && value <= 100)
                {
                    FaithSkill = value;
                    return true;
                }
                else return false;
            }

            public bool IsPrayCountMaxed()
            {
                return isPrayCountMax;
            }

            public string[] GetAllPrayers()
            {
                string[] allprayers = new string[PrayerHistory.Count];
                int indexCounter = 0;
                foreach (PrayHistoryEntry entry in PrayerHistory)
                {
                    allprayers[indexCounter] = entry.EntryDateTime + ", " + entry.EntryType + ", " + entry.Valid;
                    indexCounter++;
                }
                return allprayers;
            }

            public string[] GetAllAlignments()
            {
                string[] allalignments = new string[AlignmentHistory.Count];
                int indexCounter = 0;
                foreach (AlignmentHistoryEntry entry in AlignmentHistory)
                {
                    allalignments[indexCounter] = entry.EntryDateTime + ", " + entry.Valid + (entry.AlwaysValid ? "(skill logs)" : "");
                    indexCounter++;
                }
                return allalignments;
            }

            public string[] GetAllAlignmentsForVerifyList()
            {
                List<string> allalignments = new List<string>();
                foreach (AlignmentHistoryEntry entry in AlignmentHistory)
                {
                    if (entry.EntryDateTime > DateTime.Now - TimeSpan.FromMinutes(31))
                    {
                        if (entry.AlwaysValid) allalignments.Add(entry.EntryDateTime + ", accurate (comes from skill log)");
                        else
                        {
                            string output;
                            if (entry.Reason != null)
                            {
                                output = (entry.ComesFromLiveLogs == true ? entry.EntryDateTime + ", " + entry.Reason : entry.Reason);
                            }
                            else
                            {
                                output = entry.EntryDateTime + ", reason missing";
                            }
                            allalignments.Add(output);
                        }
                    }
                }
                return allalignments.ToArray();
            }

            internal float GetFavorLevel()
            {
                return FavorLevel;
            }

            #endregion
        }

        public class MeditationsTimer
        {
            AC_SettingsDB ModuleSettings;
            string PlayerName;
            LogSearchManager LogSearchMan;
            ModuleTimers ParentModule;

            enum MeditationStates { Unlimited, Limited }
            enum MeditHistoryEntryTypes { Meditation, LongCooldownTrigger }

            //helpers
            public static TimeSpan LongMeditCooldown = new TimeSpan(3, 0, 0);
            public static TimeSpan ShortMeditCooldown = new TimeSpan(0, 30, 0);

            MeditationStates MeditState = MeditationStates.Unlimited;
            class MeditHistoryEntry
            {
                public MeditHistoryEntryTypes EntryType;
                public DateTime EntryDateTime;
                public bool Valid = false;

                public MeditHistoryEntry(MeditHistoryEntryTypes type, DateTime date)
                {
                    this.EntryType = type;
                    this.EntryDateTime = date;
                }
            }
            List<MeditHistoryEntry> MeditHistory = new List<MeditHistoryEntry>();
            DateTime NextMeditationDate = DateTime.Now;
            bool NextMeditDateUnknown = true;
            DateTime CooldownResetSince = DateTime.Now;
            TimeSpan TimeOnThisCooldownReset = new TimeSpan(0);
            bool isLongMeditCooldown = false;

            // threading control
            bool ScheduleUpdateCache = false;
            bool isMeditHistoryUpdateScheduled = false;
            bool isMeditHistoryUpdated = false;
            bool isMeditSkillUpdateScheduled = false;
            bool isMeditSkillUpdated = false;
            bool isQuestionLookupScheduled = false;
            bool isQuestionLookupUpdated = false;
            bool isCacheUpdated = false;

            public bool isSleepBonusActive = false;
            public DateTime SleepBonusStarted = new DateTime(0);

            //debug note: ENABLED saving
            #region MODULE SETTINGS

            float _meditationSkill = 0;
            float MeditationSkill
            {
                get { return _meditationSkill; }
                set
                {
                    _meditationSkill = value;
                    ModuleSettings.Set("MeditationSkill_" + PlayerName, value);
                    UpdateMeditState();
                    MeditSkillLastUpdateDate = DateTime.Now;
                }
            }

            DateTime _meditSkillLastUpdateDate = new DateTime(0);
            DateTime MeditSkillLastUpdateDate
            {
                get { return _meditSkillLastUpdateDate; }
                set
                {
                    _meditSkillLastUpdateDate = value;
                    ModuleSettings.Set("MeditSkillLastUpdateDate_" + PlayerName, value);
                }
            }

            string _meditFsleepWarningSound = "none";
            public string MeditFsleepWarningSound
            {
                get { return _meditFsleepWarningSound; }
                set
                {
                    _meditFsleepWarningSound = value;
                    ModuleSettings.Set("MeditFsleepWarningSound_" + PlayerName, value);
                }
            }

            string _meditReadySound = "none";
            public string MeditReadySound
            {
                get { return _meditReadySound; }
                set
                {
                    _meditReadySound = value;
                    ModuleSettings.Set("MeditReadySound_" + PlayerName, value);
                }
            }

            bool _trayCooldownNotify = false;
            public bool TrayCooldownNotify
            {
                get { return _trayCooldownNotify; }
                set
                {
                    _trayCooldownNotify = value;
                    ModuleSettings.Set("TrayCooldownNotify_" + PlayerName, _trayCooldownNotify);
                }
            }

            bool _traySleepBonusNotify = false;
            public bool TraySleepBonusNotify
            {
                get { return _traySleepBonusNotify; }
                set
                {
                    _traySleepBonusNotify = value;
                    ModuleSettings.Set("TraySleepBonusNotify_" + PlayerName, _traySleepBonusNotify);
                }
            }

            bool _meditTimerEnabled = false;
            public bool MeditTimerEnabled
            {
                get { return _meditTimerEnabled; }
                set
                {
                    _meditTimerEnabled = value;
                    ModuleSettings.Set("MeditTimerEnabled_" + PlayerName, value);
                    ScheduleUpdateCache = false;
                    isMeditHistoryUpdateScheduled = false;
                    isMeditHistoryUpdated = false;
                    isMeditSkillUpdateScheduled = false;
                    isMeditSkillUpdated = false;
                    isCacheUpdated = false;
                    MeditHistory.Clear();
                    TryToStartModule();
                }
            }

            bool _remindSleepBonus = false;
            public bool RemindSleepBonus
            {
                get { return _remindSleepBonus; }
                set
                {
                    _remindSleepBonus = value;
                    ModuleSettings.Set("RemindSleepBonus_" + PlayerName, value);
                }
            }

            // sleep bonus reminder

            DateTime _lastPathAdvanceCheckup = new DateTime(0);
            public DateTime LastPathAdvanceCheckup
            {
                get { return _lastPathAdvanceCheckup; }
                set
                {
                    _lastPathAdvanceCheckup = value;
                    ModuleSettings.Set("LastPathAdvanceCheckup" + PlayerName, value);
                }
            }

            DateTime _lastPathAttemptFailedOn = new DateTime(0);
            public DateTime LastPathAttemptFailedOn            //unused
            {
                get { return _lastPathAttemptFailedOn; }
                set
                {
                    _lastPathAttemptFailedOn = value;
                    ModuleSettings.Set("LastPathAttemptFailedOn" + PlayerName, value);
                }
            }

            DateTime _dateOfNextQuestionAttempt = new DateTime(0);
            public DateTime DateOfNextQuestionAttempt
            {
                get { return _dateOfNextQuestionAttempt; }
                set
                {
                    _dateOfNextQuestionAttempt = value;
                    ModuleSettings.Set("DateOfNextQuestionAttempt" + PlayerName, value);
                }
            }

            // question timer overrides
            bool _questionTimerOverriden = false;
            public bool QuestionTimerOverriden
            {
                get { return _questionTimerOverriden; }
                set
                {
                    _questionTimerOverriden = value;
                    ModuleSettings.Set("QuestionTimerOverriden_" + PlayerName, value);
                }
            }

            DateTime _questionTimerOverrideDate = DateTime.Now;
            public DateTime QuestionTimerOverrideDate
            {
                get { return _questionTimerOverrideDate; }
                set
                {
                    _questionTimerOverrideDate = value;
                    ModuleSettings.Set("QuestionTimerOverrideDate_" + PlayerName, value);
                }
            }

            void InitSettings()
            {
                _meditationSkill = ModuleSettings.Get("MeditationSkill_" + PlayerName, _meditationSkill);
                _meditSkillLastUpdateDate = ModuleSettings.Get("MeditSkillLastUpdateDate_" + PlayerName, _meditSkillLastUpdateDate);
                _meditFsleepWarningSound = ModuleSettings.Get("MeditFsleepWarningSound_" + PlayerName, _meditFsleepWarningSound);
                _meditReadySound = ModuleSettings.Get("MeditReadySound_" + PlayerName, _meditReadySound);
                _meditTimerEnabled = ModuleSettings.Get("MeditTimerEnabled_" + PlayerName, _meditTimerEnabled);
                _remindSleepBonus = ModuleSettings.Get("RemindSleepBonus_" + PlayerName, _remindSleepBonus);
                _trayCooldownNotify = ModuleSettings.Get("TrayCooldownNotify_" + PlayerName, _trayCooldownNotify);
                _traySleepBonusNotify = ModuleSettings.Get("TraySleepBonusNotify_" + PlayerName, _traySleepBonusNotify);
                _lastPathAdvanceCheckup = ModuleSettings.Get("LastPathAdvanceCheckup" + PlayerName, _lastPathAdvanceCheckup);
                _dateOfNextQuestionAttempt = ModuleSettings.Get("DateOfNextQuestionAttempt" + PlayerName, _dateOfNextQuestionAttempt);
                _lastPathAttemptFailedOn = ModuleSettings.Get("LastPathAttemptFailedOn" + PlayerName, _lastPathAttemptFailedOn);
                _questionTimerOverriden = ModuleSettings.Get("QuestionTimerOverriden_" + PlayerName, _questionTimerOverriden);
                _questionTimerOverrideDate = ModuleSettings.Get("QuestionTimerOverrideDate_" + PlayerName, _questionTimerOverrideDate);
            }

            #endregion

            public static class MeditPathHelper
            {
                static string[] Level0 = { "Uninitiated" };
                static string[] Level1 = { "Initiate" };
                static string[] Level2 = { "Eager", "Disturbed", "Gatherer", "Nice", "Ridiculous" };
                static string[] Level3 = { "Explorer", "Crazed", "Greedy", "Gentle", "Envious" };
                static string[] Level4 = { "Sheetfolder", "Deranged", "Strong", "Warm", "Hateful" };
                static string[] Level5 = { "Desertmind", "Sicko", "Released", "Goodhearted", "Finger" };
                static string[] Level6 = { "Observer", "Mental", "Unafraid", "Giving", "Sheep" };
                static string[] Level7 = { "Bookkeeper", "Psycho", "Brave", "Rock", "Snake" };
                static string[] Level8 = { "Mud-dweller", "Beast", "Performer", "Splendid", "Shark" };
                static string[] Level9 = { "Thought Eater", "Maniac", "Liberator", "Protector", "Infection" };
                static string[] Level10 = { "Crooked", "Drooling", "Force", "Respectful", "Swarm" };
                static string[] Level11 = { "Enlightened", "Gone", "Vibrant Light", "Saint", "Free" };
                static string[] Level12 = { "12th Hierophant", "12th Eidolon", "12th Sovereign", "12th Deva", "???" };
                static string[] Level13 = { "13th Hierophant", "13th Eidolon", "13th Sovereign", "13th Deva", "???" };
                static string[] Level14 = { "14th Hierophant", "14th Eidolon", "14th Sovereign", "14th Deva", "???" };
                static string[] Level15 = { "15th Hierophant", "15th Eidolon", "15th Sovereign", "15th Deva", "???" };

                public static Dictionary<int, string[]> LevelToTitlesMap = new Dictionary<int, string[]>();
                public static Dictionary<int, int> LevelToCooldownInHoursMap = new Dictionary<int, int>();

                static MeditPathHelper()
                {
                    LevelToTitlesMap.Add(0, Level0);
                    LevelToTitlesMap.Add(1, Level1);
                    LevelToTitlesMap.Add(2, Level2);
                    LevelToTitlesMap.Add(3, Level3);
                    LevelToTitlesMap.Add(4, Level4);
                    LevelToTitlesMap.Add(5, Level5);
                    LevelToTitlesMap.Add(6, Level6);
                    LevelToTitlesMap.Add(7, Level7);
                    LevelToTitlesMap.Add(8, Level8);
                    LevelToTitlesMap.Add(9, Level9);
                    LevelToTitlesMap.Add(10, Level10);
                    LevelToTitlesMap.Add(11, Level11);
                    LevelToTitlesMap.Add(12, Level12);
                    LevelToTitlesMap.Add(13, Level13);
                    LevelToTitlesMap.Add(14, Level14);
                    LevelToTitlesMap.Add(15, Level15);

                    LevelToCooldownInHoursMap.Add(0, 0);
                    LevelToCooldownInHoursMap.Add(1, 12);
                    LevelToCooldownInHoursMap.Add(2, 24);
                    LevelToCooldownInHoursMap.Add(3, 72);
                    LevelToCooldownInHoursMap.Add(4, 144);
                    LevelToCooldownInHoursMap.Add(5, 288);
                    LevelToCooldownInHoursMap.Add(6, 576);
                    LevelToCooldownInHoursMap.Add(7, 576);
                    LevelToCooldownInHoursMap.Add(8, 576);
                    LevelToCooldownInHoursMap.Add(9, 576);
                    LevelToCooldownInHoursMap.Add(10, 576);
                    LevelToCooldownInHoursMap.Add(11, 576);
                    LevelToCooldownInHoursMap.Add(12, 576);
                    LevelToCooldownInHoursMap.Add(13, 576);
                    LevelToCooldownInHoursMap.Add(14, 576);
                    LevelToCooldownInHoursMap.Add(15, 576);
                }

                public static int FindLevel(string line)
                {
                    foreach (var item in LevelToTitlesMap)
                    {
                        foreach (string title in item.Value)
                        {
                            if (Regex.IsMatch(line, title))
                            {
                                return item.Key;
                            }
                        }
                    }
                    return -1;
                }
            }

            public MeditationsTimer(ModuleTimers parentModule, string playername, AC_SettingsDB modulesettings)
            {
                this.ParentModule = parentModule;
                this.PlayerName = playername;
                this.ModuleSettings = modulesettings;
                InitSettings();
                LogSearchMan = ModuleLogSearcher.LogSearchMan;

                TryToStartModule();
            }

            void TryToStartModule()
            {
                if (MeditTimerEnabled) ScheduleUpdateCache = true;
            }

            #region PREPARE CACHE

            internal void OnPollingTick()
            {
                if (ScheduleUpdateCache)
                {
                    if (UpdateCache())
                    {
                        ScheduleUpdateCache = false;
                    }
                }
            }

            bool UpdateCache()
            {
                if (!isMeditSkillUpdateScheduled)
                {
                    if (BeginUpdateMeditSkillCache())
                    {
                        isMeditSkillUpdateScheduled = true;
                    }
                }
                if (!isMeditHistoryUpdateScheduled)
                {
                    if (BeginUpdateMeditHistoryCache())
                    {
                        isMeditHistoryUpdateScheduled = true;
                    }
                }
                if (!isQuestionLookupScheduled)
                {
                    if (BeginQuestionLookup())
                    {
                        isQuestionLookupScheduled = true;
                    }
                }

                if (isMeditSkillUpdateScheduled && isMeditHistoryUpdateScheduled && isQuestionLookupScheduled)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            bool BeginUpdateMeditSkillCache()
            {
                if (LogSearchMan != null)
                {
                    DateTime checkFromThisDate = MeditSkillLastUpdateDate;
                    DateTime minimumUpdateDate = DateTime.Now - TimeSpan.FromDays(7);
                    if (MeditSkillLastUpdateDate > minimumUpdateDate)
                    {
                        checkFromThisDate = minimumUpdateDate;
                    }

                    LogSearchData logsearchdata = new LogSearchData();
                    logsearchdata.BuildSearchData(
                        PlayerName,
                        GameLogTypes.Skills,
                        checkFromThisDate,
                        DateTime.Now,
                        "",
                        SearchTypes.RegexEscapedCaseIns);
                    logsearchdata.CallerControl = ParentModule.TimersUI;
                    logsearchdata.CallbackID = LogSearchDataIDs.TimersMeditSkill;

                    if (LogSearchMan.PerformSearch(logsearchdata))
                    {
                        return true;
                    }
                    else return false;
                }
                else return false;
            }

            void EndUpdateMeditSkillCache(LogSearchData logsearchdata) //called back
            {
                //DebugDump.DumpToTextFile("skill.txt", logsearchdata.AllLines);
                //do smtg with results
                MeditSkillLastUpdateDate = DateTime.Now;
                float mostRecentMeditSkill = -1;
                foreach (string line in logsearchdata.AllLines)
                {
                    if (line.Contains("Meditating increased"))
                    {
                        float meditskill = MeditTimer_ExtractSkillFromLine(line);
                        if (meditskill > 0) mostRecentMeditSkill = meditskill;
                    }
                    else if (line.Contains("Meditating decreased"))
                    {
                        float meditskill = MeditTimer_ExtractSkillFromLine(line);
                        if (meditskill > 0)
                        {
                            mostRecentMeditSkill = meditskill;
                        }
                    }
                }
                if (mostRecentMeditSkill > 0)
                {
                    Logger.WriteLine("Timers: Determined current meditation skill for player " + PlayerName + " to be " + mostRecentMeditSkill);
                    this.MeditationSkill = mostRecentMeditSkill;
                }
            }

            bool BeginUpdateMeditHistoryCache()
            {
                if (LogSearchMan != null)
                {
                    LogSearchData logsearchdata = new LogSearchData();
                    logsearchdata.BuildSearchData(
                        PlayerName,
                        GameLogTypes.Event,
                        DateTime.Now - TimeSpan.FromDays(2),
                        DateTime.Now,
                        "",
                        SearchTypes.RegexEscapedCaseIns);
                    logsearchdata.CallerControl = ParentModule.TimersUI;
                    logsearchdata.CallbackID = LogSearchDataIDs.TimersMeditHistory;

                    if (LogSearchMan.PerformSearch(logsearchdata))
                    {
                        return true;
                    }
                    else return false;
                }
                else return false;
            }

            void EndUpdateMeditHistoryCache(LogSearchData logsearchdata) //called back
            {
                //DebugDump.DumpToTextFile("history.txt", logsearchdata.AllLines);
                //do smtg with results
                // "You finish your meditation"
                // "You feel that it will take you a while before you are ready to meditate again."
                foreach (string line in logsearchdata.AllLines)
                {
                    if (line.Contains("You finish your meditation"))
                    {
                        DateTime datetime;
                        if (LogSearchManager.TryParseDateTimeFromSearchResultLine(line, out datetime))
                        {
                            MeditHistory.Add(new MeditHistoryEntry(MeditHistoryEntryTypes.Meditation, datetime));
                        }
                        else
                        {
                            Logger.WriteLine("! Timers: error while parsing date in EndUpdateMeditHistoryCache");
                        }
                    }
                    else if (line.Contains("You feel that it will take you a while before you are ready to meditate again"))
                    {
                        DateTime datetime;
                        if (LogSearchManager.TryParseDateTimeFromSearchResultLine(line, out datetime))
                        {
                            MeditHistory.Add(new MeditHistoryEntry(MeditHistoryEntryTypes.LongCooldownTrigger, datetime));
                        }
                        else
                        {
                            Logger.WriteLine("! Timers: error while parsing date in EndUpdateMeditHistoryCache");
                        }
                    }
                }

                //List<string> dumphistory = new List<string>();
                //foreach (MeditHistoryEntry entry in MeditHistory)
                //{
                //    dumphistory.Add(entry.EntryDateTime.ToString() + ", " + entry.EntryType.ToString());
                //}
                //DebugDump.DumpToTextFile("parsedhistory.txt", dumphistory);
            }

            bool BeginQuestionLookup()
            {
                if (LogSearchMan != null)
                {
                    // temporarily backsearch all 24 days due to missing question fail messages
                    TimeSpan timeSinceLastCheckup = TimeSpan.FromDays(25);
                    //TimeSpan timeSinceLastCheckup = DateTime.Now - LastPathAdvanceCheckup;
                    //if (timeSinceLastCheckup < TimeSpan.FromDays(2))
                    //{
                    //    timeSinceLastCheckup = TimeSpan.FromDays(2);
                    //}
                    //else if (timeSinceLastCheckup > TimeSpan.FromDays(25))
                    //{
                    //    timeSinceLastCheckup = TimeSpan.FromDays(25);
                    //}

                    LogSearchData logsearchdata = new LogSearchData();
                    logsearchdata.BuildSearchData(
                        PlayerName,
                        GameLogTypes.Event,
                        DateTime.Now - timeSinceLastCheckup,
                        DateTime.Now,
                        "",
                        SearchTypes.RegexEscapedCaseIns);
                    logsearchdata.CallerControl = ParentModule.TimersUI;
                    logsearchdata.CallbackID = LogSearchDataIDs.TimersMeditPathAdvance;

                    if (LogSearchMan.PerformSearch(logsearchdata))
                    {
                        return true;
                    }
                    else return false;
                }
                else return false;
            }

            void EndQuestionLookup(LogSearchData logsearchdata)
            {
                bool IsPathBegin = false;
                //[00:35:09] Congratulations! You have now reached the level of Rock of the path of love!
                string lastPathAdvancedLine = null;
                string lastPathFailLine = null;
                foreach (string line in logsearchdata.AllLines)
                {
                    if (line.Contains("You decide to start pursuing the insights of the path of"))
                    {
                        lastPathAdvancedLine = line;
                        lastPathFailLine = null;
                        IsPathBegin = true;
                    }

                    if (line.Contains("Congratulations! You have now reached the level"))
                    {
                        IsPathBegin = false;
                        lastPathAdvancedLine = line;
                        lastPathFailLine = null; //reset any previous fail finds because they are irrelevant now
                    }
                    //if (line.Contains("[fail message]")
                    //    lastPathFailLine = line;
                }
                if (lastPathAdvancedLine != null)
                {
                    UpdateDateOfNextQuestionAttempt(lastPathAdvancedLine, false, IsPathBegin);
                }
                if (lastPathFailLine != null)
                {
                    //NYI
                }
            }

            void UpdateDateOfNextQuestionAttempt(string line, bool liveLogs, bool pathBegin)
            {
                LastPathAdvanceCheckup = DateTime.Now;
                int cdInHrs = 0;
                int nextMeditLevel;

                if (pathBegin) nextMeditLevel = 1;
                else nextMeditLevel = MeditPathHelper.FindLevel(line) + 1;

                if (nextMeditLevel > 15) nextMeditLevel = 15;
                MeditPathHelper.LevelToCooldownInHoursMap.TryGetValue(
                     nextMeditLevel, out cdInHrs);
                DateTime dateOfThisLine;
                if (liveLogs)
                {
                    dateOfThisLine = DateTime.Now;
                    DateOfNextQuestionAttempt = dateOfThisLine + TimeSpan.FromHours(cdInHrs);
                }
                else
                {
                    if (LogSearchManager.TryParseDateTimeFromSearchResultLine(line, out dateOfThisLine))
                    {
                        DateOfNextQuestionAttempt = dateOfThisLine + TimeSpan.FromHours(cdInHrs);
                    }
                    else
                    {
                        //do nothing, whatever happened
                        Logger.WriteLine("!! Timers: Medit: Question lookup: parse error");
                    }
                }
            }

            public void CallbackHandler(LogSearchData logsearchdata)
            {
                if (logsearchdata.CallbackID == LogSearchDataIDs.TimersMeditSkill)
                {
                    EndUpdateMeditSkillCache(logsearchdata);
                    isMeditSkillUpdated = true;
                }
                else if (logsearchdata.CallbackID == LogSearchDataIDs.TimersMeditHistory)
                {
                    EndUpdateMeditHistoryCache(logsearchdata);
                    isMeditHistoryUpdated = true;
                }
                else if (logsearchdata.CallbackID == LogSearchDataIDs.TimersMeditPathAdvance)
                {
                    EndQuestionLookup(logsearchdata);
                    isQuestionLookupUpdated = true;
                }
                else
                {
                    Debug.WriteLine("! ModuleTimers: Meditation: Wrong callback id: " + logsearchdata.CallbackID.ToString());
                }

                if (isMeditSkillUpdated && isMeditHistoryUpdated)
                {
                    isCacheUpdated = true;
                    UpdateDateOfLastCooldownReset();
                    UpdateMeditState();
                    RevalidateMeditHistory();
                    UpdateNextMeditDate();
                    ParentModule.UpdateUI(UpdateTimersUITypes.Meditation);
                }
            }

            #endregion

            #region UPDATE CACHE (using live log parsing)

            void UpdateDateOfLastCooldownReset()
            {
                DateTime currentTime = DateTime.Now;
                DateTime cooldownResetDate = currentTime;

                TimeSpan timeSinceLastServerReset = currentTime - ModuleTimingAssist.ServerUpSince;
                TimeSpan daysSinceLastServerReset = new TimeSpan(timeSinceLastServerReset.Days, 0, 0, 0);
                timeSinceLastServerReset = timeSinceLastServerReset.Subtract(daysSinceLastServerReset);

                cooldownResetDate = currentTime - timeSinceLastServerReset;
                this.CooldownResetSince = cooldownResetDate;
            }

            void UpdateMeditState()
            {
                if (MeditationSkill < 20)
                {
                    MeditState = MeditationStates.Unlimited;
                }
                else
                {
                    MeditState = MeditationStates.Limited;
                }
            }

            void RevalidateMeditHistory()
            {
                DateTime lastValidEntry = new DateTime(0);
                TimeSpan currentCooldownTimeSpan = ShortMeditCooldown;
                //validate entries
                foreach (MeditHistoryEntry entry in MeditHistory)
                {
                    entry.Valid = false;
                    //all entries are default invalid
                    //discard any entry prior to cooldown reset
                    if (entry.EntryDateTime > CooldownResetSince)
                    {
                        if (entry.EntryType == MeditHistoryEntryTypes.LongCooldownTrigger)
                        {
                            //apply longer cooldown from this point
                            currentCooldownTimeSpan = LongMeditCooldown;
                            this.isLongMeditCooldown = true;
                        }

                        //if entry date is later, than last valid + cooldown period
                        if (entry.EntryDateTime > lastValidEntry + currentCooldownTimeSpan)
                        {
                            entry.Valid = true;
                            lastValidEntry = entry.EntryDateTime;
                        }
                    }
                }
                // resets medit cooldown type in case long is set from previous uptime period
                if (currentCooldownTimeSpan == ShortMeditCooldown) this.isLongMeditCooldown = false;

                //debug
                //List<string> dumphistory = new List<string>();
                //foreach (MeditHistoryEntry entry in MeditHistory)
                //{
                //    dumphistory.Add(entry.EntryDateTime.ToString() + ", " + entry.EntryType.ToString() + ", " + entry.Valid.ToString());
                //}
                //DebugDump.DumpToTextFile("meditvalidatedlist.txt", dumphistory);
            }

            void UpdateNextMeditDate()
            {
                if (MeditState == MeditationStates.Limited)
                {
                    if (isLongMeditCooldown)
                    {
                        NextMeditationDate = FindLastValidMeditInHistory() + LongMeditCooldown;
                    }
                    else
                    {
                        NextMeditationDate = FindLastValidMeditInHistory() + ShortMeditCooldown;
                    }

                    if (NextMeditationDate > CooldownResetSince + TimeSpan.FromDays(1))
                    {
                        NextMeditationDate = CooldownResetSince + TimeSpan.FromDays(1);
                    }
                }
                else this.NextMeditationDate = DateTime.Now;
            }

            float MeditTimer_ExtractSkillFromLine(string line)
            {
                return ModuleTimers.ExtractSkillLEVELFromLine(line);
                //float meditskill = -1;
                //Match match = Regex.Match(line, @"to \d+\,\d+");
                //if (!match.Success)
                //{
                //    line = line.Replace(".", ",");
                //    match = Regex.Match(line, @"to \d+\,\d+");
                //}
                //if (!match.Success) match = Regex.Match(line, @"to \d+");

                //if (float.TryParse(match.Value.Substring(3), out meditskill))
                //{
                //    return meditskill;
                //}
                //else return -1;
            }

            public void CheckIfCooldownReset()
            {
                if (isCacheUpdated)
                {
                    TimeOnThisCooldownReset = DateTime.Now - CooldownResetSince;
                    if (TimeOnThisCooldownReset > TimeSpan.FromDays(1))
                    {
                        UpdateDateOfLastCooldownReset();
                        RevalidateMeditHistory();
                        UpdateNextMeditDate();
                        ParentModule.UpdateUI(UpdateTimersUITypes.Meditation);
                    }
                }
            }

            #endregion

            public void BeforeHandleLogs(bool engineInSleepMode)
            {
                CheckIfCooldownReset();
            }

            public void HandleNewLogEvent(List<string> newLogEvents, GameLogState log)
            {
                if (MeditTimerEnabled)
                {
                    foreach (string line in newLogEvents)
                    {
                        if (log.GetLogType() == GameLogTypes.Event)
                        {
                            if (line.StartsWith("You finish your meditation", StringComparison.Ordinal))
                            {
                                MeditHistory.Add(new MeditHistoryEntry(MeditHistoryEntryTypes.Meditation, DateTime.Now));
                                UpdateDateOfLastCooldownReset();
                                RevalidateMeditHistory();
                                UpdateNextMeditDate();
                                ParentModule.UpdateUI(UpdateTimersUITypes.Meditation);
                                ParentModule.TimersUI.MeditJustHappened();
                            }
                            else if (line.StartsWith("The server has been up", StringComparison.Ordinal)) //"The server has been up 14 hours and 22 minutes."
                            {
                                UpdateDateOfLastCooldownReset();
                                RevalidateMeditHistory();
                                UpdateNextMeditDate();
                                ParentModule.UpdateUI(UpdateTimersUITypes.Meditation);
                            }
                            else if (line.StartsWith("You start using the sleep bonus", StringComparison.Ordinal))
                            {
                                isSleepBonusActive = true;
                                SleepBonusStarted = DateTime.Now;
                            }
                            else if (line.StartsWith("You refrain from using the sleep bonus", StringComparison.Ordinal))
                            {
                                isSleepBonusActive = false;
                            }
                            //[04:31:56] You feel that it will take you a while before you are ready to meditate again.
                            else if (line.StartsWith("You feel that it will take", StringComparison.Ordinal))
                            {
                                if (line.StartsWith("You feel that it will take you a while before you are ready to meditate again", StringComparison.Ordinal))
                                {
                                    MeditHistory.Add(new MeditHistoryEntry(MeditHistoryEntryTypes.LongCooldownTrigger, DateTime.Now));
                                    UpdateDateOfLastCooldownReset();
                                    RevalidateMeditHistory();
                                    UpdateNextMeditDate();
                                }
                            }
                            else if (line.StartsWith("Congratulations", StringComparison.Ordinal))
                            {
                                if (line.Contains("Congratulations! You have now reached the level"))
                                {
                                    UpdateDateOfNextQuestionAttempt(line, true, false);
                                }
                            }
                            else if (line.StartsWith("You decide", StringComparison.Ordinal))
                            {
                                if (line.Contains("You decide to start pursuing the insights of the path of"))
                                {
                                    UpdateDateOfNextQuestionAttempt(line, true, true);
                                }
                            }
                        }
                        else if (log.GetLogType() == GameLogTypes.Skills)
                        {
                            // "[02:03:41] Meditating increased by 0,124 to 27,020"
                            if (line.StartsWith("Meditating increased", StringComparison.Ordinal) || line.StartsWith("Meditating decreased", StringComparison.Ordinal))
                            {
                                //parse into value
                                float extractedMeditSkill = MeditTimer_ExtractSkillFromLine(line);
                                if (extractedMeditSkill > 0)
                                {
                                    this.MeditationSkill = extractedMeditSkill;
                                    UpdateMeditState();
                                    Logger.WriteLine("Timers: updated meditation skill for " + PlayerName + " to " + MeditationSkill);
                                    ParentModule.UpdateUI(UpdateTimersUITypes.Meditation);
                                }
                            }
                        }
                    }
                }
            }

            DateTime FindLastValidMeditInHistory()
            {
                if (MeditHistory.Count > 0)
                {
                    for (int i = MeditHistory.Count - 1; i >= 0; i--)
                    {
                        if (MeditHistory[i].EntryType == MeditHistoryEntryTypes.Meditation)
                        {
                            if (MeditHistory[i].Valid) return MeditHistory[i].EntryDateTime;
                        }
                    }
                }
                return new DateTime(0);
            }

            #region PUBLIC ACCESS

            public DateTime GetNextMeditDate()
            {
                return NextMeditationDate;
            }

            public float GetMeditSkill()
            {
                return MeditationSkill;
            }

            public bool AdjustMeditSkill(float value)
            {
                if (value >= 0 && value <= 100)
                {
                    MeditationSkill = value;
                    return true;
                }
                else return false;
            }

            public bool IsLongCooldown()
            {
                return isLongMeditCooldown;
            }

            public string[] GetAllMedits()
            {
                string[] allmedits = new string[MeditHistory.Count];
                int indexCounter = 0;
                foreach (MeditHistoryEntry entry in MeditHistory)
                {
                    allmedits[indexCounter] = entry.EntryDateTime + ", " + entry.EntryType + ", " + entry.Valid;
                    indexCounter++;
                }
                return allmedits;
            }

            internal void HandleQTimerOverride(int meditLevel, DateTime originDate)
            {
                int cdInHours;
                if (MeditPathHelper.LevelToCooldownInHoursMap.TryGetValue(meditLevel, out cdInHours))
                {
                    QuestionTimerOverriden = true;
                    QuestionTimerOverrideDate = originDate + TimeSpan.FromHours(cdInHours);
                }
            }

            #endregion
        }

        public class LockpickingTimer
        {
            private ModuleTimers ParentModule;
            private string PlayerName;
            LogSearchManager LogSearchMan;
            private AC_SettingsDB ModuleSettings;

            public static TimeSpan LockpickCooldown = new TimeSpan(0, 10, 0);

            bool ScheduleUpdateCache = false;
            bool isLockpickSkillGainHistoryUpdateScheduled = false;
            bool isLockpickSkillGainHistoryUpdated = false;
            bool isCacheUpdated = false;

            DateTime LockpickLastDate = new DateTime(0);
            public DateTime NextLockpickDate = new DateTime(0);

            #region MODULE SETTINGS

            float _lockpickSkill = 0;
            float LockpickSkill
            {
                get { return _lockpickSkill; }
                set
                {
                    _lockpickSkill = value;
                    ModuleSettings.Set("LockpickSkill_" + PlayerName, value);
                }
            }

            string _lockpickReadySound = "none";
            public string LockpickReadySound
            {
                get { return _lockpickReadySound; }
                set
                {
                    _lockpickReadySound = value;
                    ModuleSettings.Set("LockpickReadySound_" + PlayerName, value);
                }
            }

            bool _trayLockpickNotify = false;
            public bool TrayLockpickNotify
            {
                get { return _trayLockpickNotify; }
                set
                {
                    _trayLockpickNotify = value;
                    ModuleSettings.Set("TrayLockpickNotify_" + PlayerName, value);
                }
            }

            bool _lockpickingTimerEnabled = false;
            public bool LockpickingTimerEnabled
            {
                get { return _lockpickingTimerEnabled; }
                set
                {
                    _lockpickingTimerEnabled = value;
                    ModuleSettings.Set("LockpickingTimerEnabled_" + PlayerName, value);
                    TryToStartModule();
                }
            }

            void InitSettings()
            {
                _lockpickSkill = ModuleSettings.Get("LockpickSkill_" + PlayerName, _lockpickSkill);
                _lockpickReadySound = ModuleSettings.Get("LockpickReadySound_" + PlayerName, _lockpickReadySound);
                _trayLockpickNotify = ModuleSettings.Get("TrayLockpickNotify_" + PlayerName, _trayLockpickNotify);
                _lockpickingTimerEnabled = ModuleSettings.Get("LockpickingTimerEnabled_" + PlayerName, _lockpickingTimerEnabled);
            }

            #endregion

            public LockpickingTimer(ModuleTimers moduleTimers, string PlayerName, AC_SettingsDB ModuleSettings)
            {
                this.ParentModule = moduleTimers;
                this.PlayerName = PlayerName;
                this.ModuleSettings = ModuleSettings;

                InitSettings();
                LogSearchMan = ModuleLogSearcher.LogSearchMan;

                TryToStartModule();
            }

            void TryToStartModule()
            {
                ScheduleUpdateCache = false;
                if (LockpickingTimerEnabled)
                {
                    isLockpickSkillGainHistoryUpdateScheduled = false;
                    isLockpickSkillGainHistoryUpdated = false;
                    isCacheUpdated = false;
                    ScheduleUpdateCache = true;
                }
            }

            internal void OnPollingTick()
            {
                if (ScheduleUpdateCache)
                {
                    if (UpdateCache())
                    {
                        ScheduleUpdateCache = false;
                    }
                }
            }

            private bool UpdateCache()
            {
                if (!isLockpickSkillGainHistoryUpdateScheduled)
                {
                    if (BeginUpdateLockpickSkillCache())
                    {
                        isLockpickSkillGainHistoryUpdateScheduled = true;
                    }
                }

                if (isLockpickSkillGainHistoryUpdateScheduled)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            private bool BeginUpdateLockpickSkillCache()
            {
                if (LogSearchMan != null)
                {
                    LogSearchData logsearchdata = new LogSearchData();
                    logsearchdata.BuildSearchData(
                        PlayerName,
                        GameLogTypes.Skills,
                        DateTime.Now - TimeSpan.FromDays(2),
                        DateTime.Now,
                        "",
                        SearchTypes.RegexEscapedCaseIns);
                    logsearchdata.CallerControl = ParentModule.TimersUI;
                    logsearchdata.CallbackID = LogSearchDataIDs.TimersLockpickingHistory;

                    if (LogSearchMan.PerformSearch(logsearchdata))
                    {
                        return true;
                    }
                    else return false;
                }
                else return false;
            }

            void EndUpdateLockpickSkillCache(LogSearchData logsearchdata) //called back
            {
                //DebugDump.DumpToTextFile("skill.txt", logsearchdata.AllLines);
                //do smtg with results
                string mostRecentSkillLogLine = null;
                foreach (string line in logsearchdata.AllLines)
                {
                    if (line.Contains("Lock picking increased"))
                    {
                        mostRecentSkillLogLine = line;
                        float lockpickskill = ExtractLockpickSkillFromLine(line);
                        if (lockpickskill > 0)
                        {
                            this.LockpickSkill = lockpickskill;
                        }
                    }
                }
                DateTime lockpickdate;
                if (LogSearchManager.TryParseDateTimeFromSearchResultLine(mostRecentSkillLogLine, out lockpickdate))
                {
                    this.LockpickLastDate = lockpickdate;
                    UpdateDateOfNextLockpick(mostRecentSkillLogLine, false);
                }
            }

            void UpdateDateOfNextLockpick(string line, bool liveLogs)
            {
                DateTime dateOfThisLine;
                if (liveLogs)
                {
                    dateOfThisLine = DateTime.Now;
                    NextLockpickDate = dateOfThisLine + LockpickCooldown;
                }
                else
                {
                    if (LogSearchManager.TryParseDateTimeFromSearchResultLine(line, out dateOfThisLine))
                    {
                        NextLockpickDate = dateOfThisLine + LockpickCooldown;
                    }
                    else
                    {
                        //do nothing, whatever happened
                        Logger.WriteLine("!! Timers: Lockpick lookup: parse error");
                    }
                }
            }

            internal void CallbackHandler(LogSearchData logsearchdata)
            {
                if (logsearchdata.CallbackID == LogSearchDataIDs.TimersLockpickingHistory)
                {
                    EndUpdateLockpickSkillCache(logsearchdata);
                    isLockpickSkillGainHistoryUpdated = true;
                }
                else
                {
                    Debug.WriteLine("! ModuleTimers: Priesthood: Wrong callback id: " + logsearchdata.CallbackID.ToString());
                }

                if (isLockpickSkillGainHistoryUpdated)
                {
                    isCacheUpdated = true;
                    ParentModule.UpdateUI(UpdateTimersUITypes.Lockpicking);
                }
            }

            private float ExtractLockpickSkillFromLine(string line)
            {
                float lockpickskill = -1;
                Match match = Regex.Match(line, @"to \d+\,\d+");
                if (!match.Success)
                {
                    line = line.Replace(".", ",");
                    match = Regex.Match(line, @"to \d+\,\d+");
                }
                if (!match.Success) match = Regex.Match(line, @"to \d+");

                if (float.TryParse(match.Value.Substring(3), out lockpickskill))
                {
                    return lockpickskill;
                }
                else return -1;
            }

            internal void HandleNewLogEvent(List<string> newLogEvents, GameLogState log)
            {
                if (LockpickingTimerEnabled)
                {
                    foreach (string line in newLogEvents)
                    {
                        if (log.GetLogType() == GameLogTypes.Skills)
                        {
                            if (line.StartsWith("Lock picking increased", StringComparison.Ordinal))
                            {
                                UpdateDateOfNextLockpick(line, true);
                                ParentModule.UpdateUI(UpdateTimersUITypes.Lockpicking);
                                ParentModule.TimersUI.LockpickGainJustHappened();
                            }
                        }
                    }
                }
            }

            public DateTime GetNextLockpickDate()
            {
                return NextLockpickDate;
            }

            public float GetLockpickSkill()
            {
                return LockpickSkill;
            }
        }

        public enum UpdateTimersUITypes { Meditation, Priesthood, Lockpicking }

        public string PlayerName = "";
        public MeditationsTimer MeditTimer;
        public PriesthoodTimer PriestTimer;
        public LockpickingTimer LockpickTimer;
        FormTimers TimersUI;

        public ModuleTimers(string playername)
            : base("Timers_" + playername)
        {
            this.PlayerName = playername;
            InitSettings();
            MeditTimer = new MeditationsTimer(this, PlayerName, ModuleSettings);
            PriestTimer = new PriesthoodTimer(this, PlayerName, ModuleSettings);
            LockpickTimer = new LockpickingTimer(this, PlayerName, ModuleSettings);
            TimersUI = new FormTimers(this);
        }

        void InitSettings()
        {
        }

        public override void BeforeHandleLogs(bool engineInSleepMode)
        {
            MeditTimer.BeforeHandleLogs(engineInSleepMode);
            PriestTimer.BeforeHandleLogs(engineInSleepMode);
        }

        public override void HandleNewLogEvents(List<string> newLogEvents, GameLogState log)
        {
            MeditTimer.HandleNewLogEvent(newLogEvents, log);
            PriestTimer.HandleNewLogEvent(newLogEvents, log);
            LockpickTimer.HandleNewLogEvent(newLogEvents, log);
        }

        public override void OnPollingTick(bool engineInSleepMode)
        {
            MeditTimer.OnPollingTick();
            PriestTimer.OnPollingTick();
            LockpickTimer.OnPollingTick();
        }

        public void ToggleUI(object sender, EventArgs e)
        {
            ToggleUI();
        }

        public void ToggleUI()
        {
            if (TimersUI.Visible)
            {
                TimersUI.Hide();
            }
            else
            {
                TimersUI.Show();
                TimersUI.RestoreFromMin();
            }
        }

        public void HandleSearchCallback(LogSearchData logsearchdata)
        {
            MeditTimer.CallbackHandler(logsearchdata);
            PriestTimer.CallbackHandler(logsearchdata);
            LockpickTimer.CallbackHandler(logsearchdata);
        }

        public void UpdateUI(UpdateTimersUITypes uiType)
        {
            if (uiType == UpdateTimersUITypes.Meditation) TimersUI.UpdateMeditOutput();
            if (uiType == UpdateTimersUITypes.Priesthood) TimersUI.UpdatePriesthoodOutput();
            if (uiType == UpdateTimersUITypes.Lockpicking) TimersUI.UpdateLockpickOutput();
        }

        /// <summary>
        /// returns -1 if failed
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static float ExtractSkillGAINFromLine(string line)
        {
            return GeneralHelper.ExtractSkillGAINFromLine(line);
        }

        /// <summary>
        /// returns -1 if failed
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static float ExtractSkillLEVELFromLine(string line)
        {
            return GeneralHelper.ExtractSkillLEVELFromLine(line);
        }
    }
}
