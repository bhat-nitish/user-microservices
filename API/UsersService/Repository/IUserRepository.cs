using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using UsersService.Entities;

namespace UsersService.Repository
{
    public interface IUserRepository
    {

        IDbContextTransaction BeginTransaction();

        Task<IList<User>> GetUsers();

        Task<User> GetUserTracking(long userId);

        Task<User> GetUserNonTracking(long userId);

        Task<long> AddUser(User user);

        Task<long> ModifyUser(User user);

        Task<long> DeleteUser(User user);

        Task<bool> IsExistingUser(string email);

        Task<bool> IsExistingUser(long userId);

        #region Notification

        Task<long> AddNotification(UserNotification notification);

        Task<List<UserNotification>> GetUserNotifications();

        #endregion
    }
}
