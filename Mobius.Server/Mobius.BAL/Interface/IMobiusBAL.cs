namespace Mobius.BAL.Interface
{
    #region namespace
    using System.Collections.Generic;
    using System.Data;
    using Mobius.CoreLibrary;
    using Mobius.Entity;
    using PolicyEngine;
    #endregion

    /// <summary>
    /// IMobiusBAL interface
    /// </summary>
    public interface IMobiusBAL
    {
        /// <summary>
        /// Search patient 
        /// </summary>
        /// <param name="demographics">demographics class object</param>
        /// <param name="NHINCommunities">collection of NHINcommunity class object</param>
        /// <param name="patients">patient class object</param>
        /// <returns>return Result class object</returns>
        Result SearchPatient(Demographics demographics, List<MobiusNHINCommunity> NHINCommunities,MobiusAssertion mobiusAssertion ,out List<Patient> patients);

        /// <summary>
        /// Get Document Metadata
        /// </summary>
        /// <param name="patientId">string</param>
        /// <param name="NHINCommunities">collection of NHINcommunity class object</param>
        /// <param name="getLocalDocument">Load document from local database/Call gateway</param>
        /// <param name="documents">return Result class object</param>
        /// <returns></returns>                
        Result GetDocumentMetadata(string patientId, List<MobiusNHINCommunity> NHINCommunities, MobiusAssertion mobiusAssertion, bool getLocalDocument, out List<MobiusDocument> documents);

        /// <summary>
        /// get document
        /// </summary>
        /// <param name="patientId">string</param>
        /// <param name="documentId">string</param>
        /// <param name="purpose">string</param>
        /// <param name="subject">string</param>
        /// <param name="document">document class object</param>
        /// <returns>return Result class object</returns>
        Result GetDocument(DocumentRequest doc, out MobiusDocument document);

      
        /// <summary>
        /// Get Document MetaData
        /// </summary>
        /// <param name="DocumentID">DocumentID</param>
        /// <param name="document">document class object</param>
        /// <returns>return Result class object</returns>
        Result GetDocumentMetaData(string documentID, out MobiusDocument document);

        /// <summary>
        /// Get Facility yName
        /// </summary>
        /// <param name="token">string</param>
        /// <param name="facilityId">int</param>
        /// <param name="errorCode">ErrorCode class object</param>
        /// <returns>return facility name</returns>
        string GetFacilityName(string token, int facilityId, out ErrorCode errorCode);

        /// <summary>
        /// Upload Document
        /// </summary>
        /// <param name="communityId">string</param>
        /// <param name="documentId">string</param>
        /// <param name="documentBytes">byte array</param>
        /// <param name="XACMLBytes">byte array</param>
        /// <param name="patientId">string</param>
        /// <param name="uploadedBy">string</param>
        /// <param name="repositoryId">string</param>
        /// <param name="facilityId">string</param>
        /// <returns>return Result</returns>
        Result UploadDocument(
            string communityId,
            string documentId,
            byte[] documentBytes,
            byte[] bytesXACML,
            string patientId,
            string uploadedBy,
            string repositoryId,
            bool SubmitOnGateway, MobiusAssertion mobiusAssertion);

        /// <summary>
        /// Share patient Document on NHIN
        /// </summary>
        /// <param name="docByteData">byte array</param>
        /// <param name="XACMLbyteData">byte array></param>
        /// <param name="patientId">string</param>
        /// <param name="subject">string</param>
        /// <param name="homeCommunityId">string</param>
        /// <param name="sourceRepositryId">string</param>
        /// <param name="facilityId">string</param>
        /// <returns>return Result class object</returns>
        Result ShareDocument(ShareDocument shareDocument);

        /// <summary>
        /// Get C32 Sections
        /// </summary>
        /// <param name="c32Sections"></param>
        /// <returns>return Result class object</returns>
        Result GetC32Sections(out List<C32Section> c32Sections);
        Result GetC32Sections_TODO(out List<C32Section> c32Sections);
        /// <summary>
        /// Add new Provider
        /// </summary>
        /// <param name="provider">provider class object</param>
        /// <returns>return Result class object</returns>
        Result AddProvider(Mobius.Entity.Provider provider, out string PKCS7Response);

        /// <summary>
        /// CreateReferral
        /// </summary>
        /// <param name="patientReferred">CreatePatientReferral class object</param>
        /// <returns>return Result class object</returns>
        Result CreateReferral(CreatePatientReferral createPatientReferral);

        /// <summary>
        ///  AcknowledgeReferral
        /// </summary>
        /// <param name="acceptReferral">AcceptReferral class object</param>
        /// <returns>return Result class object</returns>
        Result AcknowledgeReferral(AcceptReferral acceptReferral);

        /// <summary>
        /// CompletePatientReferral
        /// </summary>
        /// <param name="patientReferralCompleted">PatientReferralCompleted class object</param>
        /// <returns>return Result class object</returns>
        Result CompletePatientReferral(PatientReferralCompleted patientReferralCompleted);

        /// <summary>
        /// Get Patient Referral Details
        /// </summary>
        /// <param name="patientReferralId">int</param>
        /// <param name="emailAddress">string</param>
        /// <param name="patientReferrals">collection of patirntReferral class object</param>
        /// <returns>return Result class object</returns>
        Result GetPatientReferralDetails(int patientReferralId, string referredToEmailAddress, string referredByEmailAddress, out List<PatientReferral> patientReferrals);

        /// <summary>
        /// Add new Patient
        /// </summary>
        /// <param name="patient">patient class object</param>
        /// <returns>return Result class object</returns>
        Result AddPatient(Patient patient, out string PKCS7Response);

        /// <summary>
        /// Get PCKS#7 Response from CA server 
        /// </summary>
        /// <param name="oneTimePassword">string</param>
        /// <param name="pCKS7">string</param>
        /// <returns>return Result class object</returns>
        Result GetPCKSResponse(string oneTimePassword, out string strPCKS7);

        /// <summary>
        /// Delete One Time password
        /// </summary>
        /// <param name="oneTimePassword">string</param>
        void DeleteOTP(string oneTimePassword);

        /// <summary>
        /// Get Master Data
        /// </summary>
        /// <param name="masterCollection">masterCollection</param>
        /// <param name="dependentValue"></param>
        /// <param name="masterDataCollection"></param>
        /// <returns>return Result class object</returns>
        Result GetMasterData(MasterCollection masterCollection, int dependedValue, out List<MasterData> masterDataCollection);

        /// <summary>
        /// Get Patient Details behalf on MPIID or email address
        /// </summary>
        /// <param name="patient"></param>
        /// <param name="MPIID"></param>
        /// <param name="emailAdddress"></param>
        /// <returns></returns>
        Result GetPatientDetails(out Patient patient, string MPIID = "", string emailAdddress = "");

        /// <summary>
        /// Get City,State,Country via zip code
        /// </summary>
        /// <param name="ZipCode">string type</param>
        /// <param name="City">City class object</param>
        /// <returns>result class object</returns>
        Result GetLocalityByZipCode(string zipCode, out City city);

        /// <summary>
        /// Update Patient recode
        /// </summary>
        /// <param name="patientEntity">patientEntity</param>
        /// <returns>return Result class object</returns>
        Result UpdatePatient(Patient patientEntity);

        /// <summary>
        /// Get Patient Consent
        /// </summary>
        /// <param name="MPIID">MPIID</param>
        /// <param name="dsPatientConsent"></param>
        /// <returns>return Result class object</returns>
        Result GetPatientConsent(string MPIID, out List<MobiusPatientConsent> patientConsent);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="MPIID"></param>
        /// <param name="patientConsentId"></param>
        /// <param name="patientConsent"></param>
        /// <returns></returns>
        Result GetPatientConsentByConsentId(string MPIID, int patientConsentId, out MobiusPatientConsent patientConsent, out List<C32Section> C32Sections);

        /// <summary>
        /// Delete Patient Consent
        /// </summary>
        /// <param name="patientConsentId">patientConsentId</param>
        /// <returns>return Result class object</returns>
        Result DeletePatientConsent(string patientConsentId);

        /// <summary>
        /// Update OptIn Status
        /// </summary>
        /// <param name="MPIID">990099</param>
        /// <param name="isOptIn"></param>
        /// <returns>return Result class object</returns>
        Result UpdateOptInStatus(string MPIID, bool isOptIn);

        /// <summary>
        /// Get Specific Patient Consent
        /// </summary>
        /// <param name="MPIID">990099</param>
        /// <param name="patientConsentId"></param>
        /// <param name="datasetSpecificPatientConsent"></param>
        /// <returns>return Result class object</returns>
        //Result GetSpecificPatientConsent(string MPIID, int patientConsentId, out DataSet datasetSpecificPatientConsent);

        /// <summary>
        /// check Policy
        /// </summary>
        /// <param name="checkPolicyRequest">checkPolicyRequest class object</param>
        /// <param name="assertion"></param>
        /// <returns>return Result class object</returns>
        CheckPolicyResponseType CheckPolicy(CheckPolicyRequestType checkPolicyRequest, PolicyEngine.AssertionType assertion);

        /// <summary>
        /// Update Patient Consent Policy
        /// </summary>
        /// <param name="patientConsentPolicy"></param>
        /// <returns>return Result class object</returns>
        Result UpdatePatientConsentPolicy(MobiusPatientConsent patientConsentPolicy);

        /// <summary>
        /// Authenticate User using EmailAddress and Password
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="password"></param>
        /// <param name="userType"></param>
        /// <param name="certificateSerialNumber"></param>
        /// <param name="userName"></param>
        /// <returns>return Result class object</returns>
        Result AuthenticateUser(string emailAddress, string password, int userType, out string certificateSerialNumber, out string userName);


        /// <summary>
        /// This method will check user Type and verifies if exists in database then generate new password and send mail to requested user.
        /// </summary>
        /// <param name="forgotPasswordRequest"></param>
        /// <returns></returns>
        Result ForgotPassword(ForgotPassword forgotPasswordRequest);

        /// <summary>
        /// Change user Password
        /// </summary>
        /// <param name="changePassword"></param>
        /// <returns></returns>
        Result ChangePassword(ChangePassword changePassword);

        /// <summary>
        /// This method would return the Application version
        /// </summary>
        /// <returns></returns>
        string GetApplicationVersion();

        /// <summary>
        /// Get Patient Information via DocumentID
        /// </summary>
        /// <param name="DocumentID"></param>
        /// <param name="patient"></param>
        /// <returns></returns>
        Result GetPatientInformationByDocumentID(string DocumentID, out Patient patient);

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
        /// 
        /// </summary>
        /// <param name="emailAddress">User email address</param>
        /// <param name="userType">User type (patient/provider)</param>
        /// <param name="action">Activate/Deactivate user</param>
        /// <returns></returns>
        Result UpdateUserStatus(string emailAddress, int userType, bool isActive, string userName);


        /// <summary>
        /// This method will return the correlation records of selected Patient
        /// </summary>
        /// <param name="assigningAuthorityId">Home Community Id</param>
        /// <param name="MPIID">Patient Id /MPPID</param>
        /// <param name="patientIdentifiers">Collection of correlation records </param>
        /// <returns></returns>
        Result GetPHISource(string assigningAuthorityId, string patientId, out List<RemotePatientIdentifier> patientIdentifiers);

        /// <summary>
        /// GetProviderDetails
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        Result GetProviderDetails(string emailAddress, out Provider provider);

        /// <summary>
        /// ActivateUser
        /// </summary>
        /// <param name="userType"></param>
        /// <param name="emailAddress"></param>
        /// <param name="CSR"></param>
        /// <param name="PKCS7Response"></param>
        /// <returns></returns>
        Result ActivateUser(Mobius.CoreLibrary.UserType userType, string emailAddress, string CSR, out string PKCS7Response);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkPolicyRequest"></param>
        /// <param name="assertion"></param>
        /// <returns></returns>
        AdapterPEP.CheckPolicyResponseType CheckPolicyPEP(AdapterPEP.CheckPolicyRequestType checkPolicyRequest, AdapterPEP.AssertionType assertion);


        /// <summary>
        /// /// To get All the instance of Emergency Audit
        /// </summary>
        /// <param name="IsShowAll"></param>
        /// <param name="lstEmergencyAudit"></param>
        /// <param name="patientId"></param>
        /// <returns></returns>
        Result GetAllEmergencyAudit(EmergencyRecords emergencyRecords, out List<EmergencyAudit> lstEmergencyAudit, string patientId = "");

        /// <summary>
        /// Get the emergency Details by id
        /// </summary>
        /// <param name="AuditID"></param>
        /// <param name="emergencyAudit"></param>
        /// <returns></returns>
        Result GetEmergencyDetailById(int AuditID, out EmergencyAudit emergencyAudit);
        #region Admin Methods
        /// <summary>
        ///  Get UserDetials into data base Patient and provider table
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        Result GetUserDetials(ref  List<UserDetails> userDetails);

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
        /// To update the status of emergency instances
        /// </summary>
        /// <param name="lstAuditID"></param>
        /// <param name="IsAuditStatus"></param>
        /// <returns></returns>
        Result UpdateOverrideDetails(List<int> lstAuditID, bool IsAuditStatus);

        #endregion
    }
}
