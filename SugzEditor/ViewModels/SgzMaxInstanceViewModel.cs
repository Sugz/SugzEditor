using GalaSoft.MvvmLight;
using SugzEditor.Models;
using SugzEditor.Src;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugzEditor.ViewModels
{
	public class SgzMaxInstanceViewModel : ViewModelBase
	{

		public IntPtr Handle { get; private set; }


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
					MessengerInstance.Send(new ActiveMaxInstancesMessage(this));
					Send($"SugzEditor is {(value ? "connected" : "disconnected")}");
				}
			}
		}

		public SgzMaxInstanceViewModel(IntPtr handle)
		{
			Handle = handle;
			Title = NativeMethods.GetCaptionOfWindow(handle).Replace("Autodesk ", ""); ;
		}


		public bool Send(string cmd)
		{
			if (GetMacroRecorder(Handle) is IntPtr macroRecorder)
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
			IEnumerable<IntPtr> children = maxChildren.Where(x => NativeMethods.GetClassNameOfWindow(x) == "MXS_Scintilla");
			if (children.Count() != 0 && children.Last() is IntPtr macroRecorder)
				return macroRecorder;
			return null;
		}
	}
}
