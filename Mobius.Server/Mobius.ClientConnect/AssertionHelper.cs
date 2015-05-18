//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using PatientDiscovery;
//using Mobius.CoreLibrary;
//using Mobius.Client.Interface;

//namespace Mobius.Client
//{
//    public partial class MobiusConnect : IMobiusConnect
//    {
//        #region Assertion

        

//        private string GetAssertion(AssertionAction action)
//        {
//            AssertionType assertion = new AssertionType();

//            switch (action)
//            {
//                case AssertionAction.PatientDiscovery:

//                    break;
//                case AssertionAction.DocumentQuery:

//                    break;
//                case AssertionAction.DocumentRetrieve:

//                    break;
//                case AssertionAction.DocumentSubmission:
//                    assertion.uniquePatientId = new string[] { this.HL7PatientId };
//                    break;
//                default:
//                    break;
//            }

//            //Set Witness signature property to false and no need to create object for witness
//            assertion.haveWitnessSignature = true;
//            //Set second Witness signature property to false and no need to create object for witness
//            assertion.haveSecondWitnessSignature = false;
//            //
//            assertion.haveSignature = false;
//            //NOTE - Currently witness is the login user name, may be we need to change later
//            assertion.witnessName = CreatePersonName();
//            //
//            //assertion.personName = CreatePersonName();
//            //Get Home community 
//            assertion.homeCommunity = CreateHomeCommunityType();
//            //Get user information
//            //
//            assertion.userInfo = CreateUserType();
//            //
//            assertion.purposeOfDisclosureCoded = CreateCeType(this.PurposeOfUseCode, AssertionHelper.PURPOSE_USE_CodeSystem, AssertionHelper.PURPOSE_USE_CodeSystemName, AssertionHelper.PURPOSE_USE_CodeSystemsVersion, this.PurposeOfUseDescription, this.PurposeOfUseDescription);
//            //
//            assertion.samlAuthnStatement = CreateSamlAuthStatement();
//            //
//            assertion.samlAuthzDecisionStatement = CreateSamlAuthzDecisionStatementType(action);

//            //assertion.Issuer createSamlIssuer());

//            return XmlSerializerHelper.SerializeObject(assertion);

//        }
//        /// <summary>
//        /// The <AuthzDecisionStatement> element is of type AuthzDecisionStatementType, which extends StatementAbstractType with the addition of the following elements and attributes:
//        ///  Resource [Required] -- A URI reference identifying the resource to which access authorization is sought. 
//        ///  This attribute MAY have the value of the empty URI reference (""), and the meaning is defined to be "the start of the current document", as specified by IETF RFC 2396 [RFC 2396] Section 4.2.
//        ///  Decision [Required] -- The decision rendered by the SAML authority with respect to the specified resource. The value is of the DecisionType simple type.
//        ///  <Action> [One or more]  The set of actions authorized to be performed on the specified resource.
//        ///  <Evidence> [Optional] A set of assertions that the SAML authority relied on in making the decision.
//        /// </summary>
//        /// <returns></returns>
//        private SamlAuthzDecisionStatementType CreateSamlAuthzDecisionStatementType(AssertionAction action)
//        {
//            SamlAuthzDecisionStatementType authz = new SamlAuthzDecisionStatementType();
//            authz.action = action.ToString();
//            authz.decision = Decision.Permit.ToString();
//            authz.evidence = CreateSamlAuthzDecisionStatemnetEvidenceType();
//            authz.resource = AssertionHelper.SAMLAuthzDecision_RESOURCE;
//            return authz;
//        }

//        private SamlAuthzDecisionStatementEvidenceType CreateSamlAuthzDecisionStatemnetEvidenceType()
//        {
//            SamlAuthzDecisionStatementEvidenceType evidence = new SamlAuthzDecisionStatementEvidenceType();
//            evidence.assertion = CreateAuthzDSEvidenceAssertionType();
//            return evidence;
//        }

//        /// <summary>
//        /// The <Evidence> element contains one or more assertions or assertion references that the SAML
//        /// authority relied on in issuing the authorization decision. It has the EvidenceType complex type. It contains
//        ///  a mixture of one or more of the following elements:
//        ///  <AssertionIDRef> [Any number] -- Specifies an assertion by reference to the value of the assertion’s ID attribute.
//        ///  <AssertionURIRef> [Any number] -- Specifies an assertion by means of a URI reference.
//        ///  <Assertion> [Any number] -- Specifies an assertion by value.
//        ///  <EncryptedAssertion> [Any number] -- Specifies an encrypted assertion by value.
//        /// <Note>
//        /// Providing an assertion as evidence MAY affect the reliance agreement between the SAML relying party
//        /// and the SAML authority making the authorization decision.
//        /// </Note>

//        /// </summary>
//        /// <returns></returns>
//        private SamlAuthzDecisionStatementEvidenceAssertionType CreateAuthzDSEvidenceAssertionType()
//        {
//            SamlAuthzDecisionStatementEvidenceAssertionType evidenceAssertion =
//                new SamlAuthzDecisionStatementEvidenceAssertionType();
//            evidenceAssertion.id = AssertionHelper.GetNewID(); ;
//            evidenceAssertion.issueInstant = DateTime.UtcNow.ToString();
//            //TODO - Need more information on this, may be later we need to read the certificate store to get the certificate information to fill below value.
//            evidenceAssertion.issuer = AssertionHelper.SAMLAuthzDecisionStatementEvidenceAssertionType_Issuer;
//            evidenceAssertion.issuerFormat = AssertionHelper.SAMLAuthzDecisionStatementEvidenceAssertionType_IssuerFormat;
//            evidenceAssertion.version = AssertionHelper.SAMLAuthzDecisionStatementEvidenceAssertionType_Version;
//            evidenceAssertion.conditions = CreateAuthzDSEvidenceConditionType();
//            //evidenceAssertion.setSubject("Gallow Younger");
//            //evidenceAssertion.accessConsentPolicy
//            return evidenceAssertion;
//        }



//        private SamlAuthzDecisionStatementEvidenceConditionsType CreateAuthzDSEvidenceConditionType()
//        {
//            SamlAuthzDecisionStatementEvidenceConditionsType condition =
//                new SamlAuthzDecisionStatementEvidenceConditionsType();
//            condition.notBefore = DateTime.UtcNow.ToString();
//            condition.notOnOrAfter = DateTime.UtcNow.AddMinutes(30).ToString();
//            return condition;
//        }

//        /// <summary>
//        /// This method will return full description of authentication context information.
//        /// </summary>
//        /// <returns></returns>
//        private SamlAuthnStatementType CreateSamlAuthStatement()
//        {
//            /*
//            <saml2:AuthnStatement AuthnInstant="2012-06-08T18:31:44.577Z" SessionIndex="123456">
//              <saml2:AuthnContext>
//                <saml2:AuthnContextClassRef>urn:oasis:names:tc:SAML:2.0:ac:classes:X509</saml2:AuthnContextClassRef>
//              </saml2:AuthnContext>
//            </saml2:AuthnStatement>
//             */
//            SamlAuthnStatementType auth = new SamlAuthnStatementType();
//            auth.authInstant = DateTime.UtcNow.ToString();// "2013-02-14T18:45:10.738Z";
//            //auth.subjectLocalityAddress = "123 Fairfax Lane, Fairfax, VA";
//            auth.subjectLocalityDNSName = AssertionHelper.Locality_DNS_Name;
//            auth.sessionIndex = AssertionHelper.GetRoundNumber();
//            //urn:oasis:names:tc:SAML:2.0:ac:classes:X509
//            auth.authContextClassRef = AssertionHelper.SamlAuthnStatementType_AuthContextClassRef;
//            return auth;
//        }

//        private CeType CreateCeType(string code, string codeSystems, string codeSystemName, string codeSystemsVersion, string codeDisplayName, string originalText)
//        {
//            CeType ce = new CeType();
//            ce.code = code;
//            ce.codeSystem = codeSystems;
//            ce.codeSystemVersion = codeSystemsVersion;
//            ce.codeSystemName = codeSystemName;
//            ce.displayName = codeDisplayName;
//            ce.originalText = originalText;
//            return ce;
//        }

//        private PatientDiscovery.UserType CreateUserType()
//        {
//            PatientDiscovery.UserType userInfo = new PatientDiscovery.UserType();
//            userInfo.org = CreateHomeCommunityType();
//            userInfo.personName = CreatePersonName();
//            userInfo.userName = this.UserId;
//            //Create role 
//            userInfo.roleCoded = CreateCeType(this.UserRoleCode, AssertionHelper.ROLE_CodeSystem, AssertionHelper.ROLE_CodeSystemName, AssertionHelper.ROLE_codeSystemVersion, this.UserRole, this.UserRole);
//            return userInfo;
//        }

//        /// <summary>
//        /// THis method will set the home community 
//        /// </summary>
//        /// <param name="community"></param>
//        /// <returns></returns>
//        private HomeCommunityType CreateHomeCommunityType()
//        {
//            //Home community of caller
//            HomeCommunityType homeCommunity = new HomeCommunityType();
//            homeCommunity.description = this.Community.CommunityDescription;
//            homeCommunity.homeCommunityId = this.Community.CommunityIdentifier;
//            homeCommunity.name = this.Community.CommunityName;
//            return homeCommunity;
//        }

//        /// <summary>
//        /// This method will create the Person object
//        /// </summary>
//        /// <param name="patient"></param>
//        /// <returns></returns>
//        private PersonNameType CreatePersonName(Entity.Patient patient)
//        {
//            PersonNameType personNameType = new PersonNameType();
//            personNameType.familyName = patient.FamilyName[0];
//            personNameType.givenName = patient.GivenName[0];
//            personNameType.fullName = personNameType.givenName + " " + personNameType.familyName;
//            return personNameType;
//        }


//        private PersonNameType CreatePersonName()
//        {
//            PersonNameType personNameType = new PersonNameType();
//            if (!string.IsNullOrEmpty(this.UserName))
//            {
//                var name = this.UserName.Split(' ');
//                if (name.Count() <= 2)
//                {
//                    personNameType.familyName = name[1];
//                    personNameType.givenName = name[0];
//                }
//                else if (name.Count() == 1)
//                {
//                    personNameType.givenName = name[0];
//                }
//            }
//            personNameType.fullName = this.UserName;
//            return personNameType;
//        }
//        #endregion Assertion
//    }
//}
