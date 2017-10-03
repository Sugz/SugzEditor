using SugzEditor.Src;
using SugzEditor.Themes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace SugzEditor.Models
{
    public class SgzFolder : SgzDataItem, IPathItem
    {
        #region Dummy Child
        /// <summary>
        /// Dummy class use as child if folder has children before first expanding
        /// </summary>
        private class DummyChild : SgzDataItem { }
        #endregion Dummy Child


        #region IPathItem 

        public bool IsValidPath => Directory.Exists(Path);
        public string Path { get; set; }
        public string RelativePath => throw new NotImplementedException();

        #endregion IPathItem 


        public ObservableCollection<SgzDataItem> Children { get; set; }
        public override bool IsExpanded
        {
            get => base.IsExpanded;
            set
            {
                base.IsExpanded = value;

                if (Children != null)
                    Icon = IconProvider<Rectangle>.Get(value ? "ExplorerFolderOpen_16x" : "ExplorerFolder_16x");

                if (value && Children.Count == 1 && Children[0] is DummyChild)
                {
                    Children.Clear();
                    DirectoryInfo folder = new DirectoryInfo(Path);
                    foreach (DirectoryInfo dir in folder.EnumerateDirectories())
                        Children.Add(new SgzFolder(dir.FullName));
                    foreach (FileInfo file in folder.EnumerateFiles().Where(x => SgzConstants.FileTypes.Values.Contains(x.Extension)))
                        Children.Add(new SgzFile(file.FullName));
                }

                
            }
        }


        #region Constructor

        public SgzFolder(string path)
        {
            Path = path;
            Text = System.IO.Path.GetFileName(path);
            Icon = IconProvider<Rectangle>.Get("ExplorerFolder_16x");
            if (HasChildren())
                Children = new ObservableCollection<SgzDataItem>() { new DummyChild() };
        }

        #endregion Constructor


        #region Methods


        /// <summary>
        /// Check if folder contain subfolder or authorized file type
        /// </summary>
        /// <returns></returns>
        private bool HasChildren()
        {
            DirectoryInfo folder = new DirectoryInfo(Path);
            return (folder.EnumerateDirectories().Any() ||
                folder.EnumerateFiles().Where(x => SgzConstants.FileTypes.Values.Contains(x.Extension)).Any());
        }


        #endregion Methods
    }
}
