namespace Kalendarzyk.Views.CustomControls
{
	using Kalendarzyk.Models.EventModels;
	using Kalendarzyk.Services;
	using Kalendarzyk.Views.CustomControls;
	using Microsoft.Maui.Controls;
	using System;
	using System.Collections.ObjectModel;
	using System.Linq;
	using static Kalendarzyk.App;
	using MauiGrid = Microsoft.Maui.Controls.Grid;

	public class WeeklyEventsControl : BaseEventPageCC
	{
		private readonly int _minimumDayOfWeekWidthRequest = 45;
		private readonly int _minimumDayOfWeekHeightRequest = 30;
		private readonly double _firstColumnForHoursWidth = 35;
		private int _hoursSpanFrom = PreferencesManager.GetHoursSpanFrom();
		private int _hoursSpanTo = PreferencesManager.GetHoursSpanTo();
		private ScrollView _scrollView;
		private MauiGrid _grid;
		public void GenerateGrid()
		{
			ClearGrid();
			GenerateDayLabels();
			GenerateHourLabels();
			GenerateEventFrames();
		}

		private void ClearGrid()
		{
			RowDefinitions.Clear();
			ColumnDefinitions.Clear();
			Children.Clear();

			// Set up column definitions for hours including before and after spans
			int totalColumns = _hoursSpanTo - _hoursSpanFrom + 3;
			ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(_firstColumnForHoursWidth) });
			for (int i = 0; i < totalColumns; i++)
				ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

			// Set up row definitions for days of the week
			for (int i = 0; i < 8; i++) // 7 days + 1 for the header
				RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
		}

		private void GenerateDayLabels()
		{
			// Add day labels as the first column
			for (int day = 0; day < 7; day++)
			{
				var dayOfWeekLabel = new Label
				{
					FontSize = _dayNamesFontSize,
					FontAttributes = FontAttributes.Bold,
					Text = $"{((DayOfWeek)day).ToString().Substring(0, 3)} {CurrentSelectedDate.AddDays(day - (int)CurrentSelectedDate.DayOfWeek).ToString("dd")}",
					HorizontalOptions = LayoutOptions.Center
				};
				Grid.SetRow(dayOfWeekLabel, day + 1); // Offset by 1 to allow for the header row
				Grid.SetColumn(dayOfWeekLabel, 0);
				Children.Add(dayOfWeekLabel);
			}
		}

		private void GenerateHourLabels()
		{
			// Add header label for hours
			var headerLabel = new Label { FontSize = 12, FontAttributes = FontAttributes.Bold, Text = "Hour", WidthRequest = _firstColumnForHoursWidth };
			Grid.SetRow(headerLabel, 0);
			Grid.SetColumn(headerLabel, 0);
			Children.Add(headerLabel);

			// Hour labels for the specified time span as columns
			int currentColumnIndex = 1; // Start from the second column

			// Label for time before _hoursSpanFrom
			var beforeLabel = new Label { FontSize = 12, FontAttributes = FontAttributes.Bold, Text = "<" + _hoursSpanFrom.ToString("D2") };
			Grid.SetRow(beforeLabel, 0);
			Grid.SetColumn(beforeLabel, currentColumnIndex++);
			Children.Add(beforeLabel);

			// Hour labels
			for (int hour = _hoursSpanFrom; hour <= _hoursSpanTo; hour++)
			{
				var hourLabel = new Label { FontSize = 12, FontAttributes = FontAttributes.Bold, Text = hour.ToString("D2") };
				Grid.SetRow(hourLabel, 0); // Header row
				Grid.SetColumn(hourLabel, currentColumnIndex++);
				Children.Add(hourLabel);
			}
			currentColumnIndex++;
			// Label for time after _hoursSpanTo
			var afterLabel = new Label { FontSize = 12, FontAttributes = FontAttributes.Bold, Text = ">" + _hoursSpanTo.ToString("D2") };
			Grid.SetRow(afterLabel, 0); // Header row
			Grid.SetColumn(afterLabel, currentColumnIndex);
			Children.Add(afterLabel);
		}

		private Frame CreateEventFrame(int hour, int dayOfWeek)
		{
			// Calculate the date for the current column
			var date = CurrentSelectedDate.AddDays(dayOfWeek - (int)CurrentSelectedDate.DayOfWeek).Date;
			IEnumerable<IGeneralEventModel> dayEvents;

			// Special case for hours before _hoursSpanFrom
			if (hour == -1)
			{
				dayEvents = EventsToShowList.Where(e => e.StartDateTime.Date == date && e.StartDateTime.Hour < _hoursSpanFrom);
			}
			// Special case for hours after _hoursSpanTo
			else if (hour == -2)
			{
				dayEvents = EventsToShowList.Where(e => e.StartDateTime.Date == date && e.StartDateTime.Hour >= _hoursSpanTo);
			}
			// Normal case for hours within the _hoursSpanFrom and _hoursSpanTo
			else
			{
				dayEvents = EventsToShowList.Where(e => e.StartDateTime.Date == date && e.StartDateTime.Hour == hour);
			}

			var frame = new Frame { BorderColor = _frameBorderColor, Padding = 5, BackgroundColor = _emptyLabelColor, MinimumWidthRequest = _minimumDayOfWeekWidthRequest, MinimumHeightRequest = _minimumDayOfWeekHeightRequest };

			// If there are any events for this hour and dayOfWeek, create a stack layout for them
			if (dayEvents.Any())
			{
				var stackLayout = GenerateEventStackLayout(dayEvents.ToList(), dayOfWeek);
				frame.Content = stackLayout;
			}

			return frame;
		}
		private StackLayout GenerateEventStackLayout(List<IGeneralEventModel> dayEvents, int dayOfWeek)
		{
			var stackLayout = new StackLayout();
			if (dayEvents.Count > _displayEventsLimit)
			{
				var moreLabel = GenerateMoreEventsLabel(dayEvents.Count, dayOfWeek);
				stackLayout.Children.Add(moreLabel);
			}
			else
			{
				var eventItemsStackLayout = GenerateMultipleEventFrames(dayEvents);
				stackLayout.Children.Add(eventItemsStackLayout);
			}
			return stackLayout;
		}
		private Label GenerateMoreEventsLabel(int dayEventsCount, int dayOfWeek)
		{
			var moreLabel = new Label
			{
				FontSize = 15,
				FontAttributes = FontAttributes.Italic,
				Text = $"... {dayEventsCount} ...",
				TextColor = _eventTextColor,
				BackgroundColor = _moreEventsLabelColor
			};
			var tapGestureRecognizerForMoreEvents = new TapGestureRecognizer
			{
				Command = GoToSelectedDateCommand,
				CommandParameter = CurrentSelectedDate.AddDays(dayOfWeek - (int)CurrentSelectedDate.DayOfWeek)
			};
			moreLabel.GestureRecognizers.Add(tapGestureRecognizerForMoreEvents);
			return moreLabel;
		}
		private void GenerateEventFrames()
		{
			// Create frames for each hour and day
			for (int dayOfWeek = 0; dayOfWeek < 7; dayOfWeek++)
			{
				// FOR BEFORE SPAN COLUMN
				var frame = CreateEventFrame(-1, dayOfWeek); // Adjust hour for CreateEventFrame
				Grid.SetRow(frame, dayOfWeek + 1); // Offset by 1 to account for the header row
				Grid.SetColumn(frame, -1 + 2); // Offset by 2 to account for the day labels column and "before" column
				Children.Add(frame);

				for (int hour = _hoursSpanFrom; hour <= _hoursSpanTo; hour++)
				{
					frame = CreateEventFrame(hour, dayOfWeek); // Adjust hour for CreateEventFrame
					Grid.SetRow(frame, dayOfWeek + 1); // Offset by 1 to account for the header row
					Grid.SetColumn(frame, hour + 2 - _hoursSpanFrom); // Offset by 2 to account for the day labels column and "before" column
					Children.Add(frame);
				}

				// FOR AFTER SPAN COLUMN
				frame = CreateEventFrame(-2, dayOfWeek); // Adjust hour for CreateEventFrame
				Grid.SetRow(frame, dayOfWeek + 1); // Offset by 1 to account for the header row
				Grid.SetColumn(frame, _hoursSpanTo - _hoursSpanFrom + 3); // Offset by 2 to account for the day labels column and "before" column
				Children.Add(frame);
			}
		}

		private StackLayout GenerateMultipleEventFrames(List<IGeneralEventModel> dayEvents)
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
					Margin = new Thickness(5, 0, 0, 0),
					Text = eventItem.Title,
					LineBreakMode = LineBreakMode.TailTruncation,
				};

				var eventTypeLabel = new Label
				{
					Text = eventItem.EventType.MainEventType.SelectedVisualElement.ElementName,
					FontSize = 20,
					TextColor = eventItem.EventType.MainEventType.SelectedVisualElement.TextColor,
		//TODOJO			Style = Styles.GoogleFontStyle,
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
					WidthRequest = 30
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
					Padding = 0,
					HasShadow = false,
				};

				var tapGestureRecognizer = new TapGestureRecognizer
				{
					Command = EventSelectedCommand,
					CommandParameter = eventItem
				};

				eventFrame.GestureRecognizers.Add(tapGestureRecognizer);
				eventItemsStackLayout.Children.Add(eventFrame);
			}

			return eventItemsStackLayout;
		}

	}
}