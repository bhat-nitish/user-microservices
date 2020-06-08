using System.Threading.Tasks;

namespace UsersService.Events.Base
{
    public interface IBaseEvent
    {
        Task HandleEvent(string data);
    }
}
