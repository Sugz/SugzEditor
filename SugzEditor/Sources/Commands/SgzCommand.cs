using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace SugzEditor.Src.Commands
{
	/// <summary>
	/// A command whose sole purpose is to relay its functionality to other
	/// objects by invoking delegates. The default return value for the CanExecute
	/// method is 'true'.  This class does not allow you to accept command parameters in the
	/// Execute and CanExecute callback methods.
	/// </summary>
	/// <remarks>If you are using this class in WPF4.5 or above, you need to use the 
	/// GalaSoft.MvvmLight.CommandWpf namespace (instead of GalaSoft.MvvmLight.Command).
	/// This will enable (or restore) the CommandManager class which handles
	/// automatic enabling/disabling of controls based on the CanExecute delegate.</remarks>
	public class SgzCommand : ObservableObject, ICommand
	{
		private readonly WeakAction _execute;
		private readonly WeakFunc<bool> _canExecute;

		/*
		private Key _GestureKey;
		public Key GestureKey
		{
			get => _GestureKey;
			set => Set(ref _GestureKey, value);
		}

		private ModifierKeys _GestureModifiers;
		public ModifierKeys GestureModifiers
		{
			get => _GestureModifiers;
			set => Set(ref _GestureModifiers, value);
		}

		private MouseAction _MouseGesture;
		public MouseAction MouseGesture
		{
			get => _MouseGesture;
			set => Set(ref _MouseGesture, value);
		}

		private string _Text;
		public string Text
		{
			get => _Text;
			set => Set(ref _Text, value);
		}
		*/

		public Key GestureKey { get; set; }
		public ModifierKeys GestureModifiers { get; set; }
		public MouseAction MouseGesture { get; set; }
		public string Text { get; set; }


		/// <summary>
		/// Initializes a new instance of the RelayCommand class.
		/// </summary>
		/// <param name="execute">The execution logic. IMPORTANT: Note that closures are not supported at the moment
		/// due to the use of WeakActions (see http://stackoverflow.com/questions/25730530/). </param>
		/// <param name="canExecute">The execution status logic.</param>
		/// <exception cref="ArgumentNullException">If the execute argument is null. IMPORTANT: Note that closures are not supported at the moment
		/// due to the use of WeakActions (see http://stackoverflow.com/questions/25730530/). </exception>
		public SgzCommand(
			Action execute, 
			Func<bool> canExecute = null,
			Key key = Key.None,
			ModifierKeys modifiers = ModifierKeys.None,
			MouseAction mouse = MouseAction.None,
			string text = null)
		{
			if (execute == null)
				throw new ArgumentNullException("execute");
			_execute = new WeakAction(execute);

			if (canExecute != null)
				_canExecute = new WeakFunc<bool>(canExecute);

			GestureKey = key;
			GestureModifiers = modifiers;
			MouseGesture = mouse;
			Text = text;
		}


		private EventHandler _requerySuggestedLocal;

		/// <summary>
		/// Occurs when changes occur that affect whether the command should execute.
		/// </summary>
		public event EventHandler CanExecuteChanged
		{
			add
			{
				if (_canExecute != null)
				{
					// add event handler to local handler backing field in a thread safe manner
					EventHandler handler2;
					EventHandler canExecuteChanged = _requerySuggestedLocal;

					do
					{
						handler2 = canExecuteChanged;
						EventHandler handler3 = (EventHandler)Delegate.Combine(handler2, value);
						canExecuteChanged = System.Threading.Interlocked.CompareExchange<EventHandler>(
							ref _requerySuggestedLocal,
							handler3,
							handler2);
					}
					while (canExecuteChanged != handler2);

					CommandManager.RequerySuggested += value;
				}
			}

			remove
			{
				if (_canExecute != null)
				{
					// removes an event handler from local backing field in a thread safe manner
					EventHandler handler2;
					EventHandler canExecuteChanged = this._requerySuggestedLocal;

					do
					{
						handler2 = canExecuteChanged;
						EventHandler handler3 = (EventHandler)Delegate.Remove(handler2, value);
						canExecuteChanged = System.Threading.Interlocked.CompareExchange<EventHandler>(
							ref this._requerySuggestedLocal,
							handler3,
							handler2);
					}
					while (canExecuteChanged != handler2);

					CommandManager.RequerySuggested -= value;
				}
			}
		}


		/// <summary>
		/// Raises the <see cref="CanExecuteChanged" /> event.
		/// </summary>
		[SuppressMessage(
			"Microsoft.Performance",
			"CA1822:MarkMembersAsStatic",
			Justification = "The this keyword is used in the Silverlight version")]
		[SuppressMessage(
			"Microsoft.Design",
			"CA1030:UseEventsWhereAppropriate",
			Justification = "This cannot be an event")]
		public void RaiseCanExecuteChanged()
		{
			CommandManager.InvalidateRequerySuggested();
		}

		/// <summary>
		/// Defines the method that determines whether the command can execute in its current state.
		/// </summary>
		/// <param name="parameter">This parameter will always be ignored.</param>
		/// <returns>true if this command can be executed; otherwise, false.</returns>
		public bool CanExecute(object parameter)
		{
			return _canExecute == null
				|| (_canExecute.IsStatic || _canExecute.IsAlive)
					&& _canExecute.Execute();
		}

		/// <summary>
		/// Defines the method to be called when the command is invoked. 
		/// </summary>
		/// <param name="parameter">This parameter will always be ignored.</param>
		public virtual void Execute(object parameter)
		{
			if (CanExecute(parameter)
				&& _execute != null
				&& (_execute.IsStatic || _execute.IsAlive))
			{
				_execute.Execute();
			}
		}
	}
}
