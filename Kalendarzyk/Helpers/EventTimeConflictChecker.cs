using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;
/* Unmerged change from project 'Kalendarzyk (net8.0-maccatalyst)'
Before:
using Kalendarzyk.Helpers;
After:
using Kalendarzyk.Models.EventTypesModels;
*/

/* Unmerged change from project 'Kalendarzyk (net8.0-android34.0)'
Before:
using Kalendarzyk.Helpers;
After:
using Kalendarzyk.Models.EventTypesModels;
*/


namespace Kalendarzyk.Helpers
{
	public class EventTimeConflictChecker : IEventTimeConflictChecker
	{

		// foreach for debugging
		public bool IsEventConflict(bool isSubEventTimeDifferent, bool isMainEventTimeDifferent, IGeneralEventModel eventToCheck)
		{
			// Method for sub event type conflict check
			bool hasSubEventTypeConflict(ISubEventTypeModel eventType)
			{
				foreach (var x in allEvents)
				{
					bool isSameEventType = x.EventType.Equals(eventType);
					bool isStartDateTimeConflict = x.StartDateTime < eventToCheck.EndDateTime;
					bool isEndDateTimeConflict = x.EndDateTime > eventToCheck.StartDateTime;

					if (isSameEventType && isStartDateTimeConflict && isEndDateTimeConflict)
					{
						return true;
					}
				}
				return false;
			}

			// Method for main event type conflict check
			bool hasMainEventTypeConflict(IMainEventType mainEventType)
			{
				foreach (var x in allEvents)
				{
					bool isSameMainEventType = x.EventType.MainEventType.Equals(mainEventType);
					bool isStartDateTimeConflict = x.StartDateTime < eventToCheck.EndDateTime;
					bool isEndDateTimeConflict = x.EndDateTime > eventToCheck.StartDateTime;

					if (isSameMainEventType && isStartDateTimeConflict && isEndDateTimeConflict)
					{
						return true;
					}
				}
				return false;
			}


			if (isSubEventTimeDifferent && isMainEventTimeDifferent)
			{
				return hasMainEventTypeConflict(eventToCheck.EventType.MainEventType);
			}
			else if (isMainEventTimeDifferent)
			{
				return hasMainEventTypeConflict(eventToCheck.EventType.MainEventType);
			}
			else if (isSubEventTimeDifferent)
			{
				return hasSubEventTypeConflict(eventToCheck.EventType);
			}

			return false;
		}
		// todo changed for testing
		public List<IGeneralEventModel> allEvents { get; set; }

		public EventTimeConflictChecker(List<IGeneralEventModel> allEventsList)
		{
			this.allEvents = allEventsList;
		}
	}
}

/*		public bool IsEventConflict(bool isSubEventTimeDifferent, bool isMainEventTimeDifferent, IGeneralEventModel eventToCheck)
		{
			// Delegate for sub event type conflict check
			Func<ISubEventTypeModel, bool> hasSubEventTypeConflict = (eventType) =>
				allEvents.Any(x => x.EventType == eventType &&
								   x.StartDateTime < eventToCheck.EndDateTime &&
								   x.EndDateTime > eventToCheck.StartDateTime);

			// Delegate for main event type conflict check
			Func<IMainEventType, bool> hasMainEventTypeConflict = (mainEventType) =>
				allEvents.Any(x => x.EventType.MainEventType == mainEventType &&
								   x.StartDateTime < eventToCheck.EndDateTime &&
								   x.EndDateTime > eventToCheck.StartDateTime);
			// if both are checked it is enough to check only main event type
			if (isSubEventTimeDifferent && isMainEventTimeDifferent)
			{
				return hasMainEventTypeConflict(eventToCheck.EventType.MainEventType);
			}
			else if (isMainEventTimeDifferent)
			{
				return hasMainEventTypeConflict(eventToCheck.EventType.MainEventType);
			}
			else if (isSubEventTimeDifferent)
			{
				return hasSubEventTypeConflict(eventToCheck.EventType);
			}

			return false;
		}*/