using AdminService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminService.Repository
{
    public interface IIntegrationEventRepository
    {
        Task<IList<IntegrationEvent>> GetEvents();

        Task<IntegrationEvent> GetEventTracking(long eventId);

        Task<long> AddEvent(IntegrationEvent integrationEvent);

        Task<long> ModifyEvent(IntegrationEvent integrationEvent);

        Task<long> DeleteEvent(IntegrationEvent integrationEvent);
    }
}
