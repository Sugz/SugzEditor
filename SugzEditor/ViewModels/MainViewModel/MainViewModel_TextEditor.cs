using GalaSoft.MvvmLight;
using SugzEditor.Src;
using SugzEditor.Src.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace SugzEditor.ViewModels
{
	public partial class MainViewModel : ViewModelBase
	{

		public ICommand[] TextEditorCommands { get; set; }

		#region Commands


		#region UndoCommand
		private SgzRelayCommand _UndoCommand;
		public ICommand UndoCommand => _UndoCommand ?? 
			(_UndoCommand = new SgzRelayCommand(Undo, key:Key.Z, modifiers:ModifierKeys.Control, text:"Undo", icon: "Undo_16x"));

		private void Undo()
		{
			SetStatus("Undo command...", true);
		}
		#endregion UndoCommand


		#region RedoCommand
		private SgzRelayCommand _RedoCommand;
		public ICommand RedoCommand => _RedoCommand ?? 
			(_RedoCommand = new SgzRelayCommand(Redo, key: Key.Y, modifiers: ModifierKeys.Control, text: "Redo", icon: "Redo_16x"));

		private void Redo()
		{
			SetStatus("Redo command...", true);
		}
		#endregion RedoCommand



		#endregion Commands


		private void GetTextEditorCommands()
		{
			TextEditorCommands = new ICommand[]
			{
				UndoCommand, RedoCommand
			};
		}
	}
}
