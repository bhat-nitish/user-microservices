using AdminService.Constants;
using AdminService.Dto;
using AdminService.Entities;
using AdminService.Events.EventHandlers.UserNotificationAdded;
using AdminService.Helpers;
using AdminService.Repository;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace AdminService.Managers
{
    public class AdminManager : IAdminManager
    {
        private readonly IAdminRepository _repository;

        private readonly IIntegrationEventRepository _eventRepository;

        private readonly IUserNotificationAdded _notificationPublisher;

        public AdminManager(IAdminRepository repository, IIntegrationEventRepository eventRepository, IUserNotificationAdded notificationPublisher)
        {
            _repository = repository;
            _eventRepository = eventRepository;
            _notificationPublisher = notificationPublisher;
        }

        public async Task<IReadOnlyCollection<UserDto>> GetUsers()
        {
            IList<User> users = await _repository.GetUsers();
            return users.Select(u => new UserDto()
            {
                Id = u.Id,
                DateOfBirth = u.DateOfBirth,
                Email = u.EmailAddress,
                UserName = u.UserName,
                Age = AgeHelper.GetFullAge(u.DateOfBirth)
            }).ToImmutableList();
        }

        private async Task<AdminResponseDto> ValidateNotification(NotifyUserDto notification)
        {
            AdminResponseDto response = new AdminResponseDto();

            bool isExistingUser = await _repository.IsExistingUser(notification.Email);
            if (!isExistingUser)
            {
                response.Error = new ErrorDto() { Message = "User does not exist" };
                return response;
            }
            if (string.IsNullOrWhiteSpace(notification.Notification) || notification.Notification.Length > 200)
            {
                response.Error = new ErrorDto() { Message = "Invalid User Name. User Name should not be null and should be less than or equal to 200 characters." };
                return response;
            }
            response.Success = true;
            return response;
        }

        public async Task<AdminResponseDto> NotifyUser(NotifyUserDto notification)
        {
            AdminResponseDto response = new AdminResponseDto();
            bool notificationAdded = false;
            try
            {
                response = await ValidateNotification(notification);
                if (!response.Success)
                    return response;

                UserNotification notif = new UserNotification()
                {
                    Email = notification.Email,
                    Notification = notification.Notification,
                    NotificationStatus = "CREATED",
                    UpdatedOn = DateTime.Now,
                    UpdatedBy = "admin",
                    CreatedBy = "admin",
                    CreatedOn = DateTime.Now
                };
                using (IDbContextTransaction transaction = _repository.BeginTransaction())
                {
                    long notifId = await _repository.AddNotification(notif);
                    if (notifId > 0)
                    {
                        await AddNotificationAdditionEvent(notif);
                        transaction.Commit();
                        notificationAdded = true;
                        response.Id = notifId;
                        response.Success = true;
                        response.Message = $"Notification sent to the user.";
                    }
                }
                if (notificationAdded)
                {
                    // add the event published to the integration events table - this can help as an event tracker/history table
                    _notificationPublisher.PublishUserNotification(new Events.Data.NewUserNotification() { Email = notif.Email, Notification = notif.Notification });
                    await AddNotificationDispatchEvent(notif);
                }
            }
            catch (Exception e)
            {
                response.Error = new ErrorDto() { Message = "System Error : Please contact administrator" };
                response.Success = false;
                throw;
            }
            return response;
        }

        private async Task<bool> AddNotificationAdditionEvent(UserNotification notification)
        {
            long response = await _eventRepository.AddEvent(new IntegrationEvent()
            {
                EventName = notification.Email + " Notification Added",
                EventStatus = EventConstants.EVENT_STATUS_CREATED,
                EventType = EventConstants.EVENT_TYPE_ADDED,
                UpdatedOn = DateTime.Now,
                UpdatedBy = "admin",
                CreatedBy = "admin",
                CreatedOn = DateTime.Now
            });
            return response != -1; // ideally if the integration event creation fails, this needs to be handled but would not be required for the sample app
        }

        private async Task<bool> AddNotificationDispatchEvent(UserNotification notification)
        {
            long response = await _eventRepository.AddEvent(new IntegrationEvent()
            {
                EventName = notification.Email + " Notification Added",
                EventStatus = EventConstants.EVENT_STATUS_DISPATCHED,
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
