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
			// file paths for json files
			EventsFilePath = Path.Combine(FileSystem.Current.AppDataDirectory, Preferences.Default.Get("JsonEventsFileName", "CalendarEventsD"));
			SubEventsTypesFilePath = Path.Combine(FileSystem.Current.AppDataDirectory, Preferences.Default.Get("JsonSubTypesFileName", "CalendarSubTypesOfEventsD"));
			MainEventsTypesFilePath = Path.Combine(FileSystem.Current.AppDataDirectory, Preferences.Default.Get("JsonMainTypesFileName", "CalendarMainTypesOfEventsD"));

			// Create directories if they do not exist
			CreateDirectoryIfNotExists(EventsFilePath);
			CreateDirectoryIfNotExists(SubEventsTypesFilePath);
			CreateDirectoryIfNotExists(MainEventsTypesFilePath);
			// only for testing purposes
			DeleteFileIfExists(EventsFilePath);
			DeleteFileIfExists(SubEventsTypesFilePath);
			DeleteFileIfExists(MainEventsTypesFilePath);

		}
		public void DeleteFileIfExists(string filePath)	// only for testing purposes
		{
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}
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
