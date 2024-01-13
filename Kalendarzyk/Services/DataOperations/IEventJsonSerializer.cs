using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;

namespace Kalendarzyk.Services.DataOperations
{
	public interface IEventJsonSerializer
	{
		EventsAndTypesForJson DeserializeEvents(string jsonData);
		string SerializeEventsToJson(List<IGeneralEventModel> eventsToSaveList, List<IGeneralEventModel> allEventsList, List<ISubEventTypeModel> allUserEventTypesList, List<IMainEventType> allMainEventTypesList);
	}
}