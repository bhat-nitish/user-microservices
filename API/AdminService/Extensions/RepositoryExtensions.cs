using AdminService.Context;
using AdminService.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace AdminService.Extensions
{
    public static class RepositoryExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddDbContext<AdminServiceContext>();

            services.AddTransient<IAdminRepository, AdminRepository>();
            services.AddTransient<IIntegrationEventRepository, IntegrationEventRepository>();
        }
    }
}
