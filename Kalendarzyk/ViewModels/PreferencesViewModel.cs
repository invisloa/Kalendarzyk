﻿using Kalendarzyk.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Kalendarzyk.Models;
using Kalendarzyk.Helpers;
using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.Models.EventModels;
using System;

namespace Kalendarzyk.ViewModels
{
	public class PreferencesViewModel : BaseViewModel
	{

		IEventRepository _eventRepository;
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

		public PreferencesViewModel()
		{
			_eventRepository = Factory.CreateNewEventRepository();
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
		}

		public static async Task AddQuickNotesTypes()
		{
			var repository = Factory.CreateNewEventRepository();

			if (repository.AllMainEventTypesList.Where(x => x.Title == "QNOTE").Count() != 0)
			{
				return;
			}
			// QUICK NOTE
			IMainTypeVisualModel quickNoteVisualModel = new IconModel(IconFont.Quickreply, Colors.Red, Colors.AntiqueWhite);
			IMainEventType quickNoteMainType = new MainEventType("QNOTE", quickNoteVisualModel);
			await repository.AddMainEventTypeAsync(quickNoteMainType);
			ISubEventTypeModel qNoteSubTypeModel = Factory.CreateNewEventType(quickNoteMainType, "QNOTE", Colors.Red, TimeSpan.FromSeconds(0), new QuantityModel(MeasurementUnit.Money, 0), new List<MicroTaskModel>());
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
					break;		// if ok continue code below
				default:
					return;		// if cancel or null stop this method
			}
			await _eventRepository.ClearAllEventsListAsync();
			await _eventRepository.ClearAllSubEventTypesAsync();
			await _eventRepository.ClearAllMainEventTypesAsync();
			await AddQuickNotesTypes();

			//DummyData
			IMainTypeVisualModel carSpendingMainType = new IconModel(IconFont.Minor_crash, Colors.Aquamarine, Colors.AliceBlue);
			IMainEventType mainEventType = new MainEventType("Cars", carSpendingMainType);
			await _eventRepository.AddMainEventTypeAsync(mainEventType);

			IMainTypeVisualModel mainTypeVisualModel2 = new IconModel(IconFont.List, Colors.AliceBlue, Colors.Aquamarine);
			IMainEventType homeMTType = new MainEventType("Home", mainTypeVisualModel2);
			await _eventRepository.AddMainEventTypeAsync(homeMTType);

			IMainTypeVisualModel mainTypeVisualModel3 = new IconModel(IconFont.Notification_important, Colors.Red, Colors.BlanchedAlmond);
			IMainEventType birthdaysMType = new MainEventType("Important dates", mainTypeVisualModel3);
			await _eventRepository.AddMainEventTypeAsync(birthdaysMType);



			ISubEventTypeModel oilSubEventType = Factory.CreateNewEventType(mainEventType, "Oil", Colors.Blue, TimeSpan.FromSeconds(0), new QuantityModel(MeasurementUnit.Money, 100));
			ISubEventTypeModel mechaniciansSubEventType = Factory.CreateNewEventType(mainEventType, "Mechanicians", Colors.Red, TimeSpan.FromSeconds(0), new QuantityModel(MeasurementUnit.Money, 0));
			ISubEventTypeModel shoppingSubEventType = Factory.CreateNewEventType(homeMTType, "Shopping list", Colors.MediumPurple, TimeSpan.FromSeconds(0), null, new List<MicroTaskModel> { new MicroTaskModel("Milk"), new MicroTaskModel("Bread") });
			ISubEventTypeModel examsSubEventType = new SubEventTypeModel(birthdaysMType, "Exams", Colors.DarkCyan, TimeSpan.FromSeconds(0));

			await _eventRepository.AddSubEventTypeAsync(oilSubEventType);
			await _eventRepository.AddSubEventTypeAsync(mechaniciansSubEventType);
			await _eventRepository.AddSubEventTypeAsync(shoppingSubEventType);
			await _eventRepository.AddSubEventTypeAsync(examsSubEventType);

			await _eventRepository.AddEventAsync(Factory.CreatePropperEvent("Jaguar", "25l", DateTime.Now-TimeSpan.FromDays(3), DateTime.Now - TimeSpan.FromDays(3), oilSubEventType, new QuantityModel(MeasurementUnit.Money, 100)));
			await _eventRepository.AddEventAsync(Factory.CreatePropperEvent("Mercedes", "Car Wash", DateTime.Now - TimeSpan.FromDays(2), DateTime.Now - TimeSpan.FromDays(2) + TimeSpan.FromHours(1), mechaniciansSubEventType, new QuantityModel(MeasurementUnit.Money, 25)));
			await _eventRepository.AddEventAsync(Factory.CreatePropperEvent("Shopping", "", DateTime.Now, DateTime.Now.AddHours(1), shoppingSubEventType));
			await _eventRepository.AddEventAsync(Factory.CreatePropperEvent("Math", "Integrals", DateTime.Now+TimeSpan.FromDays(1), DateTime.Now + TimeSpan.FromDays(1)+TimeSpan.FromMinutes(60), examsSubEventType));

		}
	}
}
