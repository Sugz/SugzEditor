﻿using SugzTools.Extensions;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SugzEditor.Themes.Converters
{
	/// <summary>
	/// Code by bendewey : https://stackoverflow.com/a/672123/3971575
	/// </summary>
	public class LeftMarginMultiplierConverter : IValueConverter
	{
		public double Length { get; set; }
		public double InitialLength { get; set; }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var item = value as TreeViewItem;
			if (item == null)
				return new Thickness(0);

			return new Thickness(Length * item.GetDepth() + InitialLength, 0, 0, 0);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new System.NotImplementedException();
		}
	}
}