﻿using Kalendarzyk.Services.DataOperations;

namespace Kalendarzyk.ViewModels.EventsViewModels
{
	public class MonthlyEventsViewModel : AbstractEventViewModel
	{

		public MonthlyEventsViewModel
						(IEventRepository eventRepository)
						: base(eventRepository) { }
		public override void BindDataToScheduleList()
		{
			// Start of the month
			var startOfMonth = new DateTime(CurrentSelectedDate.Year, CurrentSelectedDate.Month, 1);

			// End of the month
			var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

			ApplyEventsDatesFilter(startOfMonth, endOfMonth);

			OnOnEventsToShowListUpdated(); // invoke event to update customControl

		}
	}
}
