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


		#region Properties


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

		public ObservableCollection<MaxProcessViewModel> MaxProcess { get; private set; } = new ObservableCollection<MaxProcessViewModel>();
		//public Dictionary<string, string> MaxInstalls { get; private set; } = new Dictionary<string, string>(); 
		public MaxInstall[] MaxInstalls { get; private set; }

		#endregion Properties



		#region Commands


		#region ExecuteCodeCommand
		private RelayCommand _ExecuteCodeCommand;
		public RelayCommand ExecuteCodeCommand => _ExecuteCodeCommand ?? (_ExecuteCodeCommand = new RelayCommand(ExecuteCode, CanExecuteCode));

		private bool CanExecuteCode()
		{
			return (MaxProcess.Any(x => x.IsChecked) && ActiveDocument != null);
		}

		private void ExecuteCode()
		{
			foreach (MaxProcessViewModel maxSession in MaxProcess.Where(x => x.IsChecked))
				Debug.WriteLine($"Executing code from {ActiveDocument.Title} to {maxSession.Title}");
		}
		#endregion ExecuteCodeCommand


		#region GetMaxProcessCommand
		private RelayCommand _GetMaxProcessCommand;
		public ICommand GetMaxProcessCommand => _GetMaxProcessCommand ?? (_GetMaxProcessCommand = new RelayCommand(GetMaxProcess));

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


		#region LunchMaxCommand
		private RelayCommand<string> _LunchMaxCommand;
		public ICommand LunchMaxCommand => _LunchMaxCommand ?? (_LunchMaxCommand = new RelayCommand<string>(LunchMax));

		private void LunchMax(string key)
		{
			Process.Start(MaxInstalls[key]);
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



		/// <summary>
		/// Initializes a new instance of the MainViewModel class.
		/// </summary>
		public MainViewModel()
        {
			GetMaxInstalls();
			GetMaxProcess();

			MessengerInstance.Register<ActiveMaxProcessMessage>(this, x => SetMaxProcessButtonText());
			MessengerInstance.Register<ClosedMaxProcessMessage>(this, x =>
			{
				MaxProcess.Remove(x.MaxProcess);
				SetMaxProcessButtonText();
			});
		}



		#region Methods


		/// <summary>
		/// Get 3ds max installs
		/// </summary>
		private void GetMaxInstalls()
		{
			string[] paths = MaxFolders.Get().Values.ToArray();
			MaxInstalls = new MaxInstall[paths.Length];
			for (int i = 0; i < paths.Length; i++)
				MaxInstalls[i] = new MaxInstall(paths[i]);

			//foreach (string path in MaxFolders.Get().Values)
			//	MaxInstalls.Add(
			//		path.Split('\\').Last(),
			//		path + "\\3dsmax.exe"
			//	);
		}


		/// <summary>
		/// Set the text display in the 3ds max process button
		/// </summary>
		private void SetMaxProcessButtonText()
		{
			ExecuteCodeCommand.RaiseCanExecuteChanged();
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