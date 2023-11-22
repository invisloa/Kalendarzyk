using Kalendarzyk.Helpers;
using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.ViewModels.EventOperations;
using Kalendarzyk.ViewModels.EventsViewModels;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Kalendarzyk.Views
{
	public partial class EventPage : ContentPage
	{
		private readonly IEventRepository _eventRepository;
		// For adding events
		public EventPage(IEventRepository eventRepository, DateTime selcetedDate)
		{
			InitializeComponent();

			BindingContext = new EventOperationsViewModel(eventRepository, selcetedDate);
		}
		public EventPage()
		{
			var today = DateTime.Today;
			InitializeComponent();
			_eventRepository = ServiceHelper.GetService<IEventRepository>();
			BindingContext = new EventOperationsViewModel(_eventRepository, today);

		}
		// For editing events
		public EventPage(IEventRepository eventRepository, IGeneralEventModel eventModel)
		{
			InitializeComponent();

			BindingContext = new EventOperationsViewModel(eventRepository, eventToEdit: eventModel);

		}

	}
}
