using System.IO;
using System.Windows;
using System.Windows.Input;
using SugzEditor.ViewModels;
using SugzEditor.Controls;
using GalaSoft.MvvmLight.Messaging;
using SugzEditor.Messages;
using SugzEditor.Src;
using System;

namespace SugzEditor.Views
{
	/// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : SgzWindow
    {
		public OptionsDialog OptionsDialog { get; set; }

		public MainWindow()
        {
            InitializeComponent();
			Closed += OnClosing;
			Messenger.Default.Register<OpenDialogMessage>(this, x =>
			{
				switch (x.WindowType)
				{
					case WindowType.Options:
						OpenOptionsView();
						break;
					default:
						break;
				}
			});
		}

		private void OnClosing(object sender, EventArgs e)
		{
			if (OptionsDialog != null)
				OptionsDialog.Close();
			ViewModelLocator.Cleanup();
		}

		private void OpenOptionsView()
		{
			if (OptionsDialog == null)
			{
				OptionsDialog = new OptionsDialog { Owner = this };
				OptionsDialog.Show();
			}
			OptionsDialog.Focus();
		}


	}
}
