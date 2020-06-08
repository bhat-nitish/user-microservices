using AdminService.Events.EventHandlers.UserAdded;
using AdminService.Events.EventHandlers.UserDeleted;
using AdminService.Events.EventHandlers.UserModified;
using AdminService.Events.EventHandlers.UserNotificationAdded;
using Microsoft.Extensions.DependencyInjection;

namespace AdminService.Extensions
{
    public static class EventHandlerExtensions
    {
        public static void AddEventHandlers(this IServiceCollection services)
        {

            services.AddTransient<IUserNotificationAdded, UserNotificationAdded>();

            services.AddScoped<IUserAdded, UserAdded>();
            services.AddScoped<IUserModified, UserModified>();
            services.AddScoped<IUserDeleted, UserDeleted>();
        }
    }
}
