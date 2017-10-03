using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SugzEditor.Themes
{
    public class ThemeSelector
    {
        public static void SetTheme(bool isDark)
        {
            ResourceDictionary colors = Application.Current.Resources.MergedDictionaries[0];
            ResourceDictionary icons = Application.Current.Resources.MergedDictionaries[1];
            if (isDark)
            {
                colors.Source = new Uri("/Themes/Dark/DarkColor.xaml", UriKind.Relative);
                icons.Source = new Uri("/Themes/Dark/DarkIcons.xaml", UriKind.Relative);
            }
            else
            {
                colors.Source = new Uri("/Themes/Light/LightColor.xaml", UriKind.Relative);
                icons.Source = new Uri("/Themes/Light/LightIcons.xaml", UriKind.Relative);
            }
                
        }
    }
}
