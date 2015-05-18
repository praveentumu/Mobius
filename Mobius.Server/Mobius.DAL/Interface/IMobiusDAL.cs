using System.Data;
using Mobius.CoreLibrary;
using Mobius.Entity;
using System.Collections.Generic;
using System.Data.Common;

namespace Mobius.DAL
{
    internal interface IMobiusDAL
    {
        /// <summary>
        /// GetDocumentMetaData
        /// </summary>
        /// <param name="DocumentID"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        Result GetDocumentMetaData(string DocumentID, out MobiusDocument document);

        /// <summary>
        /// GetFacilityName
        /// </summary>
        /// <param name="facilityId"></param>
        /// <returns></returns>
        string GetFacilityName(int facilityId);


        /// <summary>
        /// SaveC32DocumentMetaData
        /// </summary>
        /// <param name="documentMetadata"></param>
        /// <returns></returns>
        bool SaveC32DocumentMetaData(DocumentMetadata documentMetadata);

        /// <summary>
        /// UpdateDocumentMetadata
        /// </summary>
        /// <param name="DocumentID"></param>
        /// <param name="Location"></param>
        /// <returns></returns>
        bool UpdateDocumentMetadata(string DocumentID, string Location);


        /// <summary>
        /// HasAccessPermission
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="subject"></param>
        /// <param name="purposeOfUse"></param>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        Result HasAccessPermission(DocumentRequest doc, out int categoryID);

        /// <summary>
        /// SaveDocumentMetadata
        /// </summary>
        /// <param name="documentMetadata"></param>
        /// <returns></returns>
        bool SaveDocumentMetadata(DocumentMetadata documentMetadata);

        /// <summary>
        /// SaveSharingDetails
        /// </summary>
        /// <param name="originalDocumentID"></param>
        /// <param name="ruleStartDate"></param>
        /// <param name="ruleEndDate"></param>
        /// <param name="lstSubject"></param>
        /// <param name="purposeOfUse"></param>
        /// <returns></returns>
        bool SaveSharingDetails(string originalDocumentID, string ruleStartDate, string ruleEndDate, List<string> lstSubject, string purposeOfUse);

        /// <summary>
        /// To retrieve the list of all modules for share document purpose.
        /// </summary>
        /// <param name="c32Sections"></param>
        /// <returns></returns>
        Result GetC32Sections(out List<C32Section> c32Sections);
        Result GetC32Sections_TODO(out List<C32Section> c32Sections);

        /// <summary>
        /// GetRole
        /// </summary>
        /// <param name="userRole"></param>
        /// <returns></returns>
        string GetRole(int userRole);

        /// <summary>
        /// insert provider details in to provider table of database
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        Result AddProvider(Provider provider);

        /// <summary>
        /// insert patient details in to Patient table of database
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        Result AddPatient(Patient patient);


        /// <summary>
        /// Get PCKS Response
        /// </summary>
        /// <param name="oneTimePassword"></param>
        /// <param name="pCKS7"></param>
        /// <returns></returns>
        Result GetPCKSResponse(string oneTimePassword, out string pCKS7);

        /// <summary>
        /// Delete OTP 
        /// </summary>
        /// <param name="oneTimePassword"></param>
        void DeleteOTP(string oneTimePassword);

        /// <summary>
        /// Delete C32DocumentMetaData
        /// </summary>
        /// <param name="originalDocumentID"></param>
        void DeleteC32DocumentMetaData(string originalDocumentID);

        /// <summary>
        /// CreatePatientReferral
        /// </summary>
        /// <param name="patientReferred"></param>
        /// <param name="referPatientId"></param>
        /// <returns></returns>
        Result CreatePatientReferral(PatientReferral patientReferred, out int referPatientId);

        /// <summary>
        /// GetPatientReferralDetails
        /// </summary>
        /// <param name="patientReferralId"></param>
        /// <param name="emailAddress"></param>
        /// <param name="patientReferred"></param>
        /// <returns></returns>
        Result GetPatientReferralDetails(int patientReferralId, string referredToEmailAddress, string referredByEmailAddress, out  List<PatientReferral> patientReferred);


        /// <summary>
        /// AcknowledgePatientReferral
        /// </summary>
        /// <param name="patientReferred"></param>
        /// <param name="referPatientId"></param>
        /// <returns></returns>
        Result AcknowledgePatientReferral(PatientReferral patientReferred, out int referPatientId);

        /// <summary>
        /// GetMasterData
        /// </summary>
        /// <param name="masterCollection"></param>
        /// <param name="dependentValue"></param>
        /// <param name="masterDataCollection"></param>
        /// <returns></returns>
        Result GetMasterData(MasterCollection masterCollection, int dependedValue, out List<MasterData> masterDataCollection);

        /// <summary>
        /// Get Patient Details via MPIID
        /// </summary>
        /// <param name="patient"></param>
        /// <param name="MPIID"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Result GetPatientDetails(out Mobius.Entity.Patient patient, string MPIID = "", string email = "");

        /// <summary>
        /// Get City,State,Country via zip code
        /// </summary>
        /// <param name="ZipCode">string type</param>
        /// <param name="City">City class object</param>
        /// <returns>result class object</returns>
        Result GetLocalityByZipCode(string ZipCode, out City city);

        /// <summary>
        /// update patient details into Patient table of database
        /// </summary>
        /// <param name="patientEntity"></param>
        /// <returns></returns>
        Result UpdatePatient(Patient patientEntity);

        /// <summary>
        /// GetPatientConsent
        /// </summary>
        /// <param name="MPIID"></param>
        /// <param name="dsPatientConsent"></param>
        /// <returns></returns>
        Result GetPatientConsent(string MPIID, out List<MobiusPatientConsent> PatientConsents);

        /// <summary>
        /// Check Patient Consent Existence in Database.
        /// </summary>
        /// <param name="patientConsent"></param>
        /// <returns>return Result class object</returns>
        Result CheckPatientConsentPolicyExistence(MobiusPatientConsent patientConsent);

        /// <summary>
        /// Delete Patient Consent
        /// </summary>
        /// <param name="patientConsentId"></param>
        /// <returns>return Result class object</returns>
        Result DeletePatientConsent(string patientConsentId);

        /// <summary>
        /// Update OptIn and OptOut Status in dataBase
        /// </summary>
        /// <param name="MPIID"></param>
        /// <param name="isOptIn"></param>
        /// <returns>return Result class object</returns>
        Result UpdateOptInStatus(string MPIID, bool isOptIn);

        /// <summary>
        /// Search Patient behalf on demographics details.
        /// </summary>
        /// <param name="demographics"></param>
        /// <param name="patients"></param>
        /// <returns>return Result class object</returns>
        Result SearchPatient(Demographics demographics, out List<Patient> patients);

        /// <summary>
        /// GetSpecificPatientConsent
        /// </summary>
        /// <param name="MPIID"></param>
        /// <param name="PatientConsentId"></param>
        /// <param name="dsSpecificPatientConsent"></param>
        /// <returns>return Result class object</returns>
        //Result GetSpecificPatientConsent(string MPIID, int PatientConsentId, out DataSet dsSpecificPatientConsent);


        /// <summary>
        /// GetPatientConsentByConsentId
        /// </summary>
        /// <param name="MPIID"></param>
        /// <param name="patientConsentID"></param>
        /// <param name="PatientConsent"></param>
        /// <returns>return Result class object</returns>
        //Result GetPatientConsentByConsentId(string MPIID, int patientConsentID, out List<MobiusPatientConsent> PatientConsent);
        Result GetPatientConsentByConsentId(string MPIID, int patientConsentID, out MobiusPatientConsent PatientConsent);

        /// <summary>
        /// Add,Update and Delete Patient Consent
        /// </summary>
        /// <param name="patientConsentPolicy"></param>
        /// <returns>return Result class object</returns>
        Result UpdatePatientConsentPolicy(MobiusPatientConsent patientConsentPolicy);

        /// <summary>
        /// GetOptInStatus
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="status"></param>
        /// <returns>return Result class object</returns>
        Result GetOptInStatus(string patientId, out bool status);

        /// <summary>
        /// AuthenticateUser
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="password"></param>
        /// <param name="userType"></param>
        /// <param name="serailNumber"></param>
        /// <param name="Name"></param>
        /// <returns>return Result class object</returns>
        Result AuthenticateUser(string emailAddress, string password, int userType, out string serailNumber, out string Name);

        /// <summary>
        /// GetSerialNumber
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="userType"></param>
        /// <param name="serailNumber"></param>
        /// <returns></returns>
        Result GetSerialNumber(string emailAddress, int userType, out string serailNumber);

        /// <summary>
        /// Get Patient Information via DocumentID
        /// </summary>
        /// <param name="DocumentID"></param>
        /// <param name="Patients"></param>
        /// <returns></returns>
        Result GetPatientInformationByDocumentID(string DocumentID, out Patient Patients);

        /// <summary>
        ///  Add PFXCertificate into data base Patient and provider table
        /// </summary>
        /// <param name="pFXCertificate"></param>
        /// <returns></returns>
        Result AddPFXCertificate(PFXCertificate pFXCertificate);

        /// <summary>
        ///  Get PFXCertificate into data base Patient and provider table
        /// </summary>
        /// <param name="pFXCertificate"></param>
        /// <returns></returns>
        Result GetPFXCertificate(ref PFXCertificate pFXCertificate);

        /// <summary>
        /// Get Provider Details
        /// </summary>
        /// <param name="email"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        Result GetProviderDetails(string email, out Mobius.Entity.Provider provider);

        /// <summary>
        /// ActivateUser
        /// </summary>
        /// <param name="email"></param>
        /// <param name="serialNumber"></param>
        /// <param name="publicKey"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        Result ActivateUser(string email, string serialNumber, string publicKey, UserType userType,string CertificateCreationTime,string CertificationExpirationTime);

        /// <summary>
        /// Verify Patient available in to Patient table of database or not
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        Result CheckPatient(Patient patient);

        /// <summary>
        /// To get All the instance of Emergency Audit
        /// </summary>
        /// <param name="isShowAll"></param>
        /// <param name="lstEmergencyAudit"></param>
        /// <param name="patientId"></param>
        /// <returns></returns>
        Result GetAllEmergencyAudit(EmergencyRecords emergencyRecords, out List<EmergencyAudit> lstEmergencyAudit, string patientId);

        /// <summary>
        /// Get the emergency Details by id
        /// </summary>
        /// <param name="AuditID"></param>
        /// <param name="emergencyAudit"></param>
        /// <returns></returns>
        Result GetEmergencyDetailById(int auditID, out EmergencyAudit emergencyAudit);

      /// <summary>
      /// 
      /// </summary>
      /// <param name="documentRequest"></param>
      /// <returns></returns>
        Result CheckEmergencyAudit(DocumentRequest documentRequest);

        #region Admin Methods
        /// <summary>
        /// Get UserDetials into data base Patient and provider table
        /// </summary>
        /// <param name="pFXCertificate"></param>
        /// <returns></returns>
        Result GetUserDetials(ref List<UserDetails> userDetails);

        /// <summary>
        /// GetAdminDetails
        /// </summary>
        /// <param name="adminDetail"></param>
        /// <param name="adminDetails"></param>
        /// <returns></returns>
        Result GetAdminDetails(AdminDetails adminDetail, out List<AdminDetails> adminDetails);


        /// <summary>
        /// Update Admin Details
        /// </summary>
        /// <param name="adminDetails"></param>
        /// <returns></returns>
        Result UpdateAdminDetails(AdminDetails adminDetails);

        /// <summary>
        /// Add Admin details
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Result AddAdminDetails(string email, string password = "");

        /// <summary>
        /// To update the status of emergency audit instances
        /// </summary>
        /// <param name="lstAuditID"></param>
        /// <param name="isAuditStatus"></param>
        /// <returns></returns>
        Result UpdateOverrideDetails(List<int> lstAuditID, bool isAuditStatus);

        #endregion
    }
}

