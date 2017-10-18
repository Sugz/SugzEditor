using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SugzEditor.Controls
{
    public static class TreeViewItemHelper
    {
		
		public static object GetView(DependencyObject obj)
		{
			return (object)obj.GetValue(ViewProperty);
		}

		public static void SetView(DependencyObject obj, object value)
		{
			obj.SetValue(ViewProperty, value);
		}

		// Using a DependencyProperty as the backing store for View.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ViewProperty = DependencyProperty.RegisterAttached(
			"View", 
			typeof(object), 
			typeof(TreeViewHelper), 
			new PropertyMetadata(null)
		);


	}
}
