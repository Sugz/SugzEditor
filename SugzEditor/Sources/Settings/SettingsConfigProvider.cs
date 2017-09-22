using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SugzEditor.Settings
{
    public class SettingsConfigProvider
    {
        AppSettings _Settings = AppSettingsLocator.Instance;

        public bool IsFirstRun { get; set; }
        public WindowState WindowState { get; set; }
        public Point WindowLocation { get; set; }
        public Size WindowSize { get; set; }
        public bool IsDarktheme { get; set; }
        public bool ShowMenus { get; set; }
        public bool ShowToolbar { get; set; }
        public bool ShowExplorer { get; set; }
        public double ExplorerGridWidth { get; set; }
        public bool ShowTabs { get; set; }
        public StringCollection RecentFolders { get; set; }
        public StringCollection RecentFiles { get; set; }

        public void Load()
        {
            IsFirstRun = _Settings.IsFirstRun;
            WindowState = _Settings.WindowState;
            WindowLocation = _Settings.WindowLocation;
            WindowSize = _Settings.WindowSize;
            IsDarktheme = _Settings.IsDarktheme;
            ShowMenus = _Settings.ShowMenus;
            ShowToolbar = _Settings.ShowToolbar;
            ShowExplorer = _Settings.ShowExplorer;
            ExplorerGridWidth = _Settings.ExplorerGridWidth;
            ShowTabs = _Settings.ShowTabs;
            RecentFolders = _Settings.RecentFolders;
            RecentFiles = _Settings.RecentFiles;
        }

        public void Save()
        {
            _Settings.IsFirstRun = false;

            _Settings.WindowState = WindowState;
            _Settings.WindowLocation = WindowLocation;
            _Settings.WindowSize = WindowSize;
            _Settings.IsDarktheme = IsDarktheme;
            _Settings.ShowMenus = ShowMenus;
            _Settings.ShowToolbar = ShowToolbar;
            _Settings.ShowExplorer = ShowExplorer;
            _Settings.ExplorerGridWidth = ExplorerGridWidth;
            _Settings.ShowTabs = ShowTabs;
            _Settings.RecentFolders = RecentFolders;
            _Settings.RecentFiles = RecentFiles;

            _Settings.Save();
        }
    }
}
