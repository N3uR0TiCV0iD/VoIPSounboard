using System;
using NAudio.Wave;
namespace HiT.VoIPSoundboard.Soundboards
{
    public enum SoundBoardMode
    {
        SourceGame = 0,
        Discord = 1,
        Skype = 2
    }
    public interface ISoundboard
    {
        void PlayFile(string filePath, WaveStream waveStream);
        void SoundboardService();
        void StartRecording();
        void StopRecording();
        void StopPlaying();
        bool IsReady();
    }
}
