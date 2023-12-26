using Kalendarzyk.Models.EventModels;

namespace Kalendarzyk.Services.EventsSharing
{
	public interface IShareEvents
	{
		public Task ImportEventAsync(string jsonString);
		public Task ShareEventAsync(IGeneralEventModel eventModel);
	}
}
