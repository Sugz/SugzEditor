using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Management;
using System.Diagnostics.CodeAnalysis;

namespace ConsoleApp1
{
	class Program
	{
		
	

		static void Main(string[] args)
		{
			Process[] processlist = Process.GetProcesses();

			foreach (Process process in processlist)
			{
				IntPtr handle = process.MainWindowHandle;
				int cls_max_length = 1000;
				StringBuilder classText = new StringBuilder("", cls_max_length + 5);
				GetClassName(handle, classText, cls_max_length + 2);
				var test = classText.ToString();
				Console.WriteLine(test);
				if (test == "#32770")
				{

				}
			}
			/*
			Process[] processlist = Process.GetProcessesByName("3dsmax");
			List<IntPtr> maxChildren = GetChildWindows(processlist[0].MainWindowHandle);
			var test = maxChildren.Where(x => GetClassNameOfWindow(x) == "#32770").ToArray();
			foreach (IntPtr hwnd in test)
			{
				var children = GetChildWindows(hwnd);

				string className = GetClassNameOfWindow(hwnd);
				string title = GetCaptionOfWindow(hwnd);
				if (className == "#32770" && title == "MAXScript Listener")
				{
					Console.WriteLine(GetCaptionOfWindow(hwnd));
				}
				Console.WriteLine(GetClassNameOfWindow(hwnd));
				if (GetCaptionOfWindow(hwnd) == "MAXScript")
				{
					const int WM_GETTEXT = 0x0D;
					StringBuilder sb = new StringBuilder();
					SendMessage(hwnd, WM_GETTEXT, 100, sb);
					Console.WriteLine(sb.ToString());
				}
			}

			
			//IntPtr childHandle = maxChildren.Where(x => GetClassNameOfWindow(x) == "MXS_Scintilla").First();





			//WatchForProcessStart("3dsmax.exe");
			//WatchForProcessEnd("3dsmax.exe");

			Automation.AddAutomationEventHandler(WindowPattern.WindowOpenedEvent, AutomationElement.RootElement, TreeScope.Subtree, (sender, e) =>
			{
				AutomationElement src = sender as AutomationElement;
				if (src != null)
				{
					Console.WriteLine("Class : " + src.Current.ClassName);
					Console.WriteLine("Title : " + src.Current.Name);
					Console.WriteLine("Handle: " + src.Current.NativeWindowHandle);
				}
			});
			*/
			Console.ReadKey(true);
		}


		#region Process Watcher WMI


		private static ManagementEventWatcher WatchForProcessStart(string processName)
		{
			string queryString =
				"SELECT TargetInstance" +
				"  FROM __InstanceCreationEvent " +
				"WITHIN  10 " +
				" WHERE TargetInstance ISA 'Win32_Process' " +
				"   AND TargetInstance.Name = '" + processName + "'";

			// The dot in the scope means use the current machine
			string scope = @"\\.\root\CIMV2";

			// Create a watcher and listen for events
			ManagementEventWatcher watcher = new ManagementEventWatcher(scope, queryString);
			watcher.EventArrived += ProcessStarted;
			watcher.Start();
			return watcher;
		}

		private static ManagementEventWatcher WatchForProcessEnd(string processName)
		{
			string queryString =
				"SELECT TargetInstance" +
				"  FROM __InstanceDeletionEvent " +
				"WITHIN  10 " +
				" WHERE TargetInstance ISA 'Win32_Process' " +
				"   AND TargetInstance.Name = '" + processName + "'";

			// The dot in the scope means use the current machine
			string scope = @"\\.\root\CIMV2";

			// Create a watcher and listen for events
			ManagementEventWatcher watcher = new ManagementEventWatcher(scope, queryString);
			watcher.EventArrived += ProcessEnded;
			watcher.Start();
			return watcher;
		}

		private static void ProcessEnded(object sender, EventArrivedEventArgs e)
		{
			ManagementBaseObject targetInstance = (ManagementBaseObject)e.NewEvent.Properties["TargetInstance"].Value;
			string processName = targetInstance.Properties["Name"].Value.ToString();
			Console.WriteLine(String.Format("{0} process ended", processName));
		}

		private static void ProcessStarted(object sender, EventArrivedEventArgs e)
		{
			ManagementBaseObject targetInstance = (ManagementBaseObject)e.NewEvent.Properties["TargetInstance"].Value;
			string processName = targetInstance.Properties["Name"].Value.ToString();
			Console.WriteLine(String.Format("{0} process started", processName));
		} 


		#endregion Process Watcher WMI




		/*
	static void Main(string[] args)
	{

		Process[] processlist = Process.GetProcesses();
		var maxProcess = processlist.Where(x => x.ProcessName == "3dsmax");
		foreach (Process process in maxProcess)
		{
			string title = GetCaptionOfWindow(process.MainWindowHandle);
			process.EnableRaisingEvents = true;
			process.Exited += (s, e) => Console.WriteLine($"{title} exited with exit code {process.ExitCode.ToString()}"); ;
			//List<IntPtr> maxChildren = GetChildWindows(process.MainWindowHandle);
			//IntPtr childHandle = maxChildren.Where(x => GetClassNameOfWindow(x) == "MXS_Scintilla").Last();
			//string filePath = @"D:\test.ms";
			//string cmd = $"fileIn @\"{filePath}\" -- Send from SugzEditor\r\n";
			//SendMessage(childHandle, 0x000C, 0, cmd);
			//SendMessage(childHandle, 0x0102, 0x0D, null);
		}

		//Automation.AddAutomationEventHandler(WindowPattern.WindowOpenedEvent, AutomationElement.RootElement, TreeScope.Subtree, (sender, e) =>
		//{
		//	AutomationElement src = sender as AutomationElement;
		//	if (src != null)
		//	{
		//		Console.WriteLine("Class : " + src.Current.ClassName);
		//		Console.WriteLine("Title : " + src.Current.Name);
		//		Console.WriteLine("Handle: " + src.Current.NativeWindowHandle);
		//	}
		//});

		//Automation.AddAutomationEventHandler(WindowPattern.WindowClosedEvent, AutomationElement.RootElement, TreeScope.Subtree, (sender, e) =>
		//{
		//	AutomationElement src = sender as AutomationElement;
		//	if (src != null)
		//	{
		//		Console.WriteLine("Class : " + src.Current.ClassName);
		//		Console.WriteLine("Title : " + src.Current.Name);
		//		Console.WriteLine("Handle: " + src.Current.NativeWindowHandle);
		//	}
		//});

		//Console.WriteLine("Press any key to quit...");
		Console.ReadKey(true);
	}
	*/
		[DllImport("user32")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr i);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int GetWindowTextLength(IntPtr hWnd);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		[DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		static extern int GetClassName(IntPtr hwnd, StringBuilder lpClassName, int nMaxCount);

		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, string lParam);
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
		/// Delegate for the EnumChildWindows method
		/// </summary>
		/// <param name="hWnd">Window handle</param>
		/// <param name="parameter">Caller-defined variable; we use it for a pointer to our list</param>
		/// <returns>True to continue enumerating, false to bail.</returns>
		public delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);


		private static string GetCaptionOfWindow(IntPtr hwnd)
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
		/// 
		/// </summary>
		/// <param name="hwnd"></param>
		/// <returns></returns>
		private static string GetClassNameOfWindow(IntPtr hwnd)
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





