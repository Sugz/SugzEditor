using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SugzEditor.Src
{
    public static class SgzConstants
    {
        public static readonly Dictionary<string, string> FileTypes = new Dictionary<string, string>()
        {
            {"Maxscript", ".ms" },
            {"Macroscript", ".mcr" },
            {"Python", ".py" },
            {"CSharp", ".cs" },
        };

		public static readonly string LocalAppData = Environment.GetEnvironmentVariable("LocalAppData");
		public static readonly string Company = ((AssemblyCompanyAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCompanyAttribute), false)).Company;
		public static readonly string Product = ((AssemblyProductAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyProductAttribute), false)).Product;
		public static readonly string LocalFolder = $@"{LocalAppData}\{Company}\{Product}\";

		public const string MenuName_File = "File";
		public const string MenuName_Edit = "Edit";
		public const string MenuName_Evaluate = "Evaluate";
		public const string MenuName_View = "View";

		public const string ThemeDark = "Dark";
		public const string ThemeLight = "Light";

		public const string ViewToolbars = "Show Toolbars";
		public const string ViewExplorer = "Show Explorer";
		public const string ViewTabs = "Show Tabs";

		public const string OptionInterface = "Interface";
	}
}
