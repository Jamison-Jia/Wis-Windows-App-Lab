using System;
using System.Runtime.InteropServices;
using Microsoft.UI.Xaml;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using Microsoft.UI;
using Windows.Foundation;
using WinRT.Interop;

public static class WinUIWindowHelper
{
    #region Postions

    [DllImport("user32.dll")]
    private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref Rect rect, uint fWinIni);

    [DllImport("user32.dll")]
    private static extern uint GetDpiForWindow(IntPtr hwnd);

    private const uint SPI_GETWORKAREA = 0x0030;

    public static void SetWindowToBottomRight(Window window, int margin = 20)
    {
        var appWindow = GetAppWindow(window);

        // Retrieve the working area of the screen (excluding the taskbar and title bar)
        var displayArea = DisplayArea.GetFromWindowId(appWindow.Id, DisplayAreaFallback.Primary);
        var workingArea = displayArea.WorkArea;

        // Get the size of the window
        var windowSize = appWindow.Size;

        // Calculate the position for the bottom-right corner with a margin
        int x = workingArea.Width - windowSize.Width - margin;
        int y = workingArea.Height - windowSize.Height - margin;

        // Adjust for the position of the working area (offset for multi-monitor setups)
        x += workingArea.X;
        y += workingArea.Y;

        // Apply the calculated position
        appWindow.Move(new Windows.Graphics.PointInt32(x, y));
    }

    // Helper method to retrieve AppWindow
    private static AppWindow GetAppWindow(Window window)
    {
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
        var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
        return AppWindow.GetFromWindowId(windowId);
    }

    #endregion

    #region Styling

    public static void HideFromTaskbarPreview(this Window window)
    {
        // Get the HWND of the current window
        IntPtr hwnd = WindowNative.GetWindowHandle(window);

        // Get the current extended window style
        int currentStyle = GetWindowLong(hwnd, GWL_EXSTYLE);

        // Add the WS_EX_TOOLWINDOW style to hide the window from the taskbar preview
        SetWindowLong(hwnd, GWL_EXSTYLE, currentStyle | WS_EX_TOOLWINDOW);
    }

    // Import GetWindowLong and SetWindowLong from user32.dll
    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    // Constants for window styles
    private const int GWL_EXSTYLE = -20;
    private const int WS_EX_TOOLWINDOW = 0x00000080;

    #endregion
}
