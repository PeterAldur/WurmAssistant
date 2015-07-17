using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IrrKlang;
using System.IO;

namespace WurmAssistant
{
    /// <summary>
    /// Leftover class from wrapping NET default soundplayer, reimplemented to maintain compatibility
    /// </summary>
    public class SB_SoundPlayer
    {
        string FilePath;
        ISoundSource SoundRef;
        bool isInitalized = false;

        public SB_SoundPlayer(string filePath)
        {
            this.FilePath = filePath;
        }

        public void Load(float defVolume = 1.0F)
        {
            try
            {
                SoundRef = SoundBank.SoundEngine.AddSoundSourceFromFile(FilePath);
                ChangeVolume(defVolume);
                isInitalized = true;
            }
            catch (Exception _e)
            {
                Logger.LogException(_e);
            }
        }

        public void Play()
        {
            if (isInitalized) SoundBank.SoundEngine.Play2D(FilePath);
        }

        public void ChangeVolume(float volume)
        {
            if (volume < 0.0F) volume = 0.0F;
            else if (volume > 1.0F) volume = 1.0F;
            if (isInitalized) SoundRef.DefaultVolume = volume;
        }

        public void Remove()
        {
            if (isInitalized) SoundRef.Dispose();
        }
    }
}
