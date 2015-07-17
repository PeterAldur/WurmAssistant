using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace WurmAssistant.Granger
{
    public class ModuleGranger : Module
    {
        string playerName = "";
        public string PlayerName
        {
            get { return playerName; }
        }

        SQLiteDB db;
        public TraitValues Values;
        public FormHorseManager GrangerUI;
        public HerdList AllHerds;

        bool AHLookupCompleted = false;

        DateTime lastBreedingOn;
        Horse lastBreedingFemale;
        Horse lastBreedingMale;

        bool _moduleEnabled = true;
        public bool ModuleEnabled
        {
            get { return _moduleEnabled; }
            set
            {
                _moduleEnabled = value;
                ModuleSettings.Set("ModuleEnabled", value);
            }
        }

        DateTime _AHSkillCheckedOn = new DateTime(0);
        public DateTime AHSkillCheckedOn
        {
            get { return _AHSkillCheckedOn; }
            set
            {
                _AHSkillCheckedOn = value;
                ModuleSettings.Set("AHSkillCheckedOn", value);
            }
        }

        int _AHSkill = 0;
        public int AHSkill
        {
            get { return _AHSkill; }
            set
            {
                if (value > 100)
                {
                    Logger.WriteLine("! Granger: Animal husbandry skill was adjusted from " + value + " to 100");
                    value = 100;
                }
                if (value < 0)
                {
                    Logger.WriteLine("! Granger: Animal husbandry skill was adjusted from " + value + " to 0");
                    value = 0;
                }
                _AHSkill = value;
                ModuleSettings.Set("AHSkill" + playerName, value);
            }
        }

        bool _liveAdvisor = false;
        public bool LiveAdvisor
        {
            get { return _liveAdvisor; }
            set
            {
                _liveAdvisor = value;
                ModuleSettings.Set("LiveAdvisor", value);
            }
        }

        bool _adviseOnlyAvailable = false;
        public bool AdviseOnlyAvailable
        {
            get { return _adviseOnlyAvailable; }
            set
            {
                _adviseOnlyAvailable = value;
                ModuleSettings.Set("AdviseOnlyAvailable", value);
            }
        }

        bool _includePotentialValue = false;
        public bool IncludePotentialValue
        {
            get { return _includePotentialValue; }
            set
            {
                _includePotentialValue = value;
                ModuleSettings.Set("IncludePotentialValue", value);
            }
        }

        bool _preferMissingTraits = false;
        public bool PreferMissingTraits
        {
            get { return _preferMissingTraits; }
            set
            {
                _preferMissingTraits = value;
                ModuleSettings.Set("PreferMissingTraits", value);
            }
        }

        bool _excludeNegatives = false;
        public bool ExcludeNegatives
        {
            get { return _excludeNegatives; }
            set
            {
                _excludeNegatives = value;
                ModuleSettings.Set("ExcludeNegatives", value);
            }
        }

        bool _dontExcludeInbreeding = false;
        public bool DontExcludeInbreeding
        {
            get { return _dontExcludeInbreeding; }
            set
            {
                _dontExcludeInbreeding = value;
                ModuleSettings.Set("DontExcludeInbreeding", value);
            }
        }

        bool _disableAdvisor = false;
        public bool DisableAdvisor
        {
            get { return _disableAdvisor; }
            set
            {
                _disableAdvisor = value;
                ModuleSettings.Set("DisableAdvisor", value);
            }
        }

        bool _epicCurveOverride = false;
        public bool EpicCurveOverride
        {
            get { return _epicCurveOverride; }
            set
            {
                _epicCurveOverride = value;
                ModuleSettings.Set("EpicCurveOverride_"+playerName, value);
            }
        }

        void InitSettings()
        {
            _moduleEnabled = ModuleSettings.Get("ModuleEnabled", _moduleEnabled);
            _AHSkillCheckedOn = ModuleSettings.Get("AHSkillCheckedOn" + playerName, _AHSkillCheckedOn);
            _AHSkill = ModuleSettings.Get("AHSkill" + playerName, _AHSkill);
            _liveAdvisor = ModuleSettings.Get("LiveAdvisor", _liveAdvisor);
            _adviseOnlyAvailable = ModuleSettings.Get("AdviseOnlyAvailable", _adviseOnlyAvailable);
            _includePotentialValue = ModuleSettings.Get("IncludePotentialValue", _includePotentialValue);
            _preferMissingTraits = ModuleSettings.Get("PreferMissingTraits", _preferMissingTraits);
            _excludeNegatives = ModuleSettings.Get("ExcludeNegatives", _excludeNegatives);
            _dontExcludeInbreeding = ModuleSettings.Get("DontExcludeInbreeding", _dontExcludeInbreeding);
            _disableAdvisor = ModuleSettings.Get("DisableAdvisor", _disableAdvisor);
            _epicCurveOverride = ModuleSettings.Get("EpicCurveOverride_" + playerName, _epicCurveOverride);
        }

        public ModuleGranger(string playerName)
            : base("Granger")
        {
            HorseProcessor.Parent = this;
            this.playerName = playerName;

            GrangerDebug.Log("GRANGERINIT: initSettings");
            InitSettings();
            GrangerDebug.Log("GRANGERINIT: initDB");
            db = WurmAssistant.Database;

            GrangerDebug.Log("GRANGERINIT: initTraitvalues");
            Values = new TraitValues(db);
            GrangerDebug.Log("GRANGERINIT: initAllherds");
            AllHerds = new HerdList(ModuleSettings, db, this);

            GrangerDebug.Log("GRANGERINIT: initGrangerui");
            GrangerUI = new FormHorseManager(this);
            GrangerDebug.Log("GRANGERINIT: updateGrangerui");
            GrangerUI.UpdateContents();

            GrangerDebug.Log("GRANGERINIT: beginAHskillLookup");
            BeginAHSkillLookup();
        }

        internal void ToggleUI()
        {
            if (GrangerUI.Visible)
            {
                GrangerUI.Hide();
            }
            else
            {
                GrangerUI.Show();
                GrangerUI.RestoreFromMin();
            }
        }

        public override void OnAppClosing()
        {
            base.OnAppClosing();
            AllHerds.Save(db);
            Values.Save(db);
        }

        public override void HandleNewLogEvents(List<string> newLogEvents, GameLogState log)
        {
            if (ModuleEnabled)
            {
                if (log.GetLogType() == GameLogTypes.Event)
                {
                    foreach (var line in newLogEvents)
                    {
                        // handle adding and updating data about horses
                        HorseProcessor.HandleLogEvent(line);
                        //handle other events
                        //[03:10:29] You bury the corpse of venerable tammyrain.
                        if (line.StartsWith("You bury the corpse", StringComparison.Ordinal))
                        {
                            GrangerDebug.Log("LIVETRACKER: applying maybedead flag due to: " + line);
                            Match match = Regex.Match(line, @"You bury the corpse of .+ (\w+)");
                            if (match.Success)
                            {
                                string lowecasename = match.Groups[1].Value;
                                Horse horse;
                                if (AllHerds.TryFindHorse(lowecasename, out horse, fixCase: true))
                                {
                                    horse.maybeDied = true;
                                }
                            }
                        }
                        //[11:34:44] You have now tended to Aged fat Lightningzoe and she seems pleased.
                        if (line.StartsWith("You have now tended", StringComparison.Ordinal))
                        {
                            GrangerDebug.Log("LIVETRACKER: applying groomed flag due to: " + line);
                            Match match = Regex.Match(line, @"You have now tended to (.+) and \w+ seems pleased");
                            if (match.Success)
                            {
                                string prefixedName = match.Groups[1].Value;
                                string fixedName = GrangerHelpers.RemoveAllPrefixes(prefixedName);
                                Horse horse;
                                if (AllHerds.TryFindHorse(fixedName, out horse))
                                {
                                    horse.GroomedOn = DateTime.Now;
                                }
                            }
                        }
                        //[04:23:27] The Aged fat Dancedog and the Aged fat Cliffdog get intimate.
                        if (line.Contains("get intimate"))
                        {
                            GrangerDebug.Log("LIVETRACKER: caching last breeded pair data due to: " + line);
                            Match match = Regex.Match(line, @"The (.+) and the (.+) get intimate.");
                            if (match.Success)
                            {
                                lastBreedingFemale = null;
                                lastBreedingMale = null;
                                lastBreedingOn = DateTime.Now;

                                string horse1 = match.Groups[1].Value;
                                horse1 = GrangerHelpers.RemoveAllPrefixes(horse1);
                                Horse horse1obj;
                                if (AllHerds.TryFindHorse(horse1, out horse1obj))
                                {
                                    if (horse1obj.IsMale) lastBreedingMale = horse1obj;
                                    else lastBreedingFemale = horse1obj;
                                }

                                string horse2 = match.Groups[2].Value;
                                horse2 = GrangerHelpers.RemoveAllPrefixes(horse2);
                                Horse horse2obj;
                                if (AllHerds.TryFindHorse(horse2, out horse2obj))
                                {
                                    if (horse2obj.IsMale) lastBreedingMale = horse2obj;
                                    else lastBreedingFemale = horse2obj;
                                }
                            }
                        }
                        //[04:23:47] The Aged fat Dancedog will probably give birth in a while!
                        if (line.Contains("will probably give birth"))
                        {
                            if (lastBreedingOn > DateTime.Now - TimeSpan.FromMinutes(3) && lastBreedingFemale != null)
                            {
                                GrangerDebug.Log("LIVETRACKER: applying maybepregnant flag due to: " + line);
                                Match match = Regex.Match(line, @"The (.+) will probably give birth in a while");
                                if (match.Success)
                                {
                                    string prefixedName = match.Groups[1].Value;
                                    string fixedName = GrangerHelpers.RemoveAllPrefixes(prefixedName);
                                    Horse horse;
                                    if (AllHerds.TryFindHorse(fixedName, out horse))
                                    {
                                        if (horse.Name == lastBreedingFemale.Name)
                                        {
                                            //horse.maybePregnant = true;
                                            horse.PregnantTo = DateTime.Now + TimeSpan.FromHours(21);
                                            if (lastBreedingMale != null) lastBreedingMale.NotInMood = DateTime.Now;
                                        }
                                    }
                                }
                            }
                        }
                        //[06:18:19] The Aged fat Umasad shys away and interrupts the action.
                        if (line.Contains("shys away and interrupts"))
                        {
                            if (lastBreedingOn > DateTime.Now - TimeSpan.FromMinutes(1))
                            {
                                GrangerDebug.Log("LIVETRACKER: applyling not in mood due to: " + line);
                                Match match = Regex.Match(line, @"The (.+) shys away and interrupts the action");
                                if (match.Success)
                                {
                                    string prefixedName = match.Groups[1].Value;
                                    string fixedName = GrangerHelpers.RemoveAllPrefixes(prefixedName);
                                    Horse horse;
                                    if (AllHerds.TryFindHorse(fixedName, out horse))
                                    {
                                        if (horse.IsMale)
                                        {
                                            if (lastBreedingMale != null) lastBreedingMale.NotInMood = DateTime.Now;
                                            if (lastBreedingFemale != null) lastBreedingFemale.NotInMood = DateTime.Now;
                                        }
                                        else
                                        {
                                            if (lastBreedingMale != null) lastBreedingMale.NotInMood = DateTime.Now;
                                            if (lastBreedingFemale != null) lastBreedingFemale.NotInMood = DateTime.Now;
                                        }
                                    }
                                }
                            }
                        }
                        //[16:51:07] You now care specially for Venerable fat goblin, to ensure longevity. You may care for 0 more creatures.

                        //update AH skill
                        if (line.StartsWith("Animal husbandry increased", StringComparison.Ordinal))
                        {
                            AHSkill = (int)GeneralHelper.ExtractSkillLEVELFromLine(line);
                        }
                        else if (line.StartsWith("Animal husbandry decreased", StringComparison.Ordinal))
                        {
                            AHSkill = (int)GeneralHelper.ExtractSkillLEVELFromLine(line);
                        }
                    }
                }
            }

            if (log.GetLogType() == GameLogTypes.Skills)
            {
                foreach (var line in newLogEvents)
                {
                    if (line.StartsWith("Animal husbandry increased", StringComparison.Ordinal))
                    {
                        AHSkill = (int)GeneralHelper.ExtractSkillLEVELFromLine(line);
                        GrangerUI.UpdateAHSkillAndEnableEdit();
                    }
                    else if (line.StartsWith("Animal husbandry decreased", StringComparison.Ordinal))
                    {
                        AHSkill = (int)GeneralHelper.ExtractSkillLEVELFromLine(line);
                        GrangerUI.UpdateAHSkillAndEnableEdit();
                    }
                }
            }
        }

        public override void OnPollingTick(bool engineInSleepMode)
        {
            HorseProcessor.Update();
        }

        public void NotifyUser(string message, int delay = 3000)
        {
            WurmAssistant.ZeroRef.ScheduleCustomPopupNotify("Granger", message, delay);
        }

        public void BeginAHSkillLookup()
        {
            if (ModuleLogSearcher.LogSearchMan != null)
            {
                DateTime searchSince = AHSkillCheckedOn;
                if (AHSkillCheckedOn < DateTime.Now - TimeSpan.FromDays(90)) AHSkillCheckedOn = DateTime.Now - TimeSpan.FromDays(90);
                else if (AHSkillCheckedOn > DateTime.Now - TimeSpan.FromDays(2)) AHSkillCheckedOn = DateTime.Now - TimeSpan.FromDays(2);
                LogSearchData logsearchdata = new LogSearchData();
                logsearchdata.BuildSearchData(
                    playerName,
                    GameLogTypes.Skills,
                    searchSince,
                    DateTime.Now,
                    "",
                    SearchTypes.RegexEscapedCaseIns);
                logsearchdata.CallerControl = this.GrangerUI;
                logsearchdata.CallbackID = LogSearchDataIDs.GrangerAHLookup;
                ModuleLogSearcher.LogSearchMan.EnqueueSearch(logsearchdata);
            }
        }

        public void EndAHSkillLookup(LogSearchData logsearchdata)
        {
            int ahskill = 0;
            foreach (string line in logsearchdata.AllLines)
            {
                if (line.Contains("Animal husbandry increased"))
                {
                    ahskill = (int)GeneralHelper.ExtractSkillLEVELFromLine(line);
                }
                else if (line.Contains("Animal husbandry decreased"))
                {
                    ahskill = (int)GeneralHelper.ExtractSkillLEVELFromLine(line);
                }
            }
            AHSkill = ahskill;
            AHLookupCompleted = true;
            string log_message = "Granger: found out " + playerName + " AH skill to be: " + AHSkill;
            Logger.WriteLine(log_message);
            GrangerDebug.Log(log_message);
            GrangerUI.UpdateAHSkillAndEnableEdit();
        }

        internal void HandleSearchCallback(LogSearchData logsearchdata)
        {
            if (logsearchdata.CallbackID == LogSearchDataIDs.GrangerAHLookup)
                EndAHSkillLookup(logsearchdata);
        }

        static class HorseProcessor
        {
            public static ModuleGranger Parent = null;

            static TimeSpan ProcessorTimeout = new TimeSpan(0, 0, 5);
            static bool isProcessing = false;
            static DateTime startedProcessingOn;
            static Horse horse;
            static ProcessorVerifyList verifyList;

            struct ProcessorVerifyList
            {
                public bool Name;
                public bool Parents;
                public bool Traits;
                public bool Gender;
                public bool CaredBy;
                public bool Pregnant;

                public bool IsValid
                {
                    get { return (Name && (Gender || Parents || Traits || CaredBy)); }
                }
            }

            //TODO everything but start process
            public static void HandleLogEvent(string line)
            {
                //attempt to start building new horse data
                if (line.StartsWith("You smile at", StringComparison.Ordinal))
                {
                    GrangerDebug.Log("smile cond: " + line);
                    HorseProcessor.AttemptToStartProcessing(line);
                }
                // append/update incoming data to current horse in buffer
                if (isProcessing)
                {
                    //[20:23:18] It has fleeter movement than normal. It has a strong body. It has lightning movement. It can carry more than average. It seems overly aggressive. 
                    if (!verifyList.Traits && HorseTraitEX.CanThisBeTraitLine(line))
                    {
                        GrangerDebug.Log("found maybe trait line: " + line);
                        foreach (string traitName in HorseTraitEX.NameToEnumMap.Keys)
                        {
                            if (line.Contains(traitName))
                            {
                                GrangerDebug.Log("line verified as traits");
                                horse.SetTraitsFromLogLine(line);
                                verifyList.Traits = true;
                                GrangerDebug.Log("trait parsing finished");
                                break;
                            }
                        }
                    }
                    //[20:23:18] She is very strong and has a good reserve of fat.
                    if (line.StartsWith("He", StringComparison.Ordinal) && !verifyList.Gender)
                    {
                        horse.IsMale = true;
                        verifyList.Gender = true;
                        GrangerDebug.Log("horse set to male");
                    }
                    if (line.StartsWith("She", StringComparison.Ordinal) && !verifyList.Gender)
                    {
                        horse.IsMale = false;
                        verifyList.Gender = true;
                        GrangerDebug.Log("horse set to female");
                    }
                    //[01:05:57] Mother is Venerable fat Starkdance. Father is Venerable fat Jollypie. 
                    if ((line.Contains("Mother is") || line.Contains("Father is")) && !verifyList.Parents)
                    {
                        GrangerDebug.Log("found maybe parents line");
                        Match match = Regex.Match(line, @"Mother is (?<g>\w+ \w+ \w+)|Mother is (?<g>\w+ \w+)");
                        if (match.Success)
                        {
                            string mother = match.Groups["g"].Value;
                            mother = ExtractHorseName(mother);
                            horse.Mother = mother;
                            GrangerDebug.Log("set mother to: " + mother);
                        }
                        Match match2 = Regex.Match(line, @"Father is (?<g>\w+ \w+ \w+)|Father is (?<g>\w+ \w+)");
                        if (match2.Success)
                        {
                            string father = match2.Groups["g"].Value;
                            father = ExtractHorseName(father);
                            horse.Father = father;
                            GrangerDebug.Log("set father to: " + father);
                        }
                        verifyList.Parents = true;
                        GrangerDebug.Log("finished parsing parents line");
                    }
                    //[20:23:18] It is being taken care of by Darkprincevale.
                    if (line.Contains("It is being taken care") && !verifyList.CaredBy)
                    {
                        GrangerDebug.Log("found maybe take care of line");
                        Match caredby = Regex.Match(line, @"care of by (\w+)");
                        if (caredby.Success)
                        {
                            horse.TakenCareOfBy = caredby.Groups[1].Value;
                            GrangerDebug.Log("cared set to: " + horse.TakenCareOfBy);
                        }
                        verifyList.CaredBy = true;
                        GrangerDebug.Log("finished parsing care line");
                    }
                    //[17:11:42] She will deliver in about 4.
                    if (line.Contains("She will deliver in") && !verifyList.Pregnant)
                    {
                        GrangerDebug.Log("found maybe prengant line");
                        Match match = Regex.Match(line, @"She will deliver in about (\d+)");
                        if (match.Success)
                        {
                            float length = float.Parse(match.Groups[1].Value) + 1F;
                            horse.PregnantTo = DateTime.Now + TimeSpan.FromHours(length * 21F);
                            horse.maybePregnant = false;
                            GrangerDebug.Log("found horse to be pregnant, est delivery: " + horse.PregnantTo.ToString());
                        }
                        verifyList.Pregnant = true;
                        GrangerDebug.Log("finished parsing pregnant line");
                    }
                }
            }

            static void VerifyAndApplyProcessing()
            {
                if (horse != null)
                {
                    GrangerDebug.Log("finishing processing horse: " + horse.Name);
                    //verify if enough fields are filled to warrant updating
                    if (verifyList.IsValid)
                    {
                        GrangerDebug.Log("horse data is valid");
                        //forward update to the herds
                        if (Parent != null)
                        {
                            if (Parent.AllHerds.HandleNewHorse(horse))
                            {
                                Parent.GrangerUI.UpdateContents();
                                GrangerDebug.Log("updated horse data: " + horse.Name);
                            }
                        }
                    }
                    else GrangerDebug.Log("horse data was invalid");
                    //clear the buffer
                    horse = null;
                    GrangerDebug.Log("processor buffer cleared");
                }
            }

            static void AttemptToStartProcessing(string line)
            {
                GrangerDebug.Log("attempting to start processing horse due to line: " + line);
                //clean up if there is still non-timed out process
                VerifyAndApplyProcessing();

                //it is unknown if smiled at horse or something else
                //attempt to extract the name of game object
                try
                {
                    GrangerDebug.Log("extracting object name");
                    string objectname = line.Remove(0, 13).Replace(".", "");
                    //now verify this is at least living creature
                    if (!IsBlacklistedCreatureName(objectname) && HasAgeInName(objectname))
                    {
                        GrangerDebug.Log("object assumed to be a horse");
                        if (Parent.AHLookupCompleted)
                        {
                            GrangerDebug.Log("building new horse object and moving to processor");
                            isProcessing = true;
                            startedProcessingOn = DateTime.Now;
                            verifyList = new ProcessorVerifyList();
                            horse = new Horse();
                            horse.Name = ExtractHorseName(objectname);
                            horse.TraitsInspectedAtSkill = Parent.AHSkill;
                            verifyList.Name = true;
                            GrangerDebug.Log("finished building");
                        }
                        else
                        {
                            Parent.NotifyUser("Cannot gather data yet, please try again once Wurm Assistant fully loads.");
                            GrangerDebug.Log("processing horse canceled, still waiting for AH skill lookup to finish");
                        }
                    }
                    else GrangerDebug.Log("object is not a horse");
                }
                catch (Exception _e)
                {
                    //this shouldn't happen, there is always something player is smiling at, unless error happened elsewhere
                    Logger.WriteLine("! Granger: error while BeginProcessing, event: " + line);
                    GrangerDebug.Log("! Granger: error while BeginProcessing, event: " + line);
                    Logger.LogException(_e);
                }
            }

            static bool IsBlacklistedCreatureName(string objectname)
            {
                foreach (string name in GrangerHelpers.WildCreatureNames)
                {
                    if (objectname.Contains(name)) return true;
                }
                return false;
            }

            static bool HasAgeInName(string objectname)
            {
                foreach (string age in GrangerHelpers.HorseAges)
                {
                    if (objectname.Contains(age)) return true;
                }
                return false;
            }

            static string ExtractHorseName(string objectname)
            {
                return GrangerHelpers.RemoveAllPrefixes(objectname);
            }

            public static void Update()
            {
                if (isProcessing)
                {
                    if (DateTime.Now > startedProcessingOn + ProcessorTimeout)
                    {
                        GrangerDebug.Log("processing timed out, attempting to verify and apply last inspected horse");
                        isProcessing = false;
                        VerifyAndApplyProcessing();
                    }
                }
            }
        }

        /// <summary>
        /// note: this drops all tables, the module will still attempt to save the data, 
        ///  but this subsequently fails without error, when tables are not found in database;
        ///  new tables will be created upon next program (more specifically: module) launch;
        ///  this method REQUIRES full module rebuild, or the easy way: Application.Exit()
        /// </summary>
        internal void WipeOutEverything()
        {
            TraitValues.WipeDB(db);
            AllHerds.WipeDB(db);
            ModuleSettings.Clear();
            //clear any orphaned tables
            DataTable datatable = db.GetAllTables();
            List<string> tablesToDrop = new List<string>();
            foreach (DataRow row in datatable.Rows)
            {
                if (row[0].ToString().Contains("Granger"))
                {
                    tablesToDrop.Add(row[0].ToString());
                }
            }
            foreach (var table in tablesToDrop)
            {
                db.DropTable(table);
            }
        }
    }
}
