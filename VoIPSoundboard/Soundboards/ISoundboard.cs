using System;
using NAudio.Wave;
namespace HiT.VoIPSoundboard.Soundboards
{
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
