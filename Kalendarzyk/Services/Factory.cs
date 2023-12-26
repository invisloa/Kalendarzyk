using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.ViewModels.HelperClass;
using Kalendarzyk.Views.CustomControls;
using Kalendarzyk.Views.CustomControls.CCViewModels;
using Kalendarzyk.Views.CustomControls.CCViewModels.Kalendarzyk.Views.CustomControls.CCHelperClass;
using Kalendarzyk.Views.CustomControls.CCInterfaces;
using Kalendarzyk.Views.CustomControls.CCInterfaces.UserTypeExtraOptions;
using System.Collections.ObjectModel;
using System.Globalization;
using Kalendarzyk.Models;
using Kalendarzyk.Helpers;

namespace Kalendarzyk.Services
{
	public static class Factory
	{

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
		public static IGeneralEventModel CreatePropperEvent(string title, string description, DateTime startTime, DateTime endTime, ISubEventTypeModel eventTypeModel, QuantityModel eventQuantity = null, IEnumerable<MicroTaskModel> microTasks = null, bool isCompleted = false, TimeSpan? postponeTime = null, bool wasShown = false)
		{
			EventModelBuilder builder = new EventModelBuilder(title, description, startTime, endTime, eventTypeModel, isCompleted, postponeTime, wasShown);
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

		internal static IUserTypeExtraOptionsViewModel CreateNewUserTypeExtraOptionsHelperClass(bool isEditMode)
		{
			return new UserTypeExtraOptionsViewModel(isEditMode);
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
	}
}
