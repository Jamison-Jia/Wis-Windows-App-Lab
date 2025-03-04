using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace WinUI3.NovaUI.Controls;

public sealed partial class NebulaToggleSwitchNew : Button
{

    private Image _leftImage, _rightImage;
    private TextBlock _leftText, _rightText;
    public bool IsOn
    {
        get { return (bool)GetValue(IsOnProperty); }
        set { SetValue(IsOnProperty, value); }
    }

    public static readonly DependencyProperty IsOnProperty =
        DependencyProperty.Register("IsOn", typeof(bool), typeof(NebulaToggleSwitchNew), new PropertyMetadata(false));

    
    public NebulaToggleSwitchNew()
    {
        InitializeComponent();
    }

    protected override void OnApplyTemplate()
    {
        _leftImage = GetTemplateChild("LeftImage") as Image;
        _rightImage = GetTemplateChild("RightImage") as Image;
        _leftText = GetTemplateChild("LeftText") as TextBlock;
        _rightText = GetTemplateChild("RightText") as TextBlock;
        
        base.OnApplyTemplate();
    }

    private async void ToggleButton_Click(object sender, RoutedEventArgs e)
    {
        // Toggle the IsOn property
        IsOn = !IsOn;

        // Change the visual state based on IsOn value
        //VisualStateManager.GoToState((Control)sender, IsOn ? "On" : "Off", true);

        if (IsOn)
        {
            VisualStateManager.GoToState((Control)sender,"On" , true);
            await Task.Delay(500);
            _rightImage.Visibility = Visibility.Visible;
            _leftText.Visibility = Visibility.Visible;
            
            _leftImage.Visibility = Visibility.Collapsed;
            _rightText.Visibility = Visibility.Collapsed;
        }
        else
        {
            VisualStateManager.GoToState((Control)sender, "Off", true);
            await Task.Delay(500);
            
            _leftImage.Visibility = Visibility.Visible;
            _rightText.Visibility = Visibility.Visible;
            
            _leftText.Visibility = Visibility.Collapsed;
            _rightImage.Visibility = Visibility.Collapsed;
        }
    }
}
