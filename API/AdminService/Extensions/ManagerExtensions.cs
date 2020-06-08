using AdminService.Managers;
using Microsoft.Extensions.DependencyInjection;

namespace AdminService.Extensions
{
    public static class ManagerExtensions
    {
        public static void AddManagers(this IServiceCollection services)
        {
            services.AddTransient<IAdminManager, AdminManager>();
        }
    }
}
