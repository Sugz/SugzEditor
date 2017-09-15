using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugzEditor.Settings
{
    public class SettingsConfigProvider
    {
        AppSettings _Settings = AppSettingsLocator.Instance;

        public bool IsDarktheme { get; set; }
        public bool ShowMenus { get; set; }
        public bool ShowToolbar { get; set; }
        public bool ShowExplorer { get; set; }
        public bool ShowTabs { get; set; }

        public void Load()
        {
            IsDarktheme = _Settings.IsDarktheme;
            ShowMenus = _Settings.ShowMenus;
            ShowToolbar = _Settings.ShowToolbar;
            ShowExplorer = _Settings.ShowExplorer;
            ShowTabs = _Settings.ShowTabs;
        }

        public void Save()
        {
            _Settings.IsDarktheme = IsDarktheme;
            _Settings.ShowMenus = ShowMenus;
            _Settings.ShowToolbar = ShowToolbar;
            _Settings.ShowExplorer = ShowExplorer;
            _Settings.ShowTabs = ShowTabs;

            _Settings.Save();
        }
    }
}
