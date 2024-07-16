using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace Octofus.Common
{
    public class HotKeyManager
    {
        private static IntPtr WindowHandle { get; set; }

        private static HwndSource Source { get; set; }

        private static Action<string> SetFocusMethod { get; set; }

        private static Dictionary<string, int> HotKeys = new Dictionary<string, int>();

        private static bool IsHookPaused { get; set; }

        public static void RegisterHotKeys(Window window, Action<string> setFocusMethod, params string[] keys)
        {
            SetFocusMethod = setFocusMethod;
            WindowHandle = new WindowInteropHelper(window).Handle;
            Source = HwndSource.FromHwnd(WindowHandle);
            Source.AddHook(HwndHook);

            for (int i = 0; i < keys.Length; i++)
            {
                int hotkeyId = 0x0001 + i; // Unique ID for each hotkey
                try
                {
                    Key key = (Key)Enum.Parse(typeof(Key), keys[i], true);
                    uint vkey = (uint)KeyInterop.VirtualKeyFromKey(key);
                    WinAPI.RegisterHotKey(WindowHandle, hotkeyId, WinAPI.MOD_NONE, vkey);
                    HotKeys.Add(keys[i], hotkeyId);
                }
                catch (Exception ex)
                {
                    // Handle exceptions, such as invalid key names
                    Console.WriteLine($"Failed to register hotkey: {keys[i]}. Exception: {ex.Message}");
                }
            }
        }

        private static IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;

            if (msg == WM_HOTKEY)
            {
                int hotkeyId = wParam.ToInt32();
                int vkey = (((int)lParam >> 16) & 0xFFFF);

                var key = HotKeys.FirstOrDefault(x => x.Value == hotkeyId).Key;

                if (!string.IsNullOrEmpty(key))
                {
                    SetFocusMethod(key.ToString());
                    handled = true;
                }
            }

            return IntPtr.Zero;
        }

        public static void UnregisterHotKeys()
        {
            Source.RemoveHook(HwndHook);

            foreach (var hotkey in HotKeys)
            {
                WinAPI.UnregisterHotKey(WindowHandle, hotkey.Value);
            }

            HotKeys.Clear();
        }
    }
}
