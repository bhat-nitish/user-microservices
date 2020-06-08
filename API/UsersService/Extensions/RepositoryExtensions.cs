using Microsoft.Extensions.DependencyInjection;
using UsersService.Context;
using UsersService.Repository;

namespace UsersService.Extensions
{
    public static class RepositoryExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {

            services.AddDbContext<UsersServiceContext>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IIntegrationEventRepository, IntegrationEventRepository>();
        }
    }
}
