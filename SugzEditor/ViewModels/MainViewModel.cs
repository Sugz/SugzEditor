using GalaSoft.MvvmLight;
using SugzEditor.Model;
using SugzEditor.Themes;

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


        private bool _IsDarkTheme = true;


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
                if (value != _IsDarkTheme)
                {
                    Set(ref _IsDarkTheme, value);
                    ThemeSelector.SetTheme(value);
                }
            }
        }


        #endregion Properties


        #region RelayCommands

        

        #endregion RelayCommands
    }
}