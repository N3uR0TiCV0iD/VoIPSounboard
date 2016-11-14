using System;
using System.Drawing;
using System.Collections.Generic;
namespace HiT.VoIPSoundboard
{
    public class TTSBankData
    {
        Image flagImage;
        string bankName;
        List<string> voices;
        public TTSBankData(string bankName)
        {
            this.bankName = bankName;
            //ttsBanksBox.DrawItem += ttsBanksBox_DrawItem;
            //"http://www.acapela-group.com/demo-tts/Elements/flags/" + English%20(UK) + ".png"
            this.voices = new List<string>();
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
        public string GetVoice(int index)
        {
            return voices[index];
        }
        public void AddVoice(string voiceName)
        {
            voices.Add(voiceName);
        }
    }
}
