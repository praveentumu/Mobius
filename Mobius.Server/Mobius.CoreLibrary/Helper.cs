
using System.Resources;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System;
using FirstGenesis.Mobius.Logging;
using System.ComponentModel;
using System.Collections.Generic;
namespace Mobius.CoreLibrary
{
    public sealed class Helper
    {
        
        private static string _AdhocQueryType = "urn:uuid:14d4debf-8f97-4251-9a74-a90016b0af0d";
        private static string _SourceRepositoryId = "1";
        private static string _RepositoryUniqueId = "1";
        private static string _ReplaceString = "#Token";

        private const string KEY = "2%9*6#67";
        private const string IV = "2%9*6#67";

        private static string _DocumentSearchGateWayResponse = "urn:oasis:names:tc:ebxml-regrep:ResponseStatusType:Success";
        private static string _ResponseFailure = "urn:oasis:names:tc:ebxml-regrep:ResponseStatusType:Failure";

        private static string _Patient = "Patient";

        public static string RepositoryUniqueId { get { return _RepositoryUniqueId; } }

        public static string DocumentSearchGateWayResponseSuccess { get { return _DocumentSearchGateWayResponse; } }

        public static string ResponseFailure { get { return _ResponseFailure; } }

        public static string AdhocQueryTypeId { get { return _AdhocQueryType; } }
      
        public static string SourceRepositoryId { get { return _SourceRepositoryId; } }

        public static string Patient { get { return _Patient; } }

        public static string ReplaceString { get { return _ReplaceString; } }

        public static string GetErrorMessage(ErrorCode errorCode)
        {
            ResourceManager resourceManager = ResourceManager.CreateFileBasedResourceManager(MobiusAppSettingReader.ResourceFileName, MobiusAppSettingReader.ResourceFilePath, null);
            // retrieve the value of the specified key
            string errorMessage = resourceManager.GetString(errorCode.ToString());
            if (string.IsNullOrEmpty(errorMessage))
                errorMessage = "No error message provided!";

            return errorMessage;
        }
        /// <summary>
        /// Get IP Address
        /// </summary>
        /// <returns></returns>
        public static string GetIPAddress()
        {
            string ipAddress = string.Empty;
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    ipAddress = ip.ToString();
                }
            }
            return ipAddress;
        }

        /// <summary>
        /// Serialize response object
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static byte[] Serialize(object source)
        {
            var formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, source);
                return stream.ToArray();
            }
        }

        public static string EncryptData(string password)
        {
            if (!string.IsNullOrEmpty(password.Trim()))
            {
                byte[] arrKey = new byte[KEY.Length];
                byte[] arrIV = new byte[IV.Length];
                byte[] arrInputBytes = new byte[password.Length];
                arrKey = ASCIIEncoding.ASCII.GetBytes(KEY);
                arrIV = ASCIIEncoding.ASCII.GetBytes(IV);
                arrInputBytes = Encoding.UTF8.GetBytes(password);
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(arrKey, arrIV), CryptoStreamMode.Write))
                        {
                            cs.Write(arrInputBytes, 0, arrInputBytes.Length);
                            cs.FlushFinalBlock();
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            else
                return string.Empty;
        }

        public static string DecryptData(string encryptedPassword)
        {
            if (!string.IsNullOrEmpty(encryptedPassword.Trim()))
            {
                byte[] arrKey = new byte[KEY.Length];
                byte[] arrIV = new byte[IV.Length];
                byte[] arrInputBytes;
                arrKey = ASCIIEncoding.ASCII.GetBytes(KEY);
                arrIV = ASCIIEncoding.ASCII.GetBytes(IV);
                arrInputBytes = Convert.FromBase64String(encryptedPassword);
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(arrKey, arrIV), CryptoStreamMode.Write))
                        {
                            cs.Write(arrInputBytes, 0, arrInputBytes.Length);
                            cs.FlushFinalBlock();
                            return System.Text.Encoding.UTF8.GetString(ms.ToArray());
                        }
                    }
                }
            }
            else
                return string.Empty;
        }


        public static void LogError(Exception ex)
        {
            try
            {
                Logger logger = Logger.GetInstance();
                logger.WriteLog(LogSeverity.DEBUG, "Controller", ex.Message);
            }
            catch 
            {               
                            }
            
        }

        public static void LogError(string ex)
        {
            try
            {
                Logger logger = Logger.GetInstance();
                logger.WriteLog(LogSeverity.DEBUG, "Controller", ex);
            }
            catch
            {

            }

        }
    }


    public static class EnumHelper
    {

        /// <summary>
        /// Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        public static string GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
        {


            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            if (attributes != null && attributes.Length > 0)
                return ((DescriptionAttribute)attributes[0]).Description;
            else
                return string.Empty;
        }

        public static Enum GetEnumFromDescription<T>(Type enumType, string text) where T : System.Attribute
        {            
            var AllItem = Enum.GetValues(enumType);
         
            foreach (Enum item in AllItem)
            {
                var memInfo = enumType.GetMember(item.ToString());
                var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
                if (attributes != null && attributes.Length > 0)
                    if (((DescriptionAttribute)attributes[0]).Description.ToUpper() == text.ToUpper())
                       return item;
            }
            return null;
        }
        
    }


}
