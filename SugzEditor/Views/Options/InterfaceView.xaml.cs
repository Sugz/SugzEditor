using SugzEditor.Themes.Colors;
using SugzEditor.ViewModels;
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

namespace SugzEditor.Views.Options
{
    /// <summary>
    /// Interaction logic for ViewView.xaml
    /// </summary>
    public partial class InterfaceView : UserControl
    {
		private bool IsOpen = false;
		private MainViewModel _MainViewModel;

		public InterfaceView()
        {
            InitializeComponent();
			_MainViewModel = DataContext as MainViewModel;
			ThemeComboBox.SelectedIndex = _MainViewModel.IsDarkTheme ? 0 : 1;
			IsOpen = true;
		}

		private void OnThemeComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			// Don't reset the theme when setting ThemeComboBox.SelectedIndex on the constructor
			if (IsOpen)
			{
				bool isDarkTheme = ThemeComboBox.SelectedIndex == 0;
				_MainViewModel.SetThemeCommand.Execute(isDarkTheme);
			}
		}

		private void OnSliderRightClick(object sender, MouseButtonEventArgs e)
		{
			(sender as Slider).Value = 1;
		}
	}
}
