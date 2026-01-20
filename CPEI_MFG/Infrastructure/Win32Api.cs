using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CPEI_MFG.Infrastructure
{
    class Win32Api
    {
        private const int EM_GETLINE = 0xC4;
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessage_EX(IntPtr hwnd, int wMsg, int wParam, StringBuilder lParam);
        [DllImport("USER32.DLL")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int cch);
        [DllImport("USER32.DLL")]
        public static extern bool SetActiveWindow(IntPtr hWnd);
        [DllImport("USER32.DLL")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "FindWindowEx", CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll", EntryPoint = "mouse_event")]
        private static extern int mouse_event(
            int dwFlags,
            int dx,
            int dy,
            int cButtons,
            int dwExtraInfo
        );

        [DllImport("user32.dll", EntryPoint = "keybd_event")]
        private static extern int keybd_event
            (
           byte bVk,
           byte bScan,
           int dwFlags,
           int dwExtraInfo
            );
        //定义callBack函数跟遍历窗体所有控件相关函数
        public delegate bool CallBack(IntPtr Hwnd, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern IntPtr EnumWindows(CallBack x, IntPtr Hwnd);
        [DllImport("user32.dll")]
        public static extern IntPtr EnumChildWindows(IntPtr hwndParent, CallBack x, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern IntPtr GetClassNameW(IntPtr Hwnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpString, int nMaxCount);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int Mouse_event(int dwFlags, int dx, int dy, int cButtons, int deExtraInfo);
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, string iParam);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, uint mCmdShow);
    }
}
