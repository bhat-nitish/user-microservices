using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminService.Constants
{
    public class EventConstants
    {
        public const string EVENT_TYPE_ADDED = "ADDED";
        public const string EVENT_TYPE_MODIFIED = "MODIFIED";
        public const string EVENT_TYPE_REMOVED = "REMOVED";

        public const string EVENT_STATUS_DISPATCHED = "DISPATCHED";
        public const string EVENT_STATUS_RECEIVED = "RECEIVED";
        public const string EVENT_STATUS_COMPLETED = "COMPLETED";
        public const string EVENT_STATUS_CREATED = "CREATED";
    }
}
