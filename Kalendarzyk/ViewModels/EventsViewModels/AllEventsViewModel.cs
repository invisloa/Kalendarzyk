
/* Unmerged change from project 'Kalendarzyk (net8.0-maccatalyst)'
Before:
using Kalendarzyk.Models.EventModels;
After:
using CommunityToolkit.Mvvm.Input;
using Kalendarzyk.Models.EventModels;
*/

/* Unmerged change from project 'Kalendarzyk (net8.0-android34.0)'
Before:
using Kalendarzyk.Models.EventModels;
After:
using CommunityToolkit.Mvvm.Input;
using Kalendarzyk.Models.EventModels;
*/
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.Input;
using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.Services;
using Kalendarzyk.Views.CustomControls.CCInterfaces;
using Kalendarzyk.Views.CustomControls.CCViewModels;
using System.Collections.ObjectModel;

namespace Kalendarzyk.ViewModels.EventsViewModels
{
	public class AllEventsViewModel : AbstractEventViewModel, IMainEventTypesCCViewModel, IFilterDatesCC
	{
		public event Action<IMainEventType> MainEventTypeChanged;
		private ISubEventTypeModel _eventType;
		//MainEventTypesCC implementation
		#region MainEventTypesCC implementation
		protected IMainEventTypesCCViewModel _mainEventTypesCCHelper;
		protected ObservableCollection<ISubEventTypeModel> _allSubTypesForVisuals;

		public IMainEventType SelectedMainEventType
		{
			get => _mainEventTypesCCHelper.SelectedMainEventType;
			set
			{
				_mainEventTypesCCHelper.SelectedMainEventType = value;
				FilterAllSubEventTypesOCByMainEventType(value);
				OnPropertyChanged();
			}
		}
		private void FilterAllSubEventTypesOCByMainEventType(IMainEventType value)
		{
			var tempFilteredEventTypes = FilterSubTypesForVisuals(value);
			AllSubEventTypesOC = new ObservableCollection<ISubEventTypeModel>(tempFilteredEventTypes);
			OnPropertyChanged(nameof(AllSubEventTypesOC));
		}
		private ObservableCollection<ISubEventTypeModel> FilterSubTypesForVisuals(IMainEventType value)
		{
			return _allSubTypesForVisuals.ToList().FindAll(x => x.MainEventType.Equals(value)).ToObservableCollection();
		}
		public ObservableCollection<MainEventTypeViewModel> MainEventTypesVisualsOC
		{
			get => _mainEventTypesCCHelper.MainEventTypesVisualsOC;
			set => _mainEventTypesCCHelper.MainEventTypesVisualsOC = value;
		}
		public RelayCommand<MainEventTypeViewModel> MainEventTypeSelectedCommand { get; set; }
		public Color MainEventTypeBackgroundColor { get; set; } = Color.FromRgb(0, 0, 153); // Defeault color is blue
		protected void OnMainEventTypeSelected(MainEventTypeViewModel eventType)
		{
			_mainEventTypesCCHelper.MainEventTypeSelectedCommand.Execute(eventType);
			SelectedMainEventType = _mainEventTypesCCHelper.SelectedMainEventType;
			//OnUserEventTypeSelected(AllSubEventTypesOC[0]);
		}

		#endregion

		//IFilterDatesCC implementation
		#region IFilterDatesCC implementation
		private IFilterDatesCCHelperClass _filterDatesCCHelper = Factory.CreateFilterDatesCCHelperClass();
		public string TextFilterDateFrom { get; set; } = "FROM:";
		public string TextFilterDateTo { get; set; } = "TO:";
		public DateTime FilterDateFrom
		{
			get => _filterDatesCCHelper.FilterDateFrom;
			set
			{
				if (_filterDatesCCHelper.FilterDateFrom == value)
				{
					return;
				}
				_filterDatesCCHelper.FilterDateFrom = value;
				OnPropertyChanged();
				BindDataToScheduleList();
			}
		}
		public DateTime FilterDateTo
		{
			get => _filterDatesCCHelper.FilterDateTo;
			set
			{
				if (_filterDatesCCHelper.FilterDateTo == value)
				{
					return;
				}
				_filterDatesCCHelper.FilterDateTo = value;
				OnPropertyChanged();
				BindDataToScheduleList();
			}
		}
		private void OnFilterDateFromChanged()
		{
			FilterDateFrom = _filterDatesCCHelper.FilterDateFrom;
		}

		private void OnFilterDateToChanged()
		{
			FilterDateTo = _filterDatesCCHelper.FilterDateTo;
		}
		#endregion

		#region Fields


		#endregion

		#region Properties
		public AsyncRelayCommand DeleteAboveEventsCommand { get; set; }
		public AsyncRelayCommand SaveBelowEventsToFileCommand { get; set; }
		public AsyncRelayCommand SaveAllEventsToFileCommand { get; set; }
		public AsyncRelayCommand LoadEventsFromFileCommand { get; set; }
		public string AboveEventsListText
		{
			get
			{
				// switch selectedLanguage()
				{
					return "EVENTS LIST";
				}
			}
		}

		#endregion

		#region Constructors

		// All Events MODE
		public AllEventsViewModel() : base()
		{
			InitializeCommon();
			_allSubTypesForVisuals = new ObservableCollection<ISubEventTypeModel>(_eventRepository.DeepCopySubEventTypesList());
			SelectUserEventTypeCommand = new RelayCommand<ISubEventTypeModel>(OnUserEventTypeSelected);
			MainEventTypeSelectedCommand = new RelayCommand<MainEventTypeViewModel>(OnMainEventTypeSelected);
		}

		// Single Event Type MODE
		public AllEventsViewModel(ISubEventTypeModel eventType) : base()
		{
			InitializeCommon();
			var allTempTypes = new ObservableCollection<ISubEventTypeModel>();
			foreach (var item in AllSubEventTypesOC)
			{
				allTempTypes.Add(item);
			}
		}

		private void InitializeCommon()
		{
			_filterDatesCCHelper.FilterDateFromChanged += OnFilterDateFromChanged;
			_filterDatesCCHelper.FilterDateToChanged += OnFilterDateToChanged;
			DeleteAboveEventsCommand = new AsyncRelayCommand(DeleteShownEvents);
			SaveBelowEventsToFileCommand = new AsyncRelayCommand(OnSaveSelectedEventsAndTypesCommand);
			SaveAllEventsToFileCommand = new AsyncRelayCommand(OnSaveEventsAndTypesCommand);
			LoadEventsFromFileCommand = new AsyncRelayCommand(OnLoadEventsAndTypesCommand);
			this.SetFilterDatesValues(false); // using extension method
			_mainEventTypesCCHelper = Factory.CreateNewIMainEventTypeViewModelClass(_eventRepository.AllMainEventTypesList);
		}

		#endregion

		#region Commands

		public async Task DeleteOneEvent()
		{
			if (AllEventsListOC.Count == 0)
			{
				return;
			}
			var firstEvent = AllEventsListOC[0];
			await EventRepository.DeleteFromEventsListAsync(firstEvent);
		}

		public async Task DeleteShownEvents()
		{
			try
			{
				_eventRepository.AllEventsList.ToList().RemoveAll(item => EventsToShowList.Contains(item));
				AllEventsListOC = new ObservableCollection<IGeneralEventModel>(_eventRepository.AllEventsList);
				await EventRepository.SaveEventsListAsync();
			}
			catch (Exception ex)
			{
				await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
			}
		}
		public async Task DeleteAllEvents()
		{
			try
			{
				_eventRepository.AllEventsList.Clear();
				AllEventsListOC = new ObservableCollection<IGeneralEventModel>(_eventRepository.AllEventsList);
				await EventRepository.SaveEventsListAsync();

			}
			catch (Exception ex)
			{
				await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
			}
		}

		public async Task DeleteAllSubTypes()
		{
			try
			{
				_eventRepository.AllUserEventTypesList.Clear();
				AllSubEventTypesOC = new ObservableCollection<ISubEventTypeModel>(_eventRepository.AllUserEventTypesList);
				await EventRepository.SaveSubEventTypesListAsync();
			}
			catch (Exception ex)
			{
				await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
			}
		}

		#endregion

		#region Private Methods
		private async Task OnSaveEventsAndTypesCommand()
		{
			await _eventRepository.SaveEventsAndTypesToFile();
		}
		private async Task OnSaveSelectedEventsAndTypesCommand()
		{
			await _eventRepository.SaveEventsAndTypesToFile(new ObservableCollection<IGeneralEventModel>(EventsToShowList));
		}
		public AsyncRelayCommand TestButtonCommand2 { get; set; }

		private async Task OnLoadEventsAndTypesCommand()
		{
			await _eventRepository.LoadEventsAndTypesFromFile();
		}
		#endregion

		#region Override Methods

		public override void BindDataToScheduleList()
		{

			if (_eventType == null) // all events mode
			{
				ApplyEventsDatesFilter(FilterDateFrom, FilterDateTo);
			}
			else // single event type mode
			{
				// TODO Change to also visually select proper event type
				List<IGeneralEventModel> filteredEvents = AllEventsListOC
					.Where(x => x.EventType.Equals(_eventType))
					.ToList();

				// Clear existing items in the EventsToShowList
				EventsToShowList.Clear();

				// Add filtered items to the EventsToShowList
				foreach (var eventItem in filteredEvents)
				{
					try
					{
						EventsToShowList.Add(eventItem);

					}
					catch (Exception ex)
					{
						throw new Exception("Error adding event to EventsToShowList", ex);
					}
				}
			}
			EventsToShowList = new ObservableCollection<IGeneralEventModel>(EventsToShowList);
		}
		#endregion
		public void OnAppearing()
		{
			BindDataToScheduleList();
		}
	}
}
