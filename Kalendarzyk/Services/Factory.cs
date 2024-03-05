using Kalendarzyk.Helpers;
using Kalendarzyk.Models;
using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.Services.EventsSharing;
using Kalendarzyk.Services.Notifier;
using Kalendarzyk.ViewModels;

/* Unmerged change from project 'Kalendarzyk (net8.0-maccatalyst)'
Before:
using Kalendarzyk.Views.CustomControls.CCViewModels.Kalendarzyk.Views.CustomControls.CCHelperClass;
After:
using Kalendarzyk.Views.CustomControls.CCViewModels.Kalendarzyk.Views.CustomControls;
*/

/* Unmerged change from project 'Kalendarzyk (net8.0-android34.0)'
Before:
using Kalendarzyk.Views.CustomControls.CCViewModels.Kalendarzyk.Views.CustomControls.CCHelperClass;
After:
using Kalendarzyk.Views.CustomControls.CCViewModels.Kalendarzyk.Views.CustomControls;
*/
using Kalendarzyk.ViewModels.HelperClass;
using Kalendarzyk.Views.CustomControls.CCInterfaces;
using Kalendarzyk.Views.CustomControls.CCViewModels;
using Kalendarzyk.Views.
/* Unmerged change from project 'Kalendarzyk (net8.0-maccatalyst)'
Before:
using System.Globalization;
using Kalendarzyk.Models;
using Kalendarzyk.Helpers;
After:
using System.CCViewModels;
using Kalendarzyk.Views.CustomControls.CCViewModels.Kalendarzyk.Helpers;
*/

/* Unmerged change from project 'Kalendarzyk (net8.0-android34.0)'
Before:
using System.Globalization;
using Kalendarzyk.Models;
using Kalendarzyk.Helpers;
After:
using System.CCViewModels;
using Kalendarzyk.Views.CustomControls.CCViewModels.Kalendarzyk.Helpers;
*/
CustomControls.CCViewModels.Kalendarzyk.Views.CustomControls.CCHelperClass;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;

namespace Kalendarzyk.Services
{
	public static class Factory
	{
		// Event Repository Singleton Pattern
		private static IEventRepository _eventRepository;

		public static IEventRepository GetEventRepository()
		{
			if (_eventRepository == null)
				_eventRepository = new LocalMachineEventRepository();
			return _eventRepository;
		}

		// Event Repository
		public static ObservableCollection<MeasurementUnitItem> PopulateMeasurementCollection()
		{
			var measurementUnitItems = new ObservableCollection<MeasurementUnitItem>(
			Enum.GetValues(typeof(MeasurementUnit))
			.Cast<MeasurementUnit>()
			.Select(unit => new MeasurementUnitItem(unit)
			{
				DisplayName = unit == MeasurementUnit.Money
					? CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol
					: unit.GetDescription()

			}));
			return measurementUnitItems;
		}
		public static MeasurementSelectorCCViewModel CreateNewMeasurementSelectorCCHelperClass()
		{
			return new MeasurementSelectorCCViewModel();
		}
		public static IMeasurementOperationsHelperClass CreateMeasurementOperationsHelperClass(ObservableCollection<IGeneralEventModel> eventsToCalculateList)
		{
			return new MeasurementOperationsHelperClass(eventsToCalculateList);
		}
		public static IGeneralEventModel CreatePropperEvent(string title, string description, DateTime startTime, DateTime endTime, ISubEventTypeModel eventTypeModel, QuantityModel eventQuantity = null, IEnumerable<MicroTaskModel> microTasks = null, bool isCompleted = false, TimeSpan? postponeTime = null, bool wasShown = false, int? notificationID = null)
		{
			EventModelBuilder builder = new EventModelBuilder(title, description, startTime, endTime, eventTypeModel, isCompleted, postponeTime, wasShown, notificationID);
			if (eventQuantity != null)
				builder.SetQuantityAmount(eventQuantity);
			if (microTasks != null)
				builder.SetMicroTasksList(microTasks);
			return builder.Build();
		}
		public static ISubEventTypeModel CreateNewEventType(IMainEventType mainEventType, string eventTypeName, Color eventTypeColor, TimeSpan defaultEventTime, QuantityModel quantity = null, List<MicroTaskModel> microTasksList = null)
		{
			return new SubEventTypeModel(mainEventType, eventTypeName, eventTypeColor, defaultEventTime, quantity, microTasksList);
		}
		public static ILocalDataEncryptionService CreateNewLocalDataEncryptionService()
		{
			return new AdvancedEncryptionStandardService();
		}

		public static IMainEventTypesCCViewModel CreateNewIMainEventTypeViewModelClass(List<IMainEventType> mainEventTypes)
		{
			return new MainEventTypesSelectorCCViewModel(mainEventTypes);
		}

		public static IFilterDatesCCHelperClass CreateFilterDatesCCHelperClass()
		{
			return new FilterDatesCCViewModel();
		}

		internal static DefaultTimespanCCViewModel CreateNewDefaultEventTimespanCCHelperClass()
		{
			return new DefaultTimespanCCViewModel();
		}

		internal static ISubTypeExtraOptionsViewModel CreateNewSubTypeExtraOptionsHelperClass(bool isEditMode)
		{
			return new SubTypeExtraOptionsViewModel(isEditMode);
		}

		internal static MicroTasksCCAdapterVM CreateNewMicroTasksCCAdapter(List<MicroTaskModel> listToAddMiroTasks)
		{
			return new MicroTasksCCAdapterVM(listToAddMiroTasks);
		}

		internal static IMainTypeVisualModel CreateIMainTypeVisualElement(string selectedIconString, Color backgroundColor, Color textColor)
		{
			return new IconModel(selectedIconString, backgroundColor, textColor);
		}

		internal static IMainEventType CreateNewMainEventType(string mainTypeName, IMainTypeVisualModel iconForMainEventType)
		{
			return new MainEventType(mainTypeName, iconForMainEventType);
		}

		internal static IEventTimeConflictChecker CreateNewEventTimeConflictChecker(List<IGeneralEventModel> allEventsList)
		{
			return new EventTimeConflictChecker(allEventsList);
		}

		internal static IsCompletedCCViewModel CreateNewIsCompletedCCAdapter()
		{
			return new IsCompletedCCViewModel();
		}
		internal static IShareEventsService CreateNewShareEventsService()
		{
			return new ShareEventsJson(GetEventRepository());
		}

		internal static ILocalFilePathService CreateNewLocalFilePathService()
		{
			return new LocalFilePathService();
		}

		internal static IEventJsonSerializer CreateNewEventJsonSerializer()
		{
			return new EventJsonSerializer(CreateNewLocalDataEncryptionService());
		}

		internal static IFileStorageService CreateNewFileStorageService()
		{
			return new FileStorageService();
		}

		internal static IFileSelectorService CreateNewFileSelectorService()
		{
			return new FileSelectorService();
		}

		internal static IUserNotifier CreateNewUserNotifier()
		{
			return new ToastNotifier();
		}

		internal static IColorButtonsSelectorHelperClass CreateNewIColorButtonsHelperClass(
			ObservableCollection<SelectableButtonViewModel> colorButtons = null,
			ICommand selectedButtonCommand = null,
			Color? startingColor = null)
		{
			return new ColorButtonsSelectorViewModel(colorButtons, selectedButtonCommand, startingColor);
		}

		internal static IsNotificationCCViewModel CreateNewIsNotificationHelpercClass() => new IsNotificationCCViewModel();

		internal static INotificationIDGenerator CreateNotificationIDGenerator() => new NotificationIDGenerator();
	}
}
