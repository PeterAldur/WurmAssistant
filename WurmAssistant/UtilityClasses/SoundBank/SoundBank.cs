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
using IrrKlang;
using System.Data;

namespace WurmAssistant
{
    static public class SoundBank
    {
        public static ISoundEngine SoundEngine;
        public static string SoundsDirectory;

        static Dictionary<string, SB_SoundPlayer> dictSoundBank = new Dictionary<string, SB_SoundPlayer>();
        static List<string> allSoundsNames = new List<string>();

        static Dictionary<string, float> AdjustedVolumesDict = new Dictionary<string, float>();
        static DataSet AdjustedVolumesStorage = new DataSet("DefVolSaved");

        public static float GlobalVolume = 1.0F;

        static bool SoundBankCreated = false;

        public static void CreateSoundBank()
        {
            try
            {
                SoundEngine = new ISoundEngine();
                SoundBankCreated = true;
                SoundsDirectory = WurmAssistant.PersistentDataDir + "SoundBank";
                InitDefVolumesDict();
                InitSoundBank();
            }
            catch (Exception _e)
            {
                SoundBankCreated = false;
                Logger.WriteLine("! Unable to initialize sound engine, sounds will not work");
                Logger.LogException(_e);
            }
        }

        public static void InitSoundBank()
        {
            if (SoundBankCreated)
            {
                if (!Directory.Exists(SoundsDirectory))
                {
                    Directory.CreateDirectory(SoundsDirectory);
                }
                List<string> files = new List<string>();
                files.AddRange(Directory.GetFiles(SoundsDirectory, "*.wav"));
                files.AddRange(Directory.GetFiles(SoundsDirectory, "*.mp3"));
                files.AddRange(Directory.GetFiles(SoundsDirectory, "*.ogg"));
                files.AddRange(Directory.GetFiles(SoundsDirectory, "*.flac"));
                files.AddRange(Directory.GetFiles(SoundsDirectory, "*.mod"));
                files.AddRange(Directory.GetFiles(SoundsDirectory, "*.it"));
                files.AddRange(Directory.GetFiles(SoundsDirectory, "*.s3d"));
                files.AddRange(Directory.GetFiles(SoundsDirectory, "*.xm"));

                SoundEngine.RemoveAllSoundSources();
                dictSoundBank.Clear();
                allSoundsNames.Clear();
                foreach (string file in files)
                {
                    SB_SoundPlayer newsound = new SB_SoundPlayer(file);
                    float volume;
                    if (AdjustedVolumesDict.TryGetValue(Path.GetFileName(file), out volume))
                    {
                        newsound.Load(volume);
                    }
                    else newsound.Load();
                    dictSoundBank.Add(Path.GetFileName(file), newsound);
                    allSoundsNames.Add(Path.GetFileName(file));
                }
            }
            else
            {
                Logger.WriteLine("!!! ERROR: Sound Bank Init before Create, no sounds loaded");
            }
        }

        /// <summary>
        /// Plays sound by name, if the sound is cached
        /// </summary>
        /// <param name="name">Name of the sound, case sensitive</param>
        static public void PlaySound(string name)
        {
            SB_SoundPlayer player;
            if (dictSoundBank.TryGetValue(name, out player))
            {
                try { player.Play(); }
                catch (FileNotFoundException)
                {
                    InitSoundBank();
                }
            }
        }

        /// <summary>
        /// Returns SoundPlayer instance for this sound name
        /// </summary>
        /// <param name="soundname">Name of the sound, case sensitive</param>
        /// <returns>SB_SoundPlayer if exists, else null</returns>
        static public SB_SoundPlayer getSoundPlayer(string soundname)
        {
            SB_SoundPlayer player;
            if (dictSoundBank.TryGetValue(soundname, out player))
            {
                return player;
            }
            else return null;
        }

        /// <summary>
        /// Returns array of all sound file names without extension
        /// </summary>
        /// <returns></returns>
        static public string[] getSoundsArray()
        {
            return allSoundsNames.ToArray();
        }

        static public void ChangeGlobalVolume(float volume)
        {
            if (SoundBankCreated) SoundEngine.SoundVolume = volume;
            GlobalVolume = volume;
        }

        static public void StopSounds()
        {
            if (SoundBankCreated) SoundEngine.StopAllSounds();
        }

        static public float GetVolumeForSound(string soundname)
        {
            float adjvolume;
            if (AdjustedVolumesDict.TryGetValue(soundname, out adjvolume))
            {
                return adjvolume;
            }
            else return 1.0F;
        }

        static public void AdjustVolumeForSound(string soundname, float volume)
        {
            AdjustedVolumesDict[soundname] = volume;
            SB_SoundPlayer player;
            if (dictSoundBank.TryGetValue(soundname, out player))
            {
                player.ChangeVolume(volume);
            }
            SaveAdjustedVolumeStorage();
        }

        static public void RemoveSound(string soundname)
        {
            try
            {
                File.Delete(SoundsDirectory + @"\" + soundname);
            }
            catch (Exception _e)
            {
                RemoveAdjustedVolumeEntryFromDict(soundname);
                Logger.WriteLine("SoundBank: Error while trying to delete sound " + soundname);
                Logger.LogException(_e);
            }
            InitSoundBank();
        }

        static void RemoveAdjustedVolumeEntryFromDict(string sndname)
        {
            try //debugging
            { AdjustedVolumesDict.Remove(Path.GetFileName(sndname)); }
            catch (Exception _e) { Logger.LogException(_e); }
        }

        static public void RenameSound(string oldpath, string newpath)
        {
            bool moveFailed = false;
            try
            {
                File.Move(oldpath, newpath);
            }
            catch (Exception _e)
            {
                moveFailed = true;
                Logger.WriteLine("Exception while renaming file " + oldpath + " to " + newpath);
                Logger.LogException(_e);
            }
            if (!moveFailed)
            {
                float oldAdjVolume = GetVolumeForSound(Path.GetFileName(oldpath));
                RemoveAdjustedVolumeEntryFromDict(Path.GetFileName(oldpath));
                AdjustVolumeForSound(Path.GetFileName(newpath), oldAdjVolume);
            }
            InitSoundBank();
        }

        static public void AddSound(string filename)
        {
            try
            {
                File.Copy(filename, SoundsDirectory + @"\" + Path.GetFileName(filename));
            }
            catch (IOException _e)
            {
                if (MessageBox.Show(Path.GetFileName(filename) + " already exists in SoundBank, add it anyway?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes)
                {
                    try
                    {
                        bool fileCopied = false;
                        int index = 1;
                        while (!fileCopied)
                        {
                            string newSavePath = SoundsDirectory + @"\" + Path.GetFileNameWithoutExtension(filename) + "_" + index + Path.GetExtension(filename);
                            if (!File.Exists(newSavePath))
                            {
                                File.Copy(filename, newSavePath);
                                fileCopied = true;
                            }
                            else index++;
                        }
                    }
                    catch
                    {
                        Logger.WriteLine("Exception while trying to copy " + filename);
                        Logger.LogException(_e);
                    }
                }
            }
        }

        #region ADJUSTED VOLUMES XML STORAGE

        public static void InitDefVolumesDict()
        {
            bool readFailed = false;
            try { AdjustedVolumesStorage.ReadXml(SoundsDirectory + @"\AdjVolSaved.xml"); }
            catch (DirectoryNotFoundException)
            {
                readFailed = true;
                Logger.WriteLine("Note: could not load Sound Bank adjusted volumes due directory missing");
            }
            catch (FileNotFoundException)
            {
                readFailed = true;
                Logger.WriteLine("Note: could not load Sound Bank adjusted volumes due file missing");
            }
            catch (Exception _e)
            {
                readFailed = true;
                Logger.WriteLine("!!Soundbank: Error while loading Sound Bank adjusted volumes");
                Logger.LogException(_e);
            }
            if (!readFailed)
            {
                try // debug
                {
                    foreach (DataRow row in AdjustedVolumesStorage.Tables[0].Rows)
                    {
                        AdjustedVolumesDict.Add(Convert.ToString(row[0]), (float)(Convert.ToDouble(row[1])));
                    }
                }
                catch (Exception _e)
                {
                    Logger.WriteLine("!! Error while populating Sound Bank adjusted volumes dict");
                    Logger.LogException(_e);
                }
            }
        }

        static public void SaveAdjustedVolumeStorage()
        {
            AdjustedVolumesStorage.Clear();
            AdjustedVolumesStorage.Tables.Add();

            AdjustedVolumesStorage.Tables[0].Columns.Add();
            AdjustedVolumesStorage.Tables[0].Columns.Add();

            foreach (var keyvalue in AdjustedVolumesDict)
            {
                string[] data = new string[2];
                data[0] = keyvalue.Key;
                data[1] = keyvalue.Value.ToString();
                try
                {
                    AdjustedVolumesStorage.Tables[0].Rows.Add(data);
                }
                catch (Exception _e)
                {
                    Logger.LogException(_e);
                }
            }
            try
            {
                AdjustedVolumesStorage.WriteXml(SoundsDirectory + @"\AdjVolSaved.xml");
            }
            catch (Exception _e)
            {
                Logger.WriteLine("!! Error while saving Sound Bank adjusted volumes");
                Logger.LogException(_e);
            }
        }

        #endregion
    }
}