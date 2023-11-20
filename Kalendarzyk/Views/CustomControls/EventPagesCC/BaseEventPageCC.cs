namespace Kalendarzyk.Views.CustomControls
{
	using Microsoft.Maui.Graphics;
	using Kalendarzyk.Models.EventModels;
	using Microsoft.Maui.Layouts;
	using System;
	using System.Collections.ObjectModel;
	using System.Linq;
	using MauiGrid = Microsoft.Maui.Controls.Grid;

	public abstract class BaseEventPageCC : MauiGrid
	{
		protected int _displayEventsLimit;  // Set a limit to how many items will be displayed
		protected int _eventNamesFontSize = 15;
		protected int _dayNamesFontSize = 15;
		protected Color _eventTextColor = (Color)Application.Current.Resources["MainTextColor"];
		protected Color _emptyLabelColor = (Color)Application.Current.Resources["MainBackgroundColor"];
		protected Color _frameBorderColor = Color.FromRgba(255, 255, 255, 255);
		protected Color _moreEventsLabelColor = Color.FromRgba(0, 0, 0, 100);
		public BaseEventPageCC()
		{
			setDisplayLimit(1, 4);

		}

		public static readonly BindableProperty CurrentSelectedDateProperty =
			 BindableProperty.Create(
				 nameof(CurrentSelectedDate),
				 typeof(DateTime),
				 typeof(BaseEventPageCC),
				 defaultBindingMode: BindingMode.TwoWay);
		public DateTime CurrentSelectedDate
		{
			get => (DateTime)GetValue(CurrentSelectedDateProperty);
			set => SetValue(CurrentSelectedDateProperty, value);
		}

		public static readonly BindableProperty EventsToShowListProperty =
			BindableProperty.Create(
			nameof(EventsToShowList),
			typeof(ObservableCollection<IGeneralEventModel>),
			typeof(BaseEventPageCC),
			defaultBindingMode: BindingMode.TwoWay);
		public ObservableCollection<IGeneralEventModel> EventsToShowList
		{
			get => (ObservableCollection<IGeneralEventModel>)GetValue(EventsToShowListProperty);
			set => SetValue(EventsToShowListProperty, value);
		}

		public static readonly BindableProperty AllEventsListProperty =
			BindableProperty.Create(
			nameof(AllEventsList),
			typeof(ObservableCollection<IGeneralEventModel>),
			typeof(BaseEventPageCC));

		public ObservableCollection<IGeneralEventModel> AllEventsList
		{
			get => (ObservableCollection<IGeneralEventModel>)GetValue(AllEventsListProperty);
			set => SetValue(AllEventsListProperty, value);
		}

		public static readonly BindableProperty EventSelectedCommandProperty =
			BindableProperty.Create(
			nameof(EventSelectedCommand),
			typeof(RelayCommand<IGeneralEventModel>),
			typeof(BaseEventPageCC));

		public RelayCommand<IGeneralEventModel> EventSelectedCommand
		{
			get => (RelayCommand<IGeneralEventModel>)GetValue(EventSelectedCommandProperty);
			set => SetValue(EventSelectedCommandProperty, value);
		}

		public static readonly BindableProperty GoToSelectedDateCommandProperty =
			BindableProperty.Create(
			nameof(GoToSelectedDateCommand),
			typeof(RelayCommand<DateTime>),
			typeof(BaseEventPageCC));

		public RelayCommand<DateTime> GoToSelectedDateCommand
		{
			get => (RelayCommand<DateTime>)GetValue(GoToSelectedDateCommandProperty);
			set => SetValue(GoToSelectedDateCommandProperty, value);
		}
		private void setDisplayLimit(int phoneLimit, int desktopLimit)
		{
			var deviceInfo = DeviceInfo.Idiom;
			if (deviceInfo == DeviceIdiom.Desktop)
			{
				_displayEventsLimit = desktopLimit;
			}
			//else if (deviceInfo == DeviceIdiom.Phone)
			//{
			//	_displayEventsLimit = phoneLimit;
			//}
			else
			{
				_displayEventsLimit = phoneLimit; // Default value for other idioms
			}
		}
	}
}