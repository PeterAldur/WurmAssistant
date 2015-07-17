using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WurmAssistant
{
    /// <summary>
    /// Experimental, not intended to be used in published program, will be part of Granger module
    /// </summary>
    class ModuleHorseDump : Module
    {
        bool Active = false;

        TextFileObject dumpfile;
        int ListenCounter = 0;
        string CurrentHorse;
        string CurrentTraits;
        string CurrentGender;
        string CurrentMother = "none";
        string CurrentFather = "none";
        bool traitsNoted = false;
        bool genderNoted = false;
        bool ancestorsNoted = false;
        HashSet<string> LoggedHorseList = new HashSet<string>();

        public ModuleHorseDump()
            : base("HorseDump")
        {
            dumpfile = new TextFileObject(ModuleFolderPath + "dump.txt", false, false, true, false, false, false);
        }

        public override void BeforeHandleLogs(bool engineInSleepMode)
        {
            if (Active)
            {
                ListenCounter -= WurmAssistant.ZeroRef.TimerTickRate;
                if (ListenCounter <= 0)
                {
                    FlushData();
                    ListenCounter = 0;
                }
            }
        }

        public override void HandleNewLogEvents(List<string> newLogEvents, GameLogState log)
        {
            if (Active)
            {
                base.HandleNewLogEvents(newLogEvents, log);
                if (log.GetLogType() == GameLogTypes.Event)
                {
                    foreach (var smtg in newLogEvents)
                    {
                        if (smtg.Contains("You smile at"))
                        {
                            FlushData();

                            CurrentHorse = smtg.Remove(0, 13).Replace(".", "");
                            if (!LoggedHorseList.Contains(CurrentHorse))
                            {
                                ListenCounter = 10000;
                            }
                        }

                        if (ListenCounter > 0)
                        {
                            if (!traitsNoted)
                            {
                                if (smtg.Contains("It has"))
                                {
                                    traitsNoted = true;
                                    CurrentTraits = smtg;
                                }
                            }

                            if (!genderNoted)
                            {
                                if (smtg.Contains("He"))
                                {
                                    genderNoted = true;
                                    CurrentGender = "Male";
                                }
                                if (smtg.Contains("She"))
                                {
                                    genderNoted = true;
                                    CurrentGender = "Female";
                                }
                            }
                            if (!ancestorsNoted)
                            {
                                //[01:05:57] Mother is Venerable fat Starkdance. Father is Venerable fat Jollypie. 
                                if (smtg.Contains("Mother is") || smtg.Contains("Father is"))
                                {
                                    Match match = Regex.Match(smtg, @"Mother is .+\.");
                                    if (match.Success) 
                                        CurrentMother = match.Value.Substring(10, match.Value.Length - 10 - 1);

                                    Match match2 = Regex.Match(smtg, @"Father is .+\.");
                                    if (match2.Success) 
                                        CurrentFather = match2.Value.Substring(10, match2.Value.Length - 10 - 1);

                                    ancestorsNoted = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void FlushData()
        {
            if (!ancestorsNoted && traitsNoted && genderNoted)
            {
                SaveData();
            }
        }

        public override void AfterHandleLogs(bool engineInSleepMode)
        {
            if (Active)
            {
                if (traitsNoted && genderNoted && ancestorsNoted)
                {
                    SaveData();
                }
            }
        }

        private void SaveData()
        {
            LoggedHorseList.Add(CurrentHorse);
            dumpfile.WriteLine(
                CurrentHorse +
                " (" + CurrentGender + ") - " +
                " (Mother: " + CurrentMother + ") - " +
                " (Father: " + CurrentFather + ") - " +
                CurrentTraits);
            ListenCounter = 0;
            traitsNoted = false;
            genderNoted = false;
            ancestorsNoted = false;
            CurrentFather = "none";
            CurrentMother = "none";
        }
    }
}
