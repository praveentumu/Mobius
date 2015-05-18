using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Xml.Linq;
using Mobius.CoreLibrary;
using Mobius.Entity;
using MobiusServiceLibrary;
using My.IdentityModel;

namespace Mobius.Server.MobiusHISEService
{
    public class AuthorizationPolicy : IAuthorizationPolicy
    {
        const string UNKNOWN = "Unknown";
        const string MOBIUS = "Mobius";
        const string NAME = "name";

        string id;
        /// <summary>
        /// 
        /// </summary>
        public string SerialNumber
        {
            get;
            set;
        }

        public AuthorizationPolicy()
        {
            id = Guid.NewGuid().ToString();
        }

        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            #region Variables
            bool bRet = false;
            CustomAuthState customstate = null;
            const string Invalid = "InvalidCertificate";
            const string message = "Certificate is invalid.";
            const string InvalidUser = "InvalidUser";
            const string messageUser = "Invalid User";
            #endregion

            if (evaluationContext.ClaimSets.Count > 0)
            {
                X509CertificateClaimSet certificateClaimSet = evaluationContext.ClaimSets[0] as X509CertificateClaimSet;
                X509Certificate2 certificate = certificateClaimSet.X509Certificate;

                // Validation for Chain building and Revocation list
                try
                {
                    MyX509Validator X509Validator = new MyX509Validator();
                    if (certificate != null)
                    {
                        X509Validator.Validate(certificate);
                    }
                    else
                    {
                        throw new FaultException<Result>(new Result(), new FaultReason(message), new FaultCode(Invalid));
                    }
                }
                catch (Exception ex)
                {
                    throw new FaultException<Result>(new Result(), new FaultReason(ex.Message), new FaultCode(Invalid));
                }
                // Validation ends here

                Result result = new Result();
                
                UserInformationResponse userInformationResponse = new UserInformationResponse();
                SerialNumber = certificate.SerialNumber;
                AsymmetricAlgorithm PublicKey = certificate.PublicKey.Key;
                string role = string.Empty;
                if ((certificate.NotBefore >= DateTime.Now) || (certificate.NotAfter <= DateTime.Now))
                {
                    result.SetError(ErrorCode.Not_Valid_Certificate);
                    throw new WebFaultException<Result>(result, HttpStatusCode.PreconditionFailed);
                }
                if (string.IsNullOrEmpty(SerialNumber) && PublicKey == null)
                {
                    result.SetError(ErrorCode.Not_Authenticated_Certificate_);
                    throw new FaultException<Result>(new Result(), new FaultReason(message), new FaultCode(Invalid));

                }
                else
                {
                    MobiusHISE mobiusHISE = new MobiusHISE(SerialNumber, PublicKey);
                    userInformationResponse = mobiusHISE.GetUserInformation();
                    if (userInformationResponse.Result.IsSuccess)
                    {
                        if (userInformationResponse.UserInformation.Role.ToString() != "Unknown")
                        {
                            role = userInformationResponse.UserInformation.Role.ToString();
                        }
                    }
                    else
                    {
                        result.SetError(ErrorCode.Not_Authenticated_Certificate_);
                        throw new FaultException<Result>(new Result(), new FaultReason(messageUser), new FaultCode(InvalidUser));
                    }
                }

                // If state is null, then we've not been called before so we need
                // to set up our custom state
                if (state == null)
                {
                    customstate = new CustomAuthState();
                    state = customstate;
                }
                else
                    customstate = (CustomAuthState)state;



                // If we've not added claims yet...
                if (!customstate.ClaimsAdded)
                {
                    // Create an empty list of Claims
                    IList<Claim> claims = new List<Claim>();

                    // Iterate through each of the claimsets in the evaluation context
                    foreach (ClaimSet cs in evaluationContext.ClaimSets)
                        // Look for Name claims in the current claimset...
                        foreach (Claim c in cs.FindClaims(ClaimTypes.Name, Rights.PossessProperty))
                            // Get the list of operations the given username is allowed to call...
                            foreach (string s in GetAllowedOpList(role))
                            {
                                // Add claims to the list
                                claims.Add(new Claim(MOBIUS, s, Rights.PossessProperty));
                            }

                    // Add claims to the evaluation context    
                    evaluationContext.AddClaimSet(this, new DefaultClaimSet(this.Issuer, claims));

                    // record that we've added claims
                    customstate.ClaimsAdded = true;

                    // return true, indicating we do not need to be called again.
                    bRet = true;
                }
                else
                {
                    // Should never get here, but just in case...
                    bRet = true;
                }
            }
            return bRet;
        }

        public ClaimSet Issuer
        {
            get { return ClaimSet.System; }
        }

        public string Id
        {
            get { return id; }
        }

        // This method returns a collection of action strings thet indicate the 
        // operations the specified username is allowed to call.
        private static IEnumerable<string> GetAllowedOpList(string role)
        {
            IList<string> ret = new List<string>();

            if (!string.IsNullOrWhiteSpace(role))
            {
                if (role != UserType.Patient.ToString())
                {
                    var providers = from configuration in XElement.Load(ConfigurationManager.AppSettings["ConfigurationPath"]).Elements("Provider").Elements("Url")
                                    select new
                                    {
                                        Key = configuration.Attribute(NAME).Value,
                                    };

                    foreach (var provider in providers)
                    {
                        ret.Add(provider.Key.ToString());
                    }
                }
                else if (role == UserType.Patient.ToString())
                {

                    var patients = from configuration in XElement.Load(ConfigurationManager.AppSettings["ConfigurationPath"]).Elements("Patient").Elements("Url")
                                   select new
                                   {
                                       Key = configuration.Attribute(NAME).Value,
                                   };

                    foreach (var patient in patients)
                    {
                        ret.Add(patient.Key.ToString());
                    }
                }
                else
                {
                    var Commons = from configuration in XElement.Load(ConfigurationManager.AppSettings["ConfigurationPath"]).Elements("Common").Elements("Url")
                                  select new
                                  {
                                      Key = configuration.Attribute(NAME).Value,
                                  };

                    foreach (var Common in Commons)
                    {
                        ret.Add(Common.Key.ToString());
                    }

                }
            }

            return ret;
        }

        // internal class for state
        class CustomAuthState
        {
            bool bClaimsAdded;

            public CustomAuthState()
            {
            }

            public bool ClaimsAdded
            {
                get { return bClaimsAdded; }
                set { bClaimsAdded = value; }
            }
        }
    }
}