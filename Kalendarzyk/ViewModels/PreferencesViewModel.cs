using CommunityToolkit.Mvvm.
/* Unmerged change from project 'Kalendarzyk (net8.0-maccatalyst)'
Before:
using System.Windows.Input;
After:
using CommunityToolkit.Mvvm.Input;
*/

/* Unmerged change from project 'Kalendarzyk (net8.0-android34.0)'
Before:
using System.Windows.Input;
After:
using CommunityToolkit.Mvvm.Input;
*/
Input;
using Kalendarzyk.Helpers;
using Kalendarzyk.Models;
using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;

/* Unmerged change from project 'Kalendarzyk (net8.0-maccatalyst)'
Before:
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.Models.EventModels;
After:
using Kalendarzyk.Services;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.Views;
*/

/* Unmerged change from project 'Kalendarzyk (net8.0-android34.0)'
Before:
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.Models.EventModels;
After:
using Kalendarzyk.Services;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.Views;
*/
using Kalendarzyk.Services;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.Views;

/* Unmerged change from project 'Kalendarzyk (net8.0-maccatalyst)'
Before:
using Kalendarzyk.Views;
After:
using System.Windows.Input;
*/

/* Unmerged change from project 'Kalendarzyk (net8.0-android34.0)'
Before:
using Kalendarzyk.Views;
After:
using System.Windows.Input;
*/
using System.Windows.Input;

namespace Kalendarzyk.ViewModels
{
	public class PreferencesViewModel : BaseViewModel
	{

		IEventRepository _eventRepository;
		private ICommand _goToLanguageSelectionPage;
		public AsyncRelayCommand ResetToDefaultDataCommand
		{
			get;
			set;
		}
		public AsyncRelayCommand DeleteAllDataCommand
		{
			get;
			set;
		}
		public Enums.LanguageEnum SelectedLanguage
		{
			get
			{
				// Retrieve the stored language integer value from preferences
				int storedLanguage = PreferencesManager.GetSelectedLanguage();

				// Convert the stored integer value to LanguageEnum
				return (Enums.LanguageEnum)storedLanguage;
			}
			set
			{
				if (PreferencesManager.GetSelectedLanguage() != (int)value)
				{
					PreferencesManager.SetSelectedLanguage((int)value);
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
		public ICommand GoToLanguageSelectionPage
		{
			get
			{
				return _goToLanguageSelectionPage ?? (_goToLanguageSelectionPage = new AsyncRelayCommand(async () =>
				{
					Application.Current.MainPage.Navigation.PushAsync(new FirstRunPage());
				}));
			}
		}

		// Text properties for labels
		public string SelectedLanguageText { get => (Enums.LanguageEnum)PreferencesManager.GetSelectedLanguage() == Enums.LanguageEnum.English ? "Select Language" : "Wybierz język"; }
		public string SubEventTypeTimesDifferentText { get; set; } = "Sub Event Type Times Different";
		public string MainEventTypeTimesDifferentText { get; set; } = "Main Event Type Times Different";
		public string WeeklyHoursSpanText { get; set; } = "Weekly Preferred Hours Span";

		// Save command
		//public ICommand SavePreferencesCommand { get; } = new RelayCommand(() => PreferencesManager.SavePreferences());

		public PreferencesViewModel()
		{
			_eventRepository = Factory.GetEventRepository();
			ResetToDefaultDataCommand = new AsyncRelayCommand(ResetToDefaultData);
			DeleteAllDataCommand = new AsyncRelayCommand(ClearData);
		}

		private async Task ClearData()
		{
			var action = await App.Current.MainPage.DisplayActionSheet("Delete all current data\nAre You sure??", "Cancel", null, "Clear all data");
			switch (action)
			{
				case "Clear all data":
					break;      // if ok continue code below
				default:
					return;     // if cancel or null stop this method
			}
			await _eventRepository.ClearAllEventsListAsync();
			await _eventRepository.ClearAllSubEventTypesAsync();
			await _eventRepository.ClearAllMainEventTypesAsync();
			await AddQuickNotesTypes(); //there has to be quick notes types
		}

		public static async Task AddQuickNotesTypes()
		{
			var repository = Factory.GetEventRepository();

			if (repository.AllMainEventTypesList.Where(x => x.Title == PreferencesManager.GetMainTypeQuickNoteName()).Count() != 0)
			{
				return;
			}
			// QUICK NOTE
			IMainTypeVisualModel quickNoteVisualModel = new IconModel(IconFont.Quickreply, Colors.Red, Colors.AntiqueWhite);
			IMainEventType quickNoteMainType = new MainEventType(PreferencesManager.GetMainTypeQuickNoteName(), quickNoteVisualModel);
			await repository.AddMainEventTypeAsync(quickNoteMainType);
			ISubEventTypeModel qNoteSubTypeModel = Factory.CreateNewEventType(quickNoteMainType, PreferencesManager.GetSubTypeQuickNoteName(), Colors.Red, TimeSpan.FromSeconds(0), new QuantityModel(MeasurementUnit.Money, 0), new List<MicroTaskModel>());
			await repository.AddSubEventTypeAsync(qNoteSubTypeModel);
			var qNoteEvent = Factory.CreatePropperEvent("My first quick note", "My greates description :)", DateTime.Now, DateTime.Now, qNoteSubTypeModel, new QuantityModel(MeasurementUnit.Money, 0));
			await repository.AddEventAsync(qNoteEvent);
		}
		private async Task ResetToDefaultData() // it checks for the events before the events are imported so when there are any events added they will be deleted
		{
			var action = await App.Current.MainPage.DisplayActionSheet("Delete all current data\nAre You sure??", "Cancel", null, "Restore default data");
			switch (action)
			{
				case "Restore default data":
					break;      // if ok continue code below
				default:
					return;     // if cancel or null stop this method
			}
			await _eventRepository.ClearAllEventsListAsync();
			await _eventRepository.ClearAllSubEventTypesAsync();
			await _eventRepository.ClearAllMainEventTypesAsync();
			await AddQuickNotesTypes();

			//DummyData
			IMainTypeVisualModel carSpendingMainType = new IconModel(IconFont.Minor_crash, Colors.Aquamarine, Colors.AliceBlue);
			IMainEventType mainEventType = new MainEventType("Samochody", carSpendingMainType);
			await _eventRepository.AddMainEventTypeAsync(mainEventType);

			IMainTypeVisualModel mainTypeVisualModel2 = new IconModel(IconFont.List, Colors.AliceBlue, Colors.Aquamarine);
			IMainEventType homeMTType = new MainEventType("Dom", mainTypeVisualModel2);
			await _eventRepository.AddMainEventTypeAsync(homeMTType);

			IMainTypeVisualModel mainTypeVisualModel3 = new IconModel(IconFont.Notification_important, Colors.Red, Colors.BlanchedAlmond);
			IMainEventType birthdaysMType = new MainEventType("Wydarzenia", mainTypeVisualModel3);
			await _eventRepository.AddMainEventTypeAsync(birthdaysMType);



			ISubEventTypeModel oilSubEventType = Factory.CreateNewEventType(mainEventType, "Paliwo", Colors.Blue, TimeSpan.FromSeconds(0), new QuantityModel(MeasurementUnit.Money, 100));
			ISubEventTypeModel mechaniciansSubEventType = Factory.CreateNewEventType(mainEventType, "Naprawy", Colors.DarkRed, TimeSpan.FromSeconds(0), new QuantityModel(MeasurementUnit.Money, 0));
			ISubEventTypeModel shoppingSubEventType = Factory.CreateNewEventType(homeMTType, "Spożywcze", Colors.MediumPurple, TimeSpan.FromSeconds(0), new QuantityModel(MeasurementUnit.Money, 0), new List<MicroTaskModel> { new MicroTaskModel("Mleko"), new MicroTaskModel("Chleb") });
			ISubEventTypeModel examsSubEventType = new SubEventTypeModel(birthdaysMType, "Studia", Colors.DarkCyan, TimeSpan.FromSeconds(0));

			await _eventRepository.AddSubEventTypeAsync(oilSubEventType);
			await _eventRepository.AddSubEventTypeAsync(mechaniciansSubEventType);
			await _eventRepository.AddSubEventTypeAsync(shoppingSubEventType);
			await _eventRepository.AddSubEventTypeAsync(examsSubEventType);

			await _eventRepository.AddEventAsync(Factory.CreatePropperEvent("Jaguar", "25l", DateTime.Now - TimeSpan.FromDays(3), DateTime.Now - TimeSpan.FromDays(3), oilSubEventType, new QuantityModel(MeasurementUnit.Money, 150)));
			await _eventRepository.AddEventAsync(Factory.CreatePropperEvent("Maserati", "Olej", DateTime.Now - TimeSpan.FromDays(2), DateTime.Now - TimeSpan.FromDays(2) + TimeSpan.FromHours(1), mechaniciansSubEventType, new QuantityModel(MeasurementUnit.Money, 500)));
			await _eventRepository.AddEventAsync(Factory.CreatePropperEvent("Lidl", "", DateTime.Now, DateTime.Now.AddHours(1), shoppingSubEventType, new QuantityModel(MeasurementUnit.Money, 115), new List<MicroTaskModel> { new MicroTaskModel("Mleko"), new MicroTaskModel("Chleb"), new MicroTaskModel("Jabłka"), new MicroTaskModel("Pomidory"), new MicroTaskModel("Lasagne"), new MicroTaskModel("Ser"), }));
			await _eventRepository.AddEventAsync(Factory.CreatePropperEvent("Matematyka", "Integrals", DateTime.Now + TimeSpan.FromDays(7), DateTime.Now + TimeSpan.FromDays(7) + TimeSpan.FromMinutes(60), examsSubEventType));

		}
	}
}
