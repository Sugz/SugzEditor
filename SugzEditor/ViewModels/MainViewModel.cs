using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SugzEditor.Model;
using SugzEditor.Settings;
using SugzEditor.Themes;
using System.Windows;

namespace SugzEditor.ViewModels
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {

        #region Fields

        SettingsConfigProvider _SettingsProvider = SimpleIoc.Default.GetInstance<SettingsConfigProvider>();

        private bool _ShowMenus;// = true;
        private bool _ShowToolbar;// = true;
        private bool _ShowExplorer;// = true;
        private double _OldExplorerWidth = 300;
        //private double _ExplorerWidth = 300;
        private GridLength _ExplorerGridWidth = new GridLength(300);
        private bool _ShowTabs;// = true;
        private bool _IsDarkTheme;// = true;

        #endregion Fields


        #region Properties


        /// <summary>
        /// Get or set the menus visibility 
        /// </summary>
        public bool ShowMenus
        {
            get => _ShowMenus;
            set => Set(ref _ShowMenus, value);
        }

        /// <summary>
        /// Get or set the toolbar visibility
        /// </summary>
        public bool ShowToolbar
        {
            get => _ShowToolbar;
            set => Set(ref _ShowToolbar, value);
        }

        /// <summary>
        /// Get or set the explorer visibility
        /// </summary>
        public bool ShowExplorer
        {
            get => _ShowExplorer;
            set
            {
                Set(ref _ShowExplorer, value);
                ShowHideExplorer();
            }
        }

        /// <summary>
        /// Get or set the explorer grid column width
        /// </summary>
        public GridLength ExplorerGridWidth
        {
            get => _ExplorerGridWidth;
            set
            {
                Set(ref _ExplorerGridWidth, value);
                //_ExplorerWidth = value.Value;
                if (value.Value != 0)
                    _OldExplorerWidth = value.Value;
            }
        }

        /// <summary>
        /// Get or set the tabs visibility 
        /// </summary>
        public bool ShowTabs
        {
            get => _ShowTabs;
            set => Set(ref _ShowTabs, value);
        }

        /// <summary>
        /// Get or set if the theme is dark or light
        /// </summary>
        public bool IsDarkTheme
        {
            get => _IsDarkTheme;
            set
            {
                if (value != _IsDarkTheme)
                {
                    Set(ref _IsDarkTheme, value);
                    ThemeSelector.SetTheme(value);
                    //SaveConfig();
                }
            }
        }

        #endregion Properties


        #region RelayCommands



        #endregion RelayCommands


        public MainViewModel()
        {
            LoadConfig();
        }


        #region Methods

        /// <summary>
        /// Load user settings
        /// </summary>
        void LoadConfig()
        {
            _SettingsProvider.Load();

            IsDarkTheme = _SettingsProvider.IsDarktheme;
            ShowMenus = _SettingsProvider.ShowMenus;
            ShowToolbar = _SettingsProvider.ShowToolbar;
            ShowExplorer = _SettingsProvider.ShowExplorer;
            ShowTabs = _SettingsProvider.ShowTabs;
        }

        public void SaveConfig()
        {
            _SettingsProvider.IsDarktheme = IsDarkTheme;
            _SettingsProvider.ShowMenus = ShowMenus;
            _SettingsProvider.ShowToolbar = ShowToolbar;
            _SettingsProvider.ShowExplorer = ShowExplorer;
            _SettingsProvider.ShowTabs = ShowTabs;

            _SettingsProvider.Save();
        }

        private void ShowHideExplorer()
        {
            if (ShowExplorer)
                ExplorerGridWidth = new GridLength(_OldExplorerWidth);
            else
                ExplorerGridWidth = new GridLength(0);
            //if (_ExplorerWidth == 0)
            //    ExplorerGridWidth = new GridLength(_OldExplorerWidth);
            //else
            //    ExplorerGridWidth = new GridLength(0);
        }

        #endregion Methods
    }
}