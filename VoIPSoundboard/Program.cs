using System;
using AutoUpdateModule;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace HiT.VoIPSoundboard
{
    public static class Program
    {
        [DllImport("user32.dll")] private static extern bool ShowWindowAsync(IntPtr hwnd, int showCMD);
        [DllImport("user32.dll")] public static extern bool SetForegroundWindow(IntPtr hwnd);
        const string VERSION = "1.0c";
        const int SW_SHOWNORMAL = 1;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Process runningProcess = null;
            Process myProcess = Process.GetCurrentProcess();
            foreach (var currProcess in Process.GetProcesses())
            {
                if (currProcess.Id != myProcess.Id && currProcess.ProcessName == myProcess.ProcessName) //TODO: Find a better method?
                {
                    runningProcess = currProcess;
                    break;
                }
            }
            if (runningProcess == null)
            {
                UpdateInfoFileHelper updateHelper = new UpdateInfoFileHelper("https://raw.githubusercontent.com/N3uR0TiCV0iD/VoIPSounboard/master/latestversion", new VersionInfo(VERSION));
                Application.SetCompatibleTextRenderingDefault(false);
                Application.EnableVisualStyles();
                myProcess = null;
                if (updateHelper.IsUpdateAvailable() && MessageBox.Show("There is a new update available, would you like to download it?",
                                                                        "INFO: New update available.", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    updateHelper.ApplyUpdate();
                }
                else
                {
                    Application.Run(new MainForm());
                }
            }
            else
            {
                ShowWindowAsync(runningProcess.MainWindowHandle, SW_SHOWNORMAL);
                SetForegroundWindow(runningProcess.MainWindowHandle);
            }
        }
    }
}
