using System;
namespace HiT.VoIPSoundboard
{
    public class VoiceInfo
    {
        string voiceName;
        string voiceCode;
        public VoiceInfo(string voiceName, string voiceCode)
        {
            this.voiceName = voiceName;
            this.voiceCode = voiceCode;
        }
        public string VoiceName
        {
            get
            {
                return voiceName;
            }
        }
        public string VoiceCode
        {
            get
            {
                return voiceCode;
            }
        }
    }
}
