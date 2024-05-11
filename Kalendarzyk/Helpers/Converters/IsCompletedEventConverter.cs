using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk.Helpers.Converters
{
	internal class IsCompletedEventConverter : IValueConverter
	{
		private const int _alphaColorDivisor = 20;
		private Color IsCompleteColorAdapt(Color color)
		{
			return Color.FromRgba(color.Red, color.Green, color.Blue, color.Alpha / _alphaColorDivisor);
		}
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			try
			{

				if (parameter != null && value != null)
				{
					if (parameter is bool isTrue && isTrue)
					{

						if (value is Color color)
							return IsCompleteColorAdapt(value as Color);
						else
						{

							return Colors.Black;
						}
					}
					return null;
				}
				else
				{
					return (Color)Application.Current.Resources["MainMicroTaskBackgroundColor"];
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
}
