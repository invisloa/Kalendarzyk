using System;
using System.IO;

namespace Kalendarzyk.Services.DataOperations
{
	internal class LocalFilePathService : ILocalFilePathService
	{
		public string EventsFilePath { get; }
		public string SubEventsTypesFilePath { get; }
		public string MainEventsTypesFilePath { get; }

		public LocalFilePathService()
		{
			EventsFilePath = Path.Combine(FileSystem.Current.AppDataDirectory, Preferences.Default.Get("JsonEventsFileName", "CalendarEventsD"));
			SubEventsTypesFilePath = Path.Combine(FileSystem.Current.AppDataDirectory, Preferences.Default.Get("JsonSubTypesFileName", "CalendarSubTypesOfEventsD"));
			MainEventsTypesFilePath = Path.Combine(FileSystem.Current.AppDataDirectory, Preferences.Default.Get("JsonMainTypesFileName", "CalendarMainTypesOfEventsD"));

			// Create directories if they do not exist
			CreateDirectoryIfNotExists(EventsFilePath);
			CreateDirectoryIfNotExists(SubEventsTypesFilePath);
			CreateDirectoryIfNotExists(MainEventsTypesFilePath);
		}

		private void CreateDirectoryIfNotExists(string filePath)
		{
			var directoryPath = Path.GetDirectoryName(filePath);
			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}
		}
	}
}
