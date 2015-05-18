

namespace Mobius.Server.MobiusHISEService
{
    #region namespaces list
    using System.ServiceModel;
    using MobiusServiceLibrary;
    using System.ServiceModel.Web;
    using Mobius.CoreLibrary;
    #endregion

    #region ServiceContract
    [ServiceContract(Namespace = "urn:MHISE")]
    public interface IMobius
    {
        /// <summary>
        /// For Add Provider
        /// </summary>
        /// <param name="Provider"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        AddProviderResponse AddProvider(AddProviderRequest Provider);
        /// <summary>
        ///  Add Patient
        /// </summary>
        /// <param name="registerPatientRequest">AddPatientRequest class</param>
        /// <returns>AddPatientResponse class</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        AddPatientResponse AddPatient(AddPatientRequest registerPatientRequest);

        /// <summary>
        /// Get Master Data
        /// </summary>
        /// <param name="getMasterDataRequest">GetMasterDataRequest class</param>
        /// <param name="dependedValue">GetMasterDataResponse class</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        GetMasterDataResponse GetMasterData(GetMasterDataRequest getMasterDataRequest);

        /// <summary>
        /// Get Locality via ZipCode
        /// </summary>
        /// <param name="zipCode">string</param>
        /// <returns>GetLocalityByZipCodeResponse class</returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        GetLocalityByZipCodeResponse GetLocalityByZipCode(string zipCode);

        /// <summary>
        /// Authenticate User using EmailAddress
        /// </summary>
        /// <param name="authenticateUserRequest">AuthenticateUserRequest class</param>
        /// <returns>AuthenticateUserResponse class</returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        AuthenticateUserResponse AuthenticateUser(AuthenticateUserRequest authenticateUserRequest);



        /// <summary>
        /// ForgotPassword
        /// </summary>
        /// <param name="forgotPasswordRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        ForgotPasswordResponse ForgotPassword(ForgotPasswordRequest forgotPasswordRequest);

        /// <summary>
        /// This method would return the Application version
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        string GetApplicationVersion();

        /// <summary>
        /// This method would return the PFX Certificate
        /// </summary>
        /// <param name="getPFXCertificateRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        GetPFXCertificateResponse GetPFXCertificate(GetPFXCertificateRequest getPFXCertificateRequest);
           

        /// <summary>
        /// Activate User(patient/Provider)
        /// </summary>
        /// <param name="activateUserRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        ActivateUserResponse ActivateUser(ActivateUserRequest activateUserRequest);

        /// <summary>
        /// GetCSRDetails
        /// </summary>
        /// <param name="getCSRRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        GetCSRResponse GetCSRDetails(GetCSRRequest getCSRRequest);

        /// <summary>
        /// This method is used to validate the C32 document
        /// </summary>
        /// <param name="validateDocumentRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        ValidateDocumentResponse ValidateDocument(ValidateDocumentRequest validateDocumentRequest);
       

    }
    #endregion

}
