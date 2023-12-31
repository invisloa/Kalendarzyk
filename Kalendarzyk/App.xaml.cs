﻿using CommunityToolkit.Mvvm.Messaging;
using Kalendarzyk.Helpers;
using Kalendarzyk.Models;
using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Models.EventTypesModels;
using Kalendarzyk.Services;
using Kalendarzyk.Services.DataOperations;
using Kalendarzyk.ViewModels;

namespace Kalendarzyk
{
	public partial class App : Application
	{
		IEventRepository _repository;
		public App()
		{
			_repository = Factory.CreateNewEventRepository();
			InitializeComponent();
			MainPage = new AppShell();

		}
		protected override async void OnStart()
		{
			// Call base method 
			base.OnStart();

			// Check or request StorageRead permission
			var statusStorageRead = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
			if (statusStorageRead != PermissionStatus.Granted)
			{
				statusStorageRead = await Permissions.RequestAsync<Permissions.StorageRead>();
			}
			var statusStorageWrite = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
			if (statusStorageWrite != PermissionStatus.Granted)
			{
				statusStorageWrite = await Permissions.RequestAsync<Permissions.StorageRead>();
			}
		}
		public static class Styles
		{
			public static Style GoogleFontStyle = new Style(typeof(Label))
			{
				Setters =
				{
					new Setter { Property = Label.FontFamilyProperty, Value = "GoogleMaterialFont" },
					new Setter { Property = Label.FontSizeProperty, Value = 32 }
				}
			};
		}
	}
}
