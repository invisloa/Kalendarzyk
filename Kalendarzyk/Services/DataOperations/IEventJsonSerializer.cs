using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;

namespace Kalendarzyk.Services.DataOperations
{
	public interface IEventJsonSerializer
	{
		EventsAndTypesForJson DeserializeEventsAllInfo(string jsonString);
		List<IGeneralEventModel> DeserializeEventsFromJson(string jsonString);
		List<IMainEventType> DeserializeMainEventTypesFromJson(string jsonString);
		List<ISubEventTypeModel> DeserializeSubEventTypesFromJson(string jsonString);
		string SerializeEventsToJson(List<IGeneralEventModel> eventsToSaveList);
		string SerializeAllDataToJson(List<IGeneralEventModel> eventsToSaveList, List<IGeneralEventModel> allEventsList, List<ISubEventTypeModel> allUserEventTypesList, List<IMainEventType> allMainEventTypesList);
		string SerializeMainEventTypesToJson(List<IMainEventType> mainEventTypesToSaveList);
		string SerializeSubEventTypesToJson(List<ISubEventTypeModel> subEventTypesToSaveList);
	}
}