using Kalendarzyk.Models.EventTypesModels;

namespace Kalendarzyk.Models.EventModels
{
	public interface IGeneralEventModel
	{
		string Description { get; set; }
		ISubEventTypeModel EventType { get; set; }
		Color EventVisibleColor { get; }
		Guid Id { get; }
		bool IsCompleted { get; set; }
		List<DateTime> PostponeHistory { get; set; }
		TimeSpan ReminderTime { get; set; }
		DateTime StartDateTime { get; set; }
		DateTime EndDateTime { get; set; }
		string Title { get; set; }
		bool WasShown { get; set; }
		public QuantityModel QuantityAmount { get; set; }
		public IEnumerable<MicroTaskModel> MicroTasksList { get; set; }

		// New property to store notification integer ID
		public int? NotificationId { get; }
	}
}