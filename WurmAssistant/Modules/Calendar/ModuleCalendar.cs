using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WurmAssistant
{
    public class ModuleCalendar : Module
    {
        public enum WurmSeasonsEnum { Olive, Oleander, Maple, Camellia, Lavender, Rose, Cherry, Grape, Apple, Lemon }

        public struct WurmSeasonData
        {
            public WurmSeasonsEnum SeasonEnum;
            public string SeasonName;
            public int DayBegin;
            public int DayEnd;
            public int Length;

            public WurmSeasonData(WurmSeasonsEnum seasonEnum, string seasonName, int dayBegin, int dayEnd)
            {
                this.SeasonEnum = seasonEnum;
                this.SeasonName = seasonName;
                this.DayBegin = dayBegin;
                this.DayEnd = dayEnd;
                Length = DayEnd - DayBegin;
            }
        }

        static class WurmSeasons
        {
            public static List<WurmSeasonData> Seasons = new List<WurmSeasonData>();
            public const double WurmYearInDays = 336.0D;
            static WurmSeasons()
            {
                Seasons.Add(new WurmSeasonData(WurmSeasonsEnum.Oleander, "Oleander", 85, 91));
                Seasons.Add(new WurmSeasonData(WurmSeasonsEnum.Maple, "Maple", 113, 119));
                Seasons.Add(new WurmSeasonData(WurmSeasonsEnum.Rose, "Rose", 113, 140));
                Seasons.Add(new WurmSeasonData(WurmSeasonsEnum.Rose, "Rose", 141, 147));
                Seasons.Add(new WurmSeasonData(WurmSeasonsEnum.Lavender, "Lavender", 113, 119));
                Seasons.Add(new WurmSeasonData(WurmSeasonsEnum.Lavender, "Lavender", 141, 147));
                Seasons.Add(new WurmSeasonData(WurmSeasonsEnum.Camellia, "Camellia", 113, 126));
                Seasons.Add(new WurmSeasonData(WurmSeasonsEnum.Cherry, "Cherry", 176, 196));
                Seasons.Add(new WurmSeasonData(WurmSeasonsEnum.Olive, "Olive", 204, 224));
                Seasons.Add(new WurmSeasonData(WurmSeasonsEnum.Olive, "Olive", 92, 112));
                Seasons.Add(new WurmSeasonData(WurmSeasonsEnum.Grape, "Grape", 225, 252));
                Seasons.Add(new WurmSeasonData(WurmSeasonsEnum.Apple, "Apple", 225, 252));
                Seasons.Add(new WurmSeasonData(WurmSeasonsEnum.Lemon, "Lemon", 288, 308));

                //Seasons.Add(new WurmSeasonData(WurmSeasonsEnum.Test, "Test", 65, 91));
            }
        }

        public class WurmSeasonOutputItem : IComparable<WurmSeasonOutputItem>
        {
            WurmSeasonData SeasonData;
            int LengthDays;
            bool inSeason = false;
            bool lastInSeasonState = false;
            public bool notifyUser = false;
            TimeSpan RealTimeToSeason;
            TimeSpan WurmTimeToSeason;
            TimeSpan RealTimeToSeasonEnd;
            TimeSpan WurmTimeToSeasonEnd;
            double CompareValue;
            double CompareOffset;

            public WurmSeasonOutputItem(WurmSeasonData wurmSeasonData, double compareOffset)
            {
                SeasonData = wurmSeasonData;
                LengthDays = SeasonData.DayEnd - SeasonData.DayBegin + 1;
                CompareOffset = compareOffset;
                Update();
            }

            public void Update()
            {
                lastInSeasonState = inSeason;

                if (ModuleTimingAssist.CurrentWurmDateTime.DayInYear >= SeasonData.DayBegin
                    && ModuleTimingAssist.CurrentWurmDateTime.DayInYear <= SeasonData.DayEnd)
                {
                    inSeason = true;
                }
                else inSeason = false;

                if (inSeason)
                {
                    WurmTimeToSeasonEnd = GetTimeToSeasonEnd(SeasonData.DayEnd + 1);
                    RealTimeToSeasonEnd = new TimeSpan(WurmTimeToSeasonEnd.Ticks / 8);
                    CompareValue = (TimeSpan.FromDays(-WurmSeasons.WurmYearInDays) + WurmTimeToSeasonEnd).TotalSeconds + CompareOffset;
                }
                else
                {
                    WurmTimeToSeason = GetTimeToSeason(SeasonData.DayBegin);
                    RealTimeToSeason = new TimeSpan(WurmTimeToSeason.Ticks / 8);
                    CompareValue = WurmTimeToSeason.TotalSeconds + CompareOffset;
                }

                if (inSeason == true && lastInSeasonState == false) 
                    notifyUser = true;
            }

            TimeSpan GetTimeToSeason(int dayBegin)
            {
                return GetTimeToDay(dayBegin);
            }

            TimeSpan GetTimeToSeasonEnd(int dayEnd)
            {
                return GetTimeToDay(dayEnd);
            }

            TimeSpan GetTimeToDay(int day)
            {
                TimeSpan Value = TimeSpan.FromDays(day);
                if (Value < ModuleTimingAssist.CurrentWurmDateTime.TimeAndDayOfYear)
                {
                    return TimeSpan.FromDays(Value.Days + 336) - ModuleTimingAssist.CurrentWurmDateTime.TimeAndDayOfYear;
                }
                return Value - ModuleTimingAssist.CurrentWurmDateTime.TimeAndDayOfYear;
            }

            public string BuildName()
            {
                return SeasonData.SeasonName;
            }

            public string BuildTimeData(bool wurmTime)
            {
                string value;
                if (inSeason)
                {
                    value = "IN SEASON!";
                }
                else
                {
                    if (wurmTime) value = ParseTimeSpanToNiceStringDMS(WurmTimeToSeason);
                    else value = ParseTimeSpanToNiceStringDMS(RealTimeToSeason);
                }
                return value;
            }

            public string BuildLengthData(bool wurmTime)
            {
                string value;
                if (inSeason)
                {
                    if (wurmTime) value = ParseTimeSpanToNiceStringDMS(WurmTimeToSeasonEnd) + "more";
                    else value = ParseTimeSpanToNiceStringDMS(RealTimeToSeasonEnd) + "more";
                }
                else
                {
                    if (wurmTime) value = String.Format("{0} days", LengthDays.ToString());
                    else
                    {
                        TimeSpan ts = TimeSpan.FromDays((double)LengthDays / 8D);
                        value = ParseTimeSpanToNiceStringDMS(ts);
                    }
                }
                return value;
            }

            string ParseTimeSpanToNiceStringDMS(TimeSpan ts, bool noMinutes = false)
            {
                string value = "";
                if (ts.Days > 0)
                {
                    if (ts.Days == 1) value += String.Format("{0} day ", ts.Days);
                    else value += String.Format("{0} days ", ts.Days);
                }
                if (ts.Hours > 0 || noMinutes)
                {
                    if (ts.Hours == 1) value += String.Format("{0} hour ", ts.Hours);
                    else value += String.Format("{0} hours ", ts.Hours);
                }
                if (!noMinutes)
                {
                    if (ts.Minutes == 1) value += String.Format("{0} minute ", ts.Minutes);
                    else value += String.Format("{0} minutes ", ts.Minutes);
                }
                return value;
            }

            public int CompareTo(WurmSeasonOutputItem dtlm)
            {
                return this.CompareValue.CompareTo(dtlm.CompareValue);
            }

            public bool ShouldNotifyUser()
            {
                return notifyUser;
            }

            public bool isItemTracked(Dictionary<string, WurmSeasonsEnum> TrackedSeasons)
            {
                return TrackedSeasons.ContainsKey(SeasonData.SeasonName);
            }

            public string GetSeasonName()
            {
                return SeasonData.SeasonName;
            }

            public DateTime GetSeasonEndDate()
            {
                return DateTime.Now + RealTimeToSeasonEnd;
            }

            public void ResetInSeasonFlag()
            {
                lastInSeasonState = false;
                inSeason = false;
            }

            public void UserNotified()
            {
                notifyUser = false;
            }

            public bool IsItemInSeason()
            {
                return inSeason;
            }
        }

        #region SETTINGS

        bool _useWurmTimeForDisplay = false;
        public bool UseWurmTimeForDisplay
        {
            get { return _useWurmTimeForDisplay; }
            set
            {
                _useWurmTimeForDisplay = value;
                ModuleSettings.Set("UseWurmTimeForDisplay", _useWurmTimeForDisplay);
            }
        }

        bool _soundWarning = false;
        public bool SoundWarning
        {
            get { return _soundWarning; }
            set
            {
                _soundWarning = value;
                ModuleSettings.Set("SoundWarning", _soundWarning);
            }
        }

        string _soundName = "none";
        public string SoundName
        {
            get { return _soundName; }
            set
            {
                _soundName = value;
                ModuleSettings.Set("SoundName", _soundName);
            }
        }

        bool _popupWarning = false;
        public bool PopupWarning
        {
            get { return _popupWarning; }
            set
            {
                _popupWarning = value;
                ModuleSettings.Set("PopupWarning", _popupWarning);
            }
        }

        string __trackedSeasonListRetriever = "none";
        Dictionary<string, WurmSeasonsEnum> _TrackedSeasonListRetriever
        {
            get
            {
                Dictionary<string, WurmSeasonsEnum> dict = new Dictionary<string, WurmSeasonsEnum>();
                string parsedstring = __trackedSeasonListRetriever;
                if (parsedstring != "none")
                {
                    foreach (WurmSeasonsEnum enumer in Enum.GetValues(typeof(WurmSeasonsEnum)))
                    {
                        if (parsedstring.Contains(enumer.ToString()))
                        {
                            dict.Add(enumer.ToString(), enumer);
                        }
                    }
                    return dict;
                }
                else return dict;

            }
            set
            {
                string newval = "";
                if (value.Count == 0) newval = "none";
                else
                {
                    foreach (var keyval in value)
                    {
                        newval += keyval.Key;
                    }
                }
                __trackedSeasonListRetriever = newval;
                ModuleSettings.Set("TrackedSeasonListRetriever", __trackedSeasonListRetriever);
            }
        }

        void InitSettings()
        {
            _useWurmTimeForDisplay = ModuleSettings.Get("UseWurmTimeForDisplay", _useWurmTimeForDisplay);
            _soundWarning = ModuleSettings.Get("SoundWarning", _soundWarning);
            _popupWarning = ModuleSettings.Get("PopupWarning", _popupWarning);
            __trackedSeasonListRetriever = ModuleSettings.Get("TrackedSeasonListRetriever", __trackedSeasonListRetriever);
            _soundName = ModuleSettings.Get("SoundName", _soundName);
        }

        #endregion

        List<WurmSeasonOutputItem> WurmSeasonOutput = new List<WurmSeasonOutputItem>();
        Dictionary<string, WurmSeasonsEnum> TrackedSeasons = new Dictionary<string, WurmSeasonsEnum>();

        FormCalendar CalendarUI;

        public ModuleCalendar()
            : base("Calendar")
        {
            InitSettings();
            TrackedSeasons = _TrackedSeasonListRetriever;
            CalendarUI = new FormCalendar(this);
            CalendarUI.UpdateTrackedSeasonsList(TrackedSeasons.Keys.ToArray<string>());
            double compareOffset = 0D;
            foreach (WurmSeasonData seasondata in WurmSeasons.Seasons)
            {
                WurmSeasonOutput.Add(new WurmSeasonOutputItem(seasondata, compareOffset));
                compareOffset += 0.1D;
            }
            //PopupTest();
        }

        void PopupTest()
        {
            PopupQueue.Add(new KeyValuePair<string, DateTime>("test1", DateTime.Now));
            PopupQueue.Add(new KeyValuePair<string, DateTime>("test2", DateTime.Now));
            PopupQueue.Add(new KeyValuePair<string, DateTime>("test3", DateTime.Now));
            PopupQueue.Add(new KeyValuePair<string, DateTime>("test4", DateTime.Now));
            popupScheduled = true;
        }

        public override void BeforeHandleLogs(bool engineInSleepMode)
        {
            UpdateOutputList(engineInSleepMode);
        }

        public void ToggleUI()
        {
            if (CalendarUI.Visible)
            {
                CalendarUI.Hide();
            }
            else
            {
                CalendarUI.Show();
                CalendarUI.RestoreFromMin();
            }
        }

        List<KeyValuePair<string, DateTime>> PopupQueue = new List<KeyValuePair<string, DateTime>>();
        bool popupScheduled = false;

        public void UpdateOutputList(bool sleepMode)
        {
            if (!sleepMode || PopupWarning || SoundWarning || CalendarUI.Visible)
            {
                foreach (WurmSeasonOutputItem item in WurmSeasonOutput)
                {
                    item.Update();
                    if (item.ShouldNotifyUser())
                    {
                        if (item.isItemTracked(TrackedSeasons))
                        {
                            if (SoundWarning)
                            {
                                TriggerSoundWarning();
                                item.UserNotified();
                            }
                            if (PopupWarning)
                            {
                                PopupQueue.Add(new KeyValuePair<string, DateTime>(item.GetSeasonName(), item.GetSeasonEndDate()));
                                popupScheduled = true;
                                item.UserNotified();
                            }
                        }
                    }
                }
                if (CalendarUI.Visible)
                {
                    WurmSeasonOutput.Sort();
                    CalendarUI.UpdateSeasonOutput(WurmSeasonOutput, UseWurmTimeForDisplay);
                }
                if (popupScheduled)
                {
                    string output = "";
                    foreach (var item in PopupQueue)
                    {
                        output += item.Key + " is now in season until " + item.Value.ToString("dd-MM-yyyy hh:mm") + "\n";
                    }
                    TriggerPopupWarning(output);
                    popupScheduled = false;
                    PopupQueue.Clear();
                }
            }
        }

        public void ChooseTrackedSeasons()
        {
            FormChooseSeasons seasonsDialog = new FormChooseSeasons(Enum.GetNames(typeof(WurmSeasonsEnum)), TrackedSeasons);
            seasonsDialog.ShowDialog();
            TrackedSeasons.Clear();
            foreach (var item in seasonsDialog.checkedListBox1.CheckedItems)
            {
                WurmSeasonsEnum parsedEnum;
                if (Enum.TryParse(item.ToString(), out parsedEnum))
                {
                    TrackedSeasons.Add(item.ToString(), parsedEnum);
                }
            }
            _TrackedSeasonListRetriever = TrackedSeasons;
            CalendarUI.UpdateTrackedSeasonsList(TrackedSeasons.Keys.ToArray<string>());
        }

        void TriggerSoundWarning()
        {
            SoundBank.PlaySound(this.SoundName);
        }

        void TriggerPopupWarning(string text)
        {
            WurmAssistant.ZeroRef.ScheduleCustomPopupNotify("Wurm Season Notify", text);
        }

        internal void OnEngineWakeUp()
        {
            foreach (WurmSeasonOutputItem item in WurmSeasonOutput)
            {
                item.ResetInSeasonFlag();
            }
        }
    }
}
