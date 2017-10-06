using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Input;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Utils;
using ICSharpCode.AvalonEdit.Highlighting;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SugzEditor.Src;
using SugzEditor.Themes;
using System.Windows.Shapes;

namespace SugzEditor.ViewModels
{
    public class SgzFileViewModel : SgaDataItemViewModel
    {

        #region Constructors
        public SgzFileViewModel(string filePath)
        {
            FilePath = filePath;
            Title = System.IO.Path.GetFileName(filePath);
        }

        public SgzFileViewModel()
        {
            Title = "Untitled";
            IsModified = true;
        }
        #endregion Constructors


        #region IsActive
        private bool _IsActive;
        /// <summary>
        /// Get or set if this is the active item in the editor, the tabs and the current treeview
        /// </summary>
        public bool IsActive
        {
            get => _IsActive;
            set
            {
                Set(ref _IsActive, value);
                if (value && !IsSelected)
                    IsSelected = true;
                else if (!value && IsSelected)
                    IsSelected = false;
            }
        }
        #endregion IsActive


        #region IsSelected
        public override bool IsSelected
        {
            get => base.IsSelected;
            set
            {
                base.IsSelected = value;
                if (value)
                    IsActive = true;
            }
        } 
        #endregion IsSelected


        #region FilePath
        private string _FilePath = null;
        public string FilePath
        {
            get => _FilePath;
            set
            {
                if (_FilePath != value)
                {
                    Set(ref _FilePath, value);

                    if (File.Exists(_FilePath))
                    {
                        _Document = new TextDocument();
                        HighlightDef = HighlightingManager.Instance.GetDefinition("XML");

                        // Check file attributes and set to read-only if file attributes indicate that
                        if ((File.GetAttributes(_FilePath) & FileAttributes.ReadOnly) != 0)
                        {
                            IsReadOnly = true;
                            IsReadOnlyReason = "This file cannot be edit because another process is currently writting to it.\n" +
                                                    "Change the file access permissions or save the file in a different location if you want to edit it.";
                        }

                        using (FileStream fs = new FileStream(_FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            using (StreamReader reader = FileReader.OpenStream(fs, Encoding.UTF8))
                                _Document = new TextDocument(reader.ReadToEnd());
                        }

                        Enum.TryParse(System.IO.Path.GetExtension(_FilePath).TrimStart('.'), out FileTypes type);
                        switch (type)
                        {
                            case FileTypes.ms:
                                Icon = IconProvider<Rectangle>.Get("MsFile_16x");
                                break;
                            case FileTypes.mcr:
                                Icon = IconProvider<Rectangle>.Get("McrFile_16x");
                                break;
                            case FileTypes.py:
                                Icon = IconProvider<Rectangle>.Get("PyFile_16x");
                                break;
                            case FileTypes.cs:
                                Icon = IconProvider<Rectangle>.Get("CsFile_16x");
                                break;
                        }
                    }
                }
            }
        }
        #endregion


        #region TextContent
        private TextDocument _Document;
        public TextDocument Document
        {
            get => _Document;
            set
            {
                if (_Document != value)
                {
                    Set(ref _Document, value);
                }
            }
        }
        #endregion


        #region HighlightingDefinition
        private IHighlightingDefinition _Highlightdef = null;
        public IHighlightingDefinition HighlightDef
        {
            get => _Highlightdef;
            set
            {
                if (_Highlightdef != value)
                {
                    Set(ref _Highlightdef, value);
                    //IsModified = true;
                }
            }
        }
        #endregion


        #region IsModified
        private bool _IsModified = false;
        /// <summary>
        /// 
        /// </summary>
        public bool IsModified
        {
            get => _IsModified;
            set
            {
                if (value != _IsModified)
                    Set(ref _IsModified, value);
            }
        }
        #endregion


        #region IsReadOnly
        private bool _IsReadOnly = false;
        public bool IsReadOnly
        {
            get => _IsReadOnly;
            protected set
            {
                if (_IsReadOnly != value)
                    Set(ref _IsReadOnly, value);
            }
        }

        private string _IsReadOnlyReason = string.Empty;
        public string IsReadOnlyReason
        {
            get => _IsReadOnlyReason;
            protected set
            {
                if (_IsReadOnlyReason != value)
                    Set(ref _IsReadOnlyReason, value);
            }
        }
        #endregion IsReadOnly


        #region SaveCommand
        RelayCommand _saveCommand = null;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(OnSave, () => IsModified);
                }

                return _saveCommand;
            }
        }

        private void OnSave()
        {
            Workspace.This.Save(this, false);
        }

        #endregion


        #region SaveAsCommand
        RelayCommand _saveAsCommand = null;
        public ICommand SaveAsCommand
        {
            get
            {
                if (_saveAsCommand == null)
                {
                    _saveAsCommand = new RelayCommand(OnSaveAs, () => IsModified);
                }

                return _saveAsCommand;
            }
        }

        private void OnSaveAs()
        {
            Workspace.This.Save(this, true);
        }

        #endregion


        #region CloseCommand
        RelayCommand _closeCommand = null;
        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new RelayCommand(OnClose);
                }

                return _closeCommand;
            }
        }

        private void OnClose()
        {
            Workspace.This.Close(this);
        }
        #endregion

    }
}
