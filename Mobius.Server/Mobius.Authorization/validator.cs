#region Directives
using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
#endregion


namespace My.IdentityModel
{    
    /// <summary>
    /// Custom x509 certificate validator
    /// </summary>
    public class MyX509Validator : X509CertificateValidator
    {         
        /// <summary>
        /// Validate method
        /// </summary>
        /// <param name="certificate">This method would take requestor's X509Certificate as input parametr</param>
        public override void Validate(X509Certificate2 certificate)
        {            
            // Chain building check is being performed here
            //X509Chain chain = new X509Chain();
            //chain.ChainPolicy.RevocationMode = X509RevocationMode.Online; 
            //chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
            //chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(1000);
            //chain.ChainPolicy.VerificationFlags = X509VerificationFlags.NoFlag; 
            //chain.ChainPolicy.VerificationTime = DateTime.Now;
            //chain.Build(certificate);

            //if (!chain.Build(certificate))
            //{
            //    throw new SecurityTokenValidationException("Certificate validation failed when building chain.");
            //}
            //// Ends Chain building check
            //CertUtil CertUtil  = new CertUtil();
            //// Certificate revocation list is being checked here 
            //if (CertUtil.IsCertificateInCrl(certificate))
            //{             
            //    throw new SecurityTokenValidationException("Certificate is revoked by certificate authority.");
            //}
            //// Ends Certificate revocation list
        }    
    } 
} 
