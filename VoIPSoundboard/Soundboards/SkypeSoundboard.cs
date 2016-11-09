using System;
using NAudio.Wave;
using SKYPE4COMLib;
using System.Media;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using TAttachmentStatus = SKYPE4COMLib.TAttachmentStatus;
namespace HiT.VoIPSoundboard.Soundboards
{
    //http://users.skynet.be/fa258239/bestanden/skype4com/skype4com.pdf
    public class SkypeSoundboard : ISoundboard
    {
        [DllImport("user32.dll")] private static extern uint GetWindowThreadProcessId(IntPtr hwnd, out uint lpdwProcessId);
        TUserStatus fullscreenStatus;
        Process fullscreenProcess;
        SoundPlayer soundPlayer;
        TUserStatus lastStatus;
        Process skypeProcess;
        bool checkFullscreen;
        NotifyIcon trayIcon;
        Call currentCall;
        Skype skype;
        public SkypeSoundboard(NotifyIcon trayIcon)
        {
            this.skype = new Skype();
            this.trayIcon = trayIcon;
            this.skype.CallStatus += skype_CallStatus;
            this.fullscreenStatus = TUserStatus.cusDoNotDisturb;
        }
        public SkypeSoundboard(NotifyIcon trayIcon, bool checkFullscreen, TUserStatus fullscreenStatus) : this(trayIcon)
        {
            this.fullscreenStatus = fullscreenStatus;
            this.checkFullscreen = checkFullscreen;
        }
        public Process SkypeProcess
        {
            get
            {
                return skypeProcess;
            }
            set
            {
                skypeProcess = value;
            }
        }
        public TUserStatus FullscreenStatus
        {
            get
            {
                return fullscreenStatus;
            }
            set
            {
                fullscreenStatus = value;
            }
        }
        public bool CheckFullscreen
        {
            get
            {
                return checkFullscreen;
            }
            set
            {
                checkFullscreen = value;
            }
        }
        private void skype_CallStatus(Call pCall, TCallStatus Status)
        {
            switch (pCall.Status)
            {
                case TCallStatus.clsInProgress:
                    trayIcon.ShowBalloonTip(5000, "VoIPSoundboard - In call", "VoIPSoundboard has detected that you are in a Skype call.", ToolTipIcon.Info);
                    currentCall = pCall;
                break;
                case TCallStatus.clsFinished:
                    if (skype.ActiveCalls.Count == 0)
                    {
                        currentCall = null;
                    }
                    else
                    {
                        currentCall = skype.ActiveCalls[1];
                    }
                break;
            }
        }
        public void SoundboardService()
        {
            if (skypeProcess != null)
            {
                if (!skypeProcess.HasExited)
                {
                    if (((ISkype)skype).AttachmentStatus == TAttachmentStatus.apiAttachSuccess)
                    {
                        if (checkFullscreen)
                        {
                            CheckForFullscreen();
                        }
                    }
                    else
                    {
                        try
                        {
                            skype.Attach();
                            if (skype.ActiveCalls.Count != 0)
                            {
                                currentCall = skype.ActiveCalls[1];
                                trayIcon.ShowBalloonTip(5000, "VoIPSoundboard - In call", "VoIPSoundboard has detected that you are in a Skype call.", ToolTipIcon.Info);
                            }
                            else
                            {
                                trayIcon.ShowBalloonTip(5000, "VoIPSoundboard - Attached", "VoIPSoundboard has been successfully attached to Skype.", ToolTipIcon.Info);
                            }
                        }
                        catch (COMException ex)
                        {
                            const int NOTATTACHED_ERROR = -2147220992;
                            const int ATTACH_TIMEOUT = -2147220991;
                            int hr = Marshal.GetHRForException(ex);
                            switch (hr)
                            {
                                case ATTACH_TIMEOUT:
                                    trayIcon.ShowBalloonTip(5000, "VoIPSoundboard - Attach timeout", "VoIPSoundboard was unable to attach to Skype, retrying in 10 seconds...", ToolTipIcon.Error);
                                    Thread.Sleep(10000);
                                break;
                                case NOTATTACHED_ERROR:
                                    trayIcon.ShowBalloonTip(5000, "VoIPSoundboard - Skype detached", "VoIPSoundboard is no longer attached to Skype.", ToolTipIcon.Info);
                                break;
                                default:
                                    ShowCOMException(ex, hr, "SkypeSoundboard->SoundboardService");
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Utils.ShowUnhandledException(ex, "SkypeSoundboard->SoundboardService");
                        }
                    }
                }
                else
                {
                    skypeProcess.Dispose();
                    skypeProcess = null;
                }
            }
        }
        private void CheckForFullscreen()
        {
            if (fullscreenProcess == null)
            {
                //See if foregroundWindow is fullscreened
                var screenBounds = Screen.PrimaryScreen.Bounds;
                IntPtr foregroundWindow = Utils.GetForegroundWindow();
                RECT currWindowSize = WindowSizeGetter.GetWindowSize(foregroundWindow);
                if (currWindowSize.Left == 0 && currWindowSize.Top == 0 &&
                    currWindowSize.Right == screenBounds.Width && currWindowSize.Bottom == screenBounds.Height)
                {
                    uint processID = 0;
                    GetWindowThreadProcessId(foregroundWindow, out processID);
                    if (processID != 0)
                    {
                        lastStatus = (TUserStatus)skype.CurrentUser.OnlineStatus;
                        fullscreenProcess = Process.GetProcessById((int)processID);
                        skype.ChangeUserStatus(fullscreenStatus);
                    }
                }
            }
            else if (fullscreenProcess.HasExited)
            {
                //The fullscreen process has exited... Let's set your status back
                skype.ChangeUserStatus(lastStatus);
                fullscreenProcess.Dispose();
                fullscreenProcess = null;
            }
        }
        public void PlayFile(string filePath, WaveStream waveStream)
        {
            try
            {
                currentCall.set_InputDevice(TCallIoDeviceType.callIoDeviceTypeFile, filePath);
                soundPlayer = new SoundPlayer(filePath);
                soundPlayer.Play();
            }
            catch (COMException ex)
            {
                const int CALL_ENDED = 687685487;
                const int IOALTERATION_ERROR = -2147352567;
                int hr = Marshal.GetHRForException(ex);
                switch (hr)
                {
                    case CALL_ENDED:
                        trayIcon.ShowBalloonTip(5000, "VoIPSoundboard - Not in call", "VoIPSoundboard has detected that you were no longer in call.", ToolTipIcon.Info);
                    break;
                    case IOALTERATION_ERROR:
                        trayIcon.ShowBalloonTip(5000, "VoIPSoundboard - IO Alteration", "VoIPSoundboard was unable to change Skype's output. Try restarting the call...", ToolTipIcon.Error);
                    break;
                    default:
                        ShowCOMException(ex, hr, "SkypeSoundboard->PlayFile");
                    break;
                }
                currentCall = null;
            }
            catch (Exception ex)
            {
                Utils.ShowUnhandledException(ex, "SkypeSoundboard->PlayFile");
            }
        }
        private void ShowCOMException(Exception ex, int hr, string methodName)
        {
            MessageBox.Show("ERROR: Unhandled COMException (" + hr + "): " + ex.Message, "ERROR: Unhandled COMException. @" + methodName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public void StopPlaying()
        {
            if (currentCall != null)
            {
                currentCall.set_InputDevice(TCallIoDeviceType.callIoDeviceTypeFile, String.Empty);
                soundPlayer.Stop();
            }
        }
        public void StartRecording()
        {
            //Use the Skype API to record
        }
        public void StopRecording()
        {
            //Use the Skype API to stop recording
        }
        public bool ToggleMicMute()
        {
            bool isMuted = ((ISkype)skype).Mute;
            ((ISkype)skype).Mute = !isMuted;
            return !isMuted;
        }
        public bool HasProcess()
        {
            return skypeProcess != null && !skypeProcess.HasExited;
        }
        public bool IsReady()
        {
            return currentCall != null;
        }
    }
}
