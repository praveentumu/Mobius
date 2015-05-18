using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using Mobius.EventNotification;

namespace Mobius.EventNotification
{
     
    public sealed class EventActionData
    {
        private EventType _Events;
        private List<string> _EmailRecipients = null;
        private string _DocumentName = string.Empty;
        private DateTime _EventDate = DateTime.Now;
        private List<object> _Attachments = null;
        private string _Token = string.Empty;
        private bool _HasAttachment = false;
        internal int Id { get; set; }

        /// <summary>
        /// Gets and sets the events.
        /// </summary>
        public EventType Event
        {
            get
            {
                return _Events;
            }
            set
            {
                _Events = value;
            }
        }

        /// <summary>
        /// Gets and sets the list of email recipient(s) for the event data.
        /// </summary>        
        public List<string> EmailRecipients
        {
            get
            {
                return (_EmailRecipients != null ? _EmailRecipients : _EmailRecipients = new List<string>());
            }
            set
            {
                if (_EmailRecipients == null)
                {
                    _EmailRecipients = new List<string>();
                    _EmailRecipients = value;
                }
                else
                    _EmailRecipients = value;
            }
        }

        /// <summary>
        /// Gets and sets the document name 
        /// </summary>
        public string DocumentName
        {
            get
            {
                return _DocumentName;
            }
            set
            {
                _DocumentName = value;
            }
        }

        /// <summary>
        /// Gets and sets the data of the event or action.
        /// </summary>
        public DateTime EventDate
        {
            get
            {
                return _EventDate;
            }
        }

        /// <summary>
        /// get  and  set of attachment(s) for email. 
        /// </summary>
        public List<object> Attachments
        {
            get
            {
                return (_Attachments != null ? _Attachments : _Attachments = new List<object>());
            }
            set
            {
                if (_Attachments == null)
                {
                    _Attachments = new List<object>();
                    _Attachments = value;
                }
                else
                    _Attachments = value;
            }
        }

        /// <summary>
        /// get  and  set the token
        /// </summary>
        public string Token
        {
            get
            {
                return _Token;
            }
            set
            {
                _Token = value;
            }
        }

        /// <summary>
        /// get and set include attachment.
        /// </summary>
        public bool HasAttachment
        {
            get
            {
                return _HasAttachment;
            }
            set
            {
                _HasAttachment = value;

            }
        }

        /// <summary>
        /// get or set the user name of provider
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// get or set the user name of password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// get or set patient referral summary
        /// </summary>
        public string ReferralSummary { get; set; }

        /// <summary>
        /// get or set dispatcher summary
        /// </summary>
        public string DispatcherSummary { get; set; }

        /// <summary>
        /// get or set first name 
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// get or set last name 
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Gender { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string DOB { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string ReferralDispatcher { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DocumentId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ReferPatientId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ReferralRequestor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ReferredOn { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string PatientAppointmentDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PatientId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CommunityId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserId { get; set; }


        /// <summary>
        /// get or set The machine name or IP address 
        /// </summary>
        public string NetworkAccessPointID { get; set; }

        /// <summary>
        /// “1” for machine (DNS) name, “2” for IP address
        /// </summary>
        public int NetworkAccessPointTypeCode { get; set; }

        /// <summary>
        /// get or set message type
        /// </summary>
        public string MessageType { get; set; }

        /// <summary>
        /// get or set message 
        /// </summary>
        public byte[] RequestObject { get; set; }

        /// <summary>
        /// get or set purpose
        /// </summary>
        public string Purpose { get; set; }

        /// <summary>
        /// get or set subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// get or set message info
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// get or set message info
        /// </summary>
        public string RecipientEmail { get; set; }


        /// <summary>
        /// Converts the EventActionData to an ExpandoObject with EventData properties that have a values.
        /// </summary>
        /// <returns>The expand object.</returns>
        internal dynamic GetTemplateData()
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (var property in this.GetType().GetProperties())
            {
                if (property.CanRead && this.HasValue(property))
                {
                    expando[property.Name] = property.GetValue(this, null);
                }
            }

            return expando;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Referenceid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Serial { get; set; }


        public string ConsentTable { get; set; }




        #region Helper
        private bool HasValue(System.Reflection.PropertyInfo property)
        {
            var value = property.GetValue(this, null);
            if (value == null)
            {
                return false;
            }
            else if (value is int)
            {
                return (int)value > 0;
            }
            else if (value is DateTime)
            {
                return (DateTime)value != DateTime.MinValue && (DateTime)value != DateTime.MaxValue;
            }
            else if (value is bool)
            {
                return true;
            }
            else if (value is string)
            {
                //TODO How to replace blank ##value 
                return true;//!string.IsNullOrWhiteSpace((string)value);
            }
            else
            {
                return true;
            }
        }
        #endregion Helper




    }
}
