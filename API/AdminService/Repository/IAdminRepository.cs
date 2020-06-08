using AdminService.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminService.Repository
{
    public interface IAdminRepository
    {
        IDbContextTransaction BeginTransaction();

        Task<IList<User>> GetUsers();

        Task<User> GetUserTracking(long userId);

        Task<User> GetUserNonTracking(long userId);

        Task<long> AddUser(User user);

        Task<long> ModifyUser(User user);

        Task<long> DeleteUser(User user);

        Task<User> GetUserTracking(string email);

        Task<bool> IsExistingUser(string email);

        #region Notifications

        Task<long> AddNotification(UserNotification notification);

        #endregion
    }
}
