using Kalendarzyk.ViewModels;
using Kalendarzyk.ViewModels.EventsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk.Views.CustomControls.CCViewModels
{
	public class DefaultTimespanCCViewModel : BaseViewModel
	{

		private int _selectedUnitIndex;
		public int SelectedUnitIndex
		{
			get => _selectedUnitIndex;
			set
			{
				_selectedUnitIndex = value;
				OnPropertyChanged(nameof(SelectedUnitIndex));
			}
		}

		private double _durationValue;
		public double DurationValue
		{
			get => _durationValue;
			set
			{
				_durationValue = value;
				OnPropertyChanged(nameof(DurationValue));
			}
		}

		public void SetControlsValues(TimeSpan timeToAdjust)
		{
			if (timeToAdjust.Days >= 1 && timeToAdjust.Hours == 0 && timeToAdjust.Minutes == 0 && timeToAdjust.Seconds == 0)
			{
				SelectedUnitIndex = 2;
				DurationValue = timeToAdjust.Days;
			}
			else if (timeToAdjust.TotalHours >= 1 && timeToAdjust.Minutes == 0 && timeToAdjust.Seconds == 0)
			{
				SelectedUnitIndex = 1;
				DurationValue = timeToAdjust.TotalHours;
			}
			else if (timeToAdjust.TotalMinutes >= 1 && timeToAdjust.Seconds == 0)
			{
				SelectedUnitIndex = 0;
				DurationValue = timeToAdjust.TotalMinutes;
			}
			else
			{
				SelectedUnitIndex = 3;
				DurationValue = timeToAdjust.TotalSeconds;
			}
		}

		public TimeSpan GetDuration()
		{
			switch (SelectedUnitIndex)
			{
				case 0: // Minutes
					return TimeSpan.FromMinutes(DurationValue);
				case 1: // Hours
					return TimeSpan.FromHours(DurationValue);
				case 2: // Days
					return TimeSpan.FromDays(DurationValue);
				case 3: // Seconds
					return TimeSpan.FromSeconds(DurationValue);
				default:
					return TimeSpan.Zero;
			}
		}
	}
}
