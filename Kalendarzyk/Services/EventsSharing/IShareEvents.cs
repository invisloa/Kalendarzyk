using Kalendarzyk.Models.EventModels;

namespace Kalendarzyk.Services.EventsSharing
{
	public interface IShareEvents
	{
		public Task ShareEventAsync(IGeneralEventModel eventModel);

		public Task ImportEventAsync(string jsonString);
	}
}
