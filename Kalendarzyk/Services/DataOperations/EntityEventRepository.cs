/*using Kalendarzyk.Models.EventModels;
using Kalendarzyk.Services.DataOperations;
using Microsoft.EntityFrameworkCore;




namespace Kalendarzyk.Services.DataOperations
{
    public class EntityEventRepository : IEventRepository
	{
		private readonly EventDbContext _context;

		public EntityEventRepository(EventDbContext context)
		{
			_context = context;
		}

		public async Task<List<IGeneralEventModel>> GetEventsListAsync()
		{
			return await _context.Events.ToListAsync();
		}

		public async Task SaveEventsListAsync()
		{
			await _context.SaveChangesAsync();
		}

		public async Task DeleteFromEventsListAsync(IGeneralEventModel eventToDelete)
		{
			_context.Events.Remove(eventToDelete);
			await _context.SaveChangesAsync();
		}

		public async Task AddEventAsync(IGeneralEventModel eventToAdd)
		{
			await _context.Events.AddAsync(eventToAdd);
			await _context.SaveChangesAsync();
		}

		public async Task ClearEventsListAsync()
		{
			_context.Events.RemoveRange(_context.Events);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateEventAsync(IGeneralEventModel eventToUpdate)
		{
			_context.Events.Update(eventToUpdate);
			await _context.SaveChangesAsync();
		}
		public async Task<IGeneralEventModel> GetEventByIdAsync(Guid eventId)
		{
			var selectedEvent = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId);
			return selectedEvent;
		}

	}
}
*/