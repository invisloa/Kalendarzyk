using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk.Services.DataOperations
{
	public class EventJsonSerializer : IEventJsonSerializer
	{
		private readonly JsonSerializerSettings _settingsAll;
		private readonly JsonSerializerSettings _settingsAuto;
		private readonly ILocalDataEncryptionService _aesService;


		public EventJsonSerializer(ILocalDataEncryptionService aesService)
		{
			_aesService = aesService;
			_settingsAll = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.All
			};
			_settingsAuto = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Auto
			};

		}


		public string SerializeAllDataToJson(
			List<IGeneralEventModel> eventsToSaveList,
			List<IGeneralEventModel> allEventsList,
			List<ISubEventTypeModel> allUserEventTypesList,
			List<IMainEventType> allMainEventTypesList)
		{
			var eventsAndTypesToSave = BuildEventsAndTypesForJson(
				eventsToSaveList,
				allEventsList,
				allUserEventTypesList,
				allMainEventTypesList);

			var jsonString = JsonConvert.SerializeObject(eventsAndTypesToSave, _settingsAll);
			return _aesService.EncryptString(jsonString);
		}

		private EventsAndTypesForJson BuildEventsAndTypesForJson(
			List<IGeneralEventModel> eventsToSaveList,
			List<IGeneralEventModel> allEventsList,
			List<ISubEventTypeModel> allUserEventTypesList,
			List<IMainEventType> allMainEventTypesList)
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
				UserEventTypes = subTypesToSave.ToList(),
				MainEventTypes = mainTypesToSave.ToList()
			};
		}
		public EventsAndTypesForJson DeserializeEventsAllInfo(string jsonString)
		{
			try
			{
				var decryptedString = _aesService.DecryptString(jsonString);
				return JsonConvert.DeserializeObject<EventsAndTypesForJson>(decryptedString, _settingsAll);
			}
			catch (Exception)
			{
				return null;
			}
		}
		public List<IGeneralEventModel> DeserializeEventsFromJson(string jsonString)
		{
			try
			{
				var decryptedString = _aesService.DecryptString(jsonString);
				var deserializedEvents = JsonConvert.DeserializeObject<List<IGeneralEventModel>>(decryptedString, _settingsAll);
				return deserializedEvents;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public List<ISubEventTypeModel> DeserializeSubEventTypesFromJson(string jsonString)
		{
			try
			{
				var decryptedString = _aesService.DecryptString(jsonString);
				return JsonConvert.DeserializeObject<List<ISubEventTypeModel>>(decryptedString, _settingsAuto);
			}
			catch (Exception)
			{
				return null;
			}
		}
		public List<IMainEventType> DeserializeMainEventTypesFromJson(string jsonString)
		{
			try
			{
				var decryptedString = _aesService.DecryptString(jsonString);
				var deserializedMainEventTypes = JsonConvert.DeserializeObject<List<IMainEventType>>(decryptedString, _settingsAuto);
				return deserializedMainEventTypes;
			}
			catch (Exception)
			{
				return null;
			}
		}
		public string SerializeEventsToJson(List<IGeneralEventModel> eventsToSaveList)
		{
			var jsonString = JsonConvert.SerializeObject(eventsToSaveList, _settingsAll);
			return _aesService.EncryptString(jsonString);
		}
		public string SerializeSubEventTypesToJson(List<ISubEventTypeModel> subEventTypesToSaveList)
		{
			var jsonString = JsonConvert.SerializeObject(subEventTypesToSaveList, _settingsAuto);
			return _aesService.EncryptString(jsonString);
		}
		public string SerializeMainEventTypesToJson(List<IMainEventType> mainEventTypesToSaveList)
		{
			var jsonString = JsonConvert.SerializeObject(mainEventTypesToSaveList, _settingsAuto);
			return _aesService.EncryptString(jsonString);
		}
	}
	public class EventsAndTypesForJson
	{
		public List<IGeneralEventModel> Events { get; set; }
		public List<ISubEventTypeModel> UserEventTypes { get; set; }
		public List<IMainEventType> MainEventTypes { get; set; }
	}
}