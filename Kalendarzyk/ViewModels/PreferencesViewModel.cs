using Kalendarzyk.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace Kalendarzyk.ViewModels
{
	public class PreferencesViewModel : BaseViewModel
	{
		public bool IsDeleteAllSelected
		{
			get => PreferencesManager.GetIsDeleteAllSelected();
			set
			{
				if (PreferencesManager.GetIsDeleteAllSelected() != value)
				{
					PreferencesManager.SetIsDeleteAllSelected(value);
					OnPropertyChanged();
				}
			}
		}

		public bool IsCreateDummyDataSelected
		{
			get => PreferencesManager.GetIsCreateDummyDataSelected();
			set
			{
				if (PreferencesManager.GetIsCreateDummyDataSelected() != value)
				{
					PreferencesManager.SetIsCreateDummyDataSelected(value);
					OnPropertyChanged();
				}
			}
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
		public ICommand SaveCommand { get; }

		public PreferencesViewModel()
		{
		}


	}
}
