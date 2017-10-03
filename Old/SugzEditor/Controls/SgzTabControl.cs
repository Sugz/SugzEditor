using SugzEditor.Models;
using SugzEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(SgzTabItem))]
    [TemplatePart(Name = "PART_ScrollViewer", Type = typeof(ScrollViewer))]
    public class SgzTabControl : TabControl
    {

        #region Fields

        ScrollViewer _ScrollViewer;

        #endregion Fields


        #region Commands

        public static readonly DependencyProperty AddTabCommandProperty = DependencyProperty.Register(
            "AddTabCommand",
            typeof(ICommand),
            typeof(SgzTabControl));

        public ICommand AddTabCommand
        {
            get { return (ICommand)GetValue(AddTabCommandProperty); }
            set { SetValue(AddTabCommandProperty, value); }
        }



        public static readonly DependencyProperty CloseTabCommandProperty = DependencyProperty.Register(
            "CloseTabCommand",
            typeof(ICommand),
            typeof(SgzTabControl));

        public ICommand CloseTabCommand
        {
            get { return (ICommand)GetValue(CloseTabCommandProperty); }
            set { SetValue(CloseTabCommandProperty, value); }
        }


        #endregion Commands


        public SgzTabControl()
        {
            MouseDoubleClick += delegate { AddTab(); };
        }


        #region Overrides

        /// <summary>
        /// Get the ScrollViewer and handle the mouse wheel event
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_ScrollViewer") is ScrollViewer scv)
            {
                _ScrollViewer = scv;
                _ScrollViewer.PreviewMouseWheel += ScrollViewer_PreviewMouseWheel;
            }
        }


        /// <summary>
        /// Bring SelectedItem into view
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            if (ItemContainerGenerator.ContainerFromItem(SelectedItem) is SgzTabItem item)
                item.BringIntoView();
        }

        /// <summary>
        /// Force the container to only accept SgzTabItem
        /// </summary>
        protected override DependencyObject GetContainerForItemOverride() => new SgzTabItem();
        protected override bool IsItemItsOwnContainerOverride(object item) => item is SgzTabItem;
        

        #endregion Overrides



        #region Methods


        internal void AddTab()
        {
            if (AddTabCommand != null)
                AddTabCommand.Execute(null);
        }

        
        internal void RemoveTab(SgzTabItem tab)
        {
            if (CloseTabCommand != null)
                CloseTabCommand.Execute(tab.DataContext);
        }
        

        #endregion Methods



        #region Events Handlers


        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            _ScrollViewer.ScrollToHorizontalOffset(_ScrollViewer.HorizontalOffset - e.Delta);
            e.Handled = true;
        }


        #endregion Events Handlers
    }
}
