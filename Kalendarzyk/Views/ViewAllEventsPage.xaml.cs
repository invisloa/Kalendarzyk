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

	private async void MyExpander_ExpandedChanged(object sender, CommunityToolkit.Maui.Core.ExpandedChangedEventArgs e)
	{

		// Increment the current rotation by 180 degrees
		double newRotation = ArrowLabel.Rotation - 180;
		await ArrowLabel.RotateTo(newRotation, 350, Easing.CubicInOut);
	}
}