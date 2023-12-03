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

		private IEventRepository _eventRepository;

		private IGeneralEventModel _currentQuickNote;

		[ObservableProperty]
		private MicroTasksCCAdapterVM _microTasksCCAdapter;

		[ObservableProperty]
		private bool _isMeasurementSelected;

		[ObservableProperty]
		private MeasurementSelectorCCViewModel _defaultMeasurementSelectorCCHelper;

		[ObservableProperty]
		private bool _isEditMode;

		[ObservableProperty]
		private Color _quickNoteLabelColor=  Colors.Red;

		[ObservableProperty]
		private bool _isQuickNoteMicroTasksSelected;

		[ObservableProperty]
		private string _submitQuickNoteButtonText = "ADD QUICK NOTE";


		[ObservableProperty]
		private bool _isQuickNotDatesSelected;
		[ObservableProperty]
		private bool _isQuickNoteMeasurementSelected;

		[ObservableProperty]
		[NotifyCanExecuteChangedFor(nameof(SubmitAsyncQuickNoteCommand))]
		private string _quickNoteTitle;

		[ObservableProperty]
		private string _quickNoteDescription;

		[ObservableProperty]
		private DateTime _quickNoteStartDate = DateTime.Now;

		[ObservableProperty]
		private DateTime _quickNoteEndDate = DateTime.Now.AddMinutes(10);

		[ObservableProperty]
		private ObservableCollection<SelectableButtonViewModel> _quickNotesButtonsSelectors;

		[ObservableProperty]
		private ObservableCollection<MicroTaskModel> _quickNoteMicroTasks= new ObservableCollection<MicroTaskModel>();


		//ctor new quick note
		public AddQuickNotesViewModel()
        {
			InitializeCommon();

		}
		//ctor edit quick note
		public AddQuickNotesViewModel(IGeneralEventModel quickNote)
		{
			InitializeCommon();
			IsEditMode = true;
			QuickNoteTitle = quickNote.Title;
			QuickNoteDescription = quickNote.Description;
			QuickNoteLabelColor = quickNote.EventVisibleColor;
			QuickNoteStartDate = quickNote.StartDateTime;
			QuickNoteEndDate = quickNote.EndDateTime;
		}

		private void InitializeCommon()
		{
			_submitAsyncQuickNoteCommand = new AsyncRelayCommand(OnAsyncSubmitQuickNoteCommand, CanSubmitQuickNoteCommand);

			InitializeButtonSelectors();
			MicroTasksCCAdapter = Factory.CreateNewMicroTasksCCAdapter(QuickNoteMicroTasks.ToList());
			DefaultMeasurementSelectorCCHelper = Factory.CreateNewMeasurementSelectorCCHelperClass();
		}

		private void InitializeButtonSelectors()
		{
			QuickNotesButtonsSelectors = new ObservableCollection<SelectableButtonViewModel>
			{
				new SelectableButtonViewModel("Micro Tasks", false, new RelayCommand<SelectableButtonViewModel>(OnIsMicroTasksSelectedCommand)),
				new SelectableButtonViewModel("Value", false, new RelayCommand<SelectableButtonViewModel>(OnIsMeasurementTypeCommand)),
				new SelectableButtonViewModel("DATE", false, new RelayCommand<SelectableButtonViewModel>(OnISDatesControlsCommand)),
			};
			//InitializeIconsTabs();
		}
		private void OnIsMicroTasksSelectedCommand(SelectableButtonViewModel clickedButton)
		{
			IsQuickNoteMicroTasksSelected = !IsQuickNoteMicroTasksSelected;
			SelectableButtonViewModel.MultiButtonSelection(clickedButton);

		}
		private void OnIsMeasurementTypeCommand(SelectableButtonViewModel clickedButton)
		{
			IsQuickNoteMeasurementSelected = !IsQuickNoteMeasurementSelected;
			SelectableButtonViewModel.MultiButtonSelection(clickedButton);

		}
		private void OnISDatesControlsCommand(SelectableButtonViewModel clickedButton)
		{
			_isQuickNotDatesSelected = !_isQuickNotDatesSelected;
			SelectableButtonViewModel.MultiButtonSelection(clickedButton);

		}


		public bool CanSubmitQuickNoteCommand()
		{
			return !string.IsNullOrEmpty(QuickNoteTitle);
		}
		private async Task OnAsyncSubmitQuickNoteCommand()
		{
			var qNoteSubType = _eventRepository.AllUserEventTypesList.Where(x => x.EventTypeName == "QNOTE").First();
			_currentQuickNote = Factory.CreatePropperEvent(QuickNoteTitle, QuickNoteDescription, QuickNoteStartDate, QuickNoteEndDate, qNoteSubType, DefaultMeasurementSelectorCCHelper.QuantityAmount, MicroTasksCCAdapter.MicroTasksOC);
			await _eventRepository.AddEventAsync(_currentQuickNote);

			// go to all quick notes page
			await Shell.Current.GoToAsync("//QuickNotesPage");
		}



	}
}
