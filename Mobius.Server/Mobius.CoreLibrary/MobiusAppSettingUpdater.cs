namespace Mobius.CoreLibrary
{

    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Dynamic;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using System.Collections;

    /// <summary>
    /// This class will update XML configuration file 
    /// NOTE - Create Property with the same name as of configuration file Key name 
    /// </summary>
    public class MobiusAppSettingUpdater
    {
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public Boolean UseOnlineService
        {
            set;
            get;
        }

        /// <summary>
        /// /
        /// </summary>
        public Boolean ValidateC32Document
        {
            set;
            get;
        }

        public int EmergencyOverriddenTime
        {
            get;
            set;
        }

        public List<string> LstEmergencyRole
        {
            get;
            set;

        }

        public string AndroidApplicationVersion
        {
            get;
            set;
        }

        public string CAIssuerName
        {
            get;
            set;
        }


        public string LocalHomeCommunityID
        {
            get;
            set;
        }

        public string NHINSubmissionDocumentURL
        {
            get;
            set;
        }


        public string DocumentPath
        {
            get;
            set;
        }
     
        public string TempPath
        {
            get;
            set;
        }

        public string CertificateAuthorityServerURL
        {
            get;
            set;
        }

      
        public string ResourceFileName
        {
            get;
            set;
        }

        public string ResourceFilePath
        {
            get;
            set;
        }
        public string NHINDocumentQueryURL
        {
            get;
            set;
        }
        public string NHINPatientDiscoveryURL
        {
            get;
            set;
        }
        public string NHINPolicyEngineURL
        {
            get;
            set;
        }
        public string NHINRetrieveDocumentURL
        {
            get;
            set;

        }

        public string CertificateOrganizationalUnit
        {
            get;
            set;
        }


        public string CertificateOrgName
        {
            get;
            set;
        }
        public bool EncryptionFlag
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string MobiusPatientCorrelation
        {
            get;
            set;
        }
        public string MobiusNISTValidationServiceURL
        {
            get;
            set;
        }

        public string OnlineNISTValidationServiceURL
        {
            get;
            set;
        }



        public int ConnectGatewayTimeOut
        {
            get;
            set;
        }

        public int ServiceNotificationGap
        {
            get;
            set;
        }

        public int UserUpgradationNotificationGap
        {
            get;
            set;
        }



        public string SmtpHost
        {
            get;
            set;
        }

        public int SmtpPort
        {
            get;
            set;
        }


        public string SmtpUserName
        {
            get;
            set;
        }


        public string SmtpPassword
        {
            get;
            set;
        }


        public bool SmtpEnableSSL
        {
            get;
            set;
        }



        /// <summary>
        /// Converts the EventActionData to an ExpandoObject with EventData properties that have a values.
        /// </summary>
        /// <returns>The expand object.</returns>
        private dynamic GetMasterData()
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

        #endregion

        public void UpdateConfigurationFile()
        {
            XmlDocument xmlDoc = new XmlDocument();
            bool flag = false;
            xmlDoc.Load(ConfigurationManager.AppSettings["ConfigurationPath"]);
            XmlNodeList appSettings = xmlDoc.SelectNodes(".//AppSettings//add");
            XmlNodeList EmergencyAccess = xmlDoc.SelectNodes(".//EmergencyAccess//add");
            XmlNodeList SMTPSettings = xmlDoc.SelectNodes(".//SMTPSettings//add");
            IDictionary<string, object> updatedAppSettings = this.GetMasterData() as IDictionary<string, object>;
            foreach (XmlNode item in appSettings)
            {
                if (updatedAppSettings.Count(t => t.Key.ToUpper() == item.Attributes.GetNamedItem("key").Value.ToString().ToUpper()) == 1)
                {
                    item.Attributes.GetNamedItem("value").Value = updatedAppSettings[item.Attributes.GetNamedItem("key").Value].ToString();
                    flag = true;
                }
            }

            foreach (XmlNode item in SMTPSettings)
            {
                if (updatedAppSettings.Count(t => t.Key.ToUpper() == item.Attributes.GetNamedItem("key").Value.ToString().ToUpper()) == 1)
                {
                    item.Attributes.GetNamedItem("value").Value = updatedAppSettings[item.Attributes.GetNamedItem("key").Value].ToString();
                    flag = true;
                }
            }

            if (updatedAppSettings.Count(t => t.Key.ToUpper()=="LSTEMERGENCYROLE")==1)
            {
                foreach (XmlNode val in EmergencyAccess)
                {
                        val.Attributes.GetNamedItem("Allowed").Value = ((IEnumerable)updatedAppSettings["LstEmergencyRole"]).Cast<object>().Any(item => val.Attributes.GetNamedItem("key").Value.ToString() == item.ToString()) ? "true" : "false";
                        flag = true;
                }
            }

            if (flag)
                xmlDoc.Save(ConfigurationManager.AppSettings["ConfigurationPath"]);

        }


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
