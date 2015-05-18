using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobius.Entity
{
    public class MailEventData
    {
        
        int templateID;
        bool isDelivered;
        string fromAddress;
        string toAddress;
        string documentID;
        string patientID;
        string purpose;
        string loggedInRole;
        string htmlTemplate;
        string token;






        public string Token
        {
            get { return token; }
            set { token = value; }
        }
        public string Purpose
        {
            get { return purpose; }
            set { purpose = value; }
        }

        public string PatientID
        {
            get { return patientID; }
            set { patientID = value; }
        }

        public string FromAddress
        {
            get { return fromAddress; }
            set { fromAddress = value; }
        }
        
        public bool IsDelivered
        {
            get { return isDelivered; }
            set { isDelivered = value; }
        }
        public int TemplateID
        {
            get { return templateID; }
            set { templateID = value; }
        }

        public string LoggedInRole
        {
            get { return loggedInRole; }
            set { loggedInRole = value; }
        }
        public string DocumentID
        {
            get { return documentID; }
            set { documentID = value; }
        }
        public string HtmlTemplate
        {
            get { return htmlTemplate; }
            set { htmlTemplate = value; }
        }
        public string ToAddress
        {
            get { return toAddress; }
            set { toAddress = value; }
        }
    }
}
