using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using SugzEditor.Models;
using SugzEditor.Settings;
using SugzEditor.Themes;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

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
        private bool _IsDarkTheme;
        private bool _ShowMenus;
        private bool _ShowToolbar;
        private bool _ShowExplorer;
        private double _SavedExplorerWidth;
        private GridLength _ExplorerGridWidth;
        private bool _ShowTabs;


        #endregion Fields


        #region Properties


        /// <summary>
        /// Get or set if the theme is dark or light
        /// </summary>
        public bool IsDarkTheme
        {
            get => _IsDarkTheme;
            set
            {
                Set(ref _IsDarkTheme, value);
                ThemeSelector.SetTheme(value);
            }
        }

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
                if (value.Value != 0)
                    _SavedExplorerWidth = value.Value;
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

        #endregion Properties


        #region Commands



        #endregion Commands


        #region Constructors


        public MainViewModel()
        {
            LoadSettings();
        } 


        #endregion Constructors


        #region Methods

        /// <summary>
        /// Load user settings
        /// </summary>
        private void LoadSettings()
        {
            _SettingsProvider.Load();

            IsDarkTheme = _SettingsProvider.IsDarktheme;
            ShowMenus = _SettingsProvider.ShowMenus;
            ShowToolbar = _SettingsProvider.ShowToolbar;
            ExplorerGridWidth = new GridLength(_SettingsProvider.ExplorerGridWidth);
            ShowExplorer = _SettingsProvider.ShowExplorer;
            ShowTabs = _SettingsProvider.ShowTabs;
        }

        /// <summary>
        /// Save current properties to user settings
        /// </summary>
        public void SaveSettings()
        {
            _SettingsProvider.IsDarktheme = IsDarkTheme;
            _SettingsProvider.ShowMenus = ShowMenus;
            _SettingsProvider.ShowToolbar = ShowToolbar;
            _SettingsProvider.ShowExplorer = ShowExplorer;
            _SettingsProvider.ExplorerGridWidth = _SavedExplorerWidth;
            _SettingsProvider.ShowTabs = ShowTabs;

            _SettingsProvider.Save();
        }

        /// <summary>
        /// Set the ExplorerGridWidth to show or hide the explorer
        /// </summary>
        private void ShowHideExplorer()
        {
            if (ShowExplorer)
                ExplorerGridWidth = new GridLength(_SavedExplorerWidth);
            else
                ExplorerGridWidth = new GridLength(0);
        }

        #endregion Methods
    }
}