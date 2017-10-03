using GalaSoft.MvvmLight;
using ICSharpCode.AvalonEdit.Document;
using SugzEditor.Models;
using SugzEditor.Themes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace SugzEditor.ViewModels
{
    public class SgzFileViewModel : SgzDataItem, IPathItem
    {
        enum FileTypes
        {
            ms,
            mcr,
            py,
            cs
        }


        public object Children => null;

        #region IPathItem 

        public bool IsValidPath => File.Exists(Path);
        public string Path { get; set; }
        public string RelativePath => throw new NotImplementedException();

        #endregion IPathItem 


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


        /// <summary>
        /// Get or set if this is the selected item in the treeviews except the current
        /// </summary>
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



        private TextDocument _Document;
        /// <summary>
        /// 
        /// </summary>
        public TextDocument Document
        {
            get => _Document;
            set
            {
                if (value != _Document)
                {
                    Set(ref _Document, value);
                    IsDirty = true;
                }
            }
        }



        private bool _IsModified = false;
        /// <summary>
        /// 
        /// </summary>
        public bool IsModified
        {
            get => _IsModified;
            set
            {
                Set(ref _IsModified, value);
                Debug.WriteLine($"\n\n\n***********{value}************\n\n\n");
            }
        }


        private bool _IsDirty = false;
        /// <summary>
        /// 
        /// </summary>
        //public bool IsDirty
        //{
        //    get => _IsDirty;
        //    set => Set(ref _IsDirty, value);
        //}
        public bool IsDirty
        {
            get
            {
                return _IsDirty;
            }

            set
            {
                if (_IsDirty != value)
                {
                    _IsDirty = value;
                    RaisePropertyChanged(() => _IsDirty);
                    Debug.WriteLine($"\n\n\n***********{value}************\n\n\n");
                }
            }
        }


        private bool _IsReadOnly;
        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly
        {
            get => _IsReadOnly;
            protected set
            {
                if (value != _IsReadOnly)
                {
                    Set(ref _IsReadOnly, value);
                }
            }
        }



        public SgzFileViewModel(string path)
        {
            Path = path;
            Text = System.IO.Path.GetFileName(path);
            
            GetCode();
            IsDirty = false;
            Enum.TryParse(System.IO.Path.GetExtension(path).TrimStart('.'), out FileTypes type);
            switch (type)
            {
                case FileTypes.ms:
                    Icon = IconProvider<Rectangle>.Get("ExplorerMs_16x");
                    break;
                case FileTypes.mcr:
                    Icon = IconProvider<Rectangle>.Get("ExplorerMcr_16x");
                    break;
                case FileTypes.py:
                    Icon = IconProvider<Rectangle>.Get("ExplorerPy_16x");
                    break;
                case FileTypes.cs:
                    Icon = IconProvider<Rectangle>.Get("ExplorerCs_16x");
                    break;
            }
        }


        private void GetCode()
        {
            if (!IsValidPath)
                return;

            StringBuilder code = new StringBuilder();
            StreamReader streamReader = new StreamReader(Path, Encoding.GetEncoding("iso-8859-1"));
            while (!streamReader.EndOfStream)
            {
                string line = streamReader.ReadLine();
                code.AppendLine(line);
            }

            Document = new TextDocument();
            Document.Text = code.ToString();
        }
    }
}
