using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobius.EventNotification
{
    internal class EventAction : Event
    {
        private List<Action> _EventActions;
        /// <summary>
        /// EventActions is a collection of Events.
        /// </summary>
        public List<Action> EventActions 
        {
            get
            {
                return (_EventActions != null ? _EventActions : _EventActions = new List<Action>());
            }
            set
            {
                if (_EventActions == null)
                {
                    _EventActions = new List<Action>();
                    _EventActions = value;
                }
                else
                    _EventActions = value;
            }
        }
        
    }
}
