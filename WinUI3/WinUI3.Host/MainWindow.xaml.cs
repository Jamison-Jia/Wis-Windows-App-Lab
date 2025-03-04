using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WindowTests.NewFolder;
using WindowTests.ViewModels;
using WindowTests.Windows.Animation;
using WindowTests.Windows.Events.Mouse;
using Windows.Win32;
using System.Runtime.InteropServices;
using H.NotifyIcon;
using WinRT.Interop;
using Microsoft.UI;
using WindowsPoint = Windows.Foundation.Point;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WindowTests;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    private MenuFlyout _contextMenu;
    private MainWindowViewModel _viewModel;

    public MainWindow()
    {
        _viewModel = new();

        this.InitializeComponent();

        InitializeContextMenu();

        RootContent.DataContext = _viewModel;

        // Make the window frameless
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
        var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
        var appWindow = AppWindow.GetFromWindowId(windowId);

        var presenter = appWindow.Presenter as OverlappedPresenter;
        if (presenter != null)
        {
            //presenter.SetBorderAndTitleBar(false, false); // Remove borders and title bar
        }

        // Set corner radius for the window
        //SetWindowCornerRadius(this, 80); // 20 is the corner radius
    }

    private void myButton_Click(object sender, RoutedEventArgs e)
    {
        //myButton.Content = "Clicked";
    }
    private void OpenListViewTestWindow(object sender, RoutedEventArgs e)
    {
        //myButton.Content = "Clicked";
        ListViewTestWindow window = new ListViewTestWindow();
        window.Activate();
    }

    private void SetWindowCornerRadius(Window window, float radius)
    {
        // Get the visual root of the window's content
        var compositor = Microsoft.UI.Xaml.Hosting.ElementCompositionPreview.GetElementVisual(window.Content).Compositor;

        // Create a rounded rectangle geometry
        var roundedRectangle = compositor.CreateSpriteShape();
        var geometry = compositor.CreateRoundedRectangleGeometry();
        geometry.CornerRadius = new System.Numerics.Vector2(radius);

        roundedRectangle.Geometry = geometry;

        // Create a ShapeVisual to apply the geometry
        var shapeVisual = compositor.CreateShapeVisual();
        shapeVisual.Shapes.Add(roundedRectangle);
        shapeVisual.Size = new System.Numerics.Vector2((float)window.Bounds.Width, (float)window.Bounds.Height);

        // Set the visual to the window content
        Microsoft.UI.Xaml.Hosting.ElementCompositionPreview.SetElementChildVisual(window.Content, shapeVisual);
    }

    private void OnSwitchImageAndTextButtonClicked(object sender, RoutedEventArgs e)
    {
        Window win = new AnimationWindow();
        win.Activate();
    }

    private void OnEventsButtonClicked(object sender, RoutedEventArgs e)
    {
        Window window = new MouseEventsWindow();
        window.Activate();
    }

    private void NotificationButton_Click(object sender, RoutedEventArgs e)
    {
        var win = new NotificationWindow();
        win.Activate();
    }

    private void InitializeContextMenu()
    {
        _contextMenu = new MenuFlyout();

        // Add menu items
        var openMenuItem = new MenuFlyoutItem { Text = "Open" };
        openMenuItem.Click += (s, e) => ShowWindow();

        var exitMenuItem = new MenuFlyoutItem { Text = "Exit" };
        exitMenuItem.Click += (s, e) => CloseApp();

        _contextMenu.Items.Add(openMenuItem);
        _contextMenu.Items.Add(exitMenuItem);
    }

    private IntPtr GetIconHandle()
    {
        // Load your icon here (replace with actual icon loading)
        return SystemIcons.Application.Handle;
    }

    private void ShowWindow()
    {
        DispatcherQueue.TryEnqueue(() => this.Show());
    }

    private void ShowContextMenu()
    {
        DispatcherQueue.TryEnqueue(() =>
        {
            // Implement WinUI3 context menu here
            // Show the context menu at the cursor position
            _contextMenu.XamlRoot = this.Content.XamlRoot;
            var hwnd = WindowNative.GetWindowHandle(this);
            //var cursorPos = GetCursorPosition();
            //_contextMenu.ShowAt(null, cursorPos);


            var dummyButton = new Button();
            dummyButton.XamlRoot = this.Content.XamlRoot;
            _contextMenu.ShowAt(dummyButton);
        });
    }

    // Add to MainWindow class
    private const int WM_USER = 0x0400;
    private const int WM_TRAYICON = WM_USER + 1;

    [DllImport("user32.dll")]
    private static extern IntPtr DefWindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

    // Required interop declarations
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate IntPtr WNDPROC(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    private const int GWLP_WNDPROC = -4;


    private void CloseApp()
    {
        DispatcherQueue.TryEnqueue(() => this.Close());
    }

    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(out POINT lpPoint);

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT
    {
        public int X;
        public int Y;
    }

    public static WindowsPoint GetCursorPosition()
    {
        if (GetCursorPos(out POINT point))
        {
            return new WindowsPoint(point.X, point.Y);
        }
        return new WindowsPoint(0, 0); // Return (0,0) if retrieval fails
    }

    private void OnTrayIconContextMenuOpening(object sender, object e)
    {
        // Not working
        _viewModel.UpdateIsTargetProcessRunning();
    }

    [RelayCommand]
    private void TrayIconRightClick()
    {
        _viewModel.UpdateIsTargetProcessRunning();

        if(_viewModel.IsTargetProcessRunning)
        {
            if(!TrayIconContextMenu.Items.Contains(TargetProcessMenuItem))
            {
                TrayIconContextMenu.Items.Insert(0, TargetProcessMenuItem);
            }
        }
        else
        {
            if (TrayIconContextMenu.Items.Contains(TargetProcessMenuItem))
            {
                TrayIconContextMenu.Items.Remove(TargetProcessMenuItem);
            }
        }
    }

    private void OnTrayIconContextMenuOpened(object sender, object e)
    {
        // Not working
        _viewModel.UpdateIsTargetProcessRunning();
    }
}
