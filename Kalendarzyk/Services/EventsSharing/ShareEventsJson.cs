using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Services.DataOperations;
using Newtonsoft.Json;

namespace Kalendarzyk.Services.EventsSharing
{
	public class ShareEventsJson : IShareEvents
	{

		public ShareEventsJson(IEventRepository eventRepository)
		{
			_eventRepository = eventRepository;
		}
		private readonly IEventRepository _eventRepository;
		public async Task AddEventAsync(IGeneralEventModel eventModel)
		{
			await _eventRepository.AddEventAsync(eventModel);
		}
		public string SerializeEventToJson(IGeneralEventModel eventModel)
		{
			var jsonString = JsonConvert.SerializeObject(eventModel);
			return jsonString;
		}

		public async Task ShareEventAsync(IGeneralEventModel eventModel)
		{
			// You might want to make sure your eventModel is saved before you share the link.
			var link = $"myapp://event?id={eventModel.Id}";

			await Share.RequestAsync(new ShareTextRequest
			{
				Text = link,
				Title = $"Share {eventModel.Title}"
			});
		}
		public async Task ImportEventAsync(string jsonString)
		{
			var eventModel = JsonConvert.DeserializeObject<IGeneralEventModel>(jsonString);
			var eventExists = await _eventRepository.GetEventByIdAsync(eventModel.Id) != null;

			if (!eventExists)
			{
				await AddEventAsync(eventModel);
			}
			else
			{
				// TODO: Add a message or handle the case when the event already exists
			}
		}


	}
}