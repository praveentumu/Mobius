using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobius.Entity
{
    public class EventType
    {
        int eventID;
        string eventName;

        public int EventID
        {
            get { return eventID; }
            set { eventID = value; }
        }

        public string EventName
        {
            get { return eventName; }
            set { eventName = value; }
        }
    }
}
