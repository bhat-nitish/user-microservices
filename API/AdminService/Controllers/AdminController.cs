using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminService.Dto;
using AdminService.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AdminService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminManager _manager;

        public AdminController(IAdminManager manager)
        {
            _manager = manager;
        }

        [HttpGet("users")]
        public async Task<IReadOnlyCollection<UserDto>> GetUsers()
        {
            return await _manager.GetUsers();
        }

        [HttpPost("users/notify")]
        public async Task<AdminResponseDto> AddUserNotification([FromBody] NotifyUserDto notification)
        {
            return await _manager.NotifyUser(notification);
        }

    }
}
