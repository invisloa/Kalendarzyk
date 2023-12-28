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
		public string SerializeEventToJson(IGeneralEventModel eventModel)
		{
			var jsonString = JsonConvert.SerializeObject(eventModel);
			return jsonString;
		}
		public async Task ShareEventAsync(IGeneralEventModel eventModel)
		{

			try
			{
				// Serialize the event to a JSON string
				var eventJsonString = SerializeEventToJson(eventModel);

				// Encrypt the JSON string
				var encryptedEventJsonString = _aesService.EncryptString(eventJsonString);

				var fileName = $"{eventModel.Title}.kics";      // kics stands for Kalendarzyk ICS (ICS is a file extension for iCalendar files) file format
				var tempFilePath = GetTemporaryFilePath(fileName);

				// Write the encrypted data to the temporary file
				File.WriteAllText(tempFilePath, encryptedEventJsonString);

				// Share the file using the temporary path
				await Share.RequestAsync(new ShareFileRequest
				{
					Title = $"Share {eventModel.Title}",
					File = new ShareFile(tempFilePath)
				});
			}
			catch (Exception ex)
			{
				await App.Current.MainPage.DisplayAlert("ShareFileError", $"{ex}", "XXX");
			}
		}

		public async Task ImportEventAsync(string jsonString)
		{
			var decryptedJsonString = _aesService.DecryptString(jsonString);
			var eventModel = JsonConvert.DeserializeObject<IGeneralEventModel>(decryptedJsonString);
			var eventExists = await _eventRepository.GetEventByIdAsync(eventModel.Id) != null;

			if (!eventExists)
			{
				await _eventRepository.AddEventAsync(eventModel);
			}
			else
			{
				// TODO: Add a message or handle the case when the event already exists
			}
		}
		public string GetTemporaryFilePath(string fileName)
		{
			// Ensure the filename is unique to avoid conflicts
			var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
			return Path.Combine(FileSystem.CacheDirectory, uniqueFileName);
		}


	}
}