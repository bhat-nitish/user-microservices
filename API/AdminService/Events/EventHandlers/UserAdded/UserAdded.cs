using AdminService.Constants;
using AdminService.Entities;
using AdminService.Events.Data;
using AdminService.Repository;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AdminService.Events.EventHandlers.UserAdded
{
    public class UserAdded : IUserAdded
    {
        private readonly IAdminRepository _repository;

        private readonly IIntegrationEventRepository _eventRepository;

        public UserAdded(IAdminRepository repository, IIntegrationEventRepository eventRepository)
        {
            _repository = repository;
            _eventRepository = eventRepository;
        }

        public async Task HandleEvent(string data)
        {
            try
            {
                var newUserAdded = JsonConvert.DeserializeObject<NewUser>(data);
                if (newUserAdded != null)
                {
                    User newUser = new User()
                    {
                        DateOfBirth = newUserAdded.DateOfBirth,
                        EmailAddress = newUserAdded.Email,
                        UserName = newUserAdded.UserName,
                        UpdatedOn = DateTime.Now,
                        UpdatedBy = "admin",
                        CreatedBy = "admin",
                        CreatedOn = DateTime.Now
                    };
                    await AddUserReceivedEvent(newUser);
                    await _repository.AddUser(newUser);
                    await AddUserCreatedEvent(newUser);
                }
            }
            catch (Exception e)
            {
                throw;
            }

        }

        private async Task<bool> AddUserReceivedEvent(User user)
        {
            long response = await _eventRepository.AddEvent(new IntegrationEvent()
            {
                EventName = user.EmailAddress + " Received",
                EventStatus = EventConstants.EVENT_STATUS_RECEIVED,
                EventType = EventConstants.EVENT_TYPE_ADDED,
                UpdatedOn = DateTime.Now,
                UpdatedBy = "admin",
                CreatedBy = "admin",
                CreatedOn = DateTime.Now
            });
            return response != -1; // ideally if the integration event creation fails, this needs to be handled but would not be required for the sample app
        }

        private async Task<bool> AddUserCreatedEvent(User user)
        {
            long response = await _eventRepository.AddEvent(new IntegrationEvent()
            {
                EventName = user.EmailAddress + " Added",
                EventStatus = EventConstants.EVENT_STATUS_COMPLETED,
                EventType = EventConstants.EVENT_TYPE_ADDED,
                UpdatedOn = DateTime.Now,
                UpdatedBy = "admin",
                CreatedBy = "admin",
                CreatedOn = DateTime.Now
            });
            return response != -1; // ideally if the integration event creation fails, this needs to be handled but would not be required for the sample app
        }
    }
}
