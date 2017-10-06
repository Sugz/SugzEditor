using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace SugzEditor.Controls
{
	[TemplatePart(Name = "PART_Button", Type = typeof(Border)),
	TemplatePart(Name = "PART_Toggle", Type = typeof(Border)),
	TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(MenuItem))]
	public class SgzSplitButton : ItemsControl
	{

		#region Fields

		// Template Parts
		private Border PART_Button;
		private Border PART_Toggle;
		private Popup PART_Popup;

		#endregion Fields


		#region Properties

		/// <summary>
		/// Get or set the main button content
		/// </summary>
		[Description("The main button content"), Category("Common")]
		public object Content
		{
			get => (GetValue(ContentProperty));
			set => SetValue(ContentProperty, value);
		}

		/// <summary>
		/// Get or set the button width;
		/// </summary>
		[Description("The button width"), Category("Layout")]
		[TypeConverter(typeof(LengthConverter))]
		public double ButtonWidth
		{
			get => (double)GetValue(ButtonWidthProperty);
			set => SetValue(ButtonWidthProperty, value);
		}

		/// <summary>
		/// Get or set the expanded state (the popup visiblility)
		/// </summary>
		[Description("The expanded state (the popup visiblility)"), Category("Common")]
		public bool IsExpanded
		{
			get => (bool)GetValue(IsExpandedProperty);
			set => SetValue(IsExpandedProperty, value);
		}

		/// <summary>
		/// 
		/// </summary>
		[Description(""), Category("Common")]
		public bool AllowDisableExpand
		{
			get => (bool)GetValue(AllowDisableExpandProperty);
			set => SetValue(AllowDisableExpandProperty, value);
		}

		/// <summary>
		/// 
		/// </summary>
		[Browsable(false)]
		public bool IsButtonMouseOver
		{
			get => (bool)GetValue(IsButtonMouseOverProperty);
			set => SetValue(IsButtonMouseOverProperty, value);
		}

		/// <summary>
		/// 
		/// </summary>
		[Browsable(false)]
		public bool IsButtonPressed
		{
			get => (bool)GetValue(IsButtonPressedProperty);
			set => SetValue(IsButtonPressedProperty, value);
		}

		/// <summary>
		/// 
		/// </summary>
		[Browsable(false)]
		public bool IsToggleMouseOver
		{
			get => (bool)GetValue(IsToggleMouseOverProperty);
			set => SetValue(IsToggleMouseOverProperty, value);
		}

		/// <summary>
		/// 
		/// </summary>
		[Browsable(false)]
		public bool IsTogglePressed
		{
			get => (bool)GetValue(IsTogglePressedProperty);
			set => SetValue(IsTogglePressedProperty, value);
		}

		/// <summary>
		/// Get or sets the Command property. 
		/// </summary>
		[Description("The Command to execute on the main button. "), Category("Common")]
		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		/// <summary>
		/// Get or set the parameter to pass to the Command upon execution
		/// </summary>
		[Description("The parameter to pass to the Command upon execution. "), Category("Common")]
		public object CommandParameter
		{
			get { return GetValue(CommandParameterProperty); }
			set { SetValue(CommandParameterProperty, value); }
		}

		#endregion Properties


		#region Dependency Properties

		// DependencyProperty as the backing store for Content
		public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
			"Content",
			typeof(object),
			typeof(SgzSplitButton),
			new PropertyMetadata(null)
		);

		// DependencyProperty as the backing store for ButtonWidth
		public static readonly DependencyProperty ButtonWidthProperty = DependencyProperty.Register(
			"ButtonWidth",
			typeof(double),
			typeof(SgzSplitButton),
			new PropertyMetadata(30d)
		);

		// DependencyProperty as the backing store for IsExpanded
		public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(
			"IsExpanded",
			typeof(bool),
			typeof(SgzSplitButton),
			new PropertyMetadata(false)
		);

		// DependencyProperty as the backing store for AllowDisableExpand
		public static readonly DependencyProperty AllowDisableExpandProperty = DependencyProperty.Register(
			"AllowDisableExpand",
			typeof(bool),
			typeof(SgzSplitButton),
			new PropertyMetadata(true)
		);

		// DependencyProperty as the backing store for IsButtonMouseOver
		public static readonly DependencyProperty IsButtonMouseOverProperty = DependencyProperty.Register(
			"IsButtonMouseOver",
			typeof(bool),
			typeof(SgzSplitButton),
			new PropertyMetadata(false)
		);

		// DependencyProperty as the backing store for IsButtonPressed
		public static readonly DependencyProperty IsButtonPressedProperty = DependencyProperty.Register(
			"IsButtonPressed",
			typeof(bool),
			typeof(SgzSplitButton),
			new PropertyMetadata(false)
		);

		// DependencyProperty as the backing store for IsToggleMouseOver
		public static readonly DependencyProperty IsToggleMouseOverProperty = DependencyProperty.Register(
			"IsToggleMouseOver",
			typeof(bool),
			typeof(SgzSplitButton),
			new PropertyMetadata(false)
		);

		// DependencyProperty as the backing store for IsTogglePressed
		public static readonly DependencyProperty IsTogglePressedProperty = DependencyProperty.Register(
			"IsTogglePressed",
			typeof(bool),
			typeof(SgzSplitButton),
			new PropertyMetadata(false)
		);

		// DependencyProperty as the backing store for Command
		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
			"Command",
			typeof(ICommand),
			typeof(SgzSplitButton)
		);

		// DependencyProperty as the backing store for CommandParameter
		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
			"CommandParameter",
			typeof(Object),
			typeof(SgzSplitButton)
		);

		#endregion Dependency Properties


		#region Constructors


		static SgzSplitButton()
		{

		}
		public SgzSplitButton()
		{
			
		}


		#endregion Constructors


		#region Methods

		/// <summary>
		/// Force the container to only accept MenuItem
		/// </summary>
		protected override DependencyObject GetContainerForItemOverride() => new MenuItem();
		protected override bool IsItemItsOwnContainerOverride(object item) => item is MenuItem;

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (GetTemplateChild("PART_Button") is Border button)
			{
				PART_Button = button;
				PART_Button.MouseEnter += PART_Button_MouseEnter;
				PART_Button.MouseLeave += PART_Button_MouseLeave;
				PART_Button.PreviewMouseRightButtonUp += PART_Button_PreviewMouseRightButtonUp;
				PART_Button.MouseDown += PART_Button_MouseDown;
				PART_Button.MouseUp += PART_Button_MouseUp;
			}
			if (GetTemplateChild("PART_Toggle") is Border toggle)
			{
				PART_Toggle = toggle;
				PART_Toggle.MouseEnter += PART_Toggle_MouseEnter;
				PART_Toggle.MouseLeave += PART_Toggle_MouseLeave;
				PART_Toggle.MouseDown += PART_Toggle_MouseDown;
				PART_Toggle.MouseUp += PART_Toggle_MouseUp;
			}
			if (GetTemplateChild("PART_Popup") is Popup popup)
			{
				PART_Popup = popup;
				PART_Popup.Opened += PART_Popup_Opened;
				PART_Popup.Closed += PART_Popup_Closed;
				PART_Popup.PreviewMouseUp += PART_Popup_PreviewMouseUp;
			}
		}

		


		#region Toggle events

		private void PART_Toggle_MouseEnter(object sender, MouseEventArgs e)
		{
			IsToggleMouseOver = true;
			IsButtonMouseOver = IsButtonPressed = false;

			if (e.LeftButton == MouseButtonState.Pressed)
			{
				IsTogglePressed = true;
				if (HasItems)
					IsExpanded = !IsExpanded;
			}

			Mouse.AddPreviewMouseUpOutsideCapturedElementHandler(this, OutsideCapturedElementHandler);
			Mouse.AddPreviewMouseUpOutsideCapturedElementHandler(ContextMenu, OutsideCapturedElementHandler);
		}

		private void PART_Toggle_MouseLeave(object sender, MouseEventArgs e)
		{
			IsToggleMouseOver = false;
		}

		private void PART_Toggle_MouseDown(object sender, MouseButtonEventArgs e)
		{
			IsTogglePressed = true;
			ContextMenu.Loaded += ContextMenu_Loaded;
			if (e.RightButton == MouseButtonState.Pressed)
				ContextMenu.IsOpen = true;
			else if (HasItems)
				IsExpanded = !IsExpanded;
			else
				ContextMenu.IsOpen = true;

		}

		private void PART_Toggle_MouseUp(object sender, MouseButtonEventArgs e)
		{
			IsTogglePressed = false;
		}

		#endregion Toggle events


		#region Button events

		private void PART_Button_MouseEnter(object sender, MouseEventArgs e)
		{
			IsButtonMouseOver = true;
			IsToggleMouseOver = IsTogglePressed = false;

		}

		private void PART_Button_MouseLeave(object sender, MouseEventArgs e)
		{
			IsButtonMouseOver = false;
		}

		private void PART_Button_MouseDown(object sender, MouseButtonEventArgs e)
		{
			IsButtonPressed = true;
			if (IsExpanded)
				IsExpanded = false;
		}

		private void PART_Button_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			IsButtonPressed = false;
			e.Handled = true;
		}

		private void PART_Button_MouseUp(object sender, MouseButtonEventArgs e)
		{
			IsButtonPressed = false;
			if (Command != null && !IsExpanded)
				Command.Execute(CommandParameter);
		}

		#endregion Button events


		#region Popup events

		private void PART_Popup_Opened(object sender, EventArgs e)
		{
			Mouse.Capture(this, CaptureMode.SubTree);
			Mouse.AddPreviewMouseDownOutsideCapturedElementHandler(this, OutsideCapturedElementHandler);
		}

		private void OutsideCapturedElementHandler(object sender, MouseButtonEventArgs mouseButtonEventArgs)
		{
			IsExpanded = false;
			Mouse.RemovePreviewMouseDownOutsideCapturedElementHandler(this, OutsideCapturedElementHandler);
		}

		private void PART_Popup_Closed(object sender, EventArgs e)
		{
			IsButtonMouseOver = IsButtonPressed = IsToggleMouseOver = false;
			if (!ContextMenu.IsOpen)
				IsTogglePressed = false;

			ReleaseMouseCapture();
			if (PART_Toggle != null)
				PART_Toggle.Focus();
		}

		private void PART_Popup_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			IsExpanded = false;
		}

		#endregion Popup events



		#region ContextMenu

		private void ContextMenu_Loaded(object sender, RoutedEventArgs e)
		{
			ContextMenu.PlacementTarget = this;
			ContextMenu.Placement = PlacementMode.Bottom;
			ContextMenu.Closed += delegate { IsTogglePressed = false; };
		} 

		#endregion ContextMenu



		#endregion Methods

	}
}
