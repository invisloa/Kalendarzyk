using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Services.DataOperations;
using Newtonsoft.Json;

namespace Kalendarzyk.Services.EventsSharing
{
	public class ShareEventsJson : IShareEvents
	{
		ILocalDataEncryptionService _aesService = Factory.CreateNewLocalDataEncryptionService();

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
			// Serialize the event to a JSON string
			var eventJsonString = SerializeEventToJson(eventModel);
			var encryptedEventJsonString = _aesService.EncryptString(eventJsonString);

			// ADD ENCRYPTION???????

			// Share the JSON string using Xamarin.Essentials or .NET MAUI
			await Share.RequestAsync(new ShareTextRequest
			{
				Text = encryptedEventJsonString,
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