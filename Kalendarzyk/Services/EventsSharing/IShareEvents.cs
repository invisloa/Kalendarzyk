using Kalendarzyk.Models.EventModels;

namespace Kalendarzyk.Services.EventsSharing
{
	public interface IShareEventsService
	{
		public Task ImportEventAsync(string jsonString);
		public Task ShareEventAsync(IGeneralEventModel eventModel);
	}
}
