using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using ICSharpCode.AvalonEdit.Document;
using SugzEditor.ViewModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace SugzEditor.ViewModels
{
    public class Workspace : ViewModelBase
    {
        protected Workspace()
        {

        }

        static Workspace _this = new Workspace();

        public static Workspace This
        {
            get { return _this; }
        }


        ObservableCollection<FileViewModel> _files = new ObservableCollection<FileViewModel>();
        ReadOnlyObservableCollection<FileViewModel> _readonyFiles = null;
        public ReadOnlyObservableCollection<FileViewModel> Files
        {
            get
            {
                if (_readonyFiles == null)
                    _readonyFiles = new ReadOnlyObservableCollection<FileViewModel>(_files);

                return _readonyFiles;
            }
        }


        #region OpenCommand
        RelayCommand _openCommand = null;
        public ICommand OpenCommand
        {
            get
            {
                if (_openCommand == null)
                {
                    _openCommand = new RelayCommand(OnOpen);
                }

                return _openCommand;
            }
        }

        private void OnOpen()
        {
            var dlg = new OpenFileDialog();
            if (dlg.ShowDialog().GetValueOrDefault())
            {
                var fileViewModel = Open(dlg.FileName);
                ActiveDocument = fileViewModel;
            }
        }

        public FileViewModel Open(string filepath)
        {
            var fileViewModel = _files.FirstOrDefault(fm => fm.FilePath == filepath);
            if (fileViewModel != null)
                return fileViewModel;

            fileViewModel = new FileViewModel(filepath) { IsActive = true};
            _files.Add(fileViewModel);
            return fileViewModel;
        }

        #endregion

        #region NewCommand
        RelayCommand _newCommand = null;
        public ICommand NewCommand
        {
            get
            {
                if (_newCommand == null)
                {
                    _newCommand = new RelayCommand(OnNew);
                }

                return _newCommand;
            }
        }

        private void OnNew()
        {
            _files.Add(new FileViewModel() { Document = new TextDocument() });
            ActiveDocument = _files.Last();
        }

        #endregion

        #region ActiveDocument

        private FileViewModel _ActiveDocument = null;
        public FileViewModel ActiveDocument
        {
            get => _ActiveDocument;
            set
            {
                if (_ActiveDocument != value)
                {
                    Set(ref _ActiveDocument, value);
                    _ActiveDocument = value;
                    RaisePropertyChanged("ActiveDocument");
                    if (ActiveDocumentChanged != null)
                        ActiveDocumentChanged(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler ActiveDocumentChanged;

        #endregion


        internal void Close(FileViewModel fileToClose)
        {
            if (fileToClose.IsModified)
            {
                var res = MessageBox.Show($"Save changes for file '{fileToClose.Title}'?", "SugzEditor", MessageBoxButton.YesNoCancel);
                if (res == MessageBoxResult.Cancel)
                    return;
                if (res == MessageBoxResult.Yes)
                {
                    Save(fileToClose);
                }
            }

            _files.Remove(fileToClose);
        }

        internal void Save(FileViewModel fileToSave, bool saveAsFlag = false)
        {
            if (fileToSave.FilePath == null || saveAsFlag)
            {
                var dlg = new SaveFileDialog();
                if (dlg.ShowDialog().GetValueOrDefault())
                    fileToSave.FilePath = dlg.SafeFileName;
            }

            File.WriteAllText(fileToSave.FilePath, fileToSave.Document.Text);
            ActiveDocument.IsModified = false;
        }



    }
}
