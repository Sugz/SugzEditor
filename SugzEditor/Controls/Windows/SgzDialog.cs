using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SugzEditor.Controls
{
	[TemplatePart(Name = "PART_Header", Type = typeof(DockPanel))]
	public class SgzDialog : Window
	{

		private DockPanel PART_Header;

		/// <summary>
		/// 
		/// </summary>
		//[Description(""), Category("")]
		[Browsable(false)]
		public object IconDatas
		{
			get => (object)GetValue(IconDatasProperty);
			set => SetValue(IconDatasProperty, value);
		}

		// DependencyProperty as the backing store for IconDatas
		public static readonly DependencyProperty IconDatasProperty = DependencyProperty.Register(
			"IconDatas",
			typeof(object),
			typeof(SgzDialog),
			new PropertyMetadata(null)
		);


		/// <summary>
		/// 
		/// </summary>
		[Description(""), Category("Brush")]
		// [Browsable(false)]
		public Brush HeaderBackground
		{
			get => (Brush)GetValue(HeaderBackgroundProperty);
			set => SetValue(HeaderBackgroundProperty, value);
		}

		// DependencyProperty as the backing store for HeaderBackground
		public static readonly DependencyProperty HeaderBackgroundProperty = DependencyProperty.Register(
			"HeaderBackground",
			typeof(Brush),
			typeof(SgzDialog),
			new PropertyMetadata(new SolidColorBrush(Colors.Transparent))
		);






		static SgzDialog()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(SgzDialog), new FrameworkPropertyMetadata(typeof(SgzDialog)));
		}
		public SgzDialog()
		{
			CommandBindings.Add(new CommandBinding(SystemCommands.ShowSystemMenuCommand, OnShowSystemMenu));
			CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, OnCloseWindow));
			CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, OnMinimizeWindow, OnCanMinimizeWindow));
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (GetTemplateChild("PART_Header") is DockPanel header)
			{
				PART_Header = header;
				PART_Header.MouseLeftButtonDown += delegate { DragMove(); };
				PART_Header.MouseRightButtonUp += delegate { OnShowSystemMenu(null, null); };
			}
		}


		protected override void OnRenderSizeChanged(SizeChangedInfo info)
		{
			base.OnRenderSizeChanged(info);

			if (WindowState == WindowState.Maximized)
				WindowState = WindowState.Normal;
		}


		#region System commands

		private void OnCanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = ResizeMode != ResizeMode.NoResize;
		}

		private void OnShowSystemMenu(object sender, ExecutedRoutedEventArgs e)
		{
			Point pointToWindow = Mouse.GetPosition(this);
			Point pointToScreen = PointToScreen(pointToWindow);
			SystemCommands.ShowSystemMenu(this, pointToScreen);
		}

		private void OnCloseWindow(object sender, ExecutedRoutedEventArgs e)
		{
			SystemCommands.CloseWindow(this);
		}

		private void OnMinimizeWindow(object sender, ExecutedRoutedEventArgs e)
		{
			SystemCommands.MinimizeWindow(this);
		}

		#endregion System commands

	}
}
