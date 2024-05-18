using Kalendarzyk.Models.EventModels;

namespace Kalendarzyk.Models.EventTypesModels
{
	public interface ISubEventTypeModel : IEquatable<ISubEventTypeModel>
	{
		IMainEventType MainEventType { get; set; }
		public bool IsValueType { get; set; }   // if the event type is a value type, it will be shown in the value type list
		public bool IsMicroTaskType { get; set; }   // if the event type is a IsMicroTaskType, it will be shown in the IsMicroTaskType list
		Color EventTypeColor { get; set; }  // original event color
		Color BackgroundColor { get; set; } // color that is currently shown (isCompleted color adjustment)
		string EventTypeColorString { get; set; }   // needed for json serialization
		string EventTypeName { get; set; }
		bool IsSelectedToFilter { get; set; }		//  TODO XXXX YYYY Refactor this->  delete and move this to a viewmodel!!!
		QuantityModel QuantityAmount { get; set; }
		IEnumerable<MicroTaskModel> MicroTasksList { get; set; }
		public TimeSpan DefaultEventTimeSpan { get; set; }  // default event time for the event type
		string ToString();
		new bool Equals(ISubEventTypeModel other);  // to check if the event type is already in the list
		int GetHashCode();  // to check if the event type is already in the list
	}
}