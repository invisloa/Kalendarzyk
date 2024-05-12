using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.Services;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.ViewModels.EventOperations;
using Kalendarzyk.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk.ViewModels
{
    public partial class FavoritePageViewModel : ObservableObject
	{
		#region Fields
		private ISubEventTypeModel _subEventType;
		private IEventRepository _eventRepository;
		private string _searchBoxText;
		[ObservableProperty]
		private bool _isSelectedDeleteMode;
		[ObservableProperty]
		private bool _isBusy = true;
		[ObservableProperty]
		private ObservableCollection<IGeneralEventModel> _eventsToShowList;
		[ObservableProperty]
		private AsyncRelayCommand<IGeneralEventModel> _deleteEventCommand;
		[ObservableProperty]
		private RelayCommand _toggleDeleteModeCommand;
		[ObservableProperty]
		private AsyncRelayCommand<IGeneralEventModel> _currentCommand;
		[ObservableProperty]
		private AsyncRelayCommand<IGeneralEventModel> _goToEditEventCommand;
		[ObservableProperty]
		private AsyncRelayCommand _goToAddEventCommand;
		#endregion
		#region Properties
		public int TitleFontSize => 25;	// todo small big fonts in settings
		public string PageTitle
		{
			get
			{
				if(_subEventType == null)
					return "Quick notes";
				return _subEventType.EventTypeName;
			}
		}
		public string SearchBoxText
		{
			get => _searchBoxText;
			set
			{
				SetProperty(ref _searchBoxText, value);
				SearchEvents();
			}
		}

		#endregion

		#region ctor
		public FavoritePageViewModel(IEventRepository eventRepository)
		{
			InitializeCommon(eventRepository);
			_subEventType = _eventRepository.AllUserEventTypesList.Where(x => x.EventTypeName == PreferencesManager.GetSubTypeQuickNoteName()).FirstOrDefault();

		}
		public FavoritePageViewModel(IEventRepository eventRepository, ISubEventTypeModel subEventType)
        {
			InitializeCommon(eventRepository);
			_subEventType = subEventType;
			EventsToShowList = new ObservableCollection<IGeneralEventModel>(_eventRepository.AllEventsList.Where(x => x.EventType.Equals(subEventType)));
		}
		#endregion

		#region Methods
		private void InitializeCommon(IEventRepository eventRepository)
		{
			_eventRepository = eventRepository;
			GoToAddEventCommand = new AsyncRelayCommand(OnGoToAddEventCommand);
			GoToEditEventCommand = new AsyncRelayCommand<IGeneralEventModel>(OnGoToEditEventCommand);
			DeleteEventCommand = new AsyncRelayCommand<IGeneralEventModel>(OnDeleteEventCommand);
			ToggleDeleteModeCommand = new RelayCommand(OnToggleDeleteMode);
			CurrentCommand = GoToEditEventCommand; // Set the default command
		}
		private Task OnGoToEditEventCommand(IGeneralEventModel selectedEvent)
		{
			return Application.Current.MainPage.Navigation.PushAsync(new EventPage(selectedEvent));
		}
		public void BindDataToShow(string eventsTypeNameToShow)
		{
			var eventsToShow = _eventRepository.AllEventsList.Where(x => x.EventType.EventTypeName == eventsTypeNameToShow);
			EventsToShowList = new ObservableCollection<IGeneralEventModel>(eventsToShow);
		}
		private void OnToggleDeleteMode()
		{
			IsSelectedDeleteMode = !IsSelectedDeleteMode;
			UpdateCurrentCommand();
		}
		private void UpdateCurrentCommand()
		{
			if (IsSelectedDeleteMode)
			{
				CurrentCommand = DeleteEventCommand;
			}
			else
			{
				CurrentCommand = GoToEditEventCommand;
			}
		}
		private async Task OnDeleteEventCommand(IGeneralEventModel eventToDelete)
		{
			await _eventRepository.DeleteFromEventsListAsync(eventToDelete);
			EventsToShowList.Remove(eventToDelete);
		}


		private async Task OnGoToAddEventCommand()
		{
			var viewModel = new EventOperationsViewModel(DateTime.Today);
			viewModel.SelectUserEventTypeCommand.Execute(_subEventType);
			var eventPageToShow = new EventPage
			{
				BindingContext = viewModel
			};
			await Application.Current.MainPage.Navigation.PushAsync(eventPageToShow);
		}

		private void SearchEvents()
		{
			EventsToShowList = new ObservableCollection<IGeneralEventModel>(_eventRepository.AllEventsList.Where(x => x.EventType.EventTypeName == _subEventType.EventTypeName && x.Title.Contains(SearchBoxText)));
		}

		#endregion
	}
}
