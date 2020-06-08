using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using UsersService.Constants;
using UsersService.Entities;
using UsersService.Events.Data;
using UsersService.Repository;

namespace UsersService.Events.UserNotificationReceived
{
    public class UserNotificationReceived : IUserNotificationReceived
    {
        private readonly IUserRepository _repository;

        private readonly IIntegrationEventRepository _eventRepository;

        public UserNotificationReceived(IUserRepository repository, IIntegrationEventRepository eventRepository)
        {
            _repository = repository;
            _eventRepository = eventRepository;
        }

        public async Task HandleEvent(string data)
        {
            try
            {
                var newNotification = JsonConvert.DeserializeObject<NewUserNotification>(data);
                if (newNotification != null)
                {
                    UserNotification newNotif = new UserNotification()
                    {
                        Email = newNotification.Email,
                        Notification = newNotification.Notification,
                        NotificationStatus = "CREATED",
                        UpdatedOn = DateTime.Now,
                        UpdatedBy = "user",
                        CreatedBy = "user",
                        CreatedOn = DateTime.Now
                    };
                    await AddUserNotificationReceivedEvent(newNotif);
                    await _repository.AddNotification(newNotif);
                    await AddUserNotificationCreatedEvent(newNotif);
                }
            }
            catch (Exception e)
            {
                throw;
            }

        }

        private async Task<bool> AddUserNotificationReceivedEvent(UserNotification notification)
        {
            long response = await _eventRepository.AddEvent(new IntegrationEvent()
            {
                EventName = notification.Email + " Notification Received",
                EventStatus = EventConstants.EVENT_STATUS_RECEIVED,
                EventType = EventConstants.EVENT_TYPE_ADDED,
                UpdatedOn = DateTime.Now,
                UpdatedBy = "user",
                CreatedBy = "user",
                CreatedOn = DateTime.Now
            });
            return response != -1; // ideally if the integration event creation fails, this needs to be handled but would not be required for the sample app
        }

        private async Task<bool> AddUserNotificationCreatedEvent(UserNotification notification)
        {
            long response = await _eventRepository.AddEvent(new IntegrationEvent()
            {
                EventName = notification.Email + "Notification Added",
                EventStatus = EventConstants.EVENT_STATUS_COMPLETED,
                EventType = EventConstants.EVENT_TYPE_ADDED,
                UpdatedOn = DateTime.Now,
                UpdatedBy = "user",
                CreatedBy = "user",
                CreatedOn = DateTime.Now
            });
            return response != -1; // ideally if the integration event creation fails, this needs to be handled but would not be required for the sample app
        }
    }
}
