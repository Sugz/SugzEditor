using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SugzEditor.Controls
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(MenuItem))]
    [TemplatePart(Name = "PART_Button", Type = typeof(Border))]
    [TemplatePart(Name = "PART_Toggle", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    public class SgzSplitButtonNew : ItemsControl
    {

        #region Fields

        // Template Parts
        private Border PART_Button;
        private ToggleButton PART_Toggle;
        private Popup PART_Popup;

        #endregion Fields

        /// <summary>
        /// 
        /// </summary>
        [Description(""), Category("Common")]
        public object Content
        {
            get => (GetValue(ContentProperty));
            set => SetValue(ContentProperty, value);
        }

        // DependencyProperty as the backing store for Content
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content",
            typeof(object),
            typeof(SgzSplitButtonNew),
            new PropertyMetadata(null)
        );


        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public bool ButtonIsMouseOver
        {
            get => (bool)GetValue(ButtonIsMouseOverProperty);
            set => SetValue(ButtonIsMouseOverProperty, value);
        }

        // DependencyProperty as the backing store for IsMouseOverButton
        public static readonly DependencyProperty ButtonIsMouseOverProperty = DependencyProperty.Register(
            "ButtonIsMouseOver",
            typeof(bool),
            typeof(SgzSplitButtonNew),
            new PropertyMetadata(false)
        );


        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public bool ButtonIsMouseDown
        {
            get => (bool)GetValue(ButtonIsMouseDownProperty);
            set => SetValue(ButtonIsMouseDownProperty, value);
        }

        // DependencyProperty as the backing store for ButtonIsMouseDown
        public static readonly DependencyProperty ButtonIsMouseDownProperty = DependencyProperty.Register(
            "ButtonIsMouseDown",
            typeof(bool),
            typeof(SgzSplitButtonNew),
            new PropertyMetadata(false)
        );


        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public bool ToggleIsMouseDown
        {
            get => (bool)GetValue(ToggleIsMouseDownProperty);
            set => SetValue(ToggleIsMouseDownProperty, value);
        }

        // DependencyProperty as the backing store for ToggleIsMouseDown
        public static readonly DependencyProperty ToggleIsMouseDownProperty = DependencyProperty.Register(
            "ToggleIsMouseDown",
            typeof(bool),
            typeof(SgzSplitButtonNew),
            new PropertyMetadata(false)
        );






        /// <summary>
        /// 
        /// </summary>
        [Description(""), Category("Common")]
        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }

        // DependencyProperty as the backing store for IsOpen
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
            "IsOpen",
            typeof(bool),
            typeof(SgzSplitButtonNew),
            new PropertyMetadata(false)
        );




        public SgzSplitButtonNew()
        {
            
        }


        /// <summary>
        /// Force the container to only accept SgzTextEditorItem
        /// </summary>
        protected override DependencyObject GetContainerForItemOverride() => new MenuItem();
        protected override bool IsItemItsOwnContainerOverride(object item) => item is MenuItem;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_Button") is Border btn)
            {
                PART_Button = btn;
                PART_Button.MouseEnter += delegate { ButtonIsMouseOver = true; };
                PART_Button.MouseLeave += delegate { ButtonIsMouseOver = false; };
                PART_Button.MouseDown += delegate { ButtonIsMouseDown = true; };
                PART_Button.MouseUp += delegate { ButtonIsMouseDown = false; };
            }

            if (GetTemplateChild("PART_Toggle") is ToggleButton toggle)
            {
                PART_Toggle = toggle;
                if (PART_Toggle.IsChecked is true && PART_Popup is Popup)
                    PART_Popup.IsOpen = true;

                //PART_Button.MouseDown += delegate { ToggleIsMouseDown = true; };
            }
            if (GetTemplateChild("PART_Popup") is Popup popup)
            {
                PART_Popup = popup;
            }

        }

    }
}
