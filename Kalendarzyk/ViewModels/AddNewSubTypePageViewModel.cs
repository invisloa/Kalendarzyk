using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using Kalendarzyk.Models.EventTypesModels;
using Newtonsoft.Json;
using Kalendarzyk.Services;
using Kalendarzyk.Services.DataOperations;
using CommunityToolkit.Mvvm.Input;
using Kalendarzyk.Views;
using Kalendarzyk.Views.CustomControls.CCInterfaces;
using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Views.CustomControls.CCViewModels;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Kalendarzyk.Helpers;
using Kalendarzyk.Views.CustomControls.CCInterfaces.UserTypeExtraOptions;
using Kalendarzyk.Views.CustomControls;

namespace Kalendarzyk.ViewModels
{
	public class AddNewSubTypePageViewModel : BaseViewModel
	{
		// TODO ! CHANGE THE BELOW CLASS TO VIEW MODEL 
		public ObservableCollection<MainEventTypeViewModel> MainEventTypesVisualsOC { get => ((IMainEventTypesCCViewModel)_mainEventTypesCCHelper).MainEventTypesVisualsOC; set => ((IMainEventTypesCCViewModel)_mainEventTypesCCHelper).MainEventTypesVisualsOC = value; }
		public DefaultTimespanCCViewModel DefaultEventTimespanCCHelper { get; set; } = Factory.CreateNewDefaultEventTimespanCCHelperClass();
		public MeasurementSelectorCCViewModel DefaultMeasurementSelectorCCHelper { get; set; } = Factory.CreateNewMeasurementSelectorCCHelperClass();
		public MicroTasksCCAdapterVM MicroTasksCCAdapter { get; set; }
		public IUserTypeExtraOptionsViewModel UserTypeExtraOptionsHelper { get; set; }
		#region Fields
		private IMainEventTypesCCViewModel _mainEventTypesCCHelper;

		private TimeSpan _defaultEventTime;
		private ISubEventTypeModel _currentType;   // if null => add new type, else => edit type
		private Color _selectedColor = Color.FromRgb(255, 0, 0); // initialize with red
		private string _typeName;
		private IEventRepository _eventRepository;
		List<MicroTaskModel> microTasksList = new List<MicroTaskModel>();

		#endregion

		#region Properties
		public string QuantityValueText => IsEdit ? "DEFAULT VALUE:" : "Value:";
		public string PageTitle => IsEdit ? "EDIT TYPE" : "ADD NEW TYPE";
		public string PlaceholderText => IsEdit ? $"TYPE NEW NAME FOR: {TypeName}" : "...NEW TYPE NAME...";
		public string SubmitButtonText => IsEdit ? "SUBMIT CHANGES" : "ADD NEW TYPE";
		public bool IsEdit => _currentType != null;
		public bool IsNotEdit => !IsEdit;

		public IMainEventTypesCCViewModel MainEventTypesCCHelper
		{
			get => _mainEventTypesCCHelper;
			set
			{
				_mainEventTypesCCHelper = value;
				OnPropertyChanged();
			}
		}
		public ISubEventTypeModel CurrentType
		{
			get => _currentType;
			set
			{
				if (value == _currentType) return;
				_currentType = value;
				OnPropertyChanged();
			}
		}
		public Color MainEventTypeButtonsColor
		{
			get; set;
		}

		public Color SelectedSubTypeColor
		{
			get => _selectedColor;
			set
			{
				if (value == _selectedColor) return;
				_selectedColor = value;
				OnPropertyChanged();
			}
		}
		public string TypeName
		{
			get => _typeName;
			set
			{
				if (value == _typeName) return;
				_typeName = value;
				SubmitTypeCommand.NotifyCanExecuteChanged();
				OnPropertyChanged();
			}
		}
		public IMainEventType SelectedMainEventType
		{
			get => MainEventTypesCCHelper.SelectedMainEventType;
			set
			{
				MainEventTypesCCHelper.SelectedMainEventType = value;
				SubmitTypeCommand.NotifyCanExecuteChanged();
			}
		}

		public ObservableCollection<SelectableButtonViewModel> ButtonsColorsOC { get; set; }
		#endregion
		#region Commands
		public RelayCommand<MainEventTypeViewModel> MainEventTypeSelectedCommand { get; set; }
		public RelayCommand GoToAllSubTypesPageCommand { get; private set; }
		public RelayCommand<SelectableButtonViewModel> SelectColorCommand { get; private set; }
		public AsyncRelayCommand SubmitTypeCommand { get; private set; }
		public AsyncRelayCommand DeleteSelectedEventTypeCommand { get; private set; }

		#region Commands CanExecute
		private bool CanExecuteSubmitTypeCommand() => !string.IsNullOrEmpty(TypeName) && MainEventTypesCCHelper.SelectedMainEventType != null;
		#endregion
		#endregion

		#region Constructors
		// constructor for create mode
		public AddNewSubTypePageViewModel(IEventRepository eventRepository)
		{
			_eventRepository = eventRepository;
			InitializeCommon(eventRepository);
			MainEventTypeSelectedCommand = MainEventTypesCCHelper.MainEventTypeSelectedCommand;
			DefaultEventTimespanCCHelper.SelectedUnitIndex = 0; // minutes
			DefaultEventTimespanCCHelper.DurationValue = 30;
			MicroTasksCCAdapter = Factory.CreateNewMicroTasksCCAdapter(microTasksList);
		}

		// constructor for edit mode
		public AddNewSubTypePageViewModel(IEventRepository eventRepository, ISubEventTypeModel currentType)
		{
			CurrentType = currentType;
			InitializeCommon(eventRepository);
			if (currentType.IsMicroTaskType)
			{
				MicroTasksCCAdapter = Factory.CreateNewMicroTasksCCAdapter(currentType.MicroTasksList);
			}

			MainEventTypesCCHelper.MainEventTypeSelectedCommand.Execute(new MainEventTypeViewModel(currentType.MainEventType));  // pass some new main event type view model not the one that is on the list!!!

			//MainEventTypesCCHelper.SelectedMainEventType = currentType.MainEventType;
			SelectedSubTypeColor = currentType.EventTypeColor;
			TypeName = currentType.EventTypeName;
			DefaultEventTimespanCCHelper.SetControlsValues(currentType.DefaultEventTimeSpan);
			setIsVisibleForExtraControlsInEditMode();
			UserTypeExtraOptionsHelper.ValueTypeClickCommand = null;
			// set proper visuals for an edited event type ??
		}

		private void InitializeCommon(IEventRepository eventRepository)
		{
			_eventRepository = eventRepository;
			InitializeColorButtons();
			_mainEventTypesCCHelper = Factory.CreateNewIMainEventTypeViewModelClass(_eventRepository.AllMainEventTypesList);
			bool isEditMode = CurrentType != null;
			UserTypeExtraOptionsHelper = Factory.CreateNewUserTypeExtraOptionsHelperClass(isEditMode);
			SelectColorCommand = new RelayCommand<SelectableButtonViewModel>(OnSelectColorCommand);
			GoToAllSubTypesPageCommand = new RelayCommand(GoToAllSubTypesPage);
			SubmitTypeCommand = new AsyncRelayCommand(SubmitType, CanExecuteSubmitTypeCommand);
			DeleteSelectedEventTypeCommand = new AsyncRelayCommand(DeleteSelectedEventType);
			_mainEventTypesCCHelper.MainEventTypeChanged += OnMainEventTypeChanged;

		}

		// for telling the view that the main event type has changed
		private void OnMainEventTypeChanged(IMainEventType newMainEventType)
		{
			SubmitTypeCommand.NotifyCanExecuteChanged();
		}
		private void setIsVisibleForExtraControlsInEditMode()
		{
			UserTypeExtraOptionsHelper.IsValueTypeSelected = CurrentType.IsValueType;
			UserTypeExtraOptionsHelper.IsMicroTaskTypeSelected = CurrentType.IsMicroTaskType;
			UserTypeExtraOptionsHelper.IsDefaultEventTimespanSelected = CurrentType.DefaultEventTimeSpan != TimeSpan.Zero;
		}
		#endregion


		#region Methods
		private async Task DeleteSelectedEventType()
		{
			var eventTypesInDb = _eventRepository.AllEventsList.Where(x => x.EventType.Equals(_currentType));
			if (eventTypesInDb.Any())
			{
				var action = await App.Current.MainPage.DisplayActionSheet("This type is used in some events.", "Cancel", null, "Delete all associated events", "Go to All Events Page");
				switch (action)
				{
					case "Delete all associated events":
						// Perform the operation to delete all events of the event type.
						_eventRepository.AllEventsList.RemoveAll(x => x.EventType.Equals(_currentType.EventTypeName));
						await _eventRepository.SaveEventsListAsync();
						await _eventRepository.DeleteFromSubEventTypesListAsync(_currentType);
						// TODO make a confirmation message
						break;
					case "Go to All Events Page":
						// Redirect to the All Events Page.
						await Shell.Current.GoToAsync("ViewAllEventsPage");
						break;
					default:
						// Cancel was selected or back button was pressed.
						break;
				}
				return;
			}
			await _eventRepository.DeleteFromSubEventTypesListAsync(_currentType);
			await Shell.Current.GoToAsync($"{nameof(AllSubTypesPage)}");
		}

		private async Task SubmitType()
		{
			if(TypeName == "QNOTE")
			{
				var action = await App.Current.MainPage.DisplayActionSheet("Tip... Tiri... Poop... Tim... Tiriri....", "Riri", null, "Tiri", "Go to All Events Page");
				switch (action)
				{
					case "Riri":
						
						break;
					case "Tiri":

						break;
					default:
						break;
				}
				return; 
			}
			if (IsEdit)
			{
				_currentType.MainEventType = SelectedMainEventType;
				_currentType.EventTypeName = TypeName;
				_currentType.EventTypeColor = _selectedColor;
				SetExtraUserControlsValues(_currentType);
				await _eventRepository.UpdateSubEventTypeAsync(_currentType);
				await Shell.Current.GoToAsync("/AllSubTypesPage"); // Absolute route without "///"
			}
			else
			{
				// TODO NOW !!!!!
				var timespan = UserTypeExtraOptionsHelper.IsDefaultEventTimespanSelected ? DefaultEventTimespanCCHelper.GetDefaultDuration() : TimeSpan.Zero;
				var quantityAmount = UserTypeExtraOptionsHelper.IsValueTypeSelected ? DefaultMeasurementSelectorCCHelper.QuantityAmount : null;
				var microTasks = UserTypeExtraOptionsHelper.IsMicroTaskTypeSelected ? new List<MicroTaskModel>(MicroTasksCCAdapter.MicroTasksOC) : null;
				var newUserType = Factory.CreateNewEventType(MainEventTypesCCHelper.SelectedMainEventType, TypeName, _selectedColor, timespan, quantityAmount, microTasks);
				await _eventRepository.AddSubEventTypeAsync(newUserType);
				TypeName = string.Empty;
			}
		}
		private void OnSelectColorCommand(SelectableButtonViewModel selectedColor)
		{
			SelectedSubTypeColor = selectedColor.ButtonColor;

			foreach (var button in ButtonsColorsOC)
			{
				button.IsSelected = button.ButtonColor == selectedColor.ButtonColor;
			}
		}
		private void GoToAllSubTypesPage()
		{
			Application.Current.MainPage.Navigation.PushAsync(new AllSubTypesPage());
		}
		private void InitializeColorButtons() //TODO ! also to extract as a separate custom control
		{
			ButtonsColorsInitializerHelperClass buttonsColorsInitializerHelperClass = new ButtonsColorsInitializerHelperClass();
			ButtonsColorsOC = buttonsColorsInitializerHelperClass.ButtonsColorsOC;
		}
		public void SetExtraUserControlsValues(ISubEventTypeModel _currentType)
		{
			if (_currentType == null)
			{
				return;
			}
			if (UserTypeExtraOptionsHelper.IsDefaultEventTimespanSelected)
			{
				_currentType.DefaultEventTimeSpan = DefaultEventTimespanCCHelper.GetDefaultDuration();
			}
			else
			{
				_currentType.DefaultEventTimeSpan = TimeSpan.Zero;
			}
			if (UserTypeExtraOptionsHelper.IsMicroTaskTypeSelected)
			{
				_currentType.IsMicroTaskType = true;
				_currentType.MicroTasksList = new List<MicroTaskModel>(MicroTasksCCAdapter.MicroTasksOC);
			}
			else
			{
				_currentType.IsMicroTaskType = false;
				_currentType.MicroTasksList = null;
			}
		}
	}
	#endregion

}
