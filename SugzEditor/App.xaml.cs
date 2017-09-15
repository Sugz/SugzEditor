using System.Windows;
using GalaSoft.MvvmLight.Threading;
using SugzEditor.Settings;

namespace SugzEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }
    }
}
