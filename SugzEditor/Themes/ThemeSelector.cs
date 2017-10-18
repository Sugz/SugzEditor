using SugzEditor.Themes.Colors;
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
            //ResourceDictionary colors = Application.Current.Resources.MergedDictionaries[0];
            //ResourceDictionary icons = Application.Current.Resources.MergedDictionaries[1];
            if (isDark)
            {
				ColorHelper._Colors.Source = new Uri("/Themes/Colors/DarkColors.xaml", UriKind.Relative);
				//colors.Source = new Uri("/Themes/Colors/DarkColors.xaml", UriKind.Relative);
            }
            else
            {
				ColorHelper._Colors.Source = new Uri("/Themes/Colors/LightColors.xaml", UriKind.Relative);
				//colors.Source = new Uri("/Themes/Colors/LightColors.xaml", UriKind.Relative);
			}

        }
    }
}
