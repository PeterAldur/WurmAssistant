using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace WurmAssistant.Deprecated
{
    /// <summary>
    /// This class is deprecated
    /// </summary>
    class LogFileSearcher
    {
        List<string> FileBank = new List<string>();
        GameLogTypes LogType;

        public LogFileSearcher(string directoryPath, GameLogTypes logType, string PMplayerName = null)
        {
            this.LogType = logType;

            string[] allFiles = Directory.GetFiles(directoryPath);
            foreach (string file in allFiles)
            {
                string nametofind = GameLogTypesEX.GetNameForLogType(logType);
                if (PMplayerName != null)
                {
                    nametofind += "__" + PMplayerName;
                }
                if (Path.GetFileName(file).StartsWith(nametofind, StringComparison.Ordinal))
                {
                    FileBank.Add(file);
                }
            }
        }

        public List<DateTime> FindAllMeditsOnDate(DateTime date)
        {
            List<DateTime> Result = new List<DateTime>();
            bool fileFound = false;
            bool isFileMonthly = false;
            string foundFilePath = null;

            string filterPattern = date.ToString(@"yyyy-MM-dd");
            filterPattern += @".txt";
            string filterPattern2 = date.ToString(@"yyyy-MM");
            filterPattern2 += @".txt";

            foreach (string file in FileBank)
            {
                if (!fileFound && (Path.GetFileName(file).Contains(filterPattern)))
                {
                    fileFound = true;
                    foundFilePath = file;
                }
            }

            if (!fileFound)
            {
                foreach (string file in FileBank)
                {
                    if (!fileFound && Regex.IsMatch(Path.GetFileName(file), filterPattern2))
                    {
                        fileFound = true;
                        isFileMonthly = true;
                        foundFilePath = file;
                    }
                }
            }

            if (fileFound)
            {
                TextFileObject textfile = new TextFileObject(foundFilePath, true, true, false, true, false, false);
                string line;

                bool compareFinished = false;
                bool compareAllowed = true;
                if (isFileMonthly) compareAllowed = false;

                while (!compareFinished && ((line = textfile.ReadNextLine()) != null))
                {
                    if (isFileMonthly && compareAllowed && line.StartsWith("Logging started", StringComparison.Ordinal))
                    {
                        compareFinished = true;
                    }

                    if (!compareAllowed)
                    {
                        if (line.StartsWith("Logging started " + date.ToString("yyyy-MM-dd"), StringComparison.Ordinal))
                        {
                            compareAllowed = true;
                        }
                    }

                    if (compareAllowed && line.Contains("You finish your meditation"))
                    {
                        DateTime meditTime = new DateTime(
                            date.Year, 
                            date.Month, 
                            date.Day, 
                            Convert.ToInt32(line.Substring(1,2)), 
                            Convert.ToInt32(line.Substring(4,2)), 
                            Convert.ToInt32(line.Substring(7,2)));
                        Result.Add(meditTime);
                    }
                }
            }
            return Result;
        }

        public int FindMaxMeditSkill(DateTime date)
        {
            int Result = -1;

            bool fileFound = false;
            bool isFileMonthly = false;
            string foundFilePath = null;

            string filterPattern = date.ToString(@"yyyy-MM-dd");
            filterPattern += @".txt";
            string filterPattern2 = date.ToString(@"yyyy-MM");
            filterPattern2 += @".txt";

            foreach (string file in FileBank)
            {
                if (!fileFound && (Path.GetFileName(file).Contains(filterPattern)))
                {
                    fileFound = true;
                    foundFilePath = file;
                }
            }

            if (!fileFound)
            {
                foreach (string file in FileBank)
                {
                    if (!fileFound && Regex.IsMatch(Path.GetFileName(file), filterPattern2))
                    {
                        fileFound = true;
                        isFileMonthly = true;
                        foundFilePath = file;
                    }
                }
            }

            if (fileFound)
            {
                TextFileObject textfile = new TextFileObject(foundFilePath, true, true, false, true, false, false);
                //List<DateTime> parsedDays = new List<DateTime>();

                for (int i = textfile.getLastIndex(); i >= 0; i--)
                {
                    if (textfile.ReadLine(i).Contains("Meditating increased"))
                    {
                        string line = textfile.ReadLine(i);
                        Match meditskillcapture = Regex.Match(line, @"to \d\d*"); //does not capture decimal spaces
                        Result = GeneralHelper.MatchToInt32(meditskillcapture);
                        break;
                    }

                    //if (isFileMonthly && textfile.ReadLine(i).StartsWith("Logging started"))
                    //{
                    //    string line = textfile.ReadLine(i).Substring(16, 10);
                    //    parsedDays.Add(new DateTime(
                    //        Convert.ToInt32(line.Substring(0, 4)),
                    //        Convert.ToInt32(line.Substring(5, 2)),
                    //        Convert.ToInt32(line.Substring(8, 2))));
                    //}
                }
            }

            return Result;
        }
    }
}
