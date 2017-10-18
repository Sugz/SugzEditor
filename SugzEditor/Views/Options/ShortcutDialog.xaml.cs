using GalaSoft.MvvmLight.Command;
using SugzEditor.Src;
using SugzEditor.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SugzEditor.Controls
{
	/// <summary>
	/// Interaction logic for ShortcutDialog.xaml
	/// </summary>
	public partial class ShortcutDialog : Window, INotifyPropertyChanged
	{

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		#endregion INotifyPropertyChanged


		#region Fields

		private List<Key> _PressedKeys = new List<Key>();
		private ModifierKeys _ModifierKeys = ModifierKeys.None;
		private Key _Key = Key.None;
		private SgzRelayCommand _Command;
		private ICommand[] _Commands;
		private SgzRelayCommand _ConflictedCommand;

		#endregion Fields


		#region Properties

		public ModifierKeys ModifierKeys { get; set; } = ModifierKeys.None;
		public Key Key { get; set; } = Key.None;

		private string _CommandName;
		public string CommandName
		{
			get => _CommandName;
			set
			{
				_CommandName = value;
				OnPropertyChanged();
			}
		}

		private string _Shortcut;
		public string Shortcut
		{
			get => _Shortcut;
			set
			{
				_Shortcut = value;
				OnPropertyChanged();
			}
		}

		private string _InvalideShortcut;
		public string InvalideShortcut
		{
			get => _InvalideShortcut;
			set
			{
				_InvalideShortcut = value;
				OnPropertyChanged();
			}
		}


		#endregion Properties


		#region Constructor


		public ShortcutDialog(SgzRelayCommand command, ICommand[] commands)
		{
			InitializeComponent();
			DataContext = this;
			_Command = command;
			_Commands = commands;
			CommandName = $"Set shortcut for {command.Text}";
			PreviewKeyDown += ShortcutDialog_PreviewKeyDown;
			KeyUp += ShortcutDialog_KeyUp;
			InputTextBlock.Focus();

			//Loaded += delegate { SetBlur(true); };
			//Closed += delegate { SetBlur(false); };
		}


		#endregion Constructor


		#region Methods


		//private void SetBlur(bool value)
		//{
		//	if (Owner is OptionsDialog optionsDialog)
		//	{
		//		optionsDialog.SetBlur(value);
		//	}
		//}


		private void IsValid()
		{
			foreach (ICommand command in _Commands)
			{
				if (command is SgzRelayCommand sgzcommand && sgzcommand.InputGestureText == Shortcut)
				{
					_ConflictedCommand = sgzcommand;
					InvalideShortcut = $"This shortcut is already use by {sgzcommand.Text}";
				}
			}
		} 


		private void Accept()
		{
			if (_ConflictedCommand != null)
			{
				_ConflictedCommand.GestureKey = Key.None;
				_ConflictedCommand.GestureModifiers = ModifierKeys.None;

			}
			_Command.GestureKey = Key;
			_Command.GestureModifiers = ModifierKeys;

			Close();
		}


		#endregion Methods


		#region Event Handler


		private void ShortcutDialog_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
				Close();
			else if (e.Key == Key.Enter)
				Accept();
			else
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
							Shortcut = Key.ToString();
						}
					}
					if (_PressedKeys.Count == 2)
					{
						if (Keyboard.Modifiers != ModifierKeys.None && _ModifierKeys == ModifierKeys.None)
						{
							_ModifierKeys = Keyboard.Modifiers;
							Shortcut = HelperMethods.ModifierKeysToString(Keyboard.Modifiers) + _Key.ToString();
						}
						else if (_ModifierKeys != ModifierKeys.None && Keyboard.Modifiers == _ModifierKeys)
						{
							_Key = e.Key;
							Shortcut = HelperMethods.ModifierKeysToString(Keyboard.Modifiers) + _Key.ToString();
						}
					}
					e.Handled = true;
				}
			}

		}


		private void ShortcutDialog_KeyUp(object sender, KeyEventArgs e)
		{
			_PressedKeys.Remove(e.Key);
			if (_PressedKeys.Count == 0)
			{
				IsValid();
				ModifierKeys = _ModifierKeys;
				Key = _Key;
				_ModifierKeys = ModifierKeys.None;
				_Key = Key.None;
			}
			e.Handled = true;
		}


		private void CancelClick(object sender, RoutedEventArgs e)
		{
			Close();
		}


		private void AcceptClick(object sender, RoutedEventArgs e)
		{
			Accept();
		} 


		#endregion Event Handler

	}
}
