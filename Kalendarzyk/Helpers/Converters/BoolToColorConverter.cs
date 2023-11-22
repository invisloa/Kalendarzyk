using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk.Helpers.Converters
{
	public class BoolToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool isTrue && isTrue)
			{
					return (Color)Application.Current.Resources["DeselectedBackgroundColor"];
			}
			else
			{
				if (parameter?.ToString() == "Dangerous")
					return Colors.Red;
				else
					return (Color)Application.Current.Resources["MainMicroTaskBackgroundColor"];

			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

}
