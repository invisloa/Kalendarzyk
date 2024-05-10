using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kalendarzyk.Models.EventModels
{
	public class EventModel : AbstractEventModel
	{
		public EventModel(string title, string description, DateTime startTime, DateTime endTime, ISubEventTypeModel EventType, bool isCompleted = false, TimeSpan? postponeTime = null, bool wasShown = false, Guid? guid = null, int? notificationID = null) : base(title, description, startTime, endTime, EventType, isCompleted, postponeTime, wasShown, guid, notificationID)
		{
		}
	}
}
