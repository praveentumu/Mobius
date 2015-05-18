using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobius.EventNotification
{
    public class EventLog
    {

        /// <summary>
        /// Gets or sets the id of the EventNotificationLog.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the id of the event Id
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets the id of the event action transaction Id
        /// </summary>
        public int EventActionsId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether event was process successfully.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the last date that the notification was processed.
        /// </summary>
        public DateTime ProcessedDate { get; set; }

        /// <summary>
        /// Gets or sets the error message if the notification was not successful.
        /// </summary>
        public string ErrorMessage { get; set; }
    }    
}
