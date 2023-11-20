using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk.Views.CustomControls.CCInterfaces.UserTypeExtraOptions
{
	public interface IDefaultTimespanCC
	{
		// Properties
		int SelectedUnitIndex { get; set; }
		double DurationValue { get; set; }
		void SetControlsValues(TimeSpan timeToAdjust);
		TimeSpan GetDefaultDuration();
	}
}
