using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI3.NovaUI.Buttons
{
    public sealed partial class NovaImageTextToggleSwitch : Button
    {
        public NovaImageTextToggleSwitch()
        {
            this.InitializeComponent();
        }

        private void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            SwitchContentItems();
        }

        private async void SwitchContentItems()
        {
            await Task.Delay(400);
            int imageColumn = Grid.GetColumn(this.Image);
            int textColoumn = Grid.GetColumn(this.Text);
            Grid.SetColumn(this.Image, textColoumn);
            Grid.SetColumn(this.Text, imageColumn);
        }
    }
}
