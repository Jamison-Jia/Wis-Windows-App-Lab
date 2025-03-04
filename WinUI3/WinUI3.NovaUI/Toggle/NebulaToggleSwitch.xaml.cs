using System;
using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Nebula.UI.CreatorZone.Nebula.UI.Core.Enums;
using Nebula.UI.CreatorZone.Nebula.UI.Core.Styles;
using WinUI3.NovaUI.Helpers;
using Brush = Microsoft.UI.Xaml.Media.Brush;

namespace WinUI3.NovaUI.Controls;

/*
Feature:
    Has a transform animation to switch the imange and the text for a better user experience.

Issue:
    the layout of the image and the text can't be updated properly after system theme changed by the user.
 
 */



/// <summary>
/// ToggleSwitch control for switching status between on and off.
/// </summary>
public sealed partial class NebulaToggleSwitch : Button
{
    #region Fields

    private DateTime _lastClickTime = DateTime.MinValue;
    private readonly TimeSpan _doubleClickTimeLimit = TimeSpan.FromMilliseconds(500);

    #endregion

    #region Events

    public EventHandler<DependencyPropertyChangedEventArgs> IsOnPropertyChanged;

    #endregion

    #region Static Properties

    public static string OnText { get; set; } = "On";
    public static string OffText { get; set; } = "Off";

    #endregion

    #region DependencyProperties

    public static readonly DependencyProperty IsOnProperty =
        DependencyProperty.Register(nameof(IsOn),
                                    typeof(bool),
                                    typeof(NebulaToggleSwitch),
                                    new PropertyMetadata(false, OnIsOnChanged));

    public bool IsOn
    {
        get => (bool)GetValue(IsOnProperty);
        set => SetValue(IsOnProperty, value);
    }

    public static readonly DependencyProperty ImageSourceProperty =
        DependencyProperty.Register(nameof(ImageSource),
            typeof(ImageSource),
            typeof(NebulaToggleSwitch),
            new PropertyMetadata(null));

    public ImageSource ImageSource
    {
        get => (ImageSource)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    public static readonly DependencyProperty BackgroundBrushProperty =
        DependencyProperty.Register(nameof(BackgroundBrush),
                                    typeof(Brush),
                                    typeof(NebulaToggleSwitch),
                                    new PropertyMetadata(null));

    public Brush BackgroundBrush
    {
        get => (Brush)GetValue(BackgroundBrushProperty);
        set => SetValue(BackgroundBrushProperty, value);
    }

    public static readonly DependencyProperty TextBrushProperty =
        DependencyProperty.Register(nameof(TextBrush),
                                    typeof(Brush),
                                    typeof(NebulaToggleSwitch),
                                    new PropertyMetadata(null));

    public Brush TextBrush
    {
        get => (Brush)GetValue(TextBrushProperty);
        set => SetValue(TextBrushProperty, value);
    }

    private static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text),
                                    typeof(string),
                                    typeof(NebulaToggleSwitch),
                                    new PropertyMetadata(string.Empty));
    private string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    #endregion

    #region Commands

    public ICommand IsOnChangedCommand { get; set; }

    #endregion

    #region Constructors

    public NebulaToggleSwitch()
    {
        InitializeComponent();

        NebulaThemeType currentTheme = ThemeHelper.ConvertToNebulaTheme(RequestedTheme);
        ImageSource = ThemeHelper.GetThemeResource<SvgImageSource>("ToggleSwitchOffImage16",
                                                                   currentTheme,
                                                                   null);

        BackgroundBrush = ThemeHelper.GetResourceByKey<Brush>("ButtonGradientBackground1");

        TextBrush = new SolidColorBrush(ThemeHelper.GetResourceByKey<Windows.UI.Color>("TextForegroundUnchecked"));

        Text = OffText;

        IsOnPropertyChanged += OnIsOnPropertyChanged;
        Unloaded += OnUnloaded;
    }

    #endregion

    #region Event Handlers

    private void OnClicked(object sender, RoutedEventArgs e)
    {
        DateTime currentTime = DateTime.Now;
        if (currentTime - _lastClickTime <= _doubleClickTimeLimit)
        {
            return;
        }
        _lastClickTime = currentTime;

        IsOn = !IsOn;

        Debug.WriteLine("Clicked event fired!");
    }

    private static void OnIsOnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NebulaToggleSwitch toggleSwitch)
        {
            return;
        }

        NebulaThemeType currentTheme = ThemeHelper.ConvertToNebulaTheme(toggleSwitch.RequestedTheme);
        toggleSwitch.ImageSource = toggleSwitch.IsOn ?
            ThemeHelper.GetThemeResource<SvgImageSource>("ToggleSwitchOnImage16", currentTheme, null)
            : ThemeHelper.GetThemeResource<SvgImageSource>("ToggleSwitchOffImage16", currentTheme, null);

        toggleSwitch.BackgroundBrush = toggleSwitch.IsOn
            ? ThemeHelper.GetResourceByKey<Brush>("ButtonGradientBackground")
            : ThemeHelper.GetResourceByKey<Brush>("ButtonGradientBackground1");

        toggleSwitch.TextBrush = toggleSwitch.IsOn
            ? new SolidColorBrush(ThemeHelper.GetResourceByKey<Windows.UI.Color>("TextForegroundTran"))
            : new SolidColorBrush(ThemeHelper.GetResourceByKey<Windows.UI.Color>("TextForegroundUnchecked"));

        toggleSwitch.Text = toggleSwitch.IsOn ? OnText : OffText;

        toggleSwitch.IsOnPropertyChanged?.Invoke(d, e);
    }

    private void OnIsOnPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        SwitchImageAndText();
        IsOnChangedCommand?.Execute(e.NewValue);
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        IsOnPropertyChanged -= OnIsOnPropertyChanged;
        Unloaded -= OnUnloaded;
    }

    #endregion

    #region Private Methods

    private void SwitchImageAndText()
    {
        // Calculate the exact widths of the columns at runtime
        double column1Width = UiElementHelpers.GetGridColumnWidth(ContentGrid, 0);
        double column2Width = UiElementHelpers.GetGridColumnWidth(ContentGrid, 1);

        // Determine the current Grid.Column positions of the Image and the text.
        int column1 = Grid.GetColumn(ButtonImage);
        int column2 = Grid.GetColumn(ButtonText);

        // Calculate the target translation distances
        double targetX1;
        double targetX2;
        if (column1 > column2) // (item2, item1)
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

        Animation1.To = targetX1;
        Animation2.To = targetX2;

        SwitchStoryboard.Begin();
    }

    private void OnSwitchStoryboardCompleted(object sender, object e)
    {
        // Reset the transforms
        ImageTransform.X = 0;
        TextTransform.X = 0;

        Animation1.To = 0;
        Animation2.To = 0;

        // Swap the columns
        int column1 = Grid.GetColumn(ButtonImage);
        int column2 = Grid.GetColumn(ButtonText);
        Grid.SetColumn(ButtonImage, column2);
        Grid.SetColumn(ButtonText, column1);
    }

    #endregion
}