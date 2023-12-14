using Kalendarzyk.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Kalendarzyk.Models;
using Kalendarzyk.Helpers;
using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.Models.EventModels;

namespace Kalendarzyk.ViewModels
{
	public class PreferencesViewModel : BaseViewModel
	{

		IEventRepository _repository;
		public AsyncRelayCommand CreateDummyDataCommand
		{
			get;
			set;	
		}
		public AsyncRelayCommand DeleteAllDataCommand
		{
			get;
			set;
		}
		public bool SelectedLanguage
		{
			get => PreferencesManager.GetSelectedLanguage();
			set
			{
				if (PreferencesManager.GetSelectedLanguage() != value)
				{
					PreferencesManager.SetSelectedLanguage(value);
					OnPropertyChanged();
				}
			}
		}


		public bool SubEventTypeTimesDifferent
		{
			get => PreferencesManager.GetSubEventTypeTimesDifferent();
			set
			{
				if (PreferencesManager.GetSubEventTypeTimesDifferent() != value)
				{
					PreferencesManager.SetSubEventTypeTimesDifferent(value);
					OnPropertyChanged();
				}
			}
		}

		public bool MainEventTypeTimesDifferent
		{
			get => PreferencesManager.GetMainEventTypeTimesDifferent();
			set
			{
				if (PreferencesManager.GetMainEventTypeTimesDifferent() != value)
				{
					PreferencesManager.SetMainEventTypeTimesDifferent(value);
					OnPropertyChanged();
				}
			}
		}

		public int HoursSpanFrom
		{
			get => PreferencesManager.GetHoursSpanFrom();
			set
			{
				if (PreferencesManager.GetHoursSpanFrom() != value)
				{
					PreferencesManager.SetHoursSpanFrom(value);
					OnPropertyChanged();
				}
			}
		}

		public int HoursSpanTo
		{
			get => PreferencesManager.GetHoursSpanTo();
			set
			{
				if (PreferencesManager.GetHoursSpanTo() != value)
				{
					PreferencesManager.SetHoursSpanTo(value);
					OnPropertyChanged();
				}
			}
		}

		public bool WeeklyHoursSpan
		{
			get => PreferencesManager.GetWeeklyHoursSpan();
			set
			{
				if (PreferencesManager.GetWeeklyHoursSpan() != value)
				{
					PreferencesManager.SetWeeklyHoursSpan(value);
					if (!value)
					{
						HoursSpanFrom = 0;
						HoursSpanTo = 24;
					}
					OnPropertyChanged();
				}
			}
		}

		// Text properties for labels
		public string SelectedLanguageText { get; set; } = "Selected Language";
		public string SubEventTypeTimesDifferentText { get; set; } = "Sub Event Type Times Different";
		public string MainEventTypeTimesDifferentText { get; set; } = "Main Event Type Times Different";
		public string WeeklyHoursSpanText { get; set; } = "Weekly Preferred Hours Span";

		// Save command
		//public ICommand SavePreferencesCommand { get; } = new RelayCommand(() => PreferencesManager.SavePreferences());

		public PreferencesViewModel(IEventRepository eventRepository)
		{
			_repository = eventRepository;
			CreateDummyDataCommand = new AsyncRelayCommand(CreateDummyData);
			DeleteAllDataCommand = new AsyncRelayCommand(ClearData);
		}

		private async Task ClearData()
		{
			await _repository.ClearAllEventsListAsync();
			await _repository.ClearAllSubEventTypesAsync();
			await _repository.ClearAllMainEventTypesAsync();
		}

		public static async Task AddQuickNotesTypes(IEventRepository repository)
		{
			if(repository.AllMainEventTypesList.Where(x => x.Title == "QNOTE").Count() != 0)
			{
				return;
			}
			// QUICK NOTE
			IMainTypeVisualModel quickNoteVisualModel = new IconModel(IconFont.Quickreply, Colors.Red, Colors.AntiqueWhite);
			IMainEventType quickNoteMainType = new MainEventType("QNOTE", quickNoteVisualModel);
			await repository.AddMainEventTypeAsync(quickNoteMainType);
			ISubEventTypeModel qNoteSubTypeModel = Factory.CreateNewEventType(quickNoteMainType, "QNOTE", Colors.Red, TimeSpan.FromSeconds(0), new QuantityModel(MeasurementUnit.Money, 0), new List<MicroTaskModel>());
			await repository.AddSubEventTypeAsync(qNoteSubTypeModel);
		}
		private async Task CreateDummyData() // it checks for the events before the events are imported so when there are any events added they will be deleted
		{
			await ClearData();

			await AddQuickNotesTypes(_repository);

			//DummyData
			IMainTypeVisualModel mainTypeVisualModel = new IconModel(IconFont.Work, Colors.Aquamarine, Colors.AliceBlue);
			IMainEventType mainEventType = new MainEventType("Invioces", mainTypeVisualModel);
			await _repository.AddMainEventTypeAsync(mainEventType);

			IMainTypeVisualModel mainTypeVisualModel2 = new IconModel(IconFont.Home, Colors.AliceBlue, Colors.Aquamarine);
			IMainEventType mainEventType2 = new MainEventType("Home", mainTypeVisualModel2);
			await _repository.AddMainEventTypeAsync(mainEventType2);

			IMainTypeVisualModel mainTypeVisualModel3 = new IconModel(IconFont.Traffic, Colors.Red, Colors.BlanchedAlmond);
			IMainEventType mainEventType3 = new MainEventType("RoadTrip", mainTypeVisualModel3);
			await _repository.AddMainEventTypeAsync(mainEventType3);



			ISubEventTypeModel subEventTypeModel = Factory.CreateNewEventType(mainEventType, "Dino", Colors.Blue, TimeSpan.FromSeconds(0), new QuantityModel(MeasurementUnit.Money, 100));
			ISubEventTypeModel subEventTypeModel2 = Factory.CreateNewEventType(mainEventType, "Chrupki", Colors.Red, TimeSpan.FromSeconds(0), new QuantityModel(MeasurementUnit.Money, 300));
			ISubEventTypeModel subEventTypeModel3 = Factory.CreateNewEventType(mainEventType2, "MicroTasker", Colors.MediumPurple, TimeSpan.FromSeconds(0), null, new List<MicroTaskModel> { new MicroTaskModel("Task1"), new MicroTaskModel("Task2") });
			ISubEventTypeModel subEventTypeModel4 = Factory.CreateNewEventType(mainEventType2, "MicroTasker2", Colors.Purple, TimeSpan.FromSeconds(0), null, new List<MicroTaskModel> { new MicroTaskModel("Task1", false), new MicroTaskModel("Task2", false) });
			ISubEventTypeModel subEventTypeModel5 = new SubEventTypeModel(mainEventType3, "Plain1", Colors.DarkCyan, TimeSpan.FromSeconds(0));
			ISubEventTypeModel subEventTypeModel6 = new SubEventTypeModel(mainEventType3, "Plain2", Colors.DarkGoldenrod, TimeSpan.FromSeconds(0));

			await _repository.AddSubEventTypeAsync(subEventTypeModel);
			await _repository.AddSubEventTypeAsync(subEventTypeModel2);
			await _repository.AddSubEventTypeAsync(subEventTypeModel3);
			await _repository.AddSubEventTypeAsync(subEventTypeModel4);
			await _repository.AddSubEventTypeAsync(subEventTypeModel5);
			await _repository.AddSubEventTypeAsync(subEventTypeModel6);

			await _repository.AddEventAsync(Factory.CreatePropperEvent("Dino", "Dino", DateTime.Now, DateTime.Now.AddHours(1), subEventTypeModel, new QuantityModel(MeasurementUnit.Money, 100)));
			await _repository.AddEventAsync(Factory.CreatePropperEvent("Chrupki", "Chrupki", DateTime.Now, DateTime.Now.AddHours(1), subEventTypeModel2, new QuantityModel(MeasurementUnit.Money, 300)));
			await _repository.AddEventAsync(Factory.CreatePropperEvent("MicroTasker", "MicroTasker", DateTime.Now, DateTime.Now.AddHours(1), subEventTypeModel3));
			await _repository.AddEventAsync(Factory.CreatePropperEvent("MicroTasker2", "MicroTasker2", DateTime.Now, DateTime.Now.AddHours(1), subEventTypeModel4));
			await _repository.AddEventAsync(Factory.CreatePropperEvent("Plain1", "Plain1", DateTime.Now, DateTime.Now.AddHours(1), subEventTypeModel5));
			await _repository.AddEventAsync(Factory.CreatePropperEvent("Plain2", "Plain2", DateTime.Now, DateTime.Now.AddHours(1), subEventTypeModel6));

		}
	}
}
