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
                colors.Source = new Uri("/Themes/Colors/DarkColors.xaml", UriKind.Relative);
            }
            else
            {
                colors.Source = new Uri("/Themes/Colors/LightColors.xaml", UriKind.Relative);
            }

        }
    }
}
