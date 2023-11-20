using Kalendarzyk.Models.EventModels;

namespace Kalendarzyk.Helpers
{
	public interface IEventTimeConflictChecker
	{
		public List<IGeneralEventModel> allEvents { get; set; }

		bool IsEventConflict(bool isSubEventTimeDifferent, bool isMainEventTimeDifferent, IGeneralEventModel eventToCheck);
	}
}