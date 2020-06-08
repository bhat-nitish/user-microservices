using System.Collections.Generic;
using System.Threading.Tasks;
using UsersService.Entities;

namespace UsersService.Repository
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
