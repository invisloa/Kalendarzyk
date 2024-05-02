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
		private ISubEventTypeModel eventSubtype;
		public AsyncRelayCommand AsyncSubmitQuickNoteCommand => _asyncSubmitQuickNoteCommand;
		private IEventRepository _eventRepository;
		private IGeneralEventModel _currentQuickNote;
		private IShareEventsService _shareEventsService;
		public bool IsModified;
		[ObservableProperty]
		private AsyncRelayCommand _asyncDeleteSelectedQuckNoteCommand;
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
		private AsyncRelayCommand _asyncShareEventCommand;
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
		[ObservableProperty]
		private ExtraOptionsEventsHelperClass _extraOptionsSelectorHelperClass = Factory.CreateNewExtraOptionsEventHelperClass();
		#endregion

		//ctor new quick note
		public AddQuickNotesViewModel()
		{
			_eventRepository = Factory.GetEventRepository();
			InitializeCommon();
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

			AsyncDeleteSelectedQuckNoteCommand = new AsyncRelayCommand(OnAsyncDeleteSelectedQuckNoteCommand);
			IsModified = false;
		}

		private void InitializeCommon()
		{

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

/*			_currentQuickNote = Factory.CreatePropperEvent(QuickNoteTitle, QuickNoteDescription, StartDateTime + StartExactTime, EndDateTime + EndExactTime, eventSubtype, DefaultMeasurementSelectorCCHelper.QuantityAmount, MicroTasksCCAdapter.MicroTasksOC, _ChangableIconCCAdapter.IsCompleted);

			await _eventRepository.AddEventAsync(_currentQuickNote);
			await Shell.Current.GoToAsync("..");

*/			// go to all quick notes page
			// await Shell.Current.GoToAsync("//QuickNotesPage");
		}
		private async Task AsyncEditQucikNoteAndGoBack()
		{
			await AsyncEditQuickNote();
			await Shell.Current.GoToAsync("..");
		}
		private async Task AsyncEditQuickNote()
		{
			{
/*				_currentQuickNote.Title = QuickNoteTitle;
				_currentQuickNote.Description = QuickNoteDescription;
				_currentQuickNote.EventType = _eventRepository.AllUserEventTypesList.Where(x => x.EventTypeName == PreferencesManager.GetSubTypeQuickNoteName()).First();
				_currentQuickNote.StartDateTime = StartDateTime.Date + StartExactTime;
				_currentQuickNote.EndDateTime = EndDateTime.Date + EndExactTime;
				_currentQuickNote.IsCompleted = _extraOptionsSelectorHelperClass.ChangableIconCCAdapter.IsCompleted;




				_defaultMeasurementSelectorCCHelper.QuantityAmount = new QuantityModel(_defaultMeasurementSelectorCCHelper.SelectedMeasurementUnit.TypeOfMeasurementUnit, _defaultMeasurementSelectorCCHelper.QuantityValue);
				_currentQuickNote.QuantityAmount = _defaultMeasurementSelectorCCHelper.QuantityAmount;
				_currentQuickNote.MicroTasksList = MicroTasksCCAdapter.MicroTasksOC.ToList();




				await _eventRepository.UpdateEventAsync(_currentQuickNote);
*/			}
		}

		private async Task AsyncShareEvent()
		{
			await AsyncEditQuickNote();
			await _shareEventsService.ShareEventAsync(_currentQuickNote);
		}
	}
}
