using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace SugzEditor.Controls
{
    public class InputKeyTextBox : TextBox
    {
		int MaxKeyCount = 3;
		List<Key> PressedKeys = new List<Key>();
		List<Key> AllowedKeys = new List<Key>();

		public InputKeyTextBox()
		{
			//Init the allowed keys list
			AllowedKeys.AddRange(new Key[] { Key.LeftCtrl, Key.RightCtrl, Key.LeftShift, Key.RightShift, Key.LeftAlt, Key.RightAlt });
			AllowedKeys.AddRange(new Key[] { Key.A, Key.B, Key.C, Key.D, Key.E, Key.F, Key.G, Key.H, Key.I, Key.K, Key.L,
				Key.M, Key.N, Key.O, Key.P, Key.Q, Key.R, Key.S, Key.T, Key.U, Key.V, Key.W, Key.X, Key.Y, Key.Z });

			KeyDown += TextBox_KeyDown;
		}

		private void TextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Handled) return;

			//Check all previous keys to see if they are still pressed
			List<Key> KeysToRemove = new List<Key>();
			foreach (Key k in PressedKeys)
			{
				if (!Keyboard.IsKeyDown(k))
					KeysToRemove.Add(k);
			}

			//Remove all not pressed keys
			foreach (Key k in KeysToRemove)
				PressedKeys.Remove(k);

			//Add the key if max count is not reached
			if (PressedKeys.Count < MaxKeyCount)
				//Add the key if it is part of the allowed keys
				//if (AllowedKeys.Contains(e.Key))
				if (!PressedKeys.Contains(e.Key))
					PressedKeys.Add(e.Key);

			PrintKeys();

			e.Handled = true;
		}

		private void PrintKeys()
		{
			//Print all pressed keys
			string s = "";
			if (PressedKeys.Count == 0) return;

			foreach (Key k in PressedKeys)
				if (IsModifierKey(k))
					s += GetModifierKey(k) + " + ";
				else
					s += k + " + ";

			s = s.Substring(0, s.Length - 3);
			Text = s;
		}

		private bool IsModifierKey(Key k)
		{
			if (k == Key.LeftCtrl || k == Key.RightCtrl ||
				k == Key.LeftShift || k == Key.RightShift ||
				k == Key.LeftAlt || k == Key.RightAlt ||
				k == Key.LWin || k == Key.RWin)
				return true;
			else
				return false;
		}

		private ModifierKeys GetModifierKey(Key k)
		{
			if (k == Key.LeftCtrl || k == Key.RightCtrl)
				return ModifierKeys.Control;

			if (k == Key.LeftShift || k == Key.RightShift)
				return ModifierKeys.Shift;

			if (k == Key.LeftAlt || k == Key.RightAlt)
				return ModifierKeys.Alt;

			if (k == Key.LWin || k == Key.RWin)
				return ModifierKeys.Windows;

			return ModifierKeys.None;
		}
	}
}