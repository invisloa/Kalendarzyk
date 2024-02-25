using Kalendarzyk.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk.Helpers
{
	public static class NotificationIDGenerator
	{
		public static int GetNextUniqueId()
		{
			int uniqueId = PreferencesManager.GetNotificationID();
			PreferencesManager.SetNotificationID(uniqueId + 1);
			return uniqueId;
		}
	}
}
