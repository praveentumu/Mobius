
using System.ComponentModel;
namespace Mobius.CoreLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public enum DocumentSource
    {
        Local,
        Remote,
        LocalGateway,
    }

    public enum ErrorCode
    {
        ErrorSuccess = 0x00000000,
        FileNotFound = 0x00000001,
        RecordNotFound = 0x00000002,
        AccessDenied = 0x00000011,
        InvaidLoginCredential = 0x000000023,
        UnknownException = 0x000000027,
        NODOCUMENTFOUND = 0x000000053,

        //Validation error code
        SearchPatient_NHINCommunities_Missing = 30000,
        Document_PatientId_Missing = 30001,
        Document_communityId_Missing = 30002,
        Document_DocumentId_Missing = 30003,
        Document_Subject_Missing = 30018,
        Document_Byte_Missing = 30019,
        RepositoryId_Missing = 30020,
        No_Result_Found = 30021,


        Document_SourceRepositoryId_Missing = 30004,
        FacilityId_Missing = 30005,
        C32Sections_Missing = 30006,

        ErrorOccurredInC32Document_Parsing = 30008,
        Patient_Consent_Missing = 30009,
        Patient_Consent_Deviated = 30011,

        //RetrieveDocument related codes
        Document_Purpose_Missing = 30012,
        Invalid_Communities = 300013,
        SearchPatient_Invalid_Demographics = 300014,
        SearchPatient_Invalid_Demographics_DOB = 300015,
        SearchPatient_Invalid_Demographics_GivenName = 300016,
        SearchPatient_Invalid_Demographics_FamilyName = 300017,
        SearchPatient_Invalid_Demographics_Gender = 300018,
        SearchPatient_Missing_Demographics_MPIID = 300019,
        Provider_Doesnot_Have_Permission_ToView = 300020,
        SearchPatient_Multiple_Record_Found = 300021,
        MothersMaidenNameRequested = 300022,
        PatientAdressRequested = 300023,
        PatientTelecomRequested = 300024,


        //GateWay - Response_Error
        SearchPatient_GateWayResponse_Error = 50000,
        Document_GateWayResponse_Error = 50001,

        //Provider/Facility Management
        Provider_First_Name_Missing = 6000,
        Provider_Last_Name_Missing = 6001,
        Provider_Email_Address_Missing = 6002,
        Provider_Organization_Name_Missing = 6003,
        Provider_Invalid_Email = 6004,
        Provider_Type = 6005,
        Provider_MedicalRecordsDeliveryEmailAddress_Missing = 6006,
        Name_Contains_Specialcharacater = 6007,
        Invalid_Phone_Format = 6008,
        Provider_Registration_Failed = 6009,
        Invalid_Postal_Code = 6010,
        Invalid_URI = 6011,
        Provider_Registration_successful = 6012,
        ZipCode_Missing = 6013,
        Invalid_SSN = 6014,
        Provider_Already_Exist = 6015,
        MothersMaidenName_Contains_Specialcharacater = 6016,
        Address_Has_SpecialCharacater = 6017,
        Password_Missing = 6018,
        Invalid_Password = 6019,
        Old_Password_Missing = 6020,
        New_Password_Missing = 6021,
        Incorrect_Old_Password = 6022,
        OldPassword_And_NewPassword_Equal = 6023,
        UserId_Missing = 6024,
        Password_Change_Successfull = 6025,
        Provider_Details_Not_Found = 6026,
        //Patient Referral
        PatientReferral_Document_Missing = 7000,
        PatientReferral_Patient_Information_Missing = 7001,
        PatientReferral_Some_Of_Patient_Information_Missing = 7002,
        //PatientReferral_Accomplished_Date_Missing = 7003,
        PatientReferral_InValid_ReferralEmailAddress = 7004,
        PatientReferral_Failed = 7005,
        PatientReferral_ReferralOn = 7006,
        ReferralAccomplishedDate_GreaterThan_ReferralDate = 7007,
        Sections_Missing_In_Referral_Document = 7008,
        PatientReferral_Appointment_Date = 7009,
        PatientReferral_Referral_OutcomeDocumentID_Missing = 7010,
        Password_Reset_Failed = 7011,
        Patient_Referred_Successfully = 7012,
        Invalid_Response_Data = 7013,
        Provider_Cannot_Make_A_Referral_To_Himself = 7014,
        Acknowledgement_Sent_Successfully = 7015,
        Referral_Outcome_Document_Missing = 7016,
        Referral_Completed = 7017,
        Accomplish_Date_Should_Not_Be_Less_Than_Today = 7018,
        Appointment_Date_Should_Not_Be_Less_Than_Today = 7019,

        //patient

        patient_Registration_successful = 8000,
        patient_Consent_Already_Exists = 8001,
        patient_MPIID_Missing = 8002,
        patient_Consent_ID = 8003,
        Role_ID = 8004,
        PurposeOf_UseId_Missing = 8005,
        RuleStart_Date_Missing = 8006,
        RuleEnd_Date_Missing = 8007,
        Gender_Not_Provided = 8008,
        DOB_Not_Provided = 8009,
        Patient_Updated = 8010,
        Patient_Details_Not_Found = 8011,
        Patient_Consent_Does_Not_Exist = 8012,
        patient_Allready_Exist = 8013,
        Patient_First_Name_Missing = 8014,
        City_Is_Missing = 8015,
        Country_Is_Missing = 8017,
        One_Address_Is_Required = 8018,
        Invalid_Address = 8019,
        Invalid_Email_Address = 8020,
        DOB_Can_Not_Be_Greater_Than_Current = 8021,
        Invalid_Date_Format = 8022,
        Patient_Last_Name_Missing = 8023,
        Invalid_Extension_Format = 8024,
        CONSENT_DIRECTIVE_ALREADY_MAPPED = 8025,
        PERMISSION_CATEGORY_SUCCESSFULLY_CREATED = 8026,
        PERMISSION_CATEGORY_SUCCESSFULLY_UPDATED = 8027,
        CONSENT_FAILED = 8028,
        Invalid_RemotePatientID = 8029,
        Invalid_RemoteCommunityId = 8030,
        Invalid_Validation_SpecificationId = 8031,
        C32Document_Validation_Failed = 8032,


        //CertificateAuthority Error Codes
        CertificateAuthority_Enrollment_Pending = 11001,
        CertificateAuthority_Enrollment_Fail = 11002,
        CertificateAuthority_CSR_Missing = 11003,

        CertificateAuthority_Validate_Certificate_Missing = 11006,
        CertificateAuthority_Validate_CAIssuerName_Mismatch = 11007,
        CertificateAuthority_Validate_Certificate_Expired = 11008,
        Not_Authenticated_Certificate_ = 11009,
        Not_Valid_Certificate = 11010,
        Not_Valid_Request_Data = 11011,
        Not_Valid_Response_Data = 11012,
        PFX_Certificate_Not_Found = 11013,
        Certificate_Export_Failed = 11014,
        Successful_Export_Certificate_Message = 11015,

        EmailAddress_Missing = 12012,
        Invalid_UserType = 12013,
        CheckEmailForNewPassword = 12014,//registered 
        Invalid_UserId = 12015,
        LoginFail = 12016,
        //Community 
        MissingCommunityId = 13001,
        InvalidPatientId = 13002,
        InvaidPatientIdOrCommunityId = 13003,
        MissingPatientId = 13004,
        PatientCorrelationError = 13005,
        Inactive_Account = 13006,
        Certificate_Data_Missing = 13007,
        Activation_In_Process = 13008,
        MissingParameters = 13009,
        DocumentSubmissionFailed = 13010,
        PolicySubmissionFailed = 13011,

        //Manage NHIN community Error Codes
        Invalid_File_Format = 14001,
        Duplicate_Entry = 14002,
        Record_Successfully_Updated = 14003,
        HomeCommunity_Deletion_Failed = 14004,
        All_Exist_Records_Not_Imported = 14005,
        Some_Exist_Records_Not_Imported = 14006,
        Records_Imported_Successfully = 14007,
        Community_Identifier_Missing = 14008,
        Community_Description_Missing = 14009,
        Select_Community_to_Import = 14010,
        Select_Valid_Xls_File = 14011,
        Select_Valid_Csv_File = 14012,
        Select_Valid_File = 14013,

        //CDA Helper Method Errors 
        CDAHelper_NO_SECTION_FOUND = 15001, //Unable to load C32 Section or there is no section available in document.
        CDAHELPER_INVALID_DOCUMENT = 15002, //Invalid C32 Document.
        Assertion_Object_Invalid = 16001,
        Assertion_Required_Information_Missing = 16002,

        //UpgradeAccount or User Errors
        Account_Upgradation_Failed = 17001,
        Account_Upgraded_Successfully = 17002,
        Account_Upgradation_Error_Message = 17003,
        Certificate_Deletion_Failed = 17004,
        Account_Expired = 17005,


        // Emergency Overriden 
        Select_Record_To_Close = 18001,
        No_Record_To_Display = 18002,
        Error_in_emergency_access = 18003,
        Emergency_Override_Case = 18004,
        Select_Pupose_As_Emergency = 18005,
        Error_in_updating_audit_status = 18006
    }


    public enum Status
    {
        Active = 1,
        Inactive = 2,
        Retired = 3,
        Deceased = 4

    }

    /// <summary>
    /// Provider type enums
    /// </summary>
    /// Modified enums as per provider type values in table
    public enum ProviderType
    {
        Hospitals = 1,
        HIEs = 2,
        Associations = 3,
        IDNs = 4,
        Labs = 5,
        Clinics = 6,
        Departments = 7,
        Pharmacies = 8,
        Practice = 9,
        Audiologist = 10,
        Dental_Hygienist = 11,
        Dentist = 12,
        Dietitian = 13,
        Complementary_Healthcare_worker = 14,
        Professional_nurse = 15,
        Optometrist = 16,
        Pharmacist = 17,
        Chiropractor = 18,
        Osteopath = 19,
        Medical_doctor = 20,
        Medical_pathologist = 21,
        Podiatrist = 22,
        Psychiatrist = 23,
        Medical_Assistant = 24,
        Psychologist = 25,
        Social_worker = 26,
        Speech_therapist = 27,
        Medical_Technician = 28,
        Orthotist = 29,
        Physiotherapist_AND_OR_occupational_therapist = 30,
        Veterinarian = 31,
        Paramedic_EMT = 32,
        Philologist_translator_AND_OR_interpreter = 33,
        clerical_occupation = 34,
        Administrative_healthcare_staff = 35,
        Infection_control_nurse = 36,
        insurance_specialist_health_insurance_payor = 37,
        Profession_allied_to_medicine_non_licensed_care_giver = 38,
        Public_health_officer = 39
    }

    public enum Gender
    {
        Male = 1,
        Female = 2,
        Unspecified = 0
    }


    public enum MasterCollection
    {
        UserRole = 1,
        PurposeOfUse = 2,
        FacilityInfo = 3,
        Country = 4,
        State = 5,
        City = 6,
        Language = 7,
        Specialty = 8,
        OrganizationType = 9,
        Provider_IndividualType = 10,
        Status = 11,
        EmergencyReason = 12

    }

    public enum ActionType
    {
        Insert,
        Delete,
        Update,
        NoChange
    }
    public enum AssertionMode
    {
        Default,
        Custom
    }


    public enum UserRole
    {
        [Description("Audiologist")]
        Audiologist = 309418004,

        [Description("Dental Hygienist")]
        Dental_Hygienist = 26042002,

        [Description("Dentist")]
        Dentist = 106289002,

        [Description("Dietitian")]
        Dietitian = 159033005,

        [Description("Complementary Healthcare worker")]
        Complementary_Healthcare_worker = 224609002,

        [Description("Professional nurse")]
        Professional_nurse = 106292003,

        [Description("Optometrist")]
        Optometrist = 28229004,

        [Description("Pharmacist")]
        Pharmacist = 46255001,

        [Description("Chiropractor")]
        Chiropractor = 3842006,

        [Description("Osteopath")]
        Osteopath = 76231001,

        [Description("Medical doctor")]
        Medical_doctor = 112247003,

        [Description("Medical pathologist ")]
        Medical_pathologist = 61207006,

        [Description("Podiatrist")]
        Podiatrist = 159034004,

        [Description("Psychiatrist")]
        Psychiatrist = 80584001,

        [Description("Medical Assistant")]
        Medical_Assistant = 22515006,

        [Description("Psychologist")]
        Psychologist = 59944000,

        [Description("Social worker")]
        Social_worker = 106328005,

        [Description("Speech therapist")]
        Speech_therapist = 159026005,

        [Description("Medical Technician")]
        Medical_Technician = 307988006,

        [Description("Orthotist")]
        Orthotist = 309428008,

        [Description("Physiotherapist AND/OR occupational therapist")]
        Physiotherapist_OR_occupational_therapist = 106296000,

        [Description("Veterinarian")]
        Veterinarian = 106290006,

        [Description("Paramedic/EMT")]
        Paramedic_EMT = 397897005,

        [Description("Minister of religion AND/OR related member of religious order")]
        Minister_of_religion_OR_related_member_of_religious_order = 106311007,

        [Description("Philologist, translator AND/OR interpreter")]
        Philologist_translator_OR_interpreter = 106330007,

        [Description("clerical occupation")]
        clerical_occupation = 159483005,

        [Description("Administrative healthcare staff")]
        Administrative_healthcare_staff = 224608005,

        [Description("Infection control nurse")]
        Infection_control_nurse = 224546007,

        [Description("insurance specialist (health insurance/payor)")]
        insurance_specialist = 307785004,

        [Description("Patient")]
        Patient = 116154003,

        [Description("Patient advocate")]
        Patient_advocate = 429577009,

        [Description("Profession allied to medicine (non-licensed care giver)")]
        Profession_allied_to_medicine = 309398001,

        [Description("IT Professional")]
        IT_Professional = 265950004,

        [Description("law occupation")]
        law_occupation = 271554005,

        [Description("Public health officer")]
        Public_health_officer = 307969004,

        [Description("Hospitals")]
        Hospitals = 1,

        [Description("HIEs")]
        HIEs = 2,

        [Description("Associations")]
        Associations = 3,

        [Description("IDNs")]
        IDNs = 4,

        [Description("Labs")]
        Labs = 5,

        [Description("Clinics")]
        Clinics = 6,

        [Description("Departments")]
        Departments = 7,

        [Description("Pharmacies")]
        Pharmacies = 8,

        [Description("Practice")]
        Practice = 9,
    }

    public enum UserType
    {
        Patient = 1,
        Provider = 2,
        Unspecified = 0
    }

    public enum AddressStatus
    {
        Primary = 1,
        Secondary = 2,
        Inactive = 3

    }


    public enum OverrideReason
    {
        [Description("UnSpecified")]
        UNSPECIFIED = 0,
        [Description("Treatment")]
        TREATMENT = 1,
        [Description("Payment")]
        PAYMENT = 2,
        [Description("Healthcare Operations")]
        OPERATIONS = 3,
        [Description("System Administration")]
        SYSADMIN = 4,
        [Description("Fraud detection")]
        FRAUD = 5,
        [Description("Use or disclosure of Psychotherapy Notes")]
        PSYCHOTHERAPY = 6,
        [Description("Use or disclosure by the covered entity for its own training programs")]
        TRAINING = 7,
        [Description("Use or disclosure by the covered entity to defend itself in a legal action")]
        LEGAL = 8,
        [Description("Marketing")]
        MARKETING = 9,
        [Description("Use and disclosure for facility directories")]
        DIRECTORY = 10,
        [Description("Disclose to a family member, other relative, or a close personal friend of the individual")]
        FAMILY = 11,
        [Description("Uses and disclosures with the individual present.")]
        PRESENT = 12,
        [Description("Permission cannot practicably be provided because of the individual's incapacity or an emergency")]
        EMERGENCY = 13,
        [Description("Use and disclosures for disaster relief purposes.")]
        DISASTER = 14,
        [Description("Uses and disclosures for public health activities.")]
        PUBLICHEALTH = 15,
        [Description("Disclosures about victims of abuse, neglect or domestic violence.")]
        ABUSE = 16,
        [Description("Uses and disclosures for health oversight activities.")]
        OVERSIGHT = 17,
        [Description("Disclosures for judicial and administrative proceedings.")]
        JUDICIAL = 18,
        [Description("Disclosures for law enforcement purposes.")]
        LAW = 19,
        [Description("Uses and disclosures about decedents.")]
        DECEASED = 20,
        [Description("Uses and disclosures for cadaveric organ, eye or tissue donation purposes")]
        DONATION = 21,
        [Description("Uses and disclosures for research purposes.")]
        RESEARCH = 22,
        [Description("Uses and disclosures to avert a serious threat to health or safety.")]
        THREAT = 23,
        [Description("Uses and disclosures for specialized government functions.")]
        GOVERNMENT = 24,
        [Description("Disclosures for workers' compensation.")]
        WORKERSCOMP = 25,
        [Description("Disclosures for insurance or disability coverage determination")]
        COVERAGE = 26,
        [Description("Request of the Individual")]
        REQUEST = 27,
    }


    public enum PurposeOfUse
    {
        [Description("Treatment")]
        TREATMENT = 1,
        [Description("Payment")]
        PAYMENT = 2,
        [Description("Healthcare Operations")]
        OPERATIONS = 3,
        [Description("System Administration")]
        SYSADMIN = 4,
        [Description("Fraud detection")]
        FRAUD = 5,
        [Description("Use or disclosure of Psychotherapy Notes")]
        PSYCHOTHERAPY = 6,
        [Description("Use or disclosure by the covered entity for its own training programs")]
        TRAINING = 7,
        [Description("Use or disclosure by the covered entity to defend itself in a legal action")]
        LEGAL = 8,
        [Description("Marketing")]
        MARKETING = 9,
        [Description("Use and disclosure for facility directories")]
        DIRECTORY = 10,
        [Description("Disclose to a family member, other relative, or a close personal friend of the individual")]
        FAMILY = 11,
        [Description("Uses and disclosures with the individual present.")]
        PRESENT = 12,
        [Description("Permission cannot practicably be provided because of the individual's incapacity or an emergency")]
        EMERGENCY = 13,
        [Description("Use and disclosures for disaster relief purposes.")]
        DISASTER = 14,
        [Description("Uses and disclosures for public health activities.")]
        PUBLICHEALTH = 15,
        [Description("Disclosures about victims of abuse, neglect or domestic violence.")]
        ABUSE = 16,
        [Description("Uses and disclosures for health oversight activities.")]
        OVERSIGHT = 17,
        [Description("Disclosures for judicial and administrative proceedings.")]
        JUDICIAL = 18,
        [Description("Disclosures for law enforcement purposes.")]
        LAW = 19,
        [Description("Uses and disclosures about decedents.")]
        DECEASED = 20,
        [Description("Uses and disclosures for cadaveric organ, eye or tissue donation purposes")]
        DONATION = 21,
        [Description("Uses and disclosures for research purposes.")]
        RESEARCH = 22,
        [Description("Uses and disclosures to avert a serious threat to health or safety.")]
        THREAT = 23,
        [Description("Uses and disclosures for specialized government functions.")]
        GOVERNMENT = 24,
        [Description("Disclosures for workers' compensation.")]
        WORKERSCOMP = 25,
        [Description("Disclosures for insurance or disability coverage determination")]
        COVERAGE = 26,
        [Description("Request of the Individual")]
        REQUEST = 27
    }

    public enum NISTValidationType
    {
        ERRORS,
        WARNING,
        ALL
    }

    /// <summary>
    /// 
    /// </summary>
    public enum EmergencyRecords
    {
        All=1,
        Open=2,
        Close=3
    }

}

