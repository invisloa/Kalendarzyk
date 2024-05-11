using Kalendarzyk.Helpers;
using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.Services;
using Newtonsoft.Json;

namespace Kalendarzyk.Models.EventModels
{
	public abstract class AbstractEventModel : IGeneralEventModel
	{
		private TimeSpan _defaulteventRemindertime = TimeSpan.FromHours(24);
		public Guid Id { get; }
		public DateTime StartDateTime { get; set; }
		public DateTime EndDateTime { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public bool WasShown { get; set; }
		public virtual bool IsCompleted { get; set; }
		public ISubEventTypeModel EventType { get; set; }
		public List<DateTime> PostponeHistory { get; set; }
		public TimeSpan DefaultPostponeTime { get; set; }
		public TimeSpan ReminderTime { get; set; }

		// New property to store notification integer ID
		public int? NotificationId { get; }
		private INotificationIDGenerator _notificationIDGenerator => Factory.CreateNotificationIDGenerator();

		[JsonIgnore]
		public Color EventVisibleColor
		{
			get
			{
				Color color = EventType.EventTypeColor;
				return color;
			}
		}

		// TO Consider postpone time and maybe some other extra options for advanced event adding mode??
		public AbstractEventModel(string title, string description, DateTime startTime, DateTime endTime, ISubEventTypeModel eventType,
								bool isCompleted = false, TimeSpan? postponeTime = null, bool wasShown = false, Guid? id = null, int? notificationID = null, bool usesNotification = false)
		{
			ReminderTime = postponeTime ?? _defaulteventRemindertime;
			Id = id ?? Guid.NewGuid();
			Title = title;
			Description = description;
			StartDateTime = startTime;
			EndDateTime = endTime;
			EventType = eventType;
			IsCompleted = isCompleted;
			WasShown = wasShown;

			PostponeHistory = new List<DateTime>(); // default new list 
			NotificationId = usesNotification ? (notificationID ?? _notificationIDGenerator.GetNextUniqueId()) : null;
		}
	}
}
