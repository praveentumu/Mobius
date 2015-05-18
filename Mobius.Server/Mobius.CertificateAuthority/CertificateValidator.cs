namespace Mobius.CertificateAuthority
{
    using System;
    using System.IdentityModel.Selectors;
    using System.Security.Cryptography.X509Certificates;
    using Mobius.CoreLibrary;
    using System.Collections.Generic;
    using System.Security.Permissions;
    
    public class CertificateValidator : X509CertificateValidator
    {

        /// <summary>
        /// Get the an X.509 certificate.
        /// </summary>
        public X509Certificate2 X509Certificate2
        { get; private set; }


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


        /// <summary>
        /// This method would validates the X.509 certificate
        /// </summary>
        /// <param name="certificate">X509Certificate2 that represents the X.509 certificate to validate</param>
        public void Validate(byte[] certificateBytes)
        {
            try
            {
                X509Certificate2 certificate = new X509Certificate2(certificateBytes);
                this.Validate(certificate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method would validates the X.509 certificate
        /// </summary>
        /// <param name="certificate">X509Certificate2 that represents the X.509 certificate to validate</param>
        public override void Validate(X509Certificate2 certificate)
        {
            try
            {

                if (certificate == null)
                    throw new Exception(Helper.GetErrorMessage(ErrorCode.CertificateAuthority_Validate_Certificate_Missing));

                DateTime expirationDate = Convert.ToDateTime(certificate.GetExpirationDateString());     

                if(!certificate.IssuerName.Name.Equals(MobiusAppSettingReader.CAIssuerName, StringComparison.OrdinalIgnoreCase))
                {
                    ///check the certificate issuer name is same as Mobius server
                    throw new Exception(Helper.GetErrorMessage(ErrorCode.CertificateAuthority_Validate_CAIssuerName_Mismatch));                    
                }
                else if (expirationDate <=  DateTime.Now )
                {
                    ///check for certificate expiration date time
                    throw new Exception(Helper.GetErrorMessage(ErrorCode.CertificateAuthority_Validate_CAIssuerName_Mismatch));
                }                                
                else if (!this.VerifyCertificateRevocation(certificate))
                {
                    ///check for Verify Certificate Revocation
                    throw new Exception(Helper.GetErrorMessage(ErrorCode.CertificateAuthority_Validate_Certificate_Expired));                    
                }

                this.X509Certificate2 = certificate;
                this.PublicKey = certificate.GetPublicKeyString();
                this.SerialNumber = certificate.GetSerialNumberString();
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        /// <summary>
        /// This method would verift the certificate revocation 
        /// </summary>
        /// <param name="certificate">X509Certificate2 that represents the X.509 certificate to validate</param>
        /// <returns> success/fail based on the status of certificate passed the revocation check </returns>        
        private bool VerifyCertificateRevocation(X509Certificate2 certificate)
        {
            X509Chain certificateChain = new X509Chain(true);
            ///builds an X.509 chain using the policy specified in X509Certificates.X509ChainPolicy
            certificateChain.Build(certificate);
            /// Validate the certificate chain
            foreach (X509ChainStatus chainStatus in certificateChain.ChainStatus)
            {
                /// Check if the status pertains to a revoked certificate.
                if (chainStatus.Status == X509ChainStatusFlags.Revoked)
                {
                    return false;

                }               
            }
            return true;
        }
    }

    

}


