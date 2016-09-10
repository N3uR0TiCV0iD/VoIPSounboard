using System;
using Keys = System.Windows.Forms.Keys;
namespace HiT.VoIPSoundboard
{
    public class SoundData
    {
        Keys hotkey;
        string name;
        string path;
        int timesPlayed;
        public SoundData(string name, string path, Keys hotkey, int timesPlayed)
        {
            this.timesPlayed = timesPlayed;
            this.hotkey = hotkey;
            this.name = name;
            this.path = path;
        }
        public int TimesPlayed
        {
            get
            {
                return timesPlayed;
            }
            set
            {
                timesPlayed = value;
            }
        }
        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public Keys Hotkey
        {
            get
            {
                return hotkey;
            }
            set
            {
                hotkey = value;
            }
        }
    }
}
