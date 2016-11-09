using System;
using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;
namespace HiT.VoIPSoundboard
{
    public static class MicrophoneHelper
    {
        public static string[] GetDeviceNames()
        {
            MMDeviceEnumerator deviceEnumerator = new MMDeviceEnumerator();
            MMDeviceCollection microphones = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
            string[] deviceNames = new string[microphones.Count];
            for (int currMicrophoneIndex = 0; currMicrophoneIndex < microphones.Count; currMicrophoneIndex++)
            {
                deviceNames[currMicrophoneIndex] = microphones[currMicrophoneIndex].FriendlyName;
            }
            return deviceNames;
        }
        public static MMDevice GetMicrophone(int index)
        {
            return (new MMDeviceEnumerator()).EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active)[index];
        }
        public static MMDevice GetMicrophoneFromID(string id, out int index)
        {
            MMDevice currMicrophone;
            MMDeviceEnumerator deviceEnumerator = new MMDeviceEnumerator();
            MMDeviceCollection devices = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
            for (int currDeviceIndex = 0; currDeviceIndex < devices.Count; currDeviceIndex++)
            {
                currMicrophone = devices[currDeviceIndex];
                if (currMicrophone.ID == id)
                {
                    index = currDeviceIndex;
                    return currMicrophone;
                }
            }
            index = -1;
            return null;
        }
        public static bool IsMicrophoneActive(MMDevice microphone)
        {
            /*
            SessionCollection audioSessions = microphone.AudioSessionManager.Sessions;
            for (int currSessionIndex = 0; currSessionIndex < audioSessions.Count; currSessionIndex++)
            {
                if (audioSessions[currSessionIndex].State == AudioSessionState.AudioSessionStateActive)
                {
                    return true;
                }
            }
            */
            return false;
        }
    }
}
