using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobius.Entity
{
    public class EventData
    {
        EventType eventType;
        MailEventData mailEventData;
        Guid eventGUID;
        string createdBy;
        DateTime createdOn;
        

        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Guid EventGUID
        {
            get { return eventGUID; }
            set { eventGUID = value; }
        }
        public EventType EventType
        {
            get { return eventType; }
            set { eventType = value; }
        }
       
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public MailEventData MailEventData
        {
            get { return mailEventData; }
            set { mailEventData = value; }
        }
    }
}
