using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace HiT.VoIPSoundboard
{
    public static class Utils
    {
        [DllImport("user32.dll")] public static extern IntPtr GetForegroundWindow();
        public static void ShowUnhandledException(Exception ex, string methodName)
        {
            MessageBox.Show("ERROR: " + ex.GetType().ToString() + " | " + ex.Message, "ERROR: Unhandled Exception. @" + methodName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
