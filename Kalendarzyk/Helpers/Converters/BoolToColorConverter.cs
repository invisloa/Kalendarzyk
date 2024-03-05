using System.Globalization;

namespace Kalendarzyk.Helpers.Converters
{
	public class BoolToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			try
			{
				if (value is bool isTrue && isTrue)
				{
					if (parameter != null)
					{

						if (parameter.ToString() == "Dangerous")
							return Colors.Red;
						else
						{
							if (parameter is string resourceKey)
							{
								// Try to resolve the DynamicResource value
								if (Application.Current.Resources.TryGetValue(resourceKey, out var resolvedValue) && resolvedValue is Color color)
								{
									return color;
								}
							}
							return Colors.Black;
						}

					}
					else
					{
						return (Color)Application.Current.Resources["MainMicroTaskBackgroundColor"];
					}
				}
				else
				{
					return (Color)Application.Current.Resources["DeselectedBackgroundColor"];
				}
			}
			catch (Exception ex)
			{
				return (Color)Application.Current.Resources["DeselectedBackgroundColor"];
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
	public class IsCompleteToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool isTrue && isTrue)
			{
				return (Color)Application.Current.Resources["DeselectedBackgroundColor"];

			}
			else
			{
				return (Color)Application.Current.Resources["MainMicroTaskBackgroundColor"];

			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public static class ColorHelper
	{
		public static Color? GetColorFromHex(string hexColor)
		{
			// Check if the input is a valid hex string
			if (!string.IsNullOrWhiteSpace(hexColor) &&
				hexColor[0] == '#' &&
				(hexColor.Length == 7 || hexColor.Length == 4)) // #RGB or #RRGGBB format
			{
				try
				{
					return Color.FromHex(hexColor);
				}
				catch
				{
					// If the color conversion fails, return null
					return null;
				}
			}
			else
			{
				// If the input is not a valid hex string, return null
				return null;
			}
		}
	}

}
