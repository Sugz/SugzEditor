using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
using System.Diagnostics;
using SugzEditor.Src;
using System.Linq;
using SugzTools.Extensions;
using SugzEditor.Models;
using SugzTools.Src;
using System.Collections.Generic;
using System.Windows.Threading;

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

		DispatcherTimer _Timer = new DispatcherTimer();
		Process[] maxProcess;



		private string _MaxInstanceButtonText;
		/// <summary>
		/// 
		/// </summary>
		public string MaxInstanceButtonText
		{
			get => _MaxInstanceButtonText;
			set
			{
				if (value != _MaxInstanceButtonText)
				{
					Set(ref _MaxInstanceButtonText, value);
				}
			}
		}

		public ObservableCollection<SgzMaxInstanceViewModel> MaxInstances { get; private set; } = new ObservableCollection<SgzMaxInstanceViewModel>();
		//public ObservableCollection<string> MaxInstalls { get; private set; } = new ObservableCollection<string>();
		public Dictionary<string, string> MaxInstalls { get; private set; } = new Dictionary<string, string>();


		#region ExecuteCodeCommand
		private RelayCommand _ExecuteCodeCommand;
		public RelayCommand ExecuteCodeCommand => _ExecuteCodeCommand ?? (_ExecuteCodeCommand = new RelayCommand(ExecuteCode, CanExecuteCode));

		private bool CanExecuteCode()
		{
			return (MaxInstances.Any(x => x.IsChecked) && ActiveDocument != null);
		}

		private void ExecuteCode()
		{
			foreach (SgzMaxInstanceViewModel maxSession in MaxInstances.Where(x => x.IsChecked))
				Debug.WriteLine($"Executing code from {ActiveDocument.Title} to {maxSession.Title}");
		}
		#endregion ExecuteCodeCommand


		#region GetMaxInstancesCommand
		private RelayCommand _GetMaxInstancesCommand;
		public ICommand GetMaxInstancesCommand => _GetMaxInstancesCommand ?? (_GetMaxInstancesCommand = new RelayCommand(GetMaxInstances));

		private void GetMaxInstances()
		{
			// TODO: only add if doesn't exist instad of clearing the list to keep the checked ones
			//MaxInstances.Clear();
			maxProcess = Process.GetProcessesByName("3dsmax");
			foreach (Process process in maxProcess)
			{
				Debug.WriteLine($"\n*********\n{process.Handle}\n{process.MainWindowHandle}\nCount: {process.HandleCount}\n**********\n");
				SgzMaxInstanceViewModel maxInstance = new SgzMaxInstanceViewModel(process.MainWindowHandle);
				MaxInstances.Add(maxInstance);
				process.EnableRaisingEvents = true;
				process.Exited += (s, e) => Dispatcher.CurrentDispatcher.Invoke(delegate { MaxInstances.Remove(maxInstance); });
			}
			SetMaxInstanceButtonText();
		}
		#endregion GetMaxInstancesCommand


		#region LunchMaxCommand
		private RelayCommand<string> _LunchMaxCommand;
		public ICommand LunchMaxCommand => _LunchMaxCommand ?? (_LunchMaxCommand = new RelayCommand<string>(LunchMax));

		private void LunchMax(string key)
		{
			Debug.WriteLine($"{key} - {MaxInstalls[key]}");
			Process.Start(MaxInstalls[key]);

			_Timer.Interval = TimeSpan.FromSeconds(5);
			_Timer.Tick += (s, e) =>
			{
				foreach (Process process in Process.GetProcessesByName("3dsmax"))
				{
					if (!maxProcess.Any(x => x.Id == process.Id))
					{
						if (SgzMaxInstanceViewModel.GetMacroRecorder(process.MainWindowHandle) != null)
						{
							SgzMaxInstanceViewModel maxInstance = new SgzMaxInstanceViewModel(process.MainWindowHandle);
							MaxInstances.Add(maxInstance);
							maxInstance.IsChecked = true;
							SetMaxInstanceButtonText();
							_Timer.Stop();
						}
					}
				}
			};
			_Timer.Start();
		} 
		#endregion LunchMaxCommand


		/// <summary>
		/// Initializes a new instance of the MainViewModel class.
		/// </summary>
		public MainViewModel()
        {
			GetMaxInstalls();
			GetMaxInstances();
			MessengerInstance.Register<ActiveMaxInstancesMessage>(this, x =>
			{
				ExecuteCodeCommand.RaiseCanExecuteChanged();
				SetMaxInstanceButtonText();
			});
		}

		private void GetMaxInstalls()
		{
			foreach (string path in MaxFolders.Get().Values)
				MaxInstalls.Add(
					path.Split('\\').Last(),
					path + "\\3dsmax.exe"
				);
		}

		private void SetMaxInstanceButtonText()
		{
			if (MaxInstances.Count == 0)
				MaxInstanceButtonText = "No 3ds Max instance";
			else if (MaxInstances.Where(s => s.IsChecked).Count() > 1)
				MaxInstanceButtonText = "Mutiple instances";
			else if (MaxInstances.FirstOrDefault(s => s.IsChecked) is SgzMaxInstanceViewModel instance)
				MaxInstanceButtonText = instance.Title.Replace("Autodesk ", "");
			else
				MaxInstanceButtonText = "Not connected";
		}
	}
}