using Contracts.MessageBus.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UsersService.Events;
using UsersService.Events.UserNotificationReceived;

namespace UsersService.Extensions
{
    public static class MessageBusExtension
    {
        public static void RegisterRabbitMq(this IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<PublisherConfig>(Configuration.GetSection("PublisherConfig"));
            services.Configure<SubscriberConfig>(Configuration.GetSection("SubscriberConfig"));

            services.AddTransient<IUserAdded, UserAdded>();
            services.AddTransient<IUserModified, UserModified>();
            services.AddTransient<IUserDeleted, UserDeleted>();

            services.AddScoped<IUserNotificationReceived, UserNotificationReceived>();
        }
    }
}
