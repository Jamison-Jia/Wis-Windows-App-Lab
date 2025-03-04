using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Media.Animation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WindowTests.Windows.Animation
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NotificationWindow : Window
    {
        public NotificationWindow()
        {
            this.InitializeComponent();

            // Apply the TranslateTransform to the Window's content (Grid)
            var translateTransform = new TranslateTransform();
            this.Content.RenderTransform = translateTransform;

            // Position the window off-screen (to the right and below)
            translateTransform.X = this.Bounds.Width; // Off the right
            translateTransform.Y = this.Bounds.Height; // Off the bottom

            // Start the animation after the window is shown
            this.Activated += (sender, args) => AnimateWindow(translateTransform);
        }

        private void AnimateWindow(TranslateTransform translateTransform)
        {
            // Define the animations for X and Y translation
            DoubleAnimation xAnimation = new DoubleAnimation
            {
                From = this.Bounds.Width, // Start from off-screen (right)
                To = 0, // End at visible position (0)
                Duration = new TimeSpan(0, 0, 2) // 2 seconds duration
            };

            DoubleAnimation yAnimation = new DoubleAnimation
            {
                From = this.Bounds.Height, // Start from off-screen (bottom)
                To = 0, // End at visible position (0)
                Duration = new TimeSpan(0, 0, 2) // 2 seconds duration
            };

            // Create a Storyboard to control the animations
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(xAnimation);
            storyboard.Children.Add(yAnimation);

            // Set the target properties for the animations
            Storyboard.SetTarget(xAnimation, translateTransform);
            Storyboard.SetTargetProperty(xAnimation, "X");

            Storyboard.SetTarget(yAnimation, translateTransform);
            Storyboard.SetTargetProperty(yAnimation, "Y");

            // Start the animation
            storyboard.Begin();
        }
    }
}
