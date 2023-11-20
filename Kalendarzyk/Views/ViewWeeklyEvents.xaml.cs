using Kalendarzyk.Helpers;
using Kalendarzyk.ViewModels.EventsViewModels;


namespace Kalendarzyk.Views
{
	public partial class ViewWeeklyEvents : ContentPage
	{
		WeeklyEventsViewModel viewModel = ServiceHelper.GetService<WeeklyEventsViewModel>();

		public ViewWeeklyEvents()
		{
			InitializeComponent();
			BindingContext = viewModel;
			viewModel.OnEventsToShowListUpdated += () =>
			{
				weeklyEventsControl.GenerateGrid();
			};
		}
		public ViewWeeklyEvents(DateTime goToDate) : this()
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
			(BindingContext as WeeklyEventsViewModel).BindDataToScheduleList();
		}
	}

}
