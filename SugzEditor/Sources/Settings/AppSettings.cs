using System.Collections.Specialized;
using System.Configuration;
using System.Windows;

namespace SugzEditor.Src.Settings
{
	public sealed class AppSettings : ApplicationSettingsBase
	{
		[UserScopedSetting()]
		[DefaultSettingValue("true")]
		public bool IsFirstRun
		{
			get { return (bool)(this["IsFirstRun"]); }
			set { this["IsFirstRun"] = value; }
		}

		[UserScopedSetting()]
		[DefaultSettingValue("0,0")]
		public Point WindowLocation
		{
			get { return (Point)(this["WindowLocation"]); }
			set { this["WindowLocation"] = value; }
		}

		[UserScopedSetting()]
		[DefaultSettingValue("1280,720")]
		public Size WindowSize
		{
			get { return (Size)(this["WindowSize"]); }
			set { this["WindowSize"] = value; }
		}

		[UserScopedSetting()]
		[DefaultSettingValue("Normal")]
		public WindowState WindowState
		{
			get { return (WindowState)(this["WindowState"]); }
			set { this["WindowState"] = value; }
		}

		[UserScopedSetting()]
		[DefaultSettingValue("true")]
		public bool IsDarktheme
		{
			get { return (bool)(this["IsDarktheme"]); }
			set { this["IsDarktheme"] = value; }
		}

		[UserScopedSetting()]
		[DefaultSettingValue("1")]
		public double Brightness
		{
			get { return (double)(this["Brightness"]); }
			set { this["Brightness"] = value; }
		}

		[UserScopedSetting()]
		[DefaultSettingValue("1")]
		public double Contrast
		{
			get { return (double)(this["Contrast"]); }
			set { this["Contrast"] = value; }
		}

		[UserScopedSetting()]
		[DefaultSettingValue("true")]
		public bool ShowToolbar
		{
			get { return (bool)(this["ShowToolbar"]); }
			set { this["ShowToolbar"] = value; }
		}

		[UserScopedSetting()]
		[DefaultSettingValue("true")]
		public bool ShowExplorer
		{
			get { return (bool)(this["ShowExplorer"]); }
			set { this["ShowExplorer"] = value; }
		}

		[UserScopedSetting()]
		[DefaultSettingValue("300")]
		public double ExplorerGridWidth
		{
			get { return (double)(this["ExplorerGridWidth"]); }
			set { this["ExplorerGridWidth"] = value; }
		}

		[UserScopedSetting()]
		[DefaultSettingValue("true")]
		public bool ShowTabs
		{
			get { return (bool)(this["ShowTabs"]); }
			set { this["ShowTabs"] = value; }
		}

		[UserScopedSetting()]
		public StringCollection Files
		{
			get { return (StringCollection)(this["Files"]); }
			set { this["Files"] = value; }
		}

		[UserScopedSetting()]
		public StringCollection RecentFiles
		{
			get { return (StringCollection)(this["RecentFiles"]); }
			set { this["RecentFiles"] = value; }
		}

		[UserScopedSetting()]
		public StringCollection Folders
		{
			get { return (StringCollection)(this["Folders"]); }
			set { this["Folders"] = value; }
		}

		[UserScopedSetting()]
		public StringCollection RecentFolders
		{
			get { return (StringCollection)(this["RecentFolders"]); }
			set { this["RecentFolders"] = value; }
		}

	}
}
