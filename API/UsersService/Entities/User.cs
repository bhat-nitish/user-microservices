using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersService.Entities
{
    public class User
    {
        [Column("USER_ID")]
        public long Id { get; set; }

        [Column("USER_NAME")]
        public string UserName { get; set; }

        [Column("EMAIL_ADDRESS")]
        public string EmailAddress { get; set; }

        [Column("DATE_OF_BIRTH")]
        public DateTime DateOfBirth { get; set; }

        [Column("CREATED_ON")]
        public DateTime CreatedOn { get; set; }

        [Column("UPDATED_ON")]
        public DateTime UpdatedOn { get; set; }

        [Column("CREATED_BY")]
        public string CreatedBy { get; set; }

        [Column("UPDATED_BY")]
        public string UpdatedBy { get; set; }
    }
}


