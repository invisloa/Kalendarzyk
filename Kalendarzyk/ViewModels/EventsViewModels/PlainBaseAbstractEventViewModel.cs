﻿using CommunityToolkit.Maui.Core.Extensions;
using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.Services;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.Views;
using System.Collections.ObjectModel;

namespace Kalendarzyk.ViewModels.EventsViewModels
{
	public abstract class PlainBaseAbstractEventViewModel : BaseViewModel
	{
		#region Fields
		protected IEventRepository _eventRepository;
		private ObservableCollection<IGeneralEventModel> _allEventsListOC;
		private ObservableCollection<ISubEventTypeModel> _AllSubEventTypesOC;
		private ObservableCollection<IGeneralEventModel> _eventsToShowList = new ObservableCollection<IGeneralEventModel>();
		private RelayCommand<ISubEventTypeModel> _selectUserEventTypeCommand;
		private RelayCommand<IGeneralEventModel> _goToEditEventCommand;
		private RelayCommand _goToAddNewSubTypePageCommand;


		protected Color _deselectedUserEventTypeColor;
		#endregion

		#region Properties
		public IEventRepository EventRepository
		{
			get => _eventRepository;
		}
		public ObservableCollection<IGeneralEventModel> AllEventsListOC
		{
			get => _allEventsListOC;
			set
			{
				_allEventsListOC = value;
				OnPropertyChanged();
				OnOnEventsToShowListUpdated();
			}
		}

		public ObservableCollection<ISubEventTypeModel> AllSubEventTypesOC
		{
			get => _AllSubEventTypesOC;
			set
			{
				if (_AllSubEventTypesOC == value) return;
				_AllSubEventTypesOC = value;
				OnPropertyChanged();
				OnOnEventsToShowListUpdated();
			}
		}
		public ObservableCollection<IGeneralEventModel> EventsToShowList
		{
			get => _eventsToShowList;
			set
			{
				if (_eventsToShowList == value) return;
				_eventsToShowList = value;
				OnPropertyChanged();
			}
		}
		public RelayCommand<ISubEventTypeModel> SelectUserEventTypeCommand
		{
			get
			{
				return _selectUserEventTypeCommand ?? (_selectUserEventTypeCommand = new RelayCommand<ISubEventTypeModel>(OnUserEventTypeSelected));
			}
			set
			{
				if (_selectUserEventTypeCommand == value) return;
				_selectUserEventTypeCommand = value;
				OnPropertyChanged();
			}
		}
		public RelayCommand GoToAddNewSubTypePageCommand => _goToAddNewSubTypePageCommand ?? (_goToAddNewSubTypePageCommand = new RelayCommand(GoToAddNewSubTypePage));

		public RelayCommand<IGeneralEventModel> GoToEditEventCommand => _goToEditEventCommand ?? (_goToEditEventCommand = new RelayCommand<IGeneralEventModel>(OnGoToEditEventCommand));

		#endregion

		#region Constructor
		public PlainBaseAbstractEventViewModel()
		{
			_eventRepository = Factory.GetEventRepository();
			AllEventsListOC = _eventRepository.AllEventsList;
			AllSubEventTypesOC = _eventRepository.AllUserEventTypesList;
			if (Application.Current.Resources.TryGetValue("MainEventDeselectedBackgroundColor", out var retrievedColor))
			{
				_deselectedUserEventTypeColor = (Color)retrievedColor;
			}
			else
			{
				// if the resource is not found
				// write Color.FromArgb black color value

				_deselectedUserEventTypeColor = Color.FromArgb("#ff949494");
			}

		}
		#endregion

		#region Public Methods
		public abstract void BindDataToScheduleList();
		#endregion

		#region Protected Methods
		protected virtual void OnOnEventsToShowListUpdated()
		{
			OnEventsToShowListUpdated?.Invoke();
		}

		#endregion

		#region Private Methods

		protected virtual void OnUserEventTypeSelected(ISubEventTypeModel eventSubType)
		{
			eventSubType.IsSelectedToFilter = !eventSubType.IsSelectedToFilter;
			if (eventSubType.IsSelectedToFilter)
			{
				eventSubType.BackgroundColor = eventSubType.EventTypeColor;
			}
			else
			{
				eventSubType.BackgroundColor = _deselectedUserEventTypeColor;
			}
			BindDataToScheduleList();
		}

		private void OnGoToEditEventCommand(IGeneralEventModel selectedEvent)
		{
			Application.Current.MainPage.Navigation.PushAsync(new EventPage(selectedEvent));
		}

		#endregion
		protected virtual void ApplyEventsDatesFilter(DateTime startDate, DateTime endDate)
		{
			var selectedToFilterEventTypes = AllSubEventTypesOC
				.Where(x => x.IsSelectedToFilter)
				.Select(x => x.EventTypeName)
				.ToHashSet();

			ObservableCollection<IGeneralEventModel> filteredEvents = AllEventsListOC
				.Where(x => selectedToFilterEventTypes.Contains(x.EventType.ToString()) &&
							x.StartDateTime.Date >= startDate.Date &&
							x.StartDateTime.Date <= endDate.Date)
				.ToObservableCollection();

			// Clear existing items in the EventsToShowList
			EventsToShowList.Clear();
			EventsToShowList = new ObservableCollection<IGeneralEventModel>(filteredEvents);

		}
		private void GoToAddNewSubTypePage()
		{
			Application.Current.MainPage.Navigation.PushAsync(new AddNewSubTypePage());
		}

		#region Events
		public event Action OnEventsToShowListUpdated;
		#endregion
	}
}
