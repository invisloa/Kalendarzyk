using CommunityToolkit.Mvvm.Input;
using Kalendarzyk.Helpers;
using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.Services;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.Views;
using Kalendarzyk.Views.CustomControls.CCInterfaces;
using Kalendarzyk.Views.CustomControls.CCInterfaces.SubTypeExtraOptions;

using Kalendarzyk.Views.CustomControls.CCViewModels;
using System.Collections.ObjectModel;



namespace Kalendarzyk.ViewModels.EventOperations
{
	/// <summary>
	/// Contains only must know data for events
	/// </summary>
	public abstract class EventOperationsBaseViewModel : BaseViewModel, IMainEventTypesCCViewModel
	{

		private ExtraOptionsSelectorHelperClass extraOptionsSelectorCC;
		public ExtraOptionsSelectorHelperClass ExtraOptionsHelperToChangeName
		{
			get => extraOptionsSelectorCC;
			set => extraOptionsSelectorCC = value;
		}

		private bool _canSubmitEvent;
		public bool CanSubmitEvent      // added since color converter doesnt work with canexecute
		{
			get => _canSubmitEvent;
			set
			{
				_canSubmitEvent = value;
				OnPropertyChanged();
			}
		}
		// ctor
		public EventOperationsBaseViewModel()
		{
			_eventRepository = Factory.GetEventRepository();
			_mainEventTypesCCHelper = Factory.CreateNewIMainEventTypeViewModelClass(_eventRepository.AllMainEventTypesList);
			_allSubTypesForVisuals = new List<ISubEventTypeModel>(_eventRepository.DeepCopySubEventTypesList());
			AllSubEventTypesOC = new ObservableCollection<ISubEventTypeModel>(_eventRepository.DeepCopySubEventTypesList());
			AllEventsListOC = new ObservableCollection<IGeneralEventModel>(_eventRepository.AllEventsList);
			MainEventTypeSelectedCommand = new RelayCommand<MainEventTypeViewModel>(OnMainEventTypeSelected);
			SelectUserEventTypeCommand = new RelayCommand<ISubEventTypeModel>(OnUserEventTypeSelectedCommand);
			_eventTimeConflictChecker = Factory.CreateNewEventTimeConflictChecker(_eventRepository.AllEventsList);
			IsCompletedButton = Factory.CreateNewChangableFontsIconAdapter(false, "check_box", "check_box_outline_blank");
			EventTypesInfoButton = Factory.CreateNewChangableFontsIconAdapter(false, "info", "info_outline");

		}

		//Fields
		#region Fields
		// Language
		private int _fontSize = 20;
		protected string _submitButtonText;
		List<MicroTaskModel> microTasksList = new List<MicroTaskModel>();
		protected IEventTimeConflictChecker _eventTimeConflictChecker;
		private ChangableFontsIconAdapter _eventTypesInfoButton;
		private ChangableFontsIconAdapter _isCompletedButton;


		// normal fields
		protected IMainEventTypesCCViewModel _mainEventTypesCCHelper;
		protected IEventRepository _eventRepository;
		protected IGeneralEventModel _selectedCurrentEvent;
		protected string _title;
		protected string _description;
		protected DateTime _startDateTime = DateTime.Today;
		protected TimeSpan _startExactTime = DateTime.Now.TimeOfDay;
		protected DateTime _endDateTime = DateTime.Today;
		protected TimeSpan _endExactTime = DateTime.Now.TimeOfDay;
		protected AsyncRelayCommand _asyncSubmitEventCommand;
		protected Color _mainEventTypeBackgroundColor;
		protected List<ISubEventTypeModel> _allSubTypesForVisuals;
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
		public ChangableFontsIconAdapter EventTypesInfoButton

		{
			get => _eventTypesInfoButton;
			set
			{
				_eventTypesInfoButton = value;
				OnPropertyChanged();
			}
		}
		public ChangableFontsIconAdapter IsCompletedButton

		{
			get => _isCompletedButton;
			set
			{
				_isCompletedButton = value;
				OnPropertyChanged();
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
			var tempFilteredEventTypes = FilterSubTypesForVisuals(value);

			AllSubEventTypesOC = new ObservableCollection<ISubEventTypeModel>(tempFilteredEventTypes);
			OnPropertyChanged(nameof(AllSubEventTypesOC));
		}

		private List<ISubEventTypeModel> FilterSubTypesForVisuals(IMainEventType value)
		{
			var x = _allSubTypesForVisuals.FindAll(x => x.MainEventType.Equals(value));
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
				_asyncSubmitEventCommand.NotifyCanExecuteChanged();
				OnPropertyChanged();
				OnPropertyChanged(nameof(IsEventTypeSelected));
				CanSubmitEvent = !string.IsNullOrWhiteSpace(_title) && SelectedEventType != null;
				OnPropertyChanged(nameof(CanSubmitEvent));
			}
		}
		public string Title
		{
			get => _title;
			set
			{
				_title = value;
				OnPropertyChanged();
				CanSubmitEvent = !string.IsNullOrWhiteSpace(_title) && SelectedEventType != null;
				OnPropertyChanged(nameof(CanSubmitEvent));
				_asyncSubmitEventCommand.NotifyCanExecuteChanged();
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

		// setting logic is in the setter because TimePicker and DatePicker doesnt support commands
		public DateTime StartDateTime
		{
			get => _startDateTime;
			set
			{
				if (_startDateTime == value) return;
				_startDateTime = value;
				OnPropertyChanged();
				if (!IsEditMode)
				{
					SetEndExactTimeAccordingToEventType();
				}
				if (_startDateTime > _endDateTime)
				{
					_endDateTime = _startDateTime;
					OnPropertyChanged(nameof(EndDateTime));
				}
			}
		}
		// setting logic is in the setter because TimePicker and DatePicker doesnt support commands
		public DateTime EndDateTime
		{
			get => _endDateTime;
			set
			{
				try
				{
					if (_endDateTime == value) return;
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
				}
				catch
				{
					_endDateTime = _startDateTime;
					OnPropertyChanged(nameof(EndDateTime));
				}

			}
		}
		// setting logic is in the setter because TimePicker and DatePicker doesnt support commands
		public TimeSpan StartExactTime
		{
			get => _startExactTime;
			set
			{
				if (_startExactTime == value) return; // Avoid unnecessary setting and triggering.
				_startExactTime = value;
				OnPropertyChanged();
				if (!IsEditMode)
				{
					SetEndExactTimeAccordingToEventType();
				}
				if (_startDateTime.Date == _endDateTime.Date && _startExactTime > _endExactTime)
				{
					_endExactTime = _startExactTime;
					OnPropertyChanged(nameof(EndExactTime));
				}
			}
		}

		// setting logic is in the setter because TimePicker and DatePicker doesnt support commands
		public TimeSpan EndExactTime
		{
			get => _endExactTime;
			set
			{
				try
				{
					if (_endExactTime == value) return; // Avoid unnecessary setting and triggering.
					_endExactTime = value;
					var startDate = StartDateTime;
					var endDate = EndDateTime;
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

			}
		}



		#endregion

		// Command
		public AsyncRelayCommand AsyncSubmitEventCommand => _asyncSubmitEventCommand;
		public RelayCommand<ISubEventTypeModel> SelectUserEventTypeCommand { get; set; }
		public RelayCommand<MainEventTypeViewModel> MainEventTypeSelectedCommand { get; set; }
		public RelayCommand GoToAddNewSubTypePageCommand => _goToAddNewSubTypePageCommand ?? (_goToAddNewSubTypePageCommand = new RelayCommand(GoToAddNewSubTypePage));

		protected IEventRepository EventRepository
		{
			get => _eventRepository;
			set => _eventRepository = value;
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
			Application.Current.MainPage.Navigation.PushAsync(new EventPage(DateTime.Today));
		}


		protected void ClearFields()
		{
			Title = "";
			Description = "";
			IsCompletedButton.IsSelected = false;
			// TODO Show POPUP ???
		}
		protected void SetVisualsForSelectedSubType()
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
				OnUserEventTypeSelectedCommand(AllSubEventTypesOC[0]);
			}
			else
			{
				SelectedEventType = null;
			}
		}

		private void SetEndExactTimeAccordingToEventType()
		{
			try
			{
				/*	No idea why thi is commented??
				 *	
				 *	var timeSpanAdded = StartExactTime.Add(SelectedEventType.DefaultEventTimeSpan);

								// Calculate the number of whole days within the TimeSpan
								int days = (int)timeSpanAdded.TotalDays;

								// Calculate the remaining hours, minutes, and seconds after removing whole days
								TimeSpan remainingTime = TimeSpan.FromHours(timeSpanAdded.Hours).Add(TimeSpan.FromMinutes(timeSpanAdded.Minutes)).Add(TimeSpan.FromSeconds(timeSpanAdded.Seconds));

								// Set EndDateTime by adding whole days
								EndDateTime = StartDateTime.AddDays(days);

								// Set EndExactTime to the remaining hours, minutes, and seconds
								EndExactTime = remainingTime;*/
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
		protected void OnUserEventTypeSelectedCommand(ISubEventTypeModel selectedEvent)
		{
			SelectedEventType = selectedEvent;
			if (!IsEditMode)
			{
                ExtraOptionsHelperToChangeName.OnEventTypeChanged(selectedEvent);

                SetEndExactTimeAccordingToEventType();
			}
			SetVisualsForSelectedSubType();


		}

	}
}
