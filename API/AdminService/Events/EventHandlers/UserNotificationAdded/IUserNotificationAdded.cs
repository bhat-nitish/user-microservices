using AdminService.Events.Data;
namespace AdminService.Events.EventHandlers.UserNotificationAdded
{
    public interface IUserNotificationAdded
    {
        void PublishUserNotification(NewUserNotification notification);
    }
}
