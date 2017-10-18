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
using Microsoft.Win32;

namespace ConsoleApp1
{
	class Program
	{
		static void Main(string[] args)
		{
			

			Console.ReadKey();
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


	}


	
}





