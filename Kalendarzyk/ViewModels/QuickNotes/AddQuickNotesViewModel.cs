using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kalendarzyk.Helpers;
using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Services;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.Views.CustomControls.CCViewModels;
using Microsoft.Maui.Graphics.Text;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk.ViewModels
{
	public partial class AddQuickNotesViewModel : ObservableObject
	{
		private AsyncRelayCommand _submitAsyncQuickNoteCommand;
		public AsyncRelayCommand SubmitAsyncQuickNoteCommand => _submitAsyncQuickNoteCommand;
		private IGeneralEventModel _editedQuickNote;

		private IEventRepository _eventRepository;

		private IGeneralEventModel _currentQuickNote;
		[ObservableProperty]
		private bool _isQuickNoteDateSelected;

		[ObservableProperty]
		private IsCompletedCCViewModel _isCompletedCCAdapter;

		[ObservableProperty]
		private AsyncRelayCommand _asyncDeleteSelectedQuckNoteCommand;

		[ObservableProperty]
		private MicroTasksCCAdapterVM _microTasksCCAdapter;

		[ObservableProperty]
		private bool _isMeasurementSelected;

		[ObservableProperty]
		private MeasurementSelectorCCViewModel _defaultMeasurementSelectorCCHelper;

		[ObservableProperty]
		private bool _isEditQuickNoteMode;

		[ObservableProperty]
		private Color _quickNoteLabelColor=  Colors.Red;

		[ObservableProperty]
		private bool _isQuickNoteMicroTasksType;

		[ObservableProperty]
		private string _submitQuickNoteButtonText = "ADD QUICK NOTE";


		[ObservableProperty]
		private bool _isQuickNotDatesSelected;
		[ObservableProperty]
		private bool _isQuickNoteValueType;

		[ObservableProperty]
		[NotifyCanExecuteChangedFor(nameof(SubmitAsyncQuickNoteCommand))]
		private string _quickNoteTitle;

		[ObservableProperty]
		private string _quickNoteDescription;

		[ObservableProperty]
		private DateTime _startDateTime = DateTime.Today;
		[ObservableProperty]
		private DateTime _endDateTime = DateTime.Today;
		[ObservableProperty]
		private TimeSpan _startExactTime = DateTime.Now.TimeOfDay;
		[ObservableProperty]
		private TimeSpan _endExactTime = DateTime.Now.TimeOfDay;

		[ObservableProperty]
		private DateTime _quickNoteEndDate = DateTime.Now.AddMinutes(10);

		[ObservableProperty]
		private ObservableCollection<SelectableButtonViewModel> _quickNotesButtonsSelectors;

		[ObservableProperty]
		private ObservableCollection<MicroTaskModel> _quickNoteMicroTasks= new ObservableCollection<MicroTaskModel>();


		//ctor new quick note
		public AddQuickNotesViewModel(IEventRepository eventRepository)
        {
			_eventRepository = eventRepository;
			InitializeCommon();
			_submitAsyncQuickNoteCommand = new AsyncRelayCommand(OnAsyncSubmitQuickNoteCommand, CanSubmitQuickNoteCommand);


		}
		//ctor edit quick note
		public AddQuickNotesViewModel(IEventRepository eventRepository, IGeneralEventModel quickNote)
		{
			_eventRepository = eventRepository;
			_editedQuickNote = quickNote;
			InitializeCommon();
			IsEditQuickNoteMode = true;
			QuickNoteTitle = quickNote.Title;
			QuickNoteDescription = quickNote.Description;
			StartDateTime = quickNote.StartDateTime;
			EndDateTime = quickNote.EndDateTime;
			IsCompletedCCAdapter.IsCompleted = quickNote.IsCompleted;
			if(quickNote.QuantityAmount != null)
			{
				IsQuickNoteValueType = true;
				DefaultMeasurementSelectorCCHelper.SelectedMeasurementUnit = DefaultMeasurementSelectorCCHelper.MeasurementUnitsOC.Where(x => x.TypeOfMeasurementUnit == quickNote.QuantityAmount.Unit).First();
				DefaultMeasurementSelectorCCHelper.QuantityValue = quickNote.QuantityAmount.Value;
			}
			if(quickNote.MicroTasksList != null)
			{
				IsQuickNoteMicroTasksType = true;
				QuickNoteMicroTasks = new ObservableCollection<MicroTaskModel>(quickNote.MicroTasksList);
			}
			_submitAsyncQuickNoteCommand = new AsyncRelayCommand(OnAsynEditQuickNoteCommand, CanSubmitQuickNoteCommand);
			AsyncDeleteSelectedQuckNoteCommand = new AsyncRelayCommand(OnAsyncDeleteSelectedQuckNoteCommand);

		}

		private void InitializeCommon()
		{

			InitializeButtonSelectors();
			_isCompletedCCAdapter= Factory.CreateNewIsCompletedCCAdapter();
			MicroTasksCCAdapter = Factory.CreateNewMicroTasksCCAdapter(QuickNoteMicroTasks.ToList());
			DefaultMeasurementSelectorCCHelper = Factory.CreateNewMeasurementSelectorCCHelperClass();
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
		}
		public bool CanSubmitQuickNoteCommand()
		{
			return !string.IsNullOrEmpty(QuickNoteTitle);
		}
		private async Task OnAsyncSubmitQuickNoteCommand()
		{
			var qNoteSubType = _eventRepository.AllUserEventTypesList.Where(x => x.EventTypeName == "QNote").First();
			SetValueAndMicroTasks();
			_currentQuickNote = Factory.CreatePropperEvent(QuickNoteTitle, QuickNoteDescription, StartDateTime + StartExactTime, EndDateTime + EndExactTime, qNoteSubType, DefaultMeasurementSelectorCCHelper.QuantityAmount, MicroTasksCCAdapter.MicroTasksOC);
			
			await _eventRepository.AddEventAsync(_currentQuickNote);

			// go to all quick notes page
			// await Shell.Current.GoToAsync("//QuickNotesPage");
		}
		private async Task OnAsynEditQuickNoteCommand()
		{
			_currentQuickNote.Title = QuickNoteTitle;
			_currentQuickNote.Description = QuickNoteDescription;
			_currentQuickNote.EventType = _eventRepository.AllUserEventTypesList.Where(x => x.EventTypeName == "QNote").First(); 
			_currentQuickNote.StartDateTime = StartDateTime.Date + StartExactTime;
			_currentQuickNote.EndDateTime = EndDateTime.Date + EndExactTime;
			_currentQuickNote.IsCompleted = IsCompletedCCAdapter.IsCompleted;
			SetValueAndMicroTasks();
			_defaultMeasurementSelectorCCHelper.QuantityAmount = new QuantityModel(_defaultMeasurementSelectorCCHelper.SelectedMeasurementUnit.TypeOfMeasurementUnit, _defaultMeasurementSelectorCCHelper.QuantityValue);
			_currentQuickNote.QuantityAmount = _defaultMeasurementSelectorCCHelper.QuantityAmount;
			await _eventRepository.UpdateEventAsync(_currentQuickNote);
			await Shell.Current.GoToAsync("..");
		}
		private void SetValueAndMicroTasks()
		{
			if (!IsQuickNoteValueType)
			{
				DefaultMeasurementSelectorCCHelper.QuantityAmount = null;
			}
			if (!IsQuickNoteMicroTasksType)
			{
				MicroTasksCCAdapter.MicroTasksOC = null;
			}
		}

	}
}
