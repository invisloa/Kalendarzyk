using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;

namespace Kalendarzyk.Services.DataOperations
{
	public interface IEventRepository
	{
		public event Action OnEventListChanged;
		public event Action OnUserEventTypeListChanged;
		Task AddEventAsync(IGeneralEventModel eventToAdd);
		Task AddSubEventTypeAsync(ISubEventTypeModel eventTypeToAdd);
		Task AddMainEventTypeAsync(IMainEventType eventTypeToAdd);
		Task SaveEventsListAsync();
		Task SaveSubEventTypesListAsync();
		Task SaveMainEventTypesListAsync();
		Task UpdateEventAsync(IGeneralEventModel eventToUpdate);
		Task UpdateSubEventTypeAsync(ISubEventTypeModel eventTypeToUpdate);
		Task UpdateMainEventTypeAsync(IMainEventType eventTypeToUpdate);
		Task DeleteFromEventsListAsync(IGeneralEventModel eventToDelete);
		Task DeleteFromSubEventTypesListAsync(ISubEventTypeModel eventTypeToDelete);
		Task DeleteFromMainEventTypesListAsync(IMainEventType eventTypeToDelete);

		Task<IGeneralEventModel> GetEventByIdAsync(Guid eventId);

		Task<ISubEventTypeModel> GetSubEventTypeAsync(ISubEventTypeModel eventTypeToSelect);
		Task<IMainEventType> GetMainEventTypeAsync(IMainEventType eventTypeToSelect);
		string SerializeAllEventsDataToJson(List<IGeneralEventModel> eventsToSaveList);
		Task ClearAllEventsListAsync();
		Task ClearAllSubEventTypesAsync();
		Task ClearAllMainEventTypesAsync();

		List<IGeneralEventModel> AllEventsList { get; }
		List<ISubEventTypeModel> AllUserEventTypesList { get; }
		List<IMainEventType> AllMainEventTypesList { get; }
		List<IGeneralEventModel> DeepCopyAllEventsList();
		List<ISubEventTypeModel> DeepCopySubEventTypesList();
		List<IMainEventType> DeepCopyMainEventTypesList();
		Task<List<IGeneralEventModel>> GetEventsListAsync();
		Task<List<ISubEventTypeModel>> GetSubEventTypesListAsync();
		Task<List<IMainEventType>> GetMainEventTypesListAsync();
		Task SaveEventsAndTypesToFile(List<IGeneralEventModel> eventsToSave = null);
		Task LoadEventsAndTypesFromFile();
		Task InitializeAsync();
		Task ImportEventsFromJson(string jsonData);
	}
}




