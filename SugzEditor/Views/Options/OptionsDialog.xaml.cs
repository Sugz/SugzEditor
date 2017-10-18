using SugzEditor.Controls;
using SugzEditor.Models.Options;
using SugzEditor.ViewModels;
using SugzEditor.Views.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace SugzEditor.Views
{
    /// <summary>
    /// Interaction logic for OptionsDialog.xaml
    /// </summary>
    public partial class OptionsDialog : SgzDialog
    {

		#region Properties

		[Browsable(false)]
		public int BlurValue
		{
			get => (int)GetValue(BlurValueProperty);
			set => SetValue(BlurValueProperty, value);
		}


		#endregion Properties


		#region Dependency Properties

		// DependencyProperty as the backing store for BlurValue
		public static readonly DependencyProperty BlurValueProperty = DependencyProperty.Register(
			"BlurValue",
			typeof(int),
			typeof(OptionsDialog),
			new PropertyMetadata(0)
		);

		#endregion Dependency Properties


		public OptionsDialog()
        {
            InitializeComponent();
			Loaded += delegate { Owner = null; };
			Closed += delegate { ((MainWindow)Application.Current.MainWindow).OptionsDialog = null; };
		}

		//public void SetBlur(bool value)
		//{
		//	if(value)
		//	{
		//		BlurValue = 5;
		//		blurOverlay.Visibility = Visibility.Visible;
		//	}
		//	else
		//	{
		//		BlurValue = 0;
		//		blurOverlay.Visibility = Visibility.Collapsed;
		//	}
		//}

		private void OnTreeViewMouseUp(object sender, MouseButtonEventArgs e)
		{
			var tv = sender as TreeView;
			
			if (tv.ItemContainerGenerator.ContainerFromItem(tv.SelectedItem) is TreeViewItem item)
				item.IsExpanded = !item.IsExpanded;

			e.Handled = true;
		}

		private void OnTreeViewPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
		}

		private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			BaseOptionModel model = e.NewValue as BaseOptionModel;
			if (model.View != null && Resources[model.View] is DataTemplate template)
				ContentPresenter.ContentTemplate = template;
			else
				ContentPresenter.ContentTemplate = Resources["EmptyView"] as DataTemplate;
		}

		private void OnTreeViewFirstItemLoaded(object sender, RoutedEventArgs e)
		{
			if (sender is TreeViewItem item)
				item.IsSelected = true;
			TreeView.Focus();
		}
	}
}
