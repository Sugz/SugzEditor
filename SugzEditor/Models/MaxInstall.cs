using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SugzEditor.Models
{
	public class MaxInstall
	{
		public BitmapImage Icon { get; set; }
		public string Title { get; set; }
		public string Exe { get; set; }

		public MaxInstall(string path)
		{
			Icon = new BitmapImage(new Uri($@"{path}\Icons\icon_main.ico"));
			Title = path.Split('\\').Last();
			Exe = $@"{path}\3dsmax.exe"; 
		}
	}
}
