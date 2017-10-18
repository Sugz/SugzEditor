using SugzEditor.ViewModels;
using SugzTools.Src;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SugzEditor.Views
{
    /// <summary>
    /// Interaction logic for ExplorerView.xaml
    /// </summary>
    public partial class ExplorerView : UserControl
    {
        public ExplorerView()
        {
            InitializeComponent();
        }

		private void TreeView_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if (Helpers.FindAnchestor<TreeViewItem>(e.OriginalSource as DependencyObject) is TreeViewItem treeViewItem)
			{
				//if (treeViewItem.DataContext is MCodeItem dataContext)
				//{
				//    if (e.ChangedButton == MouseButton.Middle)
				//        (Application.Current.Resources["Locator"] as ViewModelLocator).Data.CreateFile(); //TODO use datacontext
				//    //dataContext.IsSelected = true;
				//    dataContext.IsActive = true;
				//}
				//else if (treeViewItem.DataContext is SgzFolder)
				//    treeViewItem.IsExpanded = !treeViewItem.IsExpanded;
				if (treeViewItem.DataContext is FolderViewModel folder && folder.Children != null)
					treeViewItem.IsExpanded = !treeViewItem.IsExpanded;
			}
		}

		private void MaxFoldersBtn_Click(object sender, RoutedEventArgs e)
		{
			double height = ActualHeight - 100;
			if (foldersBtn.IsChecked == true)
			{
				if (maxFoldersBtn.IsChecked == false)
					foldersTv.MaxHeight = height;
				else
					maxFoldersTv.MaxHeight = foldersTv.MaxHeight = height / 2;
			}
			else maxFoldersTv.MaxHeight = height;

		}

		private void FoldersBtn_Click(object sender, RoutedEventArgs e)
		{
			double height = ActualHeight - 100;
			if (maxFoldersBtn.IsChecked == true)
			{
				if (foldersBtn.IsChecked == false)
					maxFoldersTv.MaxHeight = height;
				else
					maxFoldersTv.MaxHeight = foldersTv.MaxHeight = height / 2;
			}
			else foldersTv.MaxHeight = height;
		}
	}
}
