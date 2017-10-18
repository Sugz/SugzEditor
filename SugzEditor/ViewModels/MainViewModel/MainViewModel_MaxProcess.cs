using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SugzEditor.Models;
using SugzEditor.Src;
using SugzTools.Src;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace SugzEditor.ViewModels
{
	public partial class MainViewModel : ViewModelBase
	{

		#region Properties


		public ICommand[] MaxProcessCommands { get; set; }

		

		public ObservableCollection<MaxProcessViewModel> MaxProcess { get; private set; } = new ObservableCollection<MaxProcessViewModel>();
		public MaxInstall[] MaxInstalls { get; private set; }
		public MaxInstall[] MaxInstalls32 { get; private set; }
		public MaxInstall[] MaxInstalls64 { get; private set; }

		#region 3ds Max process button text
		private string _MaxProcessButtonText;
		/// <summary>
		/// Get or set the text display in the 3ds max process split button
		/// </summary>
		public string MaxProcessButtonText
		{
			get => _MaxProcessButtonText;
			set
			{
				if (value != _MaxProcessButtonText)
				{
					Set(ref _MaxProcessButtonText, value);
				}
			}
		}
		#endregion 3ds Max process button text

		#endregion Properties


		#region Commands

		#region EvaluateCommand
		private SgzRelayCommand _EvaluateCommand;
		public SgzRelayCommand EvaluateCommand => _EvaluateCommand ??
			(_EvaluateCommand = new SgzRelayCommand(Evaluate, CanEvaluete, key: Key.E, modifiers: ModifierKeys.Control, text: "Evaluate", icon: "Run_16x"));

		private bool CanEvaluete()
		{
			return (MaxProcess.Any(x => x.IsChecked) && ActiveDocument != null);
		}

		private void Evaluate()
		{
			SetStatus($"Executing code from {ActiveDocument.Title}", true);
			foreach (MaxProcessViewModel maxSession in MaxProcess.Where(x => x.IsChecked))
				Debug.WriteLine($"Executing code from {ActiveDocument.Title} to {maxSession.Title}");
		}
		#endregion EvaluateCommand


		#region EvaluateSelectedCommand
		private SgzRelayCommand _EvaluateSelectedCommand;
		public SgzRelayCommand EvaluateSelectedCommand => _EvaluateSelectedCommand ??
			(_EvaluateSelectedCommand = new SgzRelayCommand(EvaluateSelected, CanEvalueteSelected, key: Key.E, modifiers: ModifierKeys.Shift, text: "Evaluate Selected", icon: "RunSelected_16x"));

		private bool CanEvalueteSelected()
		{
			return (MaxProcess.Any(x => x.IsChecked) && ActiveDocument != null);
		}

		private void EvaluateSelected()
		{
			SetStatus($"Executing selected code from {ActiveDocument.Title}", true);
			foreach (MaxProcessViewModel maxSession in MaxProcess.Where(x => x.IsChecked))
				Debug.WriteLine($"Executing selected code from {ActiveDocument.Title} to {maxSession.Title}");
		}
		#endregion EvaluateSelectedCommand


		#region GetMaxProcessCommand
		private SgzRelayCommand _GetMaxProcessCommand;
		public ICommand GetMaxProcessCommand => _GetMaxProcessCommand ?? 
			(_GetMaxProcessCommand = new SgzRelayCommand(GetMaxProcess, key:Key.F5, text:"Refresh 3ds Max list", icon: "Refresh_16x"));

		private void GetMaxProcess()
		{
			foreach (Process process in Process.GetProcessesByName("3dsmax"))
			{
				if (!MaxProcess.Any(x => x.Process.Id == process.Id))
				{
					MaxProcessViewModel maxProcessViewModel = new MaxProcessViewModel(process);
					MaxProcess.Add(maxProcessViewModel);
				}
			}
			SetMaxProcessButtonText();
		}
		#endregion GetMaxProcessCommand


		#region LunchDefaultMaxCommand

		private SgzRelayCommand<MaxInstall> _LunchDefaultMaxCommand;
		public ICommand LunchDefaultMaxCommand => _LunchDefaultMaxCommand ??
			(_LunchDefaultMaxCommand = new SgzRelayCommand<MaxInstall>(LunchDefaultMax, key: Key.F6, text: "Lunch default 3ds Max", icon: "3dsMax_16x"));

		private void LunchDefaultMax(MaxInstall obj)
		{
			
		}

		#endregion LunchDefaultMaxCommand


		#region LunchMaxCommand
		private SgzRelayCommand<MaxInstall> _LunchMaxCommand;
		public ICommand LunchMaxCommand => _LunchMaxCommand ?? (_LunchMaxCommand = new SgzRelayCommand<MaxInstall>(LunchMax));

		private void LunchMax(MaxInstall maxInstall)
		{
			Process.Start(maxInstall.Exe);
			DispatcherTimer _Timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(5) };
			_Timer.Tick += (s, e) =>
			{
				// Monitor the process to know when connection is available
				foreach (Process process in Process.GetProcessesByName("3dsmax"))
				{
					if (!MaxProcess.Any(x => x.Process.Id == process.Id) &&
						MaxProcessViewModel.GetMacroRecorder(process.MainWindowHandle) != null)
					{
						MaxProcessViewModel maxProcessViewModel = new MaxProcessViewModel(process);
						MaxProcess.Add(maxProcessViewModel);
						maxProcessViewModel.IsChecked = true;
						SetMaxProcessButtonText();
						_Timer.Stop();
					}
				}
			};
			_Timer.Start();
		}

		#endregion LunchMaxCommand 


		#endregion Commands


		#region Methods


		private void GetMaxProcessCommands()
		{
			MaxProcessCommands = new ICommand[]
			{
				EvaluateCommand, EvaluateSelectedCommand, GetMaxProcessCommand, LunchDefaultMaxCommand
			};
		}


		/// <summary>
		/// Get 3ds max installs
		/// </summary>
		private void GetMaxInstalls()
		{
			//List<string[]> folders = MaxFolders.GetByBit();
			//MaxInstalls = new MaxInstall[folders.Count];
			//for (int i = 0; i < folders.Count; i++)
			//	MaxInstalls[i] = new MaxInstall(folders[i][0], folders[i][2]);

			string[] folders32 = MaxFolders.Get32().Keys.ToArray();
			MaxInstalls32 = new MaxInstall[folders32.Length];
			for (int i = 0; i < folders32.Length; i++)
				MaxInstalls32[i] = new MaxInstall(folders32[i]);

			string[] folders64 = MaxFolders.Get64().Keys.ToArray();
			MaxInstalls64 = new MaxInstall[folders64.Length];
			for (int i = 0; i < folders64.Length; i++)
				MaxInstalls64[i] = new MaxInstall(folders64[i]);
		}


		/// <summary>
		/// Set the text display in the 3ds max process button
		/// </summary>
		private void SetMaxProcessButtonText()
		{
			EvaluateCommand.RaiseCanExecuteChanged();
			if (MaxProcess.Count == 0)
				MaxProcessButtonText = "No 3ds Max process";
			else if (MaxProcess.Where(s => s.IsChecked).Count() > 1)
				MaxProcessButtonText = "Mutiple process";
			else if (MaxProcess.FirstOrDefault(s => s.IsChecked) is MaxProcessViewModel instance)
				MaxProcessButtonText = instance.Title.Replace("Autodesk ", "");
			else
				MaxProcessButtonText = "Not connected";
		}


		#endregion Methods

	}
}
