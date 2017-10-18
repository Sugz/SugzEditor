using SugzEditor.Src;
using System;
using System.Collections.Generic;
//using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SugzEditor.Models
{
	public class MaxInstall
	{
		public Image Icon { get; set; }
		public string Title { get; set; }
		public string Exe { get; set; }

		public MaxInstall(string path)
		{
			Exe = $@"{path}\3dsmax.exe";

			Title = path.Split('\\').Last();

			// Add spaces for 3dsMax8 and 3dsMax9
			if (!Title.Contains(' '))
				Title = $"3ds Max {Title.Substring(Title.Length - 1)}";

			Icon = new Image();
			Icon.Height = 16;
			RenderOptions.SetBitmapScalingMode(Icon, BitmapScalingMode.Fant);
			RenderOptions.SetEdgeMode(Icon, EdgeMode.Aliased);
			string icoName = Title.Replace(" ", "");
			BitmapImage bmpImage = new BitmapImage(new Uri($"/Resources/3dsMaxIcons/{icoName}.png", UriKind.Relative));
			Icon.Source = bmpImage;
		}

		public MaxInstall(string path, string bit) : this(path)
		{
			Title += $" - {bit}bit";
		}
	}
}
