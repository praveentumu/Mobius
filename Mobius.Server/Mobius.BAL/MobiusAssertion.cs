using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mobius.Entity;
using Mobius.CoreLibrary;
using PatientDiscovery;

namespace Mobius.BAL
{
    public partial class Assertion
    {
        readonly string Locality_DNS_Name = "MOBIUSHISE.COM";
        
        readonly string SAMLAuthzDecision_RESOURCE = "http://mobiushise.com";

        readonly string SAMLAuthzDecisionStatementEvidenceAssertionType_Issuer = "CN=SAML User,OU=Harris,O=HITS,L=Melbourne,ST=FL,C=US";
        readonly string SAMLAuthzDecisionStatementEvidenceAssertionType_IssuerFormat = "urn:oasis:names:tc:SAML:1.1:nameid-format:X509SubjectName";
        readonly string SAMLAuthzDecisionStatementEvidenceAssertionType_Version = "2.0";
        readonly string SamlAuthnStatementType_AuthContextClassRef = "http://MOBIUSHISE.COM/";
        
        
        private string HL7PatientId { get; set; }

        public Assertion(MobiusAssertion mobiusAssertion, AssertionAction action, string patientId, MobiusNHINCommunity community)
        {
            this.MobiusAssertion = mobiusAssertion;
            if (mobiusAssertion.AssertionMode == AssertionMode.Default)
                AssertionXML = GetDefaultAssertion(action, patientId, community);
        }

        private string GetDefaultAssertion(AssertionAction action, string patientId, MobiusNHINCommunity community)
        {
            AssertionType assertion = new AssertionType();

            this.HL7PatientId = GetHL7EncodePatientId(patientId, community.CommunityIdentifier);

            switch (action)
            {
                case AssertionAction.PatientDiscovery:
                    assertion.uniquePatientId = new string[] { this.HL7PatientId };
                    break;
                case AssertionAction.DocumentQuery:
                    assertion.uniquePatientId = new string[] { this.HL7PatientId };
                    break;
                case AssertionAction.DocumentRetrieve:
                    assertion.uniquePatientId = new string[] { this.HL7PatientId };
                    break;
                case AssertionAction.DocumentSubmission:
                    assertion.uniquePatientId = new string[] { this.HL7PatientId };
                    break;
                default:
                    break;
            }

            assertion.authorized = true;
            //Set Witness signature property to false and no need to create object for witness
            assertion.haveWitnessSignature = true;
            //Set second Witness signature property to false and no need to create object for witness
            assertion.haveSecondWitnessSignature = false;
            //
            assertion.haveSignature = false;
            //NOTE - Currently witness is the login user name, may be we need to change later
            assertion.witnessName = GetPersonName(MobiusAssertion.UserInformation.Name);
            assertion.homeCommunity = GetCommunity(community);
            //
            assertion.userInfo = GetUserInfo(MobiusAssertion.UserInformation);
            //
            assertion.purposeOfDisclosureCoded = GetPurposeOfDisclosure(this.MobiusAssertion.PurposeOfUse);
            //
            assertion.samlAuthnStatement = CreateSamlAuthStatement();
            //
            assertion.samlAuthzDecisionStatement = CreateSamlAuthzDecisionStatementType(action);
                       

            return XmlSerializerHelper.SerializeObject(assertion);            
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
        private PatientDiscovery.SamlAuthzDecisionStatementType CreateSamlAuthzDecisionStatementType(AssertionAction action)
        {
            PatientDiscovery.SamlAuthzDecisionStatementType authz = new PatientDiscovery.SamlAuthzDecisionStatementType();
            authz.action = action.ToString();
            authz.decision = Decision.Permit.ToString();
            authz.evidence = CreateSamlAuthzDecisionStatemnetEvidenceType();
            authz.resource = SAMLAuthzDecision_RESOURCE;
            return authz;
        }

        private PatientDiscovery.SamlAuthzDecisionStatementEvidenceType CreateSamlAuthzDecisionStatemnetEvidenceType()
        {
            PatientDiscovery.SamlAuthzDecisionStatementEvidenceType evidence = new PatientDiscovery.SamlAuthzDecisionStatementEvidenceType();
            evidence.assertion = CreateAuthzDSEvidenceAssertionType();
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
        private PatientDiscovery.SamlAuthzDecisionStatementEvidenceAssertionType CreateAuthzDSEvidenceAssertionType()
        {
            PatientDiscovery.SamlAuthzDecisionStatementEvidenceAssertionType evidenceAssertion =
                new PatientDiscovery.SamlAuthzDecisionStatementEvidenceAssertionType();
            evidenceAssertion.id = GetNewID(); ;
            evidenceAssertion.issueInstant = DateTime.UtcNow.ToString();
            //TODO - Need more information on this, may be later we need to read the certificate store to get the certificate information to fill below value.
            evidenceAssertion.issuer = SAMLAuthzDecisionStatementEvidenceAssertionType_Issuer;
            evidenceAssertion.issuerFormat = SAMLAuthzDecisionStatementEvidenceAssertionType_IssuerFormat;
            evidenceAssertion.version = SAMLAuthzDecisionStatementEvidenceAssertionType_Version;
            evidenceAssertion.conditions = CreateAuthzDSEvidenceConditionType();
            //evidenceAssertion.setSubject("Gallow Younger");
            //evidenceAssertion.accessConsentPolicy
            return evidenceAssertion;
        }

        private PatientDiscovery.SamlAuthzDecisionStatementEvidenceConditionsType CreateAuthzDSEvidenceConditionType()
        {
            PatientDiscovery.SamlAuthzDecisionStatementEvidenceConditionsType condition =
                new PatientDiscovery.SamlAuthzDecisionStatementEvidenceConditionsType();
            condition.notBefore = DateTime.UtcNow.ToString();
            condition.notOnOrAfter = DateTime.UtcNow.AddMinutes(30).ToString();
            return condition;
        }



        /// <summary>
        /// This method will return full description of authentication context information.
        /// </summary>
        /// <returns></returns>
        private PatientDiscovery.SamlAuthnStatementType CreateSamlAuthStatement()
        {
            /*
            <saml2:AuthnStatement AuthnInstant="2012-06-08T18:31:44.577Z" SessionIndex="123456">
              <saml2:AuthnContext>
                <saml2:AuthnContextClassRef>urn:oasis:names:tc:SAML:2.0:ac:classes:X509</saml2:AuthnContextClassRef>
              </saml2:AuthnContext>
            </saml2:AuthnStatement>
             */
            PatientDiscovery.SamlAuthnStatementType auth = new PatientDiscovery.SamlAuthnStatementType();
            auth.authInstant = DateTime.UtcNow.ToString();// "2013-02-14T18:45:10.738Z";
            //auth.subjectLocalityAddress = "123 Fairfax Lane, Fairfax, VA";
            auth.subjectLocalityDNSName = Locality_DNS_Name;
            auth.sessionIndex = GetRoundNumber();
            //urn:oasis:names:tc:SAML:2.0:ac:classes:X509
            auth.authContextClassRef = SamlAuthnStatementType_AuthContextClassRef;
            return auth;
        }

        //private PatientDiscovery.UserType GetUserInfo(UserInfo user, MobiusNHINCommunity community)
        //{
        //    PatientDiscovery.UserType userInfo = new PatientDiscovery.UserType();
        //    userInfo.org = GetCommunity(community);
        //    userInfo.personName = CreatePersonName(user.Name);
        //    userInfo.userName = user.EmailAddress;
        //    //Create role 
        //    userInfo.roleCoded = GetCeType(user.UserRoleCode, ROLE_CodeSystem, ROLE_CodeSystemName, ROLE_codeSystemVersion, user.Role, user.Role);
        //    return userInfo;
        //}


        private PersonNameType CreatePersonName(string userName)
        {
            PersonNameType personNameType = new PersonNameType();
            if (!string.IsNullOrEmpty(userName))
            {
                var name = userName.Split(' ');
                if (name.Count() <= 2)
                {
                    personNameType.familyName = name[1];
                    personNameType.givenName = name[0];
                }
                else if (name.Count() == 1)
                {
                    personNameType.givenName = name[0];
                }
            }
            personNameType.fullName = userName;
            return personNameType;
        }

        public static string GetRoundNumber()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            return rnd.Next().ToString();
        }

        public static string GetNewID()
        {
            //40df7c0a-ff3e-4b26-baeb-f2910f6d05a9
            return new Guid().ToString();
        }

    }
}
