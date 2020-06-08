using System;

namespace UsersService.Dto
{
    public class AddUserDto
    {
        public string UserName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Email { get; set; }
    }
}
