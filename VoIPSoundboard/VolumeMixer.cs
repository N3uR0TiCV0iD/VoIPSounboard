using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
namespace HiT.VoIPSoundboard
{
    //http://stackoverflow.com/questions/20938934/controlling-applications-volume-by-process-id | Edited to make it work for mulitple sessions :P
    public static class VolumeMixer
    {
        public static float? GetApplicationVolume(int pid)
        {
            ISimpleAudioVolume[] volumeObjects = GetVolumeObjects(pid);
            if (volumeObjects.Length != 0)
            {
                float levelSum = 0;
                float currObjectLevel;
                foreach (var currVolumeObject in volumeObjects)
                {
                    currVolumeObject.GetMasterVolume(out currObjectLevel);
                    Marshal.ReleaseComObject(currVolumeObject);
                    levelSum += currObjectLevel;
                }
                return (levelSum / volumeObjects.Length) * 100;
            }
            return null;
        }
        public static bool? GetApplicationMute(int pid) //An application is technically muted when all volume objects are muted
        {
            ISimpleAudioVolume[] volumeObjects = GetVolumeObjects(pid);
            if (volumeObjects.Length != 0)
            {
                bool mute = true;
                foreach (var currVolumeObject in volumeObjects)
                {
                    if (mute) //If ONE of the volume objects is NOT MUTED, we stop checking...
                    {
                        currVolumeObject.GetMute(out mute);
                    }
                    Marshal.ReleaseComObject(currVolumeObject);
                }
                return mute;
            }
            return null;
        }
        public static bool SetApplicationVolume(int pid, float level)
        {
            ISimpleAudioVolume[] volumeObjects = GetVolumeObjects(pid);
            if (volumeObjects.Length != 0)
            {
                Guid guid = Guid.Empty;
                foreach (var currVolumeObject in volumeObjects)
                {
                    currVolumeObject.SetMasterVolume(level / 100, ref guid);
                    Marshal.ReleaseComObject(currVolumeObject);
                }
                return true;
            }
            return false;
        }
        public static bool SetApplicationMute(int pid, bool mute)
        {
            ISimpleAudioVolume[] volumeObjects = GetVolumeObjects(pid);
            if (volumeObjects.Length != 0)
            {
                Guid guid = Guid.Empty;
                foreach (var currVolumeObject in volumeObjects)
                {
                    currVolumeObject.SetMute(mute, ref guid);
                    Marshal.ReleaseComObject(currVolumeObject);
                }
                return true;
            }
            return false;
        }
        private static ISimpleAudioVolume[] GetVolumeObjects(int pid)
        {
            int sessionsCount;
            int currSessionPID;
            IMMDevice speakers;
            object objectReference;
            IAudioSessionManager2 audioSessionManager;
            IAudioSessionEnumerator sessionEnumerator;
            Guid IID_IAudioSessionManager2 = typeof(IAudioSessionManager2).GUID;
            List<ISimpleAudioVolume> volumeControls = new List<ISimpleAudioVolume>();
            IMMDeviceEnumerator deviceEnumerator = (IMMDeviceEnumerator)(new MMDeviceEnumerator());

            //Get the speakers (Render + Multimedia) device
            deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia, out speakers);

            //Activate the session manager. we need the enumerator
            speakers.Activate(ref IID_IAudioSessionManager2, 0, IntPtr.Zero, out objectReference);
            audioSessionManager = (IAudioSessionManager2)objectReference;

            //Enumerate sessions for this device
            audioSessionManager.GetSessionEnumerator(out sessionEnumerator);
            sessionEnumerator.GetCount(out sessionsCount);

            //Search for an audio session with the required PID
            for (int currSessionIndex = 0; currSessionIndex < sessionsCount; currSessionIndex++)
            {
                IAudioSessionControl2 sessionControl;
                sessionEnumerator.GetSession(currSessionIndex, out sessionControl);
                sessionControl.GetProcessId(out currSessionPID);
                if (currSessionPID == pid)
                {
                    volumeControls.Add((ISimpleAudioVolume)sessionControl);
                }
                else
                {
                    Marshal.ReleaseComObject(sessionControl);
                }
            }
            Marshal.ReleaseComObject(audioSessionManager);
            Marshal.ReleaseComObject(sessionEnumerator);
            Marshal.ReleaseComObject(deviceEnumerator);
            Marshal.ReleaseComObject(speakers);
            return volumeControls.ToArray();
        }

        [ComImport]
        [Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
        internal class MMDeviceEnumerator
        {
        }

        internal enum EDataFlow
        {
            eRender,
            eCapture,
            eAll,
            EDataFlow_enum_count
        }

        internal enum ERole
        {
            eConsole,
            eMultimedia,
            eCommunications,
            ERole_enum_count
        }

        [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IMMDeviceEnumerator
        {
            int NotImpl1();

            [PreserveSig]
            int GetDefaultAudioEndpoint(EDataFlow dataFlow, ERole role, out IMMDevice ppDevice);

            // the rest is not implemented
        }

        [Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IMMDevice
        {
            [PreserveSig]
            int Activate(ref Guid iid, int dwClsCtx, IntPtr pActivationParams, [MarshalAs(UnmanagedType.IUnknown)] out object ppInterface);

            // the rest is not implemented
        }

        [Guid("77AA99A0-1BD6-484F-8BC7-2C654C9A9B6F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IAudioSessionManager2
        {
            int NotImpl1();
            int NotImpl2();

            [PreserveSig]
            int GetSessionEnumerator(out IAudioSessionEnumerator SessionEnum);

            // the rest is not implemented
        }

        [Guid("E2F5BB11-0570-40CA-ACDD-3AA01277DEE8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IAudioSessionEnumerator
        {
            [PreserveSig]
            int GetCount(out int SessionCount);

            [PreserveSig]
            int GetSession(int SessionCount, out IAudioSessionControl2 Session);
        }

        [Guid("87CE5498-68D6-44E5-9215-6DA47EF883D8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface ISimpleAudioVolume
        {
            [PreserveSig]
            int SetMasterVolume(float fLevel, ref Guid EventContext);

            [PreserveSig]
            int GetMasterVolume(out float pfLevel);

            [PreserveSig]
            int SetMute(bool bMute, ref Guid EventContext);

            [PreserveSig]
            int GetMute(out bool pbMute);
        }

        [Guid("bfb7ff88-7239-4fc9-8fa2-07c950be9c6d"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IAudioSessionControl2
        {
            // IAudioSessionControl
            [PreserveSig]
            int NotImpl0();

            [PreserveSig]
            int GetDisplayName([MarshalAs(UnmanagedType.LPWStr)] out string pRetVal);

            [PreserveSig]
            int SetDisplayName([MarshalAs(UnmanagedType.LPWStr)]string Value, [MarshalAs(UnmanagedType.LPStruct)] Guid EventContext);

            [PreserveSig]
            int GetIconPath([MarshalAs(UnmanagedType.LPWStr)] out string pRetVal);

            [PreserveSig]
            int SetIconPath([MarshalAs(UnmanagedType.LPWStr)] string Value, [MarshalAs(UnmanagedType.LPStruct)] Guid EventContext);

            [PreserveSig]
            int GetGroupingParam(out Guid pRetVal);

            [PreserveSig]
            int SetGroupingParam([MarshalAs(UnmanagedType.LPStruct)] Guid Override, [MarshalAs(UnmanagedType.LPStruct)] Guid EventContext);

            [PreserveSig]
            int NotImpl1();

            [PreserveSig]
            int NotImpl2();

            // IAudioSessionControl2
            [PreserveSig]
            int GetSessionIdentifier([MarshalAs(UnmanagedType.LPWStr)] out string pRetVal);

            [PreserveSig]
            int GetSessionInstanceIdentifier([MarshalAs(UnmanagedType.LPWStr)] out string pRetVal);

            [PreserveSig]
            int GetProcessId(out int pRetVal);

            [PreserveSig]
            int IsSystemSoundsSession();

            [PreserveSig]
            int SetDuckingPreference(bool optOut);
        }
    }
}
