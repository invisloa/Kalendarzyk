using Kalendarzyk.Helpers;
using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.ViewModels.EventsViewModels;

namespace Kalendarzyk.Views;

public partial class ViewDailyEvents : ContentPage
{
	public ViewDailyEvents()
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
	public ViewDailyEvents(IEventRepository eventRepository, ISubEventTypeModel eventType)
	{
		BindingContext = new DailyEventsViewModel(eventRepository, eventType);
		InitializeComponent();
	}
	protected override void OnAppearing()
	{
		base.OnAppearing();
		(BindingContext as DailyEventsViewModel).OnAppearing();
	}

}