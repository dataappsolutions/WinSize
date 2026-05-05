using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace WinSize
{
    public static class WinSizeShared
    {
        // --- P/Invoke ---
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string? lpClassName, string? lpWindowName);

        private const uint SWP_NOMOVE = 0x0002;

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsZoomed(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);

        private const int SM_CXSCREEN = 0;
        private const int SM_CYSCREEN = 1;
        private const int SW_RESTORE = 9;
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_NOACTIVATE = 0x0010;
        private const uint SWP_ASYNCWINDOWPOS = 0x4000;



        // --- Data ---
        public sealed class WindowInfo
        {
            public IntPtr Handle { get; init; }
            public string Title { get; init; } = "";
            public string ProcessName { get; init; } = "";
            public uint ProcessId { get; init; }

            public override string ToString() => $"\"{Title}\"  ({ProcessName}, PID {ProcessId})";
        }

        // --- API used by both CLI + WinForms ---
        public static IReadOnlyList<WindowInfo> GetOpenWindows()
        {
            var windows = new List<WindowInfo>();

            EnumWindows((hWnd, lParam) =>
            {
                if (!IsWindowVisible(hWnd))
                    return true;

                int length = GetWindowTextLength(hWnd);
                if (length <= 0)
                    return true;

                var sb = new StringBuilder(length + 1);
                GetWindowText(hWnd, sb, sb.Capacity);
                string title = sb.ToString();

                if (string.IsNullOrWhiteSpace(title))
                    return true;

                GetWindowThreadProcessId(hWnd, out uint pid);

                string processName = "N/A";
                try { processName = Process.GetProcessById((int)pid).ProcessName; } catch { }

                windows.Add(new WindowInfo
                {
                    Handle = hWnd,
                    Title = title,
                    ProcessName = processName,
                    ProcessId = pid
                });

                return true;
            }, IntPtr.Zero);

            return windows;
        }

        public static bool TryResizeWindow(IntPtr hWnd, int width, int height, out int win32Error, bool centerWindow = false)
        {
            win32Error = 0;

            if (hWnd == IntPtr.Zero || width <= 0 || height <= 0)
            {
                win32Error = unchecked((int)0x80070057);
                return false;
            }

            // If minimized or maximized, restore so resize takes effect visibly.
            if (IsIconic(hWnd) || IsZoomed(hWnd))
            {
                ShowWindow(hWnd, SW_RESTORE);
            }

            uint flags = SWP_NOZORDER | SWP_NOACTIVATE | SWP_ASYNCWINDOWPOS;
            int x = 0, y = 0;

            if (centerWindow)
            {
                int screenW = GetSystemMetrics(SM_CXSCREEN);
                int screenH = GetSystemMetrics(SM_CYSCREEN);
                x = (screenW - width) / 2;
                y = (screenH - height) / 2;
            }
            else
            {
                flags |= SWP_NOMOVE;
            }

            bool ok = SetWindowPos(hWnd, IntPtr.Zero, x, y, width, height, flags);

            if (!ok)
                win32Error = Marshal.GetLastWin32Error();

            return ok;
        }


        // (Optional) for your existing CLI direct mode
        public static IntPtr FindWindowByExactTitle(string title) => FindWindow(null, title);
    }
}

