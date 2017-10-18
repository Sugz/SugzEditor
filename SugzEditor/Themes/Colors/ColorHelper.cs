using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using SD = System.Drawing;

namespace SugzEditor.Themes.Colors
{
    public static class ColorHelper
    {
		public static ResourceDictionary _Colors = Application.Current.Resources.MergedDictionaries[0];

		public static void SetBrightness(double value)
		{
			foreach (DictionaryEntry entry in GetOriginalColors())
			{
				SD.Color sdColor = FromColor((entry.Value as SolidColorBrush).Color);
				sdColor = ChangeLightness(sdColor, value);
				_Colors[entry.Key] = new SolidColorBrush(FromSDColor(sdColor));
			}
		}


		public static void SetContrast(double value)
		{
			foreach (DictionaryEntry entry in GetOriginalColors())
			{
				SD.Color sdColor = FromColor((entry.Value as SolidColorBrush).Color);
				float test = ((sdColor.R / 255f) + (sdColor.G / 255f) + (sdColor.B / 255f)) / 3f;
				if (test < 0.5)
					sdColor = ChangeLightness(sdColor, (1f - value) + 1);
				else
					sdColor = ChangeLightness(sdColor, value);
				_Colors[entry.Key] = new SolidColorBrush(FromSDColor(sdColor));
			}
		}

		public static void SetBrightnessAndContrast(double brightness, double contrast)
		{
			int medianeColorValue = 0;
			foreach (DictionaryEntry entry in GetOriginalColors())
			{
				SD.Color sdColor = FromColor((entry.Value as SolidColorBrush).Color);
				sdColor = ChangeLightness(sdColor, brightness);
				medianeColorValue += sdColor.R + sdColor.G + sdColor.B;
				_Colors[entry.Key] = new SolidColorBrush(FromSDColor(sdColor));
			}

			medianeColorValue /= _Colors.Count;
			foreach (DictionaryEntry entry in _Colors)
			{
				SD.Color sdColor = FromColor((entry.Value as SolidColorBrush).Color);
				if (sdColor.R + sdColor.G + sdColor.B < medianeColorValue)
					sdColor = ChangeLightness(sdColor, (1f - contrast) + 1);
				else
					sdColor = ChangeLightness(sdColor, contrast);
				_Colors[entry.Key] = new SolidColorBrush(FromSDColor(sdColor));
			}
		}


		private static ResourceDictionary GetOriginalColors()
		{
			string source = _Colors.Source.ToString().Replace("/Colors", "/Colors/Originals");
			ResourceDictionary resource = new ResourceDictionary();
			resource.Source = new Uri(source, UriKind.RelativeOrAbsolute);
			return resource;
		}

		private static SD.Color ChangeLightness(SD.Color color, double coef)
		{
			int r = (int)(color.R * coef);
			int g = (int)(color.G * coef);
			int b = (int)(color.B * coef);

			r = r < 0 ? r = 0 : (r > 255 ? 255 : r);
			g = g < 0 ? g = 0 : (g > 255 ? 255 : g);
			b = b < 0 ? b = 0 : (b > 255 ? 255 : b);

			return SD.Color.FromArgb(r, g, b);
		}

		private static Color FromSDColor(SD.Color color)
		{
			return Color.FromArgb(color.A, color.R, color.G, color.B);
		}

		private static SD.Color FromColor(Color color)
		{
			return SD.Color.FromArgb(color.A, color.R, color.G, color.B);
		}

	}
}
