using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SugzEditor.Themes
{
    public static class IconProvider<T>
    {
        public static T Get(string key)
        {
            ResourceDictionary resource = new ResourceDictionary();
            resource.Source = new Uri(("/Themes/Icons/Icons.xaml"), UriKind.RelativeOrAbsolute);
            return (T)resource[key];
        }
    }
}
