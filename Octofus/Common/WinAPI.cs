using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Octofus.Common
{
    public static class WinAPI
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetForegroundWindow(IntPtr handle);

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        public const int WS_EX_TRANSPARENT = 0x00000020;
        public const int GWL_EXSTYLE = (-20);

        public const int HOTKEY_ID1 = 9000;
        public const int HOTKEY_ID2 = 9001;
        public const int HOTKEY_ID3 = 9002;
        public const int HOTKEY_ID4 = 9003;
        public const int HOTKEY_ID5 = 9004;

        public const uint MOD_NONE = 0x0000; //(none)
        public const uint MOD_ALT = 0x0001; //ALT
        public const uint MOD_CONTROL = 0x0002; //CTRL
        public const uint MOD_SHIFT = 0x0004; //SHIFT
        public const uint MOD_WIN = 0x0008; //WINDOWS

        public const uint VK_F1 = 0x70;
        public const uint VK_F2 = 0x71;
        public const uint VK_F3 = 0x72;
        public const uint VK_F4 = 0x73;

        public static void SetTransparent(Window window)
        {
            var windowHandle = new WindowInteropHelper(window).Handle;
            int extendedStyle = GetWindowLong(windowHandle, GWL_EXSTYLE);
            SetWindowLong(windowHandle, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
        }
    }
}

