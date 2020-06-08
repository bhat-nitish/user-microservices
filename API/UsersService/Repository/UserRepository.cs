using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersService.Context;
using UsersService.Entities;

namespace UsersService.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UsersServiceContext _context;

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public UserRepository(UsersServiceContext context)
        {
            _context = context;
        }

        public async Task<IList<User>> GetUsers()
        {
            return await _context.Users.AsNoTracking().OrderByDescending(u => u.Id).ToListAsync();
        }

        public async Task<List<UserNotification>> GetUserNotifications()
        {
            return await _context.UserNotifications.AsNoTracking().OrderByDescending(n => n.Id).ToListAsync();
        }

        public async Task<User> GetUserTracking(long userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User> GetUserNonTracking(long userId)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<long> AddUser(User user)
        {
            if (user == null)
            {
                return -1;
            }
            else
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                PropertyValues createdUser = _context.Entry(user).OriginalValues;
                long userId = createdUser.GetValue<long>("Id");
                return userId;
            }
        }

        public async Task<long> ModifyUser(User user)
        {
            if (user == null)
            {
                return -1;
            }
            bool isExistingUser = await _context.Users.AnyAsync(u => u.Id == user.Id);
            if (!isExistingUser)
            {
                return -2;
            }
            else
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return 1;
            }
        }

        public async Task<long> DeleteUser(User user)
        {
            if (user == null)
            {
                return -1;
            }
            bool isExistingUser = await _context.Users.AnyAsync(u => u.Id == user.Id);
            if (!isExistingUser)
            {
                return -2;
            }
            else
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return 1;
            }
        }

        public async Task<bool> IsExistingUser(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            email = email.ToLower();
            return await _context.Users.AnyAsync(u => u.EmailAddress.ToLower() == email); // This is done instead of string.compare() to avoid client side execution
        }

        public async Task<bool> IsExistingUser(long userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }

        public async Task<long> AddNotification(UserNotification notification)
        {
            if (notification == null)
            {
                return -1;
            }
            else
            {
                await _context.UserNotifications.AddAsync(notification);
                await _context.SaveChangesAsync();
                PropertyValues createdNotification = _context.Entry(notification).OriginalValues;
                long userId = createdNotification.GetValue<long>("Id");
                return userId;
            }
        }

    }
}
