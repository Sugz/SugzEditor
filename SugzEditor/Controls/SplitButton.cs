using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace SugzEditor.Controls
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(MenuItem))]
    [TemplatePart(Name = "PART_Container", Type = typeof(Grid)),
    TemplatePart(Name = "PART_Button", Type = typeof(Button)),
    TemplatePart(Name = "PART_Popup", Type = typeof(Popup)),
    TemplatePart(Name = "PART_Expander", Type = typeof(Button))]
    public class SplitButton : ItemsControl
    {

        public static readonly RoutedEvent ClickEvent =
            EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(SplitButton));

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }


        public object Content
        {
            get => (object)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        // DependencyProperty as the backing store for Content
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content",
            typeof(object),
            typeof(SplitButton),
            new PropertyMetadata(null)
        );



        /// <summary>
        /// 
        /// </summary>
        [Browsable(false), Bindable(true)]
        public bool ToggleIsMouseOver
        {
            get => (bool)GetValue(ToggleIsMouseOverProperty);
            set => SetValue(ToggleIsMouseOverProperty, value);
        }

        // DependencyProperty as the backing store for ToggleIsMouseOver
        public static readonly DependencyProperty ToggleIsMouseOverProperty = DependencyProperty.Register(
            "ToggleIsMouseOver",
            typeof(bool),
            typeof(SplitButton),
            new PropertyMetadata(false)
        );


        /// <summary>
        /// 
        /// </summary>
        [Description(""), Category("")]
        // [Browsable(false)]
        public bool ToggleIsPressed
        {
            get => (bool)GetValue(ToggleIsPressedProperty);
            set => SetValue(ToggleIsPressedProperty, value);
        }

        // DependencyProperty as the backing store for ToggleIsPressed
        public static readonly DependencyProperty ToggleIsPressedProperty = DependencyProperty.Register(
            "ToggleIsPressed",
            typeof(bool),
            typeof(SplitButton),
            new PropertyMetadata(false)
        );




        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(SplitButton));

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(Object), typeof(SplitButton));
        public static readonly DependencyProperty IconTemplateProperty = DependencyProperty.Register("IconTemplate", typeof(DataTemplate), typeof(SplitButton));

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(SplitButton));
        public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(SplitButton));
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(Object), typeof(SplitButton));

        public static readonly DependencyProperty ButtonStyleProperty = DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(SplitButton), new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty ButtonArrowStyleProperty = DependencyProperty.Register("ButtonArrowStyle", typeof(Style), typeof(SplitButton), new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty ArrowBrushProperty = DependencyProperty.Register("ArrowBrush", typeof(Brush), typeof(SplitButton), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// Reflects the parameter to pass to the CommandProperty upon execution. 
        /// </summary>
        public Object CommandParameter
        {
            get { return (Object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the target element on which to fire the command.
        /// </summary>
        public IInputElement CommandTarget
        {
            get { return (IInputElement)GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
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
        /// Indicates whether the Popup is visible. 
        /// </summary>
        public Boolean IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }


        /// <summary>
        ///  Gets or sets the Content used to generate the icon part.
        /// </summary>
        [Bindable(true)]
        public Object Icon
        {
            get { return GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        /// <summary> 
        /// Gets or sets the ContentTemplate used to display the content of the icon part. 
        /// </summary>
        [Bindable(true)]
        public DataTemplate IconTemplate
        {
            get { return (DataTemplate)GetValue(IconTemplateProperty); }
            set { SetValue(IconTemplateProperty, value); }
        }

        /// <summary>
        /// Gets/sets the button style.
        /// </summary>
        public Style ButtonStyle
        {
            get { return (Style)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }

        /// <summary>
        /// Gets/sets the button arrow style.
        /// </summary>
        public Style ButtonArrowStyle
        {
            get { return (Style)GetValue(ButtonArrowStyleProperty); }
            set { SetValue(ButtonArrowStyleProperty, value); }
        }

        /// <summary>
        /// Gets/sets the brush of the button arrow icon.
        /// </summary>
        public Brush ArrowBrush
        {
            get { return (Brush)GetValue(ArrowBrushProperty); }
            set { SetValue(ArrowBrushProperty, value); }
        }

        private Button _clickButton;
        private Button _expander;
        private Popup _popup;

        static SplitButton()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitButton), new FrameworkPropertyMetadata(typeof(SplitButton)));
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            e.RoutedEvent = ClickEvent;
            RaiseEvent(e);
        }


        private void ExpanderClick(object sender, RoutedEventArgs e)
        {
            IsExpanded = !IsExpanded;
        }

        private void _expander_MouseEnter(object sender, MouseEventArgs e)
        {
            ToggleIsMouseOver = _expander.IsMouseOver;
        }

        private void _expander_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ToggleIsPressed = _expander.IsPressed;
        }


        /// <summary>
        /// Force the container to only accept MenuItem
        /// </summary>
        protected override DependencyObject GetContainerForItemOverride() => new MenuItem();
        protected override bool IsItemItsOwnContainerOverride(object item) => item is MenuItem;


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _clickButton = EnforceInstance<Button>("PART_Button");
            _expander = EnforceInstance<Button>("PART_Expander");
            _popup = EnforceInstance<Popup>("PART_Popup");
            InitializeVisualElementsContainer();
        }

        //Get element from name. If it exist then element instance return, if not, new will be created
        T EnforceInstance<T>(string partName) where T : FrameworkElement, new()
        {
            T element = GetTemplateChild(partName) as T ?? new T();
            return element;
        }

        private void InitializeVisualElementsContainer()
        {
            _expander.Click -= ExpanderClick;
            _expander.MouseEnter -= _expander_MouseEnter;
            _expander.MouseLeave -= _expander_MouseEnter;
            _clickButton.Click -= ButtonClick;
            _popup.Opened -= PopupOpened;
            _popup.Closed -= PopupClosed;

            _expander.Click += ExpanderClick;
            _expander.MouseEnter += _expander_MouseEnter;
            _expander.MouseLeave += _expander_MouseEnter;
            _expander.MouseDown += _expander_MouseDown;
            _clickButton.Click += ButtonClick;
            _popup.Opened += PopupOpened;
            _popup.Closed += PopupClosed;
        }

        

        void PopupClosed(object sender, EventArgs e)
        {
            this.ReleaseMouseCapture();
            if (this._expander != null)
            {
                this._expander.Focus();
            }
        }

        void PopupOpened(object sender, EventArgs e)
        {
            Mouse.Capture(this, CaptureMode.SubTree);
            Mouse.AddPreviewMouseDownOutsideCapturedElementHandler(this, OutsideCapturedElementHandler);
        }

        private void OutsideCapturedElementHandler(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            IsExpanded = false;
            Mouse.RemovePreviewMouseDownOutsideCapturedElementHandler(this, OutsideCapturedElementHandler);
        }
    }
}
