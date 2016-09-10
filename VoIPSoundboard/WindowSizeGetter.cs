using System;
using System.Runtime.InteropServices;
namespace HiT.VoIPSoundboard
{
    public static class WindowSizeGetter
    {
        [DllImport("user32.dll")]
        private static extern int GetWindowPlacement(IntPtr hwnd, ref WINDOWPLACEMENT lpwndl);
        public static RECT GetWindowSize(IntPtr hwnd)
        {
            WINDOWPLACEMENT currWindowPlacement = new WINDOWPLACEMENT(Marshal.SizeOf(new WINDOWPLACEMENT()));
            GetWindowPlacement(hwnd, ref currWindowPlacement);
            return currWindowPlacement.RCNormalPosition;
        }
    }
    public struct POINTAPI
    {
        int x;
        int y;
    }
    public struct RECT
    {
        int left;
        int top;
        int right;
        int bottom;
        public int Left
        {
            get { return left; }
        }
        public int Top
        {
            get { return top; }
        }
        public int Right
        {
            get { return right; }
        }
        public int Bottom
        {
            get { return bottom; }
        }
    }
    public struct WINDOWPLACEMENT
    {
        int length;
        int flags;
        int showCmd;
        POINTAPI ptMinPosition;
        POINTAPI ptMaxPosition;
        RECT rcNormalPosition;
        public WINDOWPLACEMENT(int length)
        {
            this.ptMinPosition = new POINTAPI();
            this.ptMaxPosition = new POINTAPI();
            this.rcNormalPosition = new RECT();
            this.length = length;
            this.showCmd = 0;
            this.flags = 0;
        }
        public RECT RCNormalPosition
        {
            get
            {
                return rcNormalPosition;
            }
        }
    }
}
