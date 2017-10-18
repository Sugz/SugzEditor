using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SugzEditor.Src
{
    public static class HelperMethods
    {
		public static string ModifierKeysToString(ModifierKeys keys)
		{
			switch (keys)
			{
				case ModifierKeys.Alt:
					return "Alt+";
				case ModifierKeys.Control:
					return "Ctrl+";
				case ModifierKeys.Shift:
					return "Shift+";
				case ModifierKeys.Windows:
					return "Win+";
				default:
					return null;
			}
		}

		public static string KeyToString(Key key)
		{
			switch (key)
			{
				case Key.None:
					return null;
				default:
					return key.ToString();
			}
		}
	}
}

