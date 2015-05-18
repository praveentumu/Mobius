using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobius.EventNotification
{
    /// <summary>
    /// An event can have more than one action(s ). 
    /// </summary>
    internal class Event
    {
        private int _EventId;
        private EventType _EventName;     

        /// <summary>
        /// Id represent the numerical value associated wiht event 
        /// </summary>
        public int Id
        {
            get
            {
                return _EventId;
            }
            set
            {
                _EventId = value;
            }
        }


        /// <summary>
        /// Name represents the event name.
        /// </summary>
        public EventType Name
        {
            get
            {
                return _EventName;
            }
            set
            {
                _EventName = value;
            }
        }                
    }
}

