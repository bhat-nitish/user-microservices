using AdminService.Constants;
using AdminService.Entities;
using AdminService.Events.Data;
using AdminService.Repository;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AdminService.Events.EventHandlers.UserModified
{
    public class UserModified : IUserModified
    {
        private readonly IAdminRepository _repository;

        private readonly IIntegrationEventRepository _eventRepository;

        public UserModified(IAdminRepository repository, IIntegrationEventRepository eventRepository)
        {
            _repository = repository;
            _eventRepository = eventRepository;
        }

        public async Task HandleEvent(string data)
        {
            try
            {
                var userModified = JsonConvert.DeserializeObject<ModifiedUser>(data);
                if (userModified != null)
                {
                    User user = await _repository.GetUserTracking(userModified.Email);
                    if (user != null)
                    {
                        user.UserName = userModified.UserName;
                        user.UpdatedOn = DateTime.Now;
                        user.UpdatedBy = "admin";
                        await AddUserModificationReceivedEvent(user);
                        await AddUserModifiedEvent(user);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }

        }

        private async Task<bool> AddUserModificationReceivedEvent(User user)
        {
            long response = await _eventRepository.AddEvent(new IntegrationEvent()
            {
                EventName = user.EmailAddress + " Received",
                EventStatus = EventConstants.EVENT_STATUS_RECEIVED,
                EventType = EventConstants.EVENT_TYPE_MODIFIED,
                UpdatedOn = DateTime.Now,
                UpdatedBy = "admin",
                CreatedBy = "admin",
                CreatedOn = DateTime.Now
            });
            return response != -1; // ideally if the integration event creation fails, this needs to be handled but would not be required for the sample app
        }

        private async Task<bool> AddUserModifiedEvent(User user)
        {
            long response = await _eventRepository.AddEvent(new IntegrationEvent()
            {
                EventName = user.EmailAddress + " Modified",
                EventStatus = EventConstants.EVENT_STATUS_COMPLETED,
                EventType = EventConstants.EVENT_TYPE_MODIFIED,
                UpdatedOn = DateTime.Now,
                UpdatedBy = "admin",
                CreatedBy = "admin",
                CreatedOn = DateTime.Now
            });
            return response != -1; // ideally if the integration event creation fails, this needs to be handled but would not be required for the sample app
        }
    }
}
