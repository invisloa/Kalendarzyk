using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;
using System.Collections.ObjectModel;

namespace Kalendarzyk.Services.DataOperations
{
	public interface IEventJsonSerializer
	{
		EventsAndTypesForJson DeserializeEventsAllInfo(string jsonString);
		ObservableCollection<IGeneralEventModel> DeserializeEventsFromJson(string jsonString);
		ObservableCollection<IMainEventType> DeserializeMainEventTypesFromJson(string jsonString);
		ObservableCollection<ISubEventTypeModel> DeserializeSubEventTypesFromJson(string jsonString);
		string SerializeEventsToJson(ObservableCollection<IGeneralEventModel> eventsToSaveList);
		string SerializeAllDataToJson(ObservableCollection<IGeneralEventModel> eventsToSaveList, ObservableCollection<IGeneralEventModel> allEventsList, ObservableCollection<ISubEventTypeModel> allUserEventTypesList, ObservableCollection<IMainEventType> allMainEventTypesList);
		string SerializeMainEventTypesToJson(ObservableCollection<IMainEventType> mainEventTypesToSaveList);
		string SerializeSubEventTypesToJson(ObservableCollection<ISubEventTypeModel> subEventTypesToSaveList);
	}
}