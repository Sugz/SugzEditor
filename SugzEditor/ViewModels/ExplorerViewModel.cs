using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace SugzEditor.ViewModels
{
    public class ExplorerViewModel : ViewModelBase
    {
        /*
        #region Fields


        private double _OldExplorerWidth = 300;
        private double _ExplorerWidth = 300;
        private GridLength _ExplorerGridWidth = new GridLength(300);
        private bool _ShowExplorer = true;
        private bool _ShowToolbar = true;

        #endregion Fields



        #region Properties


        /// <summary>
        /// Get or set the explorer grid column width
        /// </summary>
        public GridLength ExplorerGridWidth
        {
            get => _ExplorerGridWidth;
            set
            {
                Set(ref _ExplorerGridWidth, value);
                _ExplorerWidth = value.Value;
                if (value.Value != 0)
                    _OldExplorerWidth = value.Value;
            }
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
        /// Get or set the toolbar visibility
        /// </summary>
        public bool ShowToolbar
        {
            get => _ShowToolbar;
            set => Set(ref _ShowToolbar, value);
        }



        #endregion Properties



        #region Methods


        private void ShowHideExplorer()
        {
            if (_ExplorerWidth == 0)
                ExplorerGridWidth = new GridLength(_OldExplorerWidth);
            else
                ExplorerGridWidth = new GridLength(0);
        } 


        #endregion Methods
        */
    }
}
