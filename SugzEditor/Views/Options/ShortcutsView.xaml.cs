using SugzEditor.Controls;
using SugzEditor.Src;
using SugzEditor.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    /// Interaction logic for ShortcutsView.xaml
    /// </summary>
    public partial class ShortcutsView : UserControl
    {

		private ShortcutDialog _ShortcutDialog;
		private SgzRelayCommand _CurrentCommand;


		#region Properties


		/// <summary>
		/// 
		/// </summary>
		[Description(""), Category("Common")]
		public string Header
		{
			get => (string)GetValue(HeaderProperty);
			set => SetValue(HeaderProperty, value);
		}

		/// <summary>
		/// 
		/// </summary>
		[Description(""), Category("Common")]
		public IEnumerable ItemsSource
		{
			get => (IEnumerable)GetValue(ItemsSourceProperty);
			set => SetValue(ItemsSourceProperty, value);
		}

		/// <summary>
		/// 
		/// </summary>
		[Description(""), Category("")]
		// [Browsable(false)]
		public Window Owner
		{
			get => (Window)GetValue(OwnerProperty);
			set => SetValue(OwnerProperty, value);
		}

		#endregion Properties

		
		#region Dependency Properties

		// DependencyProperty as the backing store for Header
		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
			"Header",
			typeof(string),
			typeof(ShortcutsView),
			new PropertyMetadata(null)
		);

		// DependencyProperty as the backing store for ItemsSource
		public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
			"ItemsSource",
			typeof(IEnumerable),
			typeof(ShortcutsView),
			new PropertyMetadata(null)
		);

		// DependencyProperty as the backing store for Owner
		public static readonly DependencyProperty OwnerProperty = DependencyProperty.Register(
			"Owner",
			typeof(Window),
			typeof(ShortcutsView)
		);

		#endregion Dependency Properties


		public ShortcutsView()
        {
            InitializeComponent();
		}
		

		private void Bd_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ClickCount == 2)
			{
				MainViewModel mainVm = DataContext as MainViewModel;
				ICommand[] commands = mainVm.UICommands.Concat(mainVm.DatasCommands.Concat(mainVm.TextEditorCommands.Concat(mainVm.MaxProcessCommands))).ToArray();
				_ShortcutDialog = new ShortcutDialog(_CurrentCommand, commands);
				_ShortcutDialog.Owner = Owner;
				if (!_ShortcutDialog.ShowDialog().Value)
				{
					Debug.WriteLine($"{_ShortcutDialog.ModifierKeys} + {_ShortcutDialog.Key}");
				}
				_ShortcutDialog.Focus();
			}
		}


		private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			_CurrentCommand = ((ListBox)sender).SelectedItem as SgzRelayCommand;
		}
	}
}
