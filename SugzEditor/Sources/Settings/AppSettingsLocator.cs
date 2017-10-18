using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugzEditor.Src.Settings
{
	public class AppSettingsLocator
	{
		static AppSettings _Instance;
		public static AppSettings Instance => _Instance ?? (_Instance = new AppSettings());
	}
}
