using Kalendarzyk.Helpers;
using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.ViewModels.EventOperations;
using Kalendarzyk.ViewModels.EventsViewModels;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace Kalendarzyk.Views
{
	[QueryProperty(nameof(MyData), "data")]
	public partial class EventPage : ContentPage
	{
		private string myData;
		public string MyData
		{
			get => myData;
			set
			{
				myData = Uri.UnescapeDataString(value ?? string.Empty);
				OnPropertyChanged();
				var settings = new JsonSerializerSettings{TypeNameHandling = TypeNameHandling.All};
				try
				{
					var eventModel = JsonConvert.DeserializeObject<IGeneralEventModel>(myData, settings);
					BindingContext = new EventOperationsViewModel(eventModel);

				}
				catch (Exception ex)
				{
					DisplayAlert("Error", $"{ex}", "XXX");
				}

				// Use myData
			}
		}


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
				DisplayAlert("Error", $"{ex}", "XXX");
			}
		}
	}
}
