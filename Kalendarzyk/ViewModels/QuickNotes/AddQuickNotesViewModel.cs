using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kalendarzyk.Helpers;
using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;
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
		#region Fields and Properties
		private AsyncRelayCommand _submitAsyncQuickNoteCommand;
		private ISubEventTypeModel qNoteSubType;
		public AsyncRelayCommand SubmitAsyncQuickNoteCommand => _submitAsyncQuickNoteCommand;
		private IEventRepository _eventRepository;
		private IGeneralEventModel _currentQuickNote;

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
		private bool _isMeasurementSelected;

		[ObservableProperty]
		private MeasurementSelectorCCViewModel _defaultMeasurementSelectorCCHelper;

		[ObservableProperty]
		[NotifyPropertyChangedFor(nameof(IsNotEditQuickNoteMode))]
		private bool _isEditQuickNoteMode;

		public bool IsNotEditQuickNoteMode => !_isEditQuickNoteMode;
		[ObservableProperty]
		private Color _quickNoteLabelColor=  Colors.Red;

		[ObservableProperty]
		private bool _isQuickNoteMicroTasksType;

		public string SubmitQuickNoteButtonText
		{
			get => IsEditQuickNoteMode ? "Submit changes" : "Add quick note";
		}
		[ObservableProperty]
		private bool _isQuickNotDatesSelected;

		[ObservableProperty]
		private bool _isQuickNoteValueType;

		[ObservableProperty]
		private ObservableCollection<SelectableButtonViewModel> _quickNotesButtonsSelectors;

		private string _quickNoteTitle;
		public string QuickNoteTitle
		{
			get => _quickNoteTitle;
			set
			{
				_quickNoteTitle = value;
				SubmitAsyncQuickNoteCommand.NotifyCanExecuteChanged();
				IsModified = true;
				OnPropertyChanged();
				// Here you might also want to notify changes for the command's CanExecute
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
		public AddQuickNotesViewModel(IEventRepository eventRepository)
        {
			_eventRepository = eventRepository;
			InitializeCommon();
			_defaultMeasurementSelectorCCHelper.QuantityAmount = new QuantityModel(_defaultMeasurementSelectorCCHelper.SelectedMeasurementUnit.TypeOfMeasurementUnit, _defaultMeasurementSelectorCCHelper.QuantityValue);
			_submitAsyncQuickNoteCommand = new AsyncRelayCommand(OnAsyncSubmitQuickNoteCommand, CanSubmitQuickNoteCommand);
		}
		//ctor edit quick note
		public AddQuickNotesViewModel(IEventRepository eventRepository, IGeneralEventModel quickNote)
		{
			_eventRepository = eventRepository;
			_currentQuickNote = quickNote;
			InitializeCommon();
			IsEditQuickNoteMode = true;
			_submitAsyncQuickNoteCommand = new AsyncRelayCommand(OnAsynEditQuickNoteCommand, CanSubmitQuickNoteCommand);
			QuickNoteTitle = quickNote.Title;
			QuickNoteDescription = quickNote.Description;
			StartDateTime = quickNote.StartDateTime;
			EndDateTime = quickNote.EndDateTime;
			IsCompletedCCAdapter.IsCompleted = quickNote.IsCompleted;
			if(quickNote.QuantityAmount != null && quickNote.QuantityAmount.Value != 0)
			{
				OnIsMicroTasksSelectedCommand(QuickNotesButtonsSelectors[1]); // TODO refactor this
				DefaultMeasurementSelectorCCHelper.SelectedMeasurementUnit = DefaultMeasurementSelectorCCHelper.MeasurementUnitsOC.Where(x => x.TypeOfMeasurementUnit == quickNote.QuantityAmount.Unit).First();
				DefaultMeasurementSelectorCCHelper.QuantityValue = quickNote.QuantityAmount.Value;
			}
			if(quickNote.MicroTasksList != null && quickNote.MicroTasksList.Count() > 0)
			{
				OnIsMicroTasksSelectedCommand(QuickNotesButtonsSelectors[0]); // TODO refactor this
				MicroTasksCCAdapter.MicroTasksOC = quickNote.MicroTasksList.ToObservableCollection();
			}
			AsyncDeleteSelectedQuckNoteCommand = new AsyncRelayCommand(OnAsyncDeleteSelectedQuckNoteCommand);
			IsModified = false;
		}
		private void SetPropperValueType()
		{
			qNoteSubType = _eventRepository.AllUserEventTypesList.Where(x => x.EventTypeName == "QNOTE").First();
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
			return !string.IsNullOrEmpty(QuickNoteTitle);
		}
		private async Task OnAsyncSubmitQuickNoteCommand()
		{
			if (CanSubmitQuickNoteCommand())
			{ 
			_currentQuickNote = Factory.CreatePropperEvent(QuickNoteTitle, QuickNoteDescription, StartDateTime + StartExactTime, EndDateTime + EndExactTime, qNoteSubType, DefaultMeasurementSelectorCCHelper.QuantityAmount, MicroTasksCCAdapter.MicroTasksOC);

			await _eventRepository.AddEventAsync(_currentQuickNote);
			}
			await Shell.Current.GoToAsync("..");

			// go to all quick notes page
			// await Shell.Current.GoToAsync("//QuickNotesPage");
		}
		private async Task OnAsynEditQuickNoteCommand()
		{
			if (CanSubmitQuickNoteCommand())
			{
				_currentQuickNote.Title = QuickNoteTitle;
				_currentQuickNote.Description = QuickNoteDescription;
				_currentQuickNote.EventType = _eventRepository.AllUserEventTypesList.Where(x => x.EventTypeName == "QNOTE").First();
				_currentQuickNote.StartDateTime = StartDateTime.Date + StartExactTime;
				_currentQuickNote.EndDateTime = EndDateTime.Date + EndExactTime;
				_currentQuickNote.IsCompleted = IsCompletedCCAdapter.IsCompleted;
				_defaultMeasurementSelectorCCHelper.QuantityAmount = new QuantityModel(_defaultMeasurementSelectorCCHelper.SelectedMeasurementUnit.TypeOfMeasurementUnit, _defaultMeasurementSelectorCCHelper.QuantityValue);
				_currentQuickNote.QuantityAmount = _defaultMeasurementSelectorCCHelper.QuantityAmount;
				_currentQuickNote.MicroTasksList = MicroTasksCCAdapter.MicroTasksOC.ToList();
				await _eventRepository.UpdateEventAsync(_currentQuickNote);
			}
			await Shell.Current.GoToAsync("..");
		}
		private void CclearFields()
		{
			_defaultMeasurementSelectorCCHelper.QuantityValue = 0;
			MicroTasksCCAdapter.MicroTasksOC.Clear();
			QuickNoteTitle = "";
			QuickNoteDescription = "";
			IsCompletedCCAdapter.IsCompleted = false;
			IsQuickNoteValueType = false;
			IsQuickNoteMicroTasksType = false;
		}
	}
}
