using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SugzEditor.Controls
{
    public static class ButtonHelper
    {

		public static PathGeometry GetData(DependencyObject obj)
		{
			return (PathGeometry)obj.GetValue(DataProperty);
		}

		public static void SetData(DependencyObject obj, PathGeometry value)
		{
			obj.SetValue(DataProperty, value);
		}

		// Using a DependencyProperty as the backing store for Datas.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty DataProperty = DependencyProperty.RegisterAttached(
			"Data", 
			typeof(PathGeometry), 
			typeof(ButtonHelper), 
			new PropertyMetadata(null)
		);

	}
}
