using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
using System.Diagnostics;
using SugzEditor.Src;
using System.Linq;
using SugzTools.Extensions;
using SugzEditor.Messages;
using SugzTools.Src;
using System.Collections.Generic;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Threading;
using SugzEditor.Models;
using Microsoft.Win32;
using SugzEditor.Themes;
using System.Windows;
using SugzEditor.Src.Settings;
using GalaSoft.MvvmLight.Ioc;
using SugzEditor.Views;
using SugzEditor.Controls;
using SugzEditor.Src.Commands;
using System.Windows.Controls;
using SugzEditor.Themes.Colors;

namespace SugzEditor.ViewModels
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public partial class MainViewModel : ViewModelBase
    {

		#region Fields

		private SettingsConfigProvider _SettingsProvider = SimpleIoc.Default.GetInstance<SettingsConfigProvider>();
		private DispatcherTimer _Timer;
		
		private bool _IsDarkTheme;
		private double _Temperature = 0d;
		private double _Brightness = 1d;
		private double _Contrast = 1d;
		private bool _ShowToolbar;
		private bool _ShowExplorer;
		private double _SavedExplorerWidth;
		private GridLength _ExplorerGridWidth;
		private bool _ShowTabs;
		private string _Status;

		#endregion Fields


		#region Properties


		public ICommand[] UICommands { get; set; }

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
				Brightness = _Brightness;
				Contrast = _Contrast;
			}
		}

		/// <summary>
		/// TODO: temperature implementation ?
		/// </summary>
		public double Temperature
		{
			get => _Temperature;
			set
			{
				double i = (value <= TemperatureMin ? TemperatureMin : (value >= TemperatureMax ? TemperatureMax : value));
				Set(ref _Temperature, i);
				//if (Contrast != 1d)
				//	ColorHelper.SetTemperatureAndContrast(value, Contrast);
				//else
				//	ColorHelper.SetTemperature(value);
			}
		}
		public double TemperatureMin { get; set; } = -1d;
		public double TemperatureMax { get; set; } = 1d;

		/// <summary>
		/// 
		/// </summary>
		public double Brightness
		{
			get => _Brightness;
			set
			{
				double i = (value <= BrightnessMin ? BrightnessMin : (value >= BrightnessMax ? BrightnessMax : value));
				Set(ref _Brightness, i);
				if (Contrast != 1d)
					ColorHelper.SetBrightnessAndContrast(value, Contrast);
				else
					ColorHelper.SetBrightness(value);
			}
		}
		public double BrightnessMin { get; set; } = -1d;
		public double BrightnessMax { get; set; } = 1d;

		/// <summary>
		/// 
		/// </summary>
		public double Contrast
		{
			get => _Contrast;
			set
			{
				double i = (value <= ContrastMin ? ContrastMin : (value >= ContrastMax ? ContrastMax : value));
				Set(ref _Contrast, i);
				if (Brightness != 1d)
					ColorHelper.SetBrightnessAndContrast(Brightness, value);
				else
					ColorHelper.SetContrast(value);
			}
		}
		public double ContrastMin { get; set; } = 0.5d;
		public double ContrastMax { get; set; } = 1.5d;

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


		/// <summary>
		/// 
		/// </summary>
		public string Status
		{
			get => _Status;
			set
			{
				if (value != _Status)
				{
					Set(ref _Status, value);
				}
			}
		}


		#endregion Properties



		#region Commands

		private RelayCommand<bool> _SetThemeCommand;
		public ICommand SetThemeCommand => _SetThemeCommand ?? 
			(_SetThemeCommand = new RelayCommand<bool>(x => IsDarkTheme = x));

		private SgzRelayCommand _UpBrightnessCommand;
		public ICommand UpBrightnessCommand => _UpBrightnessCommand ??
			(_UpBrightnessCommand = new SgzRelayCommand(() => Brightness += 0.05d, key:Key.Add, modifiers:ModifierKeys.Control, text:"Increase Brightness"));

		private SgzRelayCommand _DownBrightnessCommand;
		public ICommand DownBrightnessCommand => _DownBrightnessCommand ??
			(_DownBrightnessCommand = new SgzRelayCommand(() => Brightness -= 0.05d, key: Key.Subtract, modifiers: ModifierKeys.Control, text: "Decrease Brightness"));

		private SgzRelayCommand _UpContrastCommand;
		public ICommand UpContrastCommand => _UpContrastCommand ??
			(_UpContrastCommand = new SgzRelayCommand(() => Contrast += 0.05d, key: Key.Add, modifiers: ModifierKeys.Shift, text: "Increase Contrast"));

		private SgzRelayCommand _DownContrastCommand;
		public ICommand DownContrastCommand => _DownContrastCommand ??
			(_DownContrastCommand = new SgzRelayCommand(() => Contrast -= 0.05d, key: Key.Subtract, modifiers: ModifierKeys.Shift, text: "Decrease Contrast"));



		private SgzRelayCommand _ShowToolbarCommand;
		public ICommand ShowToolbarCommand => _ShowToolbarCommand ??
			(_ShowToolbarCommand = new SgzRelayCommand(() => ShowToolbar = !ShowToolbar, text: "Show Toolbar", icon: "Toolbar_16x"));

		private SgzRelayCommand _ShowExplorerCommand;
		public ICommand ShowExplorerCommand => _ShowExplorerCommand ??
			(_ShowExplorerCommand = new SgzRelayCommand(() => ShowExplorer = !ShowExplorer, text: "Show Explorer", icon: "Treeview_16x"));

		private SgzRelayCommand _ShowTabsCommand;
		public ICommand ShowTabsCommand => _ShowTabsCommand ??
			(_ShowTabsCommand = new SgzRelayCommand(() => ShowTabs = !ShowTabs, text: "Show Tabs", icon: "Tabs_16x"));

		private SgzRelayCommand _OpenOptionsCommand;
		public ICommand OpenOptionsCommand => _OpenOptionsCommand ??
			(_OpenOptionsCommand = new SgzRelayCommand(OpenOptions, key: Key.O, modifiers: ModifierKeys.Control, text: "Options", icon: "Option_16x"));

		private void OpenOptions()
		{
			MessengerInstance.Send(new OpenDialogMessage(WindowType.Options));
		}


		private SgzRelayCommand _SetShortcutCommand;
		public ICommand SetShortcutCommand => _SetShortcutCommand ??
			(_SetShortcutCommand = new SgzRelayCommand(SetShortcut, text: "Set Shortcut"));

		private void SetShortcut()
		{
			((SgzRelayCommand)UndoCommand).Text = "Test";
			((SgzRelayCommand)UndoCommand).GestureModifiers = ModifierKeys.Shift;
		}


		#endregion Commands



		/// <summary>
		/// Initializes a new instance of the MainViewModel class.
		/// </summary>
		public MainViewModel()
        {
			LoadSettings();
			GetMaxInstalls();
			GetMaxProcess();
			GetCommands();

			MessengerInstance.Register<ActiveMaxProcessMessage>(this, x => SetMaxProcessButtonText());
			MessengerInstance.Register<ClosedMaxProcessMessage>(this, x =>
			{
				MaxProcess.Remove(x.MaxProcess);
				SetMaxProcessButtonText();
			});
		}

		


		#region Methods

		public override void Cleanup()
		{
			base.Cleanup();
			SaveSettings();
		}


		/// <summary>
		/// Load user settings
		/// </summary>
		private void LoadSettings()
		{
			_SettingsProvider.Load();

			IsDarkTheme = _SettingsProvider.IsDarktheme;
			Brightness = _SettingsProvider.Brightness;
			Contrast = _SettingsProvider.Contrast;
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
			_SettingsProvider.Brightness = Brightness;
			_SettingsProvider.Contrast = Contrast;
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


		private void SetStatus(string status, bool useTimer)
		{
			Status = status;
			if (useTimer)
			{
				_Timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(5) };
				_Timer.Tick += (s, e) =>
				{
					Status = "";
					_Timer.Stop();
				};
				_Timer.Start();
			}
		}


		private void GetCommands()
		{
			UICommands = new ICommand[]
			{
				UpBrightnessCommand, DownBrightnessCommand, UpContrastCommand, DownContrastCommand, ShowToolbarCommand, ShowExplorerCommand, ShowTabsCommand, OpenOptionsCommand
			};

			GetDatasCommands();
			GetMaxProcessCommands();
			GetTextEditorCommands();
		}


		#endregion Methods
	}
}