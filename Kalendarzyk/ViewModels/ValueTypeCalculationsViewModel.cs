using Kalendarzyk.ViewModels.EventsViewModels;
using Kalendarzyk.Views.CustomControls.CCInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kalendarzyk.Services;
using CommunityToolkit.Mvvm.Input;
using Kalendarzyk.Models.EventTypesModels;
using System.Collections.ObjectModel;
using Kalendarzyk.ViewModels.HelperClass;
using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Views;
using Kalendarzyk.Services.DataOperations;

namespace Kalendarzyk.ViewModels
{
	public class ValueTypeCalculationsViewModel : PlainBaseAbstractEventViewModel, IFilterDatesCC
	{
		#region IFilterDatesCC implementation
		private IFilterDatesCCHelperClass _filterDatesCCHelper = Factory.CreateFilterDatesCCHelperClass();
		private bool _canExecuteCalculationsCommands()
		{
			bool canExecute = _measurementOperationsHelperClass.CheckIfEventsAreSameType();
			if (!canExecute)
			{
				SetAllCalculationsControlsVisibilityOFF();
			}
			return canExecute;
		}
		// TODO change the below to factory and interface LATER
		private IMeasurementOperationsHelperClass _measurementOperationsHelperClass;

		public string TextFilterDateFrom { get; set; } = "FILTER FROM:";
		public string TextFilterDateTo { get; set; } = "FILTER UP TO:";
		public DateTime FilterDateFrom
		{
			get => _filterDatesCCHelper.FilterDateFrom;
			set
			{
				if (_filterDatesCCHelper.FilterDateFrom == value)
				{
					return;
				}
				_filterDatesCCHelper.FilterDateFrom = value;
				_measurementOperationsHelperClass.DateFrom = value;
				OnPropertyChanged();
				BindDataToScheduleList();   // Datepicker does not support commands, so we have to do it here
			}
		}
		public DateTime FilterDateTo
		{
			get => _filterDatesCCHelper.FilterDateTo;
			set
			{
				if (_filterDatesCCHelper.FilterDateTo == value)
				{
					return;
				}
				_measurementOperationsHelperClass.DateTo = value;
				_filterDatesCCHelper.FilterDateTo = value;
				OnPropertyChanged();
				BindDataToScheduleList();   // Datepicker does not support commands, so we have to do it here
			}
		}

		public override void BindDataToScheduleList()
		{
			ApplyEventsDatesFilter(FilterDateFrom, FilterDateTo);
		}

		private void OnFilterDateFromChanged()
		{
			FilterDateFrom = _filterDatesCCHelper.FilterDateFrom;
		}

		private void OnFilterDateToChanged()
		{
			FilterDateTo = _filterDatesCCHelper.FilterDateTo;
		}
		#endregion

		// CONTROLS PROPERTIES
		#region Controls properties
		public string MinByWeekCalculationText { get; set; } = "MIN WEEKS VALUE";
		public string MaxByWeekCalculationText { get; set; } = " WEEKS VALUE";
		private bool _basicOperationsVisibility = false;
		public bool BasicOperationsVisibility
		{
			get { return _basicOperationsVisibility; }
			set
			{
				if (_basicOperationsVisibility != value)
				{
					_basicOperationsVisibility = value;
					OnPropertyChanged();
				}
			}
		}
		//ADVANCED CALCULATIONS VISIBILITY PROPERTIES
		#region AdvancedCalculationsVISIBILITYProperties
		private bool _maxByWeekOperationsVisibility = false;
		public bool MaxByWeekOperationsVisibility
		{
			get { return _maxByWeekOperationsVisibility; }
			set
			{
				if (_maxByWeekOperationsVisibility != value)
				{
					_maxByWeekOperationsVisibility = value;
					OnPropertyChanged();
				}
			}
		}
		private bool _minByWeekOperationsVisibility = false;
		public bool MinByWeekOperationsVisibility
		{
			get { return _minByWeekOperationsVisibility; }
			set
			{
				if (_minByWeekOperationsVisibility != value)
				{
					_minByWeekOperationsVisibility = value;
					OnPropertyChanged();
				}
			}
		}
		#endregion

		private string _totalOfMeasurementsTextAbove = "Total:";
		private string _totalOfMeasurements = "0";
		private string _averageByDayTextAbove = "Average by day:";
		private string _averageByDayMeasurements = "0";
		private string _maxOfMeasurementsTextAbove = "Max value:";
		private string _maxOfMeasurements = "0";
		private string _minOfMeasurementsTextAbove = "Min value:";
		private string _minOfMeasurements = "0";
		private MeasurementCalculationsOutcome _measurementCalulationOutcome;
		public MeasurementCalculationsOutcome MeasurementCalculationOutcome
		{
			get { return _measurementCalulationOutcome; }
			set
			{
				if (_measurementCalulationOutcome != value)
				{
					_measurementCalulationOutcome = value;
					OnPropertyChanged();
				}
			}
		}
		public string TotalOfMeasurementsTextAbove
		{
			get { return _totalOfMeasurementsTextAbove; }
			set
			{
				if (_totalOfMeasurementsTextAbove != value)
				{
					_totalOfMeasurementsTextAbove = value;
					OnPropertyChanged();
				}
			}
		}

		public string TotalOfMeasurements
		{
			get { return _totalOfMeasurements; }
			set
			{
				if (_totalOfMeasurements != value)
				{
					_totalOfMeasurements = value;
					OnPropertyChanged();
				}
			}
		}

		public string AverageByDayTextAbove
		{
			get { return _averageByDayTextAbove; }
			set
			{
				if (_averageByDayTextAbove != value)
				{
					_averageByDayTextAbove = value;
					OnPropertyChanged();
				}
			}
		}

		public string AverageByDayMeasurements
		{
			get { return _averageByDayMeasurements; }
			set
			{
				if (_averageByDayMeasurements != value)
				{
					_averageByDayMeasurements = value;
					OnPropertyChanged();
				}
			}
		}

		public string MaxOfMeasurementsTextAbove
		{
			get { return _maxOfMeasurementsTextAbove; }
			set
			{
				if (_maxOfMeasurementsTextAbove != value)
				{
					_maxOfMeasurementsTextAbove = value;
					OnPropertyChanged();
				}
			}
		}

		public string MaxOfMeasurements
		{
			get { return _maxOfMeasurements; }
			set
			{
				if (_maxOfMeasurements != value)
				{
					_maxOfMeasurements = value;
					OnPropertyChanged();
				}
			}
		}

		public string MinOfMeasurementsTextAbove
		{
			get { return _minOfMeasurementsTextAbove; }
			set
			{
				if (_minOfMeasurementsTextAbove != value)
				{
					_minOfMeasurementsTextAbove = value;
					OnPropertyChanged();
				}
			}
		}

		public string MinOfMeasurements
		{
			get { return _minOfMeasurements; }
			set
			{
				if (_minOfMeasurements != value)
				{
					_minOfMeasurements = value;
					OnPropertyChanged();
				}
			}
		}
		#endregion
		public RelayCommand DoBasicCalculationsCommand { get; set; }
		public RelayCommand<DateTime> GoToWeeksPageCommand { get; set; }
		public RelayCommand MaxByWeekCalculationsCommand { get; set; }
		public RelayCommand MinByWeekCalculationsCommand { get; set; }
		protected override void OnUserEventTypeSelected(ISubEventTypeModel eventSubType)
		{
			base.OnUserEventTypeSelected(eventSubType);  // Call to the base class method

			CanExecuteChangedCalculationsCommandsNotifier();
		}


		// CONSTRUCTOR
		public ValueTypeCalculationsViewModel(IEventRepository eventRepository) : base(eventRepository)
		{
			AllSubEventTypesOC = new ObservableCollection<ISubEventTypeModel>(eventRepository.DeepCopySubEventTypesList().Where(x => x.IsValueType).ToList());
			DoBasicCalculationsCommand = new RelayCommand(OnDoBasicCalculationsCommand, _canExecuteCalculationsCommands);
			MaxByWeekCalculationsCommand = new RelayCommand(OnMaxByWeekCalculationsCommand, _canExecuteCalculationsCommands);
			MinByWeekCalculationsCommand = new RelayCommand(OnMinByWeekCalculationsCommand, _canExecuteCalculationsCommands);
			GoToWeeksPageCommand = new RelayCommand<DateTime>(GoToWeeksPageWithSelectedTypes);
			_measurementOperationsHelperClass = Factory.CreateMeasurementOperationsHelperClass(EventsToShowList);
			InitializeCommon();
		}
		private void GoToWeeksPageWithSelectedTypes(DateTime weeksDate)
		{
			SetIsSelectedAccordingToSelectedTypes();
			Application.Current.MainPage.Navigation.PushAsync(new WeeklyEventsPage(weeksDate));
		}
		private void SetIsSelectedAccordingToSelectedTypes()
		{
			// Extract the list of selected event types.
			var selectedEventTypes = AllSubEventTypesOC.Where(x => x.IsSelectedToFilter).ToList();

			// Create a HashSet for faster lookups.
			HashSet<ISubEventTypeModel> selectedEventTypesSet = new HashSet<ISubEventTypeModel>(selectedEventTypes);

			// Iterate through the AllUserEventTypesList once and set IsSelectedToFilter based on whether it exists in the HashSet.
			foreach (var eventType in _eventRepository.AllUserEventTypesList)
			{
				//								O(1) operation
				bool containsEvent = selectedEventTypesSet.Contains(eventType);
				if (containsEvent)
				{
					eventType.IsSelectedToFilter = true;
					eventType.BackgroundColor = eventType.EventTypeColor;
				}
				else
				{
					eventType.IsSelectedToFilter = false;
					eventType.BackgroundColor = _deselectedUserEventTypeColor;
				}
			}

		}
		private void CanExecuteChangedCalculationsCommandsNotifier()
		{
			DoBasicCalculationsCommand.NotifyCanExecuteChanged();
			MaxByWeekCalculationsCommand.NotifyCanExecuteChanged();
			MinByWeekCalculationsCommand.NotifyCanExecuteChanged();
		}
		private void OnMinByWeekCalculationsCommand()
		{
			SetAllCalculationsControlsVisibilityOFF();
			MinByWeekOperationsVisibility = true;
			MeasurementCalculationOutcome = _measurementOperationsHelperClass.MinByWeekCalculation();
		}
		private void OnMaxByWeekCalculationsCommand()
		{
			SetAllCalculationsControlsVisibilityOFF();
			MaxByWeekOperationsVisibility = true;
			MeasurementCalculationOutcome = _measurementOperationsHelperClass.MaxByWeekCalculation();

		}

		private void OnDoBasicCalculationsCommand()
		{
			SetAllCalculationsControlsVisibilityOFF();
			BasicOperationsVisibility = true;
			_measurementOperationsHelperClass.DoBasicCalculations();
			TotalOfMeasurements = _measurementOperationsHelperClass.TotalOfMeasurements.ToString();
			AverageByDayMeasurements = _measurementOperationsHelperClass.AverageOfMeasurements.ToString("F2");
			MaxOfMeasurements = _measurementOperationsHelperClass.MaxOfMeasurements.ToString();
			MinOfMeasurements = _measurementOperationsHelperClass.MinOfMeasurements.ToString();
		}
		private void SetAllCalculationsControlsVisibilityOFF()
		{
			BasicOperationsVisibility = false;
			MaxByWeekOperationsVisibility = false;
			MinByWeekOperationsVisibility = false;
		}
		private void InitializeCommon()
		{
			_filterDatesCCHelper.FilterDateFromChanged += OnFilterDateFromChanged; // for future controls use like (last 90 days, last 30 days, etc.)
			_filterDatesCCHelper.FilterDateToChanged += OnFilterDateToChanged; // for future controls use like (last 90 days, last 30 days, etc.)

			this.SetFilterDatesValues(); // using extension from IFilterDatesCC method (oldest event date and today)
		}

		internal void OnAppearing()
		{
			AllSubEventTypesOC = new ObservableCollection<ISubEventTypeModel>(AllSubEventTypesOC.Where(x => x.IsValueType).ToList());
		}
	}
}
