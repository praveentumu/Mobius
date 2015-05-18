//------------------------------------------------------------------------------
//     
//    
//
//   
//   
//------------------------------------------------------------------------------


namespace MobiusServiceUtility
{
    #region namespace
    using System.Text;
    using System.IO;
    using System.Security.Cryptography.X509Certificates;
    using System;
    using System.Security.Cryptography;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Xml.XPath;
    using Mobius.CoreLibrary;
    using System.Reflection;
    using System.Collections.Generic;
    using System.Web;
    #endregion

    /// <summary>
    /// 
    /// </summary>
    public class SoapHandler
    {
        #region Constaint
        const string CUSTOM_HEADER_NAME_KEY = "Key";
        const string CUSTOM_HEADER_NAME_IV = "IV";
        const string CUSTOM_HEADER_NAME_HASH = "HASH";
        const string CUSTOM_HEADER_NAMESPACE = "MobiusHISE";
        public bool IsEncryption = MobiusAppSettingReader.EncryptionFlag;
        private string _certificateSerialNumber = string.Empty;
        #endregion

        #region Property

        public bool x509FindBySubjectName
        { get; set; }

        public SoapHandler()
        { }

        public SoapHandler(string certificateSerialNumber)
        {
            this._certificateSerialNumber = certificateSerialNumber;
        }

        public string SerialNumber
        {
            get
            {
                return this._certificateSerialNumber;
            }
            private set
            {
                _certificateSerialNumber = value;
            }
        }

        #endregion

        #region PublicMethod
        /// <summary>
        /// Encryption of Client Request  
        /// </summary>
        /// <param name="requestObj"></param>
        /// <param name="soapProperties"></param>
        public void RequestEncryption(object requestObj, out SoapProperties soapProperties)
        {
            try
            {
                soapProperties = new SoapProperties();

                if (IsEncryption && requestObj != null)
                {
                    RSACryptoServiceProvider clientPrivateKEy = null;
                    X509Certificate2 ServerCertificate = null;
                    AsymmetricAlgorithm serverPublicKey = null;
                    byte[] EncryptedData = null;
                    byte[] requestObject = null;
                    byte[] encryptedSessionKey = null;
                    byte[] iv = null;
                    byte[] SigneEncryptedData = null;

                    string servercertificate = GetServerCertificate();

                    ServerCertificate = new X509Certificate2(Convert.FromBase64String(servercertificate));
                    serverPublicKey = ServerCertificate.PublicKey.Key;
                    // Serialize request object
                    requestObject = this.SerializeRequest(requestObj);
                    // get Client certificate private key for signing
                    clientPrivateKEy = GetClientCertificatePrivateKey(this.SerialNumber);
                    // Signe request object 
                    SigneEncryptedData = SignCertificate(requestObject, clientPrivateKEy);
                    // Encrypt request with server public key
                    EncryptedData = this.EncryptObject(serverPublicKey, SigneEncryptedData, out iv, out encryptedSessionKey);
                    // Add key in SOAP Header
                    if (encryptedSessionKey.Length > 0)
                    {
                        soapProperties.Key = Convert.ToBase64String(encryptedSessionKey);
                    }

                    // Add IV in SOAP Header
                    if (iv.Length > 0)
                    {
                        soapProperties.IV = Convert.ToBase64String(iv);
                    }

                    // Add SAOP Request data in SOAP Header
                    if (EncryptedData.Length > 0)
                    {
                        soapProperties.SignedData = Convert.ToBase64String(EncryptedData);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Decryption of Client Request  
        /// </summary>
        /// <param name="soapPropertiesObj"></param>
        /// <param name="_dataObj"></param>
        /// <param name="clientPublicKey"></param>
        /// <returns></returns>
        public bool RequestDecryption(SoapProperties soapPropertiesObj, object _dataObj, AsymmetricAlgorithm clientPublicKey)
        {
            bool verifyStatus = false;
            try
            {
                if (IsEncryption && soapPropertiesObj != null && soapPropertiesObj != null)
                {
                    AsymmetricAlgorithm serverpublicKey = null;
                    RSACryptoServiceProvider rSACryptoServiceProvider = null;
                    string strEncryptedData = string.Empty;
                    byte[] SessionKey = null;
                    byte[] iv = null;
                    byte[] requestObject = null;

                    iv = Convert.FromBase64String(soapPropertiesObj.IV);
                    SessionKey = Convert.FromBase64String(soapPropertiesObj.Key);
                    requestObject = Convert.FromBase64String(soapPropertiesObj.SignedData);
                    // get server private Key
                    rSACryptoServiceProvider = this.GetServerCertificatePrivateKey(out serverpublicKey);
                    // Decrypt Object through iv and encryptedSessionKey and server private Key.
                    requestObject = this.DecryptObject(iv, SessionKey, requestObject, rSACryptoServiceProvider);
                    // Verify request Signe data with client PublicKey
                    verifyStatus = this.VerifyData(requestObject, _dataObj, clientPublicKey);
                }
                else
                {
                    verifyStatus = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return verifyStatus;
        }
        /// <summary>
        /// Encryption of Client Response
        /// </summary>
        /// <param name="responseObj"></param>
        /// <param name="soapProperties"></param>
        /// <param name="clientPublicKey"></param>
        public void ResponseEncryption(object responseObj, out SoapProperties soapProperties, AsymmetricAlgorithm clientPublicKey)
        {
            try
            {
                soapProperties = new SoapProperties();

                if (IsEncryption && responseObj != null)
                {
                    AsymmetricAlgorithm serverPublicKey = null;
                    RSACryptoServiceProvider rSACrypServerPrivateKey = null;
                    byte[] EncryptedData = null;
                    byte[] requestObject = null;
                    byte[] Key = null;
                    byte[] iv = null;
                    byte[] signeData = null;

                    // Serialize request object
                    requestObject = this.SerializeRequest(responseObj);

                    // get server certificate private key for signing
                    rSACrypServerPrivateKey = GetServerCertificatePrivateKey(out serverPublicKey);
                    // signe request object 
                    signeData = SignCertificate(requestObject, rSACrypServerPrivateKey);

                    // Encrypt request with server public key
                    EncryptedData = this.EncryptObject(clientPublicKey, signeData, out iv, out Key);

                    // Add key in SOAP Header
                    if (Key.Length > 0)
                    {
                        soapProperties.Key = Convert.ToBase64String(Key);
                    }

                    // Add IV in SOAP Header
                    if (iv.Length > 0)
                    {
                        soapProperties.IV = Convert.ToBase64String(iv);
                    }

                    // Add SAOP Request data in SOAP Header
                    if (EncryptedData.Length > 0)
                    {
                        soapProperties.SignedData = Convert.ToBase64String(EncryptedData);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Decryption of Client Response  
        /// </summary>
        /// <param name="soapProperties"></param>
        /// <param name="responseObj"></param>
        /// <returns></returns>
        public bool ResponseDecryption(SoapProperties soapProperties, object responseObj)
        {
            bool verifyStatus = false;
            try
            {
                if (IsEncryption && responseObj != null && soapProperties != null)
                {
                    RSACryptoServiceProvider clientPrivateKey = null;
                    AsymmetricAlgorithm serverPublicKey = null;
                    byte[] encryptedSessionKeyi = null;
                    byte[] ivi = null;
                    byte[] SignedDatai = null;
                    byte[] SignedData = null;

                    ivi = Convert.FromBase64String(soapProperties.IV);
                    encryptedSessionKeyi = Convert.FromBase64String(soapProperties.Key);
                    SignedDatai = Convert.FromBase64String(soapProperties.SignedData);

                    // get client private key for descry
                    clientPrivateKey = GetClientCertificatePrivateKey(this.SerialNumber);
                    // Decrypt Object through iv and encryptedSessionKey and server private Key.
                    SignedData = this.DecryptObject(ivi, encryptedSessionKeyi, SignedDatai, clientPrivateKey);
                    // get server private Key
                    this.GetServerCertificatePrivateKey(out serverPublicKey);
                    // Verify request singe data with client PublicKey
                    verifyStatus = this.VerifyData(SignedData, responseObj, serverPublicKey);
                }
                else
                {
                    verifyStatus = true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return verifyStatus;
        }

        #endregion

        #region PrivateMethod


        /// <summary>
        ///  Encrypt Request/response object
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="secretMessage"></param>
        /// <param name="iv"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        private byte[] EncryptObject(AsymmetricAlgorithm publicKey, byte[] secretMessage, out byte[] iv, out byte[] Key)
        {
            using (Aes aes = new AesCryptoServiceProvider())
            {
                iv = aes.IV;
                RSAPKCS1KeyExchangeFormatter keyFormatter = new RSAPKCS1KeyExchangeFormatter((RSACryptoServiceProvider)publicKey);
                Key = keyFormatter.CreateKeyExchange(aes.Key, typeof(Aes));
                using (MemoryStream ciphertext = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(ciphertext, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(secretMessage, 0, secretMessage.Length);
                        cryptoStream.Close();
                        ASCIIEncoding encodingh = new ASCIIEncoding();
                        return ciphertext.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// Decrypt Request/response object
        /// </summary>
        /// <param name="iv"></param>
        /// <param name="Key"></param>
        /// <param name="encryptedMessageByte"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        private byte[] DecryptObject(byte[] iv, byte[] Key, byte[] encryptedMessageByte, RSACryptoServiceProvider privateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] bytearray = null;
            using (Aes aes = new AesCryptoServiceProvider())
            {
                aes.IV = iv;
                RSAPKCS1KeyExchangeDeformatter keyDeformatter = new RSAPKCS1KeyExchangeDeformatter(privateKey);
                bytearray = keyDeformatter.DecryptKeyExchange(Key);
                aes.Key = bytearray;

                using (MemoryStream plaintext = new MemoryStream())

                using (CryptoStream cs = new CryptoStream(plaintext, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {

                    cs.Write(encryptedMessageByte, 0, encryptedMessageByte.Length);
                    cs.Close();
                    return plaintext.ToArray();
                }

            }
        }

        /// <summary>
        ///  Verify services request/response Data
        /// </summary>
        /// <param name="SignedData"></param>
        /// <param name="originalData"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        private bool VerifyData(byte[] SignedData, object originalData, AsymmetricAlgorithm publicKey)
        {
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)publicKey;
            UnicodeEncoding encoding = new UnicodeEncoding();
            SoapProperties soapProperties = null;
            // Type type = originalData.GetType();
            var barProperty = originalData.GetType().GetProperty("SoapProperties");
            barProperty.SetValue(originalData, soapProperties, null);
            byte[] OriginalMessage1 = this.SerializeRequest(originalData);
            SHA1Managed sha1 = new SHA1Managed();
            byte[] hash = sha1.ComputeHash(OriginalMessage1);
            return csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), SignedData);
        }

        /// <summary>
        /// signe certificate via client and server private Key
        /// </summary>
        /// <param name="encryptedText"></param>
        /// <param name="csp"></param>
        /// <returns></returns>
        private byte[] SignCertificate(byte[] encryptedText, RSACryptoServiceProvider csp)
        {
            SHA1Managed sha1 = new SHA1Managed();
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] hash = sha1.ComputeHash(encryptedText);
            return csp.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));

        }
        /// <summary>
        /// Get Client Certificate PrivateKey for signing certificate
        /// </summary>
        /// <returns></returns>
        private RSACryptoServiceProvider GetClientCertificatePrivateKey(string SerialNumber)
        {
            RSACryptoServiceProvider rSACryptoServiceProvider = null;
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certCollection = null;

            if (!string.IsNullOrEmpty(SerialNumber))
            {
                certCollection = store.Certificates.Find(X509FindType.FindBySerialNumber, SerialNumber, true);

                if (certCollection != null)
                {
                    X509Certificate2 cert = new X509Certificate2(certCollection[0]);
                    rSACryptoServiceProvider = (RSACryptoServiceProvider)cert.PrivateKey;
                }
            }

            return rSACryptoServiceProvider;
        }



        /// <summary>
        /// Get server Certificate PrivateKey and pulic key for signing certificate
        /// </summary>
        /// <param name="ServerPublicKey"></param>
        /// <returns></returns>
        private RSACryptoServiceProvider GetServerCertificatePrivateKey(out AsymmetricAlgorithm ServerPublicKey)
        {
            ServerPublicKey = null;
            RSACryptoServiceProvider rSACryptoServiceProvider = null;
            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certCollection = null;
            X509Certificate2 cert = null;
            string serverCertificateName = string.Empty;
            serverCertificateName = this.GetServerCertificateName();
            if (!string.IsNullOrWhiteSpace(serverCertificateName))
            {
                if (this.x509FindBySubjectName)
                {
                    certCollection = store.Certificates.Find(X509FindType.FindBySubjectName, serverCertificateName, true);
                }
                else
                {
                    certCollection = store.Certificates.Find(X509FindType.FindByIssuerName, serverCertificateName, true);
                }
            }
            else
            {
                List<string> certificateNames = new List<string>();
                certificateNames = this.GetServerCertificateNamesFromClientConfig();
                if (certificateNames.Count == 2)
                {
                    try
                    {
                        certCollection = null;
                        certCollection = store.Certificates.Find(X509FindType.FindBySubjectName, certificateNames[0].ToString(), true);
                    }
                    catch
                    {
                        certCollection = null;
                        certCollection = store.Certificates.Find(X509FindType.FindByIssuerName, certificateNames[1].ToString(), true);
                    }
                    certificateNames = null;
                }
            }

            if (certCollection != null)
            {
                cert = new X509Certificate2(certCollection[0]);
                ServerPublicKey = cert.PublicKey.Key;
                rSACryptoServiceProvider = (RSACryptoServiceProvider)cert.PrivateKey;
            }

            return rSACryptoServiceProvider;

        }

        /// <summary>
        /// Serialize Request object
        /// </summary>
        /// <param name="_Object">object</param>
        /// <returns>byte array</returns>
        private byte[] SerializeRequest(object _Object)
        {
            return XmlSerializerHelper.ObjectToByteArray(_Object);
        }
        /// <summary>
        /// Get Server Certificate via Web config
        /// </summary>
        /// <returns></returns>
        private string GetServerCertificate()
        {
            string configFile = string.Empty;
            string serverCertificate = string.Empty;
            configFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            configFile = configFile.Substring(configFile.IndexOf("\\") + 1);
            configFile = configFile.Substring(0, configFile.LastIndexOf('\\')) + "\\web.config";
            if (File.Exists(configFile))
            {
                XPathDocument doc = new XPathDocument(configFile);
                foreach (XPathNavigator child in doc.CreateNavigator().Select("configuration/system.serviceModel/client/endpoint/identity"))
                {
                    serverCertificate = child.SelectSingleNode("certificate/@encodedValue").Value;
                }
            }
            return serverCertificate;
        }
        private string GetClientCertificateName()
        {
            string configFile = string.Empty;
            string clientCertificate = string.Empty;
            configFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            configFile = configFile.Substring(configFile.IndexOf("\\") + 1);
            configFile = configFile.Substring(0, configFile.LastIndexOf('\\')) + "\\web.config";
            this.x509FindBySubjectName = false;
            if (File.Exists(configFile))
            {
                XPathDocument doc = new XPathDocument(configFile);
                foreach (XPathNavigator child in doc.CreateNavigator().Select("configuration/system.serviceModel/behaviors/endpointBehaviors/behavior/clientCredentials"))
                {
                    if (child.SelectSingleNode("clientCertificate/@x509FindType").Value.Equals("FindBySubjectName"))
                    {
                        this.x509FindBySubjectName = true;
                    }
                    clientCertificate = child.SelectSingleNode("clientCertificate/@findValue").Value;
                }
            }
            return clientCertificate;
        }
        private string GetServerCertificateName()
        {
            string configFile = string.Empty;
            string clientCertificate = string.Empty;
            configFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            configFile = configFile.Substring(configFile.IndexOf("\\") + 1);
            configFile = configFile.Substring(0, configFile.LastIndexOf('\\')) + "\\web.config";
            this.x509FindBySubjectName = false;
            if (File.Exists(configFile))
            {
                XPathDocument doc = new XPathDocument(configFile);
                foreach (XPathNavigator child in doc.CreateNavigator().Select("configuration/system.serviceModel/behaviors/serviceBehaviors/behavior/serviceCredentials"))
                {
                    if (child.SelectSingleNode("serviceCertificate/@x509FindType").Value.Equals("FindBySubjectName"))
                    {
                        this.x509FindBySubjectName = true;
                    }
                    clientCertificate = child.SelectSingleNode("serviceCertificate/@findValue").Value;
                }
            }
            return clientCertificate;
        }


        private List<string> GetServerCertificateNamesFromClientConfig()
        {
            List<string> certificateNames = new List<string>();
            string servercertificateName = GetServerCertificate();
            string servercertificateByIssuerName = string.Empty;
            if (!string.IsNullOrEmpty(servercertificateName))
            {
                X509Certificate2 ServerCertificate = new X509Certificate2(Convert.FromBase64String(servercertificateName));
                string[] certificateBySubjectName = ServerCertificate.SubjectName.Name.Split(',');
                string[] certificateByIssuerName = ServerCertificate.IssuerName.Name.Split(',');
                if (certificateBySubjectName.Length > 0)
                {
                    servercertificateName = certificateBySubjectName[1];
                    servercertificateName = servercertificateName.Substring(servercertificateName.LastIndexOf('=') + 1);
                    certificateNames.Add(servercertificateName);
                }
                if (certificateByIssuerName.Length > 0)
                {
                    servercertificateByIssuerName = certificateByIssuerName[0];
                    servercertificateByIssuerName = servercertificateByIssuerName.Substring(servercertificateByIssuerName.LastIndexOf('=') + 1);
                    certificateNames.Add(servercertificateByIssuerName);
                }
            }
            return certificateNames;
        }
        #endregion
    }
}
