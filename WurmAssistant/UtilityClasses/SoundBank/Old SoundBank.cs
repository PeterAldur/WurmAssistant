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

namespace WurmAssistant.Old
{
    static public class SoundBank
    {
        public static ISoundEngine SoundEngine = new ISoundEngine();
        public const string SoundsDirectory = "SoundBank";

        static Dictionary<string, SoundPlayer> dictSoundBank = new Dictionary<string, SoundPlayer>();
        static List<string> allSoundsNames = new List<string>();

        static SoundBank()
        {
            InitSoundBank();
        }

        public static void InitSoundBank()
        {
            if (!Directory.Exists(SoundsDirectory))
            {
                Directory.CreateDirectory(SoundsDirectory);
            }
            String[] files = Directory.GetFiles(SoundsDirectory, "*.wav");
            if (files.Length != 0)
            {
                dictSoundBank.Clear();
                allSoundsNames.Clear();
                foreach (string file in files)
                {
                    SoundPlayer newsound = new SoundPlayer(file);
                    newsound.Load();
                    dictSoundBank.Add(Path.GetFileNameWithoutExtension(file), newsound);
                    allSoundsNames.Add(Path.GetFileNameWithoutExtension(file));
                }
            }
        }

        /// <summary>
        /// Plays sound by name, if the sound is cached
        /// </summary>
        /// <param name="name">Name of the sound, case sensitive</param>
        static public void PlaySound(string name)
        {
            SoundPlayer player;
            if (dictSoundBank.TryGetValue(name, out player))
            {
                try { player.Play(); }
                catch (FileNotFoundException) { InitSoundBank(); }
            }
        }

        /// <summary>
        /// Returns SoundPlayer instance for this sound name
        /// </summary>
        /// <param name="soundname">Name of the sound, case sensitive</param>
        /// <returns>SoundPlayer if exists, else null</returns>
        static public SoundPlayer getSoundPlayer(string soundname)
        {
            SoundPlayer player;
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
    }
}