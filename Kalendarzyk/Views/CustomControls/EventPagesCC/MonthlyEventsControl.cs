namespace Kalendarzyk.Views.CustomControls
{
	using CommunityToolkit.Maui.Core.Extensions;
	using Kalendarzyk.Models.EventModels;
	using Microsoft.Maui.Graphics;
	using Microsoft.Maui.Layouts;
	using System;
	using System.Collections.ObjectModel;
	using System.Linq;
	using static Kalendarzyk.App;

	public class MonthlyEventsControl : BaseEventPageCC
	{
		private readonly int _minimumDayWidthRequest = 45;
		private readonly int _minimumDayHeightRequest = 30;
		private readonly int _dateFontSize = 12;
		private readonly Color _watermarkDateColor = (Color)Application.Current.Resources["MainTextColor"];
		private readonly int _LimitMoreLabbelText = 8;
		private readonly int _LimitIconsToShow = 3;

		private readonly int _phoneLimitMoreLabbelText = 6;
		private readonly int _phoneLimitIconsToShow = 1;
		private readonly int _pcLimitMoreLabbelText = 8;
		private readonly int _pcLimitIconsToShow = 3;


		private readonly int _phoneTextFontSize = 15;
		private readonly int _phoneEventIconFrameSize = 15;
		private readonly int _pcTextFontSize = 30;
		private readonly int _pcEventIconFrameSize = 35;

		private readonly int _TextFontSize;
		private readonly int _EventIconFrameSize;

		public MonthlyEventsControl()
		{
			// Determine the device type and set sizes accordingly
			if (DeviceInfo.Idiom == DeviceIdiom.Phone)
			{
				// Phone specific settings
				_TextFontSize = _phoneTextFontSize;
				_EventIconFrameSize = _phoneEventIconFrameSize;
				_LimitMoreLabbelText = _phoneLimitMoreLabbelText;
				_LimitIconsToShow = _phoneLimitIconsToShow;
			}
			else
			{
				// PC or other devices
				_TextFontSize = _pcTextFontSize;
				_EventIconFrameSize = _pcEventIconFrameSize;
				_LimitMoreLabbelText = _pcLimitMoreLabbelText;
				_LimitIconsToShow = _pcLimitIconsToShow;
			}
		}

		public void GenerateGrid()
		{
			ClearGrid();
			GenerateDayLabels();
			GenerateDateFrames();
		}

		private void ClearGrid()
		{
			RowDefinitions.Clear();
			ColumnDefinitions.Clear();
			Children.Clear();

			int daysInMonth = DateTime.DaysInMonth(CurrentSelectedDate.Year, CurrentSelectedDate.Month);
			int firstDayOfWeek = (int)new DateTime(CurrentSelectedDate.Year, CurrentSelectedDate.Month, 1).DayOfWeek;
			int totalDays = firstDayOfWeek + daysInMonth;

			// Create rows for each week + 1 extra row for the day labels
			int weeksInMonth = (int)Math.Ceiling(totalDays / 7.0);
			// Set a fixed height for the first row (the one containing the day labels)
			RowDefinitions.Add(new RowDefinition { Height = new GridLength(25) });

			for (int i = 1; i <= weeksInMonth; i++) // Start from 1 to skip the first row
			{
				RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
			}

			// Create columns for each day of the week
			for (int i = 0; i < 7; i++)
			{
				ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			}
		}

		private void GenerateDayLabels()
		{
			string[] dayLabels = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
			for (int day = 0; day < 7; day++)
			{
				var dayLabel = new Label { FontSize = _dateFontSize, FontAttributes = FontAttributes.Bold, Text = dayLabels[day], HorizontalTextAlignment = TextAlignment.Center };
				Grid.SetRow(dayLabel, 0);
				Grid.SetColumn(dayLabel, day);
				Children.Add(dayLabel);
			}
		}

		private void GenerateDateFrames()
		{
			int daysInMonth = DateTime.DaysInMonth(CurrentSelectedDate.Year, CurrentSelectedDate.Month);
			int firstDayOfWeek = (int)new DateTime(CurrentSelectedDate.Year, CurrentSelectedDate.Month, 1).DayOfWeek;

			for (int day = 1; day <= daysInMonth; day++)
			{
				var dateFrame = CreateDateFrame(day, firstDayOfWeek);
				int gridRow = (firstDayOfWeek + day - 1) / 7 + 1;
				int gridColumn = (firstDayOfWeek + day - 1) % 7;
				Grid.SetRow(dateFrame, gridRow);
				Grid.SetColumn(dateFrame, gridColumn);
				Children.Add(dateFrame);
			}
		}

		private Frame CreateDateFrame(int dayNumber, int firstDayOfWeek)
		{
			var tapGestureRecognizerForFrame = new TapGestureRecognizer
			{
				NumberOfTapsRequired = 2,
				Command = GoToSelectedDateCommand,
				CommandParameter = new DateTime(CurrentSelectedDate.Year, CurrentSelectedDate.Month, dayNumber)
			};
			var dateFrame = new Frame
			{
				BorderColor = _frameBorderColor,
				Padding = 5,
				BackgroundColor = _emptyLabelColor,
				MinimumWidthRequest = _minimumDayWidthRequest,
				MinimumHeightRequest = _minimumDayHeightRequest,
				CornerRadius = 0
			};
			dateFrame.GestureRecognizers.Add(tapGestureRecognizerForFrame);
			var stackLayout = new StackLayout();
			var dateLabel = new Label
			{
				FontSize = _dateFontSize,
				FontAttributes = FontAttributes.Bold,
				Text = dayNumber.ToString(),
				TextColor = _watermarkDateColor,
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.End
			};
			stackLayout.Children.Add(dateLabel);

			var dayEvents = EventsToShowList.Where(e => e.StartDateTime.Date == new DateTime(CurrentSelectedDate.Year, CurrentSelectedDate.Month, dayNumber)).ToObservableCollection();
			if (dayEvents.Count > 0)
			{
				var eventStackLayout = GenerateEventStackLayout(dayEvents, dayNumber);
				stackLayout.Children.Add(eventStackLayout);
			}

			dateFrame.Content = stackLayout;
			return dateFrame;
		}
		private StackLayout GenerateEventStackLayout(ObservableCollection<IGeneralEventModel> dayEvents, int dayNumber)
		{
			var stackLayout = new StackLayout();
			if (dayEvents.Count > _LimitMoreLabbelText)
			{
				var moreLabel = GenerateMoreEventsLabel(dayEvents.Count, dayNumber);
				stackLayout.Children.Add(moreLabel);
			}
			else if (dayEvents.Count > _LimitIconsToShow)
			{
				var moreLabel = GenerateMultiIconsEventLabel(dayEvents);
				stackLayout.Children.Add(moreLabel);
			}
			else
			{
				var eventItemsStackLayout = GenerateMultipleEventFrames(dayEvents);
				stackLayout.Children.Add(eventItemsStackLayout);
			}
			return stackLayout;
		}
		private StackLayout GenerateMultipleEventFrames(ObservableCollection<IGeneralEventModel> dayEvents)
		{
			var eventItemsStackLayout = new StackLayout
			{
				Orientation = StackOrientation.Vertical,
				Spacing = 2,
			};

			for (int i = 0; i < Math.Min(dayEvents.Count, _displayEventsLimit); i++)
			{
				var eventItem = dayEvents[i];

				var title = new Label
				{
					FontAttributes = FontAttributes.Bold,
					Text = eventItem.Title,
					LineBreakMode = LineBreakMode.TailTruncation,
				};

				var eventTypeLabel = new Label
				{
					Text = eventItem.EventType.MainEventType.SelectedVisualElement.ElementName,
					TextColor = eventItem.EventType.MainEventType.SelectedVisualElement.TextColor,
					Style = Styles.GoogleFontStyle,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
				};

				var eventTypeFrame = new Frame
				{
					BackgroundColor = eventItem.EventType.MainEventType.SelectedVisualElement.BackgroundColor,
					Padding = 0,
					Content = eventTypeLabel,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
					WidthRequest = 30,

				};

				var grid = new Grid
				{
					ColumnDefinitions =
					{
						new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
						new ColumnDefinition { Width = new GridLength(30) }
					},
					Children = { title, eventTypeFrame }
				};
				Grid.SetColumn(title, 0);
				Grid.SetColumn(eventTypeFrame, 1);

				var eventFrame = new Frame
				{
					BackgroundColor = eventItem.EventVisibleColor,
					Content = grid,
					Padding = 2,
					HasShadow = false,
				};
				eventItemsStackLayout.Children.Add(eventFrame);
			}

			return eventItemsStackLayout;
		}
		private FlexLayout GenerateMultiIconsEventLabel(ObservableCollection<IGeneralEventModel> dayEvents)
		{
			var flexItemLayout = new FlexLayout
			{
				Direction = FlexDirection.Row,
				JustifyContent = FlexJustify.Start,
				AlignItems = FlexAlignItems.Center,
				AlignContent = FlexAlignContent.Center,
				Wrap = FlexWrap.Wrap,
				Padding = 0,
				Margin = 0,
			};
			for (int i = 0; i < dayEvents.Count; i++)
			{
				var eventItem = dayEvents[i];
				var eventTypeLabel = new Label
				{
					Text = eventItem.EventType.MainEventType.SelectedVisualElement.ElementName,
					FontSize = _TextFontSize,
					TextColor = eventItem.EventType.MainEventType.SelectedVisualElement.TextColor,
					Style = Styles.GoogleFontStyle,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
				};

				var eventTypeFrame = new Frame
				{
					BackgroundColor = eventItem.EventType.MainEventType.SelectedVisualElement.BackgroundColor,
					Padding = 0,
					Content = eventTypeLabel,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
					WidthRequest = _EventIconFrameSize,
					HeightRequest = _EventIconFrameSize,

				};

				var grid = new Grid
				{
					ColumnDefinitions =
					{
						new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
					},
					Children = { eventTypeFrame }
				};

				var eventFrame = new Frame
				{
					BackgroundColor = eventItem.EventVisibleColor,
					Content = grid,
					Padding = 0,
					HasShadow = false,
				};
				flexItemLayout.Children.Add(eventFrame);
			}



			return flexItemLayout;
		}

		private Label GenerateMoreEventsLabel(int dayEventsCount, int dayOfMonth)
		{
			var moreLabel = new Label
			{
				FontSize = _TextFontSize + 5,
				FontAttributes = FontAttributes.Italic,
				Text = $"... {dayEventsCount} ...",
				TextColor = _eventTextColor,
				BackgroundColor = _moreEventsLabelColor,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};
			var tapGestureRecognizerForMoreEvents = new TapGestureRecognizer
			{
				Command = GoToSelectedDateCommand,
				CommandParameter = new DateTime(CurrentSelectedDate.Year, CurrentSelectedDate.Month, dayOfMonth)
			};
			moreLabel.GestureRecognizers.Add(tapGestureRecognizerForMoreEvents);
			return moreLabel;
		}
	}
}
