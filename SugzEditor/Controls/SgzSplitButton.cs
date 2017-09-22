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
using System.Windows.Input;
using System.Windows.Shapes;

namespace SugzEditor.Controls
{
    [TemplatePart(Name = "PART_Button", Type = typeof(Border))]
    [TemplatePart(Name = "PART_Toggle", Type = typeof(Border))]
    public class SgzSplitButton : Button
    {

        #region Fields

        // Template Parts
        private Border PART_Button;
        private Border PART_Toggle;

        #endregion Fields


        #region Properties


        //[Browsable(false)]
        //public ItemCollection Items
        //{
        //    get => (ItemCollection)GetValue(ItemsProperty);
        //    //set => SetValue(ItemsProperty, value);
        //}

        //// DependencyProperty as the backing store for Items
        //public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
        //    "Items",
        //    typeof(ItemCollection),
        //    typeof(ownerclass),
        //    new PropertyMetadata(0)
        //);




        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        [Browsable(false)]
        public bool ItemsSourceIsEmpty
        {
            get => (bool)GetValue(ItemsSourceIsEmptyProperty);
            set => SetValue(ItemsSourceIsEmptyProperty, value);
        }

        // DependencyProperty as the backing store for ItemsSourceIsEmpty
        public static readonly DependencyProperty ItemsSourceIsEmptyProperty = DependencyProperty.Register(
            "ItemsSourceIsEmpty",
            typeof(bool),
            typeof(SgzSplitButton),
            new PropertyMetadata(true)
        );




        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }


        public Style ItemContainerStyle
        {
            get => (Style)GetValue(ItemContainerStyleProperty);
            set => SetValue(ItemContainerStyleProperty, value);
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

        // DependencyProperty as the backing store for ItemsSource
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource",
            typeof(IEnumerable),
            typeof(SgzSplitButton),
            new PropertyMetadata(default(IEnumerable), ItemsSourceChanged)
        );

        
        private static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SgzSplitButton control = d as SgzSplitButton;
            control.ItemsSourceIsEmpty = ((ICollection)control.ItemsSource).Count == 0;
            //if (control.ItemsSource is ObservableCollection<object> itemsSource)
            //    control.ItemsSourceIsEmpty = itemsSource.Count == 0;

        }

        // DependencyProperty as the backing store for ItemTemplate
        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
            "ItemTemplate",
            typeof(DataTemplate),
            typeof(SgzSplitButton),
            new PropertyMetadata(default(DataTemplate))
        );

        // DependencyProperty as the backing store for ItemContainerStyle
        public static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register(
            "ItemContainerStyle",
            typeof(Style),
            typeof(SgzSplitButton),
            new PropertyMetadata(default(Style))
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


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_Button") is Border btn)
            {
                PART_Button = btn;
                PART_Button.MouseEnter += delegate { IsButtonMouseOver = true; };
                PART_Button.MouseLeave += delegate { IsButtonMouseOver = false; };
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
                PART_Toggle.MouseDown += (s, e) =>
                {
                    if (ItemsSource != null)
                        ContextMenu.IsOpen = true;
                    e.Handled = true;
                };
            }
        

            //ContextMenu = new ContextMenu
            //{
            //    ItemsSource = ItemsSource,
            //    ItemTemplate = ItemTemplate,
            //    ItemContainerStyle = ItemContainerStyle,
            //    PlacementTarget = this,
            //    Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom
            //};
            ContextMenu.ItemsSource = ItemsSource;
            ContextMenu.ItemTemplate = ItemTemplate;
            ContextMenu.ItemContainerStyle = ItemContainerStyle;
            ContextMenu.PlacementTarget = this;
            ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            ContextMenu.Opened += delegate { ContextMenuIsOpen = true; };
            ContextMenu.Closed += delegate { ContextMenuIsOpen = false; };
            //ContextMenu.Items.CurrentChanged += (s, e) => ItemsSourceIsEmpty = ContextMenu.Items.Count == 0;
            //ContextMenu.ItemsSource = ItemsSource;
        }



        #endregion Methods

    }
}