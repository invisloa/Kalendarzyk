using Kalendarzyk.Helpers;
using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.ViewModels.EventsViewModels;

namespace Kalendarzyk.Views;

public partial class DailyEventsPage : ContentPage
{
	public DailyEventsPage()
	{
		var viewModel = ServiceHelper.GetService<DailyEventsViewModel>();
		BindingContext = viewModel;
		InitializeComponent();
		viewModel.OnEventsToShowListUpdated += () =>
		{
			viewModel.BindDataToScheduleList();
		};
	}
	protected override void OnDisappearing()
	{
		base.OnDisappearing();
		(BindingContext as DailyEventsViewModel).OnEventsToShowListUpdated -= (BindingContext as DailyEventsViewModel).BindDataToScheduleList;
	}

	public DailyEventsPage(ISubEventTypeModel eventType)
	{
		BindingContext = new DailyEventsViewModel(eventType);
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		(BindingContext as DailyEventsViewModel).AllEventsListOC = (BindingContext as DailyEventsViewModel).EventRepository.AllEventsList;    //TEMP FIX TODO  ALL events list didnt update after adding new event (but only for a second time and +)
		(BindingContext as DailyEventsViewModel).OnAppearing();
	}

}