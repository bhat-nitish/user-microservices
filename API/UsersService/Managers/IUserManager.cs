using System.Collections.Generic;
using System.Threading.Tasks;
using UsersService.Dto;

namespace UsersService.Managers
{
    public interface IUserManager
    {
        Task<IReadOnlyCollection<ViewUserDto>> GetUsers();

        Task<UserResponseDto> AddUser(AddUserDto user);

        Task<UserResponseDto> UpdateUser(long userId, UpdateUserDto user);

        Task<UserResponseDto> DeleteUser(long userId);
    }
}
