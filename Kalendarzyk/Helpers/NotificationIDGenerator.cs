using Kalendarzyk.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
