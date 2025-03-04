using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using WinUI3.NovaUI.Helpers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WindowTests.Windows.Animation
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AnimationWindow : Window
    {
        public AnimationWindow()
        {
            this.InitializeComponent();
        }

        private void OnAnimateClick(object sender, RoutedEventArgs e)
        {
            //ContinuslySwitch();

            AutoColumnSwitches();
        }

        private void AutoColumnSwitches()
        {
            // Calculate the exact widths of the columns at runtime
            double column1Width = UiElementHelpers.GetGridColumnWidth(SwitchButtonContent, 0);
            double column2Width = UiElementHelpers.GetGridColumnWidth(SwitchButtonContent, 1);

            // Determine the current Grid.Column positions of the buttons
            int column1 = Grid.GetColumn(Button1);
            int column2 = Grid.GetColumn(Button2);

            // Calculate the target translation distances
            double targetX1 = column2 > column1 ? column2Width : -column1Width; // Button1 moves to Button2's position
            double targetX2 = column1 > column2 ? column1Width : -column2Width; // Button2 moves to Button1's position
            if(column1 > column2) // (item2, item1)
            {
                // 1->left, 2->right
                targetX1 = -column1Width;
                targetX2 = column2Width;
            }
            else // (item1, item2)
            {
                // 1->right, 2->left;
                targetX1 = column2Width;
                targetX2 = -column1Width;

            }


            // Create the animations
            var animation1 = new DoubleAnimation
            {
                To = targetX1,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };

            var animation2 = new DoubleAnimation
            {
                To = targetX2,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };

            // Create a storyboard
            var storyboard = new Storyboard();

            // Add Button1's animation
            Storyboard.SetTarget(animation1, Transform1);
            Storyboard.SetTargetProperty(animation1, "X");
            storyboard.Children.Add(animation1);

            // Add Button2's animation
            Storyboard.SetTarget(animation2, Transform2);
            Storyboard.SetTargetProperty(animation2, "X");
            storyboard.Children.Add(animation2);

            // Handle storyboard completion
            storyboard.Completed += (s, args) =>
            {
                // Reset the transforms
                Transform1.X = 0;
                Transform2.X = 0;

                // Swap the columns
                Grid.SetColumn(Button1, column2);
                Grid.SetColumn(Button2, column1);
            };

            // Start the animation
            storyboard.Begin();
        }
    }
}
