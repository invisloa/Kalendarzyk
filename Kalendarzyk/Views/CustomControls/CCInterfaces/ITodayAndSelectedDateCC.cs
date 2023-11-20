using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk.Views.CustomControls.CCInterfaces
{
	public interface ITodayAndSelectedDateCC
	{
		DateTime CurrentSelectedDate { get; set; }
		DateTime CurrentDate { get; }
		RelayCommand SelectTodayDateCommand { get; set; }
	}
}
