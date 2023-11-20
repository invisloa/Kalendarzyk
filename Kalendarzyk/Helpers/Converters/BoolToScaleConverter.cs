using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Kalendarzyk.Helpers.Converters
{
	public class BoolToScaleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool isScaledUp && isScaledUp)
			{
				return 1.0; // Normal scale
			}
			return 0.7; // Scaled down or any other value you prefer
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
