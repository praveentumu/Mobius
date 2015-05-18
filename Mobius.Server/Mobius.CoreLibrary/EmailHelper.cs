

namespace Mobius.CoreLibrary
{
    using System.Net;
    using System.Net.Mail;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    #region EmailHelper
    /// <summary>
    /// Provides helper methods for sending email message using SMTP.
    /// </summary>
    public static class EmailHelper
    {
        /// <summary>
        /// Sends the email message to the SMTP server for delivery.
        /// </summary>
        /// <param name="smtpHost">The smtp host name or ipaddress.</param>
        /// <param name="smtpPort">The smtp port</param>        
        /// <param name="from">Contains the address information of the sender.</param>
        /// <param name="recipients">The list of recipients to send the message to.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body of the message.</param>
        public static void SendMail(string smtpHost, int smtpPort, string from, string recipients, string subject, string body,List<string> ccRecipients= null )
        {
            SendMail(smtpHost, smtpPort, null, null, false, recipients, from, subject, body,ccRecipients);
        }

        /// <summary>
        /// Sends the email message to the SMTP server for delivery.
        /// </summary>
        /// <param name="smtpHost">The smtp host name or ipaddress.</param>
        /// <param name="smtpPort">The smtp port</param>
        /// <param name="smtpUserName">The username of smtp server</param>
        /// <param name="smtpPassword">The password of smtp server .</param>        
        /// <param name="SmtpEnableSsl">Specify whether SMTP uses Secure Sockets Layer.</param>        
        /// <param name="from">Contains the address information of the sender.</param>
        /// /// <param name="recipients">The list of recipients to send the message to.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body of the message.</param>
        public static void SendMail(string smtpHost, int smtpPort, string smtpUserName, string smtpPassword, bool SmtpEnableSsl, string from, string recipients, string subject, string body, List<string> ccRecipients=null)
        {
            object status = new object();
            using (SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort))
            {
                if (!string.IsNullOrEmpty(smtpUserName))
                {
                    smtpClient.Credentials = new NetworkCredential(smtpUserName, smtpPassword);
                    smtpClient.EnableSsl = SmtpEnableSsl;
                }

                using (MailMessage message = new MailMessage(from, recipients, subject, body))
                {
                    if (ccRecipients != null && ccRecipients.Count > 0)
                    {
                        //to send the mail in CC
                        foreach (var rec in ccRecipients)
                        {
                            message.CC.Add(rec);
                        }
                    }
                    message.IsBodyHtml = true;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.Send(message);

                }
            }
        }
    }

    #endregion

    #region EmailTemplate Interface
    public interface IEmailTemplate
    {
        string Emailbody { get; }
        string Subject { get; }
    }

    #endregion

    #region Email Configuration For Client


    //Class used in Client to get email structure used for Renew/Expired Certification
    public static class EmailConfiguration
    {
        #region  "Private Variable"
        private static string _SmtpHost = MobiusAppSettingReader.SmtpHost;
        private static int _SmtpPort = MobiusAppSettingReader.SmtpPort;
        private static string _SmtpUserName =MobiusAppSettingReader.SmtpUserName;
        private static string _SmtpPassword = MobiusAppSettingReader.SmtpPassword;
        private static bool _SmtpSSL = MobiusAppSettingReader.SmtpEnableSSL;
        #endregion "Private Variable"

        #region  "Properties"
        /// <summary>
        /// Gets the Smtp host to use to send email.
        /// </summary>
        public static string SmtpHost
        {
            get
            {
                return _SmtpHost;
            }
        }

        /// <summary>
        /// Gets the port of the Smtp server.
        /// </summary>
        public static int SmtpPort
        {
            get
            {
                return _SmtpPort;
            }
        }


        /// <summary>
        /// Gets the user name for SMTP client(Used to set crdentials for SMTP client)
        /// </summary>
        public static string SmtpUserName
        {
            get
            {
                return _SmtpUserName;
            }
        }

        /// <summary>
        /// Gets the password for SMTP client(Used to set crdentials for SMTP client)
        /// </summary>
        public static string SmtpPassword
        {
            get
            {
                return _SmtpPassword;
            }
        }


        /// <summary>
        /// Gets the SMTP SSL setting enable/disable  
        /// </summary>
        public static bool SmtpSSL
        {
            get
            {
                return _SmtpSSL;
                //return string.IsNullOrEmpty(_SmtpSSL) ? false : bool.TryParse(_SmtpSSL, out result);
            }
        }
        #endregion  "Properties"
    }



    public class RenewCertificate : IEmailTemplate
    {

        readonly string userName;
        readonly DateTime ExpiryDate;
        readonly int DaysToExpire;
        string _EmailBody = string.Empty;
        string _Subject = string.Empty;

        public RenewCertificate(string userName, DateTime ExpiryDate, int DaysToExpire)
        {
            this.userName = userName;
            this.ExpiryDate = ExpiryDate;
            this.DaysToExpire = DaysToExpire;

            _EmailBody = @"<html><head></head><body><div>
            <p>Dear " + this.userName + @",</p>
            <p>Your Mobius account is about to expire on " + this.ExpiryDate.ToString() + @", which if happens, will prohibit you from accessing the respective account.</p> 
            <p>Day(s) left: " + this.DaysToExpire.ToString() + @" for renewal.</p>
            <p>Please upgrade the account by using ""Upgrade my account "" under User Prefernces link, once you login into the application.</p>
            <p>Thanks for your continued support. </p>  
            <p>[Note: This is a System generated mail.]</p>
            </div></body></html>";

            _Subject = "Upgrade Mobius account";

        }

        public string Emailbody
        {
            get
            {
                return _EmailBody;
            }
        }

        public string Subject
        {
            get
            {
                return _Subject;
            }

        }

    }

    public class ExpiredCertificate : IEmailTemplate
    {
        string _EmailBody = "";
        string _Subject = "";

        public string Emailbody
        {
            get
            {
                return _EmailBody;
            }
        }

        public string Subject
        {
            get
            {
                return _Subject;
            }

        }
        public ExpiredCertificate(string UserName)
        {

            _EmailBody = @"<html><head></head><body><div>
            <p>Dear Admin,</p>
            <p>I am unable to access my Mobius account <b>" + UserName + @"</b> as my account certificate is expired.</p> 
            <p>Please do the needful</p>
            <p>[Note: This is a System generated mail.]</p>
            </div></body></html>";

            _Subject = "Unable to access my account.";
        }

    }
    #endregion  Email Configuration For Client

    #region EmailConfiguration for Admin
    public class RenewServerCertificate : IEmailTemplate
    {
        readonly DateTime ExpiryDate;
        readonly int DaysToExpire;
        string _EmailBody = string.Empty;
        string _Subject = string.Empty;

        public RenewServerCertificate( DateTime ExpiryDate, int DaysToExpire)
        {
            this.ExpiryDate = ExpiryDate;
            this.DaysToExpire = DaysToExpire;

            _EmailBody = @"<html><head></head><body><div>
            <p>Dear Admin</p>
            <p>Your MHISE Service account is about to expire on " + this.ExpiryDate.ToString() + @", which if happens, will prohibit users from accessing the service offerings.</p> 
            <p>Day(s) left: " + this.DaysToExpire.ToString() + @" for renewal.</p>
            <p>Please upgrade the service certificate in LocalMachine -> Personal Store and its reference in the service configuration file.</p> 
            <p>[Note: This is a System generated mail.]</p>
            </div></body></html>";

            _Subject = "Upgrade MHISE Server Certificate";

        }

        public string Emailbody
        {
            get
            {
                return _EmailBody;
            }
        }

        public string Subject
        {
            get
            {
                return _Subject;
            }

        }

    }

    public class ExpiredServerCertificate : IEmailTemplate
    {
        string _EmailBody = "";
        string _Subject = "";

        public string Emailbody
        {
            get
            {
                return _EmailBody;
            }
        }

        public string Subject
        {
            get
            {
                return _Subject;
            }

        }
        public ExpiredServerCertificate()
        {

            _EmailBody = @"<html><head></head><body><div>
            <p>Dear Admin,</p>
            <p>Mobius service account certificate is expired.</p> 
            <p>Please upgrade the service certificate in LocalMachine -> Personal Store and its reference in the service configuration file.</p> 
            <p>[Note: This is a System generated mail.]</p>
            </div></body></html>";

            _Subject = "MHISE Server Certificate-Expired";

        }
    #endregion

    }
}
