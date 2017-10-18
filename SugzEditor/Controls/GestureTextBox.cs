using SugzEditor.Src;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace SugzEditor.Controls
{
    public class GestureTextBox : TextBox
    {
		private List<Key> _PressedKeys = new List<Key>();
		private ModifierKeys _ModifierKeys = ModifierKeys.None;
		private Key _Key = Key.None;

		public GestureTextBox()
		{
			PreviewKeyDown += TextBox_PreviewKeyDown;
			KeyUp += TextBox_KeyUp;
		}
		
		void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{

			if (_PressedKeys.Contains(e.Key))
				return;
			_PressedKeys.Add(e.Key);

			if (e.Key != Key.None)
			{
				if (_PressedKeys.Count == 1)
				{
					if (Keyboard.Modifiers != ModifierKeys.None)
						_ModifierKeys = Keyboard.Modifiers;
					else
					{
						_Key = e.Key;
						Text = _Key.ToString();
					}
				}
				if (_PressedKeys.Count == 2)
				{
					if (Keyboard.Modifiers != ModifierKeys.None && _ModifierKeys == ModifierKeys.None)
					{
						_ModifierKeys = Keyboard.Modifiers;
						Text = HelperMethods.ModifierKeysToString(Keyboard.Modifiers) + _Key.ToString();
					}
					else if (_ModifierKeys != ModifierKeys.None && Keyboard.Modifiers == _ModifierKeys)
					{
						_Key = e.Key;
						Text = HelperMethods.ModifierKeysToString(Keyboard.Modifiers) + _Key.ToString();
					}
				}
			}
			e.Handled = true;
		}

		private void TextBox_KeyUp(object sender, KeyEventArgs e)
		{
			_PressedKeys.Remove(e.Key);
			if (_PressedKeys.Count == 0)
			{
				_ModifierKeys = ModifierKeys.None;
				_Key = Key.None;
			}
			e.Handled = true;

		}

		private bool? IsKeyAllowed(Key key)
		{
			switch (key)
			{
				case Key.Space:
				case Key.PageUp:
				case Key.PageDown:
				case Key.End:
				case Key.Home:
				case Key.Left:
				case Key.Up:
				case Key.Right:
				case Key.Down:
				case Key.D0:
				case Key.D1:
				case Key.D2:
				case Key.D3:
				case Key.D4:
				case Key.D5:
				case Key.D6:
				case Key.D7:
				case Key.D8:
				case Key.D9:
				case Key.A:
				case Key.B:
				case Key.C:
				case Key.D:
				case Key.E:
				case Key.F:
				case Key.G:
				case Key.H:
				case Key.I:
				case Key.J:
				case Key.K:
				case Key.L:
				case Key.M:
				case Key.N:
				case Key.O:
				case Key.P:
				case Key.Q:
				case Key.R:
				case Key.S:
				case Key.T:
				case Key.U:
				case Key.V:
				case Key.W:
				case Key.X:
				case Key.Y:
				case Key.Z:
				case Key.NumPad0:
				case Key.NumPad1:
				case Key.NumPad2:
				case Key.NumPad3:
				case Key.NumPad4:
				case Key.NumPad5:
				case Key.NumPad6:
				case Key.NumPad7:
				case Key.NumPad8:
				case Key.NumPad9:
				case Key.F1:
				case Key.F2:
				case Key.F3:
				case Key.F4:
				case Key.F5:
				case Key.F6:
				case Key.F7:
				case Key.F8:
				case Key.F9:
				case Key.F10:
				case Key.F11:
				case Key.F12:
				case Key.F13:
				case Key.F14:
				case Key.F15:
				case Key.F16:
				case Key.F17:
				case Key.F18:
				case Key.F19:
				case Key.F20:
				case Key.F21:
				case Key.F22:
				case Key.F23:
				case Key.F24:
					return true;
				case Key.LeftShift:
				case Key.RightShift:
				case Key.LeftCtrl:
				case Key.RightCtrl:
				case Key.LeftAlt:
				case Key.RightAlt:
					return null;
				default:
					return false;
			}
		}
	}
}
