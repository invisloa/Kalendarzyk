using Kalendarzyk.Helpers;
using Kalendarzyk.ViewModels.EventsViewModels;

namespace Kalendarzyk.Views
{
	public partial class MonthlyEventsPage : ContentPage
	{
		private MonthlyEventsViewModel viewModel;

		public MonthlyEventsPage()
		{
			InitializeComponent();
			viewModel = ServiceHelper.GetService<MonthlyEventsViewModel>();
			BindingContext = viewModel;

			// Generate new grid every time the list of events to show is updated (e.g. when user adds new event)
			viewModel.OnEventsToShowListUpdated += () =>
			{
				monthlyEventsControl.GenerateGrid();
			};
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			if (viewModel != null)
			{
				viewModel.OnEventsToShowListUpdated -= monthlyEventsControl.GenerateGrid;
			}
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			if (viewModel != null)
			{
				viewModel.AllEventsListOC = viewModel.EventRepository.AllEventsList; // TEMP FIX TODO: ALL events list didn't update after adding new event (but only for a second time and +)
				viewModel.BindDataToScheduleList();
			}
		}
	}
}
