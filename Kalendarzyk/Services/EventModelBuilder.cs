using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;

namespace Kalendarzyk.Services
{
	public class EventModelBuilder
	{
		// Required Parameters
		private readonly string title;
		private readonly string description;
		private readonly DateTime startTime;
		private readonly DateTime endTime;
		private readonly ISubEventTypeModel eventType;

		// Optional Parameters with Defaults
		private bool isCompleted = false;
		private TimeSpan? postponeTime = null;
		private bool wasShown = false;
		private QuantityModel quantityAmount = null;
		private IEnumerable<MicroTaskModel> _microTasksList = null;
		private int? notificationID = null;
		private Guid? guid = null;


		public EventModelBuilder(string title, string description, DateTime startTime, DateTime endTime, ISubEventTypeModel eventType, bool isCompleted, TimeSpan? postponeTime, bool wasShown, int? notificationID)
		{
			// Validate Required Parameters Here
			if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title must not be empty", nameof(title));
			if (eventType == null) throw new ArgumentNullException(nameof(eventType));

			this.title = title;
			this.description = description;
			this.startTime = startTime;
			this.endTime = endTime;
			this.eventType = eventType;
			this.isCompleted = isCompleted;
			if (postponeTime.HasValue)
				this.postponeTime = postponeTime.Value;
			else
				this.postponeTime = TimeSpan.FromHours(24);
			this.wasShown = wasShown;

		}

		public EventModelBuilder SetIsCompleted(bool isCompleted)
		{
			this.isCompleted = isCompleted;
			return this;
		}

		public EventModelBuilder SetPostponeTime(TimeSpan? postponeTime)
		{
			this.postponeTime = postponeTime;
			return this;
		}

		public EventModelBuilder SetWasShown(bool wasShown)
		{
			this.wasShown = wasShown;
			return this;
		}

		public EventModelBuilder SetQuantityAmount(QuantityModel quantityAmount)
		{
			this.quantityAmount = quantityAmount;
			return this;
		}

		public EventModelBuilder SetMicroTasksList(IEnumerable<MicroTaskModel> microTasksList)
		{
			_microTasksList = microTasksList;
			return this;
		}
		public int? SetNotificationID(int? notificationID)
		{
			this.notificationID = notificationID;
			return notificationID;
		}
		public EventModel Build()
		{
			// Perform any final validation
			return new EventModel(title, description, startTime, endTime, eventType, isCompleted, postponeTime, wasShown, guid, notificationID);
		}
	}
}