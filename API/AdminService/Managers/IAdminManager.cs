using AdminService.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminService.Managers
{
    public interface IAdminManager
    {
        Task<IReadOnlyCollection<UserDto>> GetUsers();

        Task<AdminResponseDto> NotifyUser(NotifyUserDto notification);
    }
}
