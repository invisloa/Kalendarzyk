﻿using Kalendarzyk.Views;

/* Unmerged change from project 'Kalendarzyk (net8.0-maccatalyst)'
Before:
using Kalendarzyk.Views.CustomControls.CCViewModels;
using Kalendarzyk.Views.CustomControls.CCInterfaces;
After:
using Kalendarzyk.Views.CustomControls.CCInterfaces;
using Kalendarzyk.Views.CustomControls.CCViewModels;
*/

/* Unmerged change from project 'Kalendarzyk (net8.0-android34.0)'
Before:
using Kalendarzyk.Views.CustomControls.CCViewModels;
using Kalendarzyk.Views.CustomControls.CCInterfaces;
After:
using Kalendarzyk.Views.CustomControls.CCInterfaces;
using Kalendarzyk.Views.CustomControls.CCViewModels;
*/
using Kalendarzyk.Views.CustomControls.CCInterfaces;

namespace Kalendarzyk.ViewModels.EventsViewModels
{
	public abstract class AbstractEventViewModel : PlainBaseAbstractEventViewModel, ITodayAndSelectedDateCC
	{
		#region Fields
		private RelayCommand _goToAddEventPageCommand;
		private DateTime _currentSelectedDate = DateTime.Now;

		#endregion

		#region Properties

		private DateTime _currentDate = DateTime.Now;
		public DateTime CurrentDate => _currentDate;
		public DateTime CurrentSelectedDate
		{
			get => _currentSelectedDate;
			set
			{
				if (_currentSelectedDate != value)
				{
					_currentSelectedDate = value;
					OnPropertyChanged();
					BindDataToScheduleList();
					//DatePickerDateSelectedCommand.Execute(_currentSelectedDate); // TODO: check if this is the right way to do it
				}
			}
		}
		#endregion

		#region Constructor

		public AbstractEventViewModel() : base()
		{
			GoToSelectedDateCommand = new RelayCommand<DateTime>(GoToSelectedDatePage);
			SelectTodayDateCommand = new RelayCommand(() => CurrentSelectedDate = CurrentDate);
		}

		#endregion

		#region Commands
		public RelayCommand GoToAddEventPageCommand => _goToAddEventPageCommand ?? (_goToAddEventPageCommand = new RelayCommand(GoToAddEventPage));
		public RelayCommand<DateTime> GoToSelectedDateCommand { get; set; }
		public RelayCommand SelectTodayDateCommand { get; set; }
		#endregion

		#region Private Methods

		private void GoToAddEventPage()
		{
			Application.Current.MainPage.Navigation.PushAsync(new EventPage(_currentSelectedDate));
		}

		protected void GoToSelectedDatePage(DateTime selectedDate)
		{
			var _dailyEventsPage = new DailyEventsPage();
			var _dailyEventsPageBindingContext = _dailyEventsPage.BindingContext as DailyEventsViewModel;
			_dailyEventsPageBindingContext.CurrentSelectedDate = selectedDate;
			Application.Current.MainPage.Navigation.PushAsync(_dailyEventsPage);
		}
		#endregion

		#region Protected Methods



		#endregion
	}
}
