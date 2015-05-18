using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using SAMLAssertion.SAMLService;
using SAMLAssertion.HarrisStore;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using System.Xml;
using Mobius.CoreLibrary;
using System.ComponentModel;
using C32Utility;

namespace SAMLAssertion
{
    public partial class SAMLAssertionForDocSubmit
    {
        private bool IsPolicyDocument { get; set; }
        private SAMLAssertionHelper SAMLAssertionHelper
        {
            get;
            set;
        }


        public ProvideAndRegisterDocumentSetRequestType GetProvideAndRegisterDocumentSetRequestType(string documentSubmissionComunityId, string documentId,
            byte[] documentBytes, string patientId, string comment)
        {
            ProvideAndRegisterDocumentSetRequestType provideAndRegisterDocumentSetRequestType = null;
            this.SAMLAssertionHelper = new SAMLAssertionHelper(false);
            //Generate HL7 Patient Id
            string hl7PatientID = this.SAMLAssertionHelper.GetHL7EncodePatientId(patientId, documentSubmissionComunityId);
            //Read Basic information from C32 Document
            CDAHelper CDAHelper = new CDAHelper(documentBytes);
            string PatientFirstName = (CDAHelper.PatientName != null && CDAHelper.PatientName.GivenName != null && CDAHelper.PatientName.GivenName.Count>0) ? CDAHelper.PatientName.GivenName[0] : string.Empty;
            string PatientLastName = (CDAHelper.PatientName != null && CDAHelper.PatientName.FamilyName != null && CDAHelper.PatientName.FamilyName.Count > 0) ? CDAHelper.PatientName.FamilyName[0] : string.Empty;
            string PatientDOB = CDAHelper.PatientDOB;
            string PatientGender = CDAHelper.PatientGender;
            string LanguageCode = CDAHelper.Language;
            string PatientStreetAddress = string.Empty;
            string PatientCity = string.Empty;
            string PatientState = string.Empty;
            string PatientPostalCode = string.Empty;

            if (CDAHelper.PatientAddress != null && CDAHelper.PatientAddress.Count > 0)
            {
                PatientStreetAddress = CDAHelper.PatientAddress[0].StreetAddressLine;
                PatientCity = CDAHelper.PatientAddress[0].City;
                PatientState = CDAHelper.PatientAddress[0].State;
                PatientPostalCode = CDAHelper.PatientAddress[0].PostalCode;
            }

            ProvideAndRegisterDocumentSetRequestTypeDocument[] provideAndRegisterDocumentSetRequestTypeDocument = new ProvideAndRegisterDocumentSetRequestTypeDocument[1];
            provideAndRegisterDocumentSetRequestTypeDocument[0] = new ProvideAndRegisterDocumentSetRequestTypeDocument();
            provideAndRegisterDocumentSetRequestTypeDocument[0].id = documentId;
            provideAndRegisterDocumentSetRequestTypeDocument[0].Value = documentBytes;

            provideAndRegisterDocumentSetRequestType = new ProvideAndRegisterDocumentSetRequestType();
            provideAndRegisterDocumentSetRequestType.SubmitObjectsRequest = new SubmitObjectsRequest();
            provideAndRegisterDocumentSetRequestType.SubmitObjectsRequest.id = "123";
            provideAndRegisterDocumentSetRequestType.SubmitObjectsRequest.comment = comment;
            provideAndRegisterDocumentSetRequestType.Document = provideAndRegisterDocumentSetRequestTypeDocument;
            

            //Add document to request
            //get the Hl 7 format patientId
            ExtrinsicObjectType extrinsicObjectType = new ExtrinsicObjectType();//ExtrinsicObjectType();
            //Document Title 
            extrinsicObjectType.Name = this.SAMLAssertionHelper.GetInternationalStringType(string.Empty, string.Empty, documentId);

            List<ExternalIdentifierType> externalIdentifierTypeList = new List<ExternalIdentifierType>();
            //Set PatientId 
            externalIdentifierTypeList.Add(this.SAMLAssertionHelper.GetPatientId(documentId, hl7PatientID));
            //setDocumentUniqueId
            externalIdentifierTypeList.Add(this.SAMLAssertionHelper.GetDocumentId(documentId));
            extrinsicObjectType.ExternalIdentifier = externalIdentifierTypeList.ToArray();
            //Author
            /*
            Person
            Institution
            Role
            Specialty
            */
            //Set mimeType
            extrinsicObjectType.mimeType = CDAConstants.PROVIDE_REGISTER_MIME_TYPE;
            //Set ClassificationType
            List<ClassificationType> classificationTypeList = new List<ClassificationType>();
            //set Author Type
            classificationTypeList.Add(this.SAMLAssertionHelper.GetAuthorClassCode(documentId, CDAHelper.Authors!=null && CDAHelper.Authors.Count>0?  CDAHelper.Authors[0]: null));
            //Set Class Code                
            classificationTypeList.Add(this.SAMLAssertionHelper.GetCodeSystem(documentId, EnumClassificationType.ClassCode,
                                                                                                CDAHelper.Code, CDAHelper.CodeSystem,
                                                    CDAHelper.CodeDisplayName, CDAHelper.Language));
            //Set Format Code                
            classificationTypeList.Add(this.SAMLAssertionHelper.GetCodeSystem(documentId, EnumClassificationType.FormatCode));
            //set Confidentiality Code
            classificationTypeList.Add(this.SAMLAssertionHelper.GetCodeSystem(documentId, EnumClassificationType.ConfidentialityCode,
                                                                                CDAHelper.ConfidentialityCode,
                                                                                CDAHelper.ConfidentialitySystemCode,
                                                                                CDAHelper.ConfidentialityDisplayName,
                                                                                CDAHelper.Language));
            //Set Facility Type Code
            classificationTypeList.Add(this.SAMLAssertionHelper.GetCodeSystem(documentId, EnumClassificationType.FacilityType, CDAHelper.FacilityTypeCode,
                                                                                CDAHelper.FacilityTypeCodeSystem, CDAHelper.FacilityDisplayName, CDAHelper.Language));
            //Set Practice Setting 
            classificationTypeList.Add(this.SAMLAssertionHelper.GetCodeSystem(documentId, EnumClassificationType.PracticeSetting));
            //Set Type Code
            classificationTypeList.Add(this.SAMLAssertionHelper.GetCodeSystem(documentId, EnumClassificationType.TypeCode,
                                               CDAHelper.Code, CDAHelper.CodeSystem, CDAHelper.CodeDisplayName,
                                                    CDAHelper.Language));

            //Add Classification collection to ExtrinsicObjectType object
            extrinsicObjectType.Classification = classificationTypeList.ToArray();

            //          set document id;
            extrinsicObjectType.id = provideAndRegisterDocumentSetRequestTypeDocument[0].id;


            //Slot Creations
            List<SlotType1> SlotTypeList = new List<SlotType1>();
            //SLOT_NAME_CREATION_TIME
            SlotTypeList.Add(this.SAMLAssertionHelper.GetSlotType(CDAConstants.SLOT_NAME_CREATION_TIME, CDAHelper.DocumentCreationDate));
            // Set language code;            
            SlotTypeList.Add(this.SAMLAssertionHelper.GetSlotType(CDAConstants.SLOT_NAME_LANGUAGE_CODE, CDAHelper.Language));// "en-us";
            // Set service Start Time SLOT_NAME_SERVICE_START_TIME
            SlotTypeList.Add(this.SAMLAssertionHelper.GetSlotType(CDAConstants.SLOT_NAME_SERVICE_START_TIME, CDAHelper.ServiceStartTime));
            // Set service Start Time SLOT_NAME_SERVICE_STOP time
            SlotTypeList.Add(this.SAMLAssertionHelper.GetSlotType(CDAConstants.SLOT_NAME_SERVICE_STOP_TIME, CDAHelper.ServiceStopTime));
            //SLOT_NAME_SOURCE_PATIENT_ID
            SlotTypeList.Add(this.SAMLAssertionHelper.GetSlotType(CDAConstants.SLOT_NAME_SOURCE_PATIENT_ID, this.SAMLAssertionHelper.GetHL7EncodePatientId(patientId, MobiusAppSettingReader.LocalHomeCommunityID)));
            //SLOT_NAME_SOURCE_PATIENT_INFO
            SlotTypeList.Add(this.SAMLAssertionHelper.GetSlotType(CDAConstants.SLOT_NAME_SOURCE_PATIENT_INFO,
               "PID-3|" + hl7PatientID, "PID-5|" + PatientFirstName + "^" + PatientLastName + "^^^", "PID-7|" + PatientDOB
               , "PID-8|" + PatientGender, "PID-11|" + PatientStreetAddress + "^^" + PatientCity + "^" + PatientState + "^" + PatientPostalCode));
            ////SLOT_NAME_INTENDED_RECIPIENT
            SlotTypeList.Add(this.SAMLAssertionHelper.GetSlotType(CDAConstants.SLOT_NAME_INTENDED_RECIPIENT, documentSubmissionComunityId));
            //Assigning the slots to extrinsic object

            //LegalAuthenticator
            SlotTypeList.Add(this.SAMLAssertionHelper.GetSlotType(CDAConstants.SLOT_NAME_LEGAL_AUTHENTICATOR, CDAHelper.LegalAuthenticatorName));

            extrinsicObjectType.Slot = SlotTypeList.ToArray();
            extrinsicObjectType.status = EnumHelper.GetAttributeOfType<DescriptionAttribute>(CDADocumentStatus.Approved);
            //Set Document Title Column Name
            extrinsicObjectType.Name = this.SAMLAssertionHelper.GetInternationalStringType(string.Empty, string.Empty, CDAHelper.DocumentTitle);
            //Column Description
            extrinsicObjectType.Description = this.SAMLAssertionHelper.GetInternationalStringType(string.Empty, string.Empty, "Comment- Annual Physical");
            provideAndRegisterDocumentSetRequestType.SubmitObjectsRequest.RegistryObjectList = new SAMLAssertion.HarrisStore.ExtrinsicObjectType[1];
            //comment
            provideAndRegisterDocumentSetRequestType.SubmitObjectsRequest.comment = comment;
            provideAndRegisterDocumentSetRequestType.SubmitObjectsRequest.RegistryObjectList[0] = extrinsicObjectType;

            
            

            return provideAndRegisterDocumentSetRequestType;
        }


        public ProvideAndRegisterDocumentSetRequestType GetProvideAndRegisterDocumentSetBForPolicyDocument(string documentSubmissionComunityId, string documentId,
             byte[] xacmlDocumentBytes, byte[] docBytes, DateTime creationTime, DateTime ruleStartTime, DateTime ruleStopTime, string patientId)
        {
            IsPolicyDocument = true;
            this.SAMLAssertionHelper = new SAMLAssertionHelper(true);
            //Generate HL7 Patient Id
            string hl7PatientID = this.SAMLAssertionHelper.GetHL7EncodePatientId(patientId, documentSubmissionComunityId);

            //Read Basic information from C32 Document
            CDAHelper CDAHelper = new CDAHelper(docBytes);
            string PatientFirstName = (CDAHelper.PatientName != null && CDAHelper.PatientName.GivenName != null && CDAHelper.PatientName.GivenName.Count > 0) ? CDAHelper.PatientName.GivenName[0] : string.Empty;
            string PatientLastName = (CDAHelper.PatientName != null && CDAHelper.PatientName.FamilyName != null && CDAHelper.PatientName.FamilyName.Count > 0) ? CDAHelper.PatientName.FamilyName[0] : string.Empty;
            string PatientDOB = CDAHelper.PatientDOB;
            string PatientGender = CDAHelper.PatientGender;
            string LanguageCode = CDAHelper.Language;
            string PatientStreetAddress = string.Empty;
            string PatientCity = string.Empty;
            string PatientState = string.Empty;
            string PatientPostalCode = string.Empty;

            if (CDAHelper.PatientAddress != null && CDAHelper.PatientAddress.Count > 0)
            {
                PatientStreetAddress = CDAHelper.PatientAddress[0].StreetAddressLine;
                PatientCity = CDAHelper.PatientAddress[0].City;
                PatientState = CDAHelper.PatientAddress[0].State;
                PatientPostalCode = CDAHelper.PatientAddress[0].PostalCode;
            }

            ProvideAndRegisterDocumentSetRequestTypeDocument[] provideAndRegisterDocumentSetRequestTypeDocument = new ProvideAndRegisterDocumentSetRequestTypeDocument[1];
            provideAndRegisterDocumentSetRequestTypeDocument[0] = new ProvideAndRegisterDocumentSetRequestTypeDocument();
            provideAndRegisterDocumentSetRequestTypeDocument[0].id = documentId;
            provideAndRegisterDocumentSetRequestTypeDocument[0].Value = xacmlDocumentBytes;
            //Ends Addition

            ProvideAndRegisterDocumentSetRequestType ProvideAndRegisterDocumentSetRequest = new ProvideAndRegisterDocumentSetRequestType();
            ProvideAndRegisterDocumentSetRequest.SubmitObjectsRequest = new SubmitObjectsRequest();
            ProvideAndRegisterDocumentSetRequest.SubmitObjectsRequest.id = "123";
            ProvideAndRegisterDocumentSetRequest.SubmitObjectsRequest.comment = "System Generated Document for authorizing document use.";

            ExtrinsicObjectType extrinsicObjectType = new ExtrinsicObjectType();//ExtrinsicObjectType();
            //Document Title 
            extrinsicObjectType.Name = this.SAMLAssertionHelper.GetInternationalStringType(string.Empty, string.Empty, documentId);

            List<ExternalIdentifierType> externalIdentifierTypeList = new List<ExternalIdentifierType>();
            //Set PatientId 
            externalIdentifierTypeList.Add(this.SAMLAssertionHelper.GetPatientId(documentId, hl7PatientID));
            //setDocumentUniqueId
            externalIdentifierTypeList.Add(this.SAMLAssertionHelper.GetDocumentId(documentId));
            extrinsicObjectType.ExternalIdentifier = externalIdentifierTypeList.ToArray();
            // set mime type
            extrinsicObjectType.mimeType = CDAConstants.PROVIDE_REGISTER_MIME_TYPE;

            //Set ClassificationType
            List<ClassificationType> classificationTypeList = new List<ClassificationType>();


            //Set Class Code                
            classificationTypeList.Add(this.SAMLAssertionHelper.GetCodeSystem(documentId, EnumClassificationType.ClassCode));
            //Set Format Code                
            classificationTypeList.Add(this.SAMLAssertionHelper.GetCodeSystem(documentId, EnumClassificationType.FormatCode));
            //set Confidentiality Code
            classificationTypeList.Add(this.SAMLAssertionHelper.GetCodeSystem(documentId, EnumClassificationType.ConfidentialityCode));
            //Set Type Code
            classificationTypeList.Add(this.SAMLAssertionHelper.GetCodeSystem(documentId, EnumClassificationType.TypeCode));

            //set Author Type
            classificationTypeList.Add(this.SAMLAssertionHelper.GetAuthorClassCode(documentId, (CDAHelper.Authors !=null && CDAHelper.Authors.Count>0 )? CDAHelper.Authors[0]: null));

            //Add Classification collection to ExtrinsicObjectType object
            extrinsicObjectType.Classification = classificationTypeList.ToArray();

            //          set document id;
            extrinsicObjectType.id = provideAndRegisterDocumentSetRequestTypeDocument[0].id;
            //          objE[0].class

            extrinsicObjectType.objectType = CDAConstants.PROVIDE_REGISTER_OBJECT_TYPE;


            List<SlotType1> SlotTypeList = new List<SlotType1>();

            //CDAConstants.SLOT_NAME_CREATION_TIME;//"creationTime";
            if (IsPolicyDocument)
            {
                //In Case of policy document, document creation time will be set as current date time
                SlotTypeList.Add(this.SAMLAssertionHelper.GetSlotType(CDAConstants.SLOT_NAME_CREATION_TIME, DateTime.Now.ToString("yyyyMMddhhmm", System.Globalization.CultureInfo.GetCultureInfo("en-US"))));
            }
            else
            {
                //CDAConstants.SLOT_NAME_CREATION_TIME;//"creationTime";
                SlotTypeList.Add(this.SAMLAssertionHelper.GetSlotType(CDAConstants.SLOT_NAME_CREATION_TIME, CDAHelper.DocumentCreationDate));
            }

            // Set langauge code;
            SlotTypeList.Add(this.SAMLAssertionHelper.GetSlotType(CDAConstants.SLOT_NAME_LANGUAGE_CODE, CDAConstants.LANGUAGE_CODE_ENGLISH));// "en-us";
            // Set service Start Time;
            SlotTypeList.Add(this.SAMLAssertionHelper.GetSlotType(CDAConstants.SLOT_NAME_SERVICE_START_TIME, ruleStartTime.ToString("yyyyMMddhhmm", System.Globalization.CultureInfo.GetCultureInfo("en-US"))));
            //Set SERVICE STOP TIME
            SlotTypeList.Add(this.SAMLAssertionHelper.GetSlotType(CDAConstants.SLOT_NAME_SERVICE_STOP_TIME, ruleStopTime.ToString("yyyyMMddhhmm", System.Globalization.CultureInfo.GetCultureInfo("en-US"))));
            //SLOT_NAME_SOURCE_PATIENT_ID
            SlotTypeList.Add(this.SAMLAssertionHelper.GetSlotType(CDAConstants.SLOT_NAME_SOURCE_PATIENT_ID, hl7PatientID));

            // sourcePatientInfo - SLOT_NAME_SOURCE_PATIENT_INFO
            SlotTypeList.Add(this.SAMLAssertionHelper.GetSlotType(CDAConstants.SLOT_NAME_SOURCE_PATIENT_INFO,
                "PID-3|" + hl7PatientID,
                "PID-5|" + PatientFirstName + "^" + PatientLastName + "^^^",
                "PID-7|" + PatientDOB,
                "PID-8|" + PatientGender,
                "PID-11|" + PatientStreetAddress + "^^" + PatientCity + "^" + PatientState + "^" + PatientPostalCode));

            //SLOT_NAME_INTENDED_RECIPIENT
            //TODO Need to set the Intended recipient based on condition
            SlotTypeList.Add(this.SAMLAssertionHelper.GetSlotType(CDAConstants.SLOT_NAME_INTENDED_RECIPIENT, documentSubmissionComunityId + "|" + "documentSubmissionComunityid" + documentSubmissionComunityId));

            extrinsicObjectType.Slot = SlotTypeList.ToArray();

            extrinsicObjectType.status = EnumHelper.GetAttributeOfType<DescriptionAttribute>(CDADocumentStatus.Approved);

            //Add IdentifiableType objects
            List<IdentifiableType> RegistryObjectList = new List<IdentifiableType>();

            RegistryObjectList.Add(extrinsicObjectType);
            // add registry package;
            RegistryObjectList.Add(GetRegistryPackage(hl7PatientID, documentSubmissionComunityId));
            // add registry association;
            RegistryObjectList.Add(GetAssociation(documentId));
            // add registry ClassificationType;
            RegistryObjectList.Add(GetClassificationType());

            ProvideAndRegisterDocumentSetRequest.SubmitObjectsRequest.RegistryObjectList = RegistryObjectList.ToArray();

            //Adding Document bytes in request set
            ProvideAndRegisterDocumentSetRequest.Document = provideAndRegisterDocumentSetRequestTypeDocument;

            return ProvideAndRegisterDocumentSetRequest;

        }



        private RegistryPackageType GetRegistryPackage(string hl7PatientID, string documentSubmissionComunityId)
        {
            RegistryPackageType registryPackage = new RegistryPackageType();
            registryPackage.objectType = CDAConstants.XDS_REGISTRY_REGISTRY_PACKAGE_TYPE;
            registryPackage.id = CDAConstants.EXTERNAL_IDENTIFICATION_SCHEMA_REGISTRYOBJECT;
            registryPackage.ExternalIdentifier = GetExternalIdentifier(hl7PatientID, documentSubmissionComunityId).ToArray();

            return registryPackage;
        }

        private ClassificationType GetClassificationType()
        {
            ClassificationType classificationType = new ClassificationType();
            classificationType.classificationNode = CDAConstants.PROVIDE_REGISTER_SUBMISSION_SET_CLASSIFICATION_UUID;
            classificationType.classifiedObject = CDAConstants.EXTERNAL_IDENTIFICATION_SCHEMA_REGISTRYOBJECT;
            classificationType.objectType = CDAConstants.CLASSIFICATION_REGISTRY_OBJECT;
            classificationType.id = Guid.NewGuid().ToString();//(UUID.randomUUID().toString());
            return classificationType;
        }

        private AssociationType1 GetAssociation(string documentId)
        {
            AssociationType1 association = new AssociationType1();
            association.associationType = CDAConstants.XDS_REGISTRY_ASSOCIATION_TYPE;
            association.sourceObject = CDAConstants.EXTERNAL_IDENTIFICATION_SCHEMA_REGISTRYOBJECT;
            association.targetObject = documentId;
            association.id = Guid.NewGuid().ToString();
            association.objectType = CDAConstants.XDS_REGISTRY_ASSOCIATION_OBJECT_TYPE;

            association.Slot = new SlotType1[] { this.SAMLAssertionHelper.GetSlotType("SubmissionSetStatus", "Original") };
            return association;
        }

        private List<ExternalIdentifierType> GetExternalIdentifier(string HL7PatientId, string communityId)
        {
            List<ExternalIdentifierType> ExternalIdentifierType = new List<ExternalIdentifierType>();
            ExternalIdentifierType.Add(this.SAMLAssertionHelper.GetExternalIdentifier(CDAConstants.EXTERNAL_IDENTIFICATION_SCHEMA_REGISTRYOBJECT,
              CDAConstants.EXTERNAL_IDENTIFICATION_SCHEMA_PATIENTID,
              CDAConstants.EXTERNAL_OBJECT_IDENTIFIER_TYPE, HL7PatientId,
              CDAConstants.CHARACTER_SET,
              CDAConstants.LANGUAGE_CODE_ENGLISH,
              CDAConstants.PROVIDE_REGISTER_SLOT_NAME_DOC_SUBMISSION_SET_PATIENT_ID));


            ExternalIdentifierType.Add(this.SAMLAssertionHelper.GetExternalIdentifier(CDAConstants.EXTERNAL_IDENTIFICATION_SCHEMA_REGISTRYOBJECT,
            CDAConstants.EXTERNAL_IDENTIFICATION_SCHEMA_UNIQUEID,
            CDAConstants.EXTERNAL_OBJECT_IDENTIFIER_TYPE, "E013",
            CDAConstants.CHARACTER_SET,
            CDAConstants.LANGUAGE_CODE_ENGLISH,
            CDAConstants.PROVIDE_REGISTER_SLOT_NAME_DOC_SUBMISSION_SET_DOCUMENT_ID));

            ExternalIdentifierType.Add(this.SAMLAssertionHelper.GetExternalIdentifier(CDAConstants.EXTERNAL_IDENTIFICATION_SCHEMA_REGISTRYOBJECT,
               CDAConstants.PROVIDE_REGISTER_SUBMISSION_SET_SOURCE_ID_UUID,
               CDAConstants.EXTERNAL_OBJECT_IDENTIFIER_TYPE,
               communityId,
               CDAConstants.CHARACTER_SET,
               CDAConstants.LANGUAGE_CODE_ENGLISH,
               CDAConstants.PROVIDE_REGISTER_SLOT_NAME_SUBMISSION_SET_SOURCE_ID));
            return ExternalIdentifierType;
        }


    }
}
