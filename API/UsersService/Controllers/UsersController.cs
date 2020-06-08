using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UsersService.Dto;
using UsersService.Managers;

namespace UsersService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserManager _manager;

        public UsersController(IUserManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public async Task<IReadOnlyCollection<ViewUserDto>> GetUsers()
        {
            return await _manager.GetUsers();
        }

        [HttpPost]
        public async Task<UserResponseDto> AddUser([FromBody] AddUserDto user)
        {
            return await _manager.AddUser(user);
        }

        [HttpPut("{userId}")]
        public async Task<UserResponseDto> UpdateUser(long userId, [FromBody] UpdateUserDto user)
        {
            return await _manager.UpdateUser(userId, user);
        }

        [HttpDelete("{userId}")]
        public async Task<UserResponseDto> DeleteUser(long userId)
        {
            return await _manager.DeleteUser(userId);
        }
    }
}
