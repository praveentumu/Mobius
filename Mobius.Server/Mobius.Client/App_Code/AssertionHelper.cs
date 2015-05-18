using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MobiusServiceLibrary;
using Mobius.CoreLibrary;



namespace FirstGenesis.UI.Base
{
    public enum AssertionAction
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

    public class AssertionHelper 
    {

       private readonly string ROLE_CodeSystem = "2.16.840.1.113883.6.96";
       private readonly string ROLE_CodeSystemName = "SNOMED_CT";
       private readonly string ROLE_codeSystemVersion = "1.0";

       private readonly string PURPOSE_USE_CodeSystem = "2.16.840.1.113883.3.18.7.1";
       private readonly string PURPOSE_USE_CodeSystemName = "nhin-purpose";
       private readonly string PURPOSE_USE_CodeSystemsVersion = "1.0";
       private readonly string Locality_DNS_Name = "MOBIUSHISE.COM";

       private readonly string SAMLAuthzDecision_RESOURCE = "http://mobiushise.com";

       private readonly string SAMLAuthzDecisionStatementEvidenceAssertionType_Issuer = "CN=SAML User,OU=Harris,O=HITS,L=Melbourne,ST=FL,C=US";
       private readonly string SAMLAuthzDecisionStatementEvidenceAssertionType_IssuerFormat = "urn:oasis:names:tc:SAML:1.1:nameid-format:X509SubjectName";
       private readonly string SAMLAuthzDecisionStatementEvidenceAssertionType_Version = "2.0";
       private readonly string SamlAuthnStatementType_AuthContextClassRef = "http://MOBIUSHISE.COM/";

       public AssertionHelper()
       { }
        
        #region Assertion

        public Assertion CreateAssertion(AssertionMode AssertionMode, AssertionAction assertionAction, PurposeOfUse purposeOfUse, User userInformation, NHINCommunity homeCommunity)
        {
            Assertion assertion = new Assertion();
            assertion.AssertionMode = AssertionMode;
            assertion.PurposeOfUse = purposeOfUse;
            assertion.UserInformation = userInformation;
            assertion.HomeCommunityId = homeCommunity;

            //Set Witness signature property to false and no need to create object for witness
            //Set second Witness signature property to false and no need to create object for witness
            //assertion.haveSecondWitnessSignature = false;
            //
            assertion.HaveSignature = true;
            //NOTE - Currently witness is the login user name. This might/would be changed as per requirement at later stage.
            //assertion.witnessName = CreatePersonName();
            //
            //assertion.personName = CreatePersonName();
            //Get Home community 
           // assertion.HomeCommunityId = homeCommunity;// GetHomeCommunity();
            //Get user information
            //
            //assertion.UserInformation = CreateUserType();

            //
            assertion.SamlAuthnStatement = CreateSamlAuthStatement();
            //
            assertion.samlAuthzDecisionStatement = CreateSamlAuthzDecisionStatementType(assertionAction);
            
            return assertion;
        }

    
      
        /// <summary>
        /// The <AuthzDecisionStatement> element is of type AuthzDecisionStatementType, which extends StatementAbstractType with the addition of the following elements and attributes:
        ///  Resource [Required] -- A URI reference identifying the resource to which access authorization is sought. 
        ///  This attribute MAY have the value of the empty URI reference (""), and the meaning is defined to be "the start of the current document", as specified by IETF RFC 2396 [RFC 2396] Section 4.2.
        ///  Decision [Required] -- The decision rendered by the SAML authority with respect to the specified resource. The value is of the DecisionType simple type.
        ///  <Action> [One or more]  The set of actions authorized to be performed on the specified resource.
        ///  <Evidence> [Optional] A set of assertions that the SAML authority relied on in making the decision.
        /// </summary>
        /// <returns></returns>
        private SamlAuthzDecisionStatementType CreateSamlAuthzDecisionStatementType(AssertionAction action)
        {
            SamlAuthzDecisionStatementType authz = new SamlAuthzDecisionStatementType();
            authz.Action = action.ToString();
            authz.Decision = Decision.Permit.ToString();
            authz.Evidence = CreateSamlAuthzDecisionStatemnetEvidenceType();
            authz.Resource = this.SAMLAuthzDecision_RESOURCE;
            return authz;
        }

        private SamlAuthzDecisionStatementEvidenceType CreateSamlAuthzDecisionStatemnetEvidenceType()
        {
            SamlAuthzDecisionStatementEvidenceType evidence = new SamlAuthzDecisionStatementEvidenceType();
            evidence.Assertion = CreateAuthzDSEvidenceAssertionType();
            return evidence;
        }

        /// <summary>
        /// The <Evidence> element contains one or more assertions or assertion references that the SAML
        /// authority relied on in issuing the authorization decision. It has the EvidenceType complex type. It contains
        ///  a mixture of one or more of the following elements:
        ///  <AssertionIDRef> [Any number] -- Specifies an assertion by reference to the value of the assertion’s ID attribute.
        ///  <AssertionURIRef> [Any number] -- Specifies an assertion by means of a URI reference.
        ///  <Assertion> [Any number] -- Specifies an assertion by value.
        ///  <EncryptedAssertion> [Any number] -- Specifies an encrypted assertion by value.
        /// <Note>
        /// Providing an assertion as evidence MAY affect the reliance agreement between the SAML relying party
        /// and the SAML authority making the authorization decision.
        /// </Note>

        /// </summary>
        /// <returns></returns>
        private SamlAuthzDecisionStatementEvidenceAssertionType CreateAuthzDSEvidenceAssertionType()
        {
            SamlAuthzDecisionStatementEvidenceAssertionType evidenceAssertion =
                new SamlAuthzDecisionStatementEvidenceAssertionType();
            evidenceAssertion.Id = this.GetNewID(); ;
            evidenceAssertion.IssueInstant = DateTime.UtcNow.ToString();
            //TODO - Need more information on this, may be later we need to read the certificate store to get the certificate information to fill below value.
            evidenceAssertion.Issuer = this.SAMLAuthzDecisionStatementEvidenceAssertionType_Issuer;
            evidenceAssertion.IssuerFormat = this.SAMLAuthzDecisionStatementEvidenceAssertionType_IssuerFormat;
            evidenceAssertion.Version = this.SAMLAuthzDecisionStatementEvidenceAssertionType_Version;
            evidenceAssertion.Conditions = CreateAuthzDSEvidenceConditionType();
            //evidenceAssertion.setSubject("Gallow Younger");
            //evidenceAssertion.accessConsentPolicy
            return evidenceAssertion;
        }



        private SamlAuthzDecisionStatementEvidenceConditionsType CreateAuthzDSEvidenceConditionType()
        {
            SamlAuthzDecisionStatementEvidenceConditionsType condition =
                new SamlAuthzDecisionStatementEvidenceConditionsType();
            condition.NotBefore = DateTime.UtcNow.ToString();
            condition.NotOnOrAfter = DateTime.UtcNow.AddMinutes(30).ToString();
            return condition;
        }

        /// <summary>
        /// This method will return full description of authentication context information.
        /// </summary>
        /// <returns></returns>
        private SamlAuthnStatementType CreateSamlAuthStatement()
        {
            /*
            <saml2:AuthnStatement AuthnInstant="2012-06-08T18:31:44.577Z" SessionIndex="123456">
              <saml2:AuthnContext>
                <saml2:AuthnContextClassRef>urn:oasis:names:tc:SAML:2.0:ac:classes:X509</saml2:AuthnContextClassRef>
              </saml2:AuthnContext>
            </saml2:AuthnStatement>
             */
            SamlAuthnStatementType auth = new SamlAuthnStatementType();
            auth.AuthInstant = DateTime.UtcNow.ToString();// "2013-02-14T18:45:10.738Z";
            //auth.subjectLocalityAddress = "123 Fairfax Lane, Fairfax, VA";
            auth.SubjectLocalityDNSName = this.Locality_DNS_Name;
            auth.SessionIndex = this.GetRoundNumber();
            //urn:oasis:names:tc:SAML:2.0:ac:classes:X509
            auth.AuthContextClassRef = this.SamlAuthnStatementType_AuthContextClassRef;
            return auth;
        }

    #endregion Assertion



        public  string GetRoundNumber()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            return rnd.Next().ToString();
        }


        public string GetNewID()
        {            
            return new Guid().ToString();
        }
    }
}
