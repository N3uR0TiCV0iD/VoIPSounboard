using System;
using NAudio.CoreAudioApi;
namespace HiT.VoIPSoundboard
{
    public static class VolumeMixer //TODO: Test with NAudio
    {
        public static int? GetApplicationVolume(int pid)
        {
            SimpleAudioVolume volumeObject = GetVolumeObject(pid);
            if (volumeObject != null)
            {
                int level = (int)Math.Round(volumeObject.Volume * 100, 0);
                volumeObject.Dispose();
                return level;
            }
            return null;
        }
        public static bool? GetApplicationMute(int pid)
        {
            SimpleAudioVolume volumeObject = GetVolumeObject(pid);
            if (volumeObject != null)
            {
                bool mute = volumeObject.Mute;
                volumeObject.Dispose();
                return mute;
            }
            return null;
        }
        public static void SetApplicationVolume(int pid, int level)
        {
            SimpleAudioVolume volumeObject = GetVolumeObject(pid);
            if (volumeObject != null)
            {
                volumeObject.Volume = level / 100F;
                volumeObject.Dispose();
            }
        }
        public static void SetApplicationMute(int pid, bool mute)
        {
            SimpleAudioVolume volumeObject = GetVolumeObject(pid);
            if (volumeObject != null)
            {
                volumeObject.Mute = mute;
                volumeObject.Dispose();
            }
        }
        private static SimpleAudioVolume GetVolumeObject(int pid)
        {
            AudioSessionControl currSession;
            MMDeviceEnumerator deviceEnumerator = new MMDeviceEnumerator();
            MMDevice defaultSpeaker = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            /*
            SessionCollection audioSessions = defaultSpeaker.AudioSessionManager.Sessions;
            for (int currSessionIndex = 0; currSessionIndex < audioSessions.Count; currSessionIndex++)
            {
                currSession = audioSessions[currSessionIndex];
                if (currSession.GetProcessID == pid)
                {
                    return currSession.SimpleAudioVolume;
                }
            }
            */
            return null;
        }
    }
}
