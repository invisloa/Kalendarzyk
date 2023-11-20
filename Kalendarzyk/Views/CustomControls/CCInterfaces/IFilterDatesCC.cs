using Kalendarzyk.Models.EventModels;
using System.Collections.ObjectModel;

namespace Kalendarzyk.Views.CustomControls.CCInterfaces
{
	public interface IFilterDatesCC
	{
		DateTime FilterDateFrom { get; set; }
		DateTime FilterDateTo { get; set; }
		string TextFilterDateFrom { get; set; }
		string TextFilterDateTo { get; set; }
		ObservableCollection<IGeneralEventModel> AllEventsListOC { get; }

	}

	// EXTENSION METHODS FOR CUSTOM CONTROL INTERFACE IFilterDatesCC
	public static class FilterableByDateExtensions
	{
		public static void SetFilterDatesValues(this IFilterDatesCC filterable)
		{
			if (filterable.AllEventsListOC.Any())
			{
				filterable.FilterDateFrom = filterable.AllEventsListOC
					.OrderBy(e => e.StartDateTime)
					.FirstOrDefault()
					.StartDateTime;
			}
			else
			{
				filterable.FilterDateFrom = DateTime.Today;
			}
			filterable.FilterDateTo = DateTime.Today;
		}
		/// <summary>
		/// Sets the filter dates for the provided filterable object.
		/// </summary>
		/// <param name="filterable">The filterable object to set the dates for.</param>
		/// <param name="isTillToday">Indicates whether the filter date should be set to today's date.</param>
		public static void SetFilterDatesValues(this IFilterDatesCC filterable, bool isTillToday)
		{
			// If no events are available, set both filters to today's date and return.
			if (!filterable.AllEventsListOC.Any())
			{
				filterable.FilterDateFrom = DateTime.Today;
				filterable.FilterDateTo = DateTime.Today;
				return;
			}

			var orderedList = filterable.AllEventsListOC.OrderBy(e => e.StartDateTime).ToList();
			filterable.FilterDateFrom = orderedList.First().StartDateTime;

			filterable.FilterDateTo = isTillToday ? DateTime.Today : orderedList.Last().EndDateTime;
		}

	}
}