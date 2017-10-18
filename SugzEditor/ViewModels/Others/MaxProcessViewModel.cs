using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using SugzEditor.Messages;
using SugzEditor.Src;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugzEditor.ViewModels
{
	public class MaxProcessViewModel : ViewModelBase
	{

		public Process Process { get; private set; }


		private string _Title;
		/// <summary>
		/// 
		/// </summary>
		public string Title
		{
			get => _Title;
			set
			{
				if (value != _Title)
				{
					Set(ref _Title, value);
				}
			}
		}


		private bool _IsChecked;
		/// <summary>
		/// 
		/// </summary>
		public bool IsChecked
		{
			get => _IsChecked;
			set
			{
				if (value != _IsChecked)
				{
					Set(ref _IsChecked, value);
					MessengerInstance.Send(new ActiveMaxProcessMessage(this));
					//Send($"SugzEditor is {(value ? "connected" : "disconnected")}");
					Send($"format \"SugzEditor is {(value ? "connected" : "disconnected")} \"");
				}
			}
		}


		public MaxProcessViewModel(Process process)
		{
			Process = process;
			Title = NativeMethods.GetCaptionOfWindow(process.MainWindowHandle).Replace("Autodesk ", "");

			// Notify the MainViewModel when the process is closed
			process.EnableRaisingEvents = true;
			process.Exited += (s, e) => DispatcherHelper.UIDispatcher.Invoke(() => MessengerInstance.Send(new ClosedMaxProcessMessage(this)));
		}


		public bool Send(string cmd)
		{
			if (GetMacroRecorder(Process.MainWindowHandle) is IntPtr macroRecorder)
			{
				NativeMethods.SendMessage(macroRecorder, 0x000C, 0, cmd);
				NativeMethods.SendMessage(macroRecorder, 0x0102, 0x0D);
				return true;
			}
			return false;
		}

		public static IntPtr? GetMacroRecorder(IntPtr handle)
		{
			List<IntPtr> maxChildren = NativeMethods.GetChildWindows(handle);
			IEnumerable<IntPtr> scintillas = maxChildren.Where(x => NativeMethods.GetClassNameOfWindow(x) == "MXS_Scintilla");
			if (scintillas.Count() != 0 && scintillas.Last() is IntPtr macroRecorder)
				return macroRecorder;
			else
			{
				IEnumerable<IntPtr> statusPanels = maxChildren.Where(x => NativeMethods.GetClassNameOfWindow(x) == "StatusPanel");
				if (statusPanels.Count() != 0 && statusPanels.Last() is IntPtr statusPanel)
				{
					IEnumerable<IntPtr> richEdits = maxChildren.Where(x => NativeMethods.GetClassNameOfWindow(x) == "RICHEDIT");
					if (richEdits.Count() != 0 && richEdits.First() is IntPtr _macroRecorder)
						return _macroRecorder;
				}
			}
			return null;
		}
	}
}