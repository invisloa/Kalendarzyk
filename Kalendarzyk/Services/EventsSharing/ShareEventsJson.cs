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
				// Serialize the event to a JSON string Encrypt the JSON string
				var encryptedEventJsonString = _eventRepository.SerializeEventsToJson(new List<IGeneralEventModel> { eventModel });

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

			// at this moment of the program creation there is no eventrepository so the below code is commented out
			var eventExists = await _eventRepository.GetEventByIdAsync(eventModel.Id) != null;
			if (eventExists)
			{
				await App.Current.MainPage.DisplayAlert("EventExists", "Event with this Id already exists", "XXX");
				return;
			}
			_eventRepository.AddEventAsync(eventModel);

			//just go to editeventpage

			await Shell.Current.GoToAsync("..");


		}
		public string GetTemporaryFilePath(string fileName)
		{
			// Ensure the filename is unique to avoid conflicts
			var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
			return Path.Combine(FileSystem.CacheDirectory, uniqueFileName);
		}


	}
}