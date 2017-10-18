using GalaSoft.MvvmLight;
using SugzEditor.Src;
using SugzEditor.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugzEditor.Models
{
    public class NewFileModel : ObservableObject
    {
		private bool _IsSelected;

		public string Name { get; set; }
		public string Extension { get; set; }
		public object Icon { get; set; }
		
		/// <summary>
		/// 
		/// </summary>
		public bool IsSelected
		{
			get => _IsSelected;
			set
			{
				if (value != _IsSelected)
				{
					Set(ref _IsSelected, value);
				}
			}
		}


		public NewFileModel(FileTypes type)
		{
			switch (type)
			{
				case FileTypes.ms:
					Name = "Maxscript";
					Extension = "ms";
					Icon = IconProvider<object>.Get("MsFile_32x");
					break;
				case FileTypes.mcr:
					Name = "Macroscript";
					Extension = "mcr";
					Icon = IconProvider<object>.Get("McrFile_32x");
					break;
				case FileTypes.py:
					Name = "Python";
					Extension = "py";
					Icon = IconProvider<object>.Get("PyFile_32x");
					break;
				case FileTypes.cs:
					Name = "CSharp";
					Extension = "cs";
					Icon = IconProvider<object>.Get("CsFile_32x");
					break;
				default:
					break;
			}
		}
	}
}
