using GalaSoft.MvvmLight.Ioc;
using SugzEditor.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace SugzEditor.Controls
{
    public class SgzWindow : Window
    {
        #region Fields


        private SettingsConfigProvider _SettingsProvider;
        private bool _IsLoaded;


        #endregion Fields


        #region Constructor


        static SgzWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SgzWindow), new FrameworkPropertyMetadata(typeof(SgzWindow)));
        }
        public SgzWindow()
        {
            CommandBindings.Add(new CommandBinding(SystemCommands.ShowSystemMenuCommand, OnShowSystemMenu));
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, OnCloseWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, OnMaximizeWindow, OnCanResizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, OnMinimizeWindow, OnCanMinimizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, OnRestoreWindow, OnCanResizeWindow));

            Loaded += delegate { _IsLoaded = true; };
            Closed += delegate { _SettingsProvider.Save(); };
            LoadSettings();
        } 


        #endregion Constructor


        #region System commands


        private void OnCanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode != ResizeMode.NoResize;
        }

        private void OnCanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void OnShowSystemMenu(object sender, ExecutedRoutedEventArgs e)
        {
            Point pointToWindow = Mouse.GetPosition(this);
            Point pointToScreen = PointToScreen(pointToWindow);
            SystemCommands.ShowSystemMenu(this, pointToScreen);
        }

        private void OnCloseWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        private void OnMaximizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        private void OnMinimizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void OnRestoreWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }


        #endregion System commands


        #region Store window settings


        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);

            // We need to delay this call because we are 
            // notified of a location change before a 
            // window state change.  That causes a problem 
            // when maximizing the window because we record 
            // the maximized window's location, which is not 
            // something worth saving.
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new ThreadStart(
                delegate
                {
                    if (_IsLoaded && WindowState == WindowState.Normal)
                        _SettingsProvider.WindowLocation = new Point(Left, Top);
                }
            ));
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo info)
        {
            base.OnRenderSizeChanged(info);

            if (_IsLoaded && WindowState == WindowState.Normal)
                _SettingsProvider.WindowSize = RenderSize;
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);

            // We don't want the Window to open in the 
            // minimized state, so ignore that value.
            if (_IsLoaded)
                _SettingsProvider.WindowState = WindowState == WindowState.Maximized ? WindowState : WindowState.Normal;
        } 


        #endregion Store window settings


        private void LoadSettings()
        {
            SimpleIoc.Default.Register<SettingsConfigProvider>();
            _SettingsProvider = SimpleIoc.Default.GetInstance<SettingsConfigProvider>();
            _SettingsProvider.Load();

            Size sz = _SettingsProvider.WindowSize;
            Width = sz.Width;
            Height = sz.Height;

            Point loc = _SettingsProvider.WindowLocation;

            // If the user's machine had two monitors but now only
            // has one, and the Window was previously on the other
            // monitor, we need to move the Window into view.
            bool outOfBounds =
                loc.X <= -sz.Width ||
                loc.Y <= -sz.Height ||
                SystemParameters.VirtualScreenWidth <= loc.X ||
                SystemParameters.VirtualScreenHeight <= loc.Y;

            if (_SettingsProvider.IsFirstRun || outOfBounds)
                WindowStartupLocation = WindowStartupLocation.CenterScreen;
            else
            {
                WindowStartupLocation = WindowStartupLocation.Manual;

                Left = loc.X;
                Top = loc.Y;

                // We need to wait until the HWND window is initialized before
                // setting the state, to ensure that this works correctly on
                // a multi-monitor system.  Thanks to Andrew Smith for this fix.
                SourceInitialized += delegate { WindowState = _SettingsProvider.WindowState; };
            }
        }

    }
}
