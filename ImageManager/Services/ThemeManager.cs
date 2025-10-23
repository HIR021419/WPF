using System;
using System.Linq;
using System.Windows;

namespace ImageManager.Services
{
    public static class ThemeManager
    {
        public static void Apply(bool useDark)
        {
            var appRes = Application.Current.Resources.MergedDictionaries;
            var newTheme = new ResourceDictionary
            {
                Source = new Uri(useDark
                    ? "Themes/Colors.Dark.xaml"
                    : "Themes/Colors.Light.xaml",
                    UriKind.Relative)
            };
            if (appRes.Count > 0)
                appRes[0] = newTheme;
            else
                appRes.Add(newTheme);
        }
    }
}
