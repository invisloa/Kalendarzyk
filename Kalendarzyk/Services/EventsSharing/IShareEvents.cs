using Kalendarzyk.Models.EventModels;

namespace Kalendarzyk.Services.EventsSharing
{
	public interface IShareEventsService
	{
		public Task ImportEventsAsync(string jsonString);
		public Task ShareEventAsync(IGeneralEventModel eventModel);
	}
}
