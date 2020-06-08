using UsersService.Events.Data;

namespace UsersService.Events
{
    public interface IUserDeleted
    {
        void PublishDeletedUser(DeletedUser user);
    }
}
