using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobius.Client
{
    //public class AssertionHelper
    //{

    //    public static string ROLE_CodeSystem = "2.16.840.1.113883.6.96";
    //    public static string ROLE_CodeSystemName = "SNOMED_CT";
    //    public static string ROLE_codeSystemVersion = "1.0";

    //    public static string PURPOSE_USE_CodeSystem = "2.16.840.1.113883.3.18.7.1";
    //    public static string PURPOSE_USE_CodeSystemName = "nhin-purpose";
    //    public static string PURPOSE_USE_CodeSystemsVersion = "1.0";
    //    public static string Locality_DNS_Name = "MOBIUSHISE.COM";

    //    public static string SAMLAuthzDecision_RESOURCE = "http://mobiushise.com";

    //    public static string SAMLAuthzDecisionStatementEvidenceAssertionType_Issuer = "CN=SAML User,OU=Harris,O=HITS,L=Melbourne,ST=FL,C=US";
    //    public static string SAMLAuthzDecisionStatementEvidenceAssertionType_IssuerFormat = "urn:oasis:names:tc:SAML:1.1:nameid-format:X509SubjectName";
    //    public static string SAMLAuthzDecisionStatementEvidenceAssertionType_Version = "2.0";
    //    public static string SamlAuthnStatementType_AuthContextClassRef = "http://MOBIUSHISE.COM/";
    //    public static string GetRoundNumber()
    //    {
    //        Random rnd = new Random(DateTime.Now.Millisecond);
    //        return rnd.Next().ToString();
    //    }


    //    public static string GetNewID()
    //    {
    //        //40df7c0a-ff3e-4b26-baeb-f2910f6d05a9
    //        return new Guid().ToString();
    //    }
    //}


    internal enum AssertionAction
    {
        PatientDiscovery,
        DocumentQuery,
        DocumentRetrieve,
        DocumentSubmission
    }

    public enum Decision
    {
        Permit, //The specified action is permitted.
        Deny,// The specified action is denied
        Indeterminate//The SAML authority cannot determine whether the specified action is permitted or denied.
        //Note
        //The Indeterminate decision value is used in situations where the SAML authority requires the ability to 
        //provide an affirmative statement but where it is not able to issue a decision
    }

}
