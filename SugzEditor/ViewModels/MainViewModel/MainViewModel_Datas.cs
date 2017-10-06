using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SugzEditor.Models;
using SugzEditor.Src;
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


		public ObservableCollection<SgzFileViewModel> Files { get; private set; } = new ObservableCollection<SgzFileViewModel>();



		#region ActiveDocument

		private SgzFileViewModel _ActiveDocument;
		public SgzFileViewModel ActiveDocument
		{
			get => _ActiveDocument;
			set
			{
				if (_ActiveDocument != value)
				{
					Set(ref _ActiveDocument, value);
					ActiveDocumentChanged?.Invoke(this, EventArgs.Empty);
					ExecuteCodeCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public event EventHandler ActiveDocumentChanged;

		#endregion


		#region OpenFileCommand

		RelayCommand _OpenFileCommand = null;
		public ICommand OpenFileCommand => _OpenFileCommand ?? (_OpenFileCommand = new RelayCommand(OpenFile));

		private void OpenFile()
		{
			if (_Browser.GetFile(fileTypes: SgzConstants.FileTypes) is string selectedFile)
			{
				SgzFileViewModel fileVm = new SgzFileViewModel(selectedFile);
				if (!Files.Any(x => x.FilePath.Equals(selectedFile)))
					Files.Add(fileVm);
				fileVm.IsActive = true;
				ActiveDocument = fileVm;
			}
		}

		#endregion


		

	}
}
