using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobius.EventNotification
{
    /// <summary>
    /// Action class specifies the attributes like id, name, subject template, template, include attatchment.
    /// </summary>
    internal class Action
    {
        private int _actionId;
        private ActionType _actionName;        
        private string _Template;
        private string _Subject;

        /// <summary>
        /// get and set the action id.
        /// </summary>
        public int Id
        {
            get
            {
                return _actionId;
            }
            set
            {
                _actionId = value;
            }
        }

        /// <summary>
        /// get and set name of action
        /// </summary>
        public ActionType Name
        {
            get
            {
                return _actionName;
            }
            set
            {
                _actionName = value;
            }
        }
        
        /// <summary>
        /// get and set template.
        /// </summary>
        public string Template
        {
            get
            {
                return _Template;
            }
            set
            {
                _Template = value;
            }
        }

        public string Subject
        {
            get
            {
                return _Subject;
            }
            set
            {
                _Subject = value;
            }
        }
       


    }
}
