using GalaSoft.MvvmLight.CommandWpf;
using SugzEditor.Themes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SugzEditor.Src
{
    public class SgzRelayCommand : RelayCommand, INotifyPropertyChanged
	{
		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		#endregion INotifyPropertyChanged

		private Key _GestureKey;
		public Key GestureKey
		{
			get => _GestureKey;
			set
			{
				_GestureKey = value;
				SetInputGestureText();
				UpdateProperties();
			}
		}

		private ModifierKeys _GestureModifiers;
		public ModifierKeys GestureModifiers
		{
			get => _GestureModifiers;
			set
			{
				_GestureModifiers = value;
				SetInputGestureText();
				UpdateProperties();
			}
		}

		private MouseAction _MouseGesture;
		public MouseAction MouseGesture
		{
			get => _MouseGesture;
			set
			{
				_MouseGesture = value;
				UpdateProperties();
			}
		}

		private string _Text;
		public string Text
		{
			get => _Text;
			set
			{
				_Text = value;
				SetTooltip();
				UpdateProperties();
			}
		}

		private string _InputGestureText = null;
		public string InputGestureText
		{
			get => _InputGestureText;
			set
			{
				_InputGestureText = value;
				SetTooltip();
				OnPropertyChanged();
			}
		}

		private string _Tooltip;
		public string Tooltip
		{
			get => _Tooltip;
			set
			{
				_Tooltip = value;
				OnPropertyChanged();
			}
		}

		public object Icon { get; set; }


		/// <summary>
		/// Initializes a new instance of the RelayCommand class.
		/// </summary>
		/// <param name="execute">The execution logic.</param>
		/// <param name="canExecute">The execution status logic.</param>
		/// <param name="key"></param>
		/// <param name="modifiers"></param>
		/// <param name="mouse"></param>
		/// <param name="text"></param>
		/// <exception cref="ArgumentNullException">If the execute argument is null.</exception>
		public SgzRelayCommand(
			Action execute,
			Func<bool> canExecute = null,
			Key key = Key.None,
			ModifierKeys modifiers = ModifierKeys.None,
			MouseAction mouse = MouseAction.None,
			string text = null,
			string icon = null) : base(execute, canExecute)
		{
			GestureKey = key;
			GestureModifiers = modifiers;
			MouseGesture = mouse;
			Text = text;

			if (icon != null)
				Icon = IconProvider<object>.Get(icon);
			
		}


		private void UpdateProperties()
		{
			OnPropertyChanged("GestureKey");
			OnPropertyChanged("GestureModifiers");
			OnPropertyChanged("MouseGesture");
			OnPropertyChanged("Text");
		}

		private void SetInputGestureText()
		{
			InputGestureText = null;
			string s = HelperMethods.ModifierKeysToString(GestureModifiers);
			if (s != null)
				InputGestureText = s;
			s = HelperMethods.KeyToString(GestureKey);
			if (s != null)
				InputGestureText += s;
		}

		private void SetTooltip()
		{
			Tooltip = null;
			if (Text != null)
				Tooltip = Text;
			if (InputGestureText != null && InputGestureText != string.Empty)
				Tooltip += $" ({InputGestureText})";
		}

	}


	public class SgzRelayCommand<T> : RelayCommand<T>, INotifyPropertyChanged
	{
		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		#endregion INotifyPropertyChanged

		private Key _GestureKey;
		public Key GestureKey
		{
			get => _GestureKey;
			set
			{
				_GestureKey = value;
				SetInputGestureText();
				UpdateProperties();
			}
		}

		private ModifierKeys _GestureModifiers;
		public ModifierKeys GestureModifiers
		{
			get => _GestureModifiers;
			set
			{
				_GestureModifiers = value;
				SetInputGestureText();
				UpdateProperties();
			}
		}

		private MouseAction _MouseGesture;
		public MouseAction MouseGesture
		{
			get => _MouseGesture;
			set
			{
				_MouseGesture = value;
				UpdateProperties();
			}
		}

		private string _Text;
		public string Text
		{
			get => _Text;
			set
			{
				_Text = value;
				SetTooltip();
				UpdateProperties();
			}
		}

		private string _InputGestureText = null;
		public string InputGestureText
		{
			get => _InputGestureText;
			set
			{
				_InputGestureText = value;
				SetTooltip();
				OnPropertyChanged();
			}
		}

		private string _Tooltip;
		public string Tooltip
		{
			get => _Tooltip;
			set
			{
				_Tooltip = value;
				OnPropertyChanged();
			}
		}

		public object Icon { get; set; }


		/// <summary>
		/// Initializes a new instance of the RelayCommand class.
		/// </summary>
		/// <param name="execute">The execution logic.</param>
		/// <param name="canExecute">The execution status logic.</param>
		/// <param name="key"></param>
		/// <param name="modifiers"></param>
		/// <param name="mouse"></param>
		/// <param name="text"></param>
		/// <exception cref="ArgumentNullException">If the execute argument is null.</exception>
		public SgzRelayCommand(
			Action<T> execute, 
			Func<T, bool> canExecute = null,
			Key key = Key.None,
			ModifierKeys modifiers = ModifierKeys.None,
			MouseAction mouse = MouseAction.None,
			string text = null,
			string icon = null) : base(execute, canExecute)
		{
			GestureKey = key;
			GestureModifiers = modifiers;
			MouseGesture = mouse;
			Text = text;

			if (icon != null)
				Icon = IconProvider<object>.Get(icon);
		}

		private void UpdateProperties()
		{
			OnPropertyChanged("GestureKey");
			OnPropertyChanged("GestureModifiers");
			OnPropertyChanged("MouseGesture");
			OnPropertyChanged("Text");
		}

		private void SetInputGestureText()
		{
			InputGestureText = null;
			string s = HelperMethods.ModifierKeysToString(GestureModifiers);
			if (s != null)
				InputGestureText = s;
			s = HelperMethods.KeyToString(GestureKey);
			if (s != null)
				InputGestureText += s;
		}

		private void SetTooltip()
		{
			Tooltip = null;
			if (Text != null)
				Tooltip = Text;
			if (InputGestureText != null && InputGestureText != string.Empty)
				Tooltip += $" ({InputGestureText})";
		}

	}
}
