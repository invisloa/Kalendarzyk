using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;
using System.Collections.ObjectModel;

namespace Kalendarzyk.Services.DataOperations
{
	public interface IEventRepository
	{
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
		string SerializeAllEventsDataToJson(ObservableCollection<IGeneralEventModel> eventsToSaveList);
		Task ClearAllEventsListAsync();
		Task ClearAllSubEventTypesAsync();
		Task ClearAllMainEventTypesAsync();

		ObservableCollection<IGeneralEventModel> AllEventsList { get; }
		ObservableCollection<ISubEventTypeModel> AllUserEventTypesList { get; }
		ObservableCollection<IMainEventType> AllMainEventTypesList { get; }
		ObservableCollection<IGeneralEventModel> DeepCopyAllEventsList();
		ObservableCollection<ISubEventTypeModel> DeepCopySubEventTypesList();
		ObservableCollection<IMainEventType> DeepCopyMainEventTypesList();
		Task<ObservableCollection<IGeneralEventModel>> GetEventsListAsync();
		Task<ObservableCollection<ISubEventTypeModel>> GetSubEventTypesListAsync();
		Task<ObservableCollection<IMainEventType>> GetMainEventTypesListAsync();
		Task SaveEventsAndTypesToFile(ObservableCollection<IGeneralEventModel> eventsToSave = null);
		Task LoadEventsAndTypesFromFile();
		Task InitializeAsync();
		Task ImportEventsFromJson(string jsonData);
	}
}




