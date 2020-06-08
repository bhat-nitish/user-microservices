using Microsoft.Extensions.DependencyInjection;
using UsersService.Managers;

namespace UsersService.Extensions
{
    public static class ManagerExtensions
    {
        public static void AddManagers(this IServiceCollection services)
        {
            services.AddTransient<IUserManager, UserManager>();
        }
    }
}
