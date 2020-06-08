using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersService.Entities
{
    public class IntegrationEvent
    {
        [Column("EVENT_ID")]
        public long Id { get; set; }

        [Column("EVENT_TYPE")]
        public string EventType { get; set; }

        [Column("EVENT_NAME")]
        public string EventName { get; set; }

        [Column("EVENT_STATUS")]
        public string EventStatus { get; set; }

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
