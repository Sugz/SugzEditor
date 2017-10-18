using SugzEditor.Src;
using SugzEditor.Themes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugzEditor.ViewModels
{
    public class FolderViewModel : DataItemViewModel
    {
		#region Dummy Child
		/// <summary>
		/// Dummy class use as child if folder has children before first expanding
		/// </summary>
		private class DummyChild : DataItemViewModel { }
		#endregion Dummy Child


		public ObservableCollection<DataItemViewModel> Children { get; set; }
		public string FilePath { get; set; }


		#region IsExpanded
		public override bool IsExpanded
		{
			get => base.IsExpanded;
			set
			{
				base.IsExpanded = value;

				if (Children != null)
					Icon = IconProvider<Rectangle>.Get(value ? "FolderOpen_16x" : "Folder_16x");

				if (value && Children.Count == 1 && Children[0] is DummyChild)
				{
					Children.Clear();
					DirectoryInfo folder = new DirectoryInfo(FilePath);
					foreach (DirectoryInfo dir in folder.EnumerateDirectories())
						Children.Add(new FolderViewModel(dir.FullName));
					foreach (FileInfo file in folder.EnumerateFiles().Where(x => SgzConstants.FileTypes.Values.Contains(x.Extension)))
						Children.Add(new FolderViewModel(file.FullName));
				}


			}
		} 
		#endregion IsExpanded


		#region Constructor

		public FolderViewModel(string path)
		{
			FilePath = path;
			Title = Path.GetFileName(path);
			Icon = IconProvider<Rectangle>.Get("Folder_16x");
			if (HasChildren())
				Children = new ObservableCollection<DataItemViewModel>() { new DummyChild() };
		}

		#endregion Constructor


		#region Methods


		/// <summary>
		/// Check if folder contain subfolder or authorized file type
		/// </summary>
		/// <returns></returns>
		private bool HasChildren()
		{
			DirectoryInfo folder = new DirectoryInfo(FilePath);
			return (folder.EnumerateDirectories().Any() ||
				folder.EnumerateFiles().Where(x => SgzConstants.FileTypes.Values.Contains(x.Extension)).Any());
		}


		#endregion Methods
	}
}
