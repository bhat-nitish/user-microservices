using UsersService.Events.Data;

namespace UsersService.Events
{
    public interface IUserAdded
    {
        void PublishAddedUser(NewUser user);
    }
}
