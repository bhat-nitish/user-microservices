using System;

namespace AdminService.Dto
{
    public class UserDto
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Age { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
