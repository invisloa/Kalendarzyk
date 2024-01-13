namespace Kalendarzyk.Services.DataOperations
{
	internal interface ILocalFilePathService
	{
		string EventsFilePath { get; }
		string MainEventsTypesFilePath { get; }
		string SubEventsTypesFilePath { get; }
	}
}