using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using SugzEditor.Src.Settings;
using SugzEditor.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace SugzEditor
{
  

  /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
		static App()
		{
			//ViewLocator viewLocator = new ViewLocator();
			SimpleIoc.Default.Register<SettingsConfigProvider>();
			DispatcherHelper.Initialize();
		}

	}

	
}
