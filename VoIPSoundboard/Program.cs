using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace HiT.VoIPSoundboard
{
    public static class Program
    {
        [DllImport("user32.dll")] private static extern bool ShowWindowAsync(IntPtr hwnd, int showCMD);
        [DllImport("user32.dll")] public static extern bool SetForegroundWindow(IntPtr hwnd);
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
                myProcess = null;
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);                
                Application.Run(new MainForm());
            }
            else
            {
                ShowWindowAsync(runningProcess.MainWindowHandle, SW_SHOWNORMAL);
                SetForegroundWindow(runningProcess.MainWindowHandle);
            }
        }
    }
}
