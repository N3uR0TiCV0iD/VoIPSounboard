using System;
using NAudio.Wave;
namespace HiT.VoIPSoundboard.Soundboards
{
    public interface ISoundboard
    {
        void PlayFile(string filePath, WaveFileReader waveFileReader);
        void SoundboardService();
        void StartRecording();
        void StopRecording();
        void StopPlaying();
        bool IsReady();
    }
}
