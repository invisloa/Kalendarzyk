﻿using CommunityToolkit.Mvvm.ComponentModel;
using Kalendarzyk.Services;
using System.Windows.Input;

namespace Kalendarzyk.Views.CustomControls.CCViewModels
{
	internal partial class IsNotificationCCViewModel : ObservableObject
	{
		[ObservableProperty]
		private bool _isNotification;
		[ObservableProperty]
		private string _notificationSelectionText;

		[ObservableProperty]
		private DefaultTimespanCCViewModel _defaultEventTimespanCCViewModel = Factory.CreateNewDefaultEventTimespanCCHelperClass();

		[ObservableProperty]
		private ICommand _isNotificationFrameSelectionCommand;

		public TimeSpan NotificationTime => _defaultEventTimespanCCViewModel.GetDuration();


		public IsNotificationCCViewModel()
		{
			_isNotification = false;
			_notificationSelectionText = "Notification";
			_isNotificationFrameSelectionCommand = new Command(ChangeIsNotification);
		}

		private void ChangeIsNotification()
		{
			IsNotification = !IsNotification;
		}
	}
}
