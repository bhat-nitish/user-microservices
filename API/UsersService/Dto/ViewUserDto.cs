using System;
using System.Collections.Generic;

namespace UsersService.Dto
{

    public class ViewNotificationDto
    {
        public string Notification { get; set; }

        public DateTime CreatedDate { get; set; }
    }

    public class ViewUserDto
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Email { get; set; }

        public string Age { get; set; }

        public IList<ViewNotificationDto> Notifications { get; set; }
    }
}
