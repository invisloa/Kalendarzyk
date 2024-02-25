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
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Kalendarzyk.ViewModels
{
	public class AddNewSubTypePageViewModel : BaseViewModel
	{
		// TODO ! CHANGE THE BELOW CLASS TO VIEW MODEL 
		public ObservableCollection<MainEventTypeViewModel> MainEventTypesVisualsOC { get => ((IMainEventTypesCCViewModel)_mainEventTypesCCHelper).MainEventTypesVisualsOC; set => ((IMainEventTypesCCViewModel)_mainEventTypesCCHelper).MainEventTypesVisualsOC = value; }
		public DefaultTimespanCCViewModel DefaultEventTimespanCCHelper { get; set; } = Factory.CreateNewDefaultEventTimespanCCHelperClass();
		public MeasurementSelectorCCViewModel DefaultMeasurementSelectorCCHelper { get; set; } = Factory.CreateNewMeasurementSelectorCCHelperClass();
		public MicroTasksCCAdapterVM MicroTasksCCAdapter { get; set; }
		public ISubTypeExtraOptionsViewModel SubTypeExtraOptionsHelper { get; set; }
		#region Fields
		private IMainEventTypesCCViewModel _mainEventTypesCCHelper;
		private ISubEventTypeModel _currentType;   // if null => add new type, else => edit type
		private string _typeName;
		private IEventRepository _eventRepository;
		List<MicroTaskModel> microTasksList = new List<MicroTaskModel>();
		private IColorButtonsSelectorHelperClass _colorButtonsHelperClass = Factory.CreateNewIColorButtonsHelperClass(startingColor:Colors.Red);
		public IColorButtonsSelectorHelperClass ColorButtonsHelperClass { get => _colorButtonsHelperClass; }

		#endregion

		#region Properties
		public bool IsQuickNoteType
		{
			get => CurrentType?.EventTypeName == PreferencesManager.GetSubTypeQuickNoteName();
		}
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
		public string TypeName
		{
			get => _typeName;
			set
			{
				if (value == _typeName) return;
				_typeName = value;
				AsyncSubmitTypeCommand.NotifyCanExecuteChanged();
				SetCanSubmitTypeCommand();
				OnPropertyChanged();
			}
		}
		public IMainEventType SelectedMainEventType
		{
			get => MainEventTypesCCHelper.SelectedMainEventType;
		}

		public ObservableCollection<SelectableButtonViewModel> ButtonsColorsOC { get; set; }
		#endregion
		#region Commands
		public RelayCommand<MainEventTypeViewModel> MainEventTypeSelectedCommand { get; set; }
		//public RelayCommand GoToAllSubTypesPageCommand { get; private set; }
		public RelayCommand<SelectableButtonViewModel> SelectColorCommand { get; private set; }
		public AsyncRelayCommand AsyncSubmitTypeCommand { get; private set; }
		public AsyncRelayCommand AsyncDeleteSelectedEventTypeCommand { get; private set; }
		private bool _canSubmitTypeCommand;
		private bool _isMainEventTypeSelected;

		public bool CanSubmitTypeCommand
		{
			get => _canSubmitTypeCommand;
			set
			{
				_canSubmitTypeCommand = value;
				OnPropertyChanged();
			}
		}


		#region Commands CanExecute
		public bool CanExecuteAsyncSubmitTypeCommand() => !string.IsNullOrEmpty(TypeName) && MainEventTypesCCHelper.SelectedMainEventType != null;
		#endregion
		#endregion

		#region Constructors
		// constructor for create mode
		public AddNewSubTypePageViewModel()
		{
			_eventRepository = Factory.CreateNewEventRepository();
			InitializeCommon();
			MainEventTypeSelectedCommand = new RelayCommand<MainEventTypeViewModel>(OnMainEventTypeSelectedCommand);
			DefaultEventTimespanCCHelper.SelectedUnitIndex = 0; // minutes
			DefaultEventTimespanCCHelper.DurationValue = 30;
			MicroTasksCCAdapter = Factory.CreateNewMicroTasksCCAdapter(microTasksList);
		}

		// constructor for edit mode
		public AddNewSubTypePageViewModel(ISubEventTypeModel currentType)
		{
			_eventRepository = Factory.CreateNewEventRepository();
			CurrentType = currentType;
			InitializeCommon();
			if (currentType.IsMicroTaskType)
			{
				MicroTasksCCAdapter = Factory.CreateNewMicroTasksCCAdapter(currentType.MicroTasksList);
			}
			OnMainEventTypeSelectedCommand(new MainEventTypeViewModel(currentType.MainEventType));  // pass some new main event type view model not the one that is on the list!!!
			//MainEventTypesCCHelper.SelectedMainEventType = currentType.MainEventType;
			ColorButtonsHelperClass.SelectedColor = currentType.EventTypeColor;
			TypeName = currentType.EventTypeName;
			DefaultEventTimespanCCHelper.SetControlsValues(currentType.DefaultEventTimeSpan);
			setIsVisibleForExtraControlsInEditMode();
			SubTypeExtraOptionsHelper.ValueTypeClickCommand = null;
			AsyncDeleteSelectedEventTypeCommand = new AsyncRelayCommand(AsyncDeleteSelectedEventType, CanDeleteSelectedEventType);

			// set proper visuals for an edited event type ??
		}

		private void InitializeCommon()
		{
			InitializeColorButtons();
			_mainEventTypesCCHelper = Factory.CreateNewIMainEventTypeViewModelClass(_eventRepository.AllMainEventTypesList);
			bool isEditMode = CurrentType != null;
			SubTypeExtraOptionsHelper = Factory.CreateNewSubTypeExtraOptionsHelperClass(isEditMode);
			//GoToAllSubTypesPageCommand = new RelayCommand(GoToAllSubTypesPage);
			AsyncSubmitTypeCommand = new AsyncRelayCommand(AsyncSubmitType, CanExecuteAsyncSubmitTypeCommand);
			_mainEventTypesCCHelper.MainEventTypeChanged += OnMainEventTypeChanged;

		}

		// for telling the view that the main event type has changed
		private void OnMainEventTypeChanged(IMainEventType newMainEventType)
		{
			AsyncSubmitTypeCommand.NotifyCanExecuteChanged();
		}
		private void setIsVisibleForExtraControlsInEditMode()
		{
			SubTypeExtraOptionsHelper.IsValueTypeSelected = CurrentType.IsValueType;
			SubTypeExtraOptionsHelper.IsMicroTaskTypeSelected = CurrentType.IsMicroTaskType;
			SubTypeExtraOptionsHelper.IsDefaultEventTimespanSelected = CurrentType.DefaultEventTimeSpan != TimeSpan.Zero;
		}
		#endregion


		#region Methods
		private async Task AsyncDeleteSelectedEventType()
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
						await Shell.Current.GoToAsync("AllEventsPage");
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

		private async Task AsyncSubmitType()
		{

			if (IsEdit)
			{
				if (await CanEditType())
				{
					_currentType.MainEventType = SelectedMainEventType;
					_currentType.EventTypeName = TypeName;
					_currentType.EventTypeColor = ColorButtonsHelperClass.SelectedColor;
					SetExtraUserControlsValues(_currentType);
					await _eventRepository.UpdateSubEventTypeAsync(_currentType);
					await Shell.Current.GoToAsync("//AllSubTypesPage"); // TODO Navigation
				}
			}
			else
			{
				if (await CanAddNewType())
				{
					var timespan = SubTypeExtraOptionsHelper.IsDefaultEventTimespanSelected ? DefaultEventTimespanCCHelper.GetDuration() : TimeSpan.Zero;
					var quantityAmount = SubTypeExtraOptionsHelper.IsValueTypeSelected ? DefaultMeasurementSelectorCCHelper.QuantityAmount : null;
					var microTasks = SubTypeExtraOptionsHelper.IsMicroTaskTypeSelected ? new List<MicroTaskModel>(MicroTasksCCAdapter.MicroTasksOC) : null;
					var newSubType = Factory.CreateNewEventType(MainEventTypesCCHelper.SelectedMainEventType, TypeName, ColorButtonsHelperClass.SelectedColor, timespan, quantityAmount, microTasks);
					await _eventRepository.AddSubEventTypeAsync(newSubType);
					TypeName = string.Empty;
				}
			}
		}



		//private void GoToAllSubTypesPage()
		//{
		//	Application.Current.MainPage.Navigation.PushAsync(new AllSubTypesPage());
		//}
		private void InitializeColorButtons() //TODO ! also to extract as a separate custom control
		{
			ButtonsColorsInitializerHelperClass buttonsColorsInitializerHelperClass = new ButtonsColorsInitializerHelperClass();
			ButtonsColorsOC = buttonsColorsInitializerHelperClass.ButtonsColorsOC;
		}
		private bool CanDeleteSelectedEventType()
		{
			return CurrentType.MainEventType.Title != PreferencesManager.GetMainTypeQuickNoteName();
		}
		public void SetExtraUserControlsValues(ISubEventTypeModel _currentType)
		{
			if (_currentType == null)
			{
				return;
			}
			if (SubTypeExtraOptionsHelper.IsDefaultEventTimespanSelected)
			{
				_currentType.DefaultEventTimeSpan = DefaultEventTimespanCCHelper.GetDuration();
			}
			else
			{
				_currentType.DefaultEventTimeSpan = TimeSpan.Zero;
			}
			if (SubTypeExtraOptionsHelper.IsMicroTaskTypeSelected)
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
		private async Task<bool> CanAddNewType()
		{
			if (TypeName == PreferencesManager.GetSubTypeQuickNoteName())
			{
				await App.Current.MainPage.DisplayActionSheet($"Name is not allowed!!!", "OK", null);

				return false;
			}
			return true;
		}
		private async Task<bool> CanEditType()
		{
			if (CurrentType.EventTypeName != PreferencesManager.GetSubTypeQuickNoteName() && TypeName == PreferencesManager.GetSubTypeQuickNoteName())
			{
				await App.Current.MainPage.DisplayActionSheet($"Name is not allowed!!!", "OK", null);
				return false;
			}
			return true;
		}
		private void OnMainEventTypeSelectedCommand(MainEventTypeViewModel mainEventTypeViewModel)
		{
			_isMainEventTypeSelected = true;
			MainEventTypesCCHelper.MainEventTypeSelectedCommand.Execute(mainEventTypeViewModel);
			SetCanSubmitTypeCommand();
		}
		#endregion
		private void SetCanSubmitTypeCommand()
		{
			CanSubmitTypeCommand = !string.IsNullOrEmpty(TypeName) && _isMainEventTypeSelected;
		}
	}

}
