using Kalendarzyk.Services;

namespace Kalendarzyk.Helpers
{
	public class NotificationIDGenerator : INotificationIDGenerator
	{
		public int GetNextUniqueId()
		{
			int uniqueId = PreferencesManager.GetNotificationID();
			PreferencesManager.SetNotificationID(uniqueId + 1);
			return uniqueId;
		}
	}
}
