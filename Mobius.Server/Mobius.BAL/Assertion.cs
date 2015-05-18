using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mobius.Entity;
using PatientDiscovery;
using Mobius.CoreLibrary;
using System.ComponentModel;

namespace Mobius.BAL
{
    public enum AssertionAction
    {
        PatientDiscovery,
        DocumentQuery,
        DocumentRetrieve,
        DocumentSubmission
    }

     enum Decision
    {
        Permit, //The specified action is permitted.
        Deny,// The specified action is denied
        Indeterminate//The SAML authority cannot determine whether the specified action is permitted or denied.
        //Note
        //The Indeterminate decision value is used in situations where the SAML authority requires the ability to 
        //provide an affirmative statement but where it is not able to issue a decision
    }

     public partial class Assertion
    {

        readonly string ROLE_CodeSystem = "2.16.840.1.113883.6.96";
        readonly string ROLE_CodeSystemName = "SNOMED_CT";
        readonly string ROLE_codeSystemVersion = "1.0";

        readonly string PURPOSE_USE_CodeSystem = "2.16.840.1.113883.3.18.7.1";
        readonly string PURPOSE_USE_CodeSystemName = "nhin-purpose";
        readonly string PURPOSE_USE_CodeSystemsVersion = "1.0";
        private string AssertionXML
        {
            get;
            set;
        }

        public Assertion(MobiusAssertion mobiusAssertion)
        {
            this.MobiusAssertion = mobiusAssertion;
            if (mobiusAssertion.AssertionMode == AssertionMode.Custom)
                AssertionXML = GetAssertion();            
        }

      
       

        public PatientDiscovery.AssertionType PatientDiscoveryAssertion
        {
            get
            {
                return XmlSerializerHelper.DeserializeObject(this.AssertionXML, typeof(PatientDiscovery.AssertionType)) as PatientDiscovery.AssertionType;
            }
        }

        public DocumentQuery.AssertionType DocumentQueryAssertion
        {
            get
            {
                return XmlSerializerHelper.DeserializeObject(this.AssertionXML, typeof(DocumentQuery.AssertionType)) as DocumentQuery.AssertionType;
            }
        }

        public RetrieveDocument.AssertionType DocumentRetrieveAssertion
        {
            get
            {
                return XmlSerializerHelper.DeserializeObject(this.AssertionXML, typeof(RetrieveDocument.AssertionType)) as RetrieveDocument.AssertionType;
            }
        }

        public SAMLAssertion.HarrisStore.AssertionType DocumentSubmissionAssertion
        {
            get
            {
                return XmlSerializerHelper.DeserializeObject(this.AssertionXML, typeof(SAMLAssertion.HarrisStore.AssertionType)) as SAMLAssertion.HarrisStore.AssertionType;
            }
        }



        #region Helper
        private MobiusAssertion MobiusAssertion
        {
            get;
            set;
        }
      



   


        /// <summary>
        /// This method will create encode PatientId in HL 7 formate
        /// </summary>
        /// <param name="patientId">PatientId</param>
        /// <param name="communityId">Community Id</param>
        /// <returns></returns>
        public string GetHL7EncodePatientId(string patientId, string communityId)
        {
            string encodedPatientId = null;
            string sLocalHomeCommunityId = communityId;

            if (communityId.StartsWith("urn:oid:"))
            {
                sLocalHomeCommunityId = sLocalHomeCommunityId.Substring(0, "urn:oid:".Length);
            }

            if (patientId != null)
            {
                encodedPatientId = "'" + patientId + "^^^&" + sLocalHomeCommunityId + "&ISO" + "'";
            }

            return encodedPatientId;
        }


        private string GetAssertion()
        {

            AssertionType assertionType = new AssertionType();
            if (this.MobiusAssertion.Address != null)
            {
                assertionType.address = GetAddress(this.MobiusAssertion.Address);
            }

            assertionType.authorized = this.MobiusAssertion.authorized;
            assertionType.explanationNonClaimantSignature = this.MobiusAssertion.ExplanationNonClaimantSignature;
            assertionType.haveSignature = this.MobiusAssertion.HaveSignature;
            assertionType.homeCommunity = GetCommunity(this.MobiusAssertion.HomeCommunityId);
            assertionType.messageId = this.MobiusAssertion.MessageId;
            if(this.MobiusAssertion.PersonName!= null)
                assertionType.personName = GetPersonName(this.MobiusAssertion.PersonName);
            assertionType.purposeOfDisclosureCoded = GetPurposeOfDisclosure(this.MobiusAssertion.PurposeOfUse);
            if (this.MobiusAssertion.RelatesToList!= null &&  this.MobiusAssertion.RelatesToList.Length > 0)
                assertionType.relatesToList = this.MobiusAssertion.RelatesToList;
            if (this.MobiusAssertion.SamlAuthnStatement != null)
            {
                assertionType.samlAuthnStatement = GetSAMLAuthnStatement(this.MobiusAssertion.SamlAuthnStatement);
            }

            if (this.MobiusAssertion.samlAuthzDecisionStatement != null)
            {
                assertionType.samlAuthzDecisionStatement = GetSAMLAuthzDecisionStatement(this.MobiusAssertion.samlAuthzDecisionStatement);
            }

            if (this.MobiusAssertion.SamlSignature != null)
            {
                assertionType.samlSignature = GetSAMLSignature(this.MobiusAssertion.SamlSignature);
            }
            //TODO
            if (this.MobiusAssertion.SecondWitness != null)
            {
                assertionType.haveSecondWitnessSignature = this.MobiusAssertion.SecondWitness.HaveSignature;
                if (this.MobiusAssertion.SecondWitness.Address != null)
                    assertionType.secondWitnessAddress = GetAddress(this.MobiusAssertion.SecondWitness.Address);
                if (this.MobiusAssertion.SecondWitness.Name != null)
                    assertionType.secondWitnessName = GetPersonName(this.MobiusAssertion.SecondWitness.Name);
                if (this.MobiusAssertion.SecondWitness.Phone != null)
                    assertionType.secondWitnessPhone = GetPhone(this.MobiusAssertion.SecondWitness.Phone);
            }
            assertionType.SSN = this.MobiusAssertion.SSN;
            assertionType.uniquePatientId = new string[] { this.MobiusAssertion.PatientId };
            if (this.MobiusAssertion.UserInformation != null)
            {
                assertionType.userInfo = GetUserInfo(this.MobiusAssertion.UserInformation);
                assertionType.dateOfBirth = this.MobiusAssertion.UserInformation.DateOfBirth;
            }

            if (this.MobiusAssertion.Witness != null)
            {
                assertionType.haveWitnessSignature = this.MobiusAssertion.Witness.HaveSignature;
                if (this.MobiusAssertion.Witness.Address != null)
                    assertionType.witnessAddress = GetAddress(this.MobiusAssertion.Witness.Address);
                if (this.MobiusAssertion.Witness.Name != null)
                    assertionType.witnessName = GetPersonName(this.MobiusAssertion.Witness.Name);
                if (this.MobiusAssertion.Witness.Phone != null)
                    assertionType.witnessPhone = GetPhone(this.MobiusAssertion.Witness.Phone);
            }

            return XmlSerializerHelper.SerializeObject(assertionType);

        }


        //TODO
        private PhoneType GetPhone(Telephone telephone)
        {
            PhoneType phoneType = new PhoneType();
            phoneType.areaCode = "";
            phoneType.countryCode = "001";
            phoneType.extension = telephone.Extensionnumber;
            phoneType.localNumber = telephone.Number;
            phoneType.phoneNumberType = new CeType();
            CeType ce = GetCeType("phoneCode", "phoneCodeSyst", "phoneCodeSystName", "1.0", "phoneCode", "phoneCode");
            return phoneType;
        }

        private CeType GetCeType(string code, string codeSystem, string codeSystemName, string codeSystemVersion, string displayName, string originalText)
        {
            CeType ce = new CeType();
            ce.code = code;
            ce.codeSystem = codeSystem;
            ce.codeSystemName = codeSystemName;
            ce.codeSystemVersion = codeSystemVersion;
            ce.displayName = displayName;
            ce.originalText = originalText;
            return ce;
        }



        private PatientDiscovery.UserType GetUserInfo(User user)
        {
            PatientDiscovery.UserType userType = new PatientDiscovery.UserType();
            userType.org = GetCommunity(user.HomeCommunity);
            userType.personName = GetPersonName(user.Name);
            userType.roleCoded = GetRole(user.Role);
            userType.userName = user.UserName;
            return userType;

        }

        private CeType GetRole(UserRole userRole)
        {

            string codeText = EnumHelper.GetAttributeOfType<DescriptionAttribute>(userRole);
            return GetCeType(userRole.GetHashCode().ToString(), ROLE_CodeSystem, ROLE_CodeSystemName, ROLE_codeSystemVersion, userRole.ToString(), codeText);
        }

        private HomeCommunityType GetCommunity(MobiusNHINCommunity mobiusNHINCommunity)
        {
            HomeCommunityType community = new HomeCommunityType();
            community.description = mobiusNHINCommunity.CommunityIdentifier;
            community.homeCommunityId = "urn:oid:" + mobiusNHINCommunity.CommunityIdentifier;
            community.name = mobiusNHINCommunity.CommunityIdentifier;
            return community;
        }

        private PatientDiscovery.SamlSignatureType GetSAMLSignature(Entity.SamlSignatureType samlSignatureType)
        {
            PatientDiscovery.SamlSignatureType SamlSignatureType = new PatientDiscovery.SamlSignatureType();
            if (null != samlSignatureType.KeyInfo)
                SamlSignatureType.keyInfo = GetKeyInfo(samlSignatureType.KeyInfo);
            if (null != samlSignatureType.signatureValue)
                SamlSignatureType.signatureValue = samlSignatureType.signatureValue;
            return SamlSignatureType;
        }

        private PatientDiscovery.SamlSignatureKeyInfoType GetKeyInfo(Entity.SamlSignatureKeyInfoType samlSignatureKeyInfoType)
        {
            PatientDiscovery.SamlSignatureKeyInfoType key = new PatientDiscovery.SamlSignatureKeyInfoType();
            if (samlSignatureKeyInfoType.RSAKeyValueExponent != null)
                key.rsaKeyValueExponent = samlSignatureKeyInfoType.RSAKeyValueExponent;
            if (samlSignatureKeyInfoType.RSAKeyValueModulus != null)
                key.rsaKeyValueModulus = samlSignatureKeyInfoType.RSAKeyValueModulus;
            return key;
        }

        private PatientDiscovery.SamlAuthzDecisionStatementType GetSAMLAuthzDecisionStatement(Entity.SamlAuthzDecisionStatementType samlAuthzDecisionStatementType)
        {
            PatientDiscovery.SamlAuthzDecisionStatementType SamlAuthzDecisionStatementType = new PatientDiscovery.SamlAuthzDecisionStatementType();
            SamlAuthzDecisionStatementType.action = samlAuthzDecisionStatementType.Action;
            SamlAuthzDecisionStatementType.decision = samlAuthzDecisionStatementType.Decision;
            if (samlAuthzDecisionStatementType.Evidence != null)
                SamlAuthzDecisionStatementType.evidence = GetEvidence(samlAuthzDecisionStatementType.Evidence);
            SamlAuthzDecisionStatementType.resource = samlAuthzDecisionStatementType.Resource;
            return SamlAuthzDecisionStatementType;
        }

        private PatientDiscovery.SamlAuthzDecisionStatementEvidenceType GetEvidence(Entity.SamlAuthzDecisionStatementEvidenceType samlAuthzDecisionStatementEvidenceType)
        {
            PatientDiscovery.SamlAuthzDecisionStatementEvidenceType evidence = new PatientDiscovery.SamlAuthzDecisionStatementEvidenceType();
            if (null != samlAuthzDecisionStatementEvidenceType.Assertion)
                evidence.assertion = GetAssertion(samlAuthzDecisionStatementEvidenceType.Assertion);
            return evidence;
        }

        private PatientDiscovery.SamlAuthzDecisionStatementEvidenceAssertionType GetAssertion(Entity.SamlAuthzDecisionStatementEvidenceAssertionType samlAuthzDecisionStatementEvidenceAssertionType)
        {
            PatientDiscovery.SamlAuthzDecisionStatementEvidenceAssertionType assertion = new PatientDiscovery.SamlAuthzDecisionStatementEvidenceAssertionType();
            assertion.accessConsentPolicy = samlAuthzDecisionStatementEvidenceAssertionType.AccessConsentPolicy;
            if (null != samlAuthzDecisionStatementEvidenceAssertionType.Conditions)
                assertion.conditions = GetSAMLConditions(samlAuthzDecisionStatementEvidenceAssertionType.Conditions);
            assertion.id = samlAuthzDecisionStatementEvidenceAssertionType.Id;
            assertion.instanceAccessConsentPolicy = samlAuthzDecisionStatementEvidenceAssertionType.InstanceAccessConsentPolicy;
            assertion.issueInstant = samlAuthzDecisionStatementEvidenceAssertionType.IssueInstant;
            assertion.issuer = samlAuthzDecisionStatementEvidenceAssertionType.Issuer;
            assertion.issuerFormat = samlAuthzDecisionStatementEvidenceAssertionType.IssuerFormat;
            assertion.version = samlAuthzDecisionStatementEvidenceAssertionType.Version;
            return assertion;
        }

        private PatientDiscovery.SamlAuthzDecisionStatementEvidenceConditionsType GetSAMLConditions(Entity.SamlAuthzDecisionStatementEvidenceConditionsType samlAuthzDecisionStatementEvidenceConditionsType)
        {
            PatientDiscovery.SamlAuthzDecisionStatementEvidenceConditionsType conditions = new PatientDiscovery.SamlAuthzDecisionStatementEvidenceConditionsType();
            conditions.notBefore = samlAuthzDecisionStatementEvidenceConditionsType.NotBefore;
            conditions.notOnOrAfter = samlAuthzDecisionStatementEvidenceConditionsType.NotOnOrAfter;
            return conditions;
        }



        private PatientDiscovery.SamlAuthnStatementType GetSAMLAuthnStatement(Entity.SamlAuthnStatementType samlAuthnStatementType)
        {
            PatientDiscovery.SamlAuthnStatementType SamlAuthnStatementType = new PatientDiscovery.SamlAuthnStatementType();
            SamlAuthnStatementType.authContextClassRef = samlAuthnStatementType.AuthContextClassRef;
            SamlAuthnStatementType.authInstant = samlAuthnStatementType.AuthInstant;
            SamlAuthnStatementType.sessionIndex = samlAuthnStatementType.SessionIndex;
            SamlAuthnStatementType.subjectLocalityAddress = samlAuthnStatementType.SubjectLocalityAddress;
            SamlAuthnStatementType.subjectLocalityDNSName = samlAuthnStatementType.SubjectLocalityDNSName;
            return SamlAuthnStatementType;
        }


        private CeType GetPurposeOfDisclosure(PurposeOfUse purposeOfUse)
        {
            string description = EnumHelper.GetAttributeOfType<DescriptionAttribute>(purposeOfUse);
            return GetCeType(purposeOfUse.ToString(), PURPOSE_USE_CodeSystem, PURPOSE_USE_CodeSystemName, PURPOSE_USE_CodeSystemsVersion, description, description);
        }


        private PersonNameType GetPersonName(Name name)
        {
            PersonNameType personNameType = new PersonNameType();
            personNameType.familyName = name.FamilyName;
            personNameType.givenName = name.GivenName;
            personNameType.prefix = name.Prefix;
            personNameType.suffix = name.Suffix;
            personNameType.fullName = !string.IsNullOrEmpty(name.GivenName) ? name.GivenName + " " : "" + (!string.IsNullOrEmpty(name.FamilyName) ? name.FamilyName : "");
            return personNameType;
        }

        private AddressType GetAddress(Address address)
        {
            AddressType addressType = new AddressType();
            addressType.streetAddress = !string.IsNullOrEmpty(address.AddressLine1) ? address.AddressLine1 : " " +
                 (!string.IsNullOrEmpty(address.AddressLine2) ? " " + address.AddressLine2 : "");
            if (address.City != null)
            {
                addressType.city = address.City.CityName;
                if (address.City.State != null)
                {
                    addressType.state = address.City.State.StateName;
                    if (address.City.State.Country != null)
                        addressType.country = address.City.State.Country.CountryName;
                }
            }
            //TODO 
            addressType.addressType = GetCeType("AddrCode", "AddrCodeSyst", "AddrCodeSystName", "1.0", "AddrCode", "AddrCode");
            return addressType;
        }
        #endregion Helper

        
    }
}

