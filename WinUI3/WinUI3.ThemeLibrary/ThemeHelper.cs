using Microsoft.UI.Xaml;
using Nebula.UI.CreatorZone.Nebula.UI.Core.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.UI;

namespace Nebula.UI.CreatorZone.Nebula.UI.Core.Styles;

/// <summary>
/// The helper class for theme resources with some useful methods for accessing specific resources by given key.
/// </summary>
public static class ThemeHelper
{
    /// <summary>
    /// Get the resource by the key.
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public static T GetResourceByKey<T>(string key)
    {
        if (Application.Current?.Resources.TryGetValue(key, out object style) == true)
        {
            if (style is T value)
            {
                return value;
            }
        }
#if DEBUG
        Debugger.Break();
#endif
        throw new KeyNotFoundException ($"Didn't find the resource by key = {nameof(key)}");
    }

    /// <summary>
    /// Get the resource explicitly from the specific theme by the given key.
    /// </summary>
    /// <param name="key">resource key</param>
    /// <param name="themeType">theme type</param>
    /// <param name="defaultValue">default color value</param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public static T GetThemeResource<T>(string key, NebulaThemeType themeType, T defaultValue)
    {
        if (!Application.Current.Resources.ThemeDictionaries.TryGetValue(themeType.ToString(), out object themeResource) ||
            themeResource is not ResourceDictionary themeDictionary)
        {
#if DEBUG
            // Didn't find the specific theme resource by the themeType.
            Debugger.Break();
#endif
            return defaultValue;
        }
        if (themeDictionary.TryGetValue(key, out object value) && value is T result)
        {
            return result;
        }
#if DEBUG
        // Didn't find the resource by the key.
        Debugger.Break();
#endif
        return defaultValue;
    }

    public static NebulaThemeType ConvertToNebulaTheme(ElementTheme currentTheme)
    {
        return currentTheme switch
               {
                   ElementTheme.Light => NebulaThemeType.Light,
                   ElementTheme.Dark => NebulaThemeType.Dark,
                   _ => NebulaThemeType.Light
               };
    }

    public static void AddThemeResources(Application app)
    {
        app.Resources.ThemeDictionaries.Add("Light", new ResourceDictionary()
        {
            Source = new Uri("ms-appx:///WinUI3.ThemeLibrary/LightTheme.xaml")
        });
        app.Resources.ThemeDictionaries.Add("Dark", new ResourceDictionary()
        {
            Source = new Uri("ms-appx:///WinUI3.ThemeLibrary/DarkTheme.xaml")
        });


        app.Resources.MergedDictionaries.Add(new ResourceDictionary()
        {
            Source = new Uri("ms-appx:///WinUI3.ThemeLibrary/WinUI3Theme.xaml")
        });

        app.Resources.MergedDictionaries.Add(new ResourceDictionary()
        {
            Source = new Uri("ms-appx:///WinUI3.ThemeLibrary/Resources/Styles/TextBlockResources.xaml")
        });
    }
}