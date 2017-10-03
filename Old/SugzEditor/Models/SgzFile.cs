using SugzEditor.Themes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace SugzEditor.Models
{
    public class SgzFile : SgzDataItem, IPathItem
    {

        enum FileTypes
        {
            ms,
            mcr,
            py,
            cs
        }




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
                //if (value)
                //    MessengerInstance.Send(new MActiveItemMessage(this));
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


        public string Code { get; private set; }

        public object Children => null;


        #region Constructor

        public SgzFile(string path)
        {
            Path = path;
            Text = System.IO.Path.GetFileName(path);
            Code = GetCode();
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

        #endregion Constructor


        private string GetCode()
        {
            if (!IsValidPath)
                return null;

            StringBuilder code = new StringBuilder();
            StreamReader streamReader = new StreamReader(Path, Encoding.GetEncoding("iso-8859-1"));
            while (!streamReader.EndOfStream)
            {
                string line = streamReader.ReadLine();
                code.AppendLine(line);
            }

            return code.ToString();
        }
    }
}
