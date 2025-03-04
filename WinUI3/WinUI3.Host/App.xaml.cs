using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Nebula.UI.CreatorZone.Nebula.UI.Core.Styles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WindowTests
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private Window m_window;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();

            UnhandledException += OnUnhandledExceptionThrowed;

            //throw new NotImplementedException();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            //throw new NotImplementedException();

            //AddResouceDicts();
            ThemeHelper.AddThemeResources(Current);

            m_window = new MainWindow();

            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(m_window);

            m_window.Activate();

            // Dock the window to the bottom-right corner
            //WinUIWindowHelper.SetWindowToBottomRight(m_window, 50);
        }

        private void OnUnhandledExceptionThrowed(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
        {
#if DEBUG
            Debugger.Break();
#endif
            e.Handled = true;
        }
    }
}
