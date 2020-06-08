using UsersService.Events.Data;

namespace UsersService.Events
{
    public interface IUserModified
    {
        void PublishModifiedUser(UpdateUser user);
    }
}
