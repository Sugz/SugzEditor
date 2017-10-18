using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SugzEditor.Models;
using SugzEditor.Src;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SugzEditor.ViewModels
{
    public class NewFileViewModel : ViewModelBase
    {

		public NewFileModel[] Models { get; set; }
		public NewFileModel SelectedFileType { get; set; }
		public string FileName { get; set; }
		public string FileFolder { get; set; }

		public NewFileViewModel()
		{
			Models = new NewFileModel[]
			{
				new NewFileModel(FileTypes.ms) { IsSelected = true },
				new NewFileModel(FileTypes.mcr),
				new NewFileModel(FileTypes.py),
				new NewFileModel(FileTypes.cs),
			};
		}


		public FileViewModel GetFile()
		{
			string path;
			if (FileFolder != null && FileFolder != "" && Directory.Exists(FileFolder))
				path = FileFolder;
			else
				path = SgzConstants.LocalFolder + "TEMP\\";

			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			if (FileName != null && FileName != "")
				path += $"{FileName}.{SelectedFileType.Extension}";
			else
				path += $"Untitled.{SelectedFileType.Extension}";

			File.Create(path).Close();

			return new FileViewModel(path);
		}
	}
}
