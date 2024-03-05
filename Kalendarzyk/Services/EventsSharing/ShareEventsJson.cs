using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.Views;
using Newtonsoft.Json;

namespace Kalendarzyk.Services.EventsSharing
{
	public class ShareEventsJson : IShareEventsService
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
				var encryptedEventJsonString = _eventRepository.SerializeAllEventsDataToJson(new List<IGeneralEventModel> { eventModel });

				var fileName = $"{eventModel.Title}.json";      // kics stands for Kalendarzyk ICS (ICS is a file extension for iCalendar files) file format
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
				await App.Current.MainPage.DisplayAlert("ShareFileError", $"{ex}", "yyy");
			}
		}

		public async Task ImportEventsAsync(string jsonString)
		{
			try
			{

				_eventRepository.ImportEventsFromJson(jsonString);

				// Assuming your MainPage is a NavigationPage
				if (Application.Current.MainPage is NavigationPage navigationPage)
				{
					await navigationPage.PushAsync(new EventPage());
				}
				else
				{
					// TODO NOW await Shell.Current.GoToAsync($"///eventpage?data={Uri.EscapeDataString(decryptedJsonString)}");
					//await Shell.Current.GoToAsync("///eventpage");


					// Handle the case where MainPage is not a NavigationPage.
					// You may want to display an error message or handle this differently.
				}
			}
			catch (Exception ex)
			{
				await App.Current.MainPage.DisplayAlert("ImportEventError", $"{ex}", "yyy");
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