using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.Services;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.Views;
using Kalendarzyk.Views.CustomControls.CCViewModels;
using Kalendarzyk.Views.CustomControls.CCInterfaces;
using Kalendarzyk.Views.CustomControls.CCInterfaces.UserTypeExtraOptions;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Kalendarzyk.Helpers;

namespace Kalendarzyk.ViewModels.EventOperations
{
	/// <summary>
	/// Contains only must know data for events
	/// </summary>
	public abstract class EventOperationsBaseViewModel : BaseViewModel, IMainEventTypesCCViewModel
	{
		//MeasurementCC implementation
		#region MeasurementCC implementation
		protected IMeasurementSelectorCC _measurementSelectorHelperClass { get; set; } = Factory.CreateNewMeasurementSelectorCCHelperClass();
		public IMeasurementSelectorCC DefaultMeasurementSelectorCCHelper { get => _measurementSelectorHelperClass; }

		public DefaultTimespanCCViewModel DefaultEventTimespanCCHelper { get; set; } = Factory.CreateNewDefaultEventTimespanCCHelperClass();

		private bool _isValueTypeSelected;
		public bool IsValueTypeSelected
		{
			get => _isValueTypeSelected;
			set
			{
				_isValueTypeSelected = value;
				OnPropertyChanged();

			}
		}
		ObservableCollection<MeasurementUnitItem> _allMeasurementUnitItems;

		public IUserTypeExtraOptionsViewModel UserTypeExtraOptionsHelper { get; set; }


		private bool _isMicroTaskTypeSelected;
		public bool IsMicroTaskTypeSelected
		{
			get => _isMicroTaskTypeSelected;
			set
			{
				if (_isMicroTaskTypeSelected != value)
				{
					_isMicroTaskTypeSelected = value;
					OnPropertyChanged();
				}
			}
		}
		#endregion
		// ctor
		public EventOperationsBaseViewModel(IEventRepository eventRepository)
		{
			_mainEventTypesCCHelper = Factory.CreateNewIMainEventTypeViewModelClass(eventRepository.AllMainEventTypesList);
			UserTypeExtraOptionsHelper = Factory.CreateNewUserTypeExtraOptionsHelperClass(false);
			EventRepository = eventRepository;
			_allUserTypesForVisuals = new List<ISubEventTypeModel>(eventRepository.DeepCopySubEventTypesList());
			AllSubEventTypesOC = new ObservableCollection<ISubEventTypeModel>(eventRepository.DeepCopySubEventTypesList());
			AllEventsListOC = new ObservableCollection<IGeneralEventModel>(EventRepository.AllEventsList);
			MainEventTypeSelectedCommand = new RelayCommand<MainEventTypeViewModel>(OnMainEventTypeSelected);
			SelectUserEventTypeCommand = new RelayCommand<ISubEventTypeModel>(OnUserEventTypeSelected);
			MicroTasksCCAdapter = Factory.CreateNewMicroTasksCCAdapter(microTasksList);
			_allMeasurementUnitItems = Factory.PopulateMeasurementCollection();
			_eventTimeConflictChecker = Factory.CreateNewEventTimeConflictChecker(EventRepository.AllEventsList);
			IsCompletedCCAdapter = Factory.CreateNewIsCompletedCCAdapter();
		}

		//Fields
		#region Fields
		// Language
		private int _fontSize = 20;
		protected string _submitButtonText;
		List<MicroTaskModel> microTasksList = new List<MicroTaskModel>();
		protected IEventTimeConflictChecker _eventTimeConflictChecker;
		private IsCompletedCCViewModel _isCompletedCCAdapter;


		// normal fields
		protected IMainEventTypesCCViewModel _mainEventTypesCCHelper;
		private IEventRepository eventRepository;
		protected IGeneralEventModel _selectedCurrentEvent;
		protected string _title;
		protected string _description;
		protected DateTime _startDateTime = DateTime.Today;
		protected TimeSpan _startExactTime = DateTime.Now.TimeOfDay;
		protected DateTime _endDateTime = DateTime.Today;
		protected TimeSpan _endExactTime = DateTime.Now.TimeOfDay;
		protected AsyncRelayCommand _submitEventCommand;
		protected Color _mainEventTypeBackgroundColor;
		protected List<ISubEventTypeModel> _allUserTypesForVisuals;
		protected ObservableCollection<ISubEventTypeModel> _eventTypesOC;
		protected ObservableCollection<IGeneralEventModel> _allEventsListOC;
		protected ISubEventTypeModel _selectedEventType;
		private RelayCommand _goToAddNewSubTypePageCommand;
		private RelayCommand _goToAddEventPageCommand;
		public event Action<IMainEventType> MainEventTypeChanged;


		#endregion
		//Properties
		#region Properties
		public abstract bool IsEditMode { get; }
		public int FontSize => _fontSize;
		public abstract string SubmitButtonText { get; set; }
		public IsCompletedCCViewModel IsCompletedCCAdapter
		{
			get => _isCompletedCCAdapter;
			set
			{
				_isCompletedCCAdapter = value;
			}
		}
		public string EventTypePickerText { get => "Select event Type"; }
		public RelayCommand GoToAddEventPageCommand
		{
			get
			{
				return _goToAddEventPageCommand ?? (_goToAddEventPageCommand = new RelayCommand(GoToAddEventPage));
			}
		}
		public MicroTasksCCAdapterVM MicroTasksCCAdapter { get; set; }

		public IMainEventType SelectedMainEventType
		{
			get => _mainEventTypesCCHelper.SelectedMainEventType;
			set
			{
				_mainEventTypesCCHelper.SelectedMainEventType = value;
				OnPropertyChanged();
			}
		}

		private void FilterAllSubEventTypesOCByMainEventType(IMainEventType value)
		{
			var tempFilteredEventTypes = FilterUserTypesForVisuals(value);

			AllSubEventTypesOC = new ObservableCollection<ISubEventTypeModel>(tempFilteredEventTypes);
			OnPropertyChanged(nameof(AllSubEventTypesOC));
		}

		private List<ISubEventTypeModel> FilterUserTypesForVisuals(IMainEventType value)
		{
			var x = _allUserTypesForVisuals.FindAll(x => x.MainEventType.Equals(value));
			return x;
		}
		public ObservableCollection<MainEventTypeViewModel> MainEventTypesVisualsOC
		{
			get => _mainEventTypesCCHelper.MainEventTypesVisualsOC;
			set => _mainEventTypesCCHelper.MainEventTypesVisualsOC = value;
		}
		public ObservableCollection<ISubEventTypeModel> AllSubEventTypesOC
		{
			get => _eventTypesOC;
			set
			{
				_eventTypesOC = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<IGeneralEventModel> AllEventsListOC
		{
			get => _allEventsListOC;
			set
			{
				_allEventsListOC = value;
				OnPropertyChanged();
			}
		}
		public bool IsEventTypeSelected
		{
			get => _selectedEventType != null;
		}

		// Basic Event Information
		#region Basic Event Information
		public ISubEventTypeModel SelectedEventType
		{
			get => _selectedEventType;
			set
			{
				if (_selectedEventType == value) return;
				_selectedEventType = value;
				_submitEventCommand.NotifyCanExecuteChanged();
				OnPropertyChanged();
				OnPropertyChanged(nameof(IsEventTypeSelected));
			}
		}
		public string Title
		{
			get => _title;
			set
			{
				_title = value;
				OnPropertyChanged();
				_submitEventCommand.NotifyCanExecuteChanged();
			}
		}
		public string Description
		{
			get => _description;
			set
			{
				_description = value;
				OnPropertyChanged();
			}
		}
		// Start Date/Time
		bool _isChangingEndTimes = false;
		public DateTime StartDateTime
		{
			get => _startDateTime;
			set
			{
				if (_startDateTime == value) return;
				_startDateTime = value;
				OnPropertyChanged();
				if (SelectedEventType != null && !_isChangingEndTimes)
				{
					SetEndExactTimeAccordingToEventType();
				}
				else if (_startDateTime > _endDateTime)
				{
					_endDateTime = _startDateTime;
					OnPropertyChanged(nameof(EndDateTime));
				}
			}
		}
		public DateTime EndDateTime
		{
			get => _endDateTime;
			set
			{
				try
				{
					if (_endDateTime == value) return;
					_isChangingEndTimes = true;
					if (_startDateTime > value)
					{
						_endDateTime = _startDateTime = value;
						OnPropertyChanged(nameof(StartDateTime));
					}
					else
					{
						_endDateTime = value;
					}
					OnPropertyChanged();
					_isChangingEndTimes = false;
				}
				catch
				{
					_endDateTime = _startDateTime;
					OnPropertyChanged(nameof(EndDateTime));
				}
				finally
				{
					_isChangingEndTimes = false;
				}
			}
		}
		public TimeSpan StartExactTime
		{
			get => _startExactTime;
			set
			{
				if (_startExactTime == value) return; // Avoid unnecessary setting and triggering.
				_startExactTime = value;
				OnPropertyChanged();
				if (SelectedEventType != null && !_isChangingEndTimes)
				{
					SetEndExactTimeAccordingToEventType();
				}
				else if (_startDateTime.Date == _endDateTime.Date && _startExactTime > _endExactTime)
				{
					_endExactTime = _startExactTime;
					OnPropertyChanged(nameof(EndExactTime));
				}
			}
		}

		public TimeSpan EndExactTime
		{
			get => _endExactTime;
			set
			{
				try
				{
					if (_endExactTime == value) return; // Avoid unnecessary setting and triggering.
					_isChangingEndTimes = true;
					_endExactTime = value;
					if (_startDateTime.Date == _endDateTime.Date && value < _startExactTime)
					{
						_startExactTime = value;
						OnPropertyChanged(nameof(StartExactTime));
					}
					OnPropertyChanged();
				}
				catch
				{
					_endExactTime = _startExactTime;
					OnPropertyChanged(nameof(EndExactTime));
				}
				finally
				{
					_isChangingEndTimes = false;
				}
			}
		}



		#endregion

		// Command
		public AsyncRelayCommand SubmitEventCommand => _submitEventCommand;
		public RelayCommand<ISubEventTypeModel> SelectUserEventTypeCommand { get; set; }
		public RelayCommand<MainEventTypeViewModel> MainEventTypeSelectedCommand { get; set; }
		public RelayCommand GoToAddNewSubTypePageCommand => _goToAddNewSubTypePageCommand ?? (_goToAddNewSubTypePageCommand = new RelayCommand(GoToAddNewSubTypePage));

		protected IEventRepository EventRepository
		{
			get => eventRepository;
			set => eventRepository = value;
		}
		#endregion


		// Helper Methods
		#region Helper Methods
		private bool IsNumeric(string value)
		{
			return Decimal.TryParse(value, out _);
		}

		private void GoToAddEventPage()
		{
			Application.Current.MainPage.Navigation.PushAsync(new EventPage(EventRepository, DateTime.Today));
		}


		protected void ClearFields()
		{
			Title = "";
			Description = "";
			IsCompletedCCAdapter.IsCompleted = false;
			if (SelectedEventType.IsValueType)
			{
				DefaultMeasurementSelectorCCHelper.QuantityValue = 0;
				OnPropertyChanged(nameof(DefaultMeasurementSelectorCCHelper.QuantityValue));
			}
			// TODO Show POPUP ???
		}
		protected void OnUserEventTypeSelected(ISubEventTypeModel selectedEvent)
		{
			SelectedEventType = selectedEvent;
			UserTypeExtraOptionsHelper.IsMicroTaskTypeSelected = selectedEvent.IsMicroTaskType ? true : false;
			if (UserTypeExtraOptionsHelper.IsMicroTaskTypeSelected)
			{
				MicroTasksCCAdapter.MicroTasksOC = new ObservableCollection<MicroTaskModel>(selectedEvent.MicroTasksList);
			}
			UserTypeExtraOptionsHelper.IsValueTypeSelected = selectedEvent.IsValueType ? true : false;
			if (UserTypeExtraOptionsHelper.IsValueTypeSelected)
			{
				// TODO chcange this so it will look for types in similair families (kg, g, mg, etc...)
				var measurementUnitsForSelectedType = _allMeasurementUnitItems.Where(unit => unit.TypeOfMeasurementUnit == SelectedEventType.DefaultQuantityAmount.Unit); // TO CHECK!
				DefaultMeasurementSelectorCCHelper.QuantityAmount = SelectedEventType.DefaultQuantityAmount;
				DefaultMeasurementSelectorCCHelper.MeasurementUnitsOC = new ObservableCollection<MeasurementUnitItem>(measurementUnitsForSelectedType);
				_measurementSelectorHelperClass.SelectPropperMeasurementData(SelectedEventType);
				OnPropertyChanged(nameof(DefaultMeasurementSelectorCCHelper.MeasurementUnitsOC));
			}
			else
			{

				DefaultMeasurementSelectorCCHelper.QuantityAmount = null;
			}
			SetEndExactTimeAccordingToEventType();
			SetVisualsForSelectedUserType();
		}
		protected void SetVisualsForSelectedUserType()
		{
			foreach (var eventType in AllSubEventTypesOC)       // it sets colors in a different AllSubEventTypesOC then SelectedEventType is...
			{
				eventType.BackgroundColor = Color.FromRgba(255, 255, 255, 1);
				eventType.IsSelectedToFilter = false;
			}
			var SelectedEventType = AllSubEventTypesOC.FirstOrDefault(x => x.Equals(_selectedEventType));
			SelectedEventType.BackgroundColor = SelectedEventType.EventTypeColor;
			SelectedEventType.IsSelectedToFilter = true;
			AllSubEventTypesOC = new ObservableCollection<ISubEventTypeModel>(AllSubEventTypesOC); // ??????
			var maineventtypeviewmodel = MainEventTypesVisualsOC.Where(x => x.MainEventType.Equals(SelectedEventType.MainEventType)).FirstOrDefault();

			_mainEventTypesCCHelper.MainEventTypeSelectedCommand.Execute(maineventtypeviewmodel);


			SelectedMainEventType = SelectedEventType.MainEventType;

			OnPropertyChanged(nameof(MainEventTypesVisualsOC));

			SetSelectedEventTypeControlsVisibility();
		}
		protected virtual void OnMainEventTypeSelected(MainEventTypeViewModel selectedMainEventType)
		{
			if (SelectedMainEventType == null || SelectedMainEventType != selectedMainEventType.MainEventType)
			{
				_mainEventTypesCCHelper.MainEventTypeSelectedCommand.Execute(selectedMainEventType);
				SelectedMainEventType = _mainEventTypesCCHelper.SelectedMainEventType;
				FilterAllSubEventTypesOCByMainEventType(SelectedMainEventType);
			}
			if (AllSubEventTypesOC.Count > 0)
			{
				OnUserEventTypeSelected(AllSubEventTypesOC[0]);
			}
			else
			{
				SelectedEventType = null;
			}
		}
		private void SetSelectedEventTypeControlsVisibility()
		{
			IsValueTypeSelected = SelectedEventType.IsValueType;
			IsMicroTaskTypeSelected = SelectedEventType.IsMicroTaskType;
		}
		private void SetEndExactTimeAccordingToEventType()
		{
			try
			{
				var timeSpanAdded = StartExactTime.Add(SelectedEventType.DefaultEventTimeSpan);

				// Calculate the number of whole days within the TimeSpan
				int days = (int)timeSpanAdded.TotalDays;

				// Calculate the remaining hours, minutes, and seconds after removing whole days
				TimeSpan remainingTime = TimeSpan.FromHours(timeSpanAdded.Hours).Add(TimeSpan.FromMinutes(timeSpanAdded.Minutes)).Add(TimeSpan.FromSeconds(timeSpanAdded.Seconds));

				// Set EndDateTime by adding whole days
				EndDateTime = StartDateTime.AddDays(days);

				// Set EndExactTime to the remaining hours, minutes, and seconds
				EndExactTime = remainingTime;
			}
			catch
			{
				EndExactTime = StartExactTime;
			}
		}


		private void GoToAddNewSubTypePage()
		{
			Application.Current.MainPage.Navigation.PushAsync(new AddNewSubTypePage());
		}

		#endregion
	}
}
