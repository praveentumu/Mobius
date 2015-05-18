using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CERTCLIENTLib;
using Mobius.CoreLibrary;
using CERTENROLLLib;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Mobius.CoreLibrary;
/// <summary>
/// Summary description for CertUtility
/// </summary>
namespace FirstGenesis.UI.Base
{
    public partial class BaseClass : System.Web.UI.Page
    {
        #region Certificate Methods

        #region GenerateCSR

        //http://msdn.microsoft.com/en-us/library/windows/desktop/aa386991(v=vs.85).aspx
        protected string GenerateCSR(string emailAddress, string commonName, string organizationalUnit, string organizationName, string city, string state, string country)
        {
            // for reference use this site :- http://geekswithblogs.net/shaunxu/archive/2012/01/13/working-with-active-directory-certificate-service-via-c.aspx
            //  Create all the objects that will be required

            CX509CertificateRequestPkcs10 objPkcs10 = new CX509CertificateRequestPkcs10Class();
            CX509PrivateKey objPrivateKey = new CX509PrivateKeyClass();
            CCspInformation objCSP = new CCspInformationClass();
            CCspInformations objCSPs = new CCspInformationsClass();
            CX500DistinguishedName objDN = new CX500DistinguishedNameClass();
            CX509Enrollment objEnroll = new CX509EnrollmentClass();
            CObjectIds objObjectIds = new CObjectIdsClass();
            CObjectId objObjectId = new CObjectIdClass();
            CX509ExtensionKeyUsage objExtensionKeyUsage = new CX509ExtensionKeyUsageClass();
            CX509ExtensionEnhancedKeyUsage objX509ExtensionEnhancedKeyUsage = new CX509ExtensionEnhancedKeyUsageClass();
            string CSRRequeststring = string.Empty;
            try
            {
                //  Initialize the csp object using the desired Cryptographic Service Provider (CSP)
                objCSP.InitializeFromName(
                    "Microsoft Enhanced Cryptographic Provider v1.0"
                );

                //  Add this CSP object to the CSP collection object
                objCSPs.Add(
                    objCSP
                );


                //  Provide key container name, key length and key spec to the private key object
                //objPrivateKey.ContainerName = "AlejaCMa";
                objPrivateKey.Length = 2048;
                objPrivateKey.KeySpec = X509KeySpec.XCN_AT_SIGNATURE;
                objPrivateKey.KeyUsage = X509PrivateKeyUsageFlags.XCN_NCRYPT_ALLOW_ALL_USAGES;
                //This will enable the private key export check box.
                objPrivateKey.ExportPolicy = X509PrivateKeyExportFlags.XCN_NCRYPT_ALLOW_EXPORT_FLAG;
                objPrivateKey.KeySpec = X509KeySpec.XCN_AT_KEYEXCHANGE;
                objPrivateKey.MachineContext = false;

                //  Provide the CSP collection object (in this case containing only 1 CSP object)
                //  to the private key object

                objPrivateKey.CspInformations = objCSPs;

                //  Create the actual key pair
                objPrivateKey.Create();

                //  Initialize the PKCS#10 certificate request object based on the private key.
                //  Using the context, indicate that this is a user certificate request and don't
                //  provide a template name

                objPkcs10.InitializeFromPrivateKey(
                    X509CertificateEnrollmentContext.ContextUser,
                    objPrivateKey,
                    ""
                );

                // Key Usage Extension 
                objExtensionKeyUsage.InitializeEncode(
                   CERTENROLLLib.X509KeyUsageFlags.XCN_CERT_DIGITAL_SIGNATURE_KEY_USAGE |
                    CERTENROLLLib.X509KeyUsageFlags.XCN_CERT_NON_REPUDIATION_KEY_USAGE |
                    CERTENROLLLib.X509KeyUsageFlags.XCN_CERT_KEY_ENCIPHERMENT_KEY_USAGE |
                    CERTENROLLLib.X509KeyUsageFlags.XCN_CERT_DATA_ENCIPHERMENT_KEY_USAGE
                );
                objPkcs10.X509Extensions.Add((CX509Extension)objExtensionKeyUsage);

                // Enhanced Key Usage Extension
                objObjectId.InitializeFromValue("1.3.6.1.5.5.7.3.2"); // OID for Client Authentication usage
                objObjectIds.Add(objObjectId);

                objX509ExtensionEnhancedKeyUsage.InitializeEncode(objObjectIds);
                objPkcs10.X509Extensions.Add((CX509Extension)objX509ExtensionEnhancedKeyUsage);

                //DIL test case expects country code as USA but CA need only character value for country 
                if (country.ToUpper() == "USA") country = "US";
                //Max length CA can allow is 64 in commonName
                if (commonName.Length > 64) commonName = commonName.Substring(0, 64);

                //CA does not ally following escape characters, so we are checking the existence of escape character and then replace if exists 
                string validString = @"^([a-zA-Z0-9\s.']{1,255})$";

                Match matchInput = System.Text.RegularExpressions.Regex.Match(commonName, validString);

                if (matchInput.Success)
                {
                    List<char> escapeCharaters = new List<char>() { '<', '>', '~', '!', '@', '#', '$', '%', '^', '*', '/', '\\', '(', ')', '?', '&' };
                    List<Char> NameInArray = commonName.ToArray().ToList();
                    //Remove all escapeCharaters
                    NameInArray.RemoveAll(t => escapeCharaters.Any(a => a == t));

                    commonName = string.Join("", NameInArray);
                }

                //  Encode the name in using the Distinguished Name object
                objDN.Encode(
                    "E = " + emailAddress + ";CN = " + commonName + ";OU = " + organizationalUnit + ";O = " + organizationName + ";L =" + city + ";S =" + state + ";C = " + country,
                    X500NameFlags.XCN_CERT_NAME_STR_NONE
                );

                //  Assing the subject name by using the Distinguished Name object initialized above
                objPkcs10.Subject = objDN;

                // Create enrollment request
                objEnroll.InitializeFromRequest(objPkcs10);
                CSRRequeststring = objEnroll.CreateRequest(
                    EncodingType.XCN_CRYPT_STRING_BASE64
                );



            }
            catch (Exception ex)
            {
                throw ex;
            }
            return CSRRequeststring;
        }
        #endregion

        #region IsExpired

        protected bool IsExpired(X509Certificate2 Certificate)
        {
            if (Certificate.NotAfter < DateTime.Now)
            {
                return true;
            }
            else
            {
                GlobalSessions.SessionAdd(SessionItem.ValidTill, Certificate.NotAfter);
                return false;
            }

        }

        #endregion IsExpired




        #region DeleteExistingCertificate
        protected bool DeleteExistingCertificate()
        {
            X509Store store = null;
            bool isDeleted = false;
            try
            {
                store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadWrite);
                X509Certificate2 _ExistingCertificate = null;
                if (objProxy.ClientCredentials.ClientCertificate.Certificate != null)
                {
                    _ExistingCertificate = objProxy.ClientCredentials.ClientCertificate.Certificate;
                    if (store.Certificates.Contains(_ExistingCertificate))
                    {
                        store.Remove(_ExistingCertificate);
                        isDeleted= true;
                    }
                    else
                        isDeleted= false;
                }

            }
            catch (Exception ex)
            {
                isDeleted = false;
                throw ex;
                
            }
            finally
            {
                store.Close();
            }
            return isDeleted;

        }
        #endregion

      



        #region InstallClientCertificate
        /// <summary>
        /// install client certificate into user store 
        /// </summary>
        /// <param name="pKCS7Response"></param>
        protected bool InstallClientCertificate(string pKCS7Response)
        {
            string pfxResponse = string.Empty;
            dynamic objEnroll = new CX509Enrollment();


            try
            {
                objEnroll.Initialize(X509CertificateEnrollmentContext.ContextUser);
                objEnroll.InstallResponse(
                  InstallResponseRestrictionFlags.AllowUntrustedRoot,
                 pKCS7Response,
                 EncodingType.XCN_CRYPT_STRING_BASE64,
                  null);
                return true;
            }
            catch (Exception)
            {
                //Nothing to do: 
                return false;
            }
            finally
            {

            }

        }

        #endregion InstallClientCertificate

        #endregion Methods

    }

}