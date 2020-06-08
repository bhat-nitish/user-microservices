using System;

namespace UsersService.Events.Data
{
    public class NewUser
    {
        public string UserName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Email { get; set; }
    }
}
