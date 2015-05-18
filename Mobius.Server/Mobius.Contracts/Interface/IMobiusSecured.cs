

namespace Mobius.Server.MobiusHISEService
{
    #region namespaces list
    using System.ServiceModel;
    using System.Collections.Generic;
    using MobiusServiceLibrary;
    using System.ServiceModel.Web;
    #endregion

    #region ServiceContract
    [ServiceContract(Namespace = "urn:MHISE")]
    public interface IMobiusSecured
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchPatientRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        SearchPatientResponse SearchPatient(SearchPatientRequest searchPatientRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        GetNhinCommunityResponse GetNhinCommunity();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="getDocumentMetadataRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        GetDocumentMetadataResponse GetDocumentMetadata(GetDocumentMetadataRequest getDocumentMetadataRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="getDocumentRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        GetDocumentResponse GetDocument(GetDocumentRequest getDocumentRequest);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uploadDocumentRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        UploadDocumentResponse UploadDocument(UploadDocumentRequest uploadDocumentRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shareDocumentRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        ShareDocumentResponse ShareDocument(ShareDocumentRequest shareDocumentRequest);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="getDocumentMetadataDocumentIDRequest"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetDocumentMetaDataByDocumentId")]
        [WebInvoke(Method = "POST")]
        GetDocumentMetadataResponse GetDocumentMetaData(GetDocumentMetadataDocumentIDRequest getDocumentMetadataDocumentIDRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="getDocumentMetadataDocumentIDRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST",BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        UpgradeUserResponse UpgradeUser(UpgradeUserRequest upgradeUserRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CreateReferral"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        PatientReferralResponse CreateReferral(CreateReferralRequest createReferralRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AcknowledgeReferral"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        PatientReferralResponse AcknowledgeReferral(AcceptReferralRequest acceptReferredRequest);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CompletePatientReferral"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        PatientReferralResponse CompletePatientReferral(PatientReferralCompletedRequest referralCompletedRequest);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="getPatientReferralRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        GetPatientReferralResponse GetPatientReferralDetails(GetPatientReferralRequest getPatientReferralRequest);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="getPatientDetailsRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        GetPatientDetailsResponse GetPatientDetails(GetPatientDetailsRequest getPatientDetailsRequest);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatePatientRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST")]
        UpdatePatientResponse UpdatePatient(UpdatePatientRequest updatePatientRequest);

        /// <summary>
        /// GetUserInformation via Public Key/Serial number
        /// </summary>
        /// <returns>UserInformationResponse class object</returns>
        [OperationContract]
        [WebInvoke(Method = "POST")]
        UserInformationResponse GetUserInformation();
        /// <summary>
        /// get C32 Sections
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST")]
        GetC32SectionsResponse GetC32Sections();

        [OperationContract]
        [WebInvoke(Method = "Post")]
        GetPatientConsentResponse GetPatientConsent(GetPatientConsentRequest GetPatientConsentRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GetPatientConsentRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "Post")]
        GetPatientConsentResponse GetPatientConsentByConsentId(GetPatientConsentRequest GetPatientConsentRequest);

        /// <summary>
        /// Add,Update Patient Consent 
        /// </summary>
        /// <param name="updatePatientConsentPolicyRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST")]
        UpdatePatientConsentPolicyResponse UpdatePatientConsentPolicy(UpdatePatientConsentPolicyRequest updatePatientConsentPolicyRequest);


        /// <summary>
        /// Delete Patient Consent 
        /// </summary>
        /// <param name="DeletePatientConsentPolicyRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST")]
        DeletePatientConsentPolicyResponse DeletePatientConsentPolicy(DeletePatientConsentPolicyRequest updatePatientConsentPolicyRequest);

        /// <summary>
        /// Update OptIn/OptOut Status
        /// </summary>
        /// <param name="UpdateOptInStatusRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST")]
        UpdateOptInStatusResponse UpdateOptInStatus(UpdateOptInStatusRequest updateOptInStatusRequest);

        /// <summary>
        /// Change User Password
        /// </summary>
        /// <param name="changePasswordRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST")]
        ChangePasswordResponse ChangePassword(ChangePasswordRequest changePasswordRequest);

        /// <summary>
        /// Get Patient Information via DocumentID
        /// </summary>
        /// <param name="getPatientDetailsbyDocumentIdrequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST")]
        GetPatientDetailsResponse GetPatientInformationByDocumentID(GetPatientDetailsbyDocumentIdRequest getPatientDetailsbyDocumentIdrequest);

        /// <summary>
        ///  Add PFXCertificate into data base Patient and provider table
        /// </summary>
        /// <param name="pFXCertificate"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST")]
        AddPFXCertificateResponse AddPFXCertificate(AddPFXCertificateRequest addPFXCertificateRequest);


        /// <summary>
        /// This method will return the correlation records of selected Patient
        /// </summary>
        /// <param name="patientCorrelationRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST")]
        PHISourceResponse GetPHISource(PHISourceRequest patientCorrelationRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="getEmergencyAuditRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST")]
        GetEmergencyAuditResponse GetAllEmergencyAudit(GetEmergencyAuditRequest getEmergencyAuditRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="getEmergencyAuditRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST")]
        GetEmergencyAuditResponse GetEmergencyDetailById(GetEmergencyAuditRequest getEmergencyAuditRequest);

    }
    #endregion

}
