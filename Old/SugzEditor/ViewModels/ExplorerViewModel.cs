using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using SugzEditor.Models;
using SugzEditor.Settings;
using SugzEditor.Src;
using SugzTools.Extensions;
using SugzTools.Src;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace SugzEditor.ViewModels
{
    public class ExplorerViewModel : ViewModelBase
    {

        #region Fields

        SettingsConfigProvider _SettingsProvider = SimpleIoc.Default.GetInstance<SettingsConfigProvider>();
        private Browser _Browser = new Browser();

        #endregion Fields



        #region Properties


        public ObservableCollection<SgzFileViewModel> Files { get; private set; } = new ObservableCollection<SgzFileViewModel>();
        //public ObservableCollection<SgzFile> Files { get; private set; } = new ObservableCollection<SgzFile>();
        public ObservableCollection<SgzFile> RecentFiles { get; private set; } = new ObservableCollection<SgzFile>();
        public ObservableCollection<SgzFolder> Folders { get; private set; } = new ObservableCollection<SgzFolder>();
        public ObservableCollection<SgzFolder> RecentFolders { get; private set; } = new ObservableCollection<SgzFolder>();

        /// <summary>
        /// Get all 3ds max folders
        /// </summary>
        public ObservableCollection<SgzFolder> MaxFolders { get; set; } = new ObservableCollection<SgzFolder>();


        #endregion Properties



        #region Commands

        private RelayCommand<SgzFile> _AddFileCommand;
        public ICommand AddFileCommand => _AddFileCommand ?? (_AddFileCommand = new RelayCommand<SgzFile>(AddFile));

        private RelayCommand<SgzFolder> _AddFolderCommand;
        public ICommand AddFolderCommand => _AddFolderCommand ?? (_AddFolderCommand = new RelayCommand<SgzFolder>(AddFolder));


        #endregion Commands



        public ExplorerViewModel()
        {
            LoadSettings();
            SugzTools.Src.MaxFolders.Get().ForEach(x =>
            {
                MaxFolders.Add(new SgzFolder(x.Key) { Text = $"3ds Max {x.Value.Substring(x.Value.Length - 4)} ENU" });
                MaxFolders.Add(new SgzFolder(x.Value));
            });
        }




        #region Methods

        /// <summary>
        /// Load user settings
        /// </summary>
        private void LoadSettings()
        {
            _SettingsProvider.Load();
            if (!_SettingsProvider.IsFirstRun)
            {
                if (_SettingsProvider.RecentFiles != null)
                {
                    foreach (string file in _SettingsProvider.RecentFiles)
                        RecentFiles.Insert(0, new SgzFile(file));
                }
                if (_SettingsProvider.RecentFolders != null)
                {
                    foreach (string folder in _SettingsProvider.RecentFolders)
                        RecentFolders.Insert(0, new SgzFolder(folder));
                }
            }
        }




        private void AddFile(SgzFile file)
        {
            if (file is null && _Browser.GetFile(fileTypes: SgzConstants.FileTypes) is string selectedFile)
                file = new SgzFile(selectedFile);

            if (file != null)
            {
                SgzFileViewModel fileVm = new SgzFileViewModel(file.Path);
                if (!Files.Any(x => x.Path.Equals(file.Path)))
                    Files.Add(fileVm);
                fileVm.IsActive = true;

                if (RecentFiles.FirstOrDefault(x => x.Path.Equals(file.Path)) is SgzFile recentFile)
                    RecentFiles.Remove(recentFile);
                else if (RecentFiles.Count == 10)
                    RecentFiles.RemoveAt(RecentFiles.Count - 1);
                RecentFiles.Insert(0, file);

                _SettingsProvider.RecentFiles = new StringCollection();
                RecentFiles.ForEach(x => _SettingsProvider.RecentFiles.Add(x.Path));
                _SettingsProvider.Save();
            }
        }


        /// <summary>
        /// Let the user choose a folder to add to the folders list if it doesn't exist already
        /// </summary>
        private void AddFolder(SgzFolder folder)
        {
            if (folder is null && _Browser.GetFolder() is string selectedFolder)
                    folder = new SgzFolder(selectedFolder);

            if (folder != null)
            {
                if (!Folders.Any(x => x.Path.Equals(folder.Path)))
                    Folders.Add(folder);

                if (RecentFolders.FirstOrDefault(x => x.Path.Equals(folder.Path)) is SgzFolder recentFolder)
                    RecentFolders.Remove(recentFolder);
                else if (RecentFolders.Count == 10)
                    RecentFolders.RemoveAt(RecentFolders.Count - 1);
                RecentFolders.Insert(0, folder);

                _SettingsProvider.RecentFolders = new StringCollection();
                RecentFolders.ForEach(x => _SettingsProvider.RecentFolders.Add(x.Path));
                _SettingsProvider.Save();
            }
        }


        #endregion Methods

    }
}
