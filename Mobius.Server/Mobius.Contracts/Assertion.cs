using System.Runtime.Serialization;
using Mobius.CoreLibrary;

namespace MobiusServiceLibrary
{
    [DataContract]
    public class Assertion
    {
        [DataMember]
        public AssertionMode AssertionMode
        {
            get;
            set;
        }

        
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Address Address
        {
            get;
            set;
        }


        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string ExplanationNonClaimantSignature
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool HaveSignature
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public NHINCommunity HomeCommunityId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Name PersonName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Telephone phoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Witness Witness
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Witness SecondWitness
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string SSN
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string PatientId
        {
            get;
            set;
        }



        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public User UserInformation
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool authorized
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public PurposeOfUse PurposeOfUse
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public SamlAuthnStatementType SamlAuthnStatement
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public SamlAuthzDecisionStatementType samlAuthzDecisionStatement
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public SamlSignatureType SamlSignature
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string MessageId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string[] RelatesToList
        {
            get;
            set;
        }
    }


    public class User
    {
        /// <remarks/>
        public Name Name
        {
            get;
            set;
        }

        /// <remarks/>
        public string UserName
        {
            get;
            set;
        }

        /// <remarks/>
        public NHINCommunity HomeCommunity
        {
            get;
            set;
        }

        /// <remarks/>
        public UserRole Role
        {
            get;
            set;
        }

        
        /// <summary>
        /// 
        /// </summary>     
        public string DateOfBirth
        {
            get;
            set;
        }
    }


    public class Witness
    {

        /// <remarks/>
        public MobiusServiceLibrary.Name Name
        {
            get;
            set;
        }

        /// <remarks/>
        public MobiusServiceLibrary.Address Address
        {
            get;
            set;
        }

        /// <remarks/>
        public MobiusServiceLibrary.Telephone Phone
        {
            get;
            set;
        }

        /// <remarks/>
        public bool HaveSignature
        {
            get;
            set;
        }

    }


    public class SamlAuthnStatementType
    {

        /// <remarks/>
        public string AuthInstant
        {
            get;
            set;
        }

        /// <remarks/>
        public string SessionIndex
        {
            get;
            set;
        }

        /// <remarks/>
        public string AuthContextClassRef
        {
            get;
            set;
        }

        /// <remarks/>
        public string SubjectLocalityAddress
        {
            get;
            set;
        }

        /// <remarks/>
        public string SubjectLocalityDNSName
        {
            get;
            set;
        }
    }


    public class SamlAuthzDecisionStatementType
    {
        /// <remarks/>
        public string Decision
        {
            get;
            set;
        }

        /// <remarks/>
        public string Resource
        {
            get;
            set;
        }

        /// <remarks/>
        public string Action
        {
            get;
            set;
        }

        /// <remarks/>
        public SamlAuthzDecisionStatementEvidenceType Evidence
        {
            get;
            set;
        }
    }


    public class SamlAuthzDecisionStatementEvidenceType
    {

        /// <remarks/>
        public SamlAuthzDecisionStatementEvidenceAssertionType Assertion
        {
            get;
            set;
        }
    }

    public class SamlAuthzDecisionStatementEvidenceAssertionType
    {
        /// <remarks/>
        public string Id
        {
            get;
            set;
        }

        /// <remarks/>
        public string IssueInstant
        {
            get;
            set;
        }

        /// <remarks/>
        public string Version
        {
            get;
            set;
        }

        /// <remarks/>
        public string Issuer
        {
            get;
            set;
        }

        /// <remarks/>
        public string IssuerFormat
        {
            get;
            set;
        }

        /// <remarks/>
        public SamlAuthzDecisionStatementEvidenceConditionsType Conditions
        {
            get;
            set;
        }

        /// <remarks/>
        public string AccessConsentPolicy
        {
            get;
            set;
        }

        /// <remarks/>
        public string InstanceAccessConsentPolicy
        {
            get;
            set;
        }
    }

    public class SamlAuthzDecisionStatementEvidenceConditionsType
    {

        /// <remarks/>
        public string NotBefore
        {
            get;
            set;
        }

        /// <remarks/>
        public string NotOnOrAfter
        {
            get;
            set;
        }
    }


    public class SamlSignatureKeyInfoType
    {

        /// <remarks/>    
        public byte[] RSAKeyValueModulus
        {
            get;
            set;
        }

        /// <remarks/>    
        public byte[] RSAKeyValueExponent
        {
            get;
            set;
        }
    }


    public class SamlSignatureType
    {

        /// <remarks/>
        public SamlSignatureKeyInfoType KeyInfo
        {
            get;
            set;
        }

        public byte[] signatureValue
        {
            get;
            set;
        }
    }



}
