using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;

namespace WurmAssistant
{
    static class WurmPaths
    {
        static Dictionary<string, string> PlayerToLogsPathMap = new Dictionary<string, string>();
        static string CachedWurmDirFromRegistry;
        static string WurmDirManualOverride;
        public static bool DirChanged = false;

        static string CacheWurmDirFromRegistry()
        {
            string wurmdir = Convert.ToString(Registry.GetValue(@"HKEY_CURRENT_USER\Software\JavaSoft\Prefs\com\wurmonline\client", "wurm_dir", null));
            if (wurmdir != null)
            {
                wurmdir = wurmdir.Replace(@"//", @"\");
                wurmdir = wurmdir.Replace(@"/", @"");
                wurmdir = wurmdir.Trim();
                if (!wurmdir.EndsWith(@"\", StringComparison.Ordinal)) wurmdir += @"\";
                //if (wurmdir.EndsWith(@"players\", StringComparison.Ordinal)) wurmdir = wurmdir.Remove(wurmdir.Length - 8);
                CachedWurmDirFromRegistry = wurmdir;
                return wurmdir;
            }
            else return null;
        }

        static void BuildPlayersToLogsPathMap()
        {
            PlayerToLogsPathMap.Clear();
            try
            {
                string[] allplayers = Directory.GetDirectories(Path.Combine(WurmDir, @"players\"));
                foreach (var playerpath in allplayers)
                {
                    string playername = GeneralHelper.GetLastFolderNameFromPath(playerpath);
                    PlayerToLogsPathMap.Add(playername, Path.Combine(playerpath, @"logs\"));
                }
            }
            catch (Exception _e)
            {
                Logger.WriteLine("Error while searching for player directories, perhaps path was wrong");
                Logger.LogException(_e);
            }
        }

        //

        public static void Initialize()
        {
            WurmDirManualOverride = WurmAssistant.ZeroRef.WurmDirOverride;
            BuildPlayersToLogsPathMap();
        }

        //

        /// <summary>
        /// null if all sources fail
        /// </summary>
        public static string WurmDir
        {
            get
            {
                if (WurmDirManualOverride != null) return WurmDirManualOverride;
                else if (CachedWurmDirFromRegistry != null) return CachedWurmDirFromRegistry;
                else
                {
                    return CacheWurmDirFromRegistry();
                }
            }
        }

        public static string[] AllPlayers
        {
            get { return PlayerToLogsPathMap.Keys.ToArray<string>(); }
        }

        //public static void OverrideWurmDir(string newPath)
        //{
        //    WurmDirManualOverride = newPath;
        //    BuildPlayersToLogsPathMap();
        //}

        //public static void ClearOverridenWurmDir()
        //{
        //    WurmDirManualOverride = null;
        //    BuildPlayersToLogsPathMap();
        //}

        /// <summary>
        /// returns null if error
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static string GetLogsDirForPlayer(string player)
        {
            try
            {
                return PlayerToLogsPathMap[player];
            }
            catch (Exception _e)
            {
                Logger.WriteLine("Error while retrieving logs dir for player name");
                Logger.LogException(_e);
                return null;
            }
        }
    }
}
