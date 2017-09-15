using System.Windows;
using SugzEditor.ViewModels;
using SugzEditor.Controls;
using Microsoft.Practices.ServiceLocation;

namespace SugzEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : SgzWindow
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) =>
            {
                ServiceLocator.Current.GetInstance<MainViewModel>().SaveConfig();
                ViewModelLocator.Cleanup();
            };
        }
    }
}