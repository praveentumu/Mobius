// for refrence use this site :- http://geekswithblogs.net/shaunxu/archive/2012/01/13/working-with-active-directory-certificate-service-via-c.aspx

namespace Mobius.CertificateAuthority
{
    using System;
    using System.Security.Cryptography;
    using CERTCLIENTLib;
    using CERTENROLLLib;
    using Mobius.CoreLibrary;
    using Mobius.Entity;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Security.Cryptography.Pkcs;
    using System.IO;
    using System.Collections.Generic;
    using System.Security.Permissions;

    public class CertificateEnrollment
    {
        /// <summary>
        /// get the public key of certificate 
        /// </summary>
        public string PublicKey
        { get; private set; }

        /// <summary>
        /// get the serial number of certificate
        /// </summary>
        public string SerialNumber
        { get; private set; }

        public DateTime ExpirationDate
        { get; private set; }

        /// <summary>
        ///  this method will accepts the PKCS#10 request and cerate the certificate for the requestor 
        /// </summary>
        /// <param name="CSR">PKCS#10 request string </param>
        /// <param name="PKCS7Response">certificate </param>
        /// <returns></returns>
        public Result EnrollCertificateRequest(string CSR, out string PKCS7Response)
        {
            Result result = new Result();
            PKCS7Response = string.Empty;
            try
            {
                int CR_IN_BASE64 = 0x1;
                int CR_IN_FORMATANY = 0;
                int CR_OUT_BASE64 = 0x1;
                int CR_OUT_CHAIN = 0x100;
                result.IsSuccess = true;
                //  Create all the objects that will be required                
                CCertRequest certificateRequest = new CCertRequest();
                Disposition disposition = Disposition.Pending;
                // Submit the request
                disposition = (Disposition)certificateRequest.Submit(
                   CR_IN_BASE64 | CR_IN_FORMATANY,
                   CSR,
                   null,
                   MobiusAppSettingReader.CertificateAuthorityServerURL
               );

                // Certificate is issued then get the certificate from CA
                if (disposition == Disposition.Issued)
                {
                    // Get the certificate
                    PKCS7Response = certificateRequest.GetCertificate(
                        CR_OUT_BASE64 | CR_OUT_CHAIN
                    );


                    //var objEnroll = new CX509Enrollment();

                    //objEnroll.Initialize(X509CertificateEnrollmentContext.ContextUser);
                    //try
                    //{
                    //    objEnroll.InstallResponse(
                    //      InstallResponseRestrictionFlags.AllowUntrustedRoot,
                    //     PKCS7Response,
                    //     EncodingType.XCN_CRYPT_STRING_BASE64,
                    //      null);
                    //}
                    //catch (Exception)
                    //{
                    //    //Nothing to do: 
                    //}

                }
                else if (disposition == Disposition.Pending)
                {
                    result.IsSuccess = false;
                    result.SetError(ErrorCode.CertificateAuthority_Enrollment_Pending);
                }
                else
                {
                    result.IsSuccess = false;
                    result.SetError(ErrorCode.CertificateAuthority_Enrollment_Fail);
                }

            }
            catch (Exception ex)
            {
                result.SetError(ErrorCode.UnknownException, ex.Message);
                result.IsSuccess = false;
            }
            return result;
        }
        public Result EnrollRenewCertificationRequest(string PKCS7Request, out int EnrollId, out string PKCS7Response)
        {
            Result result = new Result();
            PKCS7Response = string.Empty;
            EnrollId = 99;
            ICertRequest2 objCertRequest = new CCertRequest();
            const int CR_IN_BASE64 = 0x1;
            const int CR_IN_FORMATANY = 0;
            const int CR_DISP_ISSUED = 0x3;
            const int CR_DISP_UNDER_SUBMISSION = 0x5;
            const int CR_OUT_BASE64 = 0x1;
            const int CR_OUT_CHAIN = 0x100;
            try
            {
                var iDisposition = objCertRequest.Submit(

                        CR_IN_BASE64 | CR_IN_FORMATANY,
                        PKCS7Request,
                        string.Empty, MobiusAppSettingReader.CertificateAuthorityServerURL);
                switch (iDisposition)
                {

                    case CR_DISP_ISSUED:
                    case CR_DISP_UNDER_SUBMISSION:
                        result.IsSuccess = true;
                        EnrollId = objCertRequest.GetRequestId();
                        PKCS7Response = objCertRequest.GetCertificate(CR_OUT_BASE64 | CR_OUT_CHAIN);
                        // Console.WriteLine("The certificate had been issued.");
                        break;

                    default:
                        result.IsSuccess = false;
                        result.SetError(ErrorCode.CertificateAuthority_Enrollment_Pending);
                        break;
                }
            }
            catch (Exception ex)
            {
                result.SetError(ErrorCode.UnknownException, ex.Message);
                result.IsSuccess = false;

            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pcks7Response"></param>
        /// <returns></returns>
        public Result GetCertificateInformation(string pcks7Response)
        {
            //if (Environment.OSVersion.Version.Major >= 6)
            //{
            //    return this.GetCertificateInformation(Encoding.UTF8.GetBytes(pcks7Response));
            //}
            //else
            //{
            return this.GetCertificateInformationViaFile(pcks7Response);
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public Result GetCertificateInformation(byte[] certifcateByteData)
        {
            Result result = new Result();
            string content = string.Empty;
            try
            {
                result.IsSuccess = true;

                content = Encoding.UTF8.GetString(certifcateByteData);
                string base64Content = content.Replace("-----BEGIN CERTIFICATE-----", "").Replace("-----END CERTIFICATE-----", "").Replace("\r", "").Replace("\n", "");
                byte[] decodedContent = Convert.FromBase64String(base64Content);
                SignedCms certContainer = new SignedCms();
                certContainer.Decode(certifcateByteData);

                if (certContainer.Certificates.Count > 0)
                {
                    X509Certificate2 X509Certificate2 = certContainer.Certificates[0];
                    this.PublicKey = X509Certificate2.GetPublicKeyString();
                    this.SerialNumber = X509Certificate2.SerialNumber;
                    this.ExpirationDate = X509Certificate2.NotAfter;
                }

            }
            catch (Exception)
            {
                result = this.GetCertificateInformationViaFile(content);
            }

            return result;
        }

        #region GetCertificateFromPKCS
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pcks7Response"></param>
        /// <returns></returns>
        public Result GetCertificateFromPKCS(string pcks7Response, out byte[] certificateRawData)
        {
            return this.GetCertificateFromPKCS(Encoding.UTF8.GetBytes(pcks7Response), out certificateRawData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="certifcateByteData"></param>
        /// <returns></returns>
        public Result GetCertificateFromPKCS(byte[] certifcateByteData, out byte[] certificateRawData)
        {
            Result result = new Result();
            certificateRawData = null;
            try
            {
                result.IsSuccess = true;
                if (Environment.OSVersion.Version.Major >= 6)
                {
                }
                string content = Encoding.UTF8.GetString(certifcateByteData);
                string base64Content = content.Replace("-----BEGIN CERTIFICATE-----", "").Replace("-----END CERTIFICATE-----", "").Replace("\r", "").Replace("\n", "");
                byte[] decodedContent = Convert.FromBase64String(base64Content);
                SignedCms certContainer = new SignedCms();
                certContainer.Decode(decodedContent);

                if (certContainer.Certificates.Count > 0)
                {
                    certificateRawData = certContainer.Certificates[0].Export(X509ContentType.Cert);
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return result;
        }
        #endregion GetCertificateFromPKCS


        /// <summary>
        /// This method would validate the CSR(PCKS10 request) string
        /// </summary>
        /// <param name="CSR">PCKS10</param>
        /// <returns>Result Object</returns>
        public Result ValidateEnrollmentRequest(string CSR)
        {
            Result result = new Result();
            try
            {
                if (string.IsNullOrWhiteSpace(CSR))
                {
                    //TODO add valid enum check 
                    result.IsSuccess = false;
                    result.SetError(ErrorCode.CertificateAuthority_CSR_Missing);
                }
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    CX509CertificateRequestPkcs10 CX509CertificateRequestPkcs10 = new CX509CertificateRequestPkcs10();
                    CX509CertificateRequestPkcs10.InitializeDecode(CSR, EncodingType.XCN_CRYPT_STRING_BASE64_ANY);
                    CX509CertificateRequestPkcs10.CheckSignature(Pkcs10AllowedSignatureTypes.AllowedKeySignature);
                }
                result.IsSuccess = true;
            }
            catch (CryptographicException CE)
            {
                result.SetError(ErrorCode.UnknownException, CE.Message);
                result.IsSuccess = false;

            }
            catch (Exception ex)
            {
                result.SetError(ErrorCode.UnknownException, ex.Message);
                result.IsSuccess = false;
            }
            return result;
        }


        /// <summary>
        /// This method would validate the CSR(PCKS10 request) string
        /// </summary>
        /// <param name="CSR">PCKS10</param>
        /// <returns>Result Object</returns>
        public Result ValidateEnrollmentRequestForCS07(string CSR)
        {
            Result result = new Result();
            try
            {
                if (string.IsNullOrWhiteSpace(CSR))
                {
                    //TODO add valid enum check 
                    result.IsSuccess = false;
                    result.SetError(ErrorCode.CertificateAuthority_CSR_Missing);
                }
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    CX509CertificateRequestPkcs7 CX509CertificateRequestPkcs07 = new CX509CertificateRequestPkcs7();
                    CX509CertificateRequestPkcs07.InitializeDecode(CSR, EncodingType.XCN_CRYPT_STRING_BASE64_ANY);
                    //CX509CertificateRequestPkcs10.CheckCertificateSignature(Pkcs10AllowedSignatureTypes.AllowedKeySignature);
                    CX509CertificateRequestPkcs07.CheckCertificateSignature(true);
                }
                result.IsSuccess = true;
            }
            catch (CryptographicException CE)
            {
                result.SetError(ErrorCode.UnknownException, CE.Message);
                result.IsSuccess = false;

            }
            catch (Exception ex)
            {
                result.SetError(ErrorCode.UnknownException, ex.Message);
                result.IsSuccess = false;
            }
            return result;
        }


        /// <summary>
        /// GetCertificateInformationViaFile
        /// </summary>
        /// <param name="pcks7Response"></param>
        /// <returns></returns>
        private Result GetCertificateInformationViaFile(string pcks7Response)
        {
            Result result = new Result();
            List<string> pcks7Responses = new List<string>();
            string pcks7ResponsesContent = string.Empty;
            string tempFile = string.Empty;
            byte[] pcks7Bytes = null;
            string clientCertificate = string.Empty;
            try
            {
                result.IsSuccess = false; ;
                pcks7Responses.Add(pcks7Response);
                tempFile = MobiusAppSettingReader.TempPath + Guid.NewGuid().ToString() + ".txt";
                File.WriteAllLines(tempFile, pcks7Responses);
                pcks7Bytes = System.IO.File.ReadAllBytes(tempFile);
                pcks7ResponsesContent = Encoding.UTF8.GetString(pcks7Bytes);
                string base64Content = pcks7ResponsesContent.Replace("-----BEGIN CERTIFICATE-----", "").Replace("-----END CERTIFICATE-----", "").Replace("\r", "").Replace("\n", "");
                byte[] decodedContent = Convert.FromBase64String(base64Content);
                // content contiene il file da firmare
                SignedCms certContainer = new SignedCms();
                certContainer.Decode(decodedContent);
                if (certContainer.Certificates.Count > 0)
                {
                    X509Certificate2 X509Certificate2 = certContainer.Certificates[0];
                    this.PublicKey = X509Certificate2.GetPublicKeyString();
                    this.SerialNumber = X509Certificate2.SerialNumber;
                    this.ExpirationDate = X509Certificate2.NotAfter;
                    result.IsSuccess = true;
                    //  X509Certificate2 X509Certificate3 = certContainer.Certificates[1];
                }
            }
            catch (Exception ex)
            {
                Helper.LogError(ex);
                result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            finally
            {
                File.Delete(tempFile);
            }
            return result;
        }
    }
}
