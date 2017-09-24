using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Shapes;

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
        /// 
        /// </summary>
        [Description(""), Category("Common")]
        public object Content
        {
            get => (GetValue(ContentProperty));
            set => SetValue(ContentProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        [Description(""), Category("Appearance")]
        public bool IsExpanded
        {
            get => (bool)GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }

        [Browsable(false)]
        public bool IsPressed
        {
            get => (bool)GetValue(IsPressedProperty);
            set => SetValue(IsPressedProperty, value);
        }

        /// <summary>
        /// Get or sets the Command property. 
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Reflects the parameter to pass to the CommandProperty upon execution. 
        /// </summary>
        public Object CommandParameter
        {
            get { return (Object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }


        [Browsable(false)]
        public bool IsButtonMouseOver
        {
            get => (bool)GetValue(IsButtonMouseOverProperty);
            set => SetValue(IsButtonMouseOverProperty, value);
        }

        [Browsable(false)]
        public bool ContextMenuIsOpen
        {
            get => (bool)GetValue(ContextMenuIsOpenProperty);
            set => SetValue(ContextMenuIsOpenProperty, value);
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

        // DependencyProperty as the backing store for IsExpanded
        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(
            "IsExpanded",
            typeof(bool),
            typeof(SgzSplitButton),
            new PropertyMetadata(false)
        );

        // DependencyProperty as the backing store for IsPressed
        public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register(
            "IsPressed",
            typeof(bool),
            typeof(SgzSplitButton),
            new PropertyMetadata(false)
        );

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(SgzSplitButton)
        );
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter",
            typeof(Object),
            typeof(SgzSplitButton)
        );

        // DependencyProperty as the backing store for IsButtonMouseOver
        public static readonly DependencyProperty IsButtonMouseOverProperty = DependencyProperty.Register(
            "IsButtonMouseOver",
            typeof(bool),
            typeof(SgzSplitButton),
            new PropertyMetadata(false)
        );

        // DependencyProperty as the backing store for ContextMenuIsOpen
        public static readonly DependencyProperty ContextMenuIsOpenProperty = DependencyProperty.Register(
            "ContextMenuIsOpen",
            typeof(bool),
            typeof(SgzSplitButton),
            new PropertyMetadata(false)
        );

        #endregion Dependency Properties


        #region Constructors


        static SgzSplitButton()
        {
            
        }
        public SgzSplitButton()
        {
            ContextMenu = new ContextMenu();
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
            if (GetTemplateChild("PART_Button") is Border btn)
            {
                PART_Button = btn;
                PART_Button.MouseEnter += delegate { if (!IsPressed) IsButtonMouseOver = true; };
                PART_Button.MouseLeave += delegate { IsButtonMouseOver = false; };
                PART_Button.MouseUp += PART_Button_MouseUp;
                PART_Button.PreviewMouseRightButtonUp += (s, e) =>
                {
                    if (PART_Toggle != null)
                    {
                        MouseButtonEventArgs mouseButtonEventArgs = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left);
                        mouseButtonEventArgs.RoutedEvent = Mouse.MouseDownEvent;
                        mouseButtonEventArgs.Source = PART_Toggle;
                        PART_Toggle.RaiseEvent(mouseButtonEventArgs);
                    }
                    e.Handled = true;
                };
            }
            if (GetTemplateChild("PART_Toggle") is Border toggle)
            {
                PART_Toggle = toggle;
                PART_Toggle.MouseDown += PART_Toggle_MouseDown; ;
                PART_Toggle.MouseUp += delegate { IsPressed = false; };
            }
            if (GetTemplateChild("PART_Popup") is Popup popup)
            {
                PART_Popup = popup;
                PART_Popup.Opened += PART_Popup_Opened;
                PART_Popup.Closed += PART_Popup_Closed;
                PART_Popup.PreviewMouseUp += delegate { IsExpanded = false; };
            }
        }

        private void PART_Button_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Command != null && !IsPressed)
                Command.Execute(CommandParameter);

        }

        private void PART_Toggle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (HasItems)
                IsExpanded = !IsExpanded;
            IsPressed = true;
        }

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
            ReleaseMouseCapture();
            if (PART_Toggle != null)
                PART_Toggle.Focus();
        }



        #endregion Methods

    }
}