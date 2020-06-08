using AdminService.Context;
using AdminService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminService.Repository
{
    public class IntegrationEventRepository : IIntegrationEventRepository
    {
        private readonly AdminServiceContext _context;

        public IDbContextTransaction BeginTransaction()
        {
            return new object() as IDbContextTransaction;
        }

        public IntegrationEventRepository(AdminServiceContext context)
        {
            _context = context;
        }

        public async Task<IList<IntegrationEvent>> GetEvents()
        {
            return await _context.IntegrationEvents.AsNoTracking().ToListAsync();
        }

        public async Task<IntegrationEvent> GetEventTracking(long eventId)
        {
            return await _context.IntegrationEvents.FirstOrDefaultAsync(e => e.Id == eventId);
        }

        public async Task<long> AddEvent(IntegrationEvent integrationEvent)
        {
            if (integrationEvent == null)
            {
                return -1;
            }
            else
            {
                await _context.IntegrationEvents.AddAsync(integrationEvent);
                await _context.SaveChangesAsync();
                PropertyValues createdEvent = _context.Entry(integrationEvent).OriginalValues;
                long eventId = createdEvent.GetValue<long>("Id");
                return eventId;
            }
        }

        public async Task<long> ModifyEvent(IntegrationEvent integrationEvent)
        {
            if (integrationEvent == null)
            {
                return -1;
            }
            bool isExistingEvent = await _context.IntegrationEvents.AnyAsync(u => u.Id == integrationEvent.Id);
            if (!isExistingEvent)
            {
                return -2;
            }
            else
            {
                _context.IntegrationEvents.Update(integrationEvent);
                await _context.SaveChangesAsync();
                return 1;
            }
        }

        public async Task<long> DeleteEvent(IntegrationEvent integrationEvent)
        {
            if (integrationEvent == null)
            {
                return -1;
            }
            bool isExistingEvent = await _context.Users.AnyAsync(u => u.Id == integrationEvent.Id);
            if (!isExistingEvent)
            {
                return -2;
            }
            else
            {
                _context.IntegrationEvents.Remove(integrationEvent);
                await _context.SaveChangesAsync();
                return 1;
            }
        }
    }
}
