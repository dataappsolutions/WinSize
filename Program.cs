using System;
using System.Collections.Generic;
using WinSize; // for WinSizeShared

namespace WinSize
{
    public class WindowManager
    {
        public static void Main(string[] args)
        {
            // If arguments are provided, assume direct resize command
            if (args.Length == 3)
            {
                string targetWindowTitle = args[0];

                if (!int.TryParse(args[1], out int newWidth) || newWidth <= 0)
                {
                    Console.WriteLine($"Error: Invalid width value '{args[1]}'. Please provide a positive integer.");
                    return;
                }

                if (!int.TryParse(args[2], out int newHeight) || newHeight <= 0)
                {
                    Console.WriteLine($"Error: Invalid height value '{args[2]}'. Please provide a positive integer.");
                    return;
                }

                // Find the window by title for direct resize (exact match)
                IntPtr hWnd = WinSizeShared.FindWindowByExactTitle(targetWindowTitle);
                if (hWnd == IntPtr.Zero)
                {
                    Console.WriteLine($"Error: Window with title '{targetWindowTitle}' not found.");
                    return;
                }

                if (!WinSizeShared.TryResizeWindow(hWnd, newWidth, newHeight, out int err))
                {
                    Console.WriteLine($"Error: Failed to resize window. Win32 Error Code: {err}");
                    return;
                }

                Console.WriteLine($"Window \"{targetWindowTitle}\" resized successfully to {newWidth}x{newHeight}.");
            }
            else if (args.Length == 0)
            {
                RunInteractive();
            }
            else
            {
                Console.WriteLine("Usage: WinSize.exe <WindowTitle> <Width> <Height>");
                Console.WriteLine("   or: WinSize.exe (to enter interactive mode)");
                Console.WriteLine("Example: WinSize.exe \"Untitled - Notepad\" 800 600");
                return;
            }

            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();
        }

        private static void RunInteractive()
        {
            var windows = WinSizeShared.GetOpenWindows();

            Console.WriteLine("\n--- Open Windows ---");
            if (windows.Count == 0)
            {
                Console.WriteLine("  No visible windows found with titles.");
                return;
            }

            for (int i = 0; i < windows.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. {windows[i]}");
            }
            Console.WriteLine("--------------------\n");

            int selectedIndex = ReadIntInRange($"Enter the number of the window to resize (1-{windows.Count}): ", 1, windows.Count) - 1;
            int newWidth = ReadPositiveInt("Enter the new width (e.g., 800): ");
            int newHeight = ReadPositiveInt("Enter the new height (e.g., 600): ");

            var selected = windows[selectedIndex];

            if (!WinSizeShared.TryResizeWindow(selected.Handle, newWidth, newHeight, out int err))
            {
                Console.WriteLine($"Error: Failed to resize window \"{selected.Title}\". Win32 Error Code: {err}");
                return;
            }

            Console.WriteLine($"Window \"{selected.Title}\" resized successfully to {newWidth}x{newHeight}.");
        }

        private static int ReadPositiveInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? "";
                if (int.TryParse(input, out int value) && value > 0)
                    return value;

                Console.WriteLine("Invalid value. Please enter a positive integer.");
            }
        }

        private static int ReadIntInRange(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? "";
                if (int.TryParse(input, out int value) && value >= min && value <= max)
                    return value;

                Console.WriteLine($"Invalid value. Please enter a number from {min} to {max}.");
            }
        }
    }
}
