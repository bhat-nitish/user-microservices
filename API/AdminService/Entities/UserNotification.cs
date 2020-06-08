using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminService.Entities
{
    public class UserNotification
    {
        [Column("NOTIFICATION_ID")]
        public long Id { get; set; }

        [Column("EMAIL")]
        public string Email { get; set; }

        [Column("NOTIFICATION")]
        public string Notification { get; set; }

        [Column("NOTIFICATION_STATUS")]
        public string NotificationStatus { get; set; }

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
