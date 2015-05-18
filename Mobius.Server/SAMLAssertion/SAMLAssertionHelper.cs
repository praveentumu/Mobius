using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAMLAssertion.HarrisStore;
using C32Utility;

namespace SAMLAssertion
{
    public enum EnumClassificationType
    {
        ClassCode,
        FormatCode,
        ConfidentialityCode,
        FacilityType,
        PracticeSetting,
        TypeCode,
    }
    public class SAMLAssertionHelper
    {

        private bool IsPolicyDocument { get; set; }

        private string NewGuid
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }

        public SAMLAssertionHelper(bool policyDocument)
        {

            IsPolicyDocument = policyDocument;
        }

        /// <summary>
        /// This method will create the ExternalIdentifierType object
        /// </summary>
        /// <param name="registryObject"></param>
        /// <param name="identificationScheme"></param>
        /// <param name="objectType"></param>
        /// <param name="value"></param>
        /// <param name="localizedCharSet"></param>
        /// <param name="language"></param>
        /// <param name="localizedValue"></param>
        /// <returns></returns>
        public ExternalIdentifierType GetExternalIdentifier(string registryObject, string identificationScheme, string objectType, string value,
            string localizedCharSet, string language, string localizedValue)
        {
            ExternalIdentifierType externalIdentifierType = new ExternalIdentifierType();
            externalIdentifierType.id = this.NewGuid;
            externalIdentifierType.registryObject = registryObject;
            externalIdentifierType.identificationScheme = identificationScheme;
            externalIdentifierType.objectType = objectType;
            externalIdentifierType.value = value;
            externalIdentifierType.Name = this.GetInternationalStringType(localizedCharSet, language, localizedValue);
            return externalIdentifierType;
        }



        /// <summary>
        /// This method will create the ExternalIdentifierType for Patient Id
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="HL7PatientId"></param>
        /// <returns></returns>
        public ExternalIdentifierType GetPatientId(string documentId, string HL7PatientId)
        {
            ExternalIdentifierType oExtIdTypePat = CreateExternalIdentifier(
                    documentId,
                    CDAConstants.EBXML_RESPONSE_PATIENTID_IDENTIFICATION_SCHEME,
                    CDAConstants.EXTERNAL_OBJECT_IDENTIFIER_TYPE,
                    HL7PatientId,
                    CDAConstants.CHARACTER_SET,
                    CDAConstants.LANGUAGE_CODE_ENGLISH,
                    CDAConstants.PROVIDE_REGISTER_SLOT_NAME_PATIENT_ID);

            return oExtIdTypePat;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="ClassificationType"></param>
        /// <returns></returns>
        public ClassificationType GetCodeSystem(string documentId, EnumClassificationType ClassificationType,
            string code = "", string codeSystem = "", string displayName = "", string language = "")
        {
            ClassificationType classificationType = null;
            switch (ClassificationType)
            {
                case EnumClassificationType.ClassCode:
                    if (this.IsPolicyDocument)
                    {
                        classificationType = this.CreateClassification(CDAConstants.XDS_CLASS_CODE_SCHEMA_UUID,
                                                                        documentId, string.Empty,
                                                                        CDAConstants.METADATA_CLASS_CODE,
                                                                        CDAConstants.CLASSIFICATION_REGISTRY_OBJECT,
                                                                        CDAConstants.CLASSIFICATION_SCHEMA_CDNAME,
                                                                        CDAConstants.CODE_SYSTEM_LOINC_OID,
                                                                        CDAConstants.CHARACTER_SET,
                                                                        CDAConstants.LANGUAGE_CODE_ENGLISH,
                                                                        CDAConstants.METADATA_CLASS_CODE_DISPLAY_NAME);
                    }
                    else
                    {
                        classificationType = this.CreateClassification(CDAConstants.XDS_CLASS_CODE_SCHEMA_UUID,
                                                                        documentId,
                                                                        string.Empty,
                                                                        code,
                                                                        CDAConstants.CLASSIFICATION_REGISTRY_OBJECT,
                                                                        CDAConstants.CLASSIFICATION_SCHEMA_CDNAME,
                                                                        codeSystem,
                                                                        CDAConstants.CHARACTER_SET,
                                                                        language,                                                                        
                                                                        displayName);
                    }
                    break;
                case EnumClassificationType.FormatCode:
                    if (this.IsPolicyDocument)
                    {
                        classificationType = this.CreateClassification(CDAConstants.CLASSIFICATION_SCHEMA_IDENTIFIER_FORMAT_CODE,
                                                                          documentId,
                                                                          "",
                                                                           CDAConstants.METADATA_FORMAT_CODE_XACML,
                                                                          CDAConstants.CLASSIFICATION_REGISTRY_OBJECT,
                                                                          CDAConstants.CLASSIFICATION_SCHEMA_CDNAME,
                                                                          CDAConstants.METADATA_FORMAT_CODE_SYSTEM,
                                                                          CDAConstants.CHARACTER_SET,
                                                                          CDAConstants.LANGUAGE_CODE_ENGLISH,
                                                                          "");
                    }
                    else
                    {
                        classificationType = this.CreateClassification(CDAConstants.CLASSIFICATION_SCHEMA_IDENTIFIER_CDAR2,
                                                                      documentId,
                                                                      string.Empty,
                                                                      "CDAR2/IHE 1.0",
                                                                      CDAConstants.CLASSIFICATION_REGISTRY_OBJECT,
                                                                      CDAConstants.CLASSIFICATION_SCHEMA_CDNAME,
                                                                      "Connect-a-thon formatCodes",
                                                                      CDAConstants.CHARACTER_SET,
                                                                      CDAConstants.LANGUAGE_CODE_ENGLISH,
                                                                      "CDAR2/IHE 1.0");
                    }
                    break;
                case EnumClassificationType.ConfidentialityCode:

                    if (!string.IsNullOrEmpty(code))
                    {
                       switch (code.ToUpper())
                        {
                            case "N":
                                displayName = CDAConstants.CONFIDENTIALITY_CODE_NORMAL_DISPLAY_NAME;
                                break;
                            case "R":
                                displayName = CDAConstants.CONFIDENTIALITY_CODE_RESTRICTED_DISPLAY_NAME;
                                break;
                            case "V":
                                displayName = CDAConstants.CONFIDENTIALITY_CODE_VERY_RESTRICTED_DISPLAY_NAME;
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        code = CDAConstants.CONFIDENTIALITY_CODE_RESTRICTED;
                    }

                    if (string.IsNullOrEmpty(codeSystem))
                    {
                        codeSystem = CDAConstants.CONFIDENTIALITY_CODE_SYSTEM;
                    }

                    if (string.IsNullOrEmpty(displayName))
                    {
                        displayName = CDAConstants.CONFIDENTIALITY_CODE_RESTRICTED_DISPLAY_NAME;
                    }

                    if (string.IsNullOrEmpty(language))
                    {
                        language = CDAConstants.LANGUAGE_CODE_ENGLISH;
                    }
                    classificationType = this.CreateClassification(CDAConstants.PROVIDE_REGISTER_CONFIDENTIALITY_CODE_UUID,
                                                                        documentId,
                                                                        "",                                                                        
                                                                        code,
                                                                        CDAConstants.CLASSIFICATION_REGISTRY_OBJECT,
                                                                        CDAConstants.CLASSIFICATION_SCHEMA_CDNAME,
                                                                        CDAConstants.CONFIDENTIALITY_CODE_SYSTEM,
                                                                        CDAConstants.CHARACTER_SET,
                                                                        language,displayName);
           
                    break;

                case EnumClassificationType.FacilityType:
                    classificationType = this.CreateClassification(CDAConstants.PROVIDE_REGISTER_FACILITY_TYPE_UUID,
                                                                           documentId,
                                                                           "",
                                                                           code,
                                                                           CDAConstants.CLASSIFICATION_REGISTRY_OBJECT,
                                                                           CDAConstants.CLASSIFICATION_SCHEMA_CDNAME,
                                                                            codeSystem,
                                                                           CDAConstants.CHARACTER_SET,
                                                                           language, displayName);
                    break;
                case EnumClassificationType.PracticeSetting:
                    classificationType = this.CreateClassification(CDAConstants.PROVIDE_REGISTER_PRACTICE_SETTING_CD_UUID,
                                                                    documentId,
                                                                    string.Empty,
                                                                    "General Medicine",
                                                                    CDAConstants.CLASSIFICATION_REGISTRY_OBJECT,
                                                                    CDAConstants.CLASSIFICATION_SCHEMA_CDNAME,
                                                                    "Connect-a-thon practiceSettingCodes",
                                                                    CDAConstants.CHARACTER_SET,
                                                                    CDAConstants.LANGUAGE_CODE_ENGLISH,
                                                                    "General Medicine");

                    break;

                case EnumClassificationType.TypeCode:

                    if (this.IsPolicyDocument)
                    {
                        classificationType = this.CreateClassification(CDAConstants.CLASSIFICATION_SCHEMA_IDENTIFIER_TYPE_CODE,
                                                                      documentId, string.Empty,
                                                                      CDAConstants.METADATA_CLASS_CODE,
                                                                      CDAConstants.CLASSIFICATION_REGISTRY_OBJECT,
                                                                      CDAConstants.CLASSIFICATION_SCHEMA_CDNAME,
                                                                      CDAConstants.CODE_SYSTEM_LOINC_OID,
                                                                      CDAConstants.CHARACTER_SET,
                                                                      CDAConstants.LANGUAGE_CODE_ENGLISH,
                                                                      CDAConstants.METADATA_CLASS_CODE_DISPLAY_NAME);
                    }
                    else
                    {
                        classificationType = this.CreateClassification(CDAConstants.CLASSIFICATION_SCHEMA_IDENTIFIER_TYPE_CODE,
                                                                     documentId,
                                                                     string.Empty,
                                                                     code,
                                                                     CDAConstants.CLASSIFICATION_REGISTRY_OBJECT,
                                                                     CDAConstants.CLASSIFICATION_SCHEMA_CDNAME,
                                                                     codeSystem,
                                                                     CDAConstants.CHARACTER_SET,
                                                                     language,displayName);
                    }


                    break;

            }




            return classificationType;

        }

        /// <summary>
        /// This method will return Class Code
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public ClassificationType GetClassCode(string documentId)
        {
            return this.CreateClassification(
               CDAConstants.XDS_CLASS_CODE_SCHEMA_UUID,
                   documentId, string.Empty,
                   CDAConstants.METADATA_CLASS_CODE,
                   CDAConstants.CLASSIFICATION_REGISTRY_OBJECT,
                   CDAConstants.CLASSIFICATION_SCHEMA_CDNAME,
                   CDAConstants.CODE_SYSTEM_LOINC_OID,
                   CDAConstants.CHARACTER_SET,
                   CDAConstants.LANGUAGE_CODE_ENGLISH,
                   CDAConstants.METADATA_CLASS_CODE_DISPLAY_NAME);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="constants"></param>
        /// <param name="slotValueList"></param>
        /// <returns></returns>
        public SlotType1 GetSlotType(string constants, params string[] slotValueList)
        {
            SlotType1 SlotType = new SlotType1();
            SlotType.name = constants;
            SlotType.ValueList = new ValueListType();
            SlotType.ValueList.Value = slotValueList;
            return SlotType;
        }

        /// <summary>
        /// This method will create the ExternalIdentifierType object for Document Id
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public ExternalIdentifierType GetDocumentId(String documentId)
        {
            ExternalIdentifierType oExtIdTypePat = CreateExternalIdentifier(documentId,
                    CDAConstants.DOCUMENT_ID_IDENT_SCHEME,
                    CDAConstants.EXTERNAL_OBJECT_IDENTIFIER_TYPE,
                    documentId,
                    CDAConstants.CHARACTER_SET,
                    CDAConstants.LANGUAGE_CODE_ENGLISH,
                    CDAConstants.PROVIDE_REGISTER_SLOT_NAME_DOCUMENT_ID);

            return oExtIdTypePat;
        }


        /// <summary>
        /// This method will cerate the Author 
        /// </summary>
        /// <param name="docUniqueId"></param>
        /// <param name="author"></param>
        /// <returns></returns>
        /// Note In case of Policy file aurhor person and institution will be Auto Generated
        public ClassificationType GetAuthorClassCode(string docUniqueId, Author author)
        {
            ClassificationType classificationType =  new ClassificationType();            
            //Set Constant Value
            classificationType.classificationScheme = CDAConstants.CLASSIFICATION_SCHEMA_AUTHOR_IDENTIFIER;
            classificationType.id = this.NewGuid;

            List<SlotType1> authorSlots = new List<SlotType1>();


            //Set Author Person
            authorSlots.Add(this.GetSlotType("authorPerson", (IsPolicyDocument ? "Auto Generated" : author.Person)));
            //Set Author Institution
            authorSlots.Add(this.GetSlotType("authorInstitution", (IsPolicyDocument ? "Auto Generated" : author.Institution)));
            //Set Author Role
            authorSlots.Add(this.GetSlotType("authorRole", author.Role));
            //Set Author Specialty
            authorSlots.Add(this.GetSlotType("authorSpecialty", author.Specialty));


            classificationType.Slot = authorSlots.ToArray();
            return classificationType;

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


        /// <summary>
        /// This method will create the International string type 
        /// </summary>
        /// <param name="localizedCharSet"></param>
        /// <param name="language"></param>
        /// <param name="localizedValue"></param>
        /// <returns></returns>
        public InternationalStringType GetInternationalStringType(string localizedCharSet, string language, string localizedValue)
        {
            InternationalStringType internationalStringType = new InternationalStringType();
            LocalizedStringType localizedStringType = new LocalizedStringType();
            localizedStringType.charset = localizedCharSet;
            localizedStringType.lang = language;
            localizedStringType.value = localizedValue;
            internationalStringType.LocalizedString = new LocalizedStringType[] { localizedStringType };

            return internationalStringType;
        }



        private ExternalIdentifierType CreateExternalIdentifier(string registryObject, string identificationScheme, string objectType
            , string value, string localizedCharSet, string language, String localizedValue)
        {
            ExternalIdentifierType idType = new ExternalIdentifierType();// createExternalIdentifierType();
            idType.id = this.NewGuid;
            idType.registryObject = registryObject;
            idType.identificationScheme = identificationScheme;
            idType.objectType = objectType;
            idType.value = value;
            idType.Name = this.GetInternationalStringType(localizedCharSet, language, localizedValue);
            return idType;
        }


        private ClassificationType CreateClassification(string classificationScheme, string classifiedObject, string ctypeId,
            string nodeRepresentation, string objectType, string slotName, String slotValue,
           string localizedCharSet, string language, string localizedValue)
        {
            ClassificationType cType = new ClassificationType();
            cType.classificationScheme = classificationScheme;
            cType.classifiedObject = classifiedObject;
            cType.nodeRepresentation = nodeRepresentation;
            cType.id = string.IsNullOrEmpty(ctypeId) ? this.NewGuid : ctypeId;

            cType.objectType = objectType;
            //Create Slot for classification 
            cType.Slot = new SlotType1[] { this.GetSlotType(slotName, slotValue) };
            //Create Name for classification
            cType.Name = this.GetInternationalStringType(localizedCharSet, language, localizedValue);

            return cType;
        }




    }
}
