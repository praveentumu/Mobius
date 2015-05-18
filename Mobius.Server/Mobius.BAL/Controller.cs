namespace Mobius.BAL
{
    #region Namespace
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Transactions;
    using System.Xml;
    using Authorization;
    using C32Utility;
    using FirstGenesis.Mobius.Server.MobiusHISEService.XACML;
    using Mobius.BAL.Interface;
    using Mobius.CertificateAuthority;
    using Mobius.Client;
    using Mobius.CoreLibrary;
    using Mobius.DAL;
    using Mobius.Entity;
    using Mobius.EventNotification;
    using Mobius.FileSystem;
    using PatientDiscovery;
    using PolicyEngine;

    #endregion

    /// <summary>
    ///  class MobiusBAL
    /// </summary>
    public partial class MobiusBAL : IMobiusBAL
    {
        #region private variables
        private XACMLHandler _XACMLHandler = null;
        private MobiusConnect _mobiusConnect;
        private MobiusDAL _mobiusDAL = null;
        private NISTValidation _NISTValidation = null;
        private Result _Result = null;
        private List<MobiusNHINCommunity> _HomeCommunity = null;
        private const string _RandomChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private XACMLClass _XACMLObject = null;
        private List<MobiusNHINCommunity> _NHINCommunities = null;
        private const string SUMMARY_PREFIX = "Summary Note:<< ";
        private const string SUMMARY_SUFFIX = " >>";
        private const string C32ValidationSpecificationID = "c32_v_2_5_c83_2_0";
        private string certifcateSerialNumber = string.Empty;

        private const string UPGRADEACCOUNT = "UPGRADEACCOUNT";
        private const string REGISTER = "REGISTER";
        private string[] format = { "MM/dd/yyyy", "M/dd/yyyy", "M/d/yyyy", "MM/d/yyyy", "MM/dd/yy", "M/d/yy", "MM/d/yy", "M/dd/yy" };

        string CreatedOn = string.Empty;
        string ExpiryOn = string.Empty;
        protected const string NO = "No";
        #endregion private variables

        #region constructor
        public MobiusBAL()
        { }

        public MobiusBAL(string certifcateSerialNumber)
        {
            this.certifcateSerialNumber = certifcateSerialNumber;
        }
        #endregion

        #region destructor
        ~MobiusBAL()
        {
            _mobiusConnect = null;
        }
        #endregion

        #region Private Properties

        private Assertion Assertion { get; set; }

        private MobiusConnect MobiusConnect
        {
            get
            {
                return _mobiusConnect != null ? _mobiusConnect : _mobiusConnect = new MobiusConnect();
            }
        }

        private string PurposeOfUse
        {
            get;
            set;
        }

        private Result Result
        {
            get
            {
                return _Result != null ? _Result : _Result = new Result();
            }
            set
            {
                _Result = value;
            }
        }


        private XACMLHandler XACMLHandler
        {
            get
            {
                return _XACMLHandler != null ? _XACMLHandler : _XACMLHandler = new XACMLHandler();
            }
        }


        private MobiusDAL MobiusDAL
        {
            get
            {
                return _mobiusDAL != null ? _mobiusDAL : _mobiusDAL = new MobiusDAL();
            }
        }

        private XACMLClass XACML
        {
            get
            {
                return _XACMLObject != null ? _XACMLObject : _XACMLObject = new XACMLClass();
            }
            set
            {
                _XACMLObject = value;
            }
        }


        private List<MobiusNHINCommunity> Communities
        {
            get
            {
                return _NHINCommunities != null ? _NHINCommunities : _NHINCommunities = new List<MobiusNHINCommunity>();
            }
            set
            {
                _NHINCommunities = value;
            }
        }

        private List<MobiusNHINCommunity> HomeCommunities
        {
            get
            {
                if (_HomeCommunity == null)
                {
                    List<MobiusNHINCommunity> communityList;
                    if (GetNhinCommunity(out communityList).IsSuccess)
                        ValidateNhinCommunities(new List<MobiusNHINCommunity>(), communityList);

                }
                return _HomeCommunity;
            }
            set
            {
                _HomeCommunity = value;
            }
        }
        private NISTValidation NISTValidation
        {
            get
            {
                return _NISTValidation != null ? _NISTValidation : _NISTValidation = new NISTValidation();
            }
        }

        #endregion Private Properties

        #region Methods Making Gateway Calls

        #region SearchPatient

        /// <summary>
        /// SearchPatient
        /// </summary>
        /// <param name="Demographics">Demographics class object</param>        
        /// <returns>return Result class object</returns>
        public Result SearchPatient(Demographics demographics, List<MobiusNHINCommunity> NHINCommunities, MobiusAssertion mobiusAssertion, out List<Patient> patients)
        {
            patients = new List<Patient>();
            List<MobiusNHINCommunity> validNHINCommunities = null;
            List<Patient> localpatients = new List<Patient>();
            DateTime date;
            try
            {

                this.Result.IsSuccess = false;
                if (NHINCommunities == null || NHINCommunities.Count == 0)
                {
                    this.Result.SetError(ErrorCode.SearchPatient_NHINCommunities_Missing);
                    return this.Result;
                }
                if (demographics == null)
                {
                    this.Result.SetError(ErrorCode.SearchPatient_Invalid_Demographics);
                    return this.Result;
                }
                if (string.IsNullOrEmpty(demographics.GivenName))
                {
                    this.Result.SetError(ErrorCode.SearchPatient_Invalid_Demographics_GivenName);

                    return this.Result;
                }
                if (string.IsNullOrEmpty(demographics.FamilyName))
                {
                    this.Result.SetError(ErrorCode.SearchPatient_Invalid_Demographics_FamilyName);

                    return this.Result;
                }

                if (string.IsNullOrEmpty(demographics.DOB))
                {
                    this.Result.SetError(ErrorCode.DOB_Not_Provided);
                    return this.Result;
                }

                if (!DateTime.TryParseExact(demographics.DOB, format, null, DateTimeStyles.None, out date))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Invalid_Date_Format);
                    return this.Result;
                }
                //if (demographics.Gender)
                //{
                //    this.Result.SetError(ErrorCode.SearchPatient_Invalid_Demographics_Gender);

                //    return this.Result;
                //}
                if (GetNhinCommunity(out validNHINCommunities).IsSuccess)
                {
                    if (!ValidateNhinCommunities(NHINCommunities, validNHINCommunities).IsSuccess)
                    {
                        return this.Result;
                    }
                }

                this.Result = GetAssertion(mobiusAssertion, AssertionAction.PatientDiscovery, demographics.LocalMPIID);
                if (!this.Result.IsSuccess)
                {
                    return this.Result;
                }


                // if Home community is exist in search
                IEnumerable<MobiusNHINCommunity> NHINHomeCommunities = this.Communities.Where(t => t.IsHomeCommunity == true);

                if (string.IsNullOrEmpty(demographics.LocalMPIID) || NHINHomeCommunities.Count() > 0)
                {
                    demographics.DOB = dateFormatter(demographics.DOB);
                    this.Result = this.MobiusDAL.SearchPatient(demographics, out patients);

                    if (patients.Count == 1)
                    {
                        demographics.LocalMPIID = patients[0].LocalMPIID;
                        //Commented By Rajanee on 29th Jan 2014, for code flow crashing for requests coming from Android app.
                        //Probabaly this section is not needed at all.
                        //UpdateAssertion(mobiusAssertion, demographics.LocalMPIID);

                    }
                    else if (patients.Count > 1)
                    {
                        this.Result = this.SpecialHandlingResponseValidation(patients, demographics);
                    }
                }


                if (NHINHomeCommunities.Count() > 0)
                {
                    //Add object only if search has selected home community
                    foreach (Patient item in patients)
                    {
                        localpatients.Add(item);
                    }
                }

                IEnumerable<MobiusNHINCommunity> NHINCommunitiesFilter = this.Communities.Where(t => t.IsHomeCommunity == false);

                if (NHINCommunitiesFilter.Count() > 0 && !string.IsNullOrEmpty(demographics.LocalMPIID))
                {

                    this.Result = this.MobiusConnect.SearchPatient(demographics, this.Communities, this.Assertion.PatientDiscoveryAssertion, out patients);

                    if (!this.Result.IsSuccess)
                    {

                        // re-assigned Local patients to patient collection
                        if (patients != null && patients.Count > 0)
                        {
                            patients.Union(localpatients);
                        }
                        else
                        {
                            patients = localpatients;
                        }
                    }

                }

                if (this.Result.IsSuccess)
                {
                    var patientList = patients.Join(this.Communities, patient => patient.CommunityId, i => i.CommunityIdentifier
                                           , (patient, i) => new { patient, i.CommunityDescription });

                    patients = new List<Patient>();
                    foreach (var item in patientList)
                    {
                        item.patient.CommunityId = item.CommunityDescription;
                        patients.Add(item.patient);
                    }


                    if (patients.Count == 0)
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.No_Result_Found);
                        return this.Result;
                    }
                }

                this.AddLogEvent(this.Result.IsSuccess ? EventType.PatientDiscovery : EventType.PatientDiscoveryFailed,
                                    0,
                                    true,
                                    demographics.LocalMPIID,
                                    string.Empty,
                                    string.Empty,
                                    string.Empty,
                                    string.Empty,
                                    string.Empty,
                                    Helper.GetIPAddress(),
                                    2,
                                    MobiusAppSettingReader.LocalHomeCommunityID,
                                    string.Empty,
                                    (object)demographics,
                                    this.Result.IsSuccess ? string.Empty : this.Result.ErrorMessage,
                                    purpose: mobiusAssertion.PurposeOfUse.ToString(),
                                    subject: mobiusAssertion.UserInformation.Role.ToString(),
                                    UserName: mobiusAssertion.UserInformation.UserName
                                    );

            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);

                this.AddLogEvent(EventType.PatientDiscoveryFailed,
                                    0,
                                    true,
                                    demographics.LocalMPIID,
                                    string.Empty,
                                    string.Empty,
                                    string.Empty,
                                    string.Empty,
                                    string.Empty,
                                    Helper.GetIPAddress(),
                                    2,
                                    MobiusAppSettingReader.LocalHomeCommunityID,
                                    string.Empty,
                                    (object)demographics,
                                    this.Result.IsSuccess ? string.Empty : this.Result.ErrorMessage,
                                    purpose: mobiusAssertion.PurposeOfUse.ToString(),
                                    subject: mobiusAssertion.UserInformation.Role.ToString(),
                                    UserName: mobiusAssertion.UserInformation.UserName);

            }
            return this.Result;
        }

        private Result GetAssertion(MobiusAssertion mobiusAssertion, AssertionAction action, string patientId)
        {
            if (mobiusAssertion == null)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.Assertion_Object_Invalid);
                return this.Result;
            }

            if (mobiusAssertion.AssertionMode == AssertionMode.Custom)
                this.Assertion = new Assertion(mobiusAssertion);
            else
            {
                if (mobiusAssertion.UserInformation == null)
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Assertion_Required_Information_Missing);
                    return this.Result;
                }
                if (mobiusAssertion.UserInformation.HomeCommunity == null || string.IsNullOrEmpty(mobiusAssertion.UserInformation.HomeCommunity.CommunityIdentifier))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Assertion_Required_Information_Missing);
                    return this.Result;
                }
                if (mobiusAssertion.UserInformation.Role == null)
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Assertion_Required_Information_Missing);
                    return this.Result;
                }
                if (mobiusAssertion.UserInformation.UserName == null)
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Assertion_Required_Information_Missing);
                    return this.Result;
                }

                if (mobiusAssertion.UserInformation.Name == null ||
                    string.IsNullOrEmpty(mobiusAssertion.UserInformation.Name.GivenName))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Assertion_Required_Information_Missing);
                    return this.Result;
                }


                //    UserInfo userInfo = this.MobiusDAL.GetUserInformation(this.certifcateSerialNumber);


                this.Assertion = new Assertion(mobiusAssertion, action, patientId, this.HomeCommunities.FirstOrDefault());
            }
            this.Result.IsSuccess = true;
            return this.Result;
        }

        private Result UpdateAssertion(MobiusAssertion mobiusAssertion, string patientId)
        {
            if (mobiusAssertion == null)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.Assertion_Object_Invalid);
                return this.Result;
            }

            Helper.LogError("mobiusAssertion is not null.");

            if (this.Assertion.PatientDiscoveryAssertion != null && mobiusAssertion.HomeCommunityId != null)
            {
                Helper.LogError("GetHL7EncodePatientId:  " + Assertion.GetHL7EncodePatientId(patientId, mobiusAssertion.HomeCommunityId.CommunityIdentifier));
                this.Assertion.PatientDiscoveryAssertion.uniquePatientId[0] =
                Assertion.GetHL7EncodePatientId(mobiusAssertion.PatientId, mobiusAssertion.HomeCommunityId.CommunityIdentifier);
            }
            else if (this.Assertion.PatientDiscoveryAssertion == null)
                Helper.LogError("this.Assertion.PatientDiscoveryAssertion IS NULL ");
            else if (mobiusAssertion.HomeCommunityId == null)
                Helper.LogError("mobiusAssertion.HomeCommunityId IS NULL ");

            this.Result.IsSuccess = true;
            return this.Result;
        }
        /// <summary>
        /// FindCandidates
        /// </summary>
        /// <param name="reqType">reqType class object</param>
        /// <returns>return Community_PRPA_IN201306UV02ResponseType class object</returns>
        public Community_PRPA_IN201306UV02ResponseType FindCandidates(RespondingGateway_PRPA_IN201305UV02RequestType RespondingGateway_PRPA_IN201305UV02Request)
        {
            Community_PRPA_IN201306UV02ResponseType community_PRPA_IN201306UV02ResponseType = null;
            List<Patient> patients = new List<Patient>();
            Demographics demographics = new Demographics();
            AdapterComponentBuildMessage adapterComponentBuildMessage = new AdapterComponentBuildMessage();

            string communityId = string.Empty;

            try
            {
                if (RespondingGateway_PRPA_IN201305UV02Request != null)
                {
                    demographics = this.ParseRespondingGateway_PRPA_IN201305UV02RequestType(RespondingGateway_PRPA_IN201305UV02Request);
                    this.Result = this.MobiusDAL.SearchPatient(demographics, out patients);

                    if (this.Result.IsSuccess)
                    {
                        if (patients.Count > 1)
                            this.Result = this.SpecialHandlingResponseValidation(patients, demographics);

                        if (this.Result.IsSuccess)
                        {
                            community_PRPA_IN201306UV02ResponseType = new Community_PRPA_IN201306UV02ResponseType();
                            community_PRPA_IN201306UV02ResponseType.PRPA_IN201306UV02 = new PRPA_IN201306UV02();
                            community_PRPA_IN201306UV02ResponseType.PRPA_IN201306UV02 = adapterComponentBuildMessage.BuildMessageFromMpiPatient(patients, RespondingGateway_PRPA_IN201305UV02Request.PRPA_IN201305UV02);
                        }

                    }
                    if (!this.Result.IsSuccess)
                    {
                        community_PRPA_IN201306UV02ResponseType = new Community_PRPA_IN201306UV02ResponseType();
                        community_PRPA_IN201306UV02ResponseType.PRPA_IN201306UV02 = adapterComponentBuildMessage.BuildMessageForErrorConditions(this.Result, RespondingGateway_PRPA_IN201305UV02Request.PRPA_IN201305UV02);

                    }
                }
            }
            catch (Exception)
            {
                community_PRPA_IN201306UV02ResponseType = new Community_PRPA_IN201306UV02ResponseType();
                community_PRPA_IN201306UV02ResponseType.PRPA_IN201306UV02 = new PRPA_IN201306UV02();
                community_PRPA_IN201306UV02ResponseType.PRPA_IN201306UV02.controlActProcess = new PRPA_IN201306UV02MFMI_MT700711UV01ControlActProcess();
                community_PRPA_IN201306UV02ResponseType.PRPA_IN201306UV02.controlActProcess.subject = new PRPA_IN201306UV02MFMI_MT700711UV01Subject1[1] { new PRPA_IN201306UV02MFMI_MT700711UV01Subject1() };
                community_PRPA_IN201306UV02ResponseType.PRPA_IN201306UV02.controlActProcess.subject[0].registrationEvent = new PRPA_IN201306UV02MFMI_MT700711UV01RegistrationEvent();
                community_PRPA_IN201306UV02ResponseType.PRPA_IN201306UV02.controlActProcess.subject[0].registrationEvent.subject1 = new PRPA_IN201306UV02MFMI_MT700711UV01Subject2();
                community_PRPA_IN201306UV02ResponseType.PRPA_IN201306UV02.controlActProcess.subject[0].registrationEvent.subject1.patient = new PRPA_MT201310UV02Patient();
                community_PRPA_IN201306UV02ResponseType.PRPA_IN201306UV02.controlActProcess.subject[0].registrationEvent.subject1.patient.id = new II[1] { new II() };
                community_PRPA_IN201306UV02ResponseType.PRPA_IN201306UV02.controlActProcess.subject[0].registrationEvent.subject1.patient.Item = new PRPA_MT201310UV02Person();

            }
            return community_PRPA_IN201306UV02ResponseType;
        }



        #endregion SearchPatient

        #region GetDocumentMetadata
        /// <summary>
        ///  Get DocumentMetadata
        /// </summary>
        /// <param name="patientId">string</param>
        /// <param name="NHINCommunities">collection of NHINCommunity class object</param>
        /// <param name="getLocalDocument">Load document from local database/Call gateway</param>
        /// <param name="documents">document class object</param>
        /// <returns>return Result class object</returns>
        public Result GetDocumentMetadata(string patientId, List<MobiusNHINCommunity> NHINCommunities, MobiusAssertion mobiusAssertion, bool getLocalDocument, out List<MobiusDocument> documents)
        {
            documents = null;
            List<MobiusDocument> remoteDocuments = null;
            List<MobiusDocument> documentsWithOutXACML = null;
            List<MobiusDocument> documentsWithXACML = null;
            List<MobiusNHINCommunity> validNHINCommunities = null;
            DocumentMetadata documentMetadata = null;
            string documentXACMLId = string.Empty;

            try
            {

                if (string.IsNullOrEmpty(patientId))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Document_PatientId_Missing);
                    return this.Result;
                }
                if (NHINCommunities.Count == 0)
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Document_communityId_Missing);
                    return this.Result;
                }
                if (GetNhinCommunity(out validNHINCommunities).IsSuccess)
                {
                    if (!ValidateNhinCommunities(NHINCommunities, validNHINCommunities).IsSuccess)
                    {
                        return this.Result;
                    }
                }
                this.Result = GetAssertion(mobiusAssertion, AssertionAction.DocumentQuery, patientId);
                if (!this.Result.IsSuccess)
                {
                    return this.Result;
                }

                if (this.Result.IsSuccess)
                {
                    // If homeCommunityId being passed is home community  than save document at local share location(No call the gateway to upload document)


                    if (!getLocalDocument)
                    {
                        this.Result = this.MobiusConnect.GetDocumentMetadata(patientId, this.Communities, this.Assertion.DocumentQueryAssertion, out remoteDocuments);
                        if (this.Result.IsSuccess)
                        {
                            documentsWithXACML = remoteDocuments.Where(t => t.DocumentUniqueId.Contains("XACML")).ToList();
                            documentsWithOutXACML = remoteDocuments.Where(t => !t.DocumentUniqueId.Contains("XACML")).ToList();

                            foreach (MobiusDocument document in documentsWithOutXACML)
                            {
                                documentMetadata = new DocumentMetadata();
                                documentXACMLId = string.Empty;

                                List<MobiusDocument> associatedXACMLID = documentsWithXACML.Where(t => t.DocumentUniqueId == (document.DocumentUniqueId + "XACML")).ToList();

                                if (associatedXACMLID.Count() > 0)
                                {
                                    documentXACMLId = ((MobiusDocument)associatedXACMLID.FirstOrDefault()).DocumentUniqueId;
                                }

                                documentMetadata.OriginalDocumentId = document.DocumentUniqueId;
                                documentMetadata.PatientId = patientId;
                                documentMetadata.DocumentTitle = document.DocumentTitle;
                                documentMetadata.Author = document.Author;
                                documentMetadata.CreatedDate = document.CreatedOn;
                                documentMetadata.SourceCommunityId = document.SourceCommunityId;
                                documentMetadata.SourceRepositryId = document.RepositoryUniqueId;
                                documentMetadata.FileName = document.DocumentUniqueId + ".xml";
                                documentMetadata.DocumentSource = document.DataSource;

                                documentMetadata.XacmlDocumentId = documentXACMLId;
                                documentMetadata.Reposed = false;
                                documentMetadata.FacilityId = string.Empty;
                                documentMetadata.UploadedBy = string.Empty;

                                this.MobiusDAL.SaveC32DocumentMetaData(documentMetadata);

                            }
                        }

                    }

                    if (this.Result.IsSuccess)
                    {
                        string CommunityIdentifiers = string.Join(",", ((List<MobiusNHINCommunity>)this.Communities).ConvertAll<string>(d => d.CommunityIdentifier));


                        this.Result = this.MobiusDAL.GetDocumentMetaData(patientId, CommunityIdentifiers, out documents);

                        if (documents.Count() > 0)
                        {
                            this.Result.IsSuccess = true;
                        }
                        else
                        {
                            this.Result.IsSuccess = false;
                            this.Result.SetError(ErrorCode.NODOCUMENTFOUND);

                        }

                    }
                }

                this.AddLogEvent(this.Result.IsSuccess ? EventType.DocumentQuery : EventType.DocumentQueryFailed,
                                    0,
                                    true,
                                    patientId,
                                    string.Empty,
                                    string.Empty,
                                    string.Empty,
                                    string.Empty,
                                    string.Empty,
                                    Helper.GetIPAddress(),
                                    2,
                                    MobiusAppSettingReader.LocalHomeCommunityID,
                                    string.Empty,
                                    (object)documents,
                                    this.Result.IsSuccess ? string.Empty : this.Result.ErrorMessage,
                                    purpose: mobiusAssertion.PurposeOfUse.ToString(),
                                    subject: mobiusAssertion.UserInformation.Role.ToString(),
                                    UserName: mobiusAssertion.UserInformation.UserName);
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
                this.AddLogEvent(EventType.DocumentQueryFailed,
                                     0,
                                     true,
                                     patientId,
                                     string.Empty,
                                     string.Empty,
                                     string.Empty,
                                     string.Empty,
                                     string.Empty,
                                     Helper.GetIPAddress(),
                                     2,
                                     MobiusAppSettingReader.LocalHomeCommunityID,
                                     string.Empty,
                                     (object)documents,
                                     this.Result.IsSuccess ? string.Empty : this.Result.ErrorMessage,
                                     purpose: mobiusAssertion.PurposeOfUse.ToString(),
                                     subject: mobiusAssertion.UserInformation.Role.ToString(),
                                     UserName: mobiusAssertion.UserInformation.UserName
                                    );
            }
            return this.Result;
        }

        #endregion GetDocumentMetadata


        /// <summary>
        /// Get Document
        /// </summary>
        /// <param name="documentId">string</param>
        /// <returns>return Result class object</returns>
        public Result GetDocument(DocumentRequest documentRequest, out MobiusDocument document)
        {

            document = null;
            try
            {
                document = new MobiusDocument();
                if (string.IsNullOrEmpty(documentRequest.documentId))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Document_DocumentId_Missing);
                    return this.Result;
                }
                if (string.IsNullOrEmpty(documentRequest.purpose))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Document_Purpose_Missing);
                    return this.Result;
                }
                if (string.IsNullOrEmpty(documentRequest.patientId))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Document_PatientId_Missing);
                    return this.Result;
                }
                if (string.IsNullOrEmpty(documentRequest.subjectRole))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Document_Subject_Missing);
                    return this.Result;
                }

                if (string.IsNullOrEmpty(documentRequest.subjectEmailID))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.EmailAddress_Missing);
                    return this.Result;
                }
                
                this.Result = GetDocumentMetaData(documentRequest.documentId, out document);

                if (!this.Result.IsSuccess || document == null)
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.RecordNotFound);
                    return this.Result;
                }
                if (!string.IsNullOrEmpty(documentRequest.Description))
                {
                    if (documentRequest.Description.Length > 500)
                    {
                        documentRequest.Description = documentRequest.Description.Substring(0, 500);

                    }
                }

                this.Result = GetAssertion(documentRequest.Assertion, AssertionAction.DocumentRetrieve, documentRequest.patientId);
                if (!this.Result.IsSuccess)
                {
                    return this.Result;
                }

                if (document.DataSource == DocumentSource.Local.ToString())
                    documentRequest.LocalData = true;

                // Check if patient himself is trying to retrieve the document  
                if (documentRequest.subjectRole == Helper.Patient)
                {

                    this.Result = ProcessDocumentRetrieveForPatient(documentRequest, document);
                }
                //Document Retrieve for provider 
                else
                {
                    this.Result = ProcessDocumentRetrieveForProvider(documentRequest, document);
                }

                //in case of Normal access
                if (documentRequest.OverrideReason == Mobius.CoreLibrary.OverrideReason.UNSPECIFIED)
                {
                    this.AddLogEvent(this.Result.IsSuccess ? EventType.DocumentRetrieval : EventType.DocumentRetrievalFailed,
                                 0,
                                 true,
                                 documentRequest.patientId,
                                 string.Empty,
                                 string.Empty,
                                 string.Empty,
                                 string.Empty,
                                 string.Empty,
                                 Helper.GetIPAddress(),
                                 2,
                                MobiusAppSettingReader.LocalHomeCommunityID,
                                 string.Empty,
                                 (object)document,
                                 this.Result.IsSuccess ? string.Empty : this.Result.ErrorMessage,
                                 documentRequest.patientId,
                                 documentRequest.purpose,
                                  documentRequest.subjectRole,
                                  UserName: documentRequest.Assertion.UserInformation.UserName
                                 );
                }
                // In case of Emergency Access
                else
                {
                    EventActionData eventActionData = new EventActionData();
                    eventActionData.Event = this.Result.IsSuccess ? EventType.EmergencyOverride : EventType.EmergencyOverrideFailed;
                    eventActionData.DocumentId = documentRequest.documentId;
                    eventActionData.NetworkAccessPointID = Helper.GetIPAddress();
                    eventActionData.NetworkAccessPointTypeCode = 2;
                    eventActionData.CommunityId = MobiusAppSettingReader.LocalHomeCommunityID;
                    eventActionData.RequestObject = Helper.Serialize((object)document);
                    eventActionData.ErrorMessage = this.Result.IsSuccess ? string.Empty : this.Result.ErrorMessage;
                    eventActionData.PatientId = documentRequest.patientId;
                    eventActionData.Purpose = documentRequest.purpose;
                    eventActionData.Subject = documentRequest.subjectRole;
                    eventActionData.UserName = documentRequest.subjectEmailID;
                    eventActionData.DispatcherSummary = documentRequest.Description;
                    eventActionData.MessageType = documentRequest.OverrideReason.ToString();
                    eventActionData.ReferPatientId = documentRequest.Name;

                    EventLogger eventLogger = new EventLogger();
                    eventLogger.LogEvent(eventActionData);

                }
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
                this.AddLogEvent(EventType.DocumentRetrievalFailed,
                                 0,
                                 true,
                                 documentRequest.patientId,
                                 string.Empty,
                                 string.Empty,
                                 string.Empty,
                                 string.Empty,
                                 string.Empty,
                                 Helper.GetIPAddress(),
                                 2,
                                 MobiusAppSettingReader.LocalHomeCommunityID,
                                 string.Empty,
                                 (object)document,
                                 this.Result.IsSuccess ? string.Empty : this.Result.ErrorMessage,
                                 documentRequest.patientId,
                                 documentRequest.purpose,
                                  documentRequest.subjectRole,
                                 UserName: documentRequest.Assertion.UserInformation.UserName);

            }
            return this.Result;
        }


        #region Document Upload

        public Result UploadDocument(string communityId, string documentId, byte[] documentBytes, byte[] XACMLBytes,
            string patientId, string uploadedBy, string repositoryId, bool submitOnGateway, MobiusAssertion mobiusAssertion)
        {
            string C32Document = string.Empty;
            string uploadPath = string.Empty;
            string fileName = string.Empty;
            DocumentMetadata documentMetadata = null;
            try
            {
                // validation
                if (string.IsNullOrEmpty(communityId))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Document_communityId_Missing);
                    return this.Result;
                }
                if (string.IsNullOrEmpty(documentId))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Document_DocumentId_Missing);
                    return this.Result;
                }
                if (documentBytes == null || documentBytes.Length == 0)
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Document_Byte_Missing);
                    return this.Result;
                }
                if (string.IsNullOrEmpty(patientId))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.patient_MPIID_Missing);
                    return this.Result;
                }
                // if (string.IsNullOrEmpty(uploadedBy))
                //    return false;
                if (string.IsNullOrEmpty(repositoryId))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.RepositoryId_Missing);
                    return this.Result;
                }

                //Validate document bytes for correctness of C32 document bytes
                if (!ValidateC32Document(documentBytes).IsSuccess)
                {
                    return this.Result;
                }

                this.Result = GetAssertion(mobiusAssertion, AssertionAction.DocumentSubmission, patientId);
                if (!this.Result.IsSuccess)
                {
                    return this.Result;
                }

                List<MobiusNHINCommunity> NHINCommunities = null;
                // get the collection of community
                this.Result = this.MobiusDAL.GetNhinCommunity(out NHINCommunities);
                if (this.Result.IsSuccess)
                {
                    // If homeCommunityId being passed is home community  than save document at local share location(No call the gateway to upload document)
                    //IEnumerable<NHINCommunity> NHINCommunityFilerResult = NHINCommunities.Where(t => t.CommunityIdentifier == communityId && t.IsHomeCommunity == true);
                    string XACMLFileName = string.Empty;

                    string locationDB = Guid.NewGuid().ToString();
                    uploadPath = Path.Combine(MobiusAppSettingReader.DocumentPath, locationDB);
                    fileName = documentId + ".xml";
                    if (FileHandler.CreateFolder(uploadPath) && (FileHandler.SaveDocument(documentBytes, Path.Combine(uploadPath, fileName))))
                    {
                        if (XACMLBytes != null)
                        {
                            XACMLFileName = documentId + "XACML.xml";
                            FileHandler.SaveDocument(XACMLBytes, Path.Combine(uploadPath, XACMLFileName));
                        }

                        C32Document = MobiusAppSettingReader.DocumentPath + documentId + ".xml";
                        // GetC32Sections(out dsCategory);  // TO DO: not in use if require then take  List<C32Section> c32Sections as second parameter of ValidateDocumentsSections method

                        CDAHelper CDAHelper = new CDAHelper(documentBytes);

                        // Save C32 document meta data in DB                        
                        documentMetadata = new DocumentMetadata();
                        documentMetadata.OriginalDocumentId = documentId;
                        documentMetadata.PatientId = patientId;

                        documentMetadata.DocumentTitle = CDAHelper.DocumentTitle;
                        documentMetadata.Author = CDAHelper.Authors != null && CDAHelper.Authors.Count > 0 ? CDAHelper.Authors[0].Person : string.Empty;
                        documentMetadata.DocumentSource = DocumentSource.Local.ToString();
                        documentMetadata.FilePath = locationDB;
                        documentMetadata.CreatedDate = CDAHelper.DocumentCreationDate;

                        documentMetadata.SourceCommunityId = communityId;
                        documentMetadata.SourceRepositryId = repositoryId;
                        documentMetadata.FileName = fileName;
                        documentMetadata.XACMLfileName = XACMLFileName.Replace(".xml", "");
                        documentMetadata.Reposed = true;
                        documentMetadata.FacilityId = string.Empty;
                        documentMetadata.UploadedBy = string.Empty;
                        this.Result.IsSuccess = this.MobiusDAL.SaveC32DocumentMetaData(documentMetadata);



                        if (this.Result.IsSuccess && submitOnGateway)
                        {
                            try
                            {
                                //TODO remove this code
                                XACMLClass xc = new XACMLClass();
                                XACMLHandler xacmldetails = new XACMLHandler();
                                xc = xacmldetails.GetXACMLDocumentDetail(XACMLBytes);
                                this.PurposeOfUse = xc.PurposeofUse;
                                // Call to gateway if communityId is not home
                                this.Result = this.MobiusConnect.UploadDocument(communityId, documentId, documentBytes, XACMLBytes, patientId, this.Assertion.DocumentSubmissionAssertion);
                                // Delete document from local database if document upload fail on DOD.
                                if (!this.Result.IsSuccess)
                                {
                                    this.MobiusDAL.DeleteC32DocumentMetaData(documentId);
                                }

                            }
                            catch (Exception)
                            {   // Delete document from local database if document upload fail on DOD.
                                this.MobiusDAL.DeleteC32DocumentMetaData(documentId);
                                throw;
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return this.Result;
        }

        #endregion Document Upload

        #region Share Document

        /// <summary>
        /// ShareDocument method makes call to Gateway to share the document to the gateway with applied consents.
        /// </summary>
        /// <param name="DocByteData">byte array</param>
        /// <param name="XACMLbyteData">byte array</param>
        /// <param name="PatientID">string</param>
        /// <param name="LoggedInUserRole">string</param>
        /// <param name="HomeCommunityId">string</param>
        /// <param name="SourceRepositryID">string</param>
        /// <param name="FacilityID">string</param>
        /// <returns>return Result class object</returns>
        public Result ShareDocument(ShareDocument shareDocument)
        {
            #region Local variables
            ErrorCode errorCode = ErrorCode.ErrorSuccess;
            string purposeOfUse = string.Empty;
            MobiusDocument docObj = null;
            List<string> sections = new List<string>();
            List<string> consentcategorysections = new List<string>();
            int categoryID = 0;
            // DataSet dsCategory = new DataSet();
            List<C32Section> c32Sections = null;
            DocumentMetadata documentMetadata = null;
            #endregion
            try
            {
                if (string.IsNullOrEmpty(shareDocument.patientId))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Document_PatientId_Missing);
                    return this.Result;
                }
                if (string.IsNullOrEmpty(shareDocument.subject))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Document_Subject_Missing);
                    return this.Result;
                }
                if (string.IsNullOrEmpty(shareDocument.homeCommunityId))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Document_communityId_Missing);
                    return this.Result;
                }

                if (string.IsNullOrEmpty(shareDocument.RemotePatientId))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Invalid_RemotePatientID);
                    return this.Result;
                }

                if (string.IsNullOrEmpty(shareDocument.RemoteCommunityId))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Invalid_RemoteCommunityId);
                    return this.Result;
                }

                if (string.IsNullOrEmpty(shareDocument.sourceRepositryId))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Document_SourceRepositoryId_Missing);
                    return this.Result;
                }

                if (string.IsNullOrEmpty(shareDocument.OriginalDocumentID))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Document_DocumentId_Missing);
                    return this.Result;
                }
                if (string.IsNullOrEmpty(shareDocument.Assertion.UserInformation.UserName))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.UserId_Missing);
                    return this.Result;
                }

                //Validate document bytes for correctness of C32 document bytes                
                if (!ValidateC32Document(shareDocument.docByteData).IsSuccess)
                {
                    return this.Result;
                }

                if (!GetAssertion(shareDocument.Assertion, AssertionAction.DocumentSubmission, shareDocument.patientId).IsSuccess)
                {
                    return this.Result;
                }

                CDAHelper CDAHelper = new CDAHelper(shareDocument.docByteData);

                XmlDocument xmldocument = new XmlDocument();
                //Modified for issue id #138
                using (MemoryStream XACMLStream = new MemoryStream(shareDocument.XACMLbyteData))
                {
                    this.Result = CDAHelper.GetC32Sections(out sections);
                    if (sections.Count == null || sections.Count <= 0)
                    {
                        // to do
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.C32Sections_Missing);
                        return this.Result;
                    }

                    xmldocument.Load(XACMLStream);
                    XACMLStream.Close();
                    //Modified for issue id #138
                }
                // TO DO
                XACML = XACMLHandler.GetXACMLDocumentDetail(shareDocument.XACMLbyteData);
                if (!string.IsNullOrEmpty(XACML.PurposeofUse))
                {
                    MobiusDocument document = new MobiusDocument();
                    DocumentRequest DocumentRequest = new DocumentRequest();
                    DocumentRequest.patientId = shareDocument.patientId;
                    DocumentRequest.subjectRole = shareDocument.subject;
                    DocumentRequest.purpose = XACML.PurposeofUse;
                    DocumentRequest.documentId = shareDocument.OriginalDocumentID;
                    DocumentRequest.subjectEmailID = shareDocument.Assertion.UserInformation.UserName;
                    this.MobiusDAL.GetDocumentMetaData(shareDocument.OriginalDocumentID, out document);
                    if (document != null)
                        DocumentRequest.FilePathLocation = Path.Combine(MobiusAppSettingReader.DocumentPath, document.Location);

                    // If user is a patient, skip patient consent verification
                    if (DocumentRequest.subjectRole == Helper.Patient)
                    {
                        this.Result.IsSuccess = true;
                    }
                    else // If user is a Provider, check for patient consent
                    {
                        this.Result = this.HasAcessPermission(DocumentRequest, out categoryID);

                        if (!this.Result.IsSuccess)
                        {
                            return this.Result;
                           
                        }
                        else
                        {
                            MobiusPatientConsent patientConsent = null;
                            List<C32Section> C32Sections = null;
                            // this categoryID returns in case of provider/role has permission in XACML file
                            if (categoryID != -214783647)
                            {
                                this.Result = this.GetPatientConsentByConsentId(shareDocument.patientId, categoryID, out  patientConsent, out C32Sections);
                            //load Patient consent in memory for validating against the document.
                            if (!this.Result.IsSuccess)
                                return this.Result;

                            //Check Patient consent on document.
                           
                                this.Result = CDAHelper.CheckPatientConsent(C32Sections);
                                if (!this.Result.IsSuccess)
                                    return this.Result;
                            }
                        }
                    }

                    //this.Result = C32Handler.GetC32DocumentDetail(docByteData, out docObj);
                    if (!this.Result.IsSuccess)
                    {
                        this.Result.SetError(ErrorCode.ErrorOccurredInC32Document_Parsing);
                        return this.Result;
                    }
                    // TO DO
                    bool hasDocumentSaved = false;
                    string fileName = Guid.NewGuid().ToString();
                    string folderName = Guid.NewGuid().ToString();
                    string docLocation = Path.Combine(MobiusAppSettingReader.DocumentPath, folderName);
                    if (FileHandler.CreateFolder(docLocation))
                    {
                        // save document
                        hasDocumentSaved = FileHandler.SaveDocument(shareDocument.docByteData, Path.Combine(docLocation, fileName + ".xml"));

                        if (hasDocumentSaved)
                        {
                            hasDocumentSaved = FileHandler.SaveDocument(shareDocument.XACMLbyteData, Path.Combine(docLocation, fileName + "XACML.xml"));

                            if (hasDocumentSaved)
                            {
                                hasDocumentSaved = false;
                                using (TransactionScope transactionScope = new TransactionScope())
                                {
                                    documentMetadata = new DocumentMetadata();
                                    documentMetadata.OriginalDocumentId = fileName;
                                    documentMetadata.PatientId = shareDocument.patientId;
                                    documentMetadata.UploadedBy = null;
                                    documentMetadata.DocumentTitle = CDAHelper.DocumentTitle;
                                    documentMetadata.Author = (CDAHelper.Authors != null && CDAHelper.Authors.Count > 0) ? CDAHelper.Authors[0].Person : string.Empty;
                                    documentMetadata.CreatedDate = CDAHelper.DocumentCreationDate;
                                    documentMetadata.DocumentSource = DocumentSource.Local.ToString();
                                    documentMetadata.SourceCommunityId = shareDocument.homeCommunityId;
                                    documentMetadata.SourceRepositryId = shareDocument.sourceRepositryId;
                                    documentMetadata.FacilityId = string.Empty;
                                    documentMetadata.Reposed = true;
                                    documentMetadata.FilePath = folderName;
                                    documentMetadata.XacmlDocumentId = fileName + "XACML";
                                    documentMetadata.IsShared = false;
                                    documentMetadata.SharedId = null;
                                    documentMetadata.FileName = fileName + ".xml";

                                    hasDocumentSaved = this.MobiusDAL.SaveDocumentMetadata(documentMetadata);
                                    documentMetadata = null;
                                    if (hasDocumentSaved)
                                    {

                                        //TODO remove this code
                                        XACMLClass xc = new XACMLClass();
                                        XACMLHandler xacmldetails = new XACMLHandler();
                                        xc = xacmldetails.GetXACMLDocumentDetail(shareDocument.XACMLbyteData);
                                        this.PurposeOfUse = xc.PurposeofUse;

                                        // TODO: Check the value of following variable, if it should be today's date.
                                        string ruleCreatedDate = DateTime.Now.ToShortDateString();
                                        this.Result = this.MobiusConnect.UploadDocument(shareDocument.homeCommunityId, fileName, shareDocument.docByteData, shareDocument.XACMLbyteData, fileName + "XACML", ruleCreatedDate, XACML.RuleStartDate,
                                            XACML.RuleEndDate, shareDocument.RemotePatientId, shareDocument.RemoteCommunityId, this.Assertion.DocumentSubmissionAssertion);

                                        if (!this.Result.IsSuccess)
                                        {
                                            transactionScope.Dispose();
                                        }

                                        if (this.Result.IsSuccess)
                                        {
                                            //remove the name/id of Logged in user.
                                            XACML.Subject.Remove(shareDocument.Assertion.UserInformation.UserName);

                                            bool bPatientShared = this.MobiusDAL.SaveSharingDetails(fileName, XACML.RuleStartDate, XACML.RuleEndDate,XACML.Subject, XACML.PurposeofUse);
                                            if (!bPatientShared)
                                            {
                                                transactionScope.Dispose();
                                            }
                                            transactionScope.Complete();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {

                        return this.Result;
                    }
                    //Modified for issue id #138                    
                }
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
                Helper.LogError(ex);
                return this.Result;
            }
            return this.Result;
        }

        #endregion Share Document



        #endregion Methods Making Gateway Calls

        #region MasterCollection

        /// <summary>
        /// This method would return the master data entries from db
        /// </summary>
        /// <param name="masterData">Enum of Master Entries</param>
        /// <param name="dependentValue">int</param>
        /// <param name="masterDataCollection">collection of masterclasscollection class</param>        
        /// <returns>return Result class object</returns>
        public Result GetMasterData(MasterCollection masterCollection, int dependedValue, out List<MasterData> masterDataCollection)
        {
            masterDataCollection = null;
            try
            {
                this.Result.IsSuccess = true;
                this.Result = this.MobiusDAL.GetMasterData(masterCollection, dependedValue, out masterDataCollection);
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return this.Result;
        }

        #endregion masterCollection

        #region Methods Making Database Calls

        /// <summary>
        /// Get DocumentMetaData
        /// </summary>
        /// <param name="DocumentID">string</param>
        /// <returns>return Result class object</returns>
        public Result GetDocumentMetaData(string documentID, out MobiusDocument document)
        {
            document = null;
            try
            {
                this.Result.IsSuccess = true;
                if (string.IsNullOrWhiteSpace(documentID))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Document_DocumentId_Missing);
                    return this.Result;

                }
                this.Result = this.MobiusDAL.GetDocumentMetaData(documentID, out document);
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }

            return this.Result;
        }

        /// <summary>
        /// Get Facility Name
        /// </summary>
        /// <param name="token">string</param>
        /// <param name="FacilityId">int</param>
        /// <param name="errorCode">errorCode class object</param>
        /// <returns>return facility name</returns>
        public string GetFacilityName(string token, int facilityId, out ErrorCode errorCode)
        {
            string facilityName = string.Empty;
            errorCode = ErrorCode.ErrorSuccess;
            try
            {
                facilityName = this.MobiusDAL.GetFacilityName(facilityId);
                if (string.IsNullOrEmpty(facilityName))
                {
                    errorCode = ErrorCode.RecordNotFound;
                }
            }
            catch (Exception)
            {
                // TODO
            }
            return facilityName;
        }

        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="userId">string</param>
        /// <param name="passwordHash">string</param>
        /// <param name="facilityId">int</param>
        /// <param name="errorCode">errorCode class object</param>
        /// <returns>return token</returns>
        //public string LoginUser(string userId, string passwordHash, int facilityId, out ErrorCode errorCode)
        //{
        //    string token = string.Empty;
        //    try
        //    {
        //        token = this.MobiusDAL.LoginUser(userId, passwordHash, facilityId, out errorCode);
        //    }
        //    catch (Exception ex)
        //    {
        //        token = ex.Message.ToString();
        //        errorCode = ErrorCode.ErrorUnknown;
        //    }
        //    return token;
        //}

        #endregion Methods Making Database Calls

        #region GetNhinCommunity
        /// <summary>
        /// 
        /// </summary>
        /// <param name="NHINCommunities"></param>
        /// <returns>return Result class object</returns>
        public Result GetNhinCommunity(out List<MobiusNHINCommunity> NHINCommunities)
        {
            NHINCommunities = null;
            try
            {
                this.Result.IsSuccess = false;
                this.Result = this.MobiusDAL.GetNhinCommunity(out NHINCommunities);
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return this.Result;
        }
        #endregion

        #region Manage Nhin Communities
        /// <summary>
        /// 
        /// </summary>
        /// <param name="NHINCommunities"></param>
        /// <returns>return Result class object</returns>
        public List<MobiusNHINCommunity> GetAllNhinCommunities()
        {
            List<MobiusNHINCommunity> NHINCommunities = new List<MobiusNHINCommunity>();
            NHINCommunities = null;
            try
            {
                this.Result.IsSuccess = false;
                NHINCommunities = this.MobiusDAL.GetAllNhinCommunities();

            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return NHINCommunities;
        }

        public Result AddNhinCommunities(List<MobiusNHINCommunity> NHINCommunities)
        {
            try
            {
                if (NHINCommunities.Count > 0)
                {
                    foreach (var community in NHINCommunities)
                    {
                        if (string.IsNullOrEmpty(community.CommunityIdentifier))
                        {
                            this.Result.IsSuccess = false;
                            this.Result.SetError(ErrorCode.Community_Identifier_Missing);
                            return this.Result;
                        }
                        if (string.IsNullOrEmpty(community.CommunityDescription))
                        {
                            this.Result.IsSuccess = false;
                            this.Result.SetError(ErrorCode.Community_Description_Missing);
                            return this.Result;
                        }
                    }
                    this.Result = this.MobiusDAL.AddNhinCommunities(NHINCommunities);
                }
                else
                {

                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Select_Community_to_Import);
                    return this.Result;
                }

            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return this.Result;
        }

        public Result UpdateNhinCommunity(MobiusNHINCommunity NHINCommunity)
        {
            try
            {
                if (string.IsNullOrEmpty(NHINCommunity.CommunityIdentifier))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Community_Identifier_Missing);
                    return this.Result;
                }
                if (string.IsNullOrEmpty(NHINCommunity.CommunityDescription))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Community_Description_Missing);
                    return this.Result;
                }
                this.Result = this.MobiusDAL.UpdateNhinCommunity(NHINCommunity);

            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return this.Result;
        }

        public Result DeleteNhinCommunity(int ID, string IsHomeCommunity)
        {
            try
            {
                if (IsHomeCommunity == NO)
                {
                    this.Result.IsSuccess = false;
                    this.Result = this.MobiusDAL.DeleteNhinCommunity(ID);
                }
                else
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.HomeCommunity_Deletion_Failed);
                    return this.Result;

                }
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return this.Result;
        }

        #endregion

        #region GetC32Sections
        /// <summary>
        /// Get C32 Sections
        /// </summary>
        /// <param name="c32Sections"></param>
        /// <returns>return Result class object</returns>
        public Result GetC32Sections(out List<C32Section> c32Sections)
        {
            c32Sections = null;
            try
            {
                this.Result = this.MobiusDAL.GetC32Sections(out c32Sections);

            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return this.Result;
        }
        #endregion


        #region GetC32Sections
        /// <summary>
        /// Get C32 Sections
        /// </summary>
        /// <param name="c32Sections"></param>
        /// <returns>return Result class object</returns>
        public Result GetC32Sections_TODO(out List<C32Section> c32Sections)
        {
            c32Sections = null;
            try
            {
                this.Result = this.MobiusDAL.GetC32Sections_TODO(out c32Sections);

            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return this.Result;
        }
        #endregion

        #region AddProvider
        /// <summary>
        /// Add New Provider
        /// </summary>
        /// <param name="provider">provider class object</param>
        /// <param name="PKCS7Response">out string</param>
        /// <returns>return Result class object</returns>
        public Result AddProvider(Mobius.Entity.Provider provider, out string PKCS7Response)
        {
            string message = string.Empty;
            string filePath = string.Empty;
            string OTPString = string.Empty;
            PKCS7Response = string.Empty;
            int pendingEnrollmentRequestId = 0;
            CertificateEnrollment certificateEnrollment = null;
            try
            {
                if (provider.IndividualProvider == true)
                {
                    // First name is required in case of individual provider
                    if (string.IsNullOrWhiteSpace(provider.FirstName))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Provider_First_Name_Missing);
                        return this.Result;
                    }
                    // Last name is required in case of individual provider
                    if (string.IsNullOrWhiteSpace(provider.LastName))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Provider_Last_Name_Missing);
                        return this.Result;
                    }
                    // Email addresses required in case of individual provider
                    if (string.IsNullOrWhiteSpace(provider.Email))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Provider_Email_Address_Missing);
                        return this.Result;
                    }
                    if (!Validator.ValidateEmail(provider.Email, out message))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Provider_Invalid_Email);
                        return this.Result;
                    }
                    if (string.IsNullOrWhiteSpace(provider.Password))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Password_Missing);
                        return this.Result;
                    }
                    if (string.IsNullOrWhiteSpace(provider.MedicalRecordsDeliveryEmailAddress))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Provider_MedicalRecordsDeliveryEmailAddress_Missing);
                        return this.Result;
                    }
                    if (!Validator.ValidateEmail(provider.MedicalRecordsDeliveryEmailAddress, out message))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Provider_Invalid_Email);
                        return this.Result;
                    }
                    if (string.IsNullOrWhiteSpace(provider.PostalCode))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.ZipCode_Missing);
                        return this.Result;
                    }
                    if (!Validator.ValidatePostalCode(provider.PostalCode, out message))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Invalid_Postal_Code);
                        return this.Result;
                    }
                    else
                    {
                        City city = new City();
                        this.Result = this.MobiusDAL.GetLocalityByZipCode(provider.PostalCode, out city);
                        if (!this.Result.IsSuccess)
                        {
                            this.Result.IsSuccess = false;
                            this.Result.SetError(ErrorCode.Invalid_Postal_Code);
                            return this.Result;
                        }

                        if (!string.IsNullOrEmpty(city.State.Country.CountryName) && city.State.Country.CountryName.ToUpper() == "US")
                        {
                            city.State.Country.CountryName = "USA";
                            provider.City = city;
                        }

                    }

                    // Added for validating street name/addressline1 and street number/addressline2
                    if (!string.IsNullOrEmpty(provider.StreetName))
                    {
                        if (!Validator.ValidateAddress(provider.StreetName))
                        {
                            this.Result.IsSuccess = false;
                            this.Result.SetError(ErrorCode.Address_Has_SpecialCharacater);
                            return this.Result;
                        }

                        provider.StreetName = provider.StreetName.Trim();
                    }

                    if (!string.IsNullOrEmpty(provider.StreetNumber))
                    {

                        if (!Validator.ValidateAddress(provider.StreetNumber))
                        {
                            this.Result.IsSuccess = false;
                            this.Result.SetError(ErrorCode.Address_Has_SpecialCharacater);
                            return this.Result;
                        }
                        provider.StreetNumber = provider.StreetNumber.Trim();
                    }
                    //Ends Addition



                    if (!Validator.ValidateName(provider.FirstName, out message))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Name_Contains_Specialcharacater);
                        return this.Result;
                    }
                    // Provider's Last name is being validated for special characters
                    if (!Validator.ValidateName(provider.LastName, out message))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Name_Contains_Specialcharacater);
                        return this.Result;
                    }
                    if (!Enum.IsDefined(typeof(ProviderType), provider.ProviderType))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Provider_Type);
                        return this.Result;
                    }

                }
                else
                {
                    // Organization name is required in case of Organizational provider
                    if (string.IsNullOrWhiteSpace(provider.OrganizationName))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Provider_Organization_Name_Missing);
                        return this.Result;
                    }
                    if (string.IsNullOrWhiteSpace(provider.Password))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Password_Missing);
                        return this.Result;
                    }
                    // MedicalRecordsDeliveryEmailAddressis required in case of Organizational provider
                    if (string.IsNullOrWhiteSpace(provider.MedicalRecordsDeliveryEmailAddress))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Provider_MedicalRecordsDeliveryEmailAddress_Missing);
                        return this.Result;
                    }
                    if (!Validator.ValidateEmail(provider.MedicalRecordsDeliveryEmailAddress, out message))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Provider_Invalid_Email);
                        return this.Result;
                    }
                    if (!Enum.IsDefined(typeof(ProviderType), provider.ProviderType))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Provider_Type);
                        return this.Result;
                    }
                    if (string.IsNullOrWhiteSpace(provider.PostalCode))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.ZipCode_Missing);
                        return this.Result;
                    }
                    if (!Validator.ValidatePostalCode(provider.PostalCode, out message))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Invalid_Postal_Code);
                        return this.Result;
                    }
                    else
                    {
                        City city = new City();
                        this.Result = this.MobiusDAL.GetLocalityByZipCode(provider.PostalCode, out city);
                        if (!this.Result.IsSuccess)
                        {
                            this.Result.IsSuccess = false;
                            this.Result.SetError(ErrorCode.Invalid_Postal_Code);
                            return this.Result;
                        }

                        if (!string.IsNullOrEmpty(city.State.Country.CountryName) && city.State.Country.CountryName.ToUpper() == "US")
                        {
                            city.State.Country.CountryName = "USA";
                            provider.City = city;
                        }

                    }
                }


                if (!Validator.ValidatePassword(provider.Password, out message))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Invalid_Password);
                    return this.Result;
                }
                else
                {
                    provider.Password = Helper.EncryptData(provider.Password);
                }

                //Added for validating street name/addressline1 and street number/addressline2
                if (!string.IsNullOrEmpty(provider.StreetName))
                {
                    if (!Validator.ValidateAddress(provider.StreetName))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Address_Has_SpecialCharacater);
                        return this.Result;
                    }
                    provider.StreetName = provider.StreetName.Trim();
                }

                if (!string.IsNullOrEmpty(provider.StreetNumber))
                {
                    if (!Validator.ValidateAddress(provider.StreetNumber))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Address_Has_SpecialCharacater);
                        return this.Result;
                    }
                    provider.StreetNumber = provider.StreetNumber.Trim();
                }
                //Ends Addition

                // Provider's phone number format validation is being performed
                if (!Validator.ValidatePhone(provider.ContactNumber, out message))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Invalid_Phone_Format);
                    return this.Result;
                }
                if (!Validator.ValidateURL(provider.ElectronicServiceURI, out message))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Invalid_URI);
                    return this.Result;
                }

                if (string.IsNullOrWhiteSpace(provider.CSR))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.CertificateAuthority_CSR_Missing);
                    return this.Result;
                }
                // Verify Provider Already registered in database or not
                if (this.MobiusDAL.HasProviderRegistered(string.Empty, provider).IsSuccess)
                {
                    certificateEnrollment = new CertificateEnrollment();
                    // ValidateEnrollmentRequest validate the CSR
                    this.Result = certificateEnrollment.ValidateEnrollmentRequest(provider.CSR);
                    if (this.Result.IsSuccess)
                    {  // EnrollCertificateRequest will accepts the PKCS#10 request and cerate the certificate for the requestor 
                        this.Result = certificateEnrollment.EnrollCertificateRequest(provider.CSR, out PKCS7Response);
                        if (!string.IsNullOrEmpty(PKCS7Response))
                        {
                            this.Result = certificateEnrollment.GetCertificateInformation(PKCS7Response);
                            provider.CertificateSerialNumber = certificateEnrollment.SerialNumber;
                            provider.PublicKey = certificateEnrollment.PublicKey;
                            provider.CreatedOn = DateTime.Now.ToString();
                            provider.ExpiryiOn = certificateEnrollment.ExpirationDate.ToString();
                        }
                        if (this.Result.IsSuccess)
                        {
                            this.Result = this.MobiusDAL.AddProvider(provider);
                            if (!this.Result.IsSuccess)
                            {
                                this.Result.IsSuccess = false;
                                this.Result.SetError(ErrorCode.Provider_Registration_Failed, message);
                                return this.Result;
                            }
                            this.Result.IsSuccess = true;
                            this.Result.SetError(ErrorCode.Provider_Registration_successful);
                        }

                    }
                    this.AddLogEvent(this.Result.IsSuccess ? EventType.RegisterProvider : EventType.RegisterProviderFailed,
                                        pendingEnrollmentRequestId,
                                        provider.IndividualProvider ? true : false,
                                        string.Empty,
                                        provider.FirstName,
                                        provider.OrganizationName,
                                        provider.Email,
                                        provider.MedicalRecordsDeliveryEmailAddress,
                                        OTPString,
                                        Helper.GetIPAddress(),
                                        2,
                                       MobiusAppSettingReader.LocalHomeCommunityID,
                                        string.Empty,
                                        (object)provider,
                                        this.Result.IsSuccess ? string.Empty : this.Result.ErrorMessage,
                                        string.Empty,
                                        REGISTER,
                                       provider.ProviderType.ToString(),
                                        UserName: provider.Email);
                }
                else
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Provider_Already_Exist);
                    return this.Result;
                }
                //Added for issue id #138
                certificateEnrollment = null;
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
                this.AddLogEvent(EventType.RegisterProviderFailed,
                                    pendingEnrollmentRequestId,
                                    provider.IndividualProvider ? true : false,
                                    string.Empty,
                                    provider.FirstName,
                                    provider.OrganizationName,
                                    provider.Email,
                                    provider.MedicalRecordsDeliveryEmailAddress,
                                    OTPString,
                                    Helper.GetIPAddress(),
                                    2,
                                    MobiusAppSettingReader.LocalHomeCommunityID,
                                    string.Empty,
                                    (object)provider,
                                    this.Result.IsSuccess ? string.Empty : this.Result.ErrorMessage,
                                    string.Empty,
                                    REGISTER,
                                    provider.ProviderType.ToString(),
                                    UserName: provider.Email
                                     );

            }
            return this.Result;
        }
        #endregion

        #region CreatePatientReferral
        public Result CreateReferral(CreatePatientReferral createPatientReferral)
        {

            #region Variables
            // variables for consent validation
            ErrorCode errorCode = ErrorCode.ErrorSuccess;
            List<string> sections = new List<string>();
            List<string> consentcategorysections = new List<string>();
            List<C32Section> c32Sections = null;
            DateTime date;
            PatientReferral patientReferred = null;
            string documentId = string.Empty;
            Provider providerDetails = null;
            #endregion
            try
            {
                int categoryID = 0;
                string message = string.Empty;
                int referPatientId = 0;
                int patientConsentId = 0;

                //Validate document bytes for correctness of C32 document bytes
                if (!ValidateC32Document(createPatientReferral.DocumentBytes).IsSuccess)
                {
                    return this.Result;
                }

                // to find the sections from document bytes
                CDAHelper CDAHelper = new CDAHelper(createPatientReferral.DocumentBytes);

                this.Result = CDAHelper.GetC32Sections(out sections);

                if (sections.Count <= 0)
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.C32Sections_Missing);
                    return this.Result;
                }

                if (createPatientReferral.Id == 0)
                {
                    MobiusDocument document = new MobiusDocument();
                    DocumentRequest documentRequest = new DocumentRequest();
                    documentRequest.patientId = createPatientReferral.Patient.LocalMPIID;
                    documentRequest.subjectRole = createPatientReferral.Subject;
                    documentRequest.purpose = createPatientReferral.PurposeOfUse.ToString();
                    documentRequest.documentId = createPatientReferral.OriginalDocumentID;
                    this.MobiusDAL.GetDocumentMetaData(createPatientReferral.OriginalDocumentID, out document);
                    if(document !=null)
                       documentRequest.FilePathLocation = Path.Combine(MobiusAppSettingReader.DocumentPath, document.Location);
                    documentRequest.subjectEmailID = createPatientReferral.ReferredByEmail;
                    this.Result = this.HasAcessPermission(documentRequest, out patientConsentId);

                    if (!this.Result.IsSuccess)
                    {
                        return this.Result;
                    }
                    // Check for deviation of categories in between Document sections and Patient consented sections.   
                    else
                    {
                        MobiusPatientConsent patientConsent = null;
                        List<C32Section> C32Sections = null;

                        // this patientConsentId returns in case of provider/role has permission in XACML file
                        if (patientConsentId != -214783647)
                        {
                        this.Result = this.GetPatientConsentByConsentId(createPatientReferral.Patient.LocalMPIID, patientConsentId, out  patientConsent, out C32Sections);
                        //load Patient consent in memory for validating against the document.
                        if (!this.Result.IsSuccess)
                            return this.Result;

                        //Check Patient consent on document.
                       
                            this.Result = CDAHelper.CheckPatientConsent(C32Sections);
                            if (!this.Result.IsSuccess)
                                return this.Result;
                        }


                    }
                }
                else
                {
                    this.Result.IsSuccess = true;
                }


                if (createPatientReferral.Patient == null)
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.PatientReferral_Patient_Information_Missing);
                    return this.Result;
                }
                if (string.IsNullOrWhiteSpace(createPatientReferral.Patient.LocalMPIID))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.PatientReferral_Some_Of_Patient_Information_Missing);
                    return this.Result;
                }

                if (string.IsNullOrWhiteSpace(createPatientReferral.ReferralAccomplishedOn))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.PatientReferral_ReferralOn);
                    return this.Result;
                }
                if (!string.IsNullOrEmpty(createPatientReferral.ReferralAccomplishedOn))
                {
                    if (DateTime.TryParseExact(createPatientReferral.ReferralAccomplishedOn, format, null, DateTimeStyles.None, out date) == false)
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Invalid_Date_Format);
                        return this.Result;
                    }
                    else if (Convert.ToDateTime(date.ToShortDateString()) < Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Accomplish_Date_Should_Not_Be_Less_Than_Today);
                        return this.Result;
                    }
                }

                if (!Validator.ValidateEmail(createPatientReferral.ReferredToEmail, out message))
                {
                    this.Result.SetError(ErrorCode.PatientReferral_InValid_ReferralEmailAddress, message);
                    return this.Result;
                }


                if (this.Result.IsSuccess)
                {
                    if (string.IsNullOrEmpty(createPatientReferral.DocumentId))
                        documentId = Guid.NewGuid().ToString();
                    else
                        documentId = createPatientReferral.DocumentId;

                    if (createPatientReferral.Id == 0)
                    {
                        this.Result = UploadDocument(createPatientReferral.CommunityId, documentId, createPatientReferral.DocumentBytes, createPatientReferral.XACMLBytes, createPatientReferral.Patient.LocalMPIID, "", Helper.SourceRepositoryId, false, createPatientReferral.Assertion);
                    }
                    if (this.Result.IsSuccess)
                    {
                        patientReferred = new PatientReferral();
                        patientReferred.CommunityId = createPatientReferral.CommunityId;
                        patientReferred.DocumentBytes = createPatientReferral.DocumentBytes;
                        patientReferred.DocumentId = documentId;
                        patientReferred.Id = createPatientReferral.Id;
                        patientReferred.Patient = createPatientReferral.Patient;
                        patientReferred.PurposeOfUse = createPatientReferral.PurposeOfUse;
                        patientReferred.ReferralAccomplishedOn = createPatientReferral.ReferralAccomplishedOn;
                        patientReferred.ReferralOn = createPatientReferral.ReferralOn;
                        patientReferred.ReferralSummary = createPatientReferral.ReferralSummary;
                        patientReferred.ReferredBy = createPatientReferral.ReferredBy;
                        patientReferred.ReferredByEmail = createPatientReferral.ReferredByEmail;
                        patientReferred.ReferredToEmail = createPatientReferral.ReferredToEmail;
                        patientReferred.Subject = createPatientReferral.Subject;
                        patientReferred.XACMLBytes = createPatientReferral.XACMLBytes;
                        this.Result = this.MobiusDAL.CreatePatientReferral(patientReferred, out referPatientId);
                    }
                    if (!this.Result.IsSuccess)
                    {
                        this.Result.SetError(ErrorCode.PatientReferral_Failed);
                        return this.Result;
                    }


                    // Send Mail.
                }

                EventActionData eventActionData = new EventActionData();
                if (createPatientReferral.Id == 0)
                {
                    eventActionData.Event = EventType.PatientReferral;
                    eventActionData.ReferralSummary = AppendBodyContent(string.IsNullOrEmpty(createPatientReferral.ReferralSummary) ? string.Empty : eventActionData.ReferralSummary = createPatientReferral.ReferralSummary);
                    eventActionData.EmailRecipients.Add(createPatientReferral.ReferredToEmail);
                }
                if (createPatientReferral.ReferredByEmail != null)
                    eventActionData.UserName = createPatientReferral.ReferredByEmail;

                if (!string.IsNullOrWhiteSpace(createPatientReferral.ReferralOn))
                    eventActionData.ReferredOn = Convert.ToDateTime(createPatientReferral.ReferralOn).ToShortDateString();
                if (createPatientReferral.Patient.GivenName.Count > 0)
                {
                    eventActionData.FirstName = createPatientReferral.Patient.GivenName[0];
                }
                if (createPatientReferral.Patient.FamilyName.Count > 0)
                {
                    eventActionData.LastName = createPatientReferral.Patient.FamilyName[0];
                }

                eventActionData.Gender = createPatientReferral.Patient.Gender.ToString();
                if (!string.IsNullOrWhiteSpace(createPatientReferral.Patient.DOB))
                    eventActionData.DOB = Convert.ToDateTime(createPatientReferral.Patient.DOB).ToShortDateString();

                this.Result = this.MobiusDAL.GetProviderDetails(createPatientReferral.ReferredToEmail, out providerDetails);
                if (this.Result.IsSuccess && providerDetails.FirstName != null)
                {
                    eventActionData.ReferralDispatcher = providerDetails.FirstName;
                }
                else
                {
                    eventActionData.ReferralDispatcher = createPatientReferral.ReferredToEmail;
                }

                //get Serial number using ReferralDispatcher email address
                eventActionData.Serial = string.IsNullOrWhiteSpace(providerDetails.CertificateSerialNumber) ? string.Empty : providerDetails.CertificateSerialNumber;

                eventActionData.ReferPatientId = referPatientId.ToString(); ;
                eventActionData.DocumentId = documentId;
                eventActionData.Purpose = "Treatment";
                eventActionData.Subject = createPatientReferral.Subject;
                eventActionData.UserName = createPatientReferral.ReferredByEmail;
                EventLogger eventLogger = new EventLogger();
                eventLogger.LogEvent(eventActionData);
                eventLogger = null;
                eventActionData = null;
                if (this.Result.IsSuccess)
                {
                    this.Result.SetError(ErrorCode.Patient_Referred_Successfully);
                }
                else
                {
                    this.Result.SetError(ErrorCode.PatientReferral_Failed);
                    return this.Result;
                }

            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
                Helper.LogError(ex);

            }

            return this.Result;
        }
        #endregion

        #region AcceptPatientReferral
        public Result AcknowledgeReferral(AcceptReferral acceptReferral)
        {

            #region Variables
            // variables for consent validation
            DateTime date;
            PatientReferral patientReferred = null;
            Provider providerDetails = null;
            #endregion
            try
            {
                string message = string.Empty;
                int referPatientId = 0;
                this.Result.IsSuccess = true;
                if (string.IsNullOrWhiteSpace(acceptReferral.ReferralAccomplishedOn))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.PatientReferral_ReferralOn);
                    return this.Result;
                }
                if (!string.IsNullOrEmpty(acceptReferral.ReferralAccomplishedOn))
                {
                    if (DateTime.TryParseExact(acceptReferral.ReferralAccomplishedOn, format, null, DateTimeStyles.None, out date) == false)
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Invalid_Date_Format);
                        return this.Result;
                    }
                }
                if (acceptReferral.AcknowledgementStatus && string.IsNullOrWhiteSpace(acceptReferral.PatientAppointmentDate))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.PatientReferral_Appointment_Date);
                    return this.Result;
                }
                if (!string.IsNullOrEmpty(acceptReferral.PatientAppointmentDate))
                {
                    if (DateTime.TryParseExact(acceptReferral.PatientAppointmentDate, format, null, DateTimeStyles.None, out date) == false)
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Invalid_Date_Format);
                        return this.Result;
                    }
                    else if (Convert.ToDateTime(date.ToShortDateString()) < Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Appointment_Date_Should_Not_Be_Less_Than_Today);
                        return this.Result;
                    }
                }
                if (!string.IsNullOrWhiteSpace(acceptReferral.ReferralAccomplishedOn) &&
                                   !string.IsNullOrWhiteSpace(acceptReferral.PatientAppointmentDate))
                {
                    if (Convert.ToDateTime(acceptReferral.PatientAppointmentDate) > Convert.ToDateTime(acceptReferral.ReferralAccomplishedOn))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.ReferralAccomplishedDate_GreaterThan_ReferralDate);
                        return this.Result;
                    }
                }

                if (this.Result.IsSuccess)
                {

                    patientReferred = new PatientReferral();
                    patientReferred.AcknowledgementStatus = acceptReferral.AcknowledgementStatus;
                    patientReferred.DispatcherSummary = acceptReferral.DispatcherSummary;
                    patientReferred.Id = acceptReferral.Id;
                    patientReferred.DocumentId = acceptReferral.DocumentId;
                    patientReferred.PatientAppointmentDate = acceptReferral.PatientAppointmentDate;
                    patientReferred.ReferralAccomplishedOn = acceptReferral.ReferralAccomplishedOn;

                    this.Result = this.MobiusDAL.AcknowledgePatientReferral(patientReferred, out referPatientId);

                    if (!this.Result.IsSuccess)
                    {
                        this.Result.SetError(ErrorCode.PatientReferral_Failed);
                        return this.Result;
                    }

                    // Send Mail.
                }

                EventActionData eventActionData = new EventActionData();
                if (acceptReferral.Id != 0 && acceptReferral.AcknowledgementStatus)
                {
                    eventActionData.Event = EventType.ReferralAccepted;

                    eventActionData.DispatcherSummary = AppendBodyContent(string.IsNullOrEmpty(acceptReferral.DispatcherSummary) ? "   " : eventActionData.ReferralSummary = acceptReferral.DispatcherSummary);
                    eventActionData.EmailRecipients.Add(acceptReferral.ReferredByEmail);

                    this.Result = this.MobiusDAL.GetProviderDetails(acceptReferral.ReferredByEmail, out providerDetails);
                    if (this.Result.IsSuccess && providerDetails.FirstName != null)
                    {
                        eventActionData.ReferralRequestor = providerDetails.FirstName;
                    }
                    else
                    {
                        eventActionData.ReferralRequestor = acceptReferral.ReferredByEmail;
                    }

                }
                else if (acceptReferral.Id != 0 && !acceptReferral.AcknowledgementStatus)
                {
                    eventActionData.Event = EventType.ReferralDeclined;
                    eventActionData.DispatcherSummary = AppendBodyContent(string.IsNullOrEmpty(acceptReferral.DispatcherSummary) ? "   " : eventActionData.ReferralSummary = acceptReferral.DispatcherSummary);
                    eventActionData.EmailRecipients.Add(acceptReferral.ReferredByEmail);

                    this.Result = this.MobiusDAL.GetProviderDetails(acceptReferral.ReferredByEmail, out providerDetails);
                    if (this.Result.IsSuccess && providerDetails.FirstName != null)
                    {
                        eventActionData.ReferralRequestor = providerDetails.FirstName;
                    }
                    else
                    {
                        eventActionData.ReferralRequestor = acceptReferral.ReferredByEmail;
                    }
                }
                if (!string.IsNullOrWhiteSpace(acceptReferral.ReferralOn))
                    eventActionData.ReferredOn = Convert.ToDateTime(acceptReferral.ReferralOn).ToShortDateString();
                if (!string.IsNullOrWhiteSpace(acceptReferral.PatientAppointmentDate))
                    eventActionData.PatientAppointmentDate = Convert.ToDateTime(acceptReferral.PatientAppointmentDate).ToShortDateString();

                if (acceptReferral.Patient.GivenName.Count > 0)
                {
                    eventActionData.FirstName = acceptReferral.Patient.GivenName[0];
                }
                if (acceptReferral.Patient.FamilyName.Count > 0)
                {
                    eventActionData.LastName = acceptReferral.Patient.FamilyName[0];
                }
                eventActionData.Gender = acceptReferral.Patient.Gender.ToString();
                eventActionData.UserName = acceptReferral.ReferredToEmail;
                if (!string.IsNullOrWhiteSpace(acceptReferral.Patient.DOB))
                    eventActionData.DOB = Convert.ToDateTime(acceptReferral.Patient.DOB).ToShortDateString();

                this.Result = this.MobiusDAL.GetProviderDetails(acceptReferral.ReferredToEmail, out providerDetails);
                if (this.Result.IsSuccess && providerDetails.FirstName != null)
                {
                    eventActionData.ReferralDispatcher = providerDetails.FirstName;
                }
                else
                {
                    eventActionData.ReferralDispatcher = acceptReferral.ReferredToEmail;
                }
                eventActionData.DocumentId = acceptReferral.DocumentId;
                eventActionData.ReferPatientId = referPatientId.ToString(); ;

                EventLogger eventLogger = new EventLogger();
                eventLogger.LogEvent(eventActionData);
                // TODO need to remove this code after root cause analysis
                // Thread.Sleep(20);

                if (acceptReferral.Id != 0 && acceptReferral.AcknowledgementStatus)
                {
                    //get Serial number using ReferralDispatcher email address
                    eventActionData.Serial = string.IsNullOrWhiteSpace(providerDetails.CertificateSerialNumber) ? string.Empty : providerDetails.CertificateSerialNumber;

                    eventActionData.Event = EventType.SendPatientDocument;
                    eventActionData.DispatcherSummary = AppendBodyContent(string.IsNullOrEmpty(acceptReferral.DispatcherSummary) ? string.Empty : eventActionData.ReferralSummary = acceptReferral.DispatcherSummary);
                    eventActionData.EmailRecipients = new List<string>();
                    eventActionData.EmailRecipients.Add(acceptReferral.ReferredToEmail);
                    if (this.Result.IsSuccess && providerDetails.FirstName != null)
                    {
                        eventActionData.ReferralRequestor = providerDetails.FirstName;
                    }
                    else
                    {
                        eventActionData.ReferralRequestor = acceptReferral.ReferredToEmail;
                    }
                    eventActionData.Token = acceptReferral.DocumentId;
                    eventActionData.PatientId = acceptReferral.Patient.LocalMPIID;
                    eventLogger = new EventLogger();
                    eventLogger.LogEvent(eventActionData);
                }
                if (this.Result.IsSuccess)
                {
                    this.Result.SetError(ErrorCode.Acknowledgement_Sent_Successfully);

                }
                else
                {
                    this.Result.SetError(ErrorCode.PatientReferral_Failed);
                    return this.Result;
                }

            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);

            }
            return this.Result;
        }
        #endregion

        #region CompletedPatientReferral
        public Result CompletePatientReferral(PatientReferralCompleted patientReferralCompleted)
        {

            #region Variables
            // variables for consent validation
            PatientReferral patientReferred = null;
            //Modified for issue id #138
            EventLogger eventLogger = null;
            Provider providerDetails = null;
            bool transactionState = false;
            #endregion
            try
            {
                string message = string.Empty;
                int referPatientId = 0;
                this.Result.IsSuccess = true;

                //............Date validation......starts
                DateTime date;
                if (!string.IsNullOrEmpty(patientReferralCompleted.PatientAppointmentDate))
                {
                    if (DateTime.TryParseExact(patientReferralCompleted.PatientAppointmentDate, format, null, DateTimeStyles.None, out date) == false)
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Invalid_Date_Format);
                        return this.Result;
                    }
                    //if Appointment date is less than today
                    else if (Convert.ToDateTime(date.ToShortDateString()) < Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Appointment_Date_Should_Not_Be_Less_Than_Today);
                        return this.Result;
                    }
                }

                if (!string.IsNullOrWhiteSpace(patientReferralCompleted.ReferralAccomplishedOn) &&
                                 !string.IsNullOrWhiteSpace(patientReferralCompleted.PatientAppointmentDate))
                {
                    if (Convert.ToDateTime(patientReferralCompleted.PatientAppointmentDate) > Convert.ToDateTime(patientReferralCompleted.ReferralAccomplishedOn))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.ReferralAccomplishedDate_GreaterThan_ReferralDate);
                        return this.Result;
                    }
                }
                //............Date validation......ends

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    transactionState = true;

                    if (patientReferralCompleted.ReferralCompleted)
                    {
                        if (string.IsNullOrWhiteSpace(patientReferralCompleted.OutcomeDocumentID))
                        {
                            this.Result.IsSuccess = false;
                            this.Result.SetError(ErrorCode.PatientReferral_Referral_OutcomeDocumentID_Missing);
                            return this.Result;
                        }
                        this.Result = this.UploadDocument(MobiusAppSettingReader.LocalHomeCommunityID, patientReferralCompleted.OutcomeDocumentID,
                                               patientReferralCompleted.DocumentBytes, patientReferralCompleted.XACMLBytes, patientReferralCompleted.Patient.LocalMPIID, "Systems",
                                               Helper.SourceRepositoryId, true
                                               , patientReferralCompleted.Assertion);

                    }
                    transactionState = this.Result.IsSuccess;
                    if (this.Result.IsSuccess)
                    {
                        patientReferred = new PatientReferral();
                        patientReferred.Id = patientReferralCompleted.Id;
                        patientReferred.OutcomeDocumentID = patientReferralCompleted.OutcomeDocumentID;
                        patientReferred.DispatcherSummary = patientReferralCompleted.DispatcherSummary;
                        patientReferred.PatientAppointmentDate = patientReferralCompleted.PatientAppointmentDate;
                        patientReferred.ReferralAccomplishedOn = patientReferralCompleted.ReferralAccomplishedOn;
                        patientReferred.AcknowledgementStatus = patientReferralCompleted.AcknowledgementStatus;
                        patientReferred.ReferralCompleted = patientReferralCompleted.ReferralCompleted;


                        this.Result = this.MobiusDAL.AcknowledgePatientReferral(patientReferred, out referPatientId);
                        transactionState = this.Result.IsSuccess;
                        if (!this.Result.IsSuccess)
                        {
                            this.Result.SetError(ErrorCode.PatientReferral_Failed);
                            return this.Result;
                        }
                    }
                    if (transactionState)
                        transactionScope.Complete();
                    else
                        transactionScope.Dispose();
                }

                if (this.Result.IsSuccess && patientReferred.ReferralCompleted)
                {
                    // Send Mail.
                    EventActionData eventActionData = new EventActionData();
                    if (patientReferralCompleted.Id != 0)
                    {
                        eventActionData.Event = EventType.PatientReferralCompleted;
                        eventActionData.DispatcherSummary = AppendBodyContent(string.IsNullOrEmpty(patientReferralCompleted.DispatcherSummary) ? "    " : eventActionData.ReferralSummary = patientReferralCompleted.DispatcherSummary);
                        eventActionData.EmailRecipients.Add(patientReferralCompleted.ReferredByEmail);

                        this.Result = this.MobiusDAL.GetProviderDetails(patientReferralCompleted.ReferredByEmail, out providerDetails);
                        if (this.Result.IsSuccess && providerDetails.FirstName != null)
                        {
                            eventActionData.ReferralRequestor = providerDetails.FirstName;
                        }
                        else
                        {
                            eventActionData.ReferralRequestor = patientReferralCompleted.ReferredByEmail;
                        }
                    }
                    if (patientReferralCompleted.Patient.GivenName.Count > 0)
                    {
                        eventActionData.FirstName = patientReferralCompleted.Patient.GivenName[0];
                    }
                    if (patientReferralCompleted.Patient.FamilyName.Count > 0)
                    {
                        eventActionData.LastName = patientReferralCompleted.Patient.FamilyName[0];
                    }
                    eventActionData.Gender = patientReferralCompleted.Patient.Gender.ToString();
                    if (!string.IsNullOrWhiteSpace(patientReferralCompleted.Patient.DOB))
                        eventActionData.DOB = Convert.ToDateTime(patientReferralCompleted.Patient.DOB).ToShortDateString();
                    eventActionData.ReferralDispatcher = patientReferralCompleted.ReferredToEmail;
                    if (!string.IsNullOrWhiteSpace(patientReferralCompleted.ReferralOn))
                        eventActionData.ReferredOn = Convert.ToDateTime(patientReferralCompleted.ReferralOn).ToShortDateString();
                    if (!string.IsNullOrWhiteSpace(patientReferralCompleted.PatientAppointmentDate))
                        eventActionData.PatientAppointmentDate = Convert.ToDateTime(patientReferralCompleted.PatientAppointmentDate).ToShortDateString();
                    // eventActionData.ReferralDispatcher = patientReferralCompleted.ReferredToEmail;
                    eventActionData.DocumentName = patientReferralCompleted.OutcomeDocumentID;
                    eventActionData.ReferPatientId = referPatientId.ToString(); ;

                    //EventLogger eventLogger = new EventLogger();
                    eventLogger = new EventLogger();
                    eventLogger.LogEvent(eventActionData);
                    //Added for issue id #138
                    eventActionData = null;

                }

                if (this.Result.IsSuccess)
                {
                    this.Result.SetError(ErrorCode.Acknowledgement_Sent_Successfully);

                }
                else
                {
                    if(this.Result.ErrorCode==null)
                        this.Result.SetError(ErrorCode.PatientReferral_Failed);
                    return this.Result;
                }

            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);

            }
            //Added for issue id #138
            finally
            {
                patientReferred = null;
                eventLogger = null;
            }
            return this.Result;
        }
        #endregion

        #region PatientReferral

        public Result GetPatientReferralDetails(int patientReferralId, string referredToEmailAddress, string referredByEmailAddress, out  List<PatientReferral> patientReferrals)
        {
            patientReferrals = null;

            try
            {
                this.Result = this.MobiusDAL.GetPatientReferralDetails(patientReferralId, referredToEmailAddress, referredByEmailAddress, out patientReferrals);

                if (!this.Result.IsSuccess)
                {
                    this.Result.SetError(ErrorCode.PatientReferral_Failed);
                    return this.Result;
                }

            }
            catch (Exception ex)
            {

                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }

            return this.Result;
        }
        #endregion

        #region UpdatePatient
        /// <summary>
        /// Update Patient
        /// </summary>
        /// <param name="patientEntity"></param>
        /// <returns></returns>
        public Result UpdatePatient(Patient patientEntity)
        {
            string message = string.Empty;
            string OTPString = string.Empty;
            string PKCS7Response = string.Empty;
            DateTime date;

            if (patientEntity.Action.Count(t => t == Mobius.CoreLibrary.ActionType.Delete) >= patientEntity.GivenName.Count)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.Patient_First_Name_Missing);
                return this.Result;
            }

            if (patientEntity.Action.Count(t => t == Mobius.CoreLibrary.ActionType.Delete) >= patientEntity.FamilyName.Count)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.Patient_Last_Name_Missing);
                return this.Result;
            }
            if (string.IsNullOrWhiteSpace(patientEntity.Gender.ToString()))
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.Gender_Not_Provided);
                return this.Result;
            }
            if (patientEntity.GivenName.Count != 0)
            {
                foreach (string name in patientEntity.GivenName)
                {
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Patient_First_Name_Missing);
                        return this.Result;
                    }

                    if (!Validator.ValidateName(name, out message))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Name_Contains_Specialcharacater);
                        return this.Result;
                    }
                }
            }

            if (patientEntity.MiddleName.Count != 0)
            {
                foreach (string name in patientEntity.MiddleName)
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        if (!Validator.ValidateName(name, out message))
                        {
                            this.Result.IsSuccess = false;
                            this.Result.SetError(ErrorCode.Name_Contains_Specialcharacater);
                            return this.Result;
                        }
                    }
                }
            }

            if (patientEntity.FamilyName.Count != 0)
            {
                foreach (string name in patientEntity.FamilyName)
                {
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Patient_Last_Name_Missing);
                        return this.Result;
                    }

                    if (!Validator.ValidateName(name, out message))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Name_Contains_Specialcharacater);
                        return this.Result;
                    }
                }
            }
            if (patientEntity.MothersMaidenName != null)
            {
                string mothersMaidenName = (string.IsNullOrWhiteSpace(patientEntity.MothersMaidenName.Prefix) ? "" : (patientEntity.MothersMaidenName.Prefix + " "))
                    + (string.IsNullOrWhiteSpace(patientEntity.MothersMaidenName.GivenName) ? "" : (patientEntity.MothersMaidenName.GivenName + " "))
                    + (string.IsNullOrWhiteSpace(patientEntity.MothersMaidenName.MiddleName) ? "" : (patientEntity.MothersMaidenName.MiddleName + " "))
                    + (string.IsNullOrWhiteSpace(patientEntity.MothersMaidenName.FamilyName) ? "" : (patientEntity.MothersMaidenName.FamilyName + " "))
                    + (string.IsNullOrWhiteSpace(patientEntity.MothersMaidenName.Suffix) ? "" : (patientEntity.MothersMaidenName.Suffix + " "));


                if (!string.IsNullOrWhiteSpace(mothersMaidenName) && !Validator.ValidateName(mothersMaidenName, out message))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.MothersMaidenName_Contains_Specialcharacater);
                    return this.Result;
                }
            }
            if (string.IsNullOrWhiteSpace(patientEntity.DOB))
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.DOB_Not_Provided);
                return this.Result;
            }

            if (DateTime.TryParseExact(patientEntity.DOB, format, null, DateTimeStyles.None, out date) == false)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.Invalid_Date_Format);
                return this.Result;
            }
            if (Convert.ToDateTime(patientEntity.DOB) > DateTime.Now)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.DOB_Can_Not_Be_Greater_Than_Current);
                return this.Result;
            }

            if (string.IsNullOrWhiteSpace(patientEntity.EmailAddress))
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.Provider_Email_Address_Missing);
                return this.Result;
            }

            if (patientEntity.Telephone.Count > 0)
            {
                for (int i = 0; i < patientEntity.Telephone.Count; i++)
                {
                    if (!Validator.ValidatePhone(patientEntity.Telephone[i].Number, out message))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Invalid_Phone_Format);
                        return this.Result;
                    }
                }
            }

            if (patientEntity.PatientAddress != null)
            {

                foreach (Address address in patientEntity.PatientAddress)
                {

                    // Added for valid zipcode validation and as well as city, state and country validation
                    if (string.IsNullOrWhiteSpace(address.Zip) || patientEntity.PatientAddress.Count(t => t.Action == Mobius.CoreLibrary.ActionType.Delete) >= patientEntity.PatientAddress.Count(t => !string.IsNullOrEmpty(t.Zip)))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.ZipCode_Missing);
                        return this.Result;
                    }
                    else
                    {
                        City city = new City();
                        this.Result = this.MobiusDAL.GetLocalityByZipCode(address.Zip, out city);
                        if (!this.Result.IsSuccess)
                        {
                            this.Result.IsSuccess = false;
                            this.Result.SetError(ErrorCode.Invalid_Postal_Code);
                            return this.Result;
                        }

                        if (!string.IsNullOrEmpty(city.State.Country.CountryName) && city.State.Country.CountryName.ToUpper() == "US")
                        {
                            city.State.Country.CountryName = "USA";
                            address.City = city;
                        }

                    }
                    // Ennds addition

                    if (string.IsNullOrEmpty(address.AddressLine1) && string.IsNullOrEmpty(address.AddressLine2) ||
                         patientEntity.PatientAddress.Count(t => t.Action == Mobius.CoreLibrary.ActionType.Delete) >= patientEntity.PatientAddress.Count(t => !string.IsNullOrEmpty(t.AddressLine1)) &&
                         patientEntity.PatientAddress.Count(t => t.Action == Mobius.CoreLibrary.ActionType.Delete) >= patientEntity.PatientAddress.Count(t => !string.IsNullOrEmpty(t.AddressLine2)))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.One_Address_Is_Required);
                        return this.Result;
                    }
                    if (!string.IsNullOrEmpty(address.AddressLine1))
                    {
                        Validator.ValidateAddress(address.AddressLine1, out message);
                        if (message.Length == 0)
                        {
                            this.Result.IsSuccess = false;
                            this.Result.SetError(ErrorCode.Invalid_Address);
                            return this.Result;
                        }
                        address.AddressLine1 = message;
                    }
                    if (!string.IsNullOrEmpty(address.AddressLine2))
                    {
                        Validator.ValidateAddress(address.AddressLine2, out message);
                        if (message.Length == 0)
                        {
                            this.Result.IsSuccess = false;
                            this.Result.SetError(ErrorCode.Invalid_Address);
                            return this.Result;
                        }
                        address.AddressLine2 = message;
                    }
                }
            }


            if (!string.IsNullOrWhiteSpace(patientEntity.SSN))
            {
                if (!Validator.ValidateSSN(patientEntity.SSN, out message))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Invalid_SSN);
                    return this.Result;
                }
            }


            if (string.IsNullOrWhiteSpace(patientEntity.CommunityId))
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.Document_communityId_Missing);
                return this.Result;
            }

            // validation REST TO DO
            if (!Validator.ValidateEmail(patientEntity.EmailAddress, out message))
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.Provider_Invalid_Email, message);
                return this.Result;
            }


            if (!string.IsNullOrEmpty(patientEntity.BirthPlaceZip))
            {
                City birthCity = null;
                this.Result = this.MobiusDAL.GetLocalityByZipCode(patientEntity.BirthPlaceZip, out birthCity);
                if (!this.Result.IsSuccess)
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Invalid_Postal_Code);
                    return this.Result;
                }
            }





            this.Result = this.MobiusDAL.UpdatePatient(patientEntity);
            if (this.Result.IsSuccess)
            {
                this.Result.SetError(ErrorCode.Patient_Updated);
            }

            return this.Result;
        }
        #endregion

        #region GetPatientDetails
        /// <summary>
        /// Get Patient Details behalf on MPIID or Email Address
        /// </summary>
        /// <param name="patient"></param>
        /// <param name="MPIID"></param>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public Result GetPatientDetails(out Patient patient, string MPIID = "", string emailAddress = "")
        {
            patient = null;
            try
            {
                this.Result = this.MobiusDAL.GetPatientDetails(out patient, MPIID: MPIID, email: emailAddress);

                if (!this.Result.IsSuccess)
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Patient_Details_Not_Found);
                    return this.Result;
                }
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);

            }
            return this.Result;
        }
        #endregion

        #region AddPatient
        /// <summary>
        /// Add New Patient
        /// </summary>
        /// <param name="patient">patient class object</param>
        /// <param name="PKCS7Response">out string</param>
        /// <returns>return Result class object</returns>
        public Result AddPatient(Patient patient, out string PKCS7Response)
        {
            string message = string.Empty;
            string OTPString = string.Empty;
            PKCS7Response = string.Empty;
            int pendingEnrollmentRequestId = 0;
            CertificateEnrollment certificateEnrollment = null;
            DateTime date;

            try
            {
                if (patient.GivenName.Count == 0)
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Patient_First_Name_Missing);
                    return this.Result;
                }
                if (patient.FamilyName.Count == 0)
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Patient_Last_Name_Missing);
                    return this.Result;
                }

                if (patient.Prefix.Count != 0)
                {
                    foreach (string name in patient.Prefix)
                    {
                        if (!string.IsNullOrEmpty(name))
                        {
                            if (!Validator.ValidateName(name, out message))
                            {
                                this.Result.IsSuccess = false;
                                this.Result.SetError(ErrorCode.Name_Contains_Specialcharacater);
                                return this.Result;
                            }
                        }
                    }
                }

                if (patient.GivenName.Count != 0)
                {
                    foreach (string name in patient.GivenName)
                    {
                        if (string.IsNullOrWhiteSpace(name))
                        {
                            this.Result.IsSuccess = false;
                            this.Result.SetError(ErrorCode.Patient_First_Name_Missing);
                            return this.Result;
                        }

                        if (!Validator.ValidateName(name, out message))
                        {
                            this.Result.IsSuccess = false;
                            this.Result.SetError(ErrorCode.Name_Contains_Specialcharacater);
                            return this.Result;
                        }
                    }
                }

                if (patient.MiddleName.Count != 0)
                {
                    foreach (string name in patient.MiddleName)
                    {
                        if (!string.IsNullOrEmpty(name))
                        {
                            if (!Validator.ValidateName(name, out message))
                            {
                                this.Result.IsSuccess = false;
                                this.Result.SetError(ErrorCode.Name_Contains_Specialcharacater);
                                return this.Result;
                            }
                        }
                    }
                }

                if (patient.FamilyName.Count != 0)
                {
                    foreach (string name in patient.FamilyName)
                    {
                        if (string.IsNullOrWhiteSpace(name))
                        {
                            this.Result.IsSuccess = false;
                            this.Result.SetError(ErrorCode.Patient_Last_Name_Missing);
                            return this.Result;
                        }

                        if (!Validator.ValidateName(name, out message))
                        {
                            this.Result.IsSuccess = false;
                            this.Result.SetError(ErrorCode.Name_Contains_Specialcharacater);
                            return this.Result;
                        }
                    }
                }


                if (patient.Suffix.Count != 0)
                {
                    foreach (string name in patient.Suffix)
                    {
                        if (!string.IsNullOrEmpty(name))
                        {
                            if (!Validator.ValidateName(name, out message))
                            {
                                this.Result.IsSuccess = false;
                                this.Result.SetError(ErrorCode.Name_Contains_Specialcharacater);
                                return this.Result;
                            }
                        }
                    }
                }





                if (patient.MothersMaidenName != null)
                {
                    string mothersMaidenName = (string.IsNullOrWhiteSpace(patient.MothersMaidenName.Prefix) ? "" : (patient.MothersMaidenName.Prefix + " "))
                        + (string.IsNullOrWhiteSpace(patient.MothersMaidenName.GivenName) ? "" : (patient.MothersMaidenName.GivenName + " "))
                        + (string.IsNullOrWhiteSpace(patient.MothersMaidenName.MiddleName) ? "" : (patient.MothersMaidenName.MiddleName + " "))
                        + (string.IsNullOrWhiteSpace(patient.MothersMaidenName.FamilyName) ? "" : (patient.MothersMaidenName.FamilyName + " "))
                        + (string.IsNullOrWhiteSpace(patient.MothersMaidenName.Suffix) ? "" : (patient.MothersMaidenName.Suffix + " "));
                    if (!string.IsNullOrWhiteSpace(mothersMaidenName) && !Validator.ValidateName(mothersMaidenName, out message))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.MothersMaidenName_Contains_Specialcharacater);
                        return this.Result;
                    }
                }

                if (string.IsNullOrWhiteSpace(patient.Gender.ToString()))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Gender_Not_Provided);
                    return this.Result;
                }
                if (string.IsNullOrWhiteSpace(patient.DOB))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.DOB_Not_Provided);
                    return this.Result;
                }
                if (DateTime.TryParseExact(patient.DOB, format, null, DateTimeStyles.None, out date) == false)
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Invalid_Date_Format);
                    return this.Result;
                }
                if (Convert.ToDateTime(patient.DOB) > DateTime.Now)
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.DOB_Can_Not_Be_Greater_Than_Current);
                    return this.Result;
                }

                if (string.IsNullOrWhiteSpace(patient.EmailAddress))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Provider_Email_Address_Missing);
                    return this.Result;
                }

                if (!Validator.ValidateEmail(patient.EmailAddress, out message))
                {

                    this.Result.IsSuccess = false;
                    patient.EmailAddress = message;
                    this.Result.SetError(ErrorCode.Invalid_Email_Address);
                    return this.Result;
                }

                if (string.IsNullOrWhiteSpace(patient.Password))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Password_Missing);
                    return this.Result;
                }

                if (!Validator.ValidatePassword(patient.Password, out message))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Invalid_Password);
                    return this.Result;
                }
                else
                {
                    patient.Password = Helper.EncryptData(patient.Password);
                }


                if (patient.PatientAddress != null)
                {
                    foreach (Address address in patient.PatientAddress)
                    {

                        if (string.IsNullOrWhiteSpace(address.Zip))
                        {
                            this.Result.IsSuccess = false;
                            this.Result.SetError(ErrorCode.ZipCode_Missing);
                            return this.Result;
                        }
                        if (!Validator.ValidatePostalCode(address.Zip, out message))
                        {
                            this.Result.IsSuccess = false;
                            this.Result.SetError(ErrorCode.Invalid_Postal_Code);
                            return this.Result;
                        }
                        else
                        {
                            City city = new City();
                            this.Result = this.MobiusDAL.GetLocalityByZipCode(address.Zip, out city);
                            if (!this.Result.IsSuccess)
                            {
                                this.Result.IsSuccess = false;
                                this.Result.SetError(ErrorCode.Invalid_Postal_Code);
                                return this.Result;
                            }

                            if (!string.IsNullOrEmpty(city.State.Country.CountryName) && city.State.Country.CountryName.ToUpper() == "US")
                            {
                                city.State.Country.CountryName = "USA";
                                address.City = city;
                            }


                        }

                        if (string.IsNullOrEmpty(address.AddressLine1) && string.IsNullOrEmpty(address.AddressLine2))
                        {
                            this.Result.IsSuccess = false;
                            this.Result.SetError(ErrorCode.One_Address_Is_Required);
                            return this.Result;
                        }
                        if (!string.IsNullOrEmpty(address.AddressLine1))
                        {
                            Validator.ValidateAddress(address.AddressLine1, out message);
                            if (message.Length == 0)
                            {
                                this.Result.IsSuccess = false;
                                this.Result.SetError(ErrorCode.Invalid_Address);
                                return this.Result;
                            }
                            address.AddressLine1 = message;
                        }
                        if (!string.IsNullOrEmpty(address.AddressLine2))
                        {
                            Validator.ValidateAddress(address.AddressLine2, out message);
                            if (message.Length == 0)
                            {
                                this.Result.IsSuccess = false;
                                this.Result.SetError(ErrorCode.Invalid_Address);
                                return this.Result;
                            }
                            address.AddressLine2 = message;
                        }
                    }
                }


                if (patient.Telephone.Count > 0)
                {
                    foreach (Telephone telephone in patient.Telephone)
                    {
                        if (!Validator.ValidatePhone(telephone.Number, out message))
                        {
                            this.Result.IsSuccess = false;
                            this.Result.SetError(ErrorCode.Invalid_Phone_Format);
                            return this.Result;
                        }
                        if (!Validator.ValidateNumeric(patient.Telephone[0].Extensionnumber))
                        {
                            this.Result.IsSuccess = false;
                            this.Result.SetError(ErrorCode.Invalid_Extension_Format);
                            return this.Result;
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(patient.SSN))
                {
                    if (!Validator.ValidateSSN(patient.SSN, out message))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Invalid_SSN);
                        return this.Result;
                    }
                }

                if (string.IsNullOrWhiteSpace(patient.CommunityId))
                {
                    this.Result.IsSuccess = false;
                    // this.Result.SetError(ErrorCode.Document_communityId_Missing, "Community Id is not provided.");
                    this.Result.SetError(ErrorCode.Document_communityId_Missing);
                    return this.Result;
                }

                // validation REST TO DO

                if (string.IsNullOrWhiteSpace(patient.CSR))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.CertificateAuthority_CSR_Missing);
                    return this.Result;
                }


                // Check existence of Patient
                this.Result = this.CheckPatient(patient);
                if (this.Result.IsSuccess)
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.patient_Allready_Exist);
                    return this.Result;
                }
                else
                {
                    // Genrate Local MPIID 
                    patient.LocalMPIID = GenrateMPIID();

                    certificateEnrollment = new CertificateEnrollment();
                    // ValidateEnrollmentRequest validate the CSR
                    this.Result = certificateEnrollment.ValidateEnrollmentRequest(patient.CSR);
                    if (this.Result.IsSuccess)
                    {  // EnrollCertificateRequest will accepts the PKCS#10 request and cerate the certificate for the requestor 
                        this.Result = certificateEnrollment.EnrollCertificateRequest(patient.CSR, out PKCS7Response);
                        if (!string.IsNullOrEmpty(PKCS7Response))
                        {
                            this.Result = certificateEnrollment.GetCertificateInformation(PKCS7Response);
                            patient.SerialNumber = certificateEnrollment.SerialNumber;
                            patient.PublicKey = certificateEnrollment.PublicKey;
                            patient.CreatedOn = DateTime.Now.ToString();
                            patient.ExpiryOn = certificateEnrollment.ExpirationDate.ToString();
                        }
                        if (this.Result.IsSuccess)
                        {
                            this.Result = this.MobiusDAL.AddPatient(patient);
                            if (this.Result.IsSuccess)
                                this.Result.SetError(ErrorCode.patient_Registration_successful);
                            else
                                return this.Result;
                        }
                    }
                }


                this.AddLogEvent(this.Result.IsSuccess ? EventType.RegisterPatient : EventType.RegisterPatientFailed,
                                    pendingEnrollmentRequestId,
                                    true,
                                    patient.LocalMPIID,
                                    patient.GivenName.FirstOrDefault(),
                                    string.Empty,
                                    patient.EmailAddress,
                                    string.Empty,
                                    OTPString,
                                    Helper.GetIPAddress(),
                                    2,
                                    patient.CommunityId,
                                    string.Empty,
                                    (object)patient,
                                    this.Result.IsSuccess ? string.Empty : this.Result.ErrorMessage,
                                    string.Empty,
                                    REGISTER,
                                    Mobius.CoreLibrary.UserType.Patient.ToString(),
                                    UserName: patient.EmailAddress);

            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
                this.AddLogEvent(EventType.RegisterPatientFailed,
                                    pendingEnrollmentRequestId,
                                    true,
                                    patient.LocalMPIID,
                                    patient.GivenName.FirstOrDefault(),
                                    string.Empty,
                                    patient.EmailAddress,
                                    string.Empty,
                                    OTPString,
                                    Helper.GetIPAddress(),
                                    2,
                                    patient.CommunityId,
                                    string.Empty,
                                    (object)patient,
                                    this.Result.IsSuccess ? string.Empty : this.Result.ErrorMessage,
                                    string.Empty,
                                    REGISTER,
                                    Mobius.CoreLibrary.UserType.Patient.ToString(),
                                    UserName: patient.EmailAddress);

            }
            finally
            {
                certificateEnrollment = null;
                patient = null;
            }
            return this.Result;
        }
        #endregion

        #region GetLocalityByZipCode
        /// <summary>
        /// Get City,State,Country via zipcode
        /// </summary>
        /// <param name="zipCode">string type</param>
        /// <param name="city">city class object</param>
        /// <returns>result class object</returns>
        public Result GetLocalityByZipCode(string zipCode, out City city)
        {
            city = new City();
            string message = string.Empty;
            if (string.IsNullOrWhiteSpace(zipCode))
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.ZipCode_Missing);
                return this.Result;
            }
            if (!Validator.ValidatePostalCode(zipCode, out message))
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.Invalid_Postal_Code);
                return this.Result;
            }
            else
            {
                this.Result = this.MobiusDAL.GetLocalityByZipCode(zipCode, out city);
            }


            return this.Result;

        }
        #endregion

        #region Helper Public Method

        /// <summary>
        /// Get PCKS Response from data behalf on  oneTimePassword 
        /// </summary>
        /// <param name="oneTimePassword"></param>
        /// <returns>return string</returns>
        public Result GetPCKSResponse(string oneTimePassword, out string pCKS7)
        {
            pCKS7 = string.Empty;
            try
            {
                this.Result = this.MobiusDAL.GetPCKSResponse(oneTimePassword, out pCKS7);

            }
            catch (Exception)
            {
                // TODO
            }
            return this.Result;
        }
        /// <summary>
        ///  Delete one time password  through database
        /// </summary>
        /// <param name="oneTimePassword"></param>
        /// <summary>
        public void DeleteOTP(string oneTimePassword)
        {
            try
            {
                this.MobiusDAL.DeleteOTP(oneTimePassword);

            }
            catch (Exception)
            {
                // TODO
            }

        }
        /// <summary>
        /// Get MPIID
        /// </summary>
        /// <returns>return MPIID</returns>
        private string GenrateMPIID()
        {
            var chars = _RandomChars;
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }

        #endregion Helper Method

        #region GetPatientConsent

        /// <summary>
        /// GetPatientConsent method would fetch consent for given MPIID 
        /// </summary>
        /// <param name="MPIID">MPIID of the patient</param>
        /// <param name="patientConsent">patient consent object</param>
        /// <returns>Result object</returns>
        public Result GetPatientConsent(string MPIID, out List<MobiusPatientConsent> patientConsent)
        {
            patientConsent = null;

            try
            {
                if (string.IsNullOrEmpty(MPIID))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.patient_MPIID_Missing);
                    return this.Result;
                }

                this.Result = this.MobiusDAL.GetPatientConsent(MPIID, out patientConsent);
                if (!this.Result.IsSuccess)
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Patient_Consent_Does_Not_Exist);
                }
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return this.Result;
        }
        #endregion

        #region GetPatientConsentById
        /// <summary>
        /// Gets patientconsent using mpiid and consentId
        /// </summary>
        /// <param name="MPIID"></param>
        /// <param name="patientConsentId"></param>
        /// <param name="patientConsent"></param>
        /// <param name="C32Sections"></param>
        /// <param name="isEmergencyOverride"></param>
        /// <returns></returns>
        public Result GetPatientConsentByConsentId(string MPIID, int patientConsentId, out MobiusPatientConsent patientConsent,
            out List<C32Section> C32Sections)
        {
            patientConsent = null;
            C32Sections = null;

            try
            {
                if (string.IsNullOrEmpty(MPIID))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.patient_MPIID_Missing);
                    return this.Result;
                }
                // If consent is not given and case of Normal Access
                if (patientConsentId <= 0)
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.patient_Consent_ID);
                    return this.Result;
                }
              
                    //this.Result = this.MobiusDAL.GetPatientConsentByConsentId(MPIID, patientConsentId, out patientConsent);
                    this.Result = this.MobiusDAL.GetPatientConsentByConsentId(MPIID, patientConsentId, out patientConsent);
              
                //load the masterData from db on success 
                if (this.Result.IsSuccess)
                {
                    this.Result = this.MobiusDAL.GetC32Sections(out C32Sections);
                    // on success use left outer join to fill the records 
                    if (this.Result.IsSuccess)
                    {
                        //TODO:Rajanee: Applied subscript which was not earlier applied
                        //this.GetC32SectionsWithConsent(C32Sections, patientConsent[0]);
                        this.GetC32SectionsWithConsent(C32Sections, patientConsent);
                    }

                }
                else
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Patient_Consent_Does_Not_Exist);
                }
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return this.Result;

        }
        #endregion

        #region GetSpecificPatientConsent

        /// <summary>
        /// Get Specific Patient Consent
        /// </summary>
        /// <param name="MPIID">string</param>
        /// <param name="patientConsentId"></param>
        /// <param name="dsSpecificPatientConsent"></param>
        /// <returns>return Result class object</returns>
        //public Result GetSpecificPatientConsent(string MPIID, int patientConsentId, out DataSet dsSpecificPatientConsent)
        //{
        //    dsSpecificPatientConsent = new DataSet();

        //    if (string.IsNullOrEmpty(MPIID))
        //    {
        //        this.Result.IsSuccess = false;
        //        this.Result.SetError(ErrorCode.patient_MPIID_Missing);
        //        return this.Result;
        //    }
        //    if (patientConsentId <= 0)
        //    {
        //        this.Result.IsSuccess = false;
        //        this.Result.SetError(ErrorCode.patient_Consent_ID);
        //        return this.Result;
        //    }

        //    try
        //    {
        //        this.Result = this.MobiusDAL.GetSpecificPatientConsent(MPIID, patientConsentId, out dsSpecificPatientConsent);
        //        if (dsSpecificPatientConsent == null)
        //        {
        //            this.Result.IsSuccess = false;
        //            this.Result.SetError(ErrorCode.Patient_Consent_Does_Not_Exist);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.Result.IsSuccess = false;
        //        this.Result.SetError(ErrorCode.UnknownException);
        //        Helper.LogError(ex);
        //    }
        //    return this.Result;
        //}

        #endregion

        #region CheckConsentPolicy
        /// <summary>
        /// Check Policy
        /// </summary>
        /// <param name="checkPolicyRequest">checkPolicyRequest object</param>
        /// <param name="assertion"></param>
        /// <returns>return CheckPolicyResponseType class object</returns>
        public CheckPolicyResponseType CheckPolicy(CheckPolicyRequestType checkPolicyRequest, PolicyEngine.AssertionType assertion)
        {
            CheckPolicyResponseType checkPolicyResponseType = null;
            ResultType resultType = new ResultType();
            try
            {
                if (checkPolicyRequest != null)
                {
                    using (AdapterPolicyEngine adapterPolicyEngine = new AdapterPolicyEngine())
                    {
                        this.Result = adapterPolicyEngine.ProcessPolicyEngineRequest(checkPolicyRequest);
                        return adapterPolicyEngine.CheckPolicyResponseType;
                    }
                }
            }
            catch (Exception ex)
            {
                resultType.Decision = DecisionType.Deny;
                resultType.Status = new StatusType();
                resultType.Status.StatusMessage = ex.Message.ToString();
                checkPolicyResponseType.response = new ResultType[1] { resultType };
            }
            return checkPolicyResponseType;

        }


        /// <summary>
        /// Check Policy
        /// </summary>
        /// <param name="checkPolicyRequest">checkPolicyRequest object</param>
        /// <param name="assertion"></param>
        /// <returns>return CheckPolicyResponseType class object</returns>
        public AdapterPEP.CheckPolicyResponseType CheckPolicyPEP(AdapterPEP.CheckPolicyRequestType checkPolicyRequest, AdapterPEP.AssertionType assertion)
        {
            AdapterPEP.CheckPolicyResponseType checkPolicyResponseType = null;
            AdapterPEP.ResultType resultType = new AdapterPEP.ResultType();
            try
            {
                if (checkPolicyRequest != null)
                {
                    using (PEP adapterPolicyEngine = new PEP())
                    {
                        this.Result = adapterPolicyEngine.ProcessPolicyEngineRequest(checkPolicyRequest);
                        return adapterPolicyEngine.CheckPolicyResponseType;
                    }
                }
            }
            catch (Exception ex)
            {
                resultType.Decision = AdapterPEP.DecisionType.Deny;
                resultType.Status = new AdapterPEP.StatusType();
                resultType.Status.StatusMessage = ex.Message.ToString();
                checkPolicyResponseType.response = new AdapterPEP.ResultType[1] { resultType };
            }
            return checkPolicyResponseType;

        }

        #endregion

        #region UpdatePatientConsentPolicy

        /// <summary>
        /// Update Patient Consent Policy
        /// </summary>
        /// <param name="patientConsentPolicy"></param>
        /// <returns>return Result class object</returns>
        public Result UpdatePatientConsentPolicy(MobiusPatientConsent patientConsentPolicy)
        {
            try
            {
                if (string.IsNullOrEmpty(patientConsentPolicy.MPIID))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.patient_MPIID_Missing);
                    return this.Result;
                }
                if (patientConsentPolicy.RoleId == 0)
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Role_ID);
                    return this.Result;
                }
                if (patientConsentPolicy.PurposeOfUseId == 0)
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.PurposeOf_UseId_Missing);
                    return this.Result;
                }

                if (patientConsentPolicy.RuleStartDate == null)
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.RuleStart_Date_Missing);
                    return this.Result;
                }
                if (patientConsentPolicy.RuleEndDate == null)
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.RuleEnd_Date_Missing);
                    return this.Result;
                }

                // Check Patient Consent Existence
                this.Result = MobiusDAL.CheckPatientConsentPolicyExistence(patientConsentPolicy);
                // Insert new Consent but Consent already exist
                // Insert new Consent but Consent already exist
                if ((patientConsentPolicy.PatientConsentID <= 0) && (Result.IsSuccess))
                {
                    //if ((patientConsentPolicy.Active) && (Result.IsSuccess))
                    //{
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.CONSENT_DIRECTIVE_ALREADY_MAPPED);
                    return this.Result;

                } // Insert or Update Consent
                else
                {
                    this.Result = MobiusDAL.UpdatePatientConsentPolicy(patientConsentPolicy);

                    if (patientConsentPolicy.Active && this.Result.IsSuccess)
                    {
                        this.Result.SetError(ErrorCode.PERMISSION_CATEGORY_SUCCESSFULLY_CREATED);
                    }
                    else if (!patientConsentPolicy.Active && this.Result.IsSuccess)
                    {
                        this.Result.SetError(ErrorCode.PERMISSION_CATEGORY_SUCCESSFULLY_UPDATED);
                    }
                    else
                    {
                        this.Result.SetError(ErrorCode.CONSENT_FAILED);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return this.Result;
        }

        #endregion

        #region DeletePatientConsent

        /// <summary>
        /// Delete Patient Consent
        /// </summary>
        /// <param name="patientConsentId">string</param>
        /// <returns>return Result class object</returns>
        public Result DeletePatientConsent(string patientConsentId)
        {
            try
            {
                if (string.IsNullOrEmpty(patientConsentId))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.patient_Consent_ID);
                    return this.Result;
                }
                this.Result = this.MobiusDAL.DeletePatientConsent(patientConsentId);
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return this.Result;
        }

        #endregion

        #region GetUserInformation
        /// <summary>
        /// Check Type Of User
        /// </summary>
        /// <param name="serialNumber">string</param>
        /// <returns>Result class</returns>
        public Result GetUserInformation(string serialNumber, out UserInfo userInformation)
        {
            userInformation = null;
            //Added for issue id #138
            Authorization authorization = null;
            try
            {
                if (string.IsNullOrEmpty(serialNumber))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Not_Valid_Certificate);
                    return this.Result;
                }
                //Modified for issue id #138
                authorization = new Authorization();
                this.Result = authorization.GetUserInformation(serialNumber, out userInformation);
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            //Added for issue id #138
            finally
            {
                authorization = null;
            }

            return this.Result;
        }
        #endregion

        #region UpdateOptInStatus

        /// <summary>
        /// Update OptIn Status
        /// </summary>
        /// <param name="MPIID">string</param>
        /// <param name="isOptIn"></param>
        /// <returns>return Result class object</returns>
        public Result UpdateOptInStatus(string MPIID, bool isOptIn)
        {
            try
            {
                if (string.IsNullOrEmpty(MPIID))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.patient_MPIID_Missing);
                    return this.Result;
                }


                this.Result = this.MobiusDAL.UpdateOptInStatus(MPIID, isOptIn);
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return this.Result;
        }
        #endregion

        #region Authenticate User
        /// <summary>
        /// Authenticate User using EmailAddress and Password
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="password"></param>
        /// <param name="userType"></param>
        /// <param name="certificateSerialNumber"></param>
        /// <param name="userName"></param>
        /// <returns>return Result class object</returns>
        public Result AuthenticateUser(string emailAddress, string password, int userType, out string certificateSerialNumber, out string userName)
        {
            certificateSerialNumber = string.Empty;
            userName = string.Empty;
            string message = string.Empty;
            MobiusDAL authenticateUser = null;
            try
            {
                if (string.IsNullOrEmpty(emailAddress))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.UserId_Missing);
                    return this.Result;
                }
                if (string.IsNullOrEmpty(password))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Password_Missing);
                    return this.Result;
                }
                // EncryptData password
                password = Helper.EncryptData(password);
                authenticateUser = new MobiusDAL();
                this.Result = authenticateUser.AuthenticateUser(emailAddress, password, userType, out certificateSerialNumber, out userName);
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return this.Result;
        }
        #endregion

        #region ForgotPassword
        /// <summary>
        /// This method will check user Type and verifies if exists in database then generate new password and send mail to requested user.
        /// </summary>
        /// <param name="forgotPasswordRequest"></param>
        /// <returns></returns>
        public Result ForgotPassword(ForgotPassword forgotPasswordRequest)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(forgotPasswordRequest.EmailAddress))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.EmailAddress_Missing);
                    return this.Result;
                }

                GenerateStrongPassword generateStrongPassword = new GenerateStrongPassword();
                string newPassword = generateStrongPassword.GeneratePassword(8);
                generateStrongPassword = null;
                switch (forgotPasswordRequest.UserType)
                {
                    case Mobius.CoreLibrary.UserType.Patient:
                        this.Result = this.MobiusDAL.UpdatePatientPassword(forgotPasswordRequest.EmailAddress, Helper.EncryptData(newPassword));
                        break;
                    case Mobius.CoreLibrary.UserType.Provider:
                        this.Result = this.MobiusDAL.UpdateProviderPassword(forgotPasswordRequest.EmailAddress, Helper.EncryptData(newPassword));
                        break;
                    case Mobius.CoreLibrary.UserType.Unspecified:
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Invalid_UserType);
                        break;
                }

                if (this.Result.IsSuccess)
                {
                    EventLogger eventLogger = new EventLogger();
                    EventActionData eventActionData = new EventActionData();
                    string userName = "";
                    string certificateNumber = "";
                    if (this.AuthenticateUser(forgotPasswordRequest.EmailAddress, newPassword, forgotPasswordRequest.UserType.GetHashCode(), out certificateNumber, out userName).IsSuccess)
                    {
                        this.Result.SetError(ErrorCode.CheckEmailForNewPassword);
                        this.Result.IsSuccess = true;
                        eventActionData.UserName = userName;
                        eventActionData.EmailRecipients.Add(forgotPasswordRequest.EmailAddress);
                        eventActionData.Event = Mobius.CoreLibrary.UserType.Patient == CoreLibrary.UserType.Patient ? EventType.PasswordResetForPatient : EventType.PasswordResetForProvider;
                        eventActionData.Password = newPassword;
                        eventActionData.Purpose = "FORGOTPASSWORD";
                        eventActionData.Subject = forgotPasswordRequest.UserType.ToString();
                        eventLogger.LogEvent(eventActionData);
                    }
                    eventLogger = null;
                    eventActionData = null;
                }
                //else
                //{
                //    this.Result.SetError(ErrorCode.Invalid_UserId);
                //}

            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return this.Result;
        }
        #endregion ForgotPassword

        #region ChangePassword
        /// <summary>
        /// Change user Password
        /// </summary>
        /// <param name="changePassword">ChangePassword request object</param>
        /// <returns>returns changed status true or false</returns>
        public Result ChangePassword(ChangePassword changePassword)
        {
            #region Variables
            string message = string.Empty;
            string certificateSerialNumber = string.Empty;
            string userName = string.Empty;
            MobiusDAL authenticateUser = null;
            #endregion
            try
            {
                if (string.IsNullOrEmpty(changePassword.EmailAddress))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.EmailAddress_Missing);
                    return this.Result;
                }
                if (string.IsNullOrEmpty(changePassword.NewPassword))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.New_Password_Missing);
                    return this.Result;
                }
                if (string.IsNullOrEmpty(changePassword.OldPassword))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Old_Password_Missing);
                    return this.Result;
                }

                if (!Validator.ValidatePassword(changePassword.NewPassword, out message))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Invalid_Password);
                    return this.Result;
                }
                if (changePassword.OldPassword == changePassword.NewPassword)
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.OldPassword_And_NewPassword_Equal);
                    return this.Result;

                }
                authenticateUser = new MobiusDAL();
                // EncryptData password
                changePassword.OldPassword = Helper.EncryptData(changePassword.OldPassword);
                changePassword.NewPassword = Helper.EncryptData(changePassword.NewPassword);

                this.Result = authenticateUser.AuthenticateUser(changePassword.EmailAddress, changePassword.OldPassword, changePassword.UserType.GetHashCode(), out certificateSerialNumber, out userName);
                if (!this.Result.IsSuccess)
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Incorrect_Old_Password);
                    return this.Result;
                }

                this.Result = this.MobiusDAL.ChangePassword(changePassword);
                if (this.Result.IsSuccess)
                {
                    this.Result.SetError(ErrorCode.Password_Change_Successfull);
                    return this.Result;
                }
                else
                {
                    this.Result.SetError(ErrorCode.Password_Reset_Failed);
                    return this.Result;
                }
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return this.Result;

        }
        #endregion

        #region GetApplicationVersion
        /// <summary>
        /// This method would return the Application version
        /// </summary>
        /// <returns></returns>
        public string GetApplicationVersion()
        {
            string applicationVersion = string.Empty;
            try
            {
                applicationVersion = MobiusAppSettingReader.AndroidApplicationVersion;

            }
            catch (Exception ex)
            {
                applicationVersion = string.Empty;
            }
            return applicationVersion;

        }
        #endregion

        #region GetPatientInformationviaDocumentID
        /// <summary>
        /// Get Patient Information via DocumentID
        /// </summary>
        /// <param name="DocumentID"></param>
        /// <param name="patient"></param>
        /// <returns></returns>
        public Result GetPatientInformationByDocumentID(string DocumentID, out Patient patient)
        {
            patient = null;
            try
            {
                if (string.IsNullOrEmpty(DocumentID))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Document_DocumentId_Missing);
                    return this.Result;
                }

                this.Result = this.MobiusDAL.GetPatientInformationByDocumentID(DocumentID, out patient);
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return this.Result;
        }
        #endregion

        #region AddPFXCertificate
        /// <summary>
        /// Insert dynamically generated PFX certificate(base64 format)
        ///  into data base Patient and provider table for activate user on new devices
        /// </summary>
        /// <param name="pFXCertificate"></param>
        /// <returns></returns>
        public Result AddPFXCertificate(PFXCertificate pFXCertificate)
        {
            try
            {
                if (string.IsNullOrEmpty(pFXCertificate.EmailAddress))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.EmailAddress_Missing);
                    return this.Result;
                }
                if (string.IsNullOrEmpty(pFXCertificate.Certificate))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.PFX_Certificate_Not_Found);
                    return this.Result;
                }

                this.Result = this.MobiusDAL.AddPFXCertificate(pFXCertificate);
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return this.Result;
        }
        #endregion

        #region GetPFXCertificate
        /// <summary>
        ///  Get PFXCertificate into data base Patient and provider table
        /// </summary>
        /// <param name="pFXCertificate"></param>
        /// <returns></returns>
        public Result GetPFXCertificate(ref PFXCertificate pFXCertificate)
        {
            try
            {
                if (string.IsNullOrEmpty(pFXCertificate.EmailAddress))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.EmailAddress_Missing);
                    return this.Result;
                }
                this.Result = this.MobiusDAL.GetPFXCertificate(ref pFXCertificate);
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return this.Result;
        }
        #endregion

        #region Deactivate User
        /// <summary>
        ///  Get PFXCertificate into data base Patient and provider table
        /// </summary>
        /// <param name="pFXCertificate"></param>
        /// <returns></returns>
        public Result UpdateUserStatus(string emailAddress, int userType, bool isActive, string userName)
        {
            string message = string.Empty;
            EventLogger eventLogger = null;
            EventActionData eventActionData = null;
            try
            {
                if (string.IsNullOrEmpty(emailAddress))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.EmailAddress_Missing);
                    return this.Result;
                }
                if (!Validator.ValidateEmail(emailAddress, out message))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Invalid_Email_Address);
                    return this.Result;
                }
                this.Result = this.MobiusDAL.UpdateUserStatus(emailAddress, userType, isActive ? 1 : 0);
                if (this.Result.IsSuccess && isActive)
                {
                    eventLogger = new EventLogger();
                    eventActionData = new EventActionData();
                    eventActionData.UserName = userName;
                    eventActionData.EmailRecipients.Add(emailAddress);
                    eventActionData.RecipientEmail = emailAddress;
                    eventActionData.Event = EventType.UserAccountActivated;
                    eventLogger.LogEvent(eventActionData);
                    eventLogger = null;
                    eventActionData = null;
                }
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return this.Result;
        }
        #endregion

        #region GetPHISource
        /// <summary>
        /// This method will return the correlation records of selected Patient
        /// </summary>
        /// <param name="assigningAuthorityId">Home Community Id</param>
        /// <param name="patientId">Patient Id /MPPID</param>
        /// <param name="patientIdentifiers">Collection of correlation records </param>
        /// <returns></returns>
        public Result GetPHISource(string assigningAuthorityId, string patientId, out List<RemotePatientIdentifier> patientIdentifiers)
        {
            patientIdentifiers = null;
            try
            {
                if (string.IsNullOrWhiteSpace(assigningAuthorityId))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.MissingCommunityId);
                    return this.Result;
                }
                if (string.IsNullOrWhiteSpace(patientId))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.MissingPatientId);
                    return this.Result;
                }
                Patient patient = new Patient();
                this.Result = this.MobiusDAL.GetPatientDetails(out patient, MPIID: patientId);
                if (!this.Result.IsSuccess)
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.InvalidPatientId);
                    return this.Result;
                }
                else
                {
                    if (patient.CommunityId != assigningAuthorityId)
                    {
                        this.Result.IsSuccess = false;
                        //Either Patient Id or communityId is not valid.  
                        this.Result.SetError(ErrorCode.InvaidPatientIdOrCommunityId);
                        return this.Result;
                    }
                    else
                    {
                        this.Result = this.MobiusConnect.GetPHISource(assigningAuthorityId, patientId, out patientIdentifiers);
                        if (!this.Result.IsSuccess)
                        {
                            this.Result.IsSuccess = false;
                            // Some error occur while fetching Patient Correlation
                            this.Result.SetError(ErrorCode.PatientCorrelationError);
                            return this.Result;
                        }
                        //Load NHINCommunities
                        List<MobiusNHINCommunity> NHINCommunities = null;
                        if (this.GetNhinCommunity(out NHINCommunities).IsSuccess)
                        {
                            var Identifiers = patientIdentifiers.Join(NHINCommunities, t => t.CommunityIdentifier, c => c.CommunityIdentifier,
                                                                (t, c) => new { PatientIdentifier = t, NHINCommunity = c });

                            RemotePatientIdentifier patientIdentifier = null;
                            patientIdentifiers = new List<RemotePatientIdentifier>();
                            foreach (var item in Identifiers)
                            {
                                patientIdentifier = new RemotePatientIdentifier();
                                patientIdentifier.PatientId = item.PatientIdentifier.PatientId;
                                patientIdentifier.CommunityDescription = item.NHINCommunity.CommunityDescription;
                                patientIdentifier.CommunityIdentifier = item.NHINCommunity.CommunityIdentifier;
                                patientIdentifier.CommunityName = item.NHINCommunity.CommunityName;
                                patientIdentifier.IsHomeCommunity = item.NHINCommunity.IsHomeCommunity;
                                patientIdentifiers.Add(patientIdentifier);
                            }
                            //Check for HomeCommunity 
                            if (patientIdentifiers.Where(t => t.IsHomeCommunity).Count() == 0)
                            {
                                NHINCommunities = NHINCommunities.Where(t => t.IsHomeCommunity == true).ToList();
                                foreach (MobiusNHINCommunity item in NHINCommunities)
                                {
                                    patientIdentifier = new RemotePatientIdentifier();
                                    patientIdentifier.PatientId = patientId;
                                    patientIdentifier.CommunityDescription = item.CommunityDescription;
                                    patientIdentifier.CommunityIdentifier = item.CommunityIdentifier;
                                    patientIdentifier.CommunityName = item.CommunityName;
                                    patientIdentifier.IsHomeCommunity = item.IsHomeCommunity;
                                }
                                patientIdentifiers.Add(patientIdentifier);
                            }

                        }
                    }
                }

                this.Result.IsSuccess = true;

            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return this.Result;
        }

        #endregion GetPHISource

        #region GetProviderDetails
        /// <summary>
        ///  GetProviderDetails
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public Result GetProviderDetails(string emailAddress, out Provider provider)
        {
            provider = null;
            string message = string.Empty;
            try
            {
                if (string.IsNullOrWhiteSpace(emailAddress))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.UserId_Missing);
                    return this.Result;
                }
                if (!Validator.ValidateEmail(emailAddress, out message))
                {

                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Invalid_Email_Address);
                    return this.Result;
                }

                this.Result = this.MobiusDAL.GetProviderDetails(emailAddress, out provider);

                if (!this.Result.IsSuccess)
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Provider_Details_Not_Found);
                    return this.Result;
                }

            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);

            }
            return this.Result;
        }
        #endregion

        #region ActivateUser
        /// <summary>
        /// ActivateUser
        /// </summary>
        /// <param name="userType"></param>
        /// <param name="emailAddress"></param>
        /// <param name="CSR"></param>
        /// <param name="PKCS7Response"></param>
        /// <returns></returns>
        public Result ActivateUser(Mobius.CoreLibrary.UserType userType, string emailAddress, string CSR, out string PKCS7Response)
        {
            string message = string.Empty;
            PKCS7Response = string.Empty;
            string CertificateSerialNumber = string.Empty;
            string PublicKey = string.Empty;
            EventLogger eventLogger = null;
            EventActionData eventActionData = null;
            string emailRecipients = string.Empty;
            AdminDetails adminDetail = null;
            List<AdminDetails> adminDetails = null;
            try
            {
                if (string.IsNullOrWhiteSpace(emailAddress))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.UserId_Missing);
                    return this.Result;
                }
                if (!Validator.ValidateEmail(emailAddress, out message))
                {

                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Invalid_Email_Address);
                    return this.Result;
                }
                if (string.IsNullOrWhiteSpace(CSR))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.CertificateAuthority_CSR_Missing);
                    return this.Result;
                }

                CertificateEnrollment certificateEnrollment = new CertificateEnrollment();
                // ValidateEnrollmentRequest validate the CSR
                this.Result = certificateEnrollment.ValidateEnrollmentRequest(CSR);
                if (this.Result.IsSuccess)
                {  // EnrollCertificateRequest will accepts the PKCS#10 request and cerate the certificate for the requestor 
                    this.Result = certificateEnrollment.EnrollCertificateRequest(CSR, out PKCS7Response);
                    if (!string.IsNullOrEmpty(PKCS7Response))
                    {
                        this.Result = certificateEnrollment.GetCertificateInformation(PKCS7Response);
                        if (this.Result.IsSuccess)
                        {
                            CertificateSerialNumber = certificateEnrollment.SerialNumber;
                            PublicKey = certificateEnrollment.PublicKey;
                            CreatedOn = DateTime.Now.ToString();
                            ExpiryOn = certificateEnrollment.ExpirationDate.ToString();
                            this.Result = this.MobiusDAL.ActivateUser(emailAddress, CertificateSerialNumber, PublicKey, userType, CreatedOn, ExpiryOn);

                            if (this.Result.IsSuccess)
                            {
                                this.Result = this.GetAdminDetails(adminDetail, out adminDetails);
                                if (adminDetails != null)
                                {
                                    emailRecipients = adminDetails[0].Email;
                                }
                                if (this.Result.IsSuccess)
                                {
                                    Result.SetError(ErrorCode.Activation_In_Process);
                                }

                            }

                            if (this.Result.IsSuccess && !string.IsNullOrEmpty(emailRecipients))
                            {
                                eventLogger = new EventLogger();
                                eventActionData = new EventActionData();
                                eventActionData.UserName = "Admin";
                                eventActionData.EmailRecipients.Add(emailRecipients);
                                eventActionData.RecipientEmail = emailAddress;
                                eventActionData.Event = EventType.RegenratedUserAccount;
                                eventLogger.LogEvent(eventActionData);
                                eventLogger = null;
                                eventActionData = null;
                            }

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);

            }
            return this.Result;
        }
        #endregion

        #region UpgradeUser
        public Result UpgradeUser(Mobius.CoreLibrary.UserType userType, string EmailAddress, string Password, string PKCS7Request, out string SerialNumber, out string PKCS7Response)
        {
            SerialNumber = string.Empty;
            string UserName = string.Empty;
            PKCS7Response = string.Empty;
            try
            {
                string message = string.Empty;
                string PublicKey = string.Empty;
                string emailRecipients = string.Empty;
                if (string.IsNullOrWhiteSpace(EmailAddress))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.UserId_Missing);
                    return this.Result;
                }
                if (string.IsNullOrEmpty(Password))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Password_Missing);
                    return this.Result;
                }
                if (string.IsNullOrWhiteSpace(PKCS7Request))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.CertificateAuthority_CSR_Missing);
                    return this.Result;
                }
                this.Result = AuthenticateUser(EmailAddress, Password, (int)userType, out SerialNumber, out UserName);

                if (this.Result.IsSuccess)
                {
                    CertificateEnrollment certificateEnrollment = new CertificateEnrollment();
                    this.Result = certificateEnrollment.ValidateEnrollmentRequest(PKCS7Request);
                    if (this.Result.IsSuccess)
                    {
                        this.Result = certificateEnrollment.EnrollCertificateRequest(PKCS7Request, out PKCS7Response);
                        if (this.Result.IsSuccess)
                        {
                            this.Result = certificateEnrollment.GetCertificateInformation(PKCS7Response);
                            if (this.Result.IsSuccess)
                            {
                                SerialNumber = certificateEnrollment.SerialNumber;
                                PublicKey = certificateEnrollment.PublicKey;
                                CreatedOn = DateTime.Now.ToString();
                                ExpiryOn = certificateEnrollment.ExpirationDate.ToString();
                                this.Result = this.MobiusDAL.UpgradeUser(EmailAddress, SerialNumber, PublicKey, userType, CreatedOn, ExpiryOn);
                            }
                        }
                    }
                    this.AddLogEvent(this.Result.IsSuccess ? EventType.UpgradeUserAccount : EventType.UpgradeUserAccountFailed,
                                       0,
                                       userType == Mobius.CoreLibrary.UserType.Provider ? true : false,
                                       string.Empty,
                                       string.Empty,
                                       string.Empty,
                                       EmailAddress,
                                       EmailAddress,
                                       string.Empty,
                                       Helper.GetIPAddress(),
                                       userType == Mobius.CoreLibrary.UserType.Provider ? 2 : 1,
                                      MobiusAppSettingReader.LocalHomeCommunityID,
                                       string.Empty,
                                       null,
                                       this.Result.IsSuccess ? string.Empty : this.Result.ErrorMessage,
                                       string.Empty,
                                       purpose: UPGRADEACCOUNT,
                                       subject: userType.ToString(),
                                       UserName: EmailAddress);

                }
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);

                this.AddLogEvent(this.Result.IsSuccess ? EventType.UpgradeUserAccount : EventType.UpgradeUserAccountFailed,
                                       0,
                                       userType == Mobius.CoreLibrary.UserType.Provider ? true : false,
                                       string.Empty,
                                       string.Empty,
                                       string.Empty,
                                       EmailAddress,
                                       EmailAddress,
                                       string.Empty,
                                       Helper.GetIPAddress(),
                                       userType == Mobius.CoreLibrary.UserType.Provider ? 2 : 1,
                                      MobiusAppSettingReader.LocalHomeCommunityID,
                                       string.Empty,
                                       null,
                                       this.Result.IsSuccess ? string.Empty : this.Result.ErrorMessage,
                                       string.Empty,
                                       purpose: UPGRADEACCOUNT,
                                       subject: userType.ToString(),
                                       UserName: EmailAddress);
            }

            return this.Result;
        }

        #endregion

        #region Admin Methods

        #region GetUserDetials
        /// <summary>
        ///  Get UserDetials into data base Patient and provider table
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public Result GetUserDetials(ref List<UserDetails> userDetails)
        {
            try
            {
                if (userDetails != null && userDetails.Count > 0)
                {
                    if (string.IsNullOrEmpty(userDetails[0].EmailAddress))
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.EmailAddress_Missing);
                        return this.Result;
                    }
                    this.Result = this.MobiusDAL.GetUserDetials(ref userDetails);
                }
                else
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.MissingParameters);
                }
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return this.Result;
        }
        #endregion

        #region GetAdminDetails
        /// <summary>
        /// GetAdminDetails
        /// </summary>
        /// <param name="adminDetail"></param>
        /// <param name="adminDetails"></param>
        /// <returns></returns>
        public Result GetAdminDetails(AdminDetails adminDetail, out List<AdminDetails> adminDetails)
        {
            adminDetails = null;
            try
            {
                this.Result = this.MobiusDAL.GetAdminDetails(adminDetail, out adminDetails);
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return this.Result;
        }
        #endregion

        #region UpdateAdminDetails
        /// <summary>
        /// Update Admin Details
        /// </summary>
        /// <param name="adminDetails"></param>
        /// <returns></returns>
        public Result UpdateAdminDetails(AdminDetails adminDetails)
        {
            try
            {
                this.Result = this.MobiusDAL.UpdateAdminDetails(adminDetails);
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return this.Result;
        }
        #endregion

        #region AddAdminDetails
        /// <summary>
        /// Add Admin details
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Result AddAdminDetails(string email, string password = "")
        {
            try
            {
                this.Result = this.MobiusDAL.AddAdminDetails(email, password);
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return this.Result;
        }
        #endregion

        #endregion

        #region NIST Document Validation
        /// <summary>
        /// This method returns the list of available type of document validations
        /// </summary>
        /// <param name="availableValidations"></param>
        /// <returns></returns>
        public Result GetAvailableValidations(out List<MobiusAvailableValidations> availableValidations)
        {
            availableValidations = null;
            try
            {
                this.Result.IsSuccess = false;
                this.Result = this.NISTValidation.getAvailableValidations(out availableValidations);

            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return this.Result;
        }

        /// <summary>
        /// This method returns the list of issues found for given document and specification ID combination
        /// </summary>
        /// <param name="specificationId"></param>
        /// <param name="document"></param>
        /// <param name="NISTValidationType"></param>
        /// <param name="validationResults"></param>
        /// <returns></returns>
        public Result ValidateDocument(string specificationId, string document, NISTValidationType NISTValidationType, out MobiusValidationResults validationResults)
        {
            validationResults = null;
            try
            {
                this.Result.IsSuccess = false;

                //Validate if given specificationID is correct.
                if (ValidateSpecificationID(specificationId).IsSuccess)
                {

                    //Call the validation service.
                    this.Result = this.NISTValidation.validateDocument(specificationId, document, NISTValidationType, out validationResults);

                    if (this.Result.IsSuccess)
                    {
                        //Filter out the result,if user has asked for specific filtration.
                        switch (NISTValidationType)
                        {
                            case NISTValidationType.ERRORS:
                                validationResults.issue = validationResults.issue.Where(t => t.severity.ToUpper() == NISTValidationType.ToString() || t.severity.ToUpper() == "ERROR").ToList();
                                break;
                            case NISTValidationType.WARNING:
                                validationResults.issue = validationResults.issue.Where(t => t.severity.ToUpper() == NISTValidationType.ToString() || t.severity.ToUpper() == "WARNINGS").ToList();
                                break;
                            case NISTValidationType.ALL:
                                break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return this.Result;

        }
        #endregion NIST Document Validation

        #region Private Method

        private Result ValidateSpecificationID(string specificationID)
        {
            List<MobiusAvailableValidations> availableValidations = null;
            try
            {
                this.Result.IsSuccess = false;
                this.Result = this.NISTValidation.getAvailableValidations(out availableValidations);
                if (this.Result.IsSuccess)
                {
                    MobiusAvailableValidations MatchingSpecificationID = availableValidations.Find(x => x.specificationId == specificationID);
                    if (MatchingSpecificationID == null)
                    {
                        this.Result.SetError(ErrorCode.Invalid_Validation_SpecificationId);
                        this.Result.IsSuccess = false;
                    }
                    else
                    {
                        this.Result.IsSuccess = true;
                    }
                }

            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return this.Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patients"></param>
        /// <returns></returns>
        private Result SpecialHandlingResponseValidation(List<Patient> patients, Demographics requestObject)
        {

            var patientCount = patients.Count;
            //Case ->PatientAdressRequested
            //Create single address collection object for patients
            // Case->Validate ZipCode in the response received                 
            //  if request have zipcode then ignore the case else
            //   Group the records on the zipcode 
            //    if record_count is equal to 1 or less then the patient count then send error message

            if (requestObject.Addresses != null && requestObject.Addresses.Count == 0)
            {
                List<Address> patientAddress = new List<Address>();
                foreach (var item in patients)
                {
                    patientAddress.AddRange(item.PatientAddress);
                }
                var patientAddressCount = patientAddress.GroupBy(t => t.AddressLine1 + " " + t.AddressLine2 + " " + t.City.CityName + " " + t.City.State.StateName + " " + t.Zip).Count();

                if (patientAddressCount < patientCount)
                {
                    this.Result.SetError(ErrorCode.PatientAdressRequested);
                    this.Result.IsSuccess = false;
                    return this.Result;
                }
            }

            //Case ->PatientTelecomRequested
            //Create single telephone collection object for patients
            // Case->Validate telephone in the response received                 
            //  if request have telephone then ignore the case else
            //      Group the records on the telephone 
            //    if record_count is equal to 1 or less then the patient count then send error message
            if (requestObject.Telephones != null && requestObject.Telephones.Count == 0)
            {
                List<Telephone> patientTelephone = new List<Telephone>();
                foreach (var item in patients)
                {
                    patientTelephone.AddRange(item.Telephone);
                }
                var patientTelephoneCount = patientTelephone.GroupBy(t => t.Number).Count();
                if (patientTelephoneCount < patientCount)
                {
                    this.Result.SetError(ErrorCode.PatientTelecomRequested);
                    this.Result.IsSuccess = false;
                    return this.Result;
                }
            }

            //Case -> Validate Mothers Maiden Name in the response received
            // if request have MothersMaidenName then ignore the case
            //  Count number of records where mothers Maiden Name is null or empty string 
            //  if count is equal to patient records then ignore the case.
            //      else if count is less then check the request object has MothersMaidenName or not - 
            //         if request does not have MothersMaidenName then group the record based on MothersMaidenName                     
            //              if record_count is equal to 1 or less then patient count then send error message
            //if (string.IsNullOrWhiteSpace(requestObject.MothersMaidenName))
            //{
            //    if (patients.Count(t => t.MothersMaidenName == string.Empty) != patientCount)
            //    {
            //        var MothersMaidenNameGroupCount = patients.GroupBy(t => t.MothersMaidenName).Count();
            //        if (MothersMaidenNameGroupCount < patientCount)
            //        {
            //            this.Result.SetError(ErrorCode.MothersMaidenNameRequested);
            //            this.Result.IsSuccess = false;
            //            return this.Result;
            //        }
            //    }
            //}
            this.Result.SetError(ErrorCode.SearchPatient_Multiple_Record_Found);
            this.Result.IsSuccess = false;
            return this.Result;
        }

        /// <summary>
        /// date Formatter
        /// </summary>
        /// <param name="date">datetime</param>
        /// <returns>return date in yyymmdd format</returns>
        private string dateFormatter(string date)
        {
            string correctDate = date;
            if (date.Length == 10)
            {
                correctDate = string.Empty;
                string year = date.Substring(6, 4);
                string month = date.Substring(0, 2);
                string dd = date.Substring(3, 2);
                correctDate = year + month + dd;
            }
            if (correctDate.Length == 8)
            {
                string year = correctDate.Substring(0, 4);
                string month = correctDate.Substring(4, 2);
                string dd = correctDate.Substring(6, 2);
                correctDate = month + "/" + dd + "/" + year;
            }
            return correctDate;

        }

        /// <summary>
        /// Adding log events
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="refernceID"></param>
        /// <param name="providerType"></param>
        /// <param name="token"></param>
        /// <param name="networkAccessPointID"></param>
        /// <param name="networkAccessPointTypeCode"></param>
        /// <param name="homeCommunityId"></param>
        /// <param name="messageType"></param>
        /// <param name="requestObject"></param>
        /// <param name="ErrorMessage"></param>
        /// <param name="providerFirstName"></param>
        /// <param name="providerOrgName"></param>
        /// <param name="providerMail"></param>
        /// <param name="medicalRecordsDeliveryEmailAddress"></param>
        private void AddLogEvent(EventType eventType,
                                    int referenceID,
                                    bool providerType,
                                    string patientID,
                                    string providerFirstName,
                                    string providerOrgName,
                                    string providerMail,
                                    string medicalRecordsDeliveryEmailAddress,
                                    string token,
                                    string networkAccessPointID,
                                    int networkAccessPointTypeCode,
                                    string homeCommunityId,
                                    string messageType,
                                    object requestObject,
                                    string ErrorMessage,
                                    string documentId = "",
                                    string purpose = "",
                                    string subject = "",
                                    string UserName = ""
                                    )
        {
            EventActionData eventActionData = new EventActionData();
            eventActionData.Event = eventType;
            eventActionData.Referenceid = referenceID;
            if (providerType) //IndividualProvider
            {
                eventActionData.ProviderName = providerFirstName;
                eventActionData.EmailRecipients.Add(providerMail);
            }
            else            //Org provider
            {
                eventActionData.ProviderName = providerOrgName;
                eventActionData.EmailRecipients.Add(medicalRecordsDeliveryEmailAddress);
            }
            eventActionData.PatientId = patientID;
            eventActionData.Token = token;
            eventActionData.ErrorMessage = ErrorMessage;
            eventActionData.NetworkAccessPointID = networkAccessPointID;
            eventActionData.NetworkAccessPointTypeCode = networkAccessPointTypeCode;
            eventActionData.CommunityId = homeCommunityId;
            eventActionData.MessageType = messageType;
            if (requestObject != null)
                eventActionData.RequestObject = Helper.Serialize(requestObject);
            else
                eventActionData.RequestObject = Helper.Serialize(this.Result);

            eventActionData.DocumentId = documentId;
            eventActionData.Purpose = purpose;
            eventActionData.Subject = subject;
            //if (!string.IsNullOrEmpty(Role))
            //    eventActionData.UserRole = (UserRole)Enum.Parse(typeof(UserRole), Role);
            //if (!string.IsNullOrEmpty(PurposeOfUse))
            //    eventActionData.Purpose = PurposeOfUse;
            eventActionData.UserName = UserName;
            EventLogger eventLogger = new EventLogger();
            eventLogger.LogEvent(eventActionData);
            //added for issue id #138
            eventActionData = null;
            eventLogger = null;
        }

        /// <summary>
        /// Get SerialNumber using  EmailAddress
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns>return string</returns>
        private string GetSerialNumber(string emailAddress)
        {
            string certificateSerialNumber = string.Empty;
            string message = string.Empty;
            int userType = 2;
            MobiusDAL authenticateUser = null;
            try
            {
                if (!string.IsNullOrEmpty(emailAddress))
                {
                    authenticateUser = new MobiusDAL();
                    authenticateUser.GetSerialNumber(emailAddress, userType, out certificateSerialNumber);
                }

            }
            catch (Exception)
            {

            }
            finally
            {
                authenticateUser = null;
            }
            return certificateSerialNumber;
        }

        /// <summary>
        /// Update Document Metadata
        /// </summary>
        /// <param name="DocumentID">string</param>
        /// <param name="Location">string</param>
        /// <returns>return true/false</returns>
        private bool UpdateDocumentMetadata(string documentID, string location)
        {
            bool bUpdated = false;
            bUpdated = this.MobiusDAL.UpdateDocumentMetadata(documentID, location);
            return bUpdated;
        }


        /// <summary>
        /// GenerateId
        /// </summary>
        /// <returns></returns>
        private static string GenerateId()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }


        /// <summary>
        /// ParseRespondingGateway_PRPA_IN201305UV02RequestType
        /// </summary>
        /// <param name="reqType">reqType class object</param>
        /// <returns>return Result class object</returns>
        private Demographics ParseRespondingGateway_PRPA_IN201305UV02RequestType(RespondingGateway_PRPA_IN201305UV02RequestType reqType)
        {
            Demographics demographics = new Demographics();
            PRPA_IN201305UV02 prpa = reqType.PRPA_IN201305UV02;
            PRPA_MT201306UV02ParameterList parameterList;

            parameterList = prpa.controlActProcess.queryByParameter.parameterList;
            //Get Gender code from request
            string gender = string.Empty;
            if (parameterList.livingSubjectAdministrativeGender.Length > 0)
            {

                if (parameterList.livingSubjectAdministrativeGender[0].value != null && parameterList.livingSubjectAdministrativeGender.Length > 0)
                {
                    gender = parameterList.livingSubjectAdministrativeGender[0].value[0].code;
                }
            }

            if (!string.IsNullOrEmpty(gender))
            {
                if (gender.Equals("M", StringComparison.OrdinalIgnoreCase) || gender.Equals("Male", StringComparison.OrdinalIgnoreCase))
                    demographics.Gender = Gender.Male;
                else if (gender.Equals("F", StringComparison.OrdinalIgnoreCase) || gender.Equals("Female", StringComparison.OrdinalIgnoreCase))
                    demographics.Gender = Gender.Female;
                else if (gender.Equals("UN", StringComparison.OrdinalIgnoreCase) || gender.Equals("U", StringComparison.OrdinalIgnoreCase) || gender.Equals("Unspecified", StringComparison.OrdinalIgnoreCase))
                    demographics.Gender = Gender.Unspecified;
            }

            //Get patient DOB
            if (parameterList.livingSubjectBirthTime != null && parameterList.livingSubjectBirthTime.Length > 0)
            {
                if (parameterList.livingSubjectBirthTime[0].value != null && parameterList.livingSubjectBirthTime[0].value.Length > 0)
                {
                    demographics.DOB = this.dateFormatter(parameterList.livingSubjectBirthTime[0].value[0].value);
                }
            }

            //Get patient Deceased Time
            if (parameterList.livingSubjectDeceasedTime != null && parameterList.livingSubjectDeceasedTime.Length > 0)
            {
                if (parameterList.livingSubjectDeceasedTime[0].value != null && parameterList.livingSubjectDeceasedTime[0].value.Length > 0)
                {
                    demographics.DeceasedTime = this.dateFormatter(parameterList.livingSubjectDeceasedTime[0].value[0].value);
                }
            }

            //Get Patient Name
            if (parameterList.livingSubjectName != null && parameterList.livingSubjectName.Length > 0)
            {
                if (parameterList.livingSubjectName[0].value != null && parameterList.livingSubjectName[0].value.Length > 0)
                {
                    if (parameterList.livingSubjectName[0].value[0].Items != null && parameterList.livingSubjectName[0].value[0].Items.Length > 0)
                    {
                        bool IsMiddleName = false;
                        foreach (var name in parameterList.livingSubjectName[0].value[0].Items)
                        {
                            //Family Name
                            if (name.Text != null && name.Text.Length > 0)
                            {
                                if (name is en_explicitfamily)
                                {
                                    demographics.FamilyName = string.Join("", name.Text);
                                    continue;
                                }
                                //Middle Name
                                else if (name is en_explicitgiven && IsMiddleName)
                                {
                                    demographics.MiddleName = string.Join("", name.Text);
                                    continue;
                                }
                                //Given Name
                                else if (name is en_explicitgiven)
                                {
                                    demographics.GivenName = string.Join("", name.Text);
                                    IsMiddleName = true;
                                    continue;
                                }

                                  //Given Name
                                else if (name is en_explicitsuffix)
                                {
                                    demographics.Suffix = string.Join("", name.Text);
                                    continue;
                                }
                                //Given Name
                                else if (name is en_explicitprefix)
                                {
                                    demographics.Prefix = string.Join("", name.Text);
                                    continue;
                                }
                            }

                        }
                    }

                }
            }


            if (parameterList.mothersMaidenName != null && parameterList.mothersMaidenName.Length > 0)
            {
                if (parameterList.mothersMaidenName[0].value != null && parameterList.mothersMaidenName[0].value.Length > 0)
                {
                    if (parameterList.mothersMaidenName[0].value[0].Items != null)
                    {
                        bool IsMiddleName = false;
                        foreach (var name in parameterList.mothersMaidenName[0].value[0].Items)
                        {
                            //Family Name
                            if (name is en_explicitfamily)
                            {
                                demographics.MothersMaidenName.FamilyName = string.Join("", name.Text);
                                continue;
                            }
                            //Middle Name
                            else if (name is en_explicitgiven && IsMiddleName)
                            {
                                demographics.MothersMaidenName.MiddleName = string.Join("", name.Text);
                                continue;
                            }
                            //Given Name
                            else if (name is en_explicitgiven)
                            {
                                demographics.MothersMaidenName.GivenName = string.Join("", name.Text);
                                IsMiddleName = true;
                                continue;
                            }
                            //Given Name
                            else if (name is en_explicitsuffix)
                            {
                                demographics.MothersMaidenName.Suffix = string.Join("", name.Text);
                                continue;
                            }
                            else if (name is en_explicitprefix)
                            {
                                demographics.MothersMaidenName.Prefix = string.Join("", name.Text);
                                continue;
                            }

                        }

                    }
                }

            }
            //Patient telephone number
            if (parameterList.patientTelecom != null && parameterList.patientTelecom.Length > 0)
            {
                Telephone Telephone = null;
                foreach (var item in parameterList.patientTelecom[0].value)
                {
                    Telephone = new Entity.Telephone();
                    Telephone.Number = item.value.Replace("tel:", "");
                    demographics.Telephones.Add(Telephone);
                }
            }

            //Patient address
            if (parameterList.patientAddress != null && parameterList.patientAddress.Length > 0)
            {
                Address address = null;
                foreach (var patientAddress in parameterList.patientAddress)
                {

                    if (parameterList.patientAddress[0].value != null && parameterList.patientAddress[0].value.Length > 0
                        && parameterList.patientAddress[0].value[0].Items != null)
                    {
                        address = new Entity.Address();
                        foreach (var item in patientAddress.value[0].Items)
                        {
                            if (item.Text != null && item.Text.Length > 0)
                            {
                                if (item is PatientDiscovery.adxp_explicitstreetAddressLine)
                                {
                                    address.AddressLine1 = string.Join("", item.Text);
                                }
                                else if (item is PatientDiscovery.adxp_explicitcity)
                                {
                                    address.City.CityName = string.Join("", item.Text);
                                }
                                else if (item is PatientDiscovery.adxp_explicitstate)
                                {
                                    address.City.State.StateName = string.Join("", item.Text);
                                }
                                else if (item is PatientDiscovery.adxp_explicitpostalCode)
                                {
                                    address.Zip = string.Join("", item.Text);
                                }
                                else if (item is PatientDiscovery.adxp_explicitcountry)
                                {
                                    address.City.State.Country.CountryName = string.Join("", item.Text);
                                }
                            }
                        }
                    }

                    demographics.Addresses.Add(address);
                }

            }

            if (parameterList.livingSubjectBirthPlaceAddress != null && parameterList.livingSubjectBirthPlaceAddress.Length > 0 && parameterList.livingSubjectBirthPlaceAddress[0].value != null)
            {

                // parameterList.patientAddress[0].value
                Address address = null;
                foreach (var item in parameterList.livingSubjectBirthPlaceAddress[0].value[0].Items)
                {
                    address = new Entity.Address();
                    if (item is PatientDiscovery.adxp_explicitstreetAddressLine)
                    {
                        address.AddressLine1 = string.Join("", item.Text);
                    }
                    else if (item is PatientDiscovery.adxp_explicitcity)
                    {
                        address.City.CityName = string.Join("", item.Text);
                    }
                    else if (item is PatientDiscovery.adxp_explicitstate)
                    {
                        address.City.State.StateName = string.Join("", item.Text);
                    }
                    else if (item is PatientDiscovery.adxp_explicitcountry)
                    {
                        address.City.State.Country.CountryName = string.Join("", item.Text);
                    }

                    else if (item is PatientDiscovery.adxp_explicitpostalCode)
                    {
                        address.Zip = string.Join("", item.Text);
                    }
                    demographics.BirthPlaceAddress.Add(address);
                }

            }

            //SSN

            if (parameterList.livingSubjectId != null && parameterList.livingSubjectId.Length > 0)
            {
                foreach (var item in parameterList.livingSubjectId)
                {
                    if (item.value != null && item.value.Length > 0 && item.value[0].root.Equals("2.16.840.1.113883.4.1"))
                    {
                        demographics.SSN = item.value[0].extension;
                    }
                }
            }

            List<MobiusNHINCommunity> NHINCommunities = new List<MobiusNHINCommunity>();
            MobiusNHINCommunity nHINCommunity = new MobiusNHINCommunity();
            foreach (NhinTargetCommunityType NHINCommunity in reqType.NhinTargetCommunities)
            {
                // create object of NHINCommunity
                nHINCommunity = new MobiusNHINCommunity();
                nHINCommunity.CommunityIdentifier = NHINCommunity.homeCommunity.homeCommunityId;
                NHINCommunities.Add(nHINCommunity);

            }

            return demographics;
        }


        #region Manage Emergency Overridden
        /// <summary>
        /// To get All the instance of Emergency Audit
        /// </summary>
        /// <returns></returns>
        public Result GetAllEmergencyAudit(EmergencyRecords emergencyRecords, out List<EmergencyAudit> lstEmergencyAudit, string patientId = "")
        {
            //lstEmergencyAudit = new List<EmergencyAudit>();
            lstEmergencyAudit = null;
            try
            {
                this.Result = this.MobiusDAL.GetAllEmergencyAudit(emergencyRecords, out lstEmergencyAudit, patientId);
                //if no record found
                if (this.Result.IsSuccess && (lstEmergencyAudit == null || lstEmergencyAudit.Count == 0))
                    this.Result.SetError(ErrorCode.RecordNotFound);

            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return this.Result;
        }


        /// <summary>
        /// Get the emergency Details by id
        /// </summary>
        /// <param name="AuditID"></param>
        /// <returns></returns>
        public Result GetEmergencyDetailById(int AuditID, out EmergencyAudit emergencyAudit)
        {
            //EmergencyAudit emergencyAudit = new EmergencyAudit();
            emergencyAudit = null;
            try
            {
                this.Result = this.MobiusDAL.GetEmergencyDetailById(AuditID, out emergencyAudit);
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
                Helper.LogError(ex);
            }
            return this.Result;
        }

        /// <summary>
        /// UpdateOverrideDetails
        /// </summary>
        /// <param name="lstAuditID"></param>
        /// <param name="IsAuditStatus"></param>
        /// <returns></returns>
        public Result UpdateOverrideDetails(List<int> lstAuditID, bool IsAuditStatus)
        {
            EmergencyAudit EmergencyAudit = new EmergencyAudit();
            EmergencyAudit = null;
            try
            {
                if (lstAuditID.Count > 0)
                {
                    this.Result = this.MobiusDAL.UpdateOverrideDetails(lstAuditID, IsAuditStatus);
                    if (this.Result.IsSuccess)
                    {
                        this.Result.SetError(ErrorCode.Record_Successfully_Updated);
                    }
                    else
                    {
                        this.Result.SetError(ErrorCode.UnknownException);
                    }
                }
                else
                {

                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Select_Record_To_Close);
                    return this.Result;
                }

            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException);
                Helper.LogError(ex);
            }
            return Result;
        }




        #endregion


        #region GetTrimmedDocument
        /// <summary>
        /// To get the document after applying patient consent
        /// </summary>
        /// <param name="documentRequest"></param>
        /// <param name="docByte"></param>
        /// <param name="patientConsentId"></param>
        /// <returns></returns>
        private Result GetTrimmedDocument(DocumentRequest documentRequest, out byte[] docByte, int patientConsentId)
        {
            docByte = null;
            #region Section level consent permission mapping

            if (FileHandler.CheckDocumentExists(documentRequest.documentId, documentRequest.FilePathLocation))
            {
                docByte = FileHandler.LoadDocument(documentRequest.documentId, documentRequest.FilePathLocation);

                //Scenario 1 - To handle documents which has associated XACML document - Return Entire document byte, without any trimming.
                if (patientConsentId == -214783647)
                {
                    return this.Result;
                }

                //Scenario 2 - To handle documents which are driven by database based patient consent. Return trimmed document bytes as per mapped consent.                
                
                MobiusPatientConsent patientConsent = null;
                List<C32Section> C32Sections = null;
                //get the Patient consent from DB 
                this.Result = this.GetPatientConsentByConsentId(documentRequest.patientId, patientConsentId, out patientConsent, out C32Sections);
                if (this.Result.IsSuccess)
                {
                    //Create object for helper
                    CDAHelper CDAHelper = new CDAHelper(docByte);
                    docByte = null;
                    //Get tri
                    this.Result = CDAHelper.GetTruncatedDocument(C32Sections, out docByte);
                }
            }
            else
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.FileNotFound);
                return this.Result;
            }
            #endregion

            return this.Result;

        }
        #endregion

        /// <summary>
        /// HasAcessPermission
        /// </summary>
        /// <param name="documentID"></param>
        /// <param name="filePathLocation"></param>
        /// <param name="subjectRole"></param>
        /// <param name="subjectEmailID"></param>
        /// <param name="purpose"></param>
        /// <param name="patientId"></param>
        /// <returns></returns>
        private Result HasAcessPermission(DocumentRequest documentRequest, out int patientConsentId)
        {
            patientConsentId = 0;
            try
            {
                byte[] XACMLbyteData = null;
                this.Result.IsSuccess = true;
                if (FileSystem.FileHandler.CheckDocumentExists(documentRequest.documentId + "XACML", documentRequest.FilePathLocation))
                //in case of Emergency override, direcly go to DAL and create an entry in emergency audit table
                {
                    XACMLbyteData = FileHandler.LoadDocument(documentRequest.documentId + "XACML", documentRequest.FilePathLocation);

                    XACML = XACMLHandler.GetXACMLDocumentDetail(XACMLbyteData);
                    if (XACML.PurposeofUse != documentRequest.purpose
                       || !XACML.Subject.Contains(documentRequest.subjectRole, StringComparer.OrdinalIgnoreCase) && !XACML.Subject.Contains(documentRequest.subjectEmailID, StringComparer.OrdinalIgnoreCase)
                        //|| (XACML.Subject.ToUpper() != documentRequest.subjectRole.ToUpper()   && XACML.Subject.ToUpper() != documentRequest.subjectEmailID.ToUpper()) 
                        || Convert.ToDateTime(XACML.RuleStartDate) > DateTime.Now
                        || Convert.ToDateTime(XACML.RuleEndDate) < DateTime.Now)
                    {
                        this.Result.SetError(ErrorCode.Patient_Consent_Deviated);
                        if (UpdateDocumentMetadata(documentRequest.documentId, Path.GetFileNameWithoutExtension(documentRequest.FilePathLocation)))
                        {
                            this.Result.IsSuccess = false;
                            this.Result.SetError(ErrorCode.Patient_Consent_Deviated);
                            return this.Result;
                        }
                    }
                    else
                        patientConsentId = -214783647;// In case When provider/role has permission in XACML file
                }
                else
                {

                    this.Result = this.MobiusDAL.HasAccessPermission(documentRequest, out patientConsentId);
                    if (!this.Result.IsSuccess)
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Patient_Consent_Deviated);
                    }
                }
            }

            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
                Helper.LogError(ex);
            }
            return this.Result;
        }


        /// <summary>
        /// this method will validate requested NHIN community 
        /// </summary>
        /// <param name="NHINCommunities">requested NHIN communities</param>
        /// <returns>Result object</returns>
        private Result ValidateNhinCommunities(List<MobiusNHINCommunity> NHINCommunities, List<MobiusNHINCommunity> validNHINCommunities)
        {
            if (this.Result.IsSuccess)
            {
                IEnumerable<MobiusNHINCommunity> verifiedNHINCommunity = from param in NHINCommunities
                                                                         join masterdata in validNHINCommunities on param.CommunityIdentifier equals masterdata.CommunityIdentifier
                                                                         select masterdata;

                if (NHINCommunities.Count != verifiedNHINCommunity.Count())
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Invalid_Communities);

                }
                else
                {
                    this.Result.IsSuccess = true;
                    this.Communities = verifiedNHINCommunity.Distinct().ToList();
                }

                // get home community 
                this.HomeCommunities = validNHINCommunities.Where(t => t.IsHomeCommunity == true).ToList();

            }
            return this.Result;
        }

        /// <summary>
        /// Check Patient
        /// </summary>
        /// <param name="demographics">demographics class object</param>
        /// <returns>return Result class object</returns>
        private Result CheckPatient(Patient patient)
        {

            Result ValidationResult = new Result();
            List<Patient> patients = null;
            Demographics demographics = new Demographics();


            //Adding validation for Email address, to avoid duplicate patient registration based on email address.
            ValidationResult = this.MobiusDAL.CheckPatient(patient);
            if (ValidationResult.IsSuccess)
            {
                ValidationResult.SetError(ErrorCode.patient_Allready_Exist);
                return ValidationResult;
            }

            //Adding validation based on 'SearchPerson' procedure call, to avoid duplicate patient registration based on demographics.                       
            demographics.DOB = patient.DOB;
            demographics.LocalMPIID = patient.LocalMPIID;
            demographics.Gender = patient.Gender;

            if (patient.PatientAddress.Count > 0)
            {
                demographics.Addresses = patient.PatientAddress;
            }
            if (patient.FamilyName.Count > 0)
            {
                demographics.FamilyName = patient.FamilyName[0];
            }
            if (patient.GivenName.Count > 0)
            {
                demographics.GivenName = patient.GivenName[0];
            }

            if (patient.MiddleName.Count > 0)
            {
                demographics.MiddleName = patient.MiddleName[0];
            }
            if (patient.Telephone.Count > 0)
            {
                demographics.Telephones.Add(patient.Telephone[0]);
            }

            demographics.MothersMaidenName.FamilyName = patient.MothersMaidenName.FamilyName;
            demographics.MothersMaidenName.MiddleName = patient.MothersMaidenName.MiddleName;
            demographics.MothersMaidenName.GivenName = patient.MothersMaidenName.GivenName;
            demographics.MothersMaidenName.Prefix = patient.MothersMaidenName.Prefix;
            demographics.MothersMaidenName.Suffix = patient.MothersMaidenName.Suffix;

            demographics.SSN = patient.SSN;

            ValidationResult = this.MobiusDAL.SearchPatient(demographics, out patients);
            return ValidationResult;
        }

        private string AppendBodyContent(string content)
        {
            if (!string.IsNullOrWhiteSpace(content) && !content.Contains(SUMMARY_SUFFIX))
            {
                content = SUMMARY_PREFIX + content + SUMMARY_SUFFIX;
            }
            return content;
        }

        private Result ValidateC32Document(byte[] documentBytes)
        {
            //Validate document bytes for correctness of C32 document bytes
            if (MobiusAppSettingReader.ValidateC32Document)
            {
                MobiusValidationResults validationResults = null;
                UTF8Encoding Encoder = new UTF8Encoding();
                string fileContent = Encoder.GetString(documentBytes);

                this.Result = this.ValidateDocument(C32ValidationSpecificationID, fileContent, NISTValidationType.ERRORS, out validationResults);

                //Check if validation was performed at all
                if (this.Result.IsSuccess && validationResults.issue.Count() > 0)
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.C32Document_Validation_Failed);
                }
            }
            else
            {
                this.Result.IsSuccess = true;
            }
            return this.Result;
        }


        private Result ProcessDocumentRetrieveForPatient(DocumentRequest documentRequest, MobiusDocument document)
        {
            return ProcessDocumentRetrieve(documentRequest, document);
        }

        private Result ProcessDocumentRetrieve(DocumentRequest documentRequest, MobiusDocument document)
        {
            MemoryStream mS = null;
            string foldername = string.Empty;
            string saveLocation = string.Empty;
            string XACMLDocumentId = string.Empty;
            byte[] docByte = null;
            string filePathLocation = string.Empty;
            string RemoteCommunityID = string.Empty;

            //Set file server path 
            filePathLocation = Path.Combine(MobiusAppSettingReader.DocumentPath, document.Location);
            RemoteCommunityID = document.SourceCommunityId;
            XACMLDocumentId = document.XACMLDocumentId;

            //If document is already reposed in file repository.
            if (documentRequest.LocalData && document.Reposed)
            {
                //Check for existence of document.
                if (FileHandler.CheckDocumentExists(documentRequest.documentId, filePathLocation))
                {
                    docByte = FileHandler.LoadDocument(documentRequest.documentId, filePathLocation);
                    mS = new MemoryStream(docByte);
                    //Validate the fetched document bytes for validiy and return the result.
                    this.Result = this.ValidateC32Document(docByte);
                    if (this.Result.IsSuccess)
                        document.DocumentBytes = docByte;
                }
                else //File was not found in the repository.
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.FileNotFound);
                    return this.Result;
                }
            }
            //Document is not reposed in the file repository.
            else
            {
                if (!documentRequest.LocalData && document.Reposed)
                    foldername = document.Location;
                else
                    foldername = Guid.NewGuid().ToString();

                MobiusDocument document2 = null;

                // Get the document from Connect gateway
                this.Result = this.MobiusConnect.RetrieveDocument(documentRequest.documentId, RemoteCommunityID, "1", this.Assertion.DocumentRetrieveAssertion, out document2);
                if (this.Result.IsSuccess && ValidateC32Document(document.DocumentBytes).IsSuccess)
                {
                    // on success Save Document                     
                    saveLocation = Path.Combine(MobiusAppSettingReader.DocumentPath, foldername);
                    // Need to ensure to check for Consent from DB.
                    // Save Document into db 
                    if (FileHandler.CreateFolder(saveLocation) && FileHandler.SaveDocument(document2.DocumentBytes, Path.Combine(saveLocation, documentRequest.documentId + ".xml")))
                    {
                        document.DocumentBytes = document2.DocumentBytes;
                        MobiusDocument XACMLDocument = null;
                        // get the XACML document from DOD
                        if (!string.IsNullOrWhiteSpace(XACMLDocumentId))
                        {
                            this.Result = this.MobiusConnect.RetrieveDocument(XACMLDocumentId, RemoteCommunityID, "1", this.Assertion.DocumentRetrieveAssertion, out XACMLDocument);
                            if (this.Result.IsSuccess)
                            {
                                // on success save XACML Document 
                                FileHandler.SaveDocument(XACMLDocument.DocumentBytes, Path.Combine(saveLocation, XACMLDocumentId + ".xml"));
                            }
                        }
                        if (UpdateDocumentMetadata(documentRequest.documentId, foldername))
                        {
                            this.Result.IsSuccess = true;
                        }
                    }

                }
            }
            return this.Result;
        }


        private Result ProcessDocumentRetrieveForProvider(DocumentRequest documentRequest, MobiusDocument document)
        {
            //check whether purpose is Emergency and role is not authorize for emergency access
            if (documentRequest.purpose == Mobius.CoreLibrary.PurposeOfUse.EMERGENCY.ToString())
            {
                if (!MobiusAppSettingReader.LstEmergencyRole.Any(t => t.Value == documentRequest.subjectRole))
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Provider_Doesnot_Have_Permission_ToView);
                    return this.Result;
                }
            }

            string foldername = string.Empty;
            string saveLocation = string.Empty;
            string XACMLDocumentId = string.Empty;
            int patientConsentId = 0;
            MobiusDocument XACMLDocument = null;
            MobiusDocument documentFromConnect = null;
            byte[] docByte = null;
            string RemoteCommunityID = string.Empty;

            documentRequest.FilePathLocation = Path.Combine(MobiusAppSettingReader.DocumentPath, document.Location);
            RemoteCommunityID = document.SourceCommunityId;
            XACMLDocumentId = document.XACMLDocumentId;

            //if Status of EMergency Override is set to true
            if (documentRequest.EmergencyOverrideStatus)
            {
                // If purpose is Emergency
                if (documentRequest.purpose == Mobius.CoreLibrary.PurposeOfUse.EMERGENCY.ToString())
                {
                    //Override reason is UnSpecified i.e. Pop up is not opened yet
                    if (documentRequest.OverrideReason == Mobius.CoreLibrary.OverrideReason.UNSPECIFIED)
                    {
                        //check if there is already an valid instannce of emergency access in Emergency Audit table
                        this.Result = this.MobiusDAL.CheckEmergencyAudit(documentRequest);
                        if (this.Result.IsSuccess)
                        {
                            //in case of Emergency Access, call the method which is called to get the document for patient, 
                            //because in case of Emergency Access, provider will get to see un-trimmed document
                            this.Result = ProcessDocumentRetrieve(documentRequest, document);
                            return this.Result;
                        }

                    }
                    else
                    {
                        //in case of Emergency Access, call the method which is called to get the document for patient, 
                        //because in case of Emergency Access, provider will get to see un-trimmed document
                        this.Result = ProcessDocumentRetrieve(documentRequest, document);
                        if (!this.Result.IsSuccess)
                            return this.Result;

                        this.Result = this.MobiusDAL.SaveEmergencyAudit(documentRequest);
                        return this.Result;
                    }
                }
                else
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Select_Pupose_As_Emergency);
                    return this.Result;
                }
            }
            //Get the XACML Bytes
            document.XACMLBytes = null;
            if (FileHandler.CheckDocumentExists(document.XACMLDocumentId, documentRequest.FilePathLocation))
            {
                document.XACMLBytes = FileHandler.LoadDocument(document.XACMLDocumentId, documentRequest.FilePathLocation);
            }

            this.Result = HasAcessPermission(documentRequest, out patientConsentId);
            if (!this.Result.IsSuccess)
            {
                //if consent permission is not given and purpose is Emergency then return with message that this is the case of Emergency Override
                if(documentRequest.purpose == Mobius.CoreLibrary.PurposeOfUse.EMERGENCY.ToString())
                    this.Result.SetError(ErrorCode.Emergency_Override_Case);
                
                return this.Result;
            }

            

            if (document.Reposed && documentRequest.LocalData)
            {
                //Check section level mapping as per consent
                this.Result = GetTrimmedDocument(documentRequest, out docByte, patientConsentId);
                if (this.Result.IsSuccess)
                {
                    document.DocumentBytes = docByte;
                }
                else
                {
                    document.DocumentBytes = null;
                }


                //Ends section level mapping based on patient consent
            }
            // Remote Cross Call.
            else
            {


                // get the document from DOD 
                this.Result = this.MobiusConnect.RetrieveDocument(documentRequest.documentId, RemoteCommunityID, document.RepositoryUniqueId, this.Assertion.DocumentRetrieveAssertion, out documentFromConnect);

                if (this.Result.IsSuccess && ValidateC32Document(documentFromConnect.DocumentBytes).IsSuccess)
                {
                    // on success Save Document 
                    if (!documentRequest.LocalData && document.Reposed)
                        foldername = document.Location;
                    else
                        foldername = Guid.NewGuid().ToString();

                    saveLocation = Path.Combine(MobiusAppSettingReader.DocumentPath, foldername);
                    documentRequest.FilePathLocation = saveLocation;
                    if (FileHandler.CreateFolder(saveLocation) && FileHandler.SaveDocument(documentFromConnect.DocumentBytes, Path.Combine(saveLocation, documentRequest.documentId + ".xml")))
                    {
                        //Check section level mapping based on patient consent
                        this.Result = GetTrimmedDocument(documentRequest, out docByte, patientConsentId);
                        if (this.Result.IsSuccess)
                        {
                            document.DocumentBytes = docByte;
                        }
                        else
                        {
                            return this.Result;
                        }
                        //Ends section level mapping based on patient consent

                        // get the XACML document from DOD
                        if (!string.IsNullOrWhiteSpace(XACMLDocumentId))
                        {
                            this.Result = this.MobiusConnect.RetrieveDocument(XACMLDocumentId, RemoteCommunityID, "1", this.Assertion.DocumentRetrieveAssertion, out XACMLDocument);
                            if (this.Result.IsSuccess)
                            {
                                FileHandler.SaveDocument(XACMLDocument.DocumentBytes, Path.Combine(saveLocation, XACMLDocumentId + ".xml"));
                            }
                        }

                        // Update the records in db
                        if (UpdateDocumentMetadata(documentRequest.documentId, foldername))
                        {
                            this.Result.IsSuccess = true;
                        }

                        //// Check the permission 
                        //this.Result = HasAcessPermission(doc);

                    }
                }
            }

            if (this.Result.IsSuccess && (documentRequest.OverrideReason != Mobius.CoreLibrary.OverrideReason.UNSPECIFIED))  //case of emergenny access
                this.Result = this.MobiusDAL.SaveEmergencyAudit(documentRequest);

            return this.Result;
        }

        /// <summary>
        /// To get the consent 
        /// </summary>
        /// <param name="C32Modules"></param>
        /// <param name="patientConsent"></param>
        /// <param name="isEmergencyOverride"></param>
        private void GetC32SectionsWithConsent(List<C32Section> C32Modules, MobiusPatientConsent patientConsent)
        {

            foreach (C32Section item in C32Modules)
            {
                ModulePermission modulePermission = patientConsent.PatientConsentPolicy.Modules.Where(t => t.Id == item.Id).FirstOrDefault();
                if (modulePermission != null)
                {
                    item.Allow = modulePermission.Allow;

                    if (item.ChildSections != null && item.ChildSections.Count > 0)
                    {
                        foreach (Mobius.Entity.Component child in item.ChildSections)
                        {
                            if (modulePermission.Sections != null && modulePermission.Sections.Count > 0)
                            {
                                Consent sectionPermission = modulePermission.Sections.Where(t => t.Id == child.Id).FirstOrDefault();
                                if (sectionPermission != null)
                                {
                                    child.Allow = sectionPermission.Allow;
                                }
                            }

                        }
                    }
                }

            }

        }
        
        #endregion

    }
}
