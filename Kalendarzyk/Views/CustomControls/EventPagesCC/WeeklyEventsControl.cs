﻿namespace Kalendarzyk.Views.CustomControls
{
	using Kalendarzyk.Models.EventModels;
	using Kalendarzyk.Services;
	using Microsoft.Maui.Controls;
	using System;
	using System.Linq;
	using static Kalendarzyk.App;

	public class WeeklyEventsControl : BaseEventPageCC
	{
		private readonly int _minimumDayOfWeekWidthRequest = 45;
		private readonly int _minimumDayOfWeekHeightRequest = 35;
		private readonly double _firstColumnForHoursWidth = 35;
		private int _hoursSpanFrom;
		private int _hoursSpanTo;

		public WeeklyEventsControl()
		{
			SetCorrectHourlySpanTimes();
		}
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
			int extraColumnForLessThen = _hoursSpanFrom > 0 ? 1 : 0;
			int extraColumnForMoreThen = _hoursSpanTo < 23 ? 1 : 0;
			int totalColumns = _hoursSpanTo - _hoursSpanFrom + extraColumnForLessThen + extraColumnForMoreThen + 1;
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
			if (_hoursSpanFrom > 0)
			{
				Label beforeLabel;
				if (_hoursSpanFrom == 1)
				{
					beforeLabel = new Label { FontSize = 12, FontAttributes = FontAttributes.Bold, Text = "00" };
				}
				else
				{
					beforeLabel = new Label { FontSize = 12, FontAttributes = FontAttributes.Bold, Text = "<" + _hoursSpanFrom.ToString("D2") };
				}
				Grid.SetRow(beforeLabel, 0);
				Grid.SetColumn(beforeLabel, currentColumnIndex++);
				Children.Add(beforeLabel);
			}
			// Hour Header labels
			for (int hour = _hoursSpanFrom; hour <= _hoursSpanTo; hour++)
			{
				var hourLabel = new Label { FontSize = 12, FontAttributes = FontAttributes.Bold, Text = hour.ToString("D2") };
				Grid.SetRow(hourLabel, 0); // Header row
				Grid.SetColumn(hourLabel, currentColumnIndex++);
				Children.Add(hourLabel);
			}
			currentColumnIndex++;
			// Label for time after _hoursSpanTo
			if (_hoursSpanTo < 23)
			{
				var afterLabel = new Label { FontSize = 12, FontAttributes = FontAttributes.Bold, Text = ">" + (_hoursSpanTo + 1).ToString("D2") };
				Grid.SetRow(afterLabel, 0); // Header row
				Grid.SetColumn(afterLabel, currentColumnIndex);
				Children.Add(afterLabel);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hour"></param> 
		/// <remarks> hour special case: -1 for before span, -2 for after span</remarks>
		/// <param name="dayOfWeek"></param>
		/// <returns></returns>
		private Frame DrawHourFrame(int hour, int dayOfWeek)
		{
			var date = CalculateFrameDate(dayOfWeek);
			var frame = DrawSingleFrame(date);
			return frame;
		}
		private Frame DrawSingleFrame(DateTime date)
		{
			var frame = new Frame
			{
				BorderColor = _frameBorderColor,
				Padding = 5,
				CornerRadius = 2,
				BackgroundColor = Colors.Transparent,
				MinimumWidthRequest = _minimumDayOfWeekWidthRequest,
				MinimumHeightRequest = _minimumDayOfWeekHeightRequest
			};

			var tapGestureRecognizerForFrame = new TapGestureRecognizer
			{
				NumberOfTapsRequired = 2,
				Command = GoToSelectedDateCommand,
				CommandParameter = date
			};
			frame.GestureRecognizers.Add(tapGestureRecognizerForFrame);

			return frame;
		}


		private Frame DrawEventFrame(IGeneralEventModel eventItem)
		{
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

			// to do in the future (there is no touch effect in maui yet) 
			// add touch effect to the frame that will go to the event details page

			var tapGestureRecognizerGoToDate = new TapGestureRecognizer
			{
				NumberOfTapsRequired = 2,
				Command = GoToSelectedDateCommand,
				CommandParameter = eventItem.StartDateTime
			};

			eventTypeFrame.GestureRecognizers.Add(tapGestureRecognizerGoToDate);
			eventFrame.GestureRecognizers.Add(tapGestureRecognizerGoToDate);
			grid.GestureRecognizers.Add(tapGestureRecognizerGoToDate);
			return eventFrame;
		}


		private IEnumerable<IGeneralEventModel> GetHourEvents(int hour, DateTime date)
		{
			if (hour == -1)
			{
				return EventsToShowList.Where(e => e.StartDateTime.Date == date && e.StartDateTime.Hour < _hoursSpanFrom);
			}
			if (hour == -2)
			{
				return EventsToShowList.Where(e => e.StartDateTime.Date == date && e.StartDateTime.Hour >= _hoursSpanTo);
			}
			return EventsToShowList.Where(e => e.StartDateTime.Date == date && e.StartDateTime.Hour == hour);
		}
		private void AddEventsToGrid(IEnumerable<IGeneralEventModel> hourlyEvents, int dayOfWeek)
		{
			if (hourlyEvents.Any())
			{
				//Add events to the grid as buttons
				var eventsCount = hourlyEvents.Count();
				if (eventsCount > _displayEventsLimit)
				{
					Children.Add(GenerateMoreButton(eventsCount, dayOfWeek));
				}
				else
				{
					var StackLayoutitem = new StackLayout();

					foreach (var eventModel in hourlyEvents)
					{
						var eventFrame = DrawEventFrame(eventModel);

						Grid.SetRow(StackLayoutitem, dayOfWeek + 1); // Offset by 1 to account for the header row
						Grid.SetColumn(StackLayoutitem, eventModel.StartDateTime.Hour + 2 - _hoursSpanFrom); // Offset by 2 to account for the day labels column and "before" column
																											 //var eventTime = eventModel.EndDateTime.Hour - eventModel.StartDateTime.Hour;
																											 //if (eventTime > 1)
																											 //{
																											 //	Grid.SetColumnSpan(StackLayoutitem, eventTime);
																											 //}
						StackLayoutitem.Add(eventFrame);

					}
					Children.Add(StackLayoutitem);

				}
			}
		}
		private Label GenerateMoreButton(int dayEventsCount, int dayOfWeek)     // Consider if use this at all or just set no limit for number of events??
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
		private void GenerateEventFrames() // to make the events span across multiple hours maybe try to use new grid placed above the main grid?? todo in the future
		{
			// Create frames for each hour and day
			for (int dayOfWeek = 0; dayOfWeek < 7; dayOfWeek++)
			{
				// FOR BEFORE SPAN COLUMN
				var frame = DrawHourFrame(-1, dayOfWeek); // Adjust hour for CreateEventFrame
				Grid.SetRow(frame, dayOfWeek + 1); // Offset by 1 to account for the header row
				Grid.SetColumn(frame, -1 + 2); // Offset by 2 to account for the day labels column and "before" column

				Children.Add(frame);

				var eventsInBeforeSpan = GetHourEvents(-1, CalculateFrameDate(dayOfWeek));
				AddEventsToGrid(eventsInBeforeSpan, dayOfWeek);



				// For normal hours span Columns
				for (int hour = _hoursSpanFrom; hour <= _hoursSpanTo; hour++)
				{
					frame = DrawHourFrame(hour, dayOfWeek); // Adjust hour for CreateEventFrame
					Grid.SetRow(frame, dayOfWeek + 1); // Offset by 1 to account for the header row
					Grid.SetColumn(frame, hour + 2 - _hoursSpanFrom); // Offset by 2 to account for the day labels column and "before" column
					Children.Add(frame);
					//Add events for Normal span
					var eventsInHour = GetHourEvents(hour, CalculateFrameDate(dayOfWeek));
					AddEventsToGrid(eventsInHour, dayOfWeek);
				}

				// FOR AFTER SPAN COLUMN
				frame = DrawHourFrame(-2, dayOfWeek); // Adjust hour for CreateEventFrame
				Grid.SetRow(frame, dayOfWeek + 1); // Offset by 1 to account for the header row
				Grid.SetColumn(frame, _hoursSpanTo - _hoursSpanFrom + 3); // Offset by 2 to account for the day labels column and "before" column
				Children.Add(frame);
				//Addevents for after span
				var eventsInAfterSpan = GetHourEvents(-2, CalculateFrameDate(dayOfWeek));
				AddEventsToGrid(eventsInAfterSpan, dayOfWeek);

			}
		}

		private DateTime CalculateFrameDate(int dayOfWeek)
		{
			return CurrentSelectedDate.AddDays(dayOfWeek - (int)CurrentSelectedDate.DayOfWeek).Date;
		}

		private void SetCorrectHourlySpanTimes()
		{
			_hoursSpanFrom = PreferencesManager.GetHoursSpanFrom() > 0 && PreferencesManager.GetHoursSpanFrom() < 23 ? PreferencesManager.GetHoursSpanFrom() : 0;
			_hoursSpanTo = PreferencesManager.GetHoursSpanTo() < 23 && PreferencesManager.GetHoursSpanTo() > 1 ? PreferencesManager.GetHoursSpanTo() : 23;

			_hoursSpanFrom = _hoursSpanFrom <= _hoursSpanTo ? _hoursSpanFrom : 0;
		}
	}
}