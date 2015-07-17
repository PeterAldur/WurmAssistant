using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace WurmAssistant
{
    internal static class GeneralHelper
    {
        #region Path and Directory Helpers

        public static string GetLastFolderNameFromPath(string path)
        {
            try
            {
                path.Trim();
                if (path.LastIndexOf(@"\") == path.Length - 1) path = path.Remove(path.Length - 1, 1);
                return path = path.Remove(0, (path.LastIndexOf(@"\") + 1));
            }
            catch
            {
                if (path != null) return path;
                else return null;
            }
        }

        /// <summary>
        /// returns only folder name, for path use GetPathToDirectoryAbove
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetPreviousFolderNameFromPath(string path)
        {
            try
            {
                path = path.Trim();
                // remove last "\" if exists
                if (path.LastIndexOf(@"\") == path.Length - 1) path = path.Remove(path.Length - 1, 1);
                // strip path of bottom dir including leading "\"
                path = path.Substring(0, path.LastIndexOf(@"\"));
                // strip path of everything except last dir name
                return path = path.Remove(0, (path.LastIndexOf(@"\") + 1));
            }
            catch
            {
                if (path != null) return path;
                else return null;
            }
        }

        /// <summary>
        /// returns complete path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetPathToDirectoryAbove(string path)
        {
            try
            {
                path = path.Trim();
                // remove last "\" if exists
                if (path.LastIndexOf(@"\") == path.Length - 1) path = path.Remove(path.Length - 1, 1);
                // strip path of bottom dir including leading "\"
                return path = path.Substring(0, path.LastIndexOf(@"\"));
            }
            catch
            {
                if (path != null) return path;
                else return null;
            }
        }

        #endregion

        #region String Parsing

        public static bool IsNumeric(Object Expression)
        {
            if (Expression == null || Expression is DateTime)
                return false;

            if (Expression is Int16 || Expression is Int32 || Expression is Int64 || Expression is Decimal || Expression is Single || Expression is Double || Expression is Boolean)
                return true;

            try
            {
                if (Expression is string)
                    Double.Parse(Expression as string);
                else
                    Double.Parse(Expression.ToString());
                return true;
            }
            catch { } // just dismiss errors but return false
            return false;
        }

        /// <summary>
        /// REGEX: Convert Match to Int32
        /// </summary>
        /// <param name="str"></param>
        /// <returns>value if success or 0 if fails</returns>
        public static int MatchToInt32(Match match)
        {
            try
            {
                return Convert.ToInt32(Regex.Match(match.ToString(), @"\d\d*").ToString());
            }
            catch
            {
                return 0;
            }
        }

        //old+

        ///// <summary>
        ///// returns -1 if failed
        ///// </summary>
        ///// <param name="line"></param>
        ///// <returns></returns>
        //public static float ExtractSkillGAINFromLine(string line)
        //{
        //    float skillgain = -1;
        //    Match match = Regex.Match(line, @"by \d+\,\d+");
        //    if (!match.Success) match = Regex.Match(line, @"by \d+\.\d+");
        //    if (!match.Success) match = Regex.Match(line, @"by \d+");

        //    if (!match.Success)
        //    {
        //        Logger.WriteLine("! Processed skill line failed to match at ExtractSkillGAINfromLine, line: " + line);
        //        return -1;
        //    }
        //    else if (float.TryParse(match.Value.Substring(3), out skillgain))
        //    {
        //        return skillgain;
        //    }
        //    else if (float.TryParse(match.Value.Substring(3).Replace(".", ","), out skillgain))
        //    {
        //        return skillgain;
        //    }
        //    else return -1;
        //}

        ///// <summary>
        ///// returns -1 if failed
        ///// </summary>
        ///// <param name="line"></param>
        ///// <returns></returns>
        //public static float ExtractSkillLEVELFromLine(string line)
        //{
        //    float skilllevel = -1;
        //    Match match = Regex.Match(line, @"to \d+\,\d+");
        //    if (!match.Success) match = Regex.Match(line, @"to \d+\.\d+");
        //    if (!match.Success) match = Regex.Match(line, @"to \d+");

        //    if (!match.Success)
        //    {
        //        Logger.WriteLine("! Processed skill line failed to match at ExtractSkillLEVELFromLine, line: " + line);
        //        return -1;
        //    }
        //    else if (float.TryParse(match.Value.Substring(3), out skilllevel))
        //    {
        //        return skilllevel;
        //    }
        //    else if (float.TryParse(match.Value.Substring(3).Replace(".", ","), out skilllevel))
        //    {
        //        return skilllevel;
        //    }
        //    else return -1;
        //}

        //old-

        /// <summary>
        /// returns -1 if failed
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static float ExtractSkillGAINFromLine(string line)
        {
            return ExtractNumerFromLine(line, true);
        }

        /// <summary>
        /// returns -1 if failed
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static float ExtractSkillLEVELFromLine(string line)
        {
            return ExtractNumerFromLine(line, false);
        }

        static float ExtractNumerFromLine(string line, bool GAIN)
        {
            string part;
            if (GAIN) part = "by "; else part = "to ";

            float level = -1;
            Match match = Regex.Match(line, part + @"\d+\,\d+");
            if (!match.Success) match = Regex.Match(line, part + @"\d+\.\d+");
            if (!match.Success) match = Regex.Match(line, part + @"\d+");

            if (!match.Success)
            {
                Logger.WriteLine("! Processed skill line failed to match at ExtractNumerFromLine(" + part + "), line: " + line);
                return -1;
            }
            else if (InvariantParseFloat(match.Value.Substring(3), out level))
            {
                return level;
            }
            else if (InvariantParseFloat(match.Value.Substring(3).Replace(",", "."), out level))
            {
                return level;
            }
            else return -1;
        }

        static bool InvariantParseFloat(string text, out float result)
        {
            return float.TryParse(
                text,
                System.Globalization.NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out result);
        }

        #endregion

        internal static int Validate(int val, int min, int max)
        {
            if (val < min)
            {
                val = min;
            }
            else if (val > max)
            {
                val = max;
            }
            return val;
        }
    }
}
