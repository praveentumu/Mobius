using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAMLAssertion.SAMLService;
using SAMLAssertion.HarrisStore;
using System.Xml.Serialization;
using System.IO;
using System.Data;
namespace SAMLAssertion
{
    public class SAMLAssertionForDocSubmit
    {
        //EntityXDR_Service objEntityXDR_Service = new EntityXDR_Service();
        public AssertionType CreateAssertion(string strReciever, string strUserName, string strEntityId, DateTime dtmRuleStartDate, DateTime dtmRuleEndDate, string strHomeCommunityId, DataSet dsUserInfo, string PurposeOfUse, string PatientId)
        {

            AssertionType objAssertionType = new AssertionType();
            SAMLService.SamlService1 obj = new SamlService1();
            SAMLService.Assertion objAssertion = new Assertion();
            objAssertion = obj.CreateAssertion(strReciever, strUserName, strEntityId, dtmRuleStartDate, dtmRuleEndDate, strHomeCommunityId);
            objAssertionType.address = new AddressType();
            objAssertionType.address.addressType = new CeType();
            objAssertionType.address.addressType.code = "Mobius";
            objAssertionType.address.addressType.codeSystem = "Mobius";
            objAssertionType.address.addressType.codeSystemName = "Mobius";
            objAssertionType.address.addressType.codeSystemVersion = "Mobius";
            objAssertionType.address.addressType.displayName = "Mobius";
            objAssertionType.address.addressType.originalText = "Mobius";
            objAssertionType.address.city = dsUserInfo.Tables[0].Rows[0][30].ToString();
            objAssertionType.address.country = dsUserInfo.Tables[0].Rows[0][28].ToString();
            objAssertionType.address.state = dsUserInfo.Tables[0].Rows[0][29].ToString();
            objAssertionType.address.streetAddress = dsUserInfo.Tables[0].Rows[0][25].ToString();
            objAssertionType.address.zipCode = dsUserInfo.Tables[0].Rows[0][26].ToString();
            objAssertionType.dateOfBirth = dsUserInfo.Tables[0].Rows[0][11].ToString();
            objAssertionType.explanationNonClaimantSignature = "Mobius";
            objAssertionType.homeCommunity = new HomeCommunityType();
            objAssertionType.homeCommunity.description = strHomeCommunityId;
            objAssertionType.homeCommunity.homeCommunityId = strHomeCommunityId;
            objAssertionType.homeCommunity.name = strHomeCommunityId;
            objAssertionType.personName = new PersonNameType();
            objAssertionType.personName.familyName = dsUserInfo.Tables[0].Rows[0][3].ToString();
            objAssertionType.personName.givenName = dsUserInfo.Tables[0].Rows[0][3].ToString();
            objAssertionType.personName.nameType = new CeType();
            objAssertionType.personName.nameType.code = "Mobius";
            objAssertionType.personName.nameType.codeSystem = "Mobius";
            objAssertionType.personName.nameType.codeSystemName = "Mobius";
            objAssertionType.personName.nameType.codeSystemVersion = "Mobius";
            objAssertionType.personName.nameType.displayName = "Mobius";
            objAssertionType.personName.nameType.originalText = "Mobius";
            objAssertionType.personName.secondNameOrInitials = Convert.ToString(dsUserInfo.Tables[0].Rows[0][13]);
            objAssertionType.personName.fullName = dsUserInfo.Tables[0].Rows[0][2].ToString() + " " + dsUserInfo.Tables[0].Rows[0][3].ToString();
            objAssertionType.phoneNumber = new PhoneType();
            objAssertionType.phoneNumber.areaCode = dsUserInfo.Tables[0].Rows[0][27].ToString().Substring(0, 2);
            objAssertionType.phoneNumber.countryCode = dsUserInfo.Tables[0].Rows[0][27].ToString().Substring(2, 2);
            objAssertionType.phoneNumber.extension = dsUserInfo.Tables[0].Rows[0][27].ToString().Substring(4, 3);
            objAssertionType.phoneNumber.localNumber = dsUserInfo.Tables[0].Rows[0][27].ToString().Substring(7, 4);
            objAssertionType.phoneNumber.phoneNumberType = new CeType();
            objAssertionType.phoneNumber.phoneNumberType.code = "Mobius";
            objAssertionType.phoneNumber.phoneNumberType.codeSystem = "Mobius";
            objAssertionType.phoneNumber.phoneNumberType.codeSystemName = "Mobius";
            objAssertionType.phoneNumber.phoneNumberType.codeSystemVersion = "Mobius";
            objAssertionType.phoneNumber.phoneNumberType.displayName = "Mobius";
            objAssertionType.phoneNumber.phoneNumberType.originalText = "Mobius";
            objAssertionType.secondWitnessAddress = new AddressType();
            objAssertionType.secondWitnessAddress.addressType = new CeType();
            objAssertionType.secondWitnessAddress.addressType.code = "Mobius";
            objAssertionType.secondWitnessAddress.addressType.codeSystem = "Mobius";
            objAssertionType.secondWitnessAddress.addressType.codeSystemName = "Mobius";
            objAssertionType.secondWitnessAddress.addressType.codeSystemVersion = "Mobius";
            objAssertionType.secondWitnessAddress.addressType.displayName = "Mobius";
            objAssertionType.secondWitnessAddress.addressType.originalText = "Mobius";
            objAssertionType.secondWitnessAddress.city = "Mobius";
            objAssertionType.secondWitnessAddress.country = "Mobius";
            objAssertionType.secondWitnessAddress.state = "Mobius";
            objAssertionType.secondWitnessAddress.streetAddress = "Mobius";
            objAssertionType.secondWitnessAddress.zipCode = "Mobius";
            objAssertionType.secondWitnessName = new PersonNameType();
            objAssertionType.secondWitnessName.familyName = "Mobius";
            objAssertionType.secondWitnessName.givenName = "Mobius";
            objAssertionType.secondWitnessName.nameType = new CeType();
            objAssertionType.secondWitnessName.nameType.code = "Mobius";
            objAssertionType.secondWitnessName.nameType.codeSystem = "Mobius";
            objAssertionType.secondWitnessName.nameType.codeSystemName = "Mobius";
            objAssertionType.secondWitnessName.nameType.codeSystemVersion = "Mobius";
            objAssertionType.secondWitnessName.nameType.displayName = "Mobius";
            objAssertionType.secondWitnessName.nameType.originalText = "Mobius";
            objAssertionType.secondWitnessName.secondNameOrInitials = "Mobius";
            objAssertionType.secondWitnessName.fullName = "Mobius";
            objAssertionType.secondWitnessPhone = new PhoneType();
            objAssertionType.secondWitnessPhone.areaCode = "Mobius";
            objAssertionType.secondWitnessPhone.countryCode = "Mobius";
            objAssertionType.secondWitnessPhone.extension = "Mobius";
            objAssertionType.secondWitnessPhone.localNumber = "Mobius";
            objAssertionType.secondWitnessPhone.phoneNumberType = new CeType();
            objAssertionType.secondWitnessPhone.phoneNumberType.code = "Mobius";
            objAssertionType.secondWitnessPhone.phoneNumberType.codeSystem = "Mobius";
            objAssertionType.secondWitnessPhone.phoneNumberType.codeSystemName = "Mobius";
            objAssertionType.secondWitnessPhone.phoneNumberType.codeSystemVersion = "Mobius";
            objAssertionType.secondWitnessPhone.phoneNumberType.displayName = "Mobius";
            objAssertionType.secondWitnessPhone.phoneNumberType.originalText = "Mobius";
            objAssertionType.SSN = dsUserInfo.Tables[0].Rows[0][6].ToString();
            string[] arrPatientId = new string[1];
            arrPatientId[0] = PatientId;
            objAssertionType.uniquePatientId = arrPatientId;
            objAssertionType.witnessAddress = new AddressType();
            objAssertionType.witnessAddress.addressType = new CeType();
            objAssertionType.witnessAddress.addressType.code = "Mobius";
            objAssertionType.witnessAddress.addressType.codeSystem = "Mobius";
            objAssertionType.witnessAddress.addressType.codeSystemName = "Mobius";
            objAssertionType.witnessAddress.addressType.codeSystemVersion = "Mobius";
            objAssertionType.witnessAddress.addressType.displayName = "Mobius";
            objAssertionType.witnessAddress.addressType.originalText = "Mobius";
            objAssertionType.witnessAddress.city = "Mobius";
            objAssertionType.witnessAddress.country = "Mobius";
            objAssertionType.witnessAddress.state = "Mobius";
            objAssertionType.witnessAddress.streetAddress = "Mobius";
            objAssertionType.witnessAddress.zipCode = "Mobius";
            objAssertionType.witnessName = new PersonNameType();
            objAssertionType.witnessName.familyName = "Mobius";
            objAssertionType.witnessName.givenName = "Mobius";
            objAssertionType.witnessName.nameType = new CeType();
            objAssertionType.witnessName.nameType.code = "Mobius";
            objAssertionType.witnessName.nameType.codeSystem = "Mobius";
            objAssertionType.witnessName.nameType.codeSystemName = "Mobius";
            objAssertionType.witnessName.nameType.codeSystemVersion = "Mobius";
            objAssertionType.witnessName.nameType.displayName = "Mobius";
            objAssertionType.witnessName.nameType.originalText = "Mobius";
            objAssertionType.witnessName.secondNameOrInitials = "Mobius";
            objAssertionType.witnessName.fullName = "Mobius";
            objAssertionType.witnessPhone = new PhoneType();
            objAssertionType.witnessPhone.areaCode = "Mobius";
            objAssertionType.witnessPhone.countryCode = "Mobius";
            objAssertionType.witnessPhone.extension = "Mobius";
            objAssertionType.witnessPhone.localNumber = "Mobius";
            objAssertionType.witnessPhone.phoneNumberType = new CeType();
            objAssertionType.witnessPhone.phoneNumberType.code = "Mobius";
            objAssertionType.witnessPhone.phoneNumberType.codeSystem = "Mobius";
            objAssertionType.witnessPhone.phoneNumberType.codeSystemName = "Mobius";
            objAssertionType.witnessPhone.phoneNumberType.codeSystemVersion = "Mobius";
            objAssertionType.witnessPhone.phoneNumberType.displayName = "Mobius";
            objAssertionType.witnessPhone.phoneNumberType.originalText = "Mobius";
            objAssertionType.userInfo = new UserType();
            objAssertionType.userInfo.personName = new PersonNameType();
            objAssertionType.userInfo.personName.familyName = "Mobius";
            objAssertionType.userInfo.personName.givenName = "Mobius";
            objAssertionType.userInfo.personName.nameType = new CeType();
            objAssertionType.userInfo.personName.nameType.code = "Mobius";
            objAssertionType.userInfo.personName.nameType.codeSystem = "Mobius";
            objAssertionType.userInfo.personName.nameType.codeSystemName = "Mobius";
            objAssertionType.userInfo.personName.nameType.codeSystemVersion = "Mobius";
            objAssertionType.userInfo.personName.nameType.displayName = "Mobius";
            objAssertionType.userInfo.personName.nameType.originalText = "Mobius";
            objAssertionType.userInfo.personName.secondNameOrInitials = "Mobius";
            objAssertionType.userInfo.personName.fullName = "Mobius";
            objAssertionType.userInfo.userName = "Mobius";
            objAssertionType.userInfo.org = new HomeCommunityType();
            objAssertionType.userInfo.org.description = "Mobius";
            objAssertionType.userInfo.org.homeCommunityId = strHomeCommunityId;
            objAssertionType.userInfo.org.name = "Mobius";
            objAssertionType.userInfo.roleCoded = new CeType();
            objAssertionType.userInfo.roleCoded.code = "Mobius";
            objAssertionType.userInfo.roleCoded.codeSystem = "Mobius";
            objAssertionType.userInfo.roleCoded.codeSystemName = "Mobius";
            objAssertionType.userInfo.roleCoded.codeSystemVersion = "Mobius";
            objAssertionType.userInfo.roleCoded.displayName = "Mobius";
            objAssertionType.userInfo.roleCoded.originalText = "Mobius";
            objAssertionType.authorized = true;
            objAssertionType.purposeOfDisclosureCoded = new CeType();
            objAssertionType.purposeOfDisclosureCoded.code = PurposeOfUse;
            objAssertionType.purposeOfDisclosureCoded.codeSystem = "Mobius";
            objAssertionType.purposeOfDisclosureCoded.codeSystemName = "Mobius";
            objAssertionType.purposeOfDisclosureCoded.codeSystemVersion = "Mobius";
            objAssertionType.purposeOfDisclosureCoded.displayName = PurposeOfUse;
            objAssertionType.purposeOfDisclosureCoded.originalText = "Mobius";
            objAssertionType.samlAuthnStatement = new SamlAuthnStatementType();
            objAssertionType.samlAuthnStatement.authInstant = ((SAMLService.AuthnStatement)(objAssertion.Items[0])).AuthnInstant;
            objAssertionType.samlAuthnStatement.sessionIndex = ((SAMLService.AuthnStatement)(objAssertion.Items[0])).SessionIndex;
            objAssertionType.samlAuthnStatement.subjectLocalityAddress = ((SAMLService.AuthnStatement)(objAssertion.Items[0])).SubjectLocality.Address;
            objAssertionType.samlAuthnStatement.subjectLocalityDNSName = ((SAMLService.AuthnStatement)(objAssertion.Items[0])).SubjectLocality.DNSName;
            objAssertionType.samlAuthzDecisionStatement = new SamlAuthzDecisionStatementType();
            objAssertionType.samlAuthzDecisionStatement.decision = ((SAMLService.AuthzDecisionStatement)(objAssertion.Items[1])).Decision.ToString();
            objAssertionType.samlAuthzDecisionStatement.resource = ((SAMLService.AuthzDecisionStatement)(objAssertion.Items[1])).Resource.ToString();
            objAssertionType.samlAuthzDecisionStatement.action = ((SAMLService.AuthzDecisionStatement)(objAssertion.Items[1])).Action.ToString();
            objAssertionType.samlAuthzDecisionStatement.evidence = new SamlAuthzDecisionStatementEvidenceType();
            objAssertionType.samlAuthzDecisionStatement.evidence.assertion = new SamlAuthzDecisionStatementEvidenceAssertionType();
            objAssertionType.samlAuthzDecisionStatement.evidence.assertion.id = objAssertion.ID;
            objAssertionType.samlAuthzDecisionStatement.evidence.assertion.issueInstant = objAssertion.IssueInstant;
            objAssertionType.samlAuthzDecisionStatement.evidence.assertion.version = objAssertion.Version;
            objAssertionType.samlAuthzDecisionStatement.evidence.assertion.issuerFormat = objAssertion.Issuer.Format;
            objAssertionType.samlAuthzDecisionStatement.evidence.assertion.conditions = new SamlAuthzDecisionStatementEvidenceConditionsType();
            objAssertionType.samlAuthzDecisionStatement.evidence.assertion.conditions.notBefore = dtmRuleStartDate.ToShortDateString();
            objAssertionType.samlAuthzDecisionStatement.evidence.assertion.conditions.notOnOrAfter = dtmRuleEndDate.ToShortDateString();
            objAssertionType.samlAuthzDecisionStatement.evidence.assertion.accessConsentPolicy = ((SAMLService.Assertion)(((SAMLService.AuthzDecisionStatement)(objAssertion.Items[1])).Evidence.Items[0])).AccessConsentPolicy;
            objAssertionType.samlAuthzDecisionStatement.evidence.assertion.instanceAccessConsentPolicy = ((SAMLService.Assertion)(((SAMLService.AuthzDecisionStatement)(objAssertion.Items[1])).Evidence.Items[0])).InstanceAccessConsentPolicy;
            return objAssertionType;
        }
        public NhinTargetCommunityType[] GetNhinComunity(string strHomeCommunityId)
        {
            NhinTargetCommunityType[] objNhinTargetCommunityTypeArray = new NhinTargetCommunityType[1];
            objNhinTargetCommunityTypeArray[0] = new NhinTargetCommunityType();
            objNhinTargetCommunityTypeArray[0].homeCommunity = new HomeCommunityType();
            objNhinTargetCommunityTypeArray[0].homeCommunity.description = strHomeCommunityId;
            objNhinTargetCommunityTypeArray[0].homeCommunity.homeCommunityId = strHomeCommunityId;
            objNhinTargetCommunityTypeArray[0].homeCommunity.name = strHomeCommunityId;
            return objNhinTargetCommunityTypeArray;
        }
        public ProvideAndRegisterDocumentSetRequestType GetProvideAndRegisterDocumentSetRequest(string homecomunityid, string sDocUniqueId,
            string docPath, DateTime creationTime, DateTime serviceStartTime, DateTime serviceStopTime, string sPatientId, bool policy)
        {
            ProvideAndRegisterDocumentSetRequestTypeDocument[] oDoc = new ProvideAndRegisterDocumentSetRequestTypeDocument[1];
            oDoc[0] = new ProvideAndRegisterDocumentSetRequestTypeDocument();
            oDoc[0].id = sDocUniqueId;//"1.123402.11112";//Guid.NewGuid().ToString();

            oDoc[0].Value = ReadByteArrayFromFile(docPath);

            SAMLAssertion.HarrisStore.ProvideAndRegisterDocumentSetRequestType ProvideAndRegisterDocumentSetRequest = new ProvideAndRegisterDocumentSetRequestType();
            ProvideAndRegisterDocumentSetRequest.SubmitObjectsRequest = new SubmitObjectsRequest();
            ProvideAndRegisterDocumentSetRequest.SubmitObjectsRequest.id = "123";
            ProvideAndRegisterDocumentSetRequest.SubmitObjectsRequest.comment = "comme";
            SAMLAssertion.HarrisStore.ExtrinsicObjectType objE = new ExtrinsicObjectType();//ExtrinsicObjectType();
            objE.ExternalIdentifier = new ExternalIdentifierType[2];
            objE.ExternalIdentifier[0] = setPatientId(sDocUniqueId, sPatientId);
            objE.ExternalIdentifier[1] = setDocumentUniqueId(sDocUniqueId);
            // set mime type
            objE.mimeType = CDAConstants.PROVIDE_REGISTER_MIME_TYPE;


            if (policy)
            {
                objE.Classification = new ClassificationType[3];
                objE.Classification[0] = setClassCode(sDocUniqueId);
                objE.Classification[1] = setFormatCode(sDocUniqueId);
                objE.Classification[2] = setConfidentialityCode(sDocUniqueId);
            }

            //          set document id;
            objE.id = oDoc[0].id;
            //          objE[0].class

            objE.objectType = CDAConstants.PROVIDE_REGISTER_OBJECT_TYPE;

            SlotType1[] objSlotType1 = new SlotType1[6];
            objSlotType1[0] = new SlotType1();
            objSlotType1[0].name = CDAConstants.SLOT_NAME_CREATION_TIME;//"creationTime";
            objSlotType1[0].ValueList = new ValueListType();
            objSlotType1[0].ValueList.Value = new string[1];
            objSlotType1[0].ValueList.Value[0] = "20051224";//DateTime.UtcNow.ToFileTime().ToString(); ToUniversalTime().ToString();//"20051224";// DateTime.UtcNow.ToString();//"20051224";//System.DateTime.Now.Year.ToString() + "0" + System.DateTime.Now.Month.ToString() + "0" + System.DateTime.Now.Day.ToString();


            // put langauge code;
            objSlotType1[1] = new SlotType1();
            objSlotType1[1].name = "languageCode";
            objSlotType1[1].ValueList = new ValueListType();
            objSlotType1[1].ValueList.Value = new string[1];
            objSlotType1[1].ValueList.Value[0] = CDAConstants.LANGUAGE_CODE_ENGLISH;// "en-us";

            // put service Start Time;
            objSlotType1[2] = new SlotType1();
            objSlotType1[2].name = CDAConstants.SLOT_NAME_SERVICE_START_TIME;// "servieStartTime";
            objSlotType1[2].ValueList = new ValueListType();
            objSlotType1[2].ValueList.Value = new string[1];
            objSlotType1[2].ValueList.Value[0] = "200412230800";//"20051224";// DateTime.UtcNow.ToString();//"200412230800";//System.DateTime.Now.Year.ToString() + "0" + System.DateTime.Now.Month.ToString() + "0" + System.DateTime.Now.Day.ToString();

            objSlotType1[3] = new SlotType1();
            objSlotType1[3].name = "servieStopTime";
            objSlotType1[3].ValueList = new ValueListType();
            objSlotType1[3].ValueList.Value = new string[1];
            objSlotType1[3].ValueList.Value[0] = "200412230801";// DateTime.UtcNow.ToString();// "200412230801";//System.DateTime.Now.Year.ToString() + "0" + System.DateTime.Now.Month.ToString() + "0" + System.DateTime.Now.Day.ToString();

            objSlotType1[4] = new SlotType1();
            objSlotType1[4].name = "sourcePatientId";
            objSlotType1[4].ValueList = new ValueListType();
            objSlotType1[4].ValueList.Value = new string[1];
            objSlotType1[4].ValueList.Value[0] = sPatientId;

            // 
            objSlotType1[5] = new SlotType1();
            objSlotType1[5].name = "sourcePatientInfo";
            objSlotType1[5].ValueList = new ValueListType();
            objSlotType1[5].ValueList.Value = new string[5];
            // objSlotType1[5].ValueList.Value[0] = "PID-3|ST-1000^^^&1.3.6.1.4.1.21367.2003.3.9&ISO";
            objSlotType1[5].ValueList.Value[0] = "PID-3|ST-1000^^^&1.3.6.1.4.1.21367.2003.3.9&ISO";
            objSlotType1[5].ValueList.Value[1] = "PID-5|Doe^John^^^";
            objSlotType1[5].ValueList.Value[2] = "PID-7|19560527";
            objSlotType1[5].ValueList.Value[3] = "PID-8|M";
            objSlotType1[5].ValueList.Value[4] = "PID-11|100 Main St^^Metropolis^Il^44130^USA";

            objE.Slot = objSlotType1;


            objE.status = CDAConstants.PROVIDE_REGISTER_STATUS_APPROVED;
            //objE[0].Name=
            ProvideAndRegisterDocumentSetRequest.SubmitObjectsRequest.RegistryObjectList = new IdentifiableType[4];
            ProvideAndRegisterDocumentSetRequest.SubmitObjectsRequest.RegistryObjectList[0] = objE;//objIType;// new IdentifiableType[1];

            HarrisStore.RegistryPackageType oRegistryPackage = new RegistryPackageType();
            oRegistryPackage.ExternalIdentifier = new ExternalIdentifierType[4];

            oRegistryPackage.objectType = CDAConstants.XDS_REGISTRY_REGISTRY_PACKAGE_TYPE;
            oRegistryPackage.id = CDAConstants.EXTERNAL_IDENTIFICATION_SCHEMA_REGISTRYOBJECT;

            oRegistryPackage.ExternalIdentifier[0] = createExternalIdentifier(CDAConstants.EXTERNAL_IDENTIFICATION_SCHEMA_REGISTRYOBJECT,
                CDAConstants.EXTERNAL_IDENTIFICATION_SCHEMA_PATIENTID,
                CDAConstants.EXTERNAL_OBJECT_IDENTIFIER_TYPE, sPatientId,

                CDAConstants.CHARACTER_SET,
                CDAConstants.LANGUAGE_CODE_ENGLISH,
                CDAConstants.PROVIDE_REGISTER_SLOT_NAME_DOC_SUBMISSION_SET_PATIENT_ID);


            oRegistryPackage.ExternalIdentifier[1] = createExternalIdentifier(CDAConstants.EXTERNAL_IDENTIFICATION_SCHEMA_REGISTRYOBJECT,
            CDAConstants.EXTERNAL_IDENTIFICATION_SCHEMA_UNIQUEID,
            CDAConstants.EXTERNAL_OBJECT_IDENTIFIER_TYPE, "E013",
            CDAConstants.CHARACTER_SET,
            CDAConstants.LANGUAGE_CODE_ENGLISH,
            CDAConstants.PROVIDE_REGISTER_SLOT_NAME_DOC_SUBMISSION_SET_DOCUMENT_ID);

            oRegistryPackage.ExternalIdentifier[2] = getHomeCommunityIdExternalIdentifier(homecomunityid);

            // add registry package;
            ProvideAndRegisterDocumentSetRequest.SubmitObjectsRequest.RegistryObjectList[1] = oRegistryPackage;

            AssociationType1 oAssociation = new AssociationType1();
            oAssociation.associationType = CDAConstants.XDS_REGISTRY_ASSOCIATION_TYPE;
            oAssociation.sourceObject = CDAConstants.EXTERNAL_IDENTIFICATION_SCHEMA_REGISTRYOBJECT;
            oAssociation.targetObject = sDocUniqueId;
            oAssociation.id = Guid.NewGuid().ToString();// (UUID.randomUUID().toString());
            oAssociation.objectType = CDAConstants.XDS_REGISTRY_ASSOCIATION_OBJECT_TYPE;

            oAssociation.Slot = new SlotType1[1];
            oAssociation.Slot[0] = createSlot("SubmissionSetStatus", "Original");

            // add registry association;
            ProvideAndRegisterDocumentSetRequest.SubmitObjectsRequest.RegistryObjectList[2] = oRegistryPackage;

            ClassificationType oClassificationSumissionSet = new ClassificationType();
            oClassificationSumissionSet.classificationNode = CDAConstants.PROVIDE_REGISTER_SUBMISSION_SET_CLASSIFICATION_UUID;
            oClassificationSumissionSet.classifiedObject = CDAConstants.EXTERNAL_IDENTIFICATION_SCHEMA_REGISTRYOBJECT;
            oClassificationSumissionSet.objectType = CDAConstants.CLASSIFICATION_REGISTRY_OBJECT;
            oClassificationSumissionSet.id = Guid.NewGuid().ToString();//(UUID.randomUUID().toString());

            // add registry association;
            ProvideAndRegisterDocumentSetRequest.SubmitObjectsRequest.RegistryObjectList[3] = oClassificationSumissionSet;

            ProvideAndRegisterDocumentSetRequest.Document = oDoc;
            //XmlSerializer serializer = new XmlSerializer(typeof(RespondingGateway_ProvideAndRegisterDocumentSetRequestType));
            //TextWriter writer = new StreamWriter(Server.MapPath("\\SubmissionRequest.xml"));
            //serializer.Serialize(writer, objRegisterDocument);
            //writer.Close();
            //RegistryResponseType objRegistryResponseType = objEntityXDR_Service.ProvideAndRegisterDocumentSetb(objRegisterDocument);
            // To print the response status
            //Response.Write(objRegistryResponseType.status);

            return ProvideAndRegisterDocumentSetRequest;

        }
        public byte[] ReadByteArrayFromFile(string fileName)
        {
            byte[] buff = null;
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(fileName).Length;
            buff = br.ReadBytes((int)numBytes);
            return buff;
        }
        private ExternalIdentifierType setPatientId(String sDocUniqueId, String hl7PatientId)
        {
            ExternalIdentifierType oExtIdTypePat = createExternalIdentifier(
                    sDocUniqueId,
                    CDAConstants.EBXML_RESPONSE_PATIENTID_IDENTIFICATION_SCHEME,
                    CDAConstants.EXTERNAL_OBJECT_IDENTIFIER_TYPE,
                    hl7PatientId,
                    CDAConstants.CHARACTER_SET,
                    CDAConstants.LANGUAGE_CODE_ENGLISH,
                    CDAConstants.PROVIDE_REGISTER_SLOT_NAME_PATIENT_ID);

            return oExtIdTypePat;
        }
        private ExternalIdentifierType setDocumentUniqueId(String sDocUniqueId)
        {
            ExternalIdentifierType oExtIdTypePat = createExternalIdentifier(sDocUniqueId,
                    CDAConstants.DOCUMENT_ID_IDENT_SCHEME,
                    CDAConstants.EXTERNAL_OBJECT_IDENTIFIER_TYPE,
                    sDocUniqueId,
                    CDAConstants.CHARACTER_SET,
                    CDAConstants.LANGUAGE_CODE_ENGLISH,
                    CDAConstants.PROVIDE_REGISTER_SLOT_NAME_DOCUMENT_ID);

            return oExtIdTypePat;
        }
        private ExternalIdentifierType createExternalIdentifier(String regObject, String identScheme, String objType, String value, String nameCharSet, String nameLang, String nameVal)
        {
            String id = Guid.NewGuid().ToString();// "id03";// UUID.randomUUID().toString();
            ExternalIdentifierType idType = new ExternalIdentifierType();// createExternalIdentifierType();
            idType.id = id;
            idType.registryObject = regObject;
            idType.identificationScheme = identScheme;
            idType.objectType = objType;
            idType.value = value;
            idType.Name = createInternationalStringType(nameCharSet, nameLang, nameVal);
            return idType;
        }
        private InternationalStringType createInternationalStringType(String charSet, String language, String value)
        {
            InternationalStringType intStr = new InternationalStringType();
            LocalizedStringType locStr = new LocalizedStringType();
            locStr.charset = charSet;
            locStr.lang = language;
            locStr.value = value;
            intStr.LocalizedString = new LocalizedStringType[1];
            intStr.LocalizedString[0] = locStr;
            return intStr;
        }
        private ExternalIdentifierType getHomeCommunityIdExternalIdentifier(String sHomeCommunityId)
        {
            ExternalIdentifierType oExtIdTypePatForReg = createExternalIdentifier(CDAConstants.EXTERNAL_IDENTIFICATION_SCHEMA_REGISTRYOBJECT,
                CDAConstants.PROVIDE_REGISTER_SUBMISSION_SET_SOURCE_ID_UUID,
                CDAConstants.EXTERNAL_OBJECT_IDENTIFIER_TYPE,
                sHomeCommunityId,
                CDAConstants.CHARACTER_SET,
                CDAConstants.LANGUAGE_CODE_ENGLISH,
                CDAConstants.PROVIDE_REGISTER_SLOT_NAME_SUBMISSION_SET_SOURCE_ID);

            return oExtIdTypePatForReg;
        }
        private SlotType1 createSlot(String name, String value)
        {
            SlotType1 slot = new SlotType1();
            slot.name = name;
            ValueListType valList = new ValueListType();
            valList.Value = new string[1];
            valList.Value[0] = value;
            slot.ValueList = valList; //setValueListvalList);
            return slot;
        }

        private ClassificationType setClassCode(String sDocUniqueId)
        {
            ClassificationType oClassificationClassCode = null;

            oClassificationClassCode = createClassification(
                CDAConstants.XDS_CLASS_CODE_SCHEMA_UUID,
                    sDocUniqueId,
                    "",
                    CDAConstants.METADATA_CLASS_CODE,
                    CDAConstants.CLASSIFICATION_REGISTRY_OBJECT,
                    CDAConstants.CLASSIFICATION_SCHEMA_CDNAME,
                    CDAConstants.CODE_SYSTEM_LOINC_OID,
                    CDAConstants.CHARACTER_SET,
                    CDAConstants.LANGUAGE_CODE_ENGLISH,
                    CDAConstants.METADATA_CLASS_CODE_DISPLAY_NAME);

            return oClassificationClassCode;
        }

        private ClassificationType setFormatCode(String sDocUniqueId)
        {
            String sFormatCode = CDAConstants.METADATA_FORMAT_CODE_XACML;

            ClassificationType oClassificationClassCode = createClassification(
                    CDAConstants.CLASSIFICATION_SCHEMA_IDENTIFIER_FORMAT_CODE,
                    sDocUniqueId,
                    "",
                    sFormatCode,
                    CDAConstants.CLASSIFICATION_REGISTRY_OBJECT,
                    CDAConstants.CLASSIFICATION_SCHEMA_CDNAME,
                    CDAConstants.METADATA_FORMAT_CODE_SYSTEM,
                    CDAConstants.CHARACTER_SET,
                    CDAConstants.LANGUAGE_CODE_ENGLISH,
                    "");

            return oClassificationClassCode;

        }

        private ClassificationType setConfidentialityCode(String sDocUniqueId)
        {
            String sConfidentialityCode = "R";
            String sConfidentialityCodeScheme = "2.16.840.1.113883.5.25";
            String sConfidentialityCodeDisplayName = "Restricted";

            ClassificationType oClassificationConfd = createClassification(CDAConstants.PROVIDE_REGISTER_CONFIDENTIALITY_CODE_UUID,
                    sDocUniqueId,
                    "",
                    sConfidentialityCode,
                    CDAConstants.CLASSIFICATION_REGISTRY_OBJECT,
                    CDAConstants.CLASSIFICATION_SCHEMA_CDNAME,
                    sConfidentialityCodeScheme,
                    CDAConstants.CHARACTER_SET,
                    CDAConstants.LANGUAGE_CODE_ENGLISH,
                    sConfidentialityCodeDisplayName);

            return oClassificationConfd;

        }

        private ClassificationType createClassification(String scheme, String clObject, String id, String nodeRep, String objType, String slotName, String slotVal, String nameCharSet, String nameLang, String nameVal)
        {
            ClassificationType cType = new ClassificationType();
            cType.classificationScheme = scheme;
            cType.classifiedObject = clObject;
            cType.nodeRepresentation = nodeRep;
            if (id != null && !id.Equals(""))
            {
                cType.id = id;
            }
            else
            {
                cType.id = Guid.NewGuid().ToString();
            }

            cType.objectType = objType;
            cType.Slot = new SlotType1[1];
            cType.Slot[0] = createSlot(slotName, slotVal);
            cType.Name = createInternationalStringType(nameCharSet, nameLang, nameVal);

            return cType;
        }


    }
}
