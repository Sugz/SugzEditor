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
        private Timer _Timer = new Timer(10);

        private double _OldExplorerWidth = 300;
        private double _ExplorerWidth = 300;
        private GridLength _ExplorerGridWidth = new GridLength(300);
        private Visibility _ExplorerVisibilty;
        private bool _ShowExplorer = true;


        /// <summary>
        /// 
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
        /// 
        /// </summary>
        public Visibility ExplorerVisibilty
        {
            get => _ExplorerVisibilty;
            set => Set(ref _ExplorerVisibilty, value);
        }


        /// <summary>
        /// 
        /// </summary>
        public bool ShowExplorer
        {
            get => _ShowExplorer;
            set
            {
                Set(ref _ShowExplorer, value);
                DoSomething();
            }
        }



        private void DoSomething()
        {
            if (_ExplorerWidth == 0)
            {
                ExplorerVisibilty = Visibility.Visible;
                ExplorerGridWidth = new GridLength(_OldExplorerWidth);
            }
            else
            {
                ExplorerVisibilty = Visibility.Collapsed;
                ExplorerGridWidth = new GridLength(0);
            }
        }
    }
}
