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


        #region Constructor

        public SgzFile(string path)
        {
            Path = path;
            Text = System.IO.Path.GetFileName(path);
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
    }
}
