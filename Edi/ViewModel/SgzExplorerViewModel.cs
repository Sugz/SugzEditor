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

namespace SugzEditor.ViewModel
{
    public class SgzExplorerViewModel : ViewModelBase
    {
        private Browser _Browser = new Browser();


        ObservableCollection<SgzFileViewModel> _Files = new ObservableCollection<SgzFileViewModel>();
        ReadOnlyObservableCollection<SgzFileViewModel> _ReadonyFiles = null;
        public ReadOnlyObservableCollection<SgzFileViewModel> Files => _ReadonyFiles ?? 
            (_ReadonyFiles = new ReadOnlyObservableCollection<SgzFileViewModel>(_Files));



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
                }
            }
        }

        public event EventHandler ActiveDocumentChanged;

        #endregion


        #region OpenCommand

        RelayCommand _OpenFileCommand = null;
        public ICommand OpenFileCommand => _OpenFileCommand ?? (_OpenFileCommand = new RelayCommand(OpenFile));


        private void OpenFile()
        {
            if (_Browser.GetFile(fileTypes: SgzConstants.FileTypes) is string selectedFile)
            {
                SgzFileViewModel fileVm = new SgzFileViewModel(selectedFile);
                if (!Files.Any(x => x.FilePath.Equals(selectedFile)))
                    _Files.Add(fileVm);
                fileVm.IsActive = true;
                ActiveDocument = fileVm;
            }
        }

        #endregion

    }
}
