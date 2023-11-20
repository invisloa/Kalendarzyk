using Kalendarzyk.Helpers;
using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.ViewModels.EventOperations;
using Kalendarzyk.ViewModels.EventsViewModels;

namespace Kalendarzyk.Views;

public partial class ViewAllEventsPage : ContentPage
{
	public ViewAllEventsPage()
	{
		// Retrieve the view model from DI container

		 var viewModel = MauiProgram.Current.Services.GetService<AllEventsViewModel>();

		BindingContext = viewModel;
		InitializeComponent();
		viewModel.OnEventsToShowListUpdated += () =>
		{
		viewModel.BindDataToScheduleList();
		};
	}
	// for viewing specific type of events
	public ViewAllEventsPage(IEventRepository eventRepository, ISubEventTypeModel eventTypeModel)
	{
		BindingContext = new AllEventsViewModel(eventRepository, eventTypeModel);
		var viewModel = BindingContext as AllEventsViewModel;
		InitializeComponent();
		viewModel.OnEventsToShowListUpdated += () =>
		{
			viewModel.BindDataToScheduleList();
		};
	}
	protected override void OnDisappearing()
	{
		base.OnDisappearing();

		(BindingContext as AllEventsViewModel).OnEventsToShowListUpdated -= (BindingContext as AllEventsViewModel).BindDataToScheduleList;
	}

	protected override void OnAppearing()
	{
		var viewModel = BindingContext as AllEventsViewModel;
		base.OnAppearing();
		viewModel.OnAppearing();
	}
}