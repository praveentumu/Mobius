
namespace Mobius.Server.MobiusHISEService
{
    #region namesapce
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Mobius.BAL;
    using Mobius.CoreLibrary;
    using Mobius.Entity;
    using MobiusServiceLibrary;
    using MobiusEntity = Mobius.Entity;
    using System.ServiceModel;
    using System.IdentityModel.Claims;
    using System.Security.Cryptography.X509Certificates;
    using System.Security.Cryptography;
    using FirstGenesis.Mobius.Logging;
    using System.Data;
    using MobiusServiceUtility;

    #endregion

    /// <summary>
    /// MobiusHISE class
    /// </summary>

    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    public partial class MobiusHISE : IMobiusSecured
    {
        #region Properties

        /// <summary>
        /// Gets the MobiusBAL class called MobiusBAL
        /// </summary>
        private MobiusBAL MobiusBAL
        {
            get
            {
                return _MobiusBAL != null ? _MobiusBAL : _MobiusBAL = new MobiusBAL(this.SerialNumber);
            }
        }

        private string SerialNumber
        { get; set; }
        private AsymmetricAlgorithm PublicKey
        { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public byte[] CertificatePulickey
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public byte[] IV
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public byte[] SignedData
        {
            get;
            set;
        }

        #endregion
        #region
        private MobiusBAL _MobiusBAL = null;
        private SoapHandler _soapHandler;
        private SoapProperties _soapProperties;

        /// <summary>
        /// Constrictor MobiusBAL
        /// </summary>
        public MobiusHISE()
        {
            _soapHandler = new SoapHandler();
            _soapProperties = new SoapProperties();
            if (OperationContext.Current.EndpointDispatcher.ContractName == "IMobiusSecured")
            {
                foreach (ClaimSet claimSet in OperationContext.Current.ServiceSecurityContext.AuthorizationContext.ClaimSets)
                {
                    X509CertificateClaimSet certificateClaimSet = claimSet as X509CertificateClaimSet;

                    if (certificateClaimSet != null)
                    {
                        X509Certificate2 certificate = certificateClaimSet.X509Certificate;
                        SerialNumber = certificate.SerialNumber;
                        PublicKey = certificate.PublicKey.Key;
                    }
                }
            }

        }

        public MobiusHISE(string serialNumber, AsymmetricAlgorithm publicKey)
        {
            SerialNumber = serialNumber;
            PublicKey = publicKey;
            _soapHandler = new SoapHandler(serialNumber);
            _soapProperties = new SoapProperties();
        }
        /// <summary>
        /// destructor
        /// </summary>
        ~MobiusHISE()
        {
            _MobiusBAL = null;
        }


        #endregion

        #region Methods Making Gateway Calls

        #region SearchPatient

        /// <summary>
        /// SearchPatient method makes call to Gateway to search patients based on the demographic data.
        /// </summary>
        /// <param name="Demographics">SearchPatientRequest class</param>
        /// <returns>SearchPatientResponse class</returns>
        public SearchPatientResponse SearchPatient(SearchPatientRequest searchPatientRequest)
        {
            SearchPatientResponse searchPatientResponse = null;
            try
            {
                if (searchPatientRequest != null)
                {

                    if (_soapHandler.RequestDecryption(searchPatientRequest.SoapProperties, searchPatientRequest, PublicKey))
                    {

                        searchPatientResponse = new SearchPatientResponse();
                        List<Mobius.Entity.Patient> patients = new List<Mobius.Entity.Patient>();
                        List<MobiusNHINCommunity> NHINCommunities = GetNhinCommunity(searchPatientRequest.NHINCommunities);

                        Mobius.Entity.Demographics demographics = new MobiusEntity.Demographics();
                        demographics.DOB = searchPatientRequest.Demographics.DOB;
                        demographics.FamilyName = searchPatientRequest.Demographics.FamilyName;
                        demographics.Gender = searchPatientRequest.Demographics.Gender;
                        demographics.GivenName = searchPatientRequest.Demographics.GivenName;
                        demographics.LocalMPIID = searchPatientRequest.Demographics.LocalMPIID;

                        if (searchPatientRequest.Demographics.MothersMaidenName != null)
                        {
                            demographics.MothersMaidenName.FamilyName = searchPatientRequest.Demographics.MothersMaidenName.FamilyName;
                            demographics.MothersMaidenName.MiddleName = searchPatientRequest.Demographics.MothersMaidenName.MiddleName;
                            demographics.MothersMaidenName.GivenName = searchPatientRequest.Demographics.MothersMaidenName.GivenName;
                            demographics.MothersMaidenName.Prefix = searchPatientRequest.Demographics.MothersMaidenName.Prefix;
                            demographics.MothersMaidenName.Suffix = searchPatientRequest.Demographics.MothersMaidenName.Suffix;
                        }

                        demographics.SSN = searchPatientRequest.Demographics.SSN;
                        demographics.Prefix = searchPatientRequest.Demographics.Prefix;
                        demographics.Suffix = searchPatientRequest.Demographics.Suffix;
                        demographics.MiddleName = searchPatientRequest.Demographics.MiddleName;

                        demographics.DeceasedTime = searchPatientRequest.Demographics.DeceasedDate;

                        //Telephone Number 

                        if (searchPatientRequest.Demographics.ContractNumbers != null && searchPatientRequest.Demographics.ContractNumbers.Count > 0)
                        {
                            Mobius.Entity.Telephone telephone = null;
                            foreach (var item in searchPatientRequest.Demographics.ContractNumbers)
                            {
                                telephone = new MobiusEntity.Telephone();
                                telephone.Number = item;
                                demographics.Telephones.Add(telephone);
                            }
                        }

                        //address search
                        List<int> counter = new List<int>();
                        int StreetCount = 0;
                        int CityCount = 0;
                        int StateCount = 0;
                        int zipCount = 0;
                        int countryCount = 0;

                        if (searchPatientRequest.Demographics.Street != null)
                        {
                            StreetCount = searchPatientRequest.Demographics.Street.Count;
                            counter.Add(StreetCount);
                        }
                        if (searchPatientRequest.Demographics.City != null)
                        {
                            CityCount = searchPatientRequest.Demographics.City.Count;
                            counter.Add(CityCount);
                        }
                        if (searchPatientRequest.Demographics.State != null)
                        {
                            StateCount = searchPatientRequest.Demographics.State.Count;
                            counter.Add(StateCount);
                        }
                        if (searchPatientRequest.Demographics.Zip != null)
                        {
                            zipCount = searchPatientRequest.Demographics.Zip.Count;
                            counter.Add(zipCount);
                        }

                        if (searchPatientRequest.Demographics.Country != null)
                        {
                            countryCount = searchPatientRequest.Demographics.Country.Count;
                            counter.Add(countryCount);
                        }

                        if (counter.Count > 0)
                        {
                            int maxLoop = counter.Max();
                            Mobius.Entity.Address addressParser = null;
                            for (int index = 0; index < maxLoop; index++)
                            {
                                addressParser = new MobiusEntity.Address();

                                if (StreetCount > index)
                                    addressParser.AddressLine1 = searchPatientRequest.Demographics.Street[index];
                                if (CityCount > index)
                                    addressParser.City.CityName = searchPatientRequest.Demographics.City[index];
                                if (StateCount > index)
                                    addressParser.City.State.StateName = searchPatientRequest.Demographics.State[index];
                                if (zipCount > index)
                                    addressParser.Zip = searchPatientRequest.Demographics.Zip[index];
                                if (countryCount > index)
                                    addressParser.City.State.Country.CountryName = searchPatientRequest.Demographics.Country[index];

                                demographics.Addresses.Add(addressParser);
                            }
                        }

                        if (!string.IsNullOrEmpty(searchPatientRequest.Demographics.BirthPlaceStreet) ||
                            !string.IsNullOrEmpty(searchPatientRequest.Demographics.BirthPlaceCity) ||
                            !string.IsNullOrEmpty(searchPatientRequest.Demographics.BirthPlaceState) ||
                            !string.IsNullOrEmpty(searchPatientRequest.Demographics.BirthPlaceZip) ||
                            !string.IsNullOrEmpty(searchPatientRequest.Demographics.BirthPlaceCountry))
                        {
                            Mobius.Entity.Address birthPlaceAddress = new MobiusEntity.Address();
                            birthPlaceAddress.AddressLine1 = searchPatientRequest.Demographics.BirthPlaceStreet;
                            birthPlaceAddress.City.CityName = searchPatientRequest.Demographics.BirthPlaceCity;
                            birthPlaceAddress.City.State.StateName = searchPatientRequest.Demographics.BirthPlaceState;
                            birthPlaceAddress.City.State.Country.CountryName = searchPatientRequest.Demographics.BirthPlaceCountry;
                            birthPlaceAddress.Zip = searchPatientRequest.Demographics.BirthPlaceZip;
                            demographics.BirthPlaceAddress.Add(birthPlaceAddress);
                        }

                        MobiusAssertion mobiusAssertion = GetAssertion(searchPatientRequest.Assertion);

                        searchPatientResponse.Result = this.MobiusBAL.SearchPatient(demographics, NHINCommunities, mobiusAssertion, out patients);

                        if (searchPatientResponse.Result.IsSuccess)
                        {
                            if (patients.Count > 0)
                            {
                                string patientsResponse = XmlSerializerHelper.SerializeObject(patients);
                                searchPatientResponse.Patients = (List<MobiusServiceLibrary.Patient>)XmlSerializerHelper.DeserializeObject(patientsResponse, typeof(List<MobiusServiceLibrary.Patient>));
                                ResponseEncryption(searchPatientResponse);
                            }

                        }
                    }
                    else
                    {
                        searchPatientResponse = new SearchPatientResponse();
                        searchPatientResponse.Patients = null;
                        searchPatientResponse.Result.IsSuccess = false;
                        searchPatientResponse.Result.SetError(ErrorCode.Not_Valid_Request_Data);
                    }
                }

            }
            catch (Exception ex)
            {
                searchPatientResponse.Result.IsSuccess = false;
                searchPatientResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }

            return searchPatientResponse;
        }



        #endregion SearchPatient

        #region GetDocuments
        /// <summary>
        /// GetDocuments Method makes call to remote community from where it wants to look for patient's data.
        /// </summary>
        /// <param name="getDocumentMetadataRequest">GetDocumentMetadataRequest class</param>
        /// <returns>GetDocumentMetadataResponse class</returns>
        public GetDocumentMetadataResponse GetDocumentMetadata(GetDocumentMetadataRequest getDocumentMetadataRequest)
        {
            GetDocumentMetadataResponse documentResponse = null;
            try
            {
                documentResponse = new GetDocumentMetadataResponse();
                List<MobiusDocument> documents = null;
                List<MobiusNHINCommunity> NHINCommunities;
                if (_soapHandler.RequestDecryption(getDocumentMetadataRequest.SoapProperties, getDocumentMetadataRequest, PublicKey))
                {
                    NHINCommunities = GetNhinCommunity(getDocumentMetadataRequest.communities);
                    documentResponse.Result = this.MobiusBAL.GetDocumentMetadata(getDocumentMetadataRequest.patientId, NHINCommunities,
                        GetAssertion(getDocumentMetadataRequest.Assertion),
                        getDocumentMetadataRequest.GetLocallyAvailable, out documents);
                    // TODO:Need to verify following error handling, as one call (like to home) can return valid value and other call (to remote) might return error.
                    if (documentResponse.Result.IsSuccess)
                    {
                        documentResponse.Documents = XmlSerializerHelper.DeserializeObject(XmlSerializerHelper.SerializeObject(documents), typeof(List<Document>)) as List<Document>;
                        ResponseEncryption(documentResponse);
                    }
                    else
                    {
                        documentResponse.Result.IsSuccess = false;
                        documentResponse.Documents = XmlSerializerHelper.DeserializeObject(XmlSerializerHelper.SerializeObject(documents), typeof(List<Document>)) as List<Document>;

                    }


                }
                else
                {
                    documentResponse.Result.IsSuccess = false;
                    documentResponse.Result.SetError(ErrorCode.Not_Valid_Request_Data);
                }
            }
            catch (Exception ex)
            {
                documentResponse.Result.IsSuccess = false;
                documentResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return documentResponse;
        }


        #endregion GetDocuments

        #region GetDocument
        /// <summary>
        /// GetDocument method returns a single document
        /// </summary>
        /// <param name="getDocumentRequest">object of GetDocumentRequest class</param>
        /// <returns>object of GetDocumentResponse class</returns>
        public GetDocumentResponse GetDocument(GetDocumentRequest getDocumentRequest)
        {
            GetDocumentResponse documentResponse = null;
            try
            {
                documentResponse = new GetDocumentResponse();
                MobiusDocument document = null;
                if (_soapHandler.RequestDecryption(getDocumentRequest.SoapProperties, getDocumentRequest, PublicKey))
                {
                    DocumentRequest documentRequest = new DocumentRequest();

                    documentRequest = XmlSerializerHelper.DeserializeObject(XmlSerializerHelper.SerializeObject(getDocumentRequest), typeof(DocumentRequest)) as DocumentRequest;

                    if (getDocumentRequest.EmergencyAccess != null && documentRequest != null)
                    {
                        documentRequest.OverrideReason = getDocumentRequest.EmergencyAccess.OverrideReason;
                        documentRequest.Description = getDocumentRequest.EmergencyAccess.Description;
                        documentRequest.EmergencyOverrideStatus = getDocumentRequest.EmergencyAccess.Status;
                    }


                    documentResponse.Result = this.MobiusBAL.GetDocument(documentRequest, out document);


                    if (documentResponse.Result.IsSuccess)
                    {
                        documentResponse.Document = XmlSerializerHelper.DeserializeObject(XmlSerializerHelper.SerializeObject(document), typeof(Document)) as Document;
                        ResponseEncryption(documentResponse);
                    }
                    else if (!documentResponse.Result.IsSuccess && document.XACMLBytes != null) //if XACML bytes are available then send the xacml bytes only because of security issue
                    {
                        documentResponse.Document.XACMLBytes = document.XACMLBytes;
                    }

                }
                else
                {
                    documentResponse.Result.IsSuccess = false;
                    documentResponse.Result.SetError(ErrorCode.Not_Valid_Request_Data);
                }
            }
            catch (Exception ex)
            {
                documentResponse.Result.IsSuccess = false;
                documentResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return documentResponse;
        }

        #endregion GetDocument

        #region UploadDocument
        /// <summary>
        /// Upload Document makes call to upload a document to gateway along with the XACML file.
        /// </summary>
        /// <param name="uploadDocumentRequest"></param>
        /// <returns></returns>
        public UploadDocumentResponse UploadDocument(UploadDocumentRequest uploadDocumentRequest)
        {
            UploadDocumentResponse uploadDocumentResponse = null;
            try
            {
                uploadDocumentResponse = new UploadDocumentResponse();
                if (_soapHandler.RequestDecryption(uploadDocumentRequest.SoapProperties, uploadDocumentRequest, PublicKey))
                {

                    uploadDocumentResponse.Result = this.MobiusBAL.UploadDocument(uploadDocumentRequest.CommunityId, uploadDocumentRequest.DocumentId, uploadDocumentRequest.DocumentBytes, uploadDocumentRequest.XACMLBytes, uploadDocumentRequest.PatientId, uploadDocumentRequest.UploadedBy, uploadDocumentRequest.RepositoryId,
                        uploadDocumentRequest.SubmitOnGateway, GetAssertion(uploadDocumentRequest.Assertion));
                    if (uploadDocumentResponse.Result.IsSuccess)
                    {
                        ResponseEncryption(uploadDocumentResponse);
                    }
                }
                else
                {
                    uploadDocumentResponse.Result.IsSuccess = false;
                    uploadDocumentResponse.Result.SetError(ErrorCode.Not_Valid_Request_Data);
                }
            }
            catch (Exception ex)
            {
                uploadDocumentResponse.Result.IsSuccess = false;
                uploadDocumentResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return uploadDocumentResponse;
        }

        #endregion UploadDocument

        #region ShareDocument
        /// <summary>
        /// ShareDocument method makes call to Gateway to share the document to the gateway with applied consents.
        /// </summary>
        /// <param name="shareDocumentRequest">object of ShareDocumentRequest class</param>
        /// <returns>object of ShareDocumentResponse class</returns>
        public ShareDocumentResponse ShareDocument(ShareDocumentRequest shareDocumentRequest)
        {
            ShareDocumentResponse shareDocumentResponse = null;
            try
            {
                shareDocumentResponse = new ShareDocumentResponse();
                ShareDocument shareDocument = new ShareDocument();
                if (_soapHandler.RequestDecryption(shareDocumentRequest.SoapProperties, shareDocumentRequest, PublicKey))
                {
                    shareDocument = XmlSerializerHelper.DeserializeObject(XmlSerializerHelper.SerializeObject(shareDocumentRequest), typeof(ShareDocument)) as ShareDocument;
               
                    shareDocumentResponse.Result = this.MobiusBAL.ShareDocument(shareDocument);
                   
                    if (shareDocumentResponse.Result.IsSuccess)
                    {
                        shareDocumentResponse.Status = true;
                        ResponseEncryption(shareDocumentResponse);
                    }

                }
                else
                {
                    shareDocumentResponse.Result.IsSuccess = false;
                    shareDocumentResponse.Result.SetError(ErrorCode.Not_Valid_Request_Data);
                }
            }
            catch (Exception ex)
            {
                shareDocumentResponse.Result.IsSuccess = false;
                shareDocumentResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);

            }
            return shareDocumentResponse;

        }
        #endregion ShareDocument

        #endregion Methods Making Gateway Calls

        #region Methods Making Database Calls
        /// <summary>
        /// GetDocumentMetaData
        /// </summary>
        /// <param name="documentID">object of GetDocumentMetadataDocumentIDRequest</param>
        /// <returns>object of GetDocumentMetadataResponse</returns>
        public GetDocumentMetadataResponse GetDocumentMetaData(GetDocumentMetadataDocumentIDRequest getDocumentMetadataDocumentIDRequest)
        {
            GetDocumentMetadataResponse documentMetadataResponse = null;
            try
            {
                MobiusDocument document = null;
                documentMetadataResponse = new GetDocumentMetadataResponse();
                if (_soapHandler.RequestDecryption(getDocumentMetadataDocumentIDRequest.SoapProperties, getDocumentMetadataDocumentIDRequest, PublicKey))
                {
                    documentMetadataResponse.Result = this.MobiusBAL.GetDocumentMetaData(getDocumentMetadataDocumentIDRequest.documentID, out document);
                    if (documentMetadataResponse.Result.IsSuccess)
                    {
                        //documentMetadataResponse.Documents.Add(document);
                        documentMetadataResponse.Documents.Add(XmlSerializerHelper.DeserializeObject(XmlSerializerHelper.SerializeObject(document), typeof(Document)) as Document);
                        ResponseEncryption(documentMetadataResponse);
                    }

                }
                else
                {
                    documentMetadataResponse.Result.IsSuccess = false;
                    documentMetadataResponse.Result.SetError(ErrorCode.Not_Valid_Request_Data);
                }
            }
            catch (Exception ex)
            {
                documentMetadataResponse.Result.IsSuccess = false;
                documentMetadataResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return documentMetadataResponse;
        }
        #endregion Methods Making Database Calls

        #region NHIN COMMUNITY
        /// <summary>
        /// GetNhinCommunity
        /// </summary>
        /// <returns>object of GetNhinCommunityResponse class</returns>
        public GetNhinCommunityResponse GetNhinCommunity()
        {
            GetNhinCommunityResponse getNhinCommunityResponse = null;
            try
            {
                List<MobiusNHINCommunity> NHINCommunities = null;
                getNhinCommunityResponse = new GetNhinCommunityResponse();
                getNhinCommunityResponse.Result = this.MobiusBAL.GetNhinCommunity(out NHINCommunities);
                if (getNhinCommunityResponse.Result.IsSuccess)
                {
                    getNhinCommunityResponse.Communities = XmlSerializerHelper.DeserializeObject(XmlSerializerHelper.SerializeObject(NHINCommunities), typeof(List<NHINCommunity>)) as List<NHINCommunity>;
                    ResponseEncryption(getNhinCommunityResponse);
                }


            }
            catch (Exception ex)
            {
                getNhinCommunityResponse.Result.IsSuccess = false;
                getNhinCommunityResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return getNhinCommunityResponse;
        }
        #endregion

        #region PatientReferral

        /// <summary>
        ///  CreateReferral
        /// </summary>
        /// <param name="createReferralRequest">object of CreateReferralRequest class</param>
        /// <returns>object of PatientReferralResponse class</returns>
        public PatientReferralResponse CreateReferral(CreateReferralRequest createReferralRequest)
        {
            PatientReferralResponse patientReferredResponse = new PatientReferralResponse();
            try
            {
                if (_soapHandler.RequestDecryption(createReferralRequest.SoapProperties, createReferralRequest, PublicKey))
                {
                    Mobius.Entity.CreatePatientReferral createPatientReferral = new MobiusEntity.CreatePatientReferral();
                    createPatientReferral.CommunityId = createReferralRequest.PatientReferred.CommunityId;
                    createPatientReferral.DocumentId = createReferralRequest.PatientReferred.DocumentId;
                    createPatientReferral.DocumentBytes = createReferralRequest.PatientReferred.DocumentBytes;
                    createPatientReferral.Id = createReferralRequest.PatientReferred.Id;
                    createPatientReferral.PurposeOfUse = createReferralRequest.PatientReferred.PurposeOfUse;
                    createPatientReferral.ReferralAccomplishedOn = createReferralRequest.PatientReferred.ReferralAccomplishedOn;
                    createPatientReferral.ReferralOn = createReferralRequest.PatientReferred.ReferralOn;
                    createPatientReferral.ReferralSummary = createReferralRequest.PatientReferred.ReferralSummary;
                    createPatientReferral.ReferredBy = createReferralRequest.PatientReferred.ReferredBy;
                    createPatientReferral.ReferredToEmail = createReferralRequest.PatientReferred.ReferredToEmail;
                    createPatientReferral.ReferredByEmail = createReferralRequest.PatientReferred.ReferredByEmail;
                    createPatientReferral.Subject = createReferralRequest.PatientReferred.Subject;
                    createPatientReferral.OriginalDocumentID = createReferralRequest.PatientReferred.OriginalDocumentID;
                    createPatientReferral.XACMLBytes = createReferralRequest.PatientReferred.XACMLBytes;
                    if (createReferralRequest.PatientReferred.Patient != null)
                    {
                        createPatientReferral.Patient.GivenName = createReferralRequest.PatientReferred.Patient.GivenName;
                        createPatientReferral.Patient.FamilyName = createReferralRequest.PatientReferred.Patient.FamilyName;
                        createPatientReferral.Patient.Gender = createReferralRequest.PatientReferred.Patient.Gender;
                        createPatientReferral.Patient.DOB = createReferralRequest.PatientReferred.Patient.DOB;
                        createPatientReferral.Patient.LocalMPIID = createReferralRequest.PatientReferred.Patient.LocalMPIID;
                    }

                    createPatientReferral.Assertion = GetAssertion(createReferralRequest.PatientReferred.Assertion);

                    patientReferredResponse.Result = this.MobiusBAL.CreateReferral(createPatientReferral);
                    // Encryption of patientReferredResponse
                    if (patientReferredResponse.Result.IsSuccess)
                    {
                        ResponseEncryption(patientReferredResponse);
                    }
                }
                else
                {
                    patientReferredResponse.Result = new Result();
                    patientReferredResponse.Result.IsSuccess = false;
                    patientReferredResponse.Result.SetError(ErrorCode.Not_Valid_Request_Data);
                }
            }
            catch (Exception ex)
            {
                patientReferredResponse.Result.IsSuccess = false;
                patientReferredResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);

            }

            return patientReferredResponse;
        }

        /// <summary>
        ///  AcknowledgeReferral
        /// </summary>
        /// <param name="patientReferredRequest">object of AcceptReferralRequest class</param>
        /// <returns>object of PatientReferralResponse class</returns>
        public PatientReferralResponse AcknowledgeReferral(AcceptReferralRequest acceptReferredRequest)
        {
            PatientReferralResponse patientReferredResponse = new PatientReferralResponse();
            try
            {
                if (_soapHandler.RequestDecryption(acceptReferredRequest.SoapProperties, acceptReferredRequest, PublicKey))
                {
                    Mobius.Entity.AcceptReferral acceptReferral = new MobiusEntity.AcceptReferral();
                    acceptReferral.AcknowledgementStatus = acceptReferredRequest.AcceptPatientReferred.AcknowledgementStatus;
                    acceptReferral.DocumentId = acceptReferredRequest.AcceptPatientReferred.DocumentId;
                    acceptReferral.Id = acceptReferredRequest.AcceptPatientReferred.Id;
                    acceptReferral.ReferralAccomplishedOn = acceptReferredRequest.AcceptPatientReferred.ReferralAccomplishedOn;
                    acceptReferral.ReferralOn = acceptReferredRequest.AcceptPatientReferred.ReferralOn;
                    acceptReferral.ReferredToEmail = acceptReferredRequest.AcceptPatientReferred.ReferredToEmail;
                    acceptReferral.ReferredByEmail = acceptReferredRequest.AcceptPatientReferred.ReferredByEmail;
                    acceptReferral.PatientAppointmentDate = acceptReferredRequest.AcceptPatientReferred.PatientAppointmentDate;
                    acceptReferral.DispatcherSummary = acceptReferredRequest.AcceptPatientReferred.DispatcherSummary;

                    if (acceptReferredRequest.AcceptPatientReferred.Patient != null)
                    {
                        acceptReferral.Patient.GivenName = acceptReferredRequest.AcceptPatientReferred.Patient.GivenName;
                        acceptReferral.Patient.FamilyName = acceptReferredRequest.AcceptPatientReferred.Patient.FamilyName;
                        acceptReferral.Patient.Gender = acceptReferredRequest.AcceptPatientReferred.Patient.Gender;
                        acceptReferral.Patient.DOB = acceptReferredRequest.AcceptPatientReferred.Patient.DOB;
                        acceptReferral.Patient.LocalMPIID = acceptReferredRequest.AcceptPatientReferred.Patient.LocalMPIID;
                    }

                    patientReferredResponse.Result = this.MobiusBAL.AcknowledgeReferral(acceptReferral);
                    // Encryption of patientReferredResponse
                    if (patientReferredResponse.Result.IsSuccess)
                    {
                        ResponseEncryption(patientReferredResponse);
                    }
                }
                else
                {
                    patientReferredResponse.Result = new Result();
                    patientReferredResponse.Result.IsSuccess = false;
                    patientReferredResponse.Result.SetError(ErrorCode.Not_Valid_Request_Data);
                }
            }
            catch (Exception ex)
            {
                patientReferredResponse.Result.IsSuccess = false;
                patientReferredResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);

            }

            return patientReferredResponse;
        }

        /// <summary>
        ///  CompletePatientReferral
        /// </summary>
        /// <param name="patientReferredRequest">object of PatientReferralCompletedRequest class</param>
        /// <returns>object of PatientReferralResponse class</returns>
        public PatientReferralResponse CompletePatientReferral(PatientReferralCompletedRequest referralCompletedRequest)
        {
            PatientReferralResponse patientReferredResponse = new PatientReferralResponse();
            try
            {
                if (_soapHandler.RequestDecryption(referralCompletedRequest.SoapProperties, referralCompletedRequest, PublicKey))
                {
                    Mobius.Entity.PatientReferralCompleted referralCompleted = new MobiusEntity.PatientReferralCompleted();
                    referralCompleted.DispatcherSummary = referralCompletedRequest.ReferralCompleted.DispatcherSummary;
                    referralCompleted.DocumentId = referralCompletedRequest.ReferralCompleted.DocumentId;
                    referralCompleted.Id = referralCompletedRequest.ReferralCompleted.Id;
                    referralCompleted.OutcomeDocumentID = referralCompletedRequest.ReferralCompleted.OutcomeDocumentID;
                    referralCompleted.ReferredByEmail = referralCompletedRequest.ReferralCompleted.ReferredByEmail;
                    referralCompleted.ReferredToEmail = referralCompletedRequest.ReferralCompleted.ReferredToEmail;
                    referralCompleted.ReferralAccomplishedOn = referralCompletedRequest.ReferralCompleted.ReferralAccomplishedOn;
                    referralCompleted.ReferralOn = referralCompletedRequest.ReferralCompleted.ReferralOn;
                    referralCompleted.AcknowledgementStatus = referralCompletedRequest.ReferralCompleted.AcknowledgementStatus;
                    referralCompleted.PatientAppointmentDate = referralCompletedRequest.ReferralCompleted.PatientAppointmentDate;
                    referralCompleted.ReferralCompleted = referralCompletedRequest.ReferralCompleted.ReferralCompleted;
                    referralCompleted.DocumentBytes = referralCompletedRequest.ReferralCompleted.DocumentBytes;
                    referralCompleted.XACMLBytes = referralCompletedRequest.ReferralCompleted.XACMLBytes;

                    if (referralCompletedRequest.ReferralCompleted.Patient != null)
                    {
                        referralCompleted.Patient.GivenName = referralCompletedRequest.ReferralCompleted.Patient.GivenName;
                        referralCompleted.Patient.FamilyName = referralCompletedRequest.ReferralCompleted.Patient.FamilyName;
                        referralCompleted.Patient.Gender = referralCompletedRequest.ReferralCompleted.Patient.Gender;
                        referralCompleted.Patient.DOB = referralCompletedRequest.ReferralCompleted.Patient.DOB;
                        referralCompleted.Patient.LocalMPIID = referralCompletedRequest.ReferralCompleted.Patient.LocalMPIID;
                    }

                    referralCompleted.Assertion = GetAssertion(referralCompletedRequest.ReferralCompleted.Assertion);

                    patientReferredResponse.Result = this.MobiusBAL.CompletePatientReferral(referralCompleted);
                    // Encryption of patientReferredResponse
                    if (patientReferredResponse.Result.IsSuccess)
                    {
                        ResponseEncryption(patientReferredResponse);
                    }
                }
                else
                {
                    patientReferredResponse.Result = new Result();
                    patientReferredResponse.Result.IsSuccess = false;
                    patientReferredResponse.Result.SetError(ErrorCode.Not_Valid_Request_Data);
                }
            }
            catch (Exception ex)
            {
                patientReferredResponse.Result.IsSuccess = false;
                patientReferredResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);

            }

            return patientReferredResponse;
        }

        #endregion PatientReferral

        #region GetPatientReferral
        /// <summary>
        /// GetPatientReferralDetails
        /// </summary>
        /// <param name="getPatientReferralRequest">object of GetPatientReferralRequest</param>
        /// <returns>object of GetPatientReferralResponse</returns>
        public GetPatientReferralResponse GetPatientReferralDetails(GetPatientReferralRequest getPatientReferralRequest)
        {
            GetPatientReferralResponse getPatientReferralDetail = new GetPatientReferralResponse();
            try
            {
                List<Mobius.Entity.PatientReferral> entityPatientReferrals = null;
                if (_soapHandler.RequestDecryption(getPatientReferralRequest.SoapProperties, getPatientReferralRequest, PublicKey))
                {
                    getPatientReferralDetail.Result = this.MobiusBAL.GetPatientReferralDetails(getPatientReferralRequest.patientReferralId, getPatientReferralRequest.referredToEmailAddress, getPatientReferralRequest.referredByEmailAddress, out entityPatientReferrals);
                    if (getPatientReferralDetail.Result.IsSuccess)
                    {
                        getPatientReferralDetail.PatientReferrals = new List<MobiusServiceLibrary.PatientReferral>();
                        if (entityPatientReferrals.Count > 0)
                        {
                            foreach (Mobius.Entity.PatientReferral entityPatientReferral in entityPatientReferrals)
                            {
                                MobiusServiceLibrary.PatientReferral patientreferral = new MobiusServiceLibrary.PatientReferral();

                                patientreferral.AcknowledgementStatus = entityPatientReferral.AcknowledgementStatus;
                                patientreferral.CommunityId = entityPatientReferral.CommunityId;
                                patientreferral.DispatcherSummary = entityPatientReferral.DispatcherSummary;
                                patientreferral.DocumentBytes = entityPatientReferral.DocumentBytes;
                                patientreferral.DocumentId = entityPatientReferral.DocumentId;
                                patientreferral.Id = entityPatientReferral.Id;
                                patientreferral.OutcomeDocumentID = entityPatientReferral.OutcomeDocumentID;
                                patientreferral.PatientAppointmentDate = entityPatientReferral.PatientAppointmentDate;
                                patientreferral.PurposeOfUse = (PurposeOfUse)Enum.Parse(typeof(PurposeOfUse), entityPatientReferral.PurposeOfUseId.ToString(), true);
                                patientreferral.PurposeOfUseId = entityPatientReferral.PurposeOfUseId;
                                patientreferral.ReferralAccomplishedOn = entityPatientReferral.ReferralAccomplishedOn;
                                patientreferral.ReferralCompleted = entityPatientReferral.ReferralCompleted;
                                patientreferral.ReferralOn = entityPatientReferral.ReferralOn;
                                patientreferral.ReferralSummary = entityPatientReferral.ReferralSummary;
                                patientreferral.ReferredBy = entityPatientReferral.ReferredBy;
                                patientreferral.ReferredByEmail = entityPatientReferral.ReferredByEmail;
                                patientreferral.ReferredToEmail = entityPatientReferral.ReferredToEmail;
                                patientreferral.Subject = entityPatientReferral.Subject;
                                patientreferral.XACMLBytes = entityPatientReferral.XACMLBytes;
                                patientreferral.ReferralCompletedOn = entityPatientReferral.ReferralCompletedOn;
                                patientreferral.OutcomeDocumentID = entityPatientReferral.OutcomeDocumentID;

                                if (entityPatientReferral.Patient != null)
                                {
                                    patientreferral.Patient = new MobiusServiceLibrary.Patient();
                                    patientreferral.Patient.GivenName = new List<string>();
                                    patientreferral.Patient.GivenName = entityPatientReferral.Patient.GivenName;
                                    patientreferral.Patient.FamilyName = new List<string>();
                                    patientreferral.Patient.FamilyName = entityPatientReferral.Patient.FamilyName;
                                    patientreferral.Patient.Gender = entityPatientReferral.Patient.Gender;
                                    patientreferral.Patient.DOB = entityPatientReferral.Patient.DOB;
                                    patientreferral.Patient.LocalMPIID = entityPatientReferral.Patient.LocalMPIID;
                                    patientreferral.Patient.MiddleName = new List<string>();
                                    patientreferral.Patient.IDNames = new List<int>();
                                    patientreferral.Patient.Telephones = new List<MobiusServiceLibrary.Telephone>();
                                    patientreferral.Patient.PatientAddress = new List<MobiusServiceLibrary.Address>();
                                }
                                getPatientReferralDetail.PatientReferrals.Add(patientreferral);
                            }
                        }

                        ResponseEncryption(getPatientReferralDetail);
                    }
                }
                else
                {
                    getPatientReferralDetail.Result = new Result();
                    getPatientReferralDetail.Result.IsSuccess = false;
                    getPatientReferralDetail.Result.SetError(ErrorCode.Not_Valid_Request_Data);
                }

            }
            catch (Exception ex)
            {
                getPatientReferralDetail.Result = new Result();
                getPatientReferralDetail.Result.IsSuccess = false;
                getPatientReferralDetail.Result.SetError(ErrorCode.UnknownException, ex.Message);

            }

            return getPatientReferralDetail;
        }
        #endregion

        #region UpdatePatient
        /// <summary>
        ///  UpdatePatientrecode
        /// </summary>
        /// <param name="updatePatientRequest">object of UpdatePatientRequest class</param>
        /// <returns>object of UpdatePatientResponse class</returns>
        public UpdatePatientResponse UpdatePatient(UpdatePatientRequest updatePatientRequest)
        {
            UpdatePatientResponse updatePatientResponse = new UpdatePatientResponse();

            // Contract Variables
            MobiusServiceLibrary.Patient patient = null;
            List<MobiusServiceLibrary.Telephone> telephones = null;
            List<MobiusServiceLibrary.Address> patientAddresses = null;

            // Entityvariables
            Mobius.Entity.Patient patientEntity = null;

            Mobius.Entity.Telephone telephoneEntity = null;
            Mobius.Entity.Address addressEntity = null;
            Mobius.Entity.City cityEntity = null;
            Mobius.Entity.State stateEntity = null;
            Mobius.Entity.Country countryEntity = null;

            try
            {
                if (_soapHandler.RequestDecryption(updatePatientRequest.SoapProperties, updatePatientRequest, PublicKey))
                {
                    patient = new MobiusServiceLibrary.Patient();
                    telephones = new List<MobiusServiceLibrary.Telephone>();
                    patientAddresses = new List<MobiusServiceLibrary.Address>();

                    patientEntity = new Mobius.Entity.Patient();


                    // Single Variable Assign
                    if (updatePatientRequest.Patient != null)
                    {
                        patientEntity.EmailAddress = updatePatientRequest.Patient.EmailAddress;
                        patientEntity.PatientId = updatePatientRequest.Patient.PatientId;
                        patientEntity.CommunityId = updatePatientRequest.Patient.CommunityId;
                    }

                    // Convert Demographics data from contract to Entity
                    if (updatePatientRequest.Patient != null)
                    {
                        patient = updatePatientRequest.Patient;

                        patientEntity.DOB = patient.DOB;
                        patientEntity.Gender = patient.Gender;


                        patientEntity.MothersMaidenName.Prefix = patient.MothersMaidenName != null ? (!string.IsNullOrEmpty(patient.MothersMaidenName.Prefix) ? patient.MothersMaidenName.Prefix : null) : null;
                        patientEntity.MothersMaidenName.GivenName = patient.MothersMaidenName != null ? (!string.IsNullOrEmpty(patient.MothersMaidenName.GivenName) ? patient.MothersMaidenName.GivenName : null) : null;
                        patientEntity.MothersMaidenName.MiddleName = patient.MothersMaidenName != null ? (!string.IsNullOrEmpty(patient.MothersMaidenName.MiddleName) ? patient.MothersMaidenName.MiddleName : null) : null;
                        patientEntity.MothersMaidenName.FamilyName = patient.MothersMaidenName != null ? (!string.IsNullOrEmpty(patient.MothersMaidenName.FamilyName) ? patient.MothersMaidenName.FamilyName : null) : null;
                        patientEntity.MothersMaidenName.Suffix = patient.MothersMaidenName != null ? (!string.IsNullOrEmpty(patient.MothersMaidenName.Suffix) ? patient.MothersMaidenName.Suffix : null) : null;


                        patientEntity.SSN = patient.SSN;
                        patientEntity.LocalMPIID = patient.LocalMPIID;
                        foreach (string familyName in patient.FamilyName)
                        {
                            patientEntity.FamilyName.Add(familyName);
                        }

                        foreach (string middleName in patient.MiddleName)
                        {
                            patientEntity.MiddleName.Add(middleName);
                        }

                        foreach (string givenName in patient.GivenName)
                        {
                            patientEntity.GivenName.Add(givenName);
                        }
                        foreach (string Prefix in patient.Prefix)
                        {
                            patientEntity.Prefix.Add(Prefix);
                        }
                        foreach (string Suffix in patient.Suffix)
                        {
                            patientEntity.Suffix.Add(Suffix);
                        }
                        foreach (int idName in patient.IDNames)
                        {
                            patientEntity.IDNames.Add(idName);
                        }
                        foreach (ActionType type in patient.Action)
                        {
                            patientEntity.Action.Add(type);
                        }
                    }

                    // Convert telephone data from contract to Entity
                    if (updatePatientRequest.Patient.Telephones != null)
                    {
                        telephones = updatePatientRequest.Patient.Telephones;
                        foreach (MobiusServiceLibrary.Telephone telephone in telephones)
                        {
                            telephoneEntity = new Entity.Telephone();
                            telephoneEntity.Id = telephone.Id;
                            telephoneEntity.Type = telephone.Type;
                            telephoneEntity.Extensionnumber = telephone.Extensionnumber;
                            telephoneEntity.Number = telephone.Number;
                            telephoneEntity.Status = telephone.Status;
                            telephoneEntity.Action = telephone.Action;
                            patientEntity.Telephone.Add(telephoneEntity);
                        }
                    }
                    // Convert Address data from contract to Entity
                    if (updatePatientRequest.Patient.PatientAddress != null)
                    {
                        patientAddresses = updatePatientRequest.Patient.PatientAddress;
                        foreach (MobiusServiceLibrary.Address address in patientAddresses)
                        {
                            addressEntity = new Entity.Address();

                            addressEntity.Id = address.Id;
                            addressEntity.AddressLine1 = address.AddressLine1;
                            addressEntity.AddressLine2 = address.AddressLine2;
                            addressEntity.AddressStatus = address.AddressStatus;
                            addressEntity.Action = address.Action;
                            addressEntity.Zip = address.Zip;
                            if (address.City != null)
                            {
                                cityEntity = new Mobius.Entity.City();
                                cityEntity.CityName = address.City.CityName;

                                if (address.City.State != null)
                                {
                                    stateEntity = new Mobius.Entity.State();
                                    stateEntity.StateName = address.City.State.StateName;
                                    cityEntity.State = stateEntity;

                                    if (address.City.State.Country != null)
                                    {
                                        countryEntity = new Mobius.Entity.Country();
                                        countryEntity.CountryName = address.City.State.Country.CountryName;
                                        stateEntity.Country = countryEntity;
                                    }
                                }
                                addressEntity.City = cityEntity;
                            }
                            patientEntity.PatientAddress.Add(addressEntity);
                        }
                    }

                    if (updatePatientRequest.Patient != null)
                    {
                        patientEntity.BirthPlaceAddress = !string.IsNullOrEmpty(updatePatientRequest.Patient.BirthPlaceAddress) ? updatePatientRequest.Patient.BirthPlaceAddress : string.Empty;
                        patientEntity.BirthPlaceCity = !string.IsNullOrEmpty(updatePatientRequest.Patient.BirthPlaceCity) ? updatePatientRequest.Patient.BirthPlaceCity : string.Empty;
                        patientEntity.BirthPlaceState = !string.IsNullOrEmpty(updatePatientRequest.Patient.BirthPlaceState) ? updatePatientRequest.Patient.BirthPlaceState : string.Empty;
                        patientEntity.BirthPlaceCountry = !string.IsNullOrEmpty(updatePatientRequest.Patient.BirthPlaceCountry) ? updatePatientRequest.Patient.BirthPlaceCountry : string.Empty;
                        patientEntity.BirthPlaceZip = !string.IsNullOrEmpty(updatePatientRequest.Patient.BirthPlaceZip) ? updatePatientRequest.Patient.BirthPlaceZip : string.Empty;
                    }

                    updatePatientResponse.Result = this.MobiusBAL.UpdatePatient(patientEntity);

                    // Encryption of updatePatientResponse
                    ResponseEncryption(updatePatientResponse);
                }
                else
                {
                    updatePatientResponse.Result.IsSuccess = false;
                    updatePatientResponse.Result.SetError(ErrorCode.Not_Valid_Request_Data);
                }
            }

            catch (Exception ex)
            {
                updatePatientResponse.Result.IsSuccess = false;
                updatePatientResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);

            }
            return updatePatientResponse;
        }

        #endregion

        #region GetPatientDetails
        /// <summary>
        /// GetPatientDetails on behalf of MPIID
        /// </summary>
        /// <param name="getPatientDetailsRequest"></param>
        /// <returns></returns>
        public GetPatientDetailsResponse GetPatientDetails(GetPatientDetailsRequest getPatientDetailsRequest)
        {
            GetPatientDetailsResponse getPatientDetailsResponse = null;
            try
            {
                getPatientDetailsResponse = new GetPatientDetailsResponse();
                getPatientDetailsResponse.Result = new Result();

                if (getPatientDetailsRequest != null)
                {
                    Mobius.Entity.Patient patient = new Mobius.Entity.Patient();
                    MobiusServiceLibrary.Telephone serviceTelephone = null;
                    MobiusServiceLibrary.Address serviceAddress = null;
                    MobiusServiceLibrary.Country serviceCountry = null;
                    MobiusServiceLibrary.State serviceState = null;
                    MobiusServiceLibrary.City serviceCity = null;

                    if (_soapHandler.RequestDecryption(getPatientDetailsRequest.SoapProperties, getPatientDetailsRequest, PublicKey))
                    {

                        if (string.IsNullOrWhiteSpace(getPatientDetailsRequest.MPIID))
                        {
                            getPatientDetailsResponse.Result.IsSuccess = false;
                            getPatientDetailsResponse.Result.SetError(ErrorCode.SearchPatient_Missing_Demographics_MPIID);

                        }
                        else
                        {
                            getPatientDetailsResponse.Result = this.MobiusBAL.GetPatientDetails(out patient, MPIID: getPatientDetailsRequest.MPIID);
                            if (getPatientDetailsResponse.Result.IsSuccess)
                            {
                                getPatientDetailsResponse.Patient = new MobiusServiceLibrary.Patient();
                                if (patient != null)
                                {
                                    getPatientDetailsResponse.Patient.PatientId = patient.PatientId;
                                    getPatientDetailsResponse.Patient.EmailAddress = patient.EmailAddress;
                                    getPatientDetailsResponse.Patient.CommunityId = patient.CommunityId;
                                    getPatientDetailsResponse.Patient.DOB = patient.DOB;
                                    getPatientDetailsResponse.Patient.Gender = patient.Gender;
                                    getPatientDetailsResponse.Patient.LocalMPIID = getPatientDetailsRequest.MPIID;
                                    //TODOBedi
                                    //getPatientDetailsResponse.Patient.MothersMaidenName = patient.MothersMaidenName;

                                    getPatientDetailsResponse.Patient.SSN = patient.SSN;
                                    getPatientDetailsResponse.Patient.GivenName = new List<string>();
                                    foreach (string givenName in patient.GivenName)
                                    {
                                        getPatientDetailsResponse.Patient.GivenName.Add(givenName);
                                    }
                                    getPatientDetailsResponse.Patient.MiddleName = new List<string>();
                                    foreach (string middleName in patient.MiddleName)
                                    {
                                        getPatientDetailsResponse.Patient.MiddleName.Add(middleName);
                                    }
                                    getPatientDetailsResponse.Patient.FamilyName = new List<string>();
                                    foreach (string familyName in patient.FamilyName)
                                    {
                                        getPatientDetailsResponse.Patient.FamilyName.Add(familyName);
                                    }
                                    getPatientDetailsResponse.Patient.IDNames = new List<int>();
                                    foreach (int idname in patient.IDNames)
                                    {
                                        getPatientDetailsResponse.Patient.IDNames.Add(idname);
                                    }
                                    getPatientDetailsResponse.Patient.MothersMaidenName = new MobiusServiceLibrary.Name();
                                    if (patient.MothersMaidenName != null)
                                    {
                                        getPatientDetailsResponse.Patient.MothersMaidenName.FamilyName = patient.MothersMaidenName.FamilyName;
                                        getPatientDetailsResponse.Patient.MothersMaidenName.Prefix = patient.MothersMaidenName.Prefix;
                                        getPatientDetailsResponse.Patient.MothersMaidenName.Suffix = patient.MothersMaidenName.Suffix;
                                        getPatientDetailsResponse.Patient.MothersMaidenName.GivenName = patient.MothersMaidenName.GivenName;
                                        getPatientDetailsResponse.Patient.MothersMaidenName.MiddleName = patient.MothersMaidenName.MiddleName;

                                    }

                                    //Update Birth Place address

                                    getPatientDetailsResponse.Patient.BirthPlaceAddress = patient.BirthPlaceAddress;
                                    getPatientDetailsResponse.Patient.BirthPlaceCity = patient.BirthPlaceCity;
                                    getPatientDetailsResponse.Patient.BirthPlaceZip = patient.BirthPlaceZip;
                                    getPatientDetailsResponse.Patient.BirthPlaceCountry = patient.BirthPlaceCountry;
                                    getPatientDetailsResponse.Patient.BirthPlaceState = patient.BirthPlaceState;

                                    getPatientDetailsResponse.Patient.Telephones = new List<MobiusServiceLibrary.Telephone>();
                                    foreach (Mobius.Entity.Telephone telephoneEntity in patient.Telephone)
                                    {
                                        serviceTelephone = new MobiusServiceLibrary.Telephone();
                                        serviceTelephone.Id = telephoneEntity.Id;
                                        serviceTelephone.Type = telephoneEntity.Type;
                                        serviceTelephone.Extensionnumber = telephoneEntity.Extensionnumber;
                                        serviceTelephone.Number = telephoneEntity.Number;
                                        serviceTelephone.Status = telephoneEntity.Status;
                                        getPatientDetailsResponse.Patient.Telephones.Add(serviceTelephone);
                                    }
                                    getPatientDetailsResponse.Patient.PatientAddress = new List<MobiusServiceLibrary.Address>();
                                    foreach (Mobius.Entity.Address addressEntity in patient.PatientAddress)
                                    {
                                        serviceAddress = new MobiusServiceLibrary.Address();
                                        serviceAddress.Id = addressEntity.Id;
                                        serviceAddress.AddressLine1 = addressEntity.AddressLine1;
                                        serviceAddress.AddressLine2 = addressEntity.AddressLine2;
                                        serviceAddress.AddressStatus = addressEntity.AddressStatus;
                                        serviceAddress.Zip = addressEntity.Zip;

                                        serviceCountry = new MobiusServiceLibrary.Country();
                                        serviceCountry.CountryName = addressEntity.City.State.Country.CountryName;


                                        serviceState = new MobiusServiceLibrary.State();
                                        serviceState.StateName = addressEntity.City.State.StateName;
                                        serviceState.Country = serviceCountry;

                                        serviceCity = new MobiusServiceLibrary.City();
                                        serviceCity.CityName = addressEntity.City.CityName;
                                        serviceCity.State = serviceState;
                                        serviceAddress.City = serviceCity;
                                        getPatientDetailsResponse.Patient.PatientAddress.Add(serviceAddress);
                                    }
                                }
                                ResponseEncryption(getPatientDetailsResponse);
                            }
                        }
                    }
                    else
                    {
                        getPatientDetailsResponse.Result.IsSuccess = false;
                        getPatientDetailsResponse.Result.SetError(ErrorCode.Not_Valid_Request_Data);
                    }
                }
            }
            catch (Exception ex)
            {
                getPatientDetailsResponse.Result.IsSuccess = false;
                getPatientDetailsResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);

            }
            return getPatientDetailsResponse;
        }
        #endregion

        #region GetUserInformation

        /// <summary>
        /// GetUserInformation via Public Key/Serialnumber
        /// </summary>
        /// <returns>UserInformationResponse class object</returns>
        public UserInformationResponse GetUserInformation()
        {
            UserInformationResponse userInformationResponse = new UserInformationResponse();
            if (!string.IsNullOrEmpty(SerialNumber))
            {
                UserInfo userInformation = null;
                userInformationResponse.Result = this.MobiusBAL.GetUserInformation(SerialNumber, out userInformation);
                if (userInformationResponse.Result.IsSuccess)
                {
                    userInformationResponse.UserInformation = XmlSerializerHelper.DeserializeObject(XmlSerializerHelper.SerializeObject(userInformation), typeof(UserInformation)) as UserInformation;
                    ResponseEncryption(userInformationResponse);
                }

            }

            return userInformationResponse;
        }

        #endregion

        #region GetC32Sections
        /// <summary>
        /// Get C32 Sections
        /// </summary>
        /// <returns>return GetC32SectionResponse class object</returns>
        public GetC32SectionsResponse GetC32Sections()
        {
            GetC32SectionsResponse getC32SectionResponse = null;
            MobiusServiceLibrary.C32Section c32Section = null;
            List<Mobius.Entity.C32Section> objC32Sections = null;

            try
            {
                getC32SectionResponse = new GetC32SectionsResponse();
                getC32SectionResponse.Result = new Result();
                getC32SectionResponse.C32Sections = new List<MobiusServiceLibrary.C32Section>();
                getC32SectionResponse.Result = MobiusBAL.GetC32Sections(out objC32Sections);
                MobiusServiceLibrary.Component childSection = null;
                if (getC32SectionResponse.Result.IsSuccess)
                {
                    foreach (Mobius.Entity.C32Section item in objC32Sections)
                    {
                        c32Section = new MobiusServiceLibrary.C32Section();
                        c32Section.Allow = item.Allow;
                        c32Section.DisplayOrder = item.DisplayOrder;
                        c32Section.Id = item.Id;
                        c32Section.LONICCode = item.LONICCode;
                        c32Section.Name = item.Name;
                        c32Section.Optionality = item.Optionality;
                        //c32Section.Repeatable = item.Repeatable;
                        c32Section.TemplateId = item.TemplateId;

                        if (item.ChildSections != null && item.ChildSections.Count != 0)
                        {
                            c32Section.ChildSections = new List<MobiusServiceLibrary.Component>();
                            foreach (var child in item.ChildSections)
                            {
                                childSection = new MobiusServiceLibrary.Component();
                                childSection.Allow = child.Allow;
                                childSection.DisplayOrder = child.DisplayOrder;
                                childSection.Id = child.Id;
                                childSection.LONICCode = child.LONICCode;
                                childSection.Name = child.Name;
                                childSection.Optionality = child.Optionality;
                                //childSection.Repeatable = child.Repeatable;
                                childSection.TemplateId = child.TemplateId;
                                c32Section.ChildSections.Add(childSection);
                            }
                        }
                        getC32SectionResponse.C32Sections.Add(c32Section);
                    }
                    ResponseEncryption(getC32SectionResponse);
                }

                //GetC32SectionsResponse getC32SectionResponse = null;
                //MobiusServiceLibrary.C32Section c32Section = null;
                //List<Mobius.Entity.C32Section> objC32Sections = null;

                //try
                //{
                //    getC32SectionResponse = new GetC32SectionsResponse();
                //    getC32SectionResponse.Result = new Result();
                //    getC32SectionResponse.C32Sections = new List<MobiusServiceLibrary.C32Section>();
                //    getC32SectionResponse.Result = MobiusBAL.GetC32Sections(out objC32Sections);
                //    if (getC32SectionResponse.Result.IsSuccess)
                //    {
                //        foreach (Mobius.Entity.C32Section section in objC32Sections)
                //        {
                //            c32Section = new MobiusServiceLibrary.C32Section();
                //            c32Section.Id = section.ModuleId;
                //            c32Section.Name = section.ModuleName;
                //            //c32Section.ModuleValue = section.ModuleValue;
                //            c32Section.Optionality = section.Optionality;
                //            //TODO:Rajanee, somehow one property is set to boolean and other is string. so commenting the following code for now.
                //            //c32Section.Repeatable = section.Repeatable;
                //            getC32SectionResponse.C32Sections.Add(c32Section);
                //        }

                //        ResponseEncryption(getC32SectionResponse);
                //    }


            }
            catch (Exception ex)
            {
                getC32SectionResponse.Result.IsSuccess = false;
                getC32SectionResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return getC32SectionResponse;
        }
        #endregion

        #region UpdatePatientConsentPolicy
        /// <summary>
        /// Add and Update Patient Consent 
        /// </summary>
        /// <param name="updatePatientConsentPolicyRequest"></param>
        ///  <returns>return UpdatePatientConsentPolicyResponse class object</returns>
        public UpdatePatientConsentPolicyResponse UpdatePatientConsentPolicy(UpdatePatientConsentPolicyRequest updatePatientConsentPolicyRequest)
        {
            UpdatePatientConsentPolicyResponse updatePatientConsentPolicyResponse = null;
            MobiusPatientConsent patientConsentPolicy = null;
            try
            {
                updatePatientConsentPolicyResponse = new UpdatePatientConsentPolicyResponse();
                updatePatientConsentPolicyResponse.Result = new Result();

                if (updatePatientConsentPolicyRequest != null)
                {
                    if (_soapHandler.RequestDecryption(updatePatientConsentPolicyRequest.SoapProperties, updatePatientConsentPolicyRequest, PublicKey))
                    {

                        patientConsentPolicy = new MobiusPatientConsent();

                        //patientConsentPolicy.Active = updatePatientConsentPolicyRequest.IsNew;
                        patientConsentPolicy.MPIID = updatePatientConsentPolicyRequest.MPIID;
                        patientConsentPolicy.Permission = Convert.ToString(updatePatientConsentPolicyRequest.Permission);

                        //patientConsentPolicy.C32SectionId = updatePatientConsentPolicyRequest.C32SectionId;

                        if (updatePatientConsentPolicyRequest.PatientConsentPolicy != null && updatePatientConsentPolicyRequest.PatientConsentPolicy.Modules != null)
                        {
                            if (updatePatientConsentPolicyRequest.PatientConsentPolicy.Modules.Count > 0)
                            {
                                string patientConsent = XmlSerializerHelper.SerializeObject(updatePatientConsentPolicyRequest.PatientConsentPolicy);
                                if (!string.IsNullOrEmpty(patientConsent))
                                    patientConsentPolicy.PatientConsentPolicy = (Mobius.Entity.MobiusPatientConsentPolicy)XmlSerializerHelper.DeserializeObject(patientConsent, typeof(Mobius.Entity.MobiusPatientConsentPolicy));
                            }

                        }
                    }
                    patientConsentPolicy.Active = updatePatientConsentPolicyRequest.Active;
                    patientConsentPolicy.PurposeOfUseId = updatePatientConsentPolicyRequest.PurposeOfUseId;
                    patientConsentPolicy.RoleId = updatePatientConsentPolicyRequest.RoleId;
                    patientConsentPolicy.RuleEndDate = updatePatientConsentPolicyRequest.RuleEndDate;
                    patientConsentPolicy.RuleStartDate = updatePatientConsentPolicyRequest.RuleStartDate;
                    patientConsentPolicy.Allow = updatePatientConsentPolicyRequest.Allow;
                    patientConsentPolicy.PatientConsentID = updatePatientConsentPolicyRequest.PatientConsentID;

                    updatePatientConsentPolicyResponse.Result = this.MobiusBAL.UpdatePatientConsentPolicy(patientConsentPolicy);
                    updatePatientConsentPolicyResponse.ConsentId = patientConsentPolicy.PatientConsentID;
                    ResponseEncryption(updatePatientConsentPolicyRequest);
                    //ResponseEncryption(updatePatientConsentPolicyRequest);
                }

            }
            catch (Exception ex)
            {
                updatePatientConsentPolicyResponse.Result.IsSuccess = false;
                updatePatientConsentPolicyResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return updatePatientConsentPolicyResponse;
        }

        #endregion

        #region DeletePatientConsentPolicy
        /// <summary>
        /// Delete Patient Consent 
        /// </summary>
        /// <param name="DeletePatientConsentPolicyRequest"></param>
        ///  <returns>return DeletePatientConsentPolicyResponse class object</returns>
        public DeletePatientConsentPolicyResponse DeletePatientConsentPolicy(DeletePatientConsentPolicyRequest deletePatientConsentPolicyRequest)
        {
            DeletePatientConsentPolicyResponse deletePatientConsentPolicyResponse = null;
            try
            {
                deletePatientConsentPolicyResponse = new DeletePatientConsentPolicyResponse();
                deletePatientConsentPolicyResponse.Result = new Result();

                if (deletePatientConsentPolicyRequest != null)
                {
                    if (_soapHandler.RequestDecryption(deletePatientConsentPolicyRequest.SoapProperties, deletePatientConsentPolicyRequest, PublicKey))
                    {
                        deletePatientConsentPolicyResponse.Result = this.MobiusBAL.DeletePatientConsent(deletePatientConsentPolicyRequest.patientConsentId);

                        ResponseEncryption(deletePatientConsentPolicyResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                deletePatientConsentPolicyResponse.Result.IsSuccess = false;
                deletePatientConsentPolicyResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return deletePatientConsentPolicyResponse;
        }
        #endregion

        #region GetPatientConsent
        /// <summary>
        /// GetPatientConsent method retrieves consent set
        /// </summary>
        /// <param name="GetPatientConsentRequest">GetPatientConsentRequest object</param>
        /// <returns>returns getPatientConsentResponse object containing consent set</returns>
        public GetPatientConsentResponse GetPatientConsent(GetPatientConsentRequest GetPatientConsentRequest)
        {
            GetPatientConsentResponse getPatientConsentResponse = null;

            try
            {
                getPatientConsentResponse = new GetPatientConsentResponse();
                getPatientConsentResponse.Result = new Result();
                List<MobiusPatientConsent> patientConsents = null;

                if (_soapHandler.RequestDecryption(GetPatientConsentRequest.SoapProperties, GetPatientConsentRequest, PublicKey))
                {
                    getPatientConsentResponse.Result = MobiusBAL.GetPatientConsent(GetPatientConsentRequest.MPIID, out patientConsents);
                    if (getPatientConsentResponse.Result.IsSuccess)
                    {
                        getPatientConsentResponse.PatientConsents = XmlSerializerHelper.DeserializeObject(XmlSerializerHelper.SerializeObject(patientConsents), typeof(List<PatientConsent>)) as List<PatientConsent>;
                    }
                    ResponseEncryption(getPatientConsentResponse);
                }
            }
            catch (Exception ex)
            {
                getPatientConsentResponse.Result.IsSuccess = false;
                getPatientConsentResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return getPatientConsentResponse;
        }
        #endregion

        #region GetPatientConsentByConsentId
        /// <summary>
        /// GetPatientConsentByConsentId method would fetch single patient consent using MPIID and ConsentId
        /// </summary>
        /// <param name="GetPatientConsentRequest">GetPatientConsentRequest object as input parameter</param>
        /// <returns>GetPatientConsentResponse object contain single consent set</returns>
        public GetPatientConsentResponse GetPatientConsentByConsentId(GetPatientConsentRequest GetPatientConsentRequest)
        {
            GetPatientConsentResponse getPatientConsentResponse = null;


            try
            {
                getPatientConsentResponse = new GetPatientConsentResponse();
                getPatientConsentResponse.Result = new Result();

                MobiusPatientConsent patientConsent = null;
                List<Mobius.Entity.C32Section> C32Sections = null;


                if (_soapHandler.RequestDecryption(GetPatientConsentRequest.SoapProperties, GetPatientConsentRequest, PublicKey))
                {
                    getPatientConsentResponse.Result = MobiusBAL.GetPatientConsentByConsentId(GetPatientConsentRequest.MPIID, GetPatientConsentRequest.PatientConsentId,
                        out patientConsent,
                        out C32Sections);

                    if (getPatientConsentResponse.Result.IsSuccess)
                    {
                        string result = XmlSerializerHelper.SerializeObject(C32Sections);

                        getPatientConsentResponse.PatientConsents = new List<PatientConsent>();

                        PatientConsent PatientConsentByConsentId = XmlSerializerHelper.DeserializeObject(XmlSerializerHelper.SerializeObject(patientConsent), typeof(PatientConsent)) as PatientConsent;
                        getPatientConsentResponse.PatientConsents.Add(PatientConsentByConsentId);

                        //getPatientConsentResponse.PatientConsents = XmlSerializerHelper.DeserializeObject(XmlSerializerHelper.SerializeObject(patientConsent), typeof(List<PatientConsent>)) as List<PatientConsent>;

                        getPatientConsentResponse.C32Section = (List<MobiusServiceLibrary.C32Section>)XmlSerializerHelper.DeserializeObject(result, typeof(List<MobiusServiceLibrary.C32Section>));

                        if (C32Sections != null)
                            getPatientConsentResponse.C32Section = (List<MobiusServiceLibrary.C32Section>)XmlSerializerHelper.DeserializeObject(XmlSerializerHelper.SerializeObject(C32Sections), typeof(List<MobiusServiceLibrary.C32Section>));



                    }
                    ResponseEncryption(getPatientConsentResponse);
                }
            }
            catch (Exception ex)
            {
                getPatientConsentResponse.Result.IsSuccess = false;
                getPatientConsentResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return getPatientConsentResponse;
        }
        #endregion

        #region UpdateOptInStatus
        /// <summary>
        /// Update OptIn/OptOut Status
        /// </summary>
        /// <param name="UpdateOptInStatusRequest"></param>
        /// <returns></returns>
        public UpdateOptInStatusResponse UpdateOptInStatus(UpdateOptInStatusRequest updateOptInStatusRequest)
        {
            UpdateOptInStatusResponse updateOptInStatusResponse = null;
            try
            {
                updateOptInStatusResponse = new UpdateOptInStatusResponse();
                updateOptInStatusResponse.Result = new Result();

                if (updateOptInStatusRequest != null)
                {
                    if (_soapHandler.RequestDecryption(updateOptInStatusRequest.SoapProperties, updateOptInStatusRequest, PublicKey))
                    {
                        updateOptInStatusResponse.Result = MobiusBAL.UpdateOptInStatus(updateOptInStatusRequest.MPIID, updateOptInStatusRequest.isOptIn);
                        ResponseEncryption(updateOptInStatusResponse);
                    }

                }
            }
            catch (Exception ex)
            {
                updateOptInStatusResponse.Result.IsSuccess = false;
                updateOptInStatusResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return updateOptInStatusResponse;
        }
        #endregion

        #region GetPatientInformationByDocumentID

        /// <summary>
        /// Get Patient Information via DocumentID
        /// </summary>
        /// <param name="getPatientDetailsbyDocumentIdrequest"></param>
        /// <returns></returns>
        public GetPatientDetailsResponse GetPatientInformationByDocumentID(GetPatientDetailsbyDocumentIdRequest getPatientDetailsbyDocumentIdrequest)
        {
            GetPatientDetailsResponse getPatientDetailsResponse = null;
            try
            {
                getPatientDetailsResponse = new GetPatientDetailsResponse();
                getPatientDetailsResponse.Result = new Result();
                Mobius.Entity.Patient patient = null;
                if (getPatientDetailsbyDocumentIdrequest != null)
                {
                    if (_soapHandler.RequestDecryption(getPatientDetailsbyDocumentIdrequest.SoapProperties, getPatientDetailsbyDocumentIdrequest, PublicKey))
                    {
                        getPatientDetailsResponse.Result = MobiusBAL.GetPatientInformationByDocumentID(getPatientDetailsbyDocumentIdrequest.DocumentID, out patient);

                        if (getPatientDetailsResponse.Result.IsSuccess && patient != null)
                        {
                            getPatientDetailsResponse.Patient = new MobiusServiceLibrary.Patient();
                            getPatientDetailsResponse.Patient.DOB = patient.DOB;
                            getPatientDetailsResponse.Patient.Gender = patient.Gender;
                            getPatientDetailsResponse.Patient.LocalMPIID = patient.LocalMPIID;
                            getPatientDetailsResponse.Patient.GivenName = new List<string>();
                            getPatientDetailsResponse.Patient.GivenName = patient.GivenName;
                            getPatientDetailsResponse.Patient.FamilyName = new List<string>();
                            getPatientDetailsResponse.Patient.FamilyName = patient.FamilyName;

                            foreach (string middleName in patient.MiddleName)
                            {
                                getPatientDetailsResponse.Patient.MiddleName.Add(middleName);
                            }
                            getPatientDetailsResponse.Patient.FamilyName = new List<string>();
                            foreach (string familyName in patient.FamilyName)
                            {
                                getPatientDetailsResponse.Patient.FamilyName.Add(familyName);
                            }

                        }

                        ResponseEncryption(getPatientDetailsResponse);
                    }

                }
            }
            catch (Exception ex)
            {
                getPatientDetailsResponse.Result.IsSuccess = false;
                getPatientDetailsResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return getPatientDetailsResponse;
        }
        #endregion

        #region AddPFXCertificate
        /// <summary>
        ///  Insert dynamically generated PFX certificate(base64 format)
        ///  into data base Patient and provider table for activate user on new devices
        /// </summary>
        /// <param name="pFXCertificate"></param>
        /// <returns></returns>
        public AddPFXCertificateResponse AddPFXCertificate(AddPFXCertificateRequest addPFXCertificateRequest)
        {
            AddPFXCertificateResponse addPFXCertificateResponse = null;
            try
            {
                addPFXCertificateResponse = new AddPFXCertificateResponse();
                addPFXCertificateResponse.Result = new Result();
                if (addPFXCertificateRequest != null)
                {
                    if (_soapHandler.RequestDecryption(addPFXCertificateRequest.SoapProperties, addPFXCertificateRequest, PublicKey))
                    {
                        PFXCertificate pFXCertificate = new PFXCertificate();
                        pFXCertificate.UserType = addPFXCertificateRequest.UserType;
                        pFXCertificate.EmailAddress = addPFXCertificateRequest.EmailAddress;
                        pFXCertificate.Certificate = addPFXCertificateRequest.Certificate;
                        addPFXCertificateResponse.Result = MobiusBAL.AddPFXCertificate(pFXCertificate);
                        ResponseEncryption(addPFXCertificateResponse);
                    }

                }
            }
            catch (Exception ex)
            {
                addPFXCertificateResponse.Result.IsSuccess = false;
                addPFXCertificateResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return addPFXCertificateResponse;
        }
        #endregion

        #region Change Password
        /// <summary>
        /// Change User Password
        /// </summary>
        /// <param name="changePasswordRequest">Change password request object</param>
        /// <returns>returns changed status</returns>
        public ChangePasswordResponse ChangePassword(ChangePasswordRequest changePasswordRequest)
        {
            ChangePasswordResponse changePasswordResponse = null;
            try
            {
                Mobius.Entity.ChangePassword changePassword = new Mobius.Entity.ChangePassword();
                changePasswordResponse = new ChangePasswordResponse();
                changePasswordResponse.Result = new Result();
                if (_soapHandler.RequestDecryption(changePasswordRequest.SoapProperties, changePasswordRequest, PublicKey))
                {
                    changePassword.EmailAddress = changePasswordRequest.EmailAddress;
                    changePassword.NewPassword = changePasswordRequest.NewPassword;
                    changePassword.OldPassword = changePasswordRequest.OldPassword;
                    changePassword.UserType = changePasswordRequest.UserType;

                    changePasswordResponse.Result = this.MobiusBAL.ChangePassword(changePassword);
                    if (changePasswordResponse.Result.IsSuccess)
                    {
                        ResponseEncryption(changePasswordResponse);
                    }
                }
                else
                {
                    changePasswordResponse.Result.IsSuccess = false;
                    changePasswordResponse.Result.SetError(ErrorCode.Not_Valid_Request_Data);
                }
            }
            catch (Exception ex)
            {
                changePasswordResponse.Result.IsSuccess = false;
                changePasswordResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return changePasswordResponse;
        }
        #endregion

        #region GetPHISource
        /// <summary>
        /// This method will load the Patient health information sources
        /// </summary>
        /// <param name="patientCorrelationRequest"></param>
        /// <returns></returns>
        public PHISourceResponse GetPHISource(PHISourceRequest patientCorrelationRequest)
        {
            PHISourceResponse patientCorrelationResponse = null;
            try
            {
                patientCorrelationResponse = new PHISourceResponse();
                patientCorrelationResponse.Result = new Result();
                if (patientCorrelationRequest != null)
                {
                    if (_soapHandler.RequestDecryption(patientCorrelationRequest.SoapProperties, patientCorrelationRequest, PublicKey))
                    {
                        List<RemotePatientIdentifier> patientIdentifiers = null;
                        patientCorrelationResponse.Result = MobiusBAL.GetPHISource(patientCorrelationRequest.AssigningCommunityId, patientCorrelationRequest.PatientId, out patientIdentifiers);
                        if (patientCorrelationResponse.Result.IsSuccess && patientIdentifiers.Count > 0)
                        {
                            patientCorrelationResponse.PatientIdentifiers = XmlSerializerHelper.DeserializeObject(XmlSerializerHelper.SerializeObject(patientIdentifiers), typeof(List<PatientIdentifier>)) as List<PatientIdentifier>;
                        }
                        ResponseEncryption(patientCorrelationResponse);
                    }
                }

            }
            catch (Exception ex)
            {

                patientCorrelationResponse.Result.IsSuccess = false;
                patientCorrelationResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return patientCorrelationResponse;
        }

        #endregion GetPHISource

        #region EmergencyOverride

        /// <summary>
        /// To get all instances of Emergency access for a patient's docuements
        /// </summary>
        /// <param name="getEmergencyAuditRequest"></param>
        /// <returns></returns>
        public GetEmergencyAuditResponse GetAllEmergencyAudit(GetEmergencyAuditRequest getEmergencyAuditRequest)
        {
            GetEmergencyAuditResponse getEmergencyAuditResponse = new GetEmergencyAuditResponse();
            getEmergencyAuditResponse.ListEmergencyAccess = new List<EmergencyAccess>();
            List<EmergencyAudit> lstEmergencyAudit;
            try
            {
                getEmergencyAuditResponse.Result = MobiusBAL.GetAllEmergencyAudit(getEmergencyAuditRequest.EmergencyRecords, out lstEmergencyAudit, getEmergencyAuditRequest.MPIID);
                if (!getEmergencyAuditResponse.Result.IsSuccess)
                    return getEmergencyAuditResponse;


                List<EmergencyAccess> lstEmergencyAccess = XmlSerializerHelper.DeserializeObject(XmlSerializerHelper.SerializeObject(lstEmergencyAudit), typeof(List<EmergencyAccess>)) as List<EmergencyAccess>;
                getEmergencyAuditResponse.ListEmergencyAccess = lstEmergencyAccess;

            }
            catch (Exception ex)
            {
                getEmergencyAuditResponse.Result.IsSuccess = false;
                getEmergencyAuditResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }

            return getEmergencyAuditResponse;
        }

        /// <summary>
        /// To get details of an instance of Emergency access for a patient's docuements
        /// </summary>
        /// <param name="getEmergencyAuditRequest"></param>
        /// <returns></returns>
        public GetEmergencyAuditResponse GetEmergencyDetailById(GetEmergencyAuditRequest getEmergencyAuditRequest)
        {
            GetEmergencyAuditResponse getEmergencyAuditResponse = new GetEmergencyAuditResponse();
            getEmergencyAuditResponse.ListEmergencyAccess = new List<EmergencyAccess>();
            try
            {

                EmergencyAudit audit;
                getEmergencyAuditResponse.Result = MobiusBAL.GetEmergencyDetailById(getEmergencyAuditRequest.EmergencyAuditId, out audit);
                if (!getEmergencyAuditResponse.Result.IsSuccess)
                    return getEmergencyAuditResponse;


                EmergencyAccess access = XmlSerializerHelper.DeserializeObject(XmlSerializerHelper.SerializeObject(audit), typeof(EmergencyAccess)) as EmergencyAccess;
                getEmergencyAuditResponse.ListEmergencyAccess.Add(access);
            }
            catch (Exception ex)
            {
                getEmergencyAuditResponse.Result.IsSuccess = false;
                getEmergencyAuditResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }

            return getEmergencyAuditResponse;
        }

        #endregion EmergencyOverride



        #region privateMethod

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assertion"></param>
        /// <returns></returns>
        private MobiusAssertion GetAssertion(MobiusServiceLibrary.Assertion assertion)
        {
            MobiusAssertion mobiusAssertion;
            mobiusAssertion = XmlSerializerHelper.DeserializeObject(XmlSerializerHelper.SerializeObject(assertion), typeof(MobiusAssertion)) as MobiusAssertion;
            return mobiusAssertion;
        }

        /// <summary>
        /// get GetNhinCommunity
        /// </summary>
        /// <param name="communities">collection of communities class object</param>
        /// <returns>collection of NHINCommunity class object</returns>
        private List<MobiusNHINCommunity> GetNhinCommunity(List<Community> communities)
        {
            MobiusNHINCommunity NHINCommunity = null;
            List<MobiusNHINCommunity> NHINCommunities = new List<MobiusNHINCommunity>();
            foreach (Community community in communities)
            {
                NHINCommunity = new MobiusNHINCommunity();
                NHINCommunity.CommunityIdentifier = community.CommunityIdentifier;
                NHINCommunities.Add(NHINCommunity);
            }
            return NHINCommunities;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_object"></param>
        private void ResponseEncryption(object _object)
        {
            SoapProperties soapProperties = null;
            // Response Encryption
            _soapHandler.ResponseEncryption(_object, out _soapProperties, PublicKey);
            // set null SoapProperties
            var barProperty = _object.GetType().GetProperty("SoapProperties");
            barProperty.SetValue(_object, soapProperties, null);
            // assigne value to SoapProperties
            var Property = _object.GetType().GetProperty("SoapProperties");
            Property.SetValue(_object, _soapProperties, null);
        }

        #endregion
    }
    /// <summary>
    /// partial class MobiusHISE for contract IMobius
    /// </summary>
    public partial class MobiusHISE : IMobius
    {
        #region AddProvider
        /// <summary>
        /// AddProvider method would be used to register provider
        /// </summary>
        /// <param name="Provider">RegisterProviderRequest object would be input parameter</param>
        /// <returns>RegisterProviderResponse object would be returned</returns>
        public AddProviderResponse AddProvider(AddProviderRequest Provider)
        {
            AddProviderResponse providerResponse = null;
            string PKCS7Response = string.Empty;
            try
            {
                providerResponse = new AddProviderResponse();
                Mobius.Entity.Provider provider = new Entity.Provider();

                MobiusServiceLibrary.Provider providerRequest = Provider.Provider;

                if (providerRequest != null)
                {
                    if (providerRequest.City != null)
                    {
                        provider.City = new Entity.City();
                        provider.City.CityName = providerRequest.City.CityName;

                        if (providerRequest.City.State != null)
                        {
                            provider.City.State = new Entity.State();
                            provider.City.State.StateName = providerRequest.City.State.StateName;

                            if (providerRequest.City.State.Country != null)
                            {
                                provider.City.State.Country = new Entity.Country();
                                provider.City.State.Country.CountryName = providerRequest.City.State.Country.CountryName;
                            }
                        }
                    }
                    provider.ContactNumber = providerRequest.ContactNumber;
                    provider.CSR = Provider.CSR;
                    provider.ElectronicServiceURI = providerRequest.ElectronicServiceURI;
                    provider.Email = providerRequest.Email;
                    provider.FirstName = providerRequest.FirstName;
                    provider.Gender = providerRequest.Gender;
                    provider.Identifier = providerRequest.Identifier;
                    provider.IndividualProvider = providerRequest.IndividualProvider;
                    provider.Password = providerRequest.Password;
                    if (providerRequest.Language != null)
                    {
                        provider.Language = new Entity.Language();
                        provider.Language.LanguageId = providerRequest.Language.LanguageId;
                    }

                    provider.LastName = providerRequest.LastName;
                    provider.MedicalRecordsDeliveryEmailAddress = providerRequest.MedicalRecordsDeliveryEmailAddress;
                    provider.MiddleName = providerRequest.MiddleName;
                    provider.OrganizationName = providerRequest.OrganizationName;
                    provider.PostalCode = providerRequest.PostalCode;
                    provider.ProviderId = providerRequest.ProviderId;
                    provider.ProviderType = providerRequest.ProviderType;

                    if (providerRequest.Specialty != null)
                    {
                        foreach (MobiusServiceLibrary.Specialty item in providerRequest.Specialty)
                        {
                            Mobius.Entity.Specialty specialty = new Entity.Specialty();
                            specialty.SpecialtyID = item.SpecialtyID;
                            specialty.SpecialtyName = item.SpecialtyName;
                            provider.Specialty.Add(specialty);
                        }
                    }

                    provider.Status = providerRequest.Status;
                    provider.StreetName = providerRequest.StreetName;
                    provider.StreetNumber = providerRequest.StreetNumber;

                }

                providerResponse.Result = this.MobiusBAL.AddProvider(provider, out PKCS7Response);
                providerResponse.PKCS7Response = PKCS7Response;
            }
            catch (Exception ex)
            {
                providerResponse.Result.IsSuccess = false;
                providerResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);

            }
            return providerResponse;
        }
        #endregion

        #region AddPatient

        /// Add New Patient
        /// </summary>
        /// <param name="registerPatientRequest">AddPatientRequest class</param>
        /// <returns>AddPatientResponse class</returns>
        public AddPatientResponse AddPatient(AddPatientRequest registerPatientRequest)
        {
            AddPatientResponse registerPatientResponse = new AddPatientResponse();
            // Contract Variables
            try
            {
                MobiusServiceLibrary.Patient patient = null;

                List<MobiusServiceLibrary.Address> patientAddresses = null;
                string PKCS7Response = string.Empty;
                // Entity variables
                Mobius.Entity.Patient patientEntity = null;

                Mobius.Entity.Telephone telephoneEntity = null;
                Mobius.Entity.Address addressEntity = null;
                Mobius.Entity.City cityEntity = null;
                Mobius.Entity.State stateEntity = null;
                Mobius.Entity.Country countryEntity = null;

                patient = new MobiusServiceLibrary.Patient();

                patientAddresses = new List<MobiusServiceLibrary.Address>();

                patientEntity = new Mobius.Entity.Patient();

                // Single Variable Assign
                if (registerPatientRequest.Patient != null)
                {
                    patientEntity.EmailAddress = registerPatientRequest.Patient.EmailAddress;
                    patientEntity.PatientId = registerPatientRequest.Patient.PatientId;
                    patientEntity.CommunityId = registerPatientRequest.Patient.CommunityId;
                    patientEntity.CSR = registerPatientRequest.Patient.CSR;
                }

                //Convert Demographics data from contract to Entity
                if (registerPatientRequest.Patient != null)
                {
                    patient = registerPatientRequest.Patient;

                    patientEntity.DOB = patient.DOB;
                    patientEntity.Gender = patient.Gender;
                    patientEntity.LocalMPIID = patient.LocalMPIID;

                    patientEntity.MothersMaidenName.Prefix = patient.MothersMaidenName.Prefix;
                    patientEntity.MothersMaidenName.GivenName = patient.MothersMaidenName.GivenName;
                    patientEntity.MothersMaidenName.MiddleName = patient.MothersMaidenName.MiddleName;
                    patientEntity.MothersMaidenName.FamilyName = patient.MothersMaidenName.FamilyName;
                    patientEntity.MothersMaidenName.Suffix = patient.MothersMaidenName.Suffix;

                    patientEntity.SSN = patient.SSN;
                    patientEntity.Password = patient.Password;
                    patientEntity.IDNames = patient.IDNames;
                    //foreach (string familyName in patient.FamilyName)
                    //{
                    //    patientEntity.FamilyName.Add(familyName);
                    //}

                    //foreach (string middleName in patient.MiddleName)
                    //{
                    //    patientEntity.MiddleName.Add(middleName);
                    //}

                    //foreach (string givenName in patient.GivenName)
                    //{
                    //    patientEntity.GivenName.Add(givenName);
                    //}

                    patientEntity.FamilyName = patient.FamilyName;
                    patientEntity.MiddleName = patient.MiddleName;
                    patientEntity.GivenName = patient.GivenName;
                    patientEntity.Prefix = patient.Prefix;
                    patientEntity.Suffix = patient.Suffix;
                    patientEntity.Action = patient.Action;

                }

                // Convert telephone data from contract to Entity
                if (registerPatientRequest.Patient.Telephones != null)
                {
                    foreach (MobiusServiceLibrary.Telephone telephone in registerPatientRequest.Patient.Telephones)
                    {
                        telephoneEntity = new Entity.Telephone();
                        telephoneEntity.Id = telephone.Id;
                        telephoneEntity.Type = telephone.Type;
                        telephoneEntity.Extensionnumber = telephone.Extensionnumber;
                        telephoneEntity.Number = telephone.Number;
                        telephoneEntity.Status = telephone.Status;
                        telephoneEntity.Action = telephone.Action;
                        patientEntity.Telephone.Add(telephoneEntity);
                    }
                }

                // Convert Address data from contract to Entity
                if (registerPatientRequest.Patient.PatientAddress != null)
                {
                    patientAddresses = registerPatientRequest.Patient.PatientAddress;
                    foreach (MobiusServiceLibrary.Address address in patientAddresses)
                    {
                        addressEntity = new Entity.Address();

                        addressEntity.Id = address.Id;
                        addressEntity.AddressLine1 = address.AddressLine1;
                        addressEntity.AddressLine2 = address.AddressLine2;
                        addressEntity.AddressStatus = address.AddressStatus;
                        addressEntity.Zip = address.Zip;
                        addressEntity.Action = address.Action;
                        if (address.City != null)
                        {
                            cityEntity = new Mobius.Entity.City();
                            cityEntity.CityName = address.City.CityName;
                            if (address.City.State != null)
                            {
                                stateEntity = new Mobius.Entity.State();
                                stateEntity.StateName = address.City.State.StateName;
                                cityEntity.State = stateEntity;
                                if (address.City.State.Country != null)
                                {
                                    countryEntity = new Mobius.Entity.Country();
                                    countryEntity.CountryName = address.City.State.Country.CountryName;
                                    stateEntity.Country = countryEntity;
                                }
                            }
                            addressEntity.City = cityEntity;
                        }
                        patientEntity.PatientAddress.Add(addressEntity);
                    }
                }


                patientEntity.BirthPlaceAddress = registerPatientRequest.Patient.BirthPlaceAddress;
                patientEntity.BirthPlaceCity = registerPatientRequest.Patient.BirthPlaceCity;
                patientEntity.BirthPlaceState = registerPatientRequest.Patient.BirthPlaceState;
                patientEntity.BirthPlaceCountry = registerPatientRequest.Patient.BirthPlaceCountry;
                patientEntity.BirthPlaceZip = registerPatientRequest.Patient.BirthPlaceZip;

                registerPatientResponse.Result = this.MobiusBAL.AddPatient(patientEntity, out PKCS7Response);
                registerPatientResponse.PKCS7Response = PKCS7Response;
            }
            catch (Exception ex)
            {
                registerPatientResponse.Result.IsSuccess = false;
                registerPatientResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);

            }
            return registerPatientResponse;
        }
        #endregion

        #region GetMasterData
        /// <summary>
        /// This method would return the master data based on sleeted enum and dependent value 
        /// </summary>
        /// <param name="getMasterDataRequest">object of GetMasterDataRequest class</param>
        /// <returns>object of GetMasterDataResponse class</returns>
        public GetMasterDataResponse GetMasterData(GetMasterDataRequest getMasterDataRequest)
        {
            GetMasterDataResponse getMasterDataResponse = new GetMasterDataResponse();
            try
            {
                if (getMasterDataRequest != null)
                {
                    List<Entity.MasterData> masterDataList = new List<Entity.MasterData>();
                    getMasterDataResponse.Result = this.MobiusBAL.GetMasterData(getMasterDataRequest.MasterCollection, getMasterDataRequest.dependedValue, out masterDataList);
                    if (getMasterDataResponse.Result.IsSuccess)
                    {
                        MobiusServiceLibrary.MasterData masterData;
                        foreach (var item in masterDataList)
                        {
                            masterData = new MobiusServiceLibrary.MasterData();
                            masterData.Code = item.Code;
                            masterData.Description = item.Description;
                            getMasterDataResponse.MasterDataCollection.Add(masterData);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                getMasterDataResponse.Result.IsSuccess = false;
                getMasterDataResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return getMasterDataResponse;
        }
        #endregion GetMasterData

        #region GetLocalityByZipCode
        /// <summary>
        /// Get City,State,Country via zip code
        /// </summary>
        /// <param name="ZipCode">string type</param>
        /// <returns>result class object</returns>
        public GetLocalityByZipCodeResponse GetLocalityByZipCode(string zipCode)
        {
            GetLocalityByZipCodeResponse getLocalityByZipCodeResponse = null;
            MobiusServiceLibrary.City serviceCity = null;
            try
            {
                Mobius.Entity.City cityEntity = null;
                getLocalityByZipCodeResponse = new GetLocalityByZipCodeResponse();
                getLocalityByZipCodeResponse.Result = this.MobiusBAL.GetLocalityByZipCode(zipCode, out cityEntity);

                if (getLocalityByZipCodeResponse.Result.IsSuccess)
                {
                    serviceCity = new MobiusServiceLibrary.City();
                    serviceCity.CityName = cityEntity.CityName;
                    serviceCity.State = new MobiusServiceLibrary.State();
                    serviceCity.State.StateName = cityEntity.State.StateName;
                    serviceCity.State.Country = new MobiusServiceLibrary.Country();
                    serviceCity.State.Country.CountryName = cityEntity.State.Country.CountryName;
                    getLocalityByZipCodeResponse.City = serviceCity;
                }
            }
            catch (Exception ex)
            {
                getLocalityByZipCodeResponse.Result.IsSuccess = false;
                getLocalityByZipCodeResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);

            }
            return getLocalityByZipCodeResponse;
        }
        #endregion

        #region AuthenticateUser
        /// <summary>
        /// Authenticate User using EmailAddress
        /// </summary>
        /// <param name="authenticateUserRequest"></param>
        /// <returns></returns>
        public AuthenticateUserResponse AuthenticateUser(AuthenticateUserRequest authenticateUserRequest)
        {
            AuthenticateUserResponse authenticateUserResponse = null;
            string certificateSerialNumber = string.Empty;
            string userName = string.Empty;
            try
            {
                authenticateUserResponse = new AuthenticateUserResponse();
                authenticateUserResponse.Result = new Result();

                if (authenticateUserRequest != null)
                {
                    authenticateUserResponse.Result = this.MobiusBAL.AuthenticateUser(authenticateUserRequest.EmailAddress, authenticateUserRequest.Password, authenticateUserRequest.UserType.GetHashCode(), out certificateSerialNumber, out userName);

                    if (authenticateUserResponse.Result.IsSuccess)
                    {
                        authenticateUserResponse.CertificateSerialNumber = certificateSerialNumber;
                        authenticateUserResponse.Name = userName;
                    }
                }
            }
            catch (Exception ex)
            {
                authenticateUserResponse.Result.IsSuccess = false;
                authenticateUserResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);

            }
            return authenticateUserResponse;
        }
        #endregion

        #region ForgotPassword
        /// <summary>
        /// This method will check user Type and verifies if exists in database then generate new password and send mail to requested user.
        /// </summary>
        /// <param name="forgotPasswordRequest"></param>
        /// <returns></returns>
        public ForgotPasswordResponse ForgotPassword(ForgotPasswordRequest forgotPasswordRequest)
        {
            ForgotPasswordResponse forgotPasswordResponse = null;
            try
            {
                forgotPasswordResponse = new ForgotPasswordResponse();
                forgotPasswordResponse.Result = new Result();
                ForgotPassword forgotPassword = new MobiusEntity.ForgotPassword();
                forgotPassword.EmailAddress = forgotPasswordRequest.EmailAddress;
                forgotPassword.UserType = forgotPasswordRequest.UserType;
                forgotPasswordResponse.Result = this.MobiusBAL.ForgotPassword(forgotPassword);

            }
            catch (Exception ex)
            {
                forgotPasswordResponse.Result.IsSuccess = false;
                forgotPasswordResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);

            }

            return forgotPasswordResponse;
        }
        #endregion ForgotPassword

        #region GetPFXCertificate
        /// <summary>
        /// Get PFXCertificate from  data base Patient and provider table
        /// To activate user on another device.
        /// </summary>
        /// <param name="getPFXCertificateRequest"></param>
        /// <returns></returns>
        public GetPFXCertificateResponse GetPFXCertificate(GetPFXCertificateRequest getPFXCertificateRequest)
        {
            GetPFXCertificateResponse getPFXCertificateResponse = null;
            try
            {
                getPFXCertificateResponse = new GetPFXCertificateResponse();
                getPFXCertificateResponse.Result = new Result();
                if (getPFXCertificateRequest != null)
                {
                    PFXCertificate pFXCertificate = new PFXCertificate();
                    pFXCertificate.UserType = getPFXCertificateRequest.UserType;
                    pFXCertificate.EmailAddress = getPFXCertificateRequest.EmailAddress;
                    getPFXCertificateResponse.Result = MobiusBAL.GetPFXCertificate(ref pFXCertificate);
                    if (getPFXCertificateResponse.Result.IsSuccess)
                    {
                        getPFXCertificateResponse.PFXCertificate = pFXCertificate.Certificate;
                    }



                }
            }
            catch (Exception ex)
            {
                getPFXCertificateResponse.Result.IsSuccess = false;
                getPFXCertificateResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return getPFXCertificateResponse;
        }
        #endregion

        #region ActivateUser
        /// <summary>
        /// get base64format PFX certificate from database to activate user on other device
        /// </summary>
        /// <param name="ActivateUserRequest"></param>
        /// <returns></returns>
        public ActivateUserResponse ActivateUser(ActivateUserRequest activateUserRequest)
        {
            ActivateUserResponse activateUserResponse = null;
            string PKCS7Response = string.Empty;
            try
            {
                activateUserResponse = new ActivateUserResponse();
                activateUserResponse.Result = new Result();
                if (activateUserRequest != null)
                {
                    activateUserResponse.Result = MobiusBAL.ActivateUser(activateUserRequest.UserType, activateUserRequest.EmailAddress, activateUserRequest.CSR, out PKCS7Response);

                    if (activateUserResponse.Result.IsSuccess)
                    {
                        activateUserResponse.PKCS7Response = PKCS7Response;
                    }
                }
            }
            catch (Exception ex)
            {
                activateUserResponse.Result.IsSuccess = false;
                activateUserResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return activateUserResponse;
        }
        #endregion

        #region UpgradeUser
        /// <summary>
        /// get base64format PFX certificate from database to Upgrade user 
        /// </summary>
        /// <param name="ActivateUserRequest"></param>
        /// <returns></returns>
        public UpgradeUserResponse UpgradeUser(UpgradeUserRequest upgradeUserRequest)
        {
            UpgradeUserResponse UpgradeUserResponse = null;
            string SerialNumber = string.Empty;
            string Pkcs7Response = string.Empty;
            try
            {
                UpgradeUserResponse = new UpgradeUserResponse();
                UpgradeUserResponse.Result = new Result();
                if (upgradeUserRequest != null)
                {
                    UpgradeUserResponse.Result = MobiusBAL.UpgradeUser(upgradeUserRequest.UserType, upgradeUserRequest.EmailAddress, upgradeUserRequest.Password, upgradeUserRequest.PKCS7Request, out SerialNumber, out Pkcs7Response);
                    UpgradeUserResponse.NewSerialNumber = SerialNumber;
                    UpgradeUserResponse.PKCS7Response = Pkcs7Response;
                }
            }
            catch (Exception ex)
            {
                UpgradeUserResponse.Result.IsSuccess = false;
                UpgradeUserResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return UpgradeUserResponse;
        }
        #endregion

        #region GetCSRDetails
        /// <summary>
        /// Get patient and provider information to generate CSR(certificate signing request)
        /// </summary>
        /// <param name="getCSRRequest"></param>
        /// <returns></returns>
        public GetCSRResponse GetCSRDetails(GetCSRRequest getCSRRequest)
        {
            GetCSRResponse getCSRResponse = null;
            Mobius.Entity.Patient patient = null;
            Mobius.Entity.Provider provider = null;
            try
            {
                getCSRResponse = new GetCSRResponse();
                getCSRResponse.Result = new Result();

                if (getCSRRequest != null)
                {
                    if (getCSRRequest.UserType.GetHashCode() == 1)
                    {
                        if (string.IsNullOrWhiteSpace(getCSRRequest.EmailAddress))
                        {
                            getCSRResponse.Result.IsSuccess = false;
                            getCSRResponse.Result.SetError(ErrorCode.EmailAddress_Missing);
                        }
                        else
                        {
                            getCSRResponse.Result = MobiusBAL.GetPatientDetails(out patient, emailAddress: getCSRRequest.EmailAddress);
                            if (getCSRResponse.Result.IsSuccess)
                            {

                                if (patient != null)
                                {
                                    getCSRResponse.IsIndividualProvider = true;
                                    foreach (string givenName in patient.GivenName)
                                    {
                                        getCSRResponse.GivenName = givenName;
                                    }
                                    foreach (string familyName in patient.FamilyName)
                                    {
                                        getCSRResponse.FamilyName = familyName;
                                    }
                                    getCSRResponse.EmailAddress = patient.EmailAddress;


                                    foreach (Mobius.Entity.Address addressEntity in patient.PatientAddress)
                                    {
                                        getCSRResponse.Country = addressEntity.City.State.Country.CountryName;
                                        getCSRResponse.State = addressEntity.City.State.StateName; ;
                                        getCSRResponse.City = addressEntity.City.CityName;
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        getCSRResponse.Result = MobiusBAL.GetProviderDetails(getCSRRequest.EmailAddress, out provider);

                        if (getCSRResponse.Result.IsSuccess)
                        {
                            if (provider != null)
                            {

                                getCSRResponse.GivenName = provider.FirstName;
                                getCSRResponse.FamilyName = provider.LastName;
                                getCSRResponse.OrganizationName = provider.OrganizationName;
                                getCSRResponse.EmailAddress = provider.MedicalRecordsDeliveryEmailAddress;
                                getCSRResponse.IsIndividualProvider = provider.IndividualProvider;

                                if (provider.City != null)
                                {
                                    getCSRResponse.City = provider.City.CityName;
                                    getCSRResponse.State = provider.City.State.StateName;
                                    getCSRResponse.Country = provider.City.State.Country.CountryName;
                                }
                            }
                        }
                    }



                }
            }
            catch (Exception ex)
            {
                getCSRResponse.Result.IsSuccess = false;
                getCSRResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return getCSRResponse;
        }
        #endregion

        #region GetApplicationVersion
        /// <summary>
        /// This method would return the android application version
        /// </summary>
        /// <returns></returns>
        public string GetApplicationVersion()
        {
            string applicationVersion = string.Empty;
            try
            {
                applicationVersion = this.MobiusBAL.GetApplicationVersion();

            }
            catch (Exception ex)
            {
                applicationVersion = string.Empty;
            }
            return applicationVersion;
        }
        #endregion

        #region NIST - GetAvailableValidations
        public GetAvailableValidationsResponse GetAvailableValidations()
        {
            GetAvailableValidationsResponse getavailableValidationsResponse = new GetAvailableValidationsResponse();
            List<MobiusAvailableValidations> availableValidations = null;
            try
            {
                getavailableValidationsResponse.Result = this.MobiusBAL.GetAvailableValidations(out availableValidations);
                if (getavailableValidationsResponse.Result.IsSuccess)
                {
                    getavailableValidationsResponse.AvailableValidations = XmlSerializerHelper.DeserializeObject(XmlSerializerHelper.SerializeObject(availableValidations), typeof(List<AvailableValidations>)) as List<AvailableValidations>;
                }
            }
            catch (Exception ex)
            {
                getavailableValidationsResponse.Result.IsSuccess = false;
                getavailableValidationsResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return getavailableValidationsResponse;
        }

        public ValidateDocumentResponse ValidateDocument(ValidateDocumentRequest validateDocumentRequest)
        {
            MobiusValidationResults validationResults = null;
            ValidateDocumentResponse validateDocumentResponse = new ValidateDocumentResponse();
            string specificationID = string.Empty;
            string document = string.Empty;

            try
            {
                specificationID = validateDocumentRequest.SpecificationId;
                document = validateDocumentRequest.Document;

                validateDocumentResponse.Result = this.MobiusBAL.ValidateDocument(specificationID, document, validateDocumentRequest.ValidationType, out validationResults);

                if (validateDocumentResponse.Result.IsSuccess)
                {
                    validateDocumentResponse.ValidationResult = XmlSerializerHelper.DeserializeObject(XmlSerializerHelper.SerializeObject(validationResults), typeof(ValidationResults)) as ValidationResults;
                }

            }
            catch (Exception ex)
            {
                validateDocumentResponse.Result.IsSuccess = false;
                validateDocumentResponse.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return validateDocumentResponse;

        }
        #endregion NIST - GetAvailableValidations
    }


}

