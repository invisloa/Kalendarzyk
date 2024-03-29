using Kalendarzyk.Models.EventModels;
using Kalendarzyk.ViewModels.EventOperations;

namespace Kalendarzyk.Views
{
	public partial class EventPage : ContentPage
	{
		// For adding events
		public EventPage(DateTime selcetedDate)
		{
			InitializeComponent();

			BindingContext = new EventOperationsViewModel(selcetedDate);
		}
		public EventPage()
		{
			var today = DateTime.Today;
			InitializeComponent();
			BindingContext = new EventOperationsViewModel(today);

		}
		// For editing events
		public EventPage(IGeneralEventModel eventModel)
		{
			InitializeComponent();
			try
			{
				BindingContext = new EventOperationsViewModel(eventToEdit: eventModel);
			}
			catch (Exception ex)
			{
				DisplayAlert("Error", $"{ex}", "yyy");
			}
		}
	}
}
