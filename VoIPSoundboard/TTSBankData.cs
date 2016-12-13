using System;
using System.Drawing;
using System.Collections.Generic;
namespace HiT.VoIPSoundboard
{
    public class TTSBankData
    {
        Image flagImage;
        string bankName;
        List<VoiceInfo> voices;
        public TTSBankData(string bankName)
        {
            this.bankName = bankName;
            //ttsBanksBox.DrawItem += ttsBanksBox_DrawItem;
            this.voices = new List<VoiceInfo>();
        }
        public string BankName
        {
            get
            {
                return bankName;
            }
        }
        public Image FlagImage
        {
            get
            {
                return flagImage;
            }
        }
        public int VoicesCount
        {
            get
            {
                return voices.Count;
            }
        }
        public VoiceInfo GetVoice(int index)
        {
            return voices[index];
        }
        public void AddVoice(VoiceInfo voice)
        {
            voices.Add(voice);
        }
    }
}
