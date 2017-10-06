using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SugzEditor.Src
{
	public static class NativeMethods
	{
		[DllImport("user32")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr i);

		/// <summary>
		/// Delegate for the EnumChildWindows method
		/// </summary>
		/// <param name="hWnd">Window handle</param>
		/// <param name="parameter">Caller-defined variable; we use it for a pointer to our list</param>
		/// <returns>True to continue enumerating, false to bail.</returns>
		public delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int GetWindowTextLength(IntPtr hWnd);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		[DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		static extern int GetClassName(IntPtr hwnd, StringBuilder lpClassName, int nMaxCount);

		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, string lParam = null);
		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, StringBuilder lParam);


		/// <summary>
		/// Returns a list of child windows
		/// </summary>
		/// <param name="parent">Parent of the windows to return</param>
		/// <returns>List of child windows</returns>
		public static List<IntPtr> GetChildWindows(IntPtr parent)
		{
			List<IntPtr> result = new List<IntPtr>();
			GCHandle listHandle = GCHandle.Alloc(result);
			try
			{
				EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
				EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
			}
			finally
			{
				if (listHandle.IsAllocated)
					listHandle.Free();
			}
			return result;
		}

		/// <summary>
		/// Callback method to be used when enumerating windows.
		/// </summary>
		/// <param name="handle">Handle of the next window</param>
		/// <param name="pointer">Pointer to a GCHandle that holds a reference to the list to fill</param>
		/// <returns>True to continue the enumeration, false to bail</returns>
		private static bool EnumWindow(IntPtr handle, IntPtr pointer)
		{
			GCHandle gch = GCHandle.FromIntPtr(pointer);
			List<IntPtr> list = gch.Target as List<IntPtr>;
			if (list == null)
			{
				throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
			}
			list.Add(handle);
			//  You can modify this to check to see if you want to cancel the operation, then return a null here
			return true;
		}

		/// <summary>
		/// Return a window title from its handle
		/// </summary>
		/// <param name="hwnd"></param>
		/// <returns></returns>
		public static string GetCaptionOfWindow(IntPtr hwnd)
		{
			string caption = "";
			StringBuilder windowText = null;
			try
			{
				int max_length = GetWindowTextLength(hwnd);
				windowText = new StringBuilder("", GetWindowTextLength(hwnd) + 5);
				GetWindowText(hwnd, windowText, max_length + 2);

				if (!String.IsNullOrEmpty(windowText.ToString()) && !String.IsNullOrWhiteSpace(windowText.ToString()))
					caption = windowText.ToString();
			}
			catch (Exception ex)
			{
				caption = ex.Message;
			}
			finally
			{
				windowText = null;
			}
			return caption;
		}

		/// <summary>
		/// Return a window class name from its handle
		/// </summary>
		/// <param name="hwnd"></param>
		/// <returns></returns>
		public static string GetClassNameOfWindow(IntPtr hwnd)
		{
			string className = "";
			StringBuilder classText = null;
			try
			{
				int cls_max_length = 1000;
				classText = new StringBuilder("", cls_max_length + 5);
				GetClassName(hwnd, classText, cls_max_length + 2);

				if (!String.IsNullOrEmpty(classText.ToString()) && !String.IsNullOrWhiteSpace(classText.ToString()))
					className = classText.ToString();
			}
			catch (Exception ex)
			{
				className = ex.Message;
			}
			finally
			{
				classText = null;
			}
			return className;
		}
	}
}
