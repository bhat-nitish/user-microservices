using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using UsersService.Constants;
using UsersService.Dto;
using UsersService.Entities;
using UsersService.Events;
using UsersService.Events.Data;
using UsersService.Helpers;
using UsersService.Repository;

namespace UsersService.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _repository;

        private readonly IIntegrationEventRepository _eventsRepository;

        private readonly IUserAdded _newUserPublisher;

        private readonly IUserModified _updateUserPublisher;

        private readonly IUserDeleted _deleteUserPublisher;

        public UserManager(IUserRepository repository, IIntegrationEventRepository eventsRepository, IUserAdded newUserPublisher, IUserModified updateUserPublisher, IUserDeleted deleteUserPublisher)
        {
            _repository = repository;
            _newUserPublisher = newUserPublisher;
            _eventsRepository = eventsRepository;
            _updateUserPublisher = updateUserPublisher;
            _deleteUserPublisher = deleteUserPublisher;
        }

        public async Task<IReadOnlyCollection<ViewUserDto>> GetUsers()
        {
            IList<User> users = await _repository.GetUsers();
            IList<UserNotification> userNotifications = await _repository.GetUserNotifications();
            return users.Select(u => new ViewUserDto()
            {
                Id = u.Id,
                DateOfBirth = u.DateOfBirth,
                Email = u.EmailAddress,
                UserName = u.UserName,
                Age = AgeHelper.GetFullAge(u.DateOfBirth),
                Notifications = userNotifications.
                                Where(n => string.Compare(n.Email, u.EmailAddress, true) == 0).
                                Take(5).
                                Select(n => new ViewNotificationDto()
                                {
                                    CreatedDate = n.CreatedOn,
                                    Notification = n.Notification
                                })
                               .ToList()
            }).ToImmutableList();
        }

        private async Task<UserResponseDto> ValidateNewUser(AddUserDto user)
        {
            UserResponseDto response = new UserResponseDto();

            bool isValidEmail = EmailValidationHelper.IsValidEmail(user.Email);
            if (!isValidEmail)
            {
                response.Error = new ErrorDto() { Message = "Invalid Email. Please check the email address." };
                return response;
            }
            bool isDuplicateUser = await _repository.IsExistingUser(user.Email);
            if (isDuplicateUser)
            {
                response.Error = new ErrorDto() { Message = "Email already exists. Please select a different email." };
                return response;
            }
            if (user.DateOfBirth == null)
            {
                response.Error = new ErrorDto() { Message = "Date Of Birth cannot be null. Please select a valid date of birth." };
                return response;
            }
            if (string.IsNullOrWhiteSpace(user.UserName) || user.UserName.Length > 20)
            {
                response.Error = new ErrorDto() { Message = "Invalid User Name. User Name should not be null and should be less than or equal to 20 characters." };
                return response;
            }
            response.Success = true;
            return response;
        }

        private async Task<UserResponseDto> ValidateUpdateUser(UpdateUserDto user, long userId)
        {
            UserResponseDto response = new UserResponseDto();

            bool isExistingUser = await _repository.IsExistingUser(userId);
            if (!isExistingUser)
            {
                response.Error = new ErrorDto() { Message = "User Not Found." };
                return response;
            }
            if (string.IsNullOrWhiteSpace(user.UserName) || user.UserName.Length > 20)
            {
                response.Error = new ErrorDto() { Message = "Invalid User Name. User Name should not be null and should be less than or equal to 20 characters." };
                return response;
            }
            response.Success = true;
            return response;
        }

        private async Task<UserResponseDto> ValidateDeleteUser(long userId)
        {
            UserResponseDto response = new UserResponseDto();
            bool isExistingUser = await _repository.IsExistingUser(userId);
            if (!isExistingUser)
            {
                response.Error = new ErrorDto() { Message = "User Not Found." };
                return response;
            }
            response.Success = true;
            return response;
        }

        public async Task<UserResponseDto> AddUser(AddUserDto user)
        {
            UserResponseDto response = new UserResponseDto();
            bool userAdded = false;
            try
            {
                response = await ValidateNewUser(user);
                if (!response.Success)
                    return response;

                User newUser = new User()
                {
                    DateOfBirth = user.DateOfBirth,
                    EmailAddress = user.Email,
                    UserName = user.UserName,
                    UpdatedOn = DateTime.Now,
                    UpdatedBy = "user",
                    CreatedBy = "user",
                    CreatedOn = DateTime.Now
                };
                using (IDbContextTransaction transaction = _repository.BeginTransaction())
                {
                    long userId = await _repository.AddUser(newUser);
                    if (userId > 0)
                    {
                        await AddUserAdditionEvent(newUser);
                        transaction.Commit();
                        userAdded = true;
                        response.Id = userId;
                        response.Success = true;
                        response.Message = $"User {newUser.UserName} was added. Current age of {newUser.UserName} is {AgeHelper.GetFullAge(newUser.DateOfBirth)}";
                    }
                }
                if (userAdded)
                {
                    // add the event published to the integration events table - this can help as an event tracker/history table
                    _newUserPublisher.PublishAddedUser(MapUserToPublishData(newUser));
                    await AddUserDispatchEvent(newUser);
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

        public async Task<UserResponseDto> UpdateUser(long userId, UpdateUserDto user)
        {
            UserResponseDto response = new UserResponseDto();
            bool userModified = false;
            try
            {
                response = await ValidateUpdateUser(user, userId);
                if (!response.Success)
                    return response;
                User existingUser = await _repository.GetUserTracking(userId);
                using (IDbContextTransaction transaction = _repository.BeginTransaction())
                {
                    existingUser.UserName = user.UserName;
                    existingUser.UpdatedOn = DateTime.Now;
                    existingUser.UpdatedBy = "user";
                    await ModifyUserAdditionEvent(existingUser);
                    transaction.Commit();
                    userModified = true;
                    response.Id = userId;
                    response.Success = true;
                    response.Message = $"User {existingUser.UserName} was modified successfully";
                }
                if (userModified)
                {
                    // add the event published to the integration events table - this can help as an event tracker/history table
                    _updateUserPublisher.PublishModifiedUser(new UpdateUser() { Email = existingUser.EmailAddress, UserName = existingUser.UserName });
                    await ModifyUserDispatchEvent(existingUser);
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

        public async Task<UserResponseDto> DeleteUser(long userId)
        {
            UserResponseDto response = new UserResponseDto();
            bool userDeleted = false;
            try
            {
                response = await ValidateDeleteUser(userId);
                if (!response.Success)
                    return response;
                User existingUser = await _repository.GetUserTracking(userId);
                using (IDbContextTransaction transaction = _repository.BeginTransaction())
                {
                    long userdeletionResponse = await _repository.DeleteUser(existingUser);
                    if (userdeletionResponse == 1)
                    {
                        await DeleteUserAdditionEvent(existingUser);
                        transaction.Commit();
                    }
                    userDeleted = true;
                    response.Id = userId;
                    response.Success = true;
                    response.Message = $"User {existingUser.UserName} was deleted successfully";
                }
                if (userDeleted)
                {
                    // add the event published to the integration events table - this can help as an event tracker/history table
                    _deleteUserPublisher.PublishDeletedUser(new DeletedUser() { Email = existingUser.EmailAddress });
                    await DeleteUserDispatchEvent(existingUser);
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

        private NewUser MapUserToPublishData(User user)
        {
            return new NewUser()
            {
                DateOfBirth = user.DateOfBirth,
                Email = user.EmailAddress,
                UserName = user.UserName
            };
        }

        private async Task<bool> AddUserAdditionEvent(User user)
        {
            long response = await _eventsRepository.AddEvent(new IntegrationEvent()
            {
                EventName = user.EmailAddress + " Added",
                EventStatus = EventConstants.EVENT_STATUS_CREATED,
                EventType = EventConstants.EVENT_TYPE_ADDED,
                UpdatedOn = DateTime.Now,
                UpdatedBy = "user",
                CreatedBy = "user",
                CreatedOn = DateTime.Now
            });
            return response != -1; // ideally if the integration event creation fails, this needs to be handled but would not be required for the sample app
        }

        private async Task<bool> AddUserDispatchEvent(User user)
        {
            long response = await _eventsRepository.AddEvent(new IntegrationEvent()
            {
                EventName = user.EmailAddress + " Added",
                EventStatus = EventConstants.EVENT_STATUS_DISPATCHED,
                EventType = EventConstants.EVENT_TYPE_ADDED,
                UpdatedOn = DateTime.Now,
                UpdatedBy = "user",
                CreatedBy = "user",
                CreatedOn = DateTime.Now
            });
            return response != -1; // ideally if the integration event creation fails, this needs to be handled but would not be required for the sample app
        }

        private async Task<bool> ModifyUserAdditionEvent(User user)
        {
            long response = await _eventsRepository.AddEvent(new IntegrationEvent()
            {
                EventName = user.UserName + " Updated",
                EventStatus = EventConstants.EVENT_STATUS_CREATED,
                EventType = EventConstants.EVENT_TYPE_MODIFIED,
                UpdatedOn = DateTime.Now,
                UpdatedBy = "user",
                CreatedBy = "user",
                CreatedOn = DateTime.Now
            });
            return response != -1; // ideally if the integration event creation fails, this needs to be handled but would not be required for the sample app
        }

        private async Task<bool> ModifyUserDispatchEvent(User user)
        {
            long response = await _eventsRepository.AddEvent(new IntegrationEvent()
            {
                EventName = user.UserName + " Updated",
                EventStatus = EventConstants.EVENT_STATUS_DISPATCHED,
                EventType = EventConstants.EVENT_TYPE_MODIFIED,
                UpdatedOn = DateTime.Now,
                UpdatedBy = "user",
                CreatedBy = "user",
                CreatedOn = DateTime.Now
            });
            return response != -1; // ideally if the integration event creation fails, this needs to be handled but would not be required for the sample app
        }

        private async Task<bool> DeleteUserAdditionEvent(User user)
        {
            long response = await _eventsRepository.AddEvent(new IntegrationEvent()
            {
                EventName = user.UserName + " Deleted",
                EventStatus = EventConstants.EVENT_STATUS_CREATED,
                EventType = EventConstants.EVENT_TYPE_REMOVED,
                UpdatedOn = DateTime.Now,
                UpdatedBy = "user",
                CreatedBy = "user",
                CreatedOn = DateTime.Now
            });
            return response != -1; // ideally if the integration event creation fails, this needs to be handled but would not be required for the sample app
        }

        private async Task<bool> DeleteUserDispatchEvent(User user)
        {
            long response = await _eventsRepository.AddEvent(new IntegrationEvent()
            {
                EventName = user.UserName + " Deleted",
                EventStatus = EventConstants.EVENT_STATUS_DISPATCHED,
                EventType = EventConstants.EVENT_TYPE_REMOVED,
                UpdatedOn = DateTime.Now,
                UpdatedBy = "user",
                CreatedBy = "user",
                CreatedOn = DateTime.Now
            });
            return response != -1; // ideally if the integration event creation fails, this needs to be handled but would not be required for the sample app
        }


    }
}
