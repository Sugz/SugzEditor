using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
    public class SgzExplorerViewModel : ViewModelBase
    {
        private Browser _Browser = new Browser();


        public ObservableCollection<FileViewModel> Files { get; private set; } = new ObservableCollection<FileViewModel>();



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
                FileViewModel fileVm = new FileViewModel(selectedFile);
                if (!Files.Any(x => x.FilePath.Equals(selectedFile)))
                    Files.Add(fileVm);
                fileVm.IsActive = true;
                ActiveDocument = fileVm;
            }
        }

        #endregion

    }
}
