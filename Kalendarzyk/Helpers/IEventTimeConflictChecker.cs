using Kalendarzyk.Models.EventModels;
using System.Collections.ObjectModel;

namespace Kalendarzyk.Helpers
{
	public interface IEventTimeConflictChecker
	{
		public ObservableCollection<IGeneralEventModel> allEvents { get; set; }

		bool IsEventConflict(bool isSubEventTimeDifferent, bool isMainEventTimeDifferent, IGeneralEventModel eventToCheck);
	}
}