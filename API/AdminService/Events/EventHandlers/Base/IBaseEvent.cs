using System.Threading.Tasks;

namespace AdminService.Events.EventHandlers.Base
{
    public interface IBaseEvent
    {
        Task HandleEvent(string data);
    }
}
