using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace SugzEditor.Controls
{
    public class SgzTabItem : TabItem
    {
        private SgzTabControl ParentTabControl => ItemsControl.ItemsControlFromItemContainer(this) as SgzTabControl;


        public static RoutedUICommand CloseTabCommand => _CloseTabCommand;
        private static readonly RoutedUICommand _CloseTabCommand = new RoutedUICommand(
            "Close tab",
            "CloseTab",
            typeof(SgzTabItem)
        );

        static SgzTabItem()
        {
            CommandManager.RegisterClassCommandBinding(typeof(SgzTabItem), new CommandBinding(_CloseTabCommand, CloseTab));
        }


        private static void CloseTab(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is SgzTabItem item)
                item.ParentTabControl.RemoveTab(item);
        }
    }
}
