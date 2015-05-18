namespace Mobius.CoreLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Xml.Linq;


    /// <summary>
    /// This class will return all the App setting Configuration
    /// </summary>
    public static class MobiusAppSettingReader
    {
        private static string _DocumentPath = string.Empty;
        private static string _TempPath = string.Empty;
        //private static string _FromEmail = string.Empty;
        private static string _CertificateAuthorityServerURL = string.Empty;
        private static int _OTPValidity = 0;
        private static string _CAIssuerName = string.Empty;
        private static string _ResourceFilePath = string.Empty;
        private static string _ResourceFileName = string.Empty;
        private static string _NHINDocumentQueryURL = string.Empty;
        private static string _NHINPatientDiscoveryURL = string.Empty;
        private static string _NHINPolicyEngineURL = string.Empty;
        private static string _NHINRetrieveDocumentURL = string.Empty;
        private static string _CertificateCommonName = string.Empty;
        private static string _CertificateOrganizationalUnit = string.Empty;
        private static string _CertificateOrgName = string.Empty;
        private static bool _EncryptionFlag = false;
        private static string _LocalHomeCommunityID = string.Empty;
        private static string _ConnectionString = string.Empty;
        private static string _NHINSubmissionDocumentURL = string.Empty;        
        private static string _AndroidApplicationVersion = "1.00";
        private static string _MobiusPatientCorrelation = string.Empty;

        private static string _MobiusNISTValidationServiceURL = string.Empty;
        private static string _OnlineNISTValidationServiceURL = string.Empty;
        private static int _ConnectGateway = 180000;
        private static int _UserUpgradationNotificationGap = 30;
        private static int _ServiceNotificationGap = 30;
        

        private static string _SmtpHost;
        private static int _SmtpPort;
        private static string _EmailFromAddress;
        private static string _SmtpUserName;
        private static string _SmtpPassword;
        private static bool _SmtpSSL;

        private static List<string> truePart = new List<string>() { "TRUE", "1", "YES", "Y" };






        static MobiusAppSettingReader()
        {

            var appSettings = from configuration in XElement.Load(ConfigurationManager.AppSettings["ConfigurationPath"]).Elements("AppSettings").Elements("add")
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
                        case "ConnectionString":
                            _ConnectionString = item.value;
                            break;
                        case "DocumentPath":
                            _DocumentPath = item.value;
                            break;
                        //case "EmailFromAddress":
                        //    _FromEmail = item.value;
                        //    break;
                        case "TempPath":
                            _TempPath = item.value;
                            break;
                        case "CertificateAuthorityServerURL":
                            _CertificateAuthorityServerURL = item.value;
                            break;
                        case "OTPValidity":
                            //if configuration setting does not have configuration value then set the default value of OTP 72 hr
                            _OTPValidity = !string.IsNullOrWhiteSpace(item.value) ? 72 : Convert.ToInt32(_OTPValidity);
                            break;
                        case "ResourceFilePath":
                            _ResourceFilePath = item.value;
                            break;
                        case "ResourceFileName":
                            _ResourceFileName = item.value;
                            break;
                        case "CAIssuerName":
                            _CAIssuerName = item.value;
                            break;
                        case "NHINDocumentQueryURL":
                            _NHINDocumentQueryURL = item.value;
                            break;
                        case "NHINPatientDiscoveryURL":
                            _NHINPatientDiscoveryURL = item.value;
                            break;
                        case "NHINPolicyEngineURL":
                            _NHINPolicyEngineURL = item.value;
                            break;
                        case "NHINRetrieveDocumentURL":
                            _NHINRetrieveDocumentURL = item.value;
                            break;
                        case "CertificateCommonName":
                            _CertificateCommonName = item.value;
                            break;
                        case "CertificateOrganizationalUnit":
                            _CertificateOrganizationalUnit = item.value;
                            break;
                        case "CertificateOrgName":
                            _CertificateOrgName = item.value;
                            break;
                        case "EncryptionFlag":
                            _EncryptionFlag = string.IsNullOrEmpty(item.value) ? false : truePart.Contains(item.value.ToUpper()); ;
                            break;
                        case "LocalHomeCommunityID":
                            _LocalHomeCommunityID = item.value;
                            break;                    
                        case "NHINSubmissionDocumentURL":
                            _NHINSubmissionDocumentURL = item.value;
                            break;
                        case "AndroidApplicationVersion":
                            _AndroidApplicationVersion = item.value;
                            break;
                        case "MobiusPatientCorrelation":
                            _MobiusPatientCorrelation = item.value;
                            break;
                        case "MobiusNISTValidationServiceURL":
                            _MobiusNISTValidationServiceURL = item.value;
                            break;
                        case "OnlineNISTValidationServiceURL":
                            _OnlineNISTValidationServiceURL = item.value;
                            break;
                        case "ConnectGatewayTimeOut":
                            if(!String.IsNullOrEmpty(item.value))
                                _ConnectGateway =Convert.ToInt32(item.value);
                           break;
                     
                        case "UserUpgradationNotificationGap":
                           if (!String.IsNullOrEmpty(item.value))
                               _UserUpgradationNotificationGap  = Convert.ToInt32(item.value);
                           break;
                        case "ServiceNotificationGap":
                           if (!String.IsNullOrEmpty(item.value))
                               _ServiceNotificationGap = Convert.ToInt32(item.value);
                           break;
                        
                        default:
                            break;
                    }
                }
            }



            var smtpSettings = from configuration in XElement.Load(ConfigurationManager.AppSettings["ConfigurationPath"]).Elements("SMTPSettings").Elements("add")
                              select new
                              {
                                  Key = configuration.Attribute("key").Value,
                                  value = configuration.Attribute("value").Value
                              };


            if (smtpSettings.Count() > 0)
            {
                foreach (var item in smtpSettings)
                {
                    switch (item.Key)
                    {
                        case "SmtpHost":
                            _SmtpHost = item.value;
                            break;
                        case "SmtpPort":
                            _SmtpPort =Convert.ToInt32(item.value);
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
                            _SmtpSSL = Convert.ToBoolean(item.value);
                            break;
                    
                        default:
                            break;
                    }
                }
            }

            //var EmergencyAccess = from configuration in XElement.Load(ConfigurationManager.AppSettings["ConfigurationPath"]).Elements("ProviderRole").Elements("add")
            //                      select new
            //                      {
            //                          Key = configuration.Attribute("key").Value,
            //                          value = configuration.Attribute("value").Value
            //                      };
            //List<string> _LstEmergencyRole = new List<string>();
            //if (EmergencyAccess.Count() > 0)
            //{
            //    foreach (var item in EmergencyAccess)
            //    {
            //        _LstEmergencyRole.Add(item.value);
            //    }
            //}


        }


        public static string AndroidApplicationVersion { get { return GetConfigurationProperties("AndroidApplicationVersion"); } }

        /// <summary>
        /// 
        /// </summary>
        public static string CAIssuerName { get { return _CAIssuerName; } }


        public static string LocalHomeCommunityID { get { return GetConfigurationProperties("LocalHomeCommunityID"); } }

      



        public static string NHINSubmissionDocumentURL
        {
            get
            {
                return GetConfigurationProperties("NHINSubmissionDocumentURL");
            }
        }





        /// <summary>
        /// Get the information about the DataBase ConnectionString
        /// </summary>
        public static string ConnectionString
        {
            get { return _ConnectionString; }
        }

        /// <summary>
        /// Get the information about the Document's path
        /// </summary>
        public static string DocumentPath
        {
            get { return GetConfigurationProperties("DocumentPath"); }
        }

        ///// <summary>
        ///// Get the information about the Retrieve document path
        ///// </summary>
        //public static string EmailFromAddress
        //{
        //    get { return _FromEmail; }
        //}


        /// <summary>
        /// Get the CA server Location(URL) 
        /// </summary>
        public static string TempPath
        {
            get { return GetConfigurationProperties("TempPath"); }
        }

        public static string CertificateAuthorityServerURL
        {
            get { return GetConfigurationProperties("CertificateAuthorityServerURL"); }
        }

        /// <summary>
        /// Get the information about the one time password expired Hour
        /// </summary>
        public static int OTPValidity
        {
            get { return _OTPValidity; }
        }

        public static string ResourceFileName
        {
            get { return GetConfigurationProperties("ResourceFileName"); }
        }

        public static string ResourceFilePath
        {
            get { return GetConfigurationProperties("ResourceFilePath"); }
        }
        public static string NHINDocumentQueryURL
        {
            get { return GetConfigurationProperties("NHINDocumentQueryURL"); }
        }
        public static string NHINPatientDiscoveryURL
        {
            get { return GetConfigurationProperties("NHINPatientDiscoveryURL"); }
        }
        public static string NHINPolicyEngineURL
        {
            get { return GetConfigurationProperties("NHINPolicyEngineURL"); }
        }
        public static string NHINRetrieveDocumentURL
        {
            get 
            {
                return GetConfigurationProperties("NHINRetrieveDocumentURL");
            
            }

        }

        public static string CertificateOrganizationalUnit
        {
            get
            {
                return GetConfigurationProperties("CertificateOrganizationalUnit");
                
            }
        }

        public static string CertificateCommonName
        {
            get
            {
                return GetConfigurationProperties("CertificateCommonName");
                 
            }
        }

        public static string CertificateOrgName
        {
            get
            {
                return GetConfigurationProperties("CertificateOrgName");
              
            }
        }
        public static bool EncryptionFlag
        {
            get
            {
                string value = GetConfigurationProperties("EncryptionFlag");
                return Convert.ToBoolean(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string MobiusPatientCorrelation
        {
            get
            {
                return GetConfigurationProperties("MobiusPatientCorrelation");
               
            }
        }
        public static string MobiusNISTValidationServiceURL
        {
            get
            {
                return GetConfigurationProperties("MobiusNISTValidationServiceURL");
             
            }
        }

        public static string OnlineNISTValidationServiceURL
        {
            get
            {
                return GetConfigurationProperties("OnlineNISTValidationServiceURL");
                
            }
        }


        public static bool UseOnlineService
        {
            get
            {
                string value = GetConfigurationProperties("UseOnlineService");
                return string.IsNullOrEmpty(value) ? false : truePart.Contains(value.ToUpper());
            }
        }

        public static bool ValidateC32Document
        {
            get
            {
                string value = GetConfigurationProperties("ValidateC32Document");
                return string.IsNullOrEmpty(value) ? false : truePart.Contains(value.ToUpper());
            }
        }

        public static int ConnectGatewayTimeOut
        {
            get
            {
                string value = GetConfigurationProperties("ConnectGatewayTimeOut");
                return Convert.ToInt32(value);
            }
        }

        public static int ServiceNotificationGap
        {
            get
            {
                string value = GetConfigurationProperties("ServiceNotificationGap");
                return Convert.ToInt32(value);
            }
        }

        public static int UserUpgradationNotificationGap
        {
            get
            {
                string value = GetConfigurationProperties("UserUpgradationNotificationGap");
                return Convert.ToInt32(value);
            }
        }

      

        public static string SmtpHost
        {
            get
            {
                return GetConfigurationProperties("SmtpHost", "SMTPSettings");
            }
        }

        public static int SmtpPort
        {
            get
            {
                return Convert.ToInt32(GetConfigurationProperties("SmtpPort", "SMTPSettings"));
            }
        }


        public static string EmailFromAddress
        {
            get
            {
                return _EmailFromAddress;
            }
        }


        public static string SmtpUserName
        {
            get
            {
                return GetConfigurationProperties("SmtpUserName", "SMTPSettings");
            }
        }


        public static string SmtpPassword
        {
            get
            {
                return GetConfigurationProperties("SmtpPassword", "SMTPSettings");
            }
        }


        public static bool SmtpEnableSSL
        {
            get
            {
                return Convert.ToBoolean(GetConfigurationProperties("SmtpEnableSSL", "SMTPSettings"));
            }
        }






        public static int EmergencyOverriddenTime
        {
            get
            {
                //int value = Convert.ToInt32(GetConfigurationProperties("EmergencyOverriddenTime"));
                int value = (int)Math.Ceiling(Convert.ToDecimal(GetConfigurationProperties("EmergencyOverriddenTime")));
                return value;
            }
        }


        public static Dictionary<string, string> LstEmergencyRole
        {
            get
            {
                return GetEmergencyAccessProperties(true);
            }

        }

        public static Dictionary<string, string> LstNonEmergencyRole
        {
            get
            {
                return GetEmergencyAccessProperties(false);
            }

        }






        #region Helper Method
        private static string
            GetConfigurationProperties(string key,string setting="")
        {
           setting=string.IsNullOrEmpty(setting)? "AppSettings": "SMTPSettings";
           var appSettings = from configuration in XElement.Load(ConfigurationManager.AppSettings["ConfigurationPath"]).Elements(setting).Elements("add")
                                  where (string)configuration.Attribute("key").Value == key
                                  select new
                                  {
                                      value = configuration.Attribute("value").Value
                                  };
           
            string propertyValue = string.Empty;

            if (appSettings.Count() > 0)
            {
                propertyValue = appSettings.ElementAt(0).value;
            }
            return propertyValue;
        }

        private static Dictionary<string, string>
           GetEmergencyAccessProperties(bool status)
        {
            var EmergencyAccess = from configuration in XElement.Load(ConfigurationManager.AppSettings["ConfigurationPath"]).Elements("EmergencyAccess").Elements("add")
                                  select new
                                  {
                                      Key = configuration.Attribute("key").Value,
                                      value = configuration.Attribute("value").Value,
                                      Allowed = configuration.Attribute("Allowed").Value
                                  };
            Dictionary<string, string> _LstEmergencyRole = new Dictionary<string, string>();
            if (EmergencyAccess.Count() > 0)
            {
                foreach (var item in EmergencyAccess)
                {
                    if ( string.Compare(item.Allowed, status.ToString(),true)==0)
                    {
                        _LstEmergencyRole.Add(item.Key, item.value);
                    }
                   
                }
            }

            return _LstEmergencyRole;
        }


        #endregion Helper
    }
}