using Contracts.MessageBus.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UsersService.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void RegisterConfiguration(this IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<PublisherConfig>(Configuration.GetSection("PublisherConfig"));
            services.Configure<SubscriberConfig>(Configuration.GetSection("SubscriberConfig"));
        }

    }
}
