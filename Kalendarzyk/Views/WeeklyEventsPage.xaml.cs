using Kalendarzyk.Helpers;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.ViewModels.EventsViewModels;


namespace Kalendarzyk.Views
{
	public partial class WeeklyEventsPage : ContentPage
	{
		WeeklyEventsViewModel viewModel = ServiceHelper.GetService<WeeklyEventsViewModel>();

		public WeeklyEventsPage()
		{
			InitializeComponent();
			BindingContext = viewModel;
			viewModel.OnEventsToShowListUpdated += () =>
			{
				weeklyEventsControl.GenerateGrid();
			};
		}
		public WeeklyEventsPage(DateTime goToDate) : this()
		{
			viewModel.CurrentSelectedDate = goToDate;
		}
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			(BindingContext as WeeklyEventsViewModel).OnEventsToShowListUpdated -= weeklyEventsControl.GenerateGrid;
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			(BindingContext as WeeklyEventsViewModel).AllEventsListOC = (BindingContext as WeeklyEventsViewModel).EventRepository.AllEventsList;	//TEMP FIX TODO  ALL events list didnt update after adding new event (but only for a second time and +)
			(BindingContext as WeeklyEventsViewModel).BindDataToScheduleList();
		}
	}

}
