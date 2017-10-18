using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using SugzEditor.Messages;
using SugzEditor.Src;
using SugzEditor.Views;
using SugzTools.Src;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SugzEditor.ViewModels
{
	public partial class MainViewModel : ViewModelBase
	{

		private Browser _Browser = new Browser();
		public ICommand[] DatasCommands { get; set; }

		public ObservableCollection<FileViewModel> Files { get; private set; } = new ObservableCollection<FileViewModel>();
		public ObservableCollection<FolderViewModel> Folders { get; private set; } = new ObservableCollection<FolderViewModel>();


		#region ActiveDocument

		private FileViewModel _ActiveDocument;
		public FileViewModel ActiveDocument
		{
			get => _ActiveDocument;
			set
			{
				if (_ActiveDocument != value)
				{
					Set(ref _ActiveDocument, value);
					ActiveDocumentChanged?.Invoke(this, EventArgs.Empty);
					EvaluateCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public event EventHandler ActiveDocumentChanged;

		#endregion


		#region NewFileCommand

		SgzRelayCommand _NewFileCommand = null;
		public ICommand NewFileCommand => _NewFileCommand ??
			(_NewFileCommand = new SgzRelayCommand(NewFile, key: Key.N, modifiers: ModifierKeys.Control, text: "New File", icon: "NewFile_16x"));

		private async void NewFile()
		{
			NewFileViewModel datacontext = new NewFileViewModel();
			NewFileView view = new NewFileView { DataContext = datacontext };
			bool result = (bool)await DialogHost.Show(view, "MainWindow");
			if (result)
			{
				FileViewModel fileViewModel = datacontext.GetFile();
				Files.Add(fileViewModel);
				fileViewModel.IsActive = true;
				ActiveDocument = fileViewModel;
			}

			//check the result...


			//MessengerInstance.Send(new OpenDialogMessage(WindowType.NewFile));

			//FileViewModel fileViewModel = new FileViewModel();
			//Files.Add(fileViewModel);
			//fileViewModel.IsActive = true;
			//ActiveDocument = fileViewModel;
		}

		private void OnNewFileViewClosing(object sender, DialogClosingEventArgs eventArgs)
		{
			
		}

		#endregion NewFileCommand


		#region OpenFileCommand

		SgzRelayCommand _OpenFileCommand = null;
		public ICommand OpenFileCommand => _OpenFileCommand ?? 
			(_OpenFileCommand = new SgzRelayCommand(OpenFile, key: Key.O, modifiers: ModifierKeys.Control, text:"Open File", icon:"OpenFile_16x"));

		private void OpenFile()
		{
			if (_Browser.GetFile(fileTypes: SgzConstants.FileTypes) is string selectedFile)
			{
				FileViewModel fileViewModel = new FileViewModel(selectedFile);
				if (!Files.Any(x => x.FilePath is string s && s.Equals(selectedFile)))
					Files.Add(fileViewModel);
				fileViewModel.IsActive = true;
				ActiveDocument = fileViewModel;
			}
		}

		#endregion OpenFileCommand


		#region OpenFolderCommand

		SgzRelayCommand _OpenFolderCommand = null;
		public ICommand OpenFolderCommand => _OpenFolderCommand ??
			(_OpenFolderCommand = new SgzRelayCommand(OpenFolder, key: Key.O, modifiers: ModifierKeys.Shift, text: "Open Folder", icon:"OpenFolder_16x"));

		private void OpenFolder()
		{
			if (_Browser.GetFolder() is string selectedFolder)
			{
				FolderViewModel folderViewModel = new FolderViewModel(selectedFolder);
				if (!Folders.Any(x => x.FilePath.Equals(selectedFolder)))
					Folders.Add(folderViewModel);
			}
		}

		#endregion OpenFolderCommand


		#region SaveFileCommand

		SgzRelayCommand _SaveFileCommand = null;
		public ICommand SaveFileCommand => _SaveFileCommand ??
			(_SaveFileCommand = new SgzRelayCommand(SaveFile, key: Key.S, modifiers: ModifierKeys.Control, text: "Save File", icon: "Save_16x"));

		private void SaveFile()
		{
			throw new NotImplementedException();
		}

		#endregion SaveFileCommand


		#region SaveFileAsCommand
		SgzRelayCommand _SaveFileAsCommand = null;
		public ICommand SaveFileAsCommand => _SaveFileAsCommand ??
			(_SaveFileAsCommand = new SgzRelayCommand(SaveFileAs, key: Key.S, modifiers: ModifierKeys.Alt, text: "Save File As", icon: "SaveAs_16x"));

		private void SaveFileAs()
		{
			throw new NotImplementedException();
		}

		#endregion SaveFileAsCommand


		#region SaveAllCommand

		SgzRelayCommand _SaveAllCommand = null;
		public ICommand SaveAllCommand => _SaveAllCommand ??
			(_SaveAllCommand = new SgzRelayCommand(SaveAll, key: Key.S, modifiers: ModifierKeys.Shift, text: "Save All", icon: "SaveAll_16x"));

		private void SaveAll()
		{
			throw new NotImplementedException();
		}

		#endregion SaveAllCommand



		private void GetDatasCommands()
		{
			DatasCommands = new ICommand[]
			{
				NewFileCommand, OpenFileCommand, OpenFolderCommand, SaveFileCommand, SaveFileAsCommand, SaveAllCommand
			};
		}
	}
}
