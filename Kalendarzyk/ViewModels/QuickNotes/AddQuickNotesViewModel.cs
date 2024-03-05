using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.Services;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.Services.EventsSharing;
using Kalendarzyk.Views.CustomControls.CCViewModels;
using System.Collections.ObjectModel;

namespace Kalendarzyk.ViewModels
{
	public partial class AddQuickNotesViewModel : ObservableObject
	{
		#region Fields and Properties
		private AsyncRelayCommand _asyncSubmitQuickNoteCommand;
		private ISubEventTypeModel qNoteSubType;
		public AsyncRelayCommand AsyncSubmitQuickNoteCommand => _asyncSubmitQuickNoteCommand;
		private IEventRepository _eventRepository;
		private IGeneralEventModel _currentQuickNote;

		/* Unmerged change from project 'Kalendarzyk (net8.0-maccatalyst)'
		Before:
				private IShareEventsService _shareEventsService;

				public bool IsModified;
		After:
				private IShareEventsService _shareEventsService;

				public bool IsModified;
		*/

		/* Unmerged change from project 'Kalendarzyk (net8.0-android34.0)'
		Before:
				private IShareEventsService _shareEventsService;

				public bool IsModified;
		After:
				private IShareEventsService _shareEventsService;

				public bool IsModified;
		*/
		private IShareEventsService _shareEventsService;

		public bool IsModified;

		[ObservableProperty]
		private bool _isQuickNoteDateSelected;

		[ObservableProperty]
		private IsCompletedCCViewModel _isCompletedCCAdapter;

		[ObservableProperty]
		private AsyncRelayCommand _asyncDeleteSelectedQuckNoteCommand;

		[ObservableProperty]
		private MicroTasksCCAdapterVM _microTasksCCAdapter;

		[ObservableProperty]
		private MeasurementSelectorCCViewModel _defaultMeasurementSelectorCCHelper;

		[ObservableProperty]
		[NotifyPropertyChangedFor(nameof(IsNotEditQuickNoteMode))]
		private bool _isEditQuickNoteMode;

		public bool IsNotEditQuickNoteMode => !_isEditQuickNoteMode;
		[ObservableProperty]
		private Color _quickNoteLabelColor = Colors.Red;


		public string SubmitQuickNoteButtonText
		{
			get => IsEditQuickNoteMode ? "Submit changes" : "Add quick note";
		}
		[ObservableProperty]
		private bool _isQuickNotDatesSelected;

		[ObservableProperty]
		private ObservableCollection<SelectableButtonViewModel> _quickNotesButtonsSelectors;
		[ObservableProperty]
		private AsyncRelayCommand _asyncShareEventCommand;

		private bool _isQuickNoteMicroTasksType;

		public bool IsQuickNoteMicroTasksType
		{
			get => _isQuickNoteMicroTasksType;
			set
			{
				SetProperty(ref _isQuickNoteMicroTasksType, value);
				IsModified = true;
			}
		}
		private bool _isQuickNoteValueType;
		public bool IsQuickNoteValueType
		{
			get => _isQuickNoteValueType;
			set
			{
				SetProperty(ref _isQuickNoteValueType, value);
				IsModified = true;
			}
		}
		private string _quickNoteTitle;
		public string QuickNoteTitle
		{
			get => _quickNoteTitle;
			set
			{
				_quickNoteTitle = value;
				AsyncSubmitQuickNoteCommand.NotifyCanExecuteChanged();
				IsModified = true;
				CanSubmitQuickNote = !string.IsNullOrEmpty(value);
				OnPropertyChanged();
			}
		}
		// the below is added for converter use
		private bool _canSubmitQuickNote;
		public bool CanSubmitQuickNote
		{
			get => _canSubmitQuickNote;
			set
			{
				_canSubmitQuickNote = value;
				OnPropertyChanged();
			}
		}

		private string _quickNoteDescription;
		public string QuickNoteDescription
		{
			get => _quickNoteDescription;
			set
			{
				SetProperty(ref _quickNoteDescription, value);
				IsModified = true;
			}
		}

		private DateTime _startDateTime = DateTime.Today;
		public DateTime StartDateTime
		{
			get => _startDateTime;
			set
			{
				SetProperty(ref _startDateTime, value);
				IsModified = true;
			}
		}

		private DateTime _endDateTime = DateTime.Today;
		public DateTime EndDateTime
		{
			get => _endDateTime;
			set
			{
				SetProperty(ref _endDateTime, value);
				IsModified = true;
			}
		}

		private TimeSpan _startExactTime = DateTime.Now.TimeOfDay;
		public TimeSpan StartExactTime
		{
			get => _startExactTime;
			set
			{
				SetProperty(ref _startExactTime, value);
				// this one is not tracked due to this being fired when the page is loaded
				//IsUnsavedChange = true;
			}
		}

		private TimeSpan _endExactTime = DateTime.Now.TimeOfDay;
		public TimeSpan EndExactTime
		{
			get => _endExactTime;
			set
			{
				SetProperty(ref _endExactTime, value);
				// this one is not tracked due to this being fired when the page is loaded
				//IsUnsavedChange = true;
			}
		}
		#endregion

		//ctor new quick note
		public AddQuickNotesViewModel()
		{
			_eventRepository = Factory.GetEventRepository();
			InitializeCommon();
			_defaultMeasurementSelectorCCHelper.QuantityAmount = new QuantityModel(_defaultMeasurementSelectorCCHelper.SelectedMeasurementUnit.TypeOfMeasurementUnit, _defaultMeasurementSelectorCCHelper.QuantityValue);
			_asyncSubmitQuickNoteCommand = new AsyncRelayCommand(OnAsyncSubmitQuickNoteCommand, CanSubmitQuickNoteCommand);
		}
		//ctor edit quick note
		public AddQuickNotesViewModel(IGeneralEventModel quickNote)
		{
			_shareEventsService = Factory.CreateNewShareEventsService();
			AsyncShareEventCommand = new AsyncRelayCommand(AsyncShareEvent);
			_eventRepository = Factory.GetEventRepository();
			_currentQuickNote = quickNote;
			InitializeCommon();
			IsEditQuickNoteMode = true;
			_asyncSubmitQuickNoteCommand = new AsyncRelayCommand(AsyncEditQucikNoteAndGoBack, CanSubmitQuickNoteCommand);
			QuickNoteTitle = quickNote.Title;
			QuickNoteDescription = quickNote.Description;
			StartDateTime = quickNote.StartDateTime;
			EndDateTime = quickNote.EndDateTime;
			IsCompletedCCAdapter.IsCompleted = quickNote.IsCompleted;
			if (quickNote.QuantityAmount != null && quickNote.QuantityAmount.Value != 0)
			{
				OnIsMicroTasksSelectedCommand(QuickNotesButtonsSelectors[1]); // TODO refactor this
				DefaultMeasurementSelectorCCHelper.SelectedMeasurementUnit = DefaultMeasurementSelectorCCHelper.MeasurementUnitsOC.Where(x => x.TypeOfMeasurementUnit == quickNote.QuantityAmount.Unit).First();
				DefaultMeasurementSelectorCCHelper.QuantityValue = quickNote.QuantityAmount.Value;
			}
			if (quickNote.MicroTasksList != null && quickNote.MicroTasksList.Count() > 0)
			{
				OnIsMicroTasksSelectedCommand(QuickNotesButtonsSelectors[0]); // TODO refactor this
				MicroTasksCCAdapter.MicroTasksOC = quickNote.MicroTasksList.ToObservableCollection();
			}
			AsyncDeleteSelectedQuckNoteCommand = new AsyncRelayCommand(OnAsyncDeleteSelectedQuckNoteCommand);
			IsModified = false;
		}
		private void SetPropperValueType()
		{
			qNoteSubType = _eventRepository.AllUserEventTypesList.Where(x => x.EventTypeName == PreferencesManager.GetSubTypeQuickNoteName()).First();
			var measurementUnitsForSelectedType = DefaultMeasurementSelectorCCHelper.MeasurementUnitsOC.Where(unit => unit.TypeOfMeasurementUnit == qNoteSubType.DefaultQuantityAmount.Unit); // TO CHECK!
			DefaultMeasurementSelectorCCHelper.QuantityAmount = qNoteSubType.DefaultQuantityAmount;
			DefaultMeasurementSelectorCCHelper.MeasurementUnitsOC = new ObservableCollection<MeasurementUnitItem>(measurementUnitsForSelectedType);
			DefaultMeasurementSelectorCCHelper.SelectedMeasurementUnit = measurementUnitsForSelectedType.FirstOrDefault(mu => mu.TypeOfMeasurementUnit == qNoteSubType.DefaultQuantityAmount.Unit);
			OnPropertyChanged(nameof(DefaultMeasurementSelectorCCHelper.MeasurementUnitsOC));

		}
		private void InitializeCommon()
		{
			_isCompletedCCAdapter = Factory.CreateNewIsCompletedCCAdapter();
			MicroTasksCCAdapter = Factory.CreateNewMicroTasksCCAdapter(new List<MicroTaskModel>());
			DefaultMeasurementSelectorCCHelper = Factory.CreateNewMeasurementSelectorCCHelperClass();

			SetPropperValueType();
			InitializeButtonSelectors();

		}

		private void InitializeButtonSelectors()
		{
			QuickNotesButtonsSelectors = new ObservableCollection<SelectableButtonViewModel>
			{
				new SelectableButtonViewModel("Micro Tasks", false, new RelayCommand<SelectableButtonViewModel>(OnIsMicroTasksSelectedCommand)),
				new SelectableButtonViewModel("Value", false, new RelayCommand<SelectableButtonViewModel>(OnIsQuickNoteValueTypeCommand)),
				new SelectableButtonViewModel("DATE", false, new RelayCommand<SelectableButtonViewModel>(OnIsDateControlsSelectedCommand)),
			};
			//InitializeIconsTabs();
		}
		private void OnIsMicroTasksSelectedCommand(SelectableButtonViewModel clickedButton)
		{
			IsQuickNoteMicroTasksType = !IsQuickNoteMicroTasksType;
			SelectableButtonViewModel.MultiButtonSelection(clickedButton);

		}
		private void OnIsQuickNoteValueTypeCommand(SelectableButtonViewModel clickedButton)
		{
			IsQuickNoteValueType = !IsQuickNoteValueType;
			SelectableButtonViewModel.MultiButtonSelection(clickedButton);

		}
		private void OnIsDateControlsSelectedCommand(SelectableButtonViewModel clickedButton)
		{
			IsQuickNoteDateSelected = !IsQuickNoteDateSelected;
			SelectableButtonViewModel.MultiButtonSelection(clickedButton);
		}

		private async Task OnAsyncDeleteSelectedQuckNoteCommand()
		{
			await _eventRepository.DeleteFromEventsListAsync(_currentQuickNote);
			await Shell.Current.GoToAsync("..");
		}
		public bool CanSubmitQuickNoteCommand()
		{
			return CanSubmitQuickNote;
		}
		private async Task OnAsyncSubmitQuickNoteCommand()
		{

			_currentQuickNote = Factory.CreatePropperEvent(QuickNoteTitle, QuickNoteDescription, StartDateTime + StartExactTime, EndDateTime + EndExactTime, qNoteSubType, DefaultMeasurementSelectorCCHelper.QuantityAmount, MicroTasksCCAdapter.MicroTasksOC, _isCompletedCCAdapter.IsCompleted);

			await _eventRepository.AddEventAsync(_currentQuickNote);
			await Shell.Current.GoToAsync("..");

			// go to all quick notes page
			// await Shell.Current.GoToAsync("//QuickNotesPage");
		}
		private async Task AsyncEditQucikNoteAndGoBack()
		{
			await AsyncEditQuickNote();
			await Shell.Current.GoToAsync("..");
		}
		private async Task AsyncEditQuickNote()
		{
			// ?? if (CanSubmitQuickNoteCommand())
			{
				_currentQuickNote.Title = QuickNoteTitle;
				_currentQuickNote.Description = QuickNoteDescription;
				_currentQuickNote.EventType = _eventRepository.AllUserEventTypesList.Where(x => x.EventTypeName == PreferencesManager.GetSubTypeQuickNoteName()).First();
				_currentQuickNote.StartDateTime = StartDateTime.Date + StartExactTime;
				_currentQuickNote.EndDateTime = EndDateTime.Date + EndExactTime;
				_currentQuickNote.IsCompleted = IsCompletedCCAdapter.IsCompleted;
				_defaultMeasurementSelectorCCHelper.QuantityAmount = new QuantityModel(_defaultMeasurementSelectorCCHelper.SelectedMeasurementUnit.TypeOfMeasurementUnit, _defaultMeasurementSelectorCCHelper.QuantityValue);
				_currentQuickNote.QuantityAmount = _defaultMeasurementSelectorCCHelper.QuantityAmount;
				_currentQuickNote.MicroTasksList = MicroTasksCCAdapter.MicroTasksOC.ToList();
				await _eventRepository.UpdateEventAsync(_currentQuickNote);
			}
		}
		//private void ClearFields()
		//{
		//	_defaultMeasurementSelectorCCHelper.QuantityValue = 0;
		//	MicroTasksCCAdapter.MicroTasksOC.Clear();
		//	QuickNoteTitle = "";
		//	QuickNoteDescription = "";
		//	IsCompletedCCAdapter.IsCompleted = false;
		//	IsQuickNoteValueType = false;
		//	IsQuickNoteMicroTasksType = false;
		//}
		private async Task AsyncShareEvent()
		{
			await AsyncEditQuickNote();
			await _shareEventsService.ShareEventAsync(_currentQuickNote);
		}
	}
}
