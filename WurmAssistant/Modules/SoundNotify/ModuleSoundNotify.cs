using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Media;
using System.Diagnostics;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

namespace WurmAssistant
{
    public class PlaylistEntry
    {
        /// <summary>
        /// Name of the sound (file name without extension)
        /// </summary>
        public string SoundName;
        /// <summary>
        /// Soundplayer instance used to cache the sound file
        /// </summary>
        public SB_SoundPlayer Soundplayer;
        /// <summary>
        /// Condition that triggers this sound
        /// </summary>
        public string Condition;
        /// <summary>
        /// List of all special settings
        /// </summary>
        public HashSet<string> SpecialSettings = new HashSet<string>();
        public bool isActive = true;
        public bool isCustomRegex = false;
    }

    public class PlaylistEntryCacheable
    {
        public SB_SoundPlayer Soundplayer;
        public string Condition;
        public string SoundName;
        public bool isActive = true;

        public PlaylistEntryCacheable(SB_SoundPlayer player, string cond, string SndName, bool Active = true)
        {
            this.Soundplayer = player;
            this.Condition = cond;
            this.SoundName = SndName;
            this.isActive = Active;
        }
    }

    /// <summary>
    /// Arrays used to help parsing log messages
    /// </summary>
    static public class LogQueueParseHelper
    {
        // queue sound parsing helper arrays
        static public string[] ActionStart = { "You start", "You continue to" };
        static public string[] ActionFalstart = { "You start dragging", "You start leading" };
        static public string[] QueueAdd_contains_and = { "After", "you will start" };  //unused
        static public string[] ActionEnd =   { "You improve", 
                                              "You continue on",
                                              "You nail",
                                              "You dig",
                                              "You attach",
                                              "You repair", 
                                              "You fail", 
                                              "You stop",
                                              "You mine some", 
                                              "You damage",
                                              "You harvest",
                                              "You plant", 
                                              "You pull", 
                                              "You push", 
                                              "You almost made it", 
                                              "You create", 
                                              "You will want to",
                                              "You need to temper",
                                              "You finish",
                                              "Roof completed",
                                              "Metal needs to be glowing hot while smithing",
                                              "You continue to build on a floor",
                                              "You have now tended",
                                              "You sow",
                                              "You turn",
                                              "You realize you harvested",
                                              "The ground is cultivated",
                                              "You gather",
                                              "You cut down",
                                              "You cut a sprout",
                                              "You must use a",
                                              "You notice some notches",
                                              "The dirt is packed",
                                              "You lay the foundation",
                                              "You bandage",
                                              "You prune", 
                                              "You make a lot of errors", 
                                              "You try to bandage",
                                              "Sadly, the sprout does not",
                                              "You chip away",
                                              "The last parts of the",
                                              "The ground is paved",
                                              "The ground is no longer paved",
                                              "You hit rock",
                                              "You must rest",
                                              "You use some of the dirt in one corner",
                                              "You proudly close the gift",
                                              "You add a stone and some mortar"};
        static public string[] ActionEnd_contains = {   "has some irregularities", 
                                                        "has some dents", //bug: fires for examining lamp //fixed via ActionFalsEndDueToLastAction
                                                        "needs to be sharpened",
                                                        "is finished",
                                                        "will probably give birth",
                                                        "shys away and interrupts",
                                                        "falls apart with a crash"};
        //"You must use a",
        //"You notice some notches"};
        static public string[] ActionFalsEnd = { "You stop dragging", 
                                                   "A forge made from", 
                                                   "It is made from", 
                                                   "A small, very rudimentary", 
                                                   "You stop leading", 
                                                   "You fail to produce", 
                                                   "A tool for",
                                                   "The roof is finished already",
                                                   "A high guard tower",
                                                   "You create a box side",
                                                   "You create another box side",
                                                   "You create yet another box side",
                                                   "You create the last box side",
                                                   "You create a bottom",
                                                   "You create a top"};
        static public string[] ActionFalsEndDueToLastAction = { "A decorative lamp", "A high guard tower" };
        static public string[] QueueReset = { "You ride", "You mount" };   //unused
    }

    /// <summary>
    /// Main Sound Notify module class
    /// </summary>
    public class ModuleSoundNotify : Module
    {
        string PlayerName;

        FormSoundNotifyConfig SoundManagerUI;
        List<PlaylistEntry> Playlist = new List<PlaylistEntry>();

        // playlists cached per log type
        List<PlaylistEntryCacheable> EventPlaylist = new List<PlaylistEntryCacheable>();
        List<PlaylistEntryCacheable> CombatPlaylist = new List<PlaylistEntryCacheable>();
        List<PlaylistEntryCacheable> AlliancePlaylist = new List<PlaylistEntryCacheable>();
        List<PlaylistEntryCacheable> CA_HELPPlaylist = new List<PlaylistEntryCacheable>();
        List<PlaylistEntryCacheable> FreedomPlaylist = new List<PlaylistEntryCacheable>();
        List<PlaylistEntryCacheable> FriendsPlaylist = new List<PlaylistEntryCacheable>();
        List<PlaylistEntryCacheable> GLFreedomPlaylist = new List<PlaylistEntryCacheable>();
        List<PlaylistEntryCacheable> LocalPlaylist = new List<PlaylistEntryCacheable>();
        List<PlaylistEntryCacheable> MGMTPlaylist = new List<PlaylistEntryCacheable>();
        List<PlaylistEntryCacheable> SkillsPlaylist = new List<PlaylistEntryCacheable>();
        List<PlaylistEntryCacheable> TeamPlaylist = new List<PlaylistEntryCacheable>();
        List<PlaylistEntryCacheable> VillagePlaylist = new List<PlaylistEntryCacheable>();
        List<PlaylistEntryCacheable> PMPlaylist = new List<PlaylistEntryCacheable>();

        // last queue ending action
        DateTime lastActionFinished = DateTime.Now;
        // last queue starting action
        DateTime lastActionStarted = DateTime.Now;

        // whether queue sound is scheduled to play on next update
        bool scheduledQueueSound = false;
        // wurm log line that triggered queue sound
        string LogEntryThatTriggeredLastQueueSound;

        // used if no custom sound set
        static SB_SoundPlayer defQueueSoundPlayer;
        bool defQueueSoundPlayerEnabled = true;

        // previous processed line
        string lastline;

        // is the entire module muted?
        public bool ModuleMuted = false;

        #region MODULE SETTINGS

        // default delay (seconds) for queue sound after event triggers it
        double _queueDefDelay = 1.0D;
        public double QueueDefDelay
        {
            get { return _queueDefDelay; }
            set
            {
                _queueDefDelay = value;
                ModuleSettings.Set("QueueDelay_" + PlayerName, value);
            }
        }

        // if queue sound should be played at all
        bool _queueSoundEnabled = false;
        public bool QueueSoundEnabled
        {
            get { return _queueSoundEnabled; }
            set
            {
                _queueSoundEnabled = value;
                ModuleSettings.Set("QueueSound_" + PlayerName, value);
            }
        }

        // if queue sound not set, notify user once
        //deprec
        bool _userNotifiedDefaultQueueSound = false;
        public bool UserNotifiedDefaultQueueSound
        {
            get { return _userNotifiedDefaultQueueSound; }
            set
            {
                _userNotifiedDefaultQueueSound = value;
                ModuleSettings.Set("QSoundNotified", value);
            }
        }

        string _queueSoundName = "__none__"; //default cannot be empty string or null, due to some error in settings
        public string QueueSoundName
        {
            get
            {
                if (_queueSoundName == "__none__") return null;
                else return _queueSoundName;
            }
            set
            {
                if (value == null) _queueSoundName = "__none__";
                else _queueSoundName = value;
                ModuleSettings.Set("QueueSoundName_" + PlayerName, _queueSoundName);
            }
        }

        void InitSettings()
        {
            QueueDefDelay = ModuleSettings.Get("QueueDelay_" + PlayerName, _queueDefDelay);
            QueueSoundEnabled = ModuleSettings.Get("QueueSound_" + PlayerName, _queueSoundEnabled);
            UserNotifiedDefaultQueueSound = ModuleSettings.Get("QSoundNotified", _userNotifiedDefaultQueueSound);
            _queueSoundName = ModuleSettings.Get("QueueSoundName_" + PlayerName, _queueSoundName);
        }

        #endregion

        /// <summary>
        /// Specifies name of the module, serves as directory name
        /// </summary>
        public ModuleSoundNotify(string playername)
            : base("SoundNotify_"+playername)
        {
            this.PlayerName = playername;
            // init settings
            InitSettings();

            PlaylistClumsyDB = new TextFileObject(ModuleFolderPath + PlayerName + "_playlist.txt", true, false, true, false, false, false);
            LoadPlaylist();

            InitUI();

            try
            {
                defQueueSoundPlayer = new SB_SoundPlayer(WurmAssistant.DefaultDir + "defQueueSound.wav");
                defQueueSoundPlayer.Load();
            }
            catch (Exception)
            {
                Logger.WriteLine(ModuleName + " could not load default queue sound");
                Logger.WriteLine("Attempted at location: " + WurmAssistant.DefaultDir + "defQueueSound.wav");
                defQueueSoundPlayerEnabled = false;
            }

        }

        void InitUI()
        {
            SoundManagerUI = new FormSoundNotifyConfig(this);
        }

        public void ToggleUI(object sender, EventArgs e)
        {
            ToggleUI();
        }

        public void ToggleUI()
        {
            if (SoundManagerUI.Visible) SoundManagerUI.RestoreFromMin(); //SoundManagerUI.Close();
            else { InitUI(); SoundManagerUI.Show(); }
        }

        #region PLAYLIST

        // save playlist to text file
        TextFileObject PlaylistClumsyDB;

        public readonly Char[] DefDelimiter = new Char[] { ';' };

        void LoadPlaylist()
        {
            //check file version
            bool queueSoundFixNeeded = false;
            string version = PlaylistClumsyDB.ReadNextLine();

            int activePos;
            int soundnamePos;
            int conditionPos;
            int specialPos;
            // assign index positions for data in file in respect to version
            if (version == "FILEVERSION 3")
            {
                activePos = 0;
                soundnamePos = 1;
                conditionPos = 2;
                specialPos = 3;
            }
            else if (version == "FILEVERSION 2")
            {
                activePos = 0;
                soundnamePos = 1;
                conditionPos = 2;
                specialPos = 3;
                queueSoundFixNeeded = true;
            }
            else //old versionless playlist
            {
                PlaylistClumsyDB.resetReadPos(); //it has no version line
                activePos = -1; //def indicator that it doesnt exist in this version
                soundnamePos = 0;
                conditionPos = 1;
                specialPos = 2;
            }

            string line = PlaylistClumsyDB.ReadNextLine();
            while (line != null)
            {
                PlaylistEntry playlistentry = new PlaylistEntry();

                //parse line entries
                string[] entries = line.Split(DefDelimiter);
                if (activePos != -1)
                {
                    if (Convert.ToBoolean(entries[activePos]) == false) playlistentry.isActive = false;
                    else playlistentry.isActive = true;
                }
                playlistentry.SoundName = entries[soundnamePos];
                playlistentry.Soundplayer = SoundBank.getSoundPlayer(playlistentry.SoundName);
                playlistentry.Condition = entries[conditionPos];
                for (int i = specialPos; i < entries.Length; i++)
                {
                    playlistentry.SpecialSettings.Add(entries[i]);
                    if (entries[i].Contains("s:CustomRegex")) playlistentry.isCustomRegex = true;
                }
                Playlist.Add(playlistentry);
                line = PlaylistClumsyDB.ReadNextLine();
            }

            if (queueSoundFixNeeded) MoveQueueSound();
            CacheSpecializedPlaylists();
        }

        private void MoveQueueSound()
        {
            int plistEntryToRemove = -1;
            for (int i = 0; i < Playlist.Count; i++)
            {
                foreach (string specialcond in Playlist[i].SpecialSettings)
                {
                    if (specialcond == "s:queue sound")
                    {
                        QueueSoundName = Playlist[i].SoundName;
                        plistEntryToRemove = i;
                        //handle moving special sound to the new setting
                    };
                }
            }
            if (plistEntryToRemove > -1)
            {
                Playlist.RemoveAt(plistEntryToRemove);
            }
            SavePlaylist();
        }

        private void CacheSpecializedPlaylists()
        {
            EventPlaylist.Clear();
            CombatPlaylist.Clear();
            AlliancePlaylist.Clear();
            CA_HELPPlaylist.Clear();
            FreedomPlaylist.Clear();
            FriendsPlaylist.Clear();
            GLFreedomPlaylist.Clear();
            LocalPlaylist.Clear();
            MGMTPlaylist.Clear();
            SkillsPlaylist.Clear();
            TeamPlaylist.Clear();
            VillagePlaylist.Clear();
            PMPlaylist.Clear();

            foreach (PlaylistEntry entry in Playlist)
            {
                foreach (string cond in entry.SpecialSettings)
                {
                    if (cond == GameLogTypesEX.GetNameForLogType(GameLogTypes.Event))
                    {
                        EventPlaylist.Add(new PlaylistEntryCacheable(entry.Soundplayer, entry.Condition, entry.SoundName, entry.isActive));
                    }
                    if (cond == GameLogTypesEX.GetNameForLogType(GameLogTypes.Combat))
                    {
                        CombatPlaylist.Add(new PlaylistEntryCacheable(entry.Soundplayer, entry.Condition, entry.SoundName, entry.isActive));
                    }
                    if (cond == GameLogTypesEX.GetNameForLogType(GameLogTypes.Alliance))
                    {
                        AlliancePlaylist.Add(new PlaylistEntryCacheable(entry.Soundplayer, entry.Condition, entry.SoundName, entry.isActive));
                    }
                    if (cond == GameLogTypesEX.GetNameForLogType(GameLogTypes.CA_HELP))
                    {
                        CA_HELPPlaylist.Add(new PlaylistEntryCacheable(entry.Soundplayer, entry.Condition, entry.SoundName, entry.isActive));
                    }
                    if (cond == GameLogTypesEX.GetNameForLogType(GameLogTypes.Freedom))
                    {
                        FreedomPlaylist.Add(new PlaylistEntryCacheable(entry.Soundplayer, entry.Condition, entry.SoundName, entry.isActive));
                    }
                    if (cond == GameLogTypesEX.GetNameForLogType(GameLogTypes.Friends))
                    {
                        FriendsPlaylist.Add(new PlaylistEntryCacheable(entry.Soundplayer, entry.Condition, entry.SoundName, entry.isActive));
                    }
                    if (cond == GameLogTypesEX.GetNameForLogType(GameLogTypes.GLFreedom))
                    {
                        GLFreedomPlaylist.Add(new PlaylistEntryCacheable(entry.Soundplayer, entry.Condition, entry.SoundName, entry.isActive));
                    }
                    if (cond == GameLogTypesEX.GetNameForLogType(GameLogTypes.Local))
                    {
                        LocalPlaylist.Add(new PlaylistEntryCacheable(entry.Soundplayer, entry.Condition, entry.SoundName, entry.isActive));
                    }
                    if (cond == GameLogTypesEX.GetNameForLogType(GameLogTypes.MGMT))
                    {
                        MGMTPlaylist.Add(new PlaylistEntryCacheable(entry.Soundplayer, entry.Condition, entry.SoundName, entry.isActive));
                    }
                    if (cond == GameLogTypesEX.GetNameForLogType(GameLogTypes.Skills))
                    {
                        SkillsPlaylist.Add(new PlaylistEntryCacheable(entry.Soundplayer, entry.Condition, entry.SoundName, entry.isActive));
                    }
                    if (cond == GameLogTypesEX.GetNameForLogType(GameLogTypes.Team))
                    {
                        TeamPlaylist.Add(new PlaylistEntryCacheable(entry.Soundplayer, entry.Condition, entry.SoundName, entry.isActive));
                    }
                    if (cond == GameLogTypesEX.GetNameForLogType(GameLogTypes.Village))
                    {
                        VillagePlaylist.Add(new PlaylistEntryCacheable(entry.Soundplayer, entry.Condition, entry.SoundName, entry.isActive));
                    }
                    if (cond == GameLogTypesEX.GetNameForLogType(GameLogTypes.PM))
                    {
                        PMPlaylist.Add(new PlaylistEntryCacheable(entry.Soundplayer, entry.Condition, entry.SoundName, entry.isActive));
                    }
                }
            }
        }

        /// <summary>
        /// called from GUI Form
        /// </summary>
        /// <param name="soundname"></param>
        /// <param name="cond"></param>
        /// <param name="speccond"></param>
        /// <param name="insertIndex">needed only for inserting, def -1 for adding</param>
        public void AddPlaylistEntry(string soundname, string cond, List<string> speccond, bool active, int insertIndex = -1)
        {
            PlaylistEntry playlistentry = new PlaylistEntry();
            playlistentry.SoundName = soundname;
            playlistentry.Soundplayer = SoundBank.getSoundPlayer(playlistentry.SoundName);
            playlistentry.Condition = cond;
            if (speccond != null)
            {
                foreach (string _cond in speccond)
                {
                    playlistentry.SpecialSettings.Add(_cond);
                    if (_cond.Contains("s:CustomRegex")) playlistentry.isCustomRegex = true;
                }
            }
            if (insertIndex == -1) Playlist.Add(playlistentry);
            else Playlist.Insert(insertIndex, playlistentry);
            SavePlaylist();
        }

        // called from GUI Form
        public void RemovePlaylistEntry(int index)
        {
            Playlist.RemoveAt(index);
            SavePlaylist();
        }

        public List<PlaylistEntry> getPlaylist()
        {
            return Playlist;
        }

        public PlaylistEntry getPlaylistEntryAtIndex(int parIndex)
        {
            try { return Playlist[parIndex]; }
            catch { return null; }
        }

        public void TogglePlaylistEntryActive(int index)
        {
            if (Playlist[index].isActive) Playlist[index].isActive = false;
            else Playlist[index].isActive = true;
            SavePlaylist();
        }

        void SavePlaylist()
        {
            //backup old file format
            try
            {
                string PLversion = PlaylistClumsyDB.ReadLine(0);
                if (PLversion != null)
                {
                    if (!PLversion.StartsWith("FILEVERSION 3", StringComparison.Ordinal))
                    {
                        Logger.WriteLine("SoundNotify: detected old playlist format, attempting to backup before conversion");

                        try
                        {
                            PlaylistClumsyDB.BackupFile();
                            Logger.WriteLine("SoundNotify: backup successful");
                        }
                        catch (Exception _e)
                        {
                            Logger.WriteLine("SoundNotify: Exception while backing up, may have failed");
                            Logger.LogException(_e);
                        }
                    }
                }
            }
            catch (Exception _e)
            {
                Logger.WriteLine("! Uknown exception");
                Logger.LogException(_e);
                PlaylistClumsyDB.ClearFile();
            }

            PlaylistClumsyDB.ClearFile();

            //write this file version
            PlaylistClumsyDB.WriteLine("FILEVERSION 3");

            foreach (PlaylistEntry entry in Playlist)
            {
                string DBrecord = entry.isActive.ToString();
                DBrecord += DefDelimiter[0];
                DBrecord += entry.SoundName;
                DBrecord += DefDelimiter[0];
                DBrecord += entry.Condition;
                foreach (string specialcond in entry.SpecialSettings)
                {
                    DBrecord += DefDelimiter[0];
                    DBrecord += specialcond;
                }
                PlaylistClumsyDB.WriteLine(DBrecord);
            }
            //MoveQueueSound();
            CacheSpecializedPlaylists();
        }

        public string ConvertCondOutputToRegex(string cond)
        {
            cond = Regex.Escape(cond);
            cond = cond.Replace(@"\*", @".+");
            return cond;
        }

        public string ConvertRegexToCondOutput(string regexCond)
        {
            regexCond = regexCond.Replace(@".+", @"\*");
            regexCond = Regex.Unescape(regexCond);
            return regexCond;
        }

        #endregion

        public float GetSoundEngineVolume()
        {
            return SoundBank.GlobalVolume;
        }

        //button click in ui
        public void SetQueueSound()
        {
            FormChooseSound ChooseSoundUI = new FormChooseSound();
            if (ChooseSoundUI.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                QueueSoundName = ChooseSoundUI.ChosenSound;
                SoundManagerUI.UpdateSoundName(QueueSoundName);
            }
        }

        //get the name for ui
        public string GetQueueSoundForUI()
        {
            if (QueueSoundName != null) return QueueSoundName;
            else return "default";
        }

        #region MODULE ENGINE

        override public void HandleNewLogEvents(List<string> newLogEvents, GameLogState log)
        {
            List<PlaylistEntryCacheable> currentPlaylist;

            switch (log.GetLogType())
            {
                case GameLogTypes.Event:
                    currentPlaylist = EventPlaylist;
                    break;
                case GameLogTypes.Combat:
                    currentPlaylist = CombatPlaylist;
                    break;
                case GameLogTypes.Alliance:
                    currentPlaylist = AlliancePlaylist;
                    break;
                case GameLogTypes.CA_HELP:
                    currentPlaylist = CA_HELPPlaylist;
                    break;
                case GameLogTypes.Freedom:
                    currentPlaylist = FreedomPlaylist;
                    break;
                case GameLogTypes.Friends:
                    currentPlaylist = FriendsPlaylist;
                    break;
                case GameLogTypes.GLFreedom:
                    currentPlaylist = GLFreedomPlaylist;
                    break;
                case GameLogTypes.Local:
                    currentPlaylist = LocalPlaylist;
                    break;
                case GameLogTypes.MGMT:
                    currentPlaylist = MGMTPlaylist;
                    break;
                case GameLogTypes.Skills:
                    currentPlaylist = SkillsPlaylist;
                    break;
                case GameLogTypes.Team:
                    currentPlaylist = TeamPlaylist;
                    break;
                case GameLogTypes.Village:
                    currentPlaylist = VillagePlaylist;
                    break;
                case GameLogTypes.PM:
                    currentPlaylist = PMPlaylist;
                    break;
                default: throw new Exception("No cached playlist for this log type: " + log.GetLogType());
            }

            lastline = "";
            foreach (string line in newLogEvents)
            {
                if (lastline != line)
                {
                    Debug.WriteLine("> SoundNotify > HandleNewLogEvents > line processed");
                    // determine if queue sound should be played
                    if (QueueSoundEnabled && log.GetLogType() == GameLogTypes.Event)
                        handleQueueSound(line);

                    if (!ModuleMuted)
                    {
                        foreach (PlaylistEntryCacheable playlistentry in currentPlaylist)
                        {
                            if (playlistentry.isActive
                                && playlistentry.Soundplayer != null
                                && playlistentry.Condition != ""
                                && Regex.IsMatch(line, playlistentry.Condition, RegexOptions.IgnoreCase))
                            {
                                try
                                {
                                    playlistentry.Soundplayer.Play();
                                    Logger.WriteLine(ModuleName + " played sound: " + playlistentry.SoundName + " on event: " + line);
                                }
                                catch (FileNotFoundException _e)
                                {
                                    Logger.WriteLine(ModuleName + " could not play sound " + playlistentry.SoundName + ", reason: " + _e.Message);
                                }

                            }
                        }
                    }
                }
                lastline = line;
            }
        }

        private void handleQueueSound(string line)
        {
            bool _PlayerActionStarted = false;

            foreach (string cond in LogQueueParseHelper.ActionStart)
            {
                if (line.StartsWith(cond, StringComparison.Ordinal)) _PlayerActionStarted = true;
            }
            foreach (string cond in LogQueueParseHelper.ActionFalstart)
            {
                if (line.StartsWith(cond, StringComparison.Ordinal)) _PlayerActionStarted = false;
            }
            if (_PlayerActionStarted == true)
            {
                lastActionStarted = ModuleTimingAssist.currentTime;
            }

            bool _PlayerActionFinished = false;

            foreach (string cond in LogQueueParseHelper.ActionEnd)
            {
                if (line.StartsWith(cond, StringComparison.Ordinal)) _PlayerActionFinished = true;
            }
            foreach (string cond in LogQueueParseHelper.ActionEnd_contains)
            {
                if (line.Contains(cond)) _PlayerActionFinished = true;
            }
            foreach (string cond in LogQueueParseHelper.ActionFalsEnd)
            {
                if (line.StartsWith(cond, StringComparison.Ordinal)) _PlayerActionFinished = false;
            }
            foreach (string cond in LogQueueParseHelper.ActionFalsEndDueToLastAction)
            {
                if (lastline.StartsWith(cond, StringComparison.Ordinal)) _PlayerActionFinished = false;
            }
            if (_PlayerActionFinished == true)
            {
                LogEntryThatTriggeredLastQueueSound = line;
                lastActionFinished = ModuleTimingAssist.currentTime;
                // if action finished, older action started is no longer valid
                // and should not disable queuesound in next conditional
                lastActionStarted = lastActionStarted.AddSeconds(-QueueDefDelay); // datetime is not nullable
                scheduledQueueSound = true;
            }

            // disable scheduled queue sound if new action started before its played
            if (lastActionStarted.AddSeconds(QueueDefDelay) >= ModuleTimingAssist.currentTime)
            {
                scheduledQueueSound = false;
            }
        }

        override public void AfterHandleLogs(bool engineInSleepMode)
        {
            if (scheduledQueueSound
                && ModuleTimingAssist.currentTime >= lastActionFinished.AddSeconds(QueueDefDelay))
            {
                if (!ModuleMuted)
                {
                    if (QueueSoundName != null)
                    {
                        SoundBank.PlaySound(QueueSoundName);
                        Logger.WriteLine(ModuleName + " played queue sound due to event: " + LogEntryThatTriggeredLastQueueSound);
                    }
                    else
                    {
                        defQueueSoundPlayer.Play();
                        Logger.WriteLine(ModuleName + " played default queue sound due to event: " + LogEntryThatTriggeredLastQueueSound);
                    }
                }
                scheduledQueueSound = false;
            }
        }

        #endregion
    }
}
