using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk.ViewModels.HelperClass.ExtensionsMethods
{
	public static class DateTimeExtensions
	{
		public static DateTime NextMonday(this DateTime date)
		{
			int daysUntilNextMonday = ((int)DayOfWeek.Monday - (int)date.DayOfWeek + 7) % 7;
			if (daysUntilNextMonday == 0) daysUntilNextMonday = 7; // if it's already Monday, add 7 days
			return date.AddDays(daysUntilNextMonday);
		}
	}
}
