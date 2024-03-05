
using Kalendarzyk;
using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.Services;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.Services.Notifier;
using Kalendarzyk.Views;

public class LocalMachineEventRepository : IEventRepository
{
	ILocalDataEncryptionService _aesService = Factory.CreateNewLocalDataEncryptionService();
	ILocalFilePathService _localFilePathService = Factory.CreateNewLocalFilePathService();
	IEventJsonSerializer _eventJsonSerializer = Factory.CreateNewEventJsonSerializer();
	IFileStorageService _fileStorageService = Factory.CreateNewFileStorageService();
	IFileSelectorService _fileSelectorService = Factory.CreateNewFileSelectorService();
	IUserNotifier userNotifier = Factory.CreateNewUserNotifier();
	// File Paths generation code
	#region File Paths generation code

	public event Action OnEventListChanged;
	public event Action OnMainEventTypesListChanged;    // TODO - implement
	public event Action OnUserEventTypeListChanged;
	#endregion

	//CTOR
	public LocalMachineEventRepository()
	{
	}

	#region Events Repository
	private List<IGeneralEventModel> _allEventsList = new List<IGeneralEventModel>();
	public List<IGeneralEventModel> AllEventsList
	{
		get
		{
			return _allEventsList;
		}
		private set
		{
			if (_allEventsList == value) { return; }
			_allEventsList = value;
			OnEventListChanged?.Invoke();
		}
	}
	private List<IMainEventType> _allMainEventTypesList = new List<IMainEventType>();
	public List<IMainEventType> AllMainEventTypesList
	{
		get
		{
			return _allMainEventTypesList;
		}
		private set
		{
			if (_allMainEventTypesList == value) { return; }
			_allMainEventTypesList = value;
			OnMainEventTypesListChanged?.Invoke();
		}
	}
	public async Task AddMainEventTypeAsync(IMainEventType mainEventTypeToAdd)
	{
		if (AllMainEventTypesList.Contains(mainEventTypeToAdd))
		{
			var action = await App.Current.MainPage.DisplayActionSheet($"Event {mainEventTypeToAdd.Title} already exists", "Cancel", null, "Overwrite", "Duplicate");
			switch (action)
			{
				case "Overwrite":
					var eventItem = AllMainEventTypesList.FirstOrDefault(e => e.Equals(mainEventTypeToAdd));            // to check
					if (eventItem != null)
					{
						AllMainEventTypesList.Remove(eventItem);
						AllMainEventTypesList.Add(eventItem);
					}
					break;
				case "Duplicate":
					mainEventTypeToAdd.Title += " (.)";
					AllMainEventTypesList.Add(mainEventTypeToAdd);
					break;

				default:
					// Cancel was selected or back button was pressed.
					return;
			}
		}
		else
		{
			AllMainEventTypesList.Add(mainEventTypeToAdd);
		}
		OnMainEventTypesListChanged?.Invoke();
		await SaveMainEventTypesListAsync();
	}
	public async Task AddEventAsync(IGeneralEventModel eventToAdd)
	{
		AllEventsList.Add(eventToAdd);
		await SaveEventsListAsync();
		OnEventListChanged?.Invoke();
	}
	public async Task ClearAllEventsListAsync()
	{
		AllEventsList.Clear();
		await SaveEventsListAsync();
		OnEventListChanged?.Invoke();

	}
	public async Task ClearAllSubEventTypesAsync()
	{
		await ClearAllEventsListAsync();
		AllUserEventTypesList.Clear();
		await SaveSubEventTypesListAsync();
		OnUserEventTypeListChanged?.Invoke();
	}
	public async Task ClearAllMainEventTypesAsync()
	{

		AllMainEventTypesList.Clear();
		await SaveMainEventTypesListAsync();
		OnMainEventTypesListChanged?.Invoke();
	}
	public async Task<List<IGeneralEventModel>> GetEventsListAsync()
	{
		var jsonString = await _fileStorageService.ReadFileAsync(_localFilePathService.EventsFilePath);
		AllEventsList = _eventJsonSerializer.DeserializeEventsFromJson(jsonString) ?? new List<IGeneralEventModel>();
		return AllEventsList;
	}
	public async Task SaveEventsListAsync()
	{
		try
		{
			if (AllEventsList.Count > 0)
			{
				AllEventsList = AllEventsList.OrderBy(e => e.StartDateTime).ToList();
			}
			var jsonString = _eventJsonSerializer.SerializeEventsToJson(AllEventsList);
			await _fileStorageService.WriteFileAsync(_localFilePathService.EventsFilePath, jsonString);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message + "while SaveEventsListAsync");
		}
	}
	public Task<IGeneralEventModel> GetEventByIdAsync(Guid eventId)
	{
		var selectedEvent = AllEventsList.FirstOrDefault(e => e.Id == eventId);
		return Task.FromResult(selectedEvent);
	}
	#endregion


	// SubTypes Repository
	#region SubTypes Repository
	private List<ISubEventTypeModel> _allUserEventTypesList = new List<ISubEventTypeModel>();
	public List<ISubEventTypeModel> AllUserEventTypesList
	{
		get
		{
			return _allUserEventTypesList;
		}
		private set
		{
			if (_allUserEventTypesList == value) { return; }
			_allUserEventTypesList = value;
			OnUserEventTypeListChanged?.Invoke();
		}
	}
	public async Task InitializeAsync()
	{
		AllEventsList = await GetEventsListAsync();                         // TO CHECK -  ConfigureAwait
		AllUserEventTypesList = await GetSubEventTypesListAsync();          // TO CHECK -  ConfigureAwait
		AllMainEventTypesList = await GetMainEventTypesListAsync();         // TO CHECK -  ConfigureAwait
	}
	public async Task<List<IMainEventType>> GetMainEventTypesListAsync()
	{
		var jsonString = await _fileStorageService.ReadFileAsync(_localFilePathService.MainEventsTypesFilePath);
		var deserializedMainEventTypes = _eventJsonSerializer.DeserializeMainEventTypesFromJson(jsonString);
		AllMainEventTypesList = deserializedMainEventTypes ?? new List<IMainEventType>();
		return AllMainEventTypesList;
	}
	public async Task<List<ISubEventTypeModel>> GetSubEventTypesListAsync()
	{

		var jsonString = await _fileStorageService.ReadFileAsync(_localFilePathService.SubEventsTypesFilePath);
		var deserializedSubEventTypes = _eventJsonSerializer.DeserializeSubEventTypesFromJson(jsonString);
		AllUserEventTypesList = deserializedSubEventTypes ?? new List<ISubEventTypeModel>();
		return AllUserEventTypesList;
	}

	public async Task SaveSubEventTypesListAsync()
	{
		var jsonString = _eventJsonSerializer.SerializeSubEventTypesToJson(AllUserEventTypesList);
		await _fileStorageService.WriteFileAsync(_localFilePathService.SubEventsTypesFilePath, jsonString);
		// TO CHECK OnUserEventTypeListChanged?.Invoke();
	}
	public async Task SaveMainEventTypesListAsync()
	{
		/*		var settings = JsonSerializerSettings_Auto;
				var jsonString = JsonConvert.SerializeObject(AllMainEventTypesList, settings);
				await File.WriteAllTextAsync(_localFilePathService.MainEventsTypesFilePath, jsonString);*/
		var jsonString = _eventJsonSerializer.SerializeMainEventTypesToJson(AllMainEventTypesList);
		await _fileStorageService.WriteFileAsync(_localFilePathService.MainEventsTypesFilePath, jsonString);
		// TO CHECK OnMainEventTypesListChanged?.Invoke();
	}
	public async Task DeleteFromEventsListAsync(IGeneralEventModel eventToDelete)
	{
		AllEventsList.Remove(eventToDelete);
		await SaveEventsListAsync();
		OnEventListChanged?.Invoke();
	}
	public async Task DeleteFromMainEventTypesListAsync(IMainEventType mainEventTypeToDelete)
	{
		AllMainEventTypesList.Remove(mainEventTypeToDelete);
		await SaveMainEventTypesListAsync();
		OnMainEventTypesListChanged?.Invoke();
	}
	public async Task DeleteFromSubEventTypesListAsync(ISubEventTypeModel eventTypeToDelete)
	{
		AllUserEventTypesList.Remove(eventTypeToDelete);
		await SaveSubEventTypesListAsync();
		OnUserEventTypeListChanged?.Invoke();
	}
	public async Task AddSubEventTypeAsync(ISubEventTypeModel eventTypeToAdd)
	{
		AllUserEventTypesList.Add(eventTypeToAdd);
		OnUserEventTypeListChanged?.Invoke();
		await SaveSubEventTypesListAsync();
	}
	public async Task UpdateEventAsync(IGeneralEventModel eventToUpdate)   // TO TEST
	{
		var updatedEvent = AllEventsList.FirstOrDefault(e => e.Id == eventToUpdate.Id);
		if (updatedEvent != null)
		{
			updatedEvent = eventToUpdate;

			// TO CHECK
			await SaveEventsListAsync();
		}
		else
		{
			await Task.CompletedTask;
		}
	}

	public async Task UpdateSubEventTypeAsync(ISubEventTypeModel eventTypeToUpdate)
	{
		var x = AllUserEventTypesList.FirstOrDefault(e => e.Equals(eventTypeToUpdate));
		if (x != null)
		{
			x = eventTypeToUpdate;
		}
		else
		{
			await Task.CompletedTask;
		}
		await SaveSubEventTypesListAsync();
		OnUserEventTypeListChanged?.Invoke();
	}
	public async Task UpdateMainEventTypeAsync(IMainEventType eventTypeToUpdate)
	{
		await SaveMainEventTypesListAsync();
		OnMainEventTypesListChanged?.Invoke();
	}
	public Task<ISubEventTypeModel> GetSubEventTypeAsync(ISubEventTypeModel eventTypeToSelect)
	{
		var selectedEventType = AllUserEventTypesList.FirstOrDefault(e => e.Equals(eventTypeToSelect));
		return Task.FromResult(selectedEventType);
	}
	public Task<IMainEventType> GetMainEventTypeAsync(IMainEventType eventTypeToSelect)
	{
		var selectedEventType = AllMainEventTypesList.FirstOrDefault(e => e.Equals(eventTypeToSelect));
		return Task.FromResult(selectedEventType);
	}
	public List<IGeneralEventModel> DeepCopyAllEventsList()
	{
		/*		var settings = JsonSerializerSettings_Auto;
				var serialized = JsonConvert.SerializeObject(_allEventsList, settings);
				return JsonConvert.DeserializeObject<List<IGeneralEventModel>>(serialized, settings);
		*/
		var jsonString = _eventJsonSerializer.SerializeEventsToJson(AllEventsList);
		return _eventJsonSerializer.DeserializeEventsFromJson(jsonString);
	}
	public List<ISubEventTypeModel> DeepCopySubEventTypesList()
	{
		/*		var settings = JsonSerializerSettings_Auto;
				var serialized = JsonConvert.SerializeObject(_allUserEventTypesList, settings);
				return JsonConvert.DeserializeObject<List<ISubEventTypeModel>>(serialized, settings);
		*/
		var jsonString = _eventJsonSerializer.SerializeSubEventTypesToJson(AllUserEventTypesList);
		return _eventJsonSerializer.DeserializeSubEventTypesFromJson(jsonString);
	}
	public List<IMainEventType> DeepCopyMainEventTypesList()
	{
		/*		var settings = JsonSerializerSettings_Auto;
				var serialized = JsonConvert.SerializeObject(_allMainEventTypesList, settings);
				return JsonConvert.DeserializeObject<List<IMainEventType>>(serialized, settings);*/
		var jsonString = _eventJsonSerializer.SerializeSubEventTypesToJson(AllUserEventTypesList);
		return _eventJsonSerializer.DeserializeMainEventTypesFromJson(jsonString);
	}
	#endregion

	#region FILE SAVE AND LOAD

	// Method for creating JSON string from events data it uses the EventsAndTypesForJson class because interface types cannot be newed up
	public string SerializeAllEventsDataToJson(List<IGeneralEventModel> eventsToSaveList)
	{
		return _eventJsonSerializer.SerializeAllDataToJson(eventsToSaveList, AllEventsList, AllUserEventTypesList, AllMainEventTypesList);
	}


	public async Task<string> SelectAndReadFileAsync(CancellationToken cancellationToken)
	{

		try
		{
			var selectedFilePath = await _fileSelectorService.AsyncSelectFile();
			if (selectedFilePath != null)
			{
				return await _fileStorageService.ReadFileAsync(selectedFilePath);
			}
			else
			{
				await userNotifier.ShowMessageAsync($"Failed to pick a file: User canceled file picking", cancellationToken);
				return null;
			}
		}
		catch (Exception ex)
		{
			await userNotifier.ShowMessageAsync($"An error occurred while loading the file: {ex.Message}", cancellationToken);
			return null;
		}
	}
	// Method for processing the events data
	public async Task ImportEventsFromJson(string jsonData)
	{
		try
		{
			if (string.IsNullOrEmpty(jsonData))
			{
				await App.Current.MainPage.DisplayAlert("LoadEventsFromJsonError", $"jsonData is null or empty", "yyy");
				return;
			}
			var deserializedEventsAndTypesdData = DeserializeEventsDataFromJson(jsonData);
			await ImportEventsData(deserializedEventsAndTypesdData);
		}
		catch (Exception ex)
		{
			await App.Current.MainPage.DisplayAlert("LoadEventsFromJsonError", $"{ex}", "yyy");
		}
	}
	private EventsAndTypesForJson DeserializeEventsDataFromJson(string encryptedJsonData)
	{
		return _eventJsonSerializer.DeserializeEventsAllInfo(encryptedJsonData);
	}

	private async Task ImportEventsData(EventsAndTypesForJson deserializedEventsAndTypesdData)  // TO REFACTOR
	{
		// before adding event there has to be its main and sub type added
		ImportMainAndSubEventTypes(deserializedEventsAndTypesdData);       // for now just import all main and sub event types without asking the user what to do


		if (deserializedEventsAndTypesdData.Events.Count > 1)
		{
			await LoadMultipleEventsFromJson(deserializedEventsAndTypesdData);
		}
		else
		{
			await LoadSingleEventFromJson(deserializedEventsAndTypesdData);
		}
	}

	private async Task LoadMultipleEventsFromJson(EventsAndTypesForJson deserializedEventsAndTypesdData)
	{

		foreach (var eventItem in deserializedEventsAndTypesdData.Events)
		{
			var isEventAlreadyAdded = AllEventsList.Any(e => e.Id == eventItem.Id);
			if (!isEventAlreadyAdded)
			{
				AllEventsList.Add(eventItem);
			}
			else
			{
				// ask the user if he wants to overwrite the event
				var action = await App.Current.MainPage.DisplayActionSheet($"Event {eventItem.Title} already exists", "Cancel", null, "Overwrite", "Duplicate", "Edit shared event");       // TO REFACTOR
				switch (action)
				{
					case "Overwrite":
						var eventToUpdate = AllEventsList.FirstOrDefault(e => e.Id == eventItem.Id);
						if (eventToUpdate != null)
						{
							AllEventsList.Remove(eventToUpdate);
							AllEventsList.Add(eventItem);
						}
						break;
					case "Duplicate":
						eventItem.Title += " (.)";
						AllEventsList.Add(eventItem);
						break;
					case "Edit shared event":

						// TODO NOW	await Shell.Current.GoToAsync($"///eventpage?data={Uri.EscapeDataString(decryptedJsonData)}");

						break;
					case "Skip":
						// Do nothing, just skip.
						break;
					default:
						// Cancel was selected or back button was pressed.
						break;
				}
			}
		}
		await SaveEventsListAsync();
		await SaveSubEventTypesListAsync();
		await SaveMainEventTypesListAsync();
	}
	private async Task LoadSingleEventFromJson(EventsAndTypesForJson deserializedEventsAndTypesdData)
	{
		var eventItem = deserializedEventsAndTypesdData.Events[0];
		var isEventAlreadyAdded = AllEventsList.Any(e => e.Id == eventItem.Id);
		if (!isEventAlreadyAdded)
		{
			try
			{
				Application.Current.MainPage.Navigation.PushAsync(new EventPage(eventItem));

			}
			catch (Exception ex)
			{
				await App.Current.MainPage.DisplayAlert("LoadSingleEventFromJsonError", $"{ex}", "yyy");
			}
		}
		else
		{
			// ask the user if he wants to overwrite the event
			var action = await App.Current.MainPage.DisplayActionSheet($"Event {eventItem.Title} already exists", "Cancel", null, "Overwrite", "Duplicate", "Edit", "Skip");        // TO REFACTOR
			switch (action)
			{
				case "Overwrite":
					var eventToUpdate = AllEventsList.FirstOrDefault(e => e.Id == eventItem.Id);
					if (eventToUpdate != null)
					{
						AllEventsList.Remove(eventToUpdate);
						AllEventsList.Add(eventItem);
					}
					break;
				case "Duplicate":
					eventItem.Title += " (.)";
					AllEventsList.Add(eventItem);
					break;
				case "Edit":
					Application.Current.MainPage.Navigation.PushAsync(new EventPage(eventItem));
					break;
				case "Skip":
					// Do nothing, just skip.
					break;
				default:
					// Cancel was selected or back button was pressed.
					break;
			}
			await SaveEventsListAsync();
		}
	}
	private void ImportMainAndSubEventTypes(EventsAndTypesForJson deserializedEventsAndTypesdData)
	{       // for now just import all main and sub event types without asking the user

		foreach (var eventType in deserializedEventsAndTypesdData.UserEventTypes)
		{
			if (!AllUserEventTypesList.Contains(eventType))
			{
				AllUserEventTypesList.Add(eventType);
			}
			if (!AllMainEventTypesList.Contains(eventType.MainEventType))
			{
				AllMainEventTypesList.Add(eventType.MainEventType);
			}
		}
	}

	// Original method refactored to use the new methods
	public async Task LoadEventsAndTypesFromFile(CancellationToken cancellationToken)
	{
		var jsonData = await SelectAndReadFileAsync(cancellationToken);
		await ImportEventsFromJson(jsonData);
	}

	//private static readonly JsonSerializerSettings JsonSerializerSettings_Auto = new JsonSerializerSettings
	//{
	//	TypeNameHandling = TypeNameHandling.Auto
	//};
	//private static readonly JsonSerializerSettings JsonSerializerSettings_All = new JsonSerializerSettings
	//{
	//	TypeNameHandling = TypeNameHandling.All
	//};

	// if eventsToSaveList is null then all events will be saved
	public async Task SaveEventsAndTypesToFile(List<IGeneralEventModel> eventsToSaveList = null)
	{
		try
		{
			/*			var encryptedString = SerializeEventsToJson(eventsToSaveList); // Create the jsonString with encryption
						using var stream = new MemoryStream(Encoding.UTF8.GetBytes(encryptedString)); // Use UTF8 Encoding

						var fileSaverResult = await FileSaver.Default.SaveAsync("EventsList.json", stream, CancellationToken.None);
						if (fileSaverResult.IsSuccessful)
						{
							await Toast.Make($"The file was saved successfully to location: {fileSaverResult.FilePath}").Show(CancellationToken.None);
						}
						else
						{
							await Toast.Make($"The file was not saved successfully with error: {fileSaverResult.Exception.Message}").Show(CancellationToken.None);
						}*/
			var encryptedString = SerializeAllEventsDataToJson(eventsToSaveList); // Create the jsonString with encryption
			await _fileStorageService.WriteFileAsync(_localFilePathService.EventsFilePath, encryptedString);
			await userNotifier.ShowMessageAsync($"The file was saved successfully to location: {_localFilePathService.EventsFilePath}", CancellationToken.None);
		}
		catch (Exception ex)
		{
			await userNotifier.ShowMessageAsync($"The file was not saved successfully with error: {ex.Message}", CancellationToken.None);
		}

	}
	public async Task LoadEventsAndTypesFromFile()
	{
		await LoadEventsAndTypesFromFile(CancellationToken.None);
	}


	#endregion
}
