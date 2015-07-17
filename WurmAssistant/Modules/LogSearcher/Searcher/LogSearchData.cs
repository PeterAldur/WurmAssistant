using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WurmAssistant
{
    public enum LogSearchDataIDs
    {
        TimersMeditSkill,
        TimersMeditHistory,
        LogSearcherForceRecache,
        TimingAssistUptimeSearch,
        TimingAssistDateTimeSearch,
        TimersMeditPathAdvance,
        TimersFaithSkill,
        TimersPrayHistory,
        TimersSermonLookup,
        TimersAlignmentLookup,
        TimersLockpickingHistory,
        GrangerAHLookup
    }

    public class LogSearchData
    {
        public class SearchData
        {
            public string Player;
            public GameLogTypes GameLogType;
            public DateTime TimeFrom;
            public DateTime TimeTo;
            public string SearchKey;
            public SearchTypes SearchType;

            public string PM_Player = null;

            /// <summary>
            /// Builds new object holding all necessary data for searching
            /// </summary>
            /// <param name="player"></param>
            /// <param name="gamelogtype"></param>
            /// <param name="timefrom"></param>
            /// <param name="timeto"></param>
            /// <param name="searchkey">null or "" (empty string) to indicate this requires no match search</param>
            /// <param name="searchtype"></param>
            public SearchData(string player, GameLogTypes gamelogtype, DateTime timefrom, DateTime timeto, string searchkey, SearchTypes searchtype)
            {
                this.Player = player;
                this.GameLogType = gamelogtype;
                this.TimeFrom = timefrom;
                this.TimeTo = timeto;
                if (searchkey != null) this.SearchKey = searchkey;
                else this.SearchKey = "";
                this.SearchType = searchtype;
            }
        }

        public struct SingleSearchMatch
        {
            public long Begin;
            public long Length;
            public DateTime MatchDate;

            public SingleSearchMatch(long begin, long end, DateTime matchdate)
            {
                this.Begin = begin;
                this.Length = end;
                this.MatchDate = matchdate;
            }
        }

        public volatile bool StopSearching = false;

        public Control CallerControl;
        public LogSearchDataIDs CallbackID;

        public List<string> AllLines = new List<string>();
        //public List<long> AllLinesCharStartIndex = new List<long>();
        public string[] AllLinesArray = null;

        public List<SingleSearchMatch> SearchResults = new List<SingleSearchMatch>();
        public SearchData SearchCriteria;
        int Count = 0;
        bool Ready = false;

        public void BuildSearchData(string player, GameLogTypes gamelogtype, DateTime timefrom, DateTime timeto, string searchkey, SearchTypes searchtype)
        {
            SearchCriteria = new SearchData(player, gamelogtype, timefrom, timeto, searchkey, searchtype);
        }

        public void SetPM_Player(string player)
        {
            player = player.Trim();
            if (player != "") this.SearchCriteria.PM_Player = player;
            else this.SearchCriteria.PM_Player = null;
        }

        public void AddResult(int begin, int end, DateTime matchdate)
        {
            Count++;
            SearchResults.Add(new SingleSearchMatch(begin, end, matchdate));
        }

        public void Finish()
        {
            Ready = true;
        }

        public bool IsReady()
        {
            return Ready;
        }
    }
}
