using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SugzEditor.Src.Settings
{
	public class SettingsConfigProvider
	{
		AppSettings _Settings = AppSettingsLocator.Instance;

		public bool IsFirstRun { get; set; }
		public WindowState WindowState { get; set; }
		public Point WindowLocation { get; set; }
		public Size WindowSize { get; set; }
		public bool IsDarktheme { get; set; }
		public double Brightness { get; set; }
		public double Contrast { get; set; }
		public bool ShowToolbar { get; set; }
		public bool ShowExplorer { get; set; }
		public double ExplorerGridWidth { get; set; }
		public bool ShowTabs { get; set; }
		public StringCollection Files { get; set; }
		public StringCollection RecentFiles { get; set; }
		public StringCollection Folders { get; set; }
		public StringCollection RecentFolders { get; set; }
		

		public void Load()
		{
			if (_Settings.IsFirstRun)
				_Settings.Upgrade();

			IsFirstRun = _Settings.IsFirstRun;
			WindowState = _Settings.WindowState;
			WindowLocation = _Settings.WindowLocation;
			WindowSize = _Settings.WindowSize;
			IsDarktheme = _Settings.IsDarktheme;
			Brightness = _Settings.Brightness;
			Contrast = _Settings.Contrast;
			ShowToolbar = _Settings.ShowToolbar;
			ShowExplorer = _Settings.ShowExplorer;
			ExplorerGridWidth = _Settings.ExplorerGridWidth;
			ShowTabs = _Settings.ShowTabs;
			Files = _Settings.Files;
			RecentFiles = _Settings.RecentFiles;
			Folders = _Settings.Folders;
			RecentFolders = _Settings.RecentFolders;
		}

		public void Save()
		{
			_Settings.IsFirstRun = false;
			_Settings.WindowState = WindowState;
			_Settings.WindowLocation = WindowLocation;
			_Settings.WindowSize = WindowSize;
			_Settings.IsDarktheme = IsDarktheme;
			_Settings.Brightness = Brightness;
			_Settings.Contrast = Contrast;
			_Settings.ShowToolbar = ShowToolbar;
			_Settings.ShowExplorer = ShowExplorer;
			_Settings.ExplorerGridWidth = ExplorerGridWidth;
			_Settings.ShowTabs = ShowTabs;
			_Settings.Files = Files;
			_Settings.RecentFiles = RecentFiles;
			_Settings.Folders = Folders;
			_Settings.RecentFolders = RecentFolders;

			_Settings.Save();
		}
	}
}
