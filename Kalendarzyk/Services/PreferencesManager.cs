using Kalendarzyk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk.Services
{
	public static class PreferencesManager
	{
		// Define keys as constants
		public const string MainTypeQuickNoteName = "QuickNote";
		public const string SubTypeQuickNoteName = "QuickNote";
		public const string SelectedLanguageKey = "SelectedLanguage";
		public const string SubEventTypeTimesDifferentKey = "SubEventTypeTimesDifferent";
		public const string MainEventTypeTimesDifferentKey = "MainEventTypeTimesDifferent";
		public const string WeeklyHoursSpanKey = "WeeklyHoursSpan";
		public const string HoursSpanFromKey = "HoursSpanFrom";
		public const string HoursSpanToKey = "HoursSpanTo";
		public const string NotificationID = "NotificationID";

		//public const string IsDeleteAllSelectedKey = "IsDeleteAllSelected";
		//public const string IsCreateDummyDataSelectedKey = "IsCreateDummyDataSelected";



		//public static bool GetIsDeleteAllSelected() => Preferences.Get(IsDeleteAllSelectedKey, false);
		//public static void SetIsDeleteAllSelected(bool value) => Preferences.Set(IsDeleteAllSelectedKey, value);

		//public static bool GetIsCreateDummyDataSelected() => Preferences.Get(IsCreateDummyDataSelectedKey, false);
		//public static void SetIsCreateDummyDataSelected(bool value) => Preferences.Set(IsCreateDummyDataSelectedKey, value);


		public static string GetMainTypeQuickNoteName() => Preferences.Get(MainTypeQuickNoteName, "QNOTE");
		public static string GetSubTypeQuickNoteName() => Preferences.Get(SubTypeQuickNoteName, "QNOTE");

		public static int GetSelectedLanguage() => Preferences.Get(SelectedLanguageKey, (int)Enums.LanguageEnum.English); // it uses int cause Get second parameter has to be simple type
		public static void SetSelectedLanguage(int value) => Preferences.Set(SelectedLanguageKey, value);

		public static bool GetSubEventTypeTimesDifferent() => Preferences.Get(SubEventTypeTimesDifferentKey, false);
		public static void SetSubEventTypeTimesDifferent(bool value) => Preferences.Set(SubEventTypeTimesDifferentKey, value);

		public static bool GetMainEventTypeTimesDifferent() => Preferences.Get(MainEventTypeTimesDifferentKey, false);
		public static void SetMainEventTypeTimesDifferent(bool value) => Preferences.Set(MainEventTypeTimesDifferentKey, value);

		public static bool GetWeeklyHoursSpan() => Preferences.Get(WeeklyHoursSpanKey, true);
		public static void SetWeeklyHoursSpan(bool value) => Preferences.Set(WeeklyHoursSpanKey, value);

		public static int GetHoursSpanFrom() => Preferences.Get(HoursSpanFromKey, 7);
		public static void SetHoursSpanFrom(int value) => Preferences.Set(HoursSpanFromKey, value);

		public static int GetHoursSpanTo() => Preferences.Get(HoursSpanToKey, 18);
		public static void SetHoursSpanTo(int value) => Preferences.Set(HoursSpanToKey, value);

		public static int GetNotificationID() => Preferences.Get(NotificationID, 1);
		public static void SetNotificationID(int value) => Preferences.Set(NotificationID, value);

		public static void ClearAllPreferences()
		{
			Preferences.Clear();
		}
	}
}
