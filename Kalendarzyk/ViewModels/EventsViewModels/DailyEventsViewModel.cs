using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.Views.CustomControls.CCInterfaces;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Kalendarzyk.ViewModels.EventsViewModels
{
	public class DailyEventsViewModel : AbstractEventViewModel
	{
		public string AboveEventsListText
		{
			get
			{
				// switch selectedLanguage()
				{
					return "EVENTS LIST";
				}
			}
		}
        // ctor 
        public DailyEventsViewModel()
        {
        }
        public DailyEventsViewModel(ISubEventTypeModel eventType) : base()	// IDK ???
		{
		}
		protected override void ApplyEventsDatesFilter(DateTime startDate, DateTime endDate)
		{

			var selectedToFilterEventTypes = AllSubEventTypesOC
				.Where(x => x.IsSelectedToFilter)
				.Select(x => x.EventTypeName)
				.ToHashSet();

			List<IGeneralEventModel> filteredEvents = AllEventsListOC
				.Where(x => selectedToFilterEventTypes.Contains(x.EventType.ToString()) &&
							x.StartDateTime.Date == startDate.Date &&
							x.EndDateTime.Date <= endDate.Date)
				.ToList();

			// Clear existing items in the EventsToShowList
			EventsToShowList.Clear();

			// Add filtered items to the EventsToShowList
			foreach (var eventItem in filteredEvents)
			{
				EventsToShowList.Add(eventItem);
			}
		}
		public override void BindDataToScheduleList()
		{
			ApplyEventsDatesFilter(CurrentSelectedDate.Date, DateTime.MaxValue);
		}

		public void OnAppearing()
		{

			BindDataToScheduleList();
			EventsToShowList = new ObservableCollection<IGeneralEventModel>(EventsToShowList);
		}




	}
}
