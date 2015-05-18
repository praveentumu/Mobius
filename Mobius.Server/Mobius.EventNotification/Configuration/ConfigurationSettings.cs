using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml.Linq;

namespace Mobius.EventNotification
{
    internal static class ConfigurationSetting
    {
        private static string _SmtpHost;
        private static int _SmtpPort;
        private static string _EmailFromAddress;
        private static string _SmtpUserName;
        private static string _SmtpPassword;
        private static string _SmtpSSL;
        /// <summary>
        /// Static constructor, so that above variable get initialized at the beginning and exactly once.
        /// </summary>
        static ConfigurationSetting()
        {

            var appSettings = from configuration in XElement.Load(ConfigurationManager.AppSettings["ConfigurationPath"]).Elements("SMTPSettings").Elements("add")
                              select new
                              {
                                  Key = configuration.Attribute("key").Value,
                                  value = configuration.Attribute("value").Value
                              };

            if (appSettings.Count() > 0)
            {
                foreach (var item in appSettings)
                {
                    switch (item.Key)
                    {
                        case "SmtpHost":
                            _SmtpHost = item.value;
                            break;
                        case "SmtpPort":
                            _SmtpPort = string.IsNullOrWhiteSpace(item.value) ? 0 : Convert.ToInt32(item.value);
                            break;
                        case "EmailFromAddress":
                            _EmailFromAddress = item.value;
                            break;
                        case "SmtpUserName":
                            _SmtpUserName = item.value;
                            break;
                        case "SmtpPassword":
                            _SmtpPassword = item.value;
                            break;
                        case "SmtpEnableSSL":
                            _SmtpSSL = item.value;
                            break;
                        default:
                            break;
                    }
                }
            }
        }


        ///Todo - Add keys in webconfi and app confi 
        ///     - read web/ app confi for following properties 
        ///     



        /// <summary>
        /// Gets the Smtp host to use to send email.
        /// </summary>
        internal static string SmtpHost
        {
            get
            {
                return _SmtpHost;
            }
        }

        /// <summary>
        /// Gets the port of the Smtp server.
        /// </summary>
        internal static int SmtpPort
        {
            get
            {
                return _SmtpPort;
            }
        }

        /// <summary>
        /// Gets the from email address to use when sending email.
        /// </summary>
        internal static string EmailFromAddress
        {
            get
            {
                return _EmailFromAddress;
            }
        }

        /// <summary>
        /// Gets the user name for SMTP client(Used to set crdentials for SMTP client)
        /// </summary>
        internal static string SmtpUserName
        {
            get
            {
                return _SmtpUserName;
            }
        }

        /// <summary>
        /// Gets the password for SMTP client(Used to set crdentials for SMTP client)
        /// </summary>
        internal static string SmtpPassword
        {
            get
            {
                return _SmtpPassword;
            }
        }


        /// <summary>
        /// Gets the SMTP SSL setting enable/disable  
        /// </summary>
        internal static bool SmtpSSL
        {
            get
            {
                bool result;
                return string.IsNullOrEmpty(_SmtpSSL) ? false : bool.TryParse(_SmtpSSL, out result);
            }
        }
    }
}
