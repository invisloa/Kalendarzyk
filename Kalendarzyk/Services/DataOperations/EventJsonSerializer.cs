using CommunityToolkit.Maui.Core.Extensions;
using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace Kalendarzyk.Services.DataOperations
{
	public class EventJsonSerializer : IEventJsonSerializer
	{
		private readonly JsonSerializerSettings _settingsAll;
		private readonly ILocalDataEncryptionService _aesService;


		public EventJsonSerializer(ILocalDataEncryptionService aesService)
		{
			_aesService = aesService;
			_settingsAll = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.All
			};
		}


		public string SerializeAllDataToJson(
			ObservableCollection<IGeneralEventModel> eventsToSaveList,
				ObservableCollection<IGeneralEventModel> allEventsList,
			ObservableCollection<ISubEventTypeModel> allUserEventTypesList,
			ObservableCollection<IMainEventType> allMainEventTypesList)
		{
			var eventsAndTypesToSave = BuildEventsAndTypesForJson(
				eventsToSaveList,
				allEventsList,
				allUserEventTypesList,
				allMainEventTypesList);

			var jsonString = JsonConvert.SerializeObject(eventsAndTypesToSave, _settingsAll);
			//return _aesService.EncryptString(jsonString);
			return jsonString;
		}

		private EventsAndTypesForJson BuildEventsAndTypesForJson(
			ObservableCollection<IGeneralEventModel> eventsToSaveList,
			ObservableCollection<IGeneralEventModel> allEventsList,
			ObservableCollection<ISubEventTypeModel> allUserEventTypesList,
			ObservableCollection<IMainEventType> allMainEventTypesList)
		{
			if (eventsToSaveList == null)
			{
				return new EventsAndTypesForJson
				{
					Events = allEventsList,
					UserEventTypes = allUserEventTypesList,
					MainEventTypes = allMainEventTypesList
				};
			}
			var subTypesToSave = new HashSet<ISubEventTypeModel>();     // HashSet to avoid duplicates - hashset prevents adding items with the same value
			var mainTypesToSave = new HashSet<IMainEventType>();        // HashSet to avoid duplicates 

			foreach (var eventItem in eventsToSaveList)
			{
				subTypesToSave.Add(eventItem.EventType);
				mainTypesToSave.Add(eventItem.EventType.MainEventType);
			}

			return new EventsAndTypesForJson
			{
				Events = eventsToSaveList,
				UserEventTypes = subTypesToSave.ToObservableCollection(),
				MainEventTypes = mainTypesToSave.ToObservableCollection()
			};
		}
		public EventsAndTypesForJson DeserializeEventsAllInfo(string jsonString)
		{
			try
			{
				//var decryptedString = _aesService.DecryptString(jsonString);
				return JsonConvert.DeserializeObject<EventsAndTypesForJson>(jsonString, _settingsAll);
			}
			catch (Exception)
			{
				return null;
			}
		}
		public ObservableCollection<IGeneralEventModel> DeserializeEventsFromJson(string jsonString)
		{
			try
			{
				//var decryptedString = _aesService.DecryptString(jsonString);
				var deserializedEvents = JsonConvert.DeserializeObject<ObservableCollection<IGeneralEventModel>>(jsonString, _settingsAll);
				return deserializedEvents;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public ObservableCollection<ISubEventTypeModel> DeserializeSubEventTypesFromJson(string jsonString)
		{
			try
			{
				//var decryptedString = _aesService.DecryptString(jsonString);
				return JsonConvert.DeserializeObject<ObservableCollection<ISubEventTypeModel>>(jsonString, _settingsAll);
			}
			catch (Exception)
			{
				return null;
			}
		}
		public ObservableCollection<IMainEventType> DeserializeMainEventTypesFromJson(string jsonString)
		{
			try
			{
				//var decryptedString = _aesService.DecryptString(jsonString);
				var deserializedMainEventTypes = JsonConvert.DeserializeObject<ObservableCollection<IMainEventType>>(jsonString, _settingsAll);
				return deserializedMainEventTypes;
			}
			catch (Exception)
			{
				return null;
			}
		}
		public string SerializeEventsToJson(ObservableCollection<IGeneralEventModel> eventsToSaveList)
		{
			var jsonString = JsonConvert.SerializeObject(eventsToSaveList, _settingsAll);
			//return _aesService.EncryptString(jsonString);
			return jsonString;
		}
		public string SerializeSubEventTypesToJson(ObservableCollection<ISubEventTypeModel> subEventTypesToSaveList)
		{
			var jsonString = JsonConvert.SerializeObject(subEventTypesToSaveList, _settingsAll);
			//return _aesService.EncryptString(jsonString);
			return jsonString;
		}
		public string SerializeMainEventTypesToJson(ObservableCollection<IMainEventType> mainEventTypesToSaveList)
		{
			var jsonString = JsonConvert.SerializeObject(mainEventTypesToSaveList, _settingsAll);
			//return _aesService.EncryptString(jsonString);
			return jsonString;
		}
	}
	public class EventsAndTypesForJson
	{
		public ObservableCollection<IGeneralEventModel> Events { get; set; }
		public ObservableCollection<ISubEventTypeModel> UserEventTypes { get; set; }
		public ObservableCollection<IMainEventType> MainEventTypes { get; set; }
	}
}