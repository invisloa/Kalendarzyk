﻿using CommunityToolkit.Mvvm.Input;
using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.Services;
using Kalendarzyk.Services.EventsSharing;
using Kalendarzyk.Views.CustomControls;
using Kalendarzyk.Views.CustomControls.CCViewModels;
using System.Collections.ObjectModel;

namespace Kalendarzyk.ViewModels.EventOperations
{
	class EventOperationsViewModel : EventOperationsBaseViewModel
	{

		/*	TODO XXX	public bool IsDefaultEventTimespanSelected
		{
			get
			{
				return SubTypeExtraOptionsHelper.IsDefaultEventTimespanSelected;
			}
			set
			{
				SubTypeExtraOptionsHelper.IsDefaultEventTimespanSelected = value;
				OnPropertyChanged();
			}
		}*/
		/*	TODO XXX	public RelayCommand IsDefaultTimespanSelectedCommand
		{
			get
			{
				return SubTypeExtraOptionsHelper.IsDefaultTimespanSelectedCommand;
			}
		}*/

		#region Fields
		private AsyncRelayCommand _asyncDeleteEventCommand;
		private AsyncRelayCommand _asyncShareEventCommand;
		private IsNotificationCCViewModel _isNotificationCCAdapter = Factory.CreateNewIsNotificationHelpercClass();

		#endregion
		#region Properties
		public string PageTitle => IsEditMode ? "Edit Event" : "Add Event";
		public string HeaderText => IsEditMode ? $"EDIT EVENT" : "ADD NEW EVENT";

		public override bool IsEditMode => _selectedCurrentEvent != null;
		private IShareEventsService _shareEventsService;    // made private not tested

		public AsyncRelayCommand AsyncDeleteEventCommand
		{
			get => _asyncDeleteEventCommand;
			set => _asyncDeleteEventCommand = value;
		}

		public AsyncRelayCommand AsyncShareEventCommand
		{
			get => _asyncShareEventCommand;
			set => _asyncShareEventCommand = value;
		}

		public override string SubmitButtonText
		{
			get
			{
				if (IsEditMode)
				{
					return "SUBMIT CHANGES";
				}
				else
				{
					return "ADD NEW EVENT";
				}
			}
			set
			{

				// for now set is not used maybe it will be implemented when Languages will be added
				_submitButtonText = value;
				OnPropertyChanged();
			}
		}
		public string DeleteButtonText
		{
			get
			{
				return "DELETE CURRENT EVENT";
			}
		}

		public IsNotificationCCViewModel IsNotificationCCAdapter
		{
			get => _isNotificationCCAdapter;
			set => _isNotificationCCAdapter = value;
		}
		#endregion


		#region Constructors
		// ctor for creating evnents create event mode
		public EventOperationsViewModel(DateTime selectedDate)
			: base()
		{
			ExtraOptionsHelperToChangeName = Factory.CreateNewExtraOptionsSelectorHelperClass();

			StartDateTime = selectedDate;
			EndDateTime = selectedDate;
			_asyncSubmitEventCommand = new AsyncRelayCommand(AddEventAsync, CanExecuteSubmitCommand);
		}


		// ctor for editing events edit event mode
		public EventOperationsViewModel(IGeneralEventModel eventToEdit)
		: base()
		{
			_asyncSubmitEventCommand = new AsyncRelayCommand(AsyncEditEventAndGoBack, CanExecuteSubmitCommand);
			AsyncDeleteEventCommand = new AsyncRelayCommand(AsyncDeleteSelectedEvent);
			AsyncShareEventCommand = new AsyncRelayCommand(AsyncShareEvent);
			SelectUserEventTypeCommand = null;
			_shareEventsService = Factory.CreateNewShareEventsService();

			// Set properties based on eventToEdit
			ExtraOptionsHelperToChangeName = Factory.CreateNewExtraOptionsSelectorHelperClass(eventToEdit);

			_selectedCurrentEvent = eventToEdit;
			Title = _selectedCurrentEvent.Title;
			Description = _selectedCurrentEvent.Description;
			StartDateTime = _selectedCurrentEvent.StartDateTime.Date;
			EndDateTime = _selectedCurrentEvent.EndDateTime.Date;
			SelectedMainEventType = _selectedCurrentEvent.EventType.MainEventType;
			SelectedEventType = _selectedCurrentEvent.EventType;
			ExtraOptionsHelperToChangeName = Factory.CreateNewExtraOptionsSelectorHelperClass(eventToEdit);

			OnUserEventTypeSelectedCommand(eventToEdit.EventType);
			IsCompletedCCAdapter.IsCompleted = _selectedCurrentEvent.IsCompleted;
			FilterAllSubEventTypesOCByMainEventType(SelectedMainEventType); // CANNOT CHANGE MAIN EVENT TYPE


			MainEventTypeSelectedCommand = null;
			StartExactTime = _selectedCurrentEvent.StartDateTime.TimeOfDay;
			EndExactTime = _selectedCurrentEvent.EndDateTime.TimeOfDay;
		}
		#endregion

		#region Command Execution Methods

		private bool CanExecuteSubmitCommand()
		{
			if (string.IsNullOrWhiteSpace(Title) || SelectedEventType == null)
			{
				return false;
			}
			return true;
		}

		private async Task AddEventAsync()
		{
			_selectedCurrentEvent = Factory.CreatePropperEvent(Title, Description, StartDateTime.Date + StartExactTime, EndDateTime.Date + EndExactTime, SelectedEventType, ExtraOptionsHelperToChangeName.DefaultMeasurementSelectorCCHelper.QuantityAmount ??  null, MicroTasksCCAdapter.MicroTasksOC ?? null); // TODO !!!!!add microtasks


			// TODO In some day check why the lists are becoming different after adding first event
			bool areSameList = ReferenceEquals(EventRepository.AllEventsList, _eventTimeConflictChecker.allEvents);
			// create a new confilict checker to stop not same list issues
			if (!areSameList)
			{
				_eventTimeConflictChecker = Factory.CreateNewEventTimeConflictChecker(EventRepository.AllEventsList);
			}
			// Create a new Event based on the selected EventType
			if (!_eventTimeConflictChecker.IsEventConflict(PreferencesManager.GetSubEventTypeTimesDifferent(), PreferencesManager.GetMainEventTypeTimesDifferent(), _selectedCurrentEvent))
			{
				await EventRepository.AddEventAsync(_selectedCurrentEvent);
			}
			else
			{
				await App.Current.MainPage.DisplayActionSheet("ALREADY AN EVENT\nIN THE SPECIFIED TIME.", "OK", null);
				return;
			}
			ClearFields();
		}

		private async Task AsyncEditEvent()
		{
			_selectedCurrentEvent.Title = Title;
			_selectedCurrentEvent.Description = Description;
			_selectedCurrentEvent.EventType = SelectedEventType;
			_selectedCurrentEvent.StartDateTime = StartDateTime.Date + StartExactTime;
			_selectedCurrentEvent.EndDateTime = EndDateTime.Date + EndExactTime;
			_selectedCurrentEvent.IsCompleted = IsCompletedCCAdapter.IsCompleted;
			_selectedCurrentEvent.MicroTasksList = MicroTasksCCAdapter.MicroTasksOC.ToList();
			ExtraOptionsHelperToChangeName.DefaultMeasurementSelectorCCHelper.QuantityAmount = new QuantityModel(ExtraOptionsHelperToChangeName.DefaultMeasurementSelectorCCHelper.SelectedMeasurementUnit.TypeOfMeasurementUnit, ExtraOptionsHelperToChangeName.DefaultMeasurementSelectorCCHelper.QuantityValue);
			_selectedCurrentEvent.QuantityAmount = ExtraOptionsHelperToChangeName.DefaultMeasurementSelectorCCHelper.QuantityAmount;
			await EventRepository.UpdateEventAsync(_selectedCurrentEvent);
		}
		private async Task AsyncEditEventAndGoBack()
		{
			await AsyncEditEvent();
			await Shell.Current.GoToAsync("..");
		}
		private void FilterAllSubEventTypesOCByMainEventType(IMainEventType value)
		{
			var tempFilteredEventTypes = AllSubEventTypesOC.ToList().FindAll(x => x.MainEventType.Equals(value));
			AllSubEventTypesOC = new ObservableCollection<ISubEventTypeModel>(tempFilteredEventTypes);
			OnPropertyChanged(nameof(AllSubEventTypesOC));
		}

		private async Task AsyncDeleteSelectedEvent()
		{
			var action = await App.Current.MainPage.DisplayActionSheet($"Delete event {_selectedCurrentEvent.Title}", "Cancel", null, "Delete");
			switch (action)
			{
				case "Delete":
					await EventRepository.DeleteFromEventsListAsync(_selectedCurrentEvent);
					await Shell.Current.GoToAsync("..");
					break;
				default:
					// Cancel was selected or back button was pressed.
					break;
			}
			return;

		}

		private async Task AsyncShareEvent()
		{
			await AsyncEditEvent();
			await _shareEventsService.ShareEventAsync(_selectedCurrentEvent);
		}
		#endregion


	}
}


