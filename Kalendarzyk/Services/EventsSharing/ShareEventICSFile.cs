﻿using Kalendarzyk.Models.EventModels;

namespace Kalendarzyk.Services.EventsSharing
{
	internal class ShareEventICSFile : IShareEventsService
	{
		public Task ImportEventAsync(string jsonString)
		{
			throw new NotImplementedException();
		}

		public Task ShareEventAsync(IGeneralEventModel eventModel)
		{
			throw new NotImplementedException();
		}
	}
}