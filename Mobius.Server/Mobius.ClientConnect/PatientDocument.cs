using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using DocumentQuery;
using FirstGenesis.Mobius.Server.MobiusHISEService.XACML;
using Mobius.Client.Interface;
using Mobius.CoreLibrary;
using Mobius.Entity;
using Mobius.FileSystem;
using RetrieveDocument;
using SAMLAssertion;
using System.ComponentModel;
using C32Utility;


namespace Mobius.Client
{
    public partial class MobiusConnect : IMobiusConnect
    {
        // private readonly string documentQuerydate = "DocQuerydate";
        #region Document Query
        /// <summary>
        /// This method will fetch the document(s) from gateway
        /// </summary>
        /// <param name="patientId">patient id</param>
        /// <param name="communityId">collection of NHIN community</param>
        /// <param name="patientDocuments">Collection of document</param>
        /// <returns>Result class object will represent success/fail of method</returns>
        public Result GetDocumentMetadata(string patientId, List<MobiusNHINCommunity> NHINCommunities, DocumentQuery.AssertionType assertion , out List<MobiusDocument> patientDocuments )
        {
            patientDocuments = new List<MobiusDocument>();
            try
            {
                int NHINCommunityCtr = 0;
                AdhocQueryRequest adhocQueryRequest = null;
                ResponseOptionType responseOptionType = null;
                AdhocQueryType adhocQueryType = null;
                List<DocumentQuery.SlotType1> slotType1 = null;
                EntityDocQuery entityDocQuery = null;
                AdhocQueryResponse adhocQueryResponse = null;
                RespondingGateway_CrossGatewayQueryRequestType respondingGatewayCrossGatewayQueryInRequest = new RespondingGateway_CrossGatewayQueryRequestType();
                // Setting NHIN target values             
                DocumentQuery.NhinTargetCommunityType[] objNhinTargetCommunityTypeArray = new DocumentQuery.NhinTargetCommunityType[NHINCommunities.Count];
                foreach (MobiusNHINCommunity NHINCommunity in NHINCommunities)
                {
                    objNhinTargetCommunityTypeArray[NHINCommunityCtr] = new DocumentQuery.NhinTargetCommunityType();
                    objNhinTargetCommunityTypeArray[NHINCommunityCtr].homeCommunity = new DocumentQuery.HomeCommunityType();
                    objNhinTargetCommunityTypeArray[NHINCommunityCtr].homeCommunity.description = NHINCommunity.CommunityDescription;
                    objNhinTargetCommunityTypeArray[NHINCommunityCtr].homeCommunity.homeCommunityId = NHINCommunity.CommunityIdentifier;
                    objNhinTargetCommunityTypeArray[NHINCommunityCtr].homeCommunity.name = NHINCommunity.CommunityName;
                    NHINCommunityCtr++;
                }
                // NHIN settings ends
                // Setting the response option 
                responseOptionType = new ResponseOptionType();
                responseOptionType.returnType = ResponseOptionTypeReturnType.LeafClass;
                responseOptionType.returnComposedObjects = true; // change one it was false earlier
                // adhocquerytype
                adhocQueryType = new AdhocQueryType();
                adhocQueryType.id = Helper.AdhocQueryTypeId;

                adhocQueryRequest = new AdhocQueryRequest();
                adhocQueryRequest.ResponseOption = responseOptionType;

                slotType1 = new List<DocumentQuery.SlotType1>();
                DocumentQuery.SlotType1 SlotType = new DocumentQuery.SlotType1();


                SlotType.name = "$XDSDocumentEntryPatientId";
                SlotType.ValueList = new DocumentQuery.ValueListType();
                SlotType.ValueList.Value = new string[1];
                SlotType.ValueList.Value[0] = this.hl7EncodePatientId(patientId, MobiusAppSettingReader.LocalHomeCommunityID);
                slotType1.Add(SlotType);

                SlotType = new DocumentQuery.SlotType1();
                SlotType.name = "$XDSDocumentEntryStatus";
                SlotType.ValueList = new DocumentQuery.ValueListType();
                SlotType.ValueList.Value = new string[1];
                SlotType.ValueList.Value[0] = "('urn:oasis:names:tc:ebxml-regrep:StatusType:Approved')";
                slotType1.Add(SlotType);

                ///TODO Remove below slot code - this temp code to clear DIL test scenarios                 
                //SlotType = new DocumentQuery.SlotType1();
                //SlotType.name = "$XDSDocumentEntryAuthorPerson";
                //SlotType.ValueList = new DocumentQuery.ValueListType();
                //SlotType.ValueList.Value = new string[1];
                //SlotType.ValueList.Value[0] = "('^Hunter^Adam^^^')";
                //slotType1.Add(SlotType); 





                //$XDSDocumentEntryClassCode->Coded element describing the type of document
                //Code from IHE specification 
                //http://www.healthit.gov/sites/default/files/nhin-trial-implementations-query-for-documents-service-interface-specification-v-1.6.8.1-1.pdf
                /*
                Code    - DisplayName                           Additional comments
                34133-9 - SUMMARIZATION OF EPISODE NOTE     -> Used for Summary Patient Record (CCD)
                11502-2 - LABORATORY REPORT.TOTAL           -> Used for Laboratory Reports
                10160-0 - History of Medication             -> Use Used for documents containing only a consumer’s medication history
                44943-9 - Self management:^Patient          -> Used for patient-provided documents, such as from a Personal Health Record.
                */

                //slotType1[2] = new DocumentQuery.SlotType1();
                //slotType1[2].name = "$XDSDocumentEntryClassCode";
                //slotType1[2].ValueList = new DocumentQuery.ValueListType();
                //slotType1[2].ValueList.Value = new string[1];
                ////slotType1[2].ValueList.Value[0] = "34133-9";
                ////As per IHE guideline, XDSDocumentEntryClassCode and XDSDocumentEntryClassCodeScheme should be combined into a string separated by "^^".  
                //slotType1[2].ValueList.Value[0] = "34133-9^^2.16.840.1.113883.6.1";

                //XDSDocumentEntryClassCodeScheme
                //Code
                //slotType1[3] = new DocumentQuery.SlotType1();
                //slotType1[3].name = "$XDSDocumentEntryClassCodeScheme";
                //slotType1[3].ValueList = new DocumentQuery.ValueListType();
                //slotType1[3].ValueList.Value = new string[1];
                //slotType1[3].ValueList.Value[0] = "2.16.840.1.113883.6.1";


                adhocQueryType.Slot = slotType1.ToArray();
                adhocQueryRequest.AdhocQuery = adhocQueryType;

                respondingGatewayCrossGatewayQueryInRequest.NhinTargetCommunities = objNhinTargetCommunityTypeArray;
                respondingGatewayCrossGatewayQueryInRequest.assertion = assertion;
                respondingGatewayCrossGatewayQueryInRequest.AdhocQueryRequest = adhocQueryRequest;
                entityDocQuery = new EntityDocQuery();

                //Call gateway 
                adhocQueryResponse = entityDocQuery.RespondingGateway_CrossGatewayQuery(respondingGatewayCrossGatewayQueryInRequest);

                if (adhocQueryResponse == null || !adhocQueryResponse.status.Contains(Helper.DocumentSearchGateWayResponseSuccess))
                {
                    this.Result.IsSuccess = false;
                    if (adhocQueryResponse.RegistryErrorList != null
                        && adhocQueryResponse.RegistryErrorList.RegistryError != null
                        && adhocQueryResponse.RegistryErrorList.RegistryError.Length > 0)
                    {
                        this.Result.SetError(ErrorCode.Document_GateWayResponse_Error, adhocQueryResponse.RegistryErrorList.RegistryError[0].codeContext);
                    }
                    else
                    {

                     
                        this.Result.SetError(ErrorCode.Document_GateWayResponse_Error);
                    }
                    return this.Result;

                }

                patientDocuments = this.ParseRespondingGateway_CrossGatewayQuery(adhocQueryResponse);

                this.Result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }

            return this.Result;
        }

        #endregion
                
        #region Document Retrieve

        public Result RetrieveDocument(string DocId, string remoteCommunityId, string RepositoryUniqueId, RetrieveDocument.AssertionType assertion, out MobiusDocument document)
        {
            document = null;
            try
            {
                RespondingGateway_CrossGatewayRetrieveRequestType retrieveRequest = new RespondingGateway_CrossGatewayRetrieveRequestType();
                EntityDocRetrieve entityDocRetrieve = new EntityDocRetrieve();
                RetrieveDocument.AssertionType assertionType = new RetrieveDocument.AssertionType();
                RetrieveDocumentSetResponseType retrieveDocumentSetResponseType = new RetrieveDocumentSetResponseType();
                RetrieveDocumentSetRequestTypeDocumentRequest[] retrieveDocument = new RetrieveDocumentSetRequestTypeDocumentRequest[1];

                retrieveDocument[0] = new RetrieveDocumentSetRequestTypeDocumentRequest();
                retrieveDocument[0].DocumentUniqueId = DocId;
                retrieveDocument[0].HomeCommunityId = remoteCommunityId;
                retrieveDocument[0].RepositoryUniqueId = RepositoryUniqueId;
                retrieveRequest.RetrieveDocumentSetRequest = retrieveDocument;


                retrieveRequest.assertion = assertion; 


                retrieveDocumentSetResponseType = entityDocRetrieve.RespondingGateway_CrossGatewayRetrieve(retrieveRequest);

                if (retrieveDocumentSetResponseType != null && retrieveDocumentSetResponseType.RegistryResponse.status != Helper.ResponseFailure && retrieveDocumentSetResponseType.DocumentResponse != null)
                {
                    document = new MobiusDocument();
                    document.DocumentBytes = retrieveDocumentSetResponseType.DocumentResponse[0].Document;
                    this.Result.IsSuccess = true;
                }
                else
                {
                    this.Result.IsSuccess = false;
                    if (retrieveDocumentSetResponseType.RegistryResponse != null
                        && retrieveDocumentSetResponseType.RegistryResponse.RegistryErrorList != null
                        && retrieveDocumentSetResponseType.RegistryResponse.RegistryErrorList.RegistryError != null
                        && retrieveDocumentSetResponseType.RegistryResponse.RegistryErrorList.RegistryError.Length > 0)
                    {
                        this.Result.SetError(ErrorCode.Document_GateWayResponse_Error, retrieveDocumentSetResponseType.RegistryResponse.RegistryErrorList.RegistryError[0].codeContext);
                    }
                    else
                    {
                        this.Result.SetError(ErrorCode.Document_GateWayResponse_Error);
                    }

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

        #endregion Document Retrieve

        #region Document Upload

        public Result UploadDocument(string homeCommunityId, string documentId, byte[] documentBytes, byte[] XACMLdocumentBytes, string patientId, SAMLAssertion.HarrisStore.AssertionType assertion)
        {
            try
            {

                Random randomNumber = new Random();
                this.Result.IsSuccess = true;
                DocumentSubmission objSAMLAssertionForDocSubmit = new DocumentSubmission();
                SAMLAssertion.HarrisStore.EntityXDR_Service objEntityXDR_Service = new SAMLAssertion.HarrisStore.EntityXDR_Service();
                SAMLAssertion.HarrisStore.RespondingGateway_ProvideAndRegisterDocumentSetRequestType objRespondingGateway_ProvideAndRegisterDocumentSetRequestType = new SAMLAssertion.HarrisStore.RespondingGateway_ProvideAndRegisterDocumentSetRequestType();
                // Document submission starts 
                objRespondingGateway_ProvideAndRegisterDocumentSetRequestType.nhinTargetCommunities = objSAMLAssertionForDocSubmit.GetNhinComunity(homeCommunityId);
                objRespondingGateway_ProvideAndRegisterDocumentSetRequestType.ProvideAndRegisterDocumentSetRequest =
                    objSAMLAssertionForDocSubmit.GetProvideAndRegisterDocumentSetRequestType(homeCommunityId, documentId, documentBytes, patientId, "Comments");

                this.HL7PatientId = objSAMLAssertionForDocSubmit.HL7PatientId;
                objRespondingGateway_ProvideAndRegisterDocumentSetRequestType.assertion = assertion; 
                SAMLAssertion.HarrisStore.RegistryResponseType objRegistryResponseType = objEntityXDR_Service.ProvideAndRegisterDocumentSetb(objRespondingGateway_ProvideAndRegisterDocumentSetRequestType);

                if (objRegistryResponseType.status != Helper.DocumentSearchGateWayResponseSuccess)
                {
                    this.Result.IsSuccess = false;
                    if (objRegistryResponseType.RegistryErrorList != null && objRegistryResponseType.RegistryErrorList.RegistryError != null
                        && objRegistryResponseType.RegistryErrorList.RegistryError.Length > 0 && objRegistryResponseType.RegistryErrorList.RegistryError[0].errorCode != null)
                    {
                        string errorMessage= Helper.GetErrorMessage(ErrorCode.DocumentSubmissionFailed)+ " [Gateway Error: "+ objRegistryResponseType.RegistryErrorList.RegistryError[0].errorCode + " ]";
                        this.Result.SetError(ErrorCode.DocumentSubmissionFailed,errorMessage);
                    }
                    else
                    {
                        this.Result.SetError(ErrorCode.DocumentSubmissionFailed);
                    }

                }

                string ruleStartDate = string.Empty;
                string ruleEndDate = string.Empty;
                string ruleCreatedDate = string.Empty;
                string policyFilePath = string.Empty;
                XACMLClass xc = new XACMLClass();
                XACMLHandler xacmldetails = new XACMLHandler();

                xc = xacmldetails.GetXACMLDocumentDetail(XACMLdocumentBytes);
                DateTime dtstart;
                DateTime dtend;
                DateTime dtcreated;

                dtstart = Convert.ToDateTime(xc.RuleStartDate);
                dtend = Convert.ToDateTime(xc.RuleEndDate);
                dtcreated = System.DateTime.Now;

                ruleCreatedDate = DateTime.Now.ToString();



                documentId = documentId + "XACML";
                objRespondingGateway_ProvideAndRegisterDocumentSetRequestType.ProvideAndRegisterDocumentSetRequest =
                objSAMLAssertionForDocSubmit.GetProvideAndRegisterDocumentSetBForPolicyDocument(
                                                                   homeCommunityId,
                                                                   documentId,
                                                                   XACMLdocumentBytes, documentBytes,
                                                                   Convert.ToDateTime(ruleCreatedDate),
                                                                   Convert.ToDateTime(xc.RuleStartDate),
                                                                   Convert.ToDateTime(xc.RuleEndDate),
                                                                   patientId);


                objRespondingGateway_ProvideAndRegisterDocumentSetRequestType.assertion = assertion; 

                SAMLAssertion.HarrisStore.RegistryResponseType objRegistryResponseTypeXACML = objEntityXDR_Service.ProvideAndRegisterDocumentSetb(objRespondingGateway_ProvideAndRegisterDocumentSetRequestType);

                if (objRegistryResponseTypeXACML.status != Helper.DocumentSearchGateWayResponseSuccess)
                {
                    this.Result.IsSuccess = false;
                    if (objRegistryResponseTypeXACML.RegistryErrorList != null && objRegistryResponseTypeXACML.RegistryErrorList.RegistryError != null
                       && objRegistryResponseTypeXACML.RegistryErrorList.RegistryError.Length > 0 && objRegistryResponseTypeXACML.RegistryErrorList.RegistryError[0].errorCode != null)
                    {
                        string errorMessage = Helper.GetErrorMessage(ErrorCode.DocumentSubmissionFailed) + " [Gateway Error: " + objRegistryResponseTypeXACML.RegistryErrorList.RegistryError[0].errorCode + " ]";
                        this.Result.SetError(ErrorCode.DocumentSubmissionFailed, errorMessage);
                    }
                    else
                    {
                        this.Result.SetError(ErrorCode.PolicySubmissionFailed);
                    }
                    return this.Result;
                }
                // XACML submission ends
                this.Result.IsSuccess = true;

            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }

            return this.Result;
        }

        //TO DO 
        //Modify code for accepting dynamic date in 
        public Result UploadDocument(string homeComunityid, string documentId, byte[] documentBytes, byte[] xacmlDocumentBytes, string xacmlDocumentId, string ruleCreatedDate, string ruleStartDate, string ruleEndDate, string remotePatientId, string remoteCommunityId,SAMLAssertion.HarrisStore.AssertionType assertion)
        {
            try
            {
                Random randomNumber = new Random();
                this.Result.IsSuccess = false;
                // XACML policy starts
                DocumentSubmission objSAMLAssertionForDocSubmit = new DocumentSubmission();
                SAMLAssertion.HarrisStore.EntityXDR_Service objEntityXDR_Service = new SAMLAssertion.HarrisStore.EntityXDR_Service();
                SAMLAssertion.HarrisStore.RespondingGateway_ProvideAndRegisterDocumentSetRequestType objRespondingGateway_ProvideAndRegisterDocumentSetRequestType = new SAMLAssertion.HarrisStore.RespondingGateway_ProvideAndRegisterDocumentSetRequestType();
                // Document submission starts                 

                objRespondingGateway_ProvideAndRegisterDocumentSetRequestType.nhinTargetCommunities = objSAMLAssertionForDocSubmit.GetNhinComunity(remoteCommunityId);
                objRespondingGateway_ProvideAndRegisterDocumentSetRequestType.ProvideAndRegisterDocumentSetRequest =
                    objSAMLAssertionForDocSubmit.GetProvideAndRegisterDocumentSetRequestType(remoteCommunityId, documentId, documentBytes, remotePatientId, "Comment");

                this.HL7PatientId = objSAMLAssertionForDocSubmit.HL7PatientId;
                objRespondingGateway_ProvideAndRegisterDocumentSetRequestType.assertion = assertion;
                  
                SAMLAssertion.HarrisStore.RegistryResponseType objRegistryResponseType = objEntityXDR_Service.ProvideAndRegisterDocumentSetb(objRespondingGateway_ProvideAndRegisterDocumentSetRequestType);

                if (objRegistryResponseType.status != Helper.DocumentSearchGateWayResponseSuccess)
                {
                    this.Result.IsSuccess = false;
                    if (objRegistryResponseType.RegistryErrorList != null && objRegistryResponseType.RegistryErrorList.RegistryError != null
                        && objRegistryResponseType.RegistryErrorList.RegistryError.Length > 0 && objRegistryResponseType.RegistryErrorList.RegistryError[0].errorCode != null)
                    {
                        string errorMessage = Helper.GetErrorMessage(ErrorCode.DocumentSubmissionFailed) + " [Gateway Error: " + objRegistryResponseType.RegistryErrorList.RegistryError[0].errorCode + " ]";
                        this.Result.SetError(ErrorCode.DocumentSubmissionFailed, errorMessage);
                    }
                    else
                    {
                        this.Result.SetError(ErrorCode.DocumentSubmissionFailed);
                    }
                }

                // Document submission ends
                // XACML submission starts
                objRespondingGateway_ProvideAndRegisterDocumentSetRequestType.ProvideAndRegisterDocumentSetRequest =
                objSAMLAssertionForDocSubmit.GetProvideAndRegisterDocumentSetBForPolicyDocument(
                                                                    remoteCommunityId,
                                                                    xacmlDocumentId,
                                                                    xacmlDocumentBytes, documentBytes,
                                                                    Convert.ToDateTime(ruleCreatedDate),
                                                                    Convert.ToDateTime(ruleStartDate),
                                                                    Convert.ToDateTime(ruleEndDate),
                                                                    remotePatientId);

                objRespondingGateway_ProvideAndRegisterDocumentSetRequestType.assertion = assertion;
                 
                SAMLAssertion.HarrisStore.RegistryResponseType objRegistryResponseTypeXACML = objEntityXDR_Service.ProvideAndRegisterDocumentSetb(objRespondingGateway_ProvideAndRegisterDocumentSetRequestType);

                if (objRegistryResponseTypeXACML.status != Helper.DocumentSearchGateWayResponseSuccess)
                {
                    this.Result.IsSuccess = false;
                    if (objRegistryResponseTypeXACML.RegistryErrorList != null && objRegistryResponseTypeXACML.RegistryErrorList.RegistryError != null
                       && objRegistryResponseTypeXACML.RegistryErrorList.RegistryError.Length > 0 && objRegistryResponseTypeXACML.RegistryErrorList.RegistryError[0].errorCode != null)
                    {
                        string errorMessage = Helper.GetErrorMessage(ErrorCode.DocumentSubmissionFailed) + " [Gateway Error: " + objRegistryResponseTypeXACML.RegistryErrorList.RegistryError[0].errorCode + " ]";
                        this.Result.SetError(ErrorCode.DocumentSubmissionFailed, errorMessage);
                    }
                    else
                    {
                        this.Result.SetError(ErrorCode.PolicySubmissionFailed);
                    }
                    return this.Result;

                }
                // XACML submission ends
                this.Result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                //TODO
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
                return this.Result;
            }

            return this.Result;
        }

        #endregion Document Upload

        #region Private helper methods

        //public SAMLAssertion.HarrisStore.ProvideAndRegisterDocumentSetRequestType GetProvideAndRegisterDocumentSetRequest(string documentSubmissionComunityid, string sDocUniqueId, byte[] docByte, string sPatientId, bool policy)
        //{
        //    //Create document object 
        //    string[] Author = new string[4];
        //    SAMLAssertion.HarrisStore.ProvideAndRegisterDocumentSetRequestTypeDocument[] oDoc = new SAMLAssertion.HarrisStore.ProvideAndRegisterDocumentSetRequestTypeDocument[1];
        //    SAMLAssertionForDocSubmit objSAMLAssertionForDocSubmit = new SAMLAssertionForDocSubmit();
        //    oDoc[0] = new SAMLAssertion.HarrisStore.ProvideAndRegisterDocumentSetRequestTypeDocument();
        //    oDoc[0].id = sDocUniqueId;
        //    oDoc[0].Value = docByte; //ReadByteArrayFromFile(docPath);


        //    SAMLAssertion.HarrisStore.ProvideAndRegisterDocumentSetRequestType ProvideAndRegisterDocumentSetRequest = new SAMLAssertion.HarrisStore.ProvideAndRegisterDocumentSetRequestType();
        //    ProvideAndRegisterDocumentSetRequest.SubmitObjectsRequest = new SAMLAssertion.HarrisStore.SubmitObjectsRequest();
        //    ProvideAndRegisterDocumentSetRequest.SubmitObjectsRequest.id = "123";
        //    ProvideAndRegisterDocumentSetRequest.SubmitObjectsRequest.comment = "comment";

        //    //Add document to request
        //    //get the Hl 7 format patientId

        //    string hl7PatientID = this.hl7EncodePatientId(sPatientId, documentSubmissionComunityid);
        //    ProvideAndRegisterDocumentSetRequest.Document = oDoc;
        //    SAMLAssertion.HarrisStore.ExtrinsicObjectType extrinsicObjectType = new SAMLAssertion.HarrisStore.ExtrinsicObjectType();
        //    extrinsicObjectType.ExternalIdentifier = new SAMLAssertion.HarrisStore.ExternalIdentifierType[2];

        //    extrinsicObjectType.ExternalIdentifier[0] = this.setPatientId(sDocUniqueId, hl7PatientID);
        //    extrinsicObjectType.ExternalIdentifier[1] = this.setDocumentUniqueId(sDocUniqueId);
        //    CDAHelper CDAHelper = new CDAHelper(docByte);

        //    string PatientFirstName = CDAHelper.PatientName.GivenName[0];
        //    string PatientLastName = CDAHelper.PatientName.FamilyName[0];
        //    string PatientDOB = CDAHelper.PatientDOB;
        //    string PatientGender = CDAHelper.PatientGender;
        //    string PatientStreetAddress = CDAHelper.PatientAddress[0].StreetAddressLine;
        //    string PatientCity = CDAHelper.PatientAddress[0].City;
        //    string PatientState = CDAHelper.PatientAddress[0].State;
        //    string PatientPostalCode = CDAHelper.PatientAddress[0].PostalCode;
        //    string LanguageCode = CDAHelper.Language;

        //    //Author
        //    /*
        //          Person
        //          Institution
        //          Role
        //          Specialty
        //           */
        //    Author[0] = CDAHelper.Authors[0].Person;
        //    Author[1] = CDAHelper.Authors[0].Institution;
        //    Author[2] = CDAHelper.Authors[0].Role;
        //    Author[3] = CDAHelper.Authors[0].Specialty;

        //    extrinsicObjectType.mimeType = CDAConstants.PROVIDE_REGISTER_MIME_TYPE;

        //    extrinsicObjectType.Classification = new SAMLAssertion.HarrisStore.ClassificationType[7];
        //    extrinsicObjectType.Classification[0] = this.setAuthorClassCode(sDocUniqueId, Author);
        //    extrinsicObjectType.Classification[1] = this.setClassCode(
        //                                            sDocUniqueId,
        //                                            CDAHelper.Code,
        //                                            CDAHelper.CodeSystem,
        //                                            CDAHelper.CodeDisplayName,
        //                                            LanguageCode);

        //    extrinsicObjectType.Classification[2] = this.setFormatCode(sDocUniqueId);

        //    extrinsicObjectType.Classification[3] = this.setConfidentialityCode(
        //                                            sDocUniqueId,
        //                                            CDAHelper.ConfidentialityCode,
        //                                            CDAHelper.ConfidentialitySystemCode,
        //                                            CDAHelper.ConfidentialityDisplayName,
        //                                            LanguageCode);



        //    extrinsicObjectType.Classification[4] = this.SetFacilityType(
        //                                            sDocUniqueId,
        //                                            CDAHelper.FacilityTypeCode,
        //                                            CDAHelper.FacilityTypeCodeSystem,
        //                                            CDAHelper.FacilityDisplayName,
        //                                            LanguageCode);
        //    extrinsicObjectType.Classification[5] = this.SetPracticeSetting(sDocUniqueId);

        //    extrinsicObjectType.Classification[6] = this.SetTypeCode(
        //                                            sDocUniqueId,
        //                                            CDAHelper.Code,
        //                                            CDAHelper.CodeSystem,
        //                                            CDAHelper.CodeDisplayName,
        //                                            LanguageCode);


        //    extrinsicObjectType.id = oDoc[0].id;
        //    extrinsicObjectType.objectType = CDAConstants.PROVIDE_REGISTER_OBJECT_TYPE;


        //    List<SAMLAssertion.HarrisStore.SlotType1> SlotTypeList = new List<SAMLAssertion.HarrisStore.SlotType1>();
        //    SAMLAssertion.HarrisStore.SlotType1 SlotType = null;

        //    //SLOT_NAME_CREATION_TIME
        //    SlotType = this.createSlotType(CDAConstants.SLOT_NAME_CREATION_TIME, CDAHelper.DocumentCreationDate);
        //    SlotTypeList.Add(SlotType);

        //    // put langauge code; SLOT_NAME_LANGUAGE_CODE            
        //    SlotType = this.createSlotType(CDAConstants.SLOT_NAME_LANGUAGE_CODE, LanguageCode);
        //    SlotTypeList.Add(SlotType);

        //    // put service Start Time SLOT_NAME_SERVICE_START_TIME
        //    SlotType = this.createSlotType(CDAConstants.SLOT_NAME_SERVICE_START_TIME, CDAHelper.ServiceStartTime);
        //    SlotTypeList.Add(SlotType);
        //    //SLOT_NAME_SERVICE_STOP_TIME
        //    SlotType = this.createSlotType(CDAConstants.SLOT_NAME_SERVICE_STOP_TIME, CDAHelper.ServiceStopTime);
        //    SlotTypeList.Add(SlotType);

        //    //SLOT_NAME_SOURCE_PATIENT_ID
        //    SlotType = this.createSlotType(CDAConstants.SLOT_NAME_SOURCE_PATIENT_ID, this.hl7EncodePatientId(sPatientId, MobiusAppSettingReader.LocalHomeCommunityID));
        //    SlotTypeList.Add(SlotType);

        //    //SLOT_NAME_SOURCE_PATIENT_INFO
        //    SlotType = this.createSlotType(CDAConstants.SLOT_NAME_SOURCE_PATIENT_INFO,
        //       "PID-3|" + hl7PatientID, "PID-5|" + PatientFirstName + "^" + PatientLastName + "^^^", "PID-7|" + PatientDOB
        //       , "PID-8|" + PatientGender, "PID-11|" + PatientStreetAddress + "^^" + PatientCity + "^" + PatientState + "^" + PatientPostalCode);
        //    SlotTypeList.Add(SlotType);

        //    ////SLOT_NAME_INTENDED_RECIPIENT
        //    SlotType = this.createSlotType(CDAConstants.SLOT_NAME_INTENDED_RECIPIENT, documentSubmissionComunityid);
        //    SlotTypeList.Add(SlotType);



        //    extrinsicObjectType.Slot = SlotTypeList.ToArray();
        //    extrinsicObjectType.status = EnumHelper.GetAttributeOfType<DescriptionAttribute>(CDADocumentStatus.Approved); //CDAConstants.PROVIDE_REGISTER_STATUS_APPROVED;

        //    //Set Document Title Column Name
        //    extrinsicObjectType.Name = this.createInternationalStringType(string.Empty, string.Empty, CDAHelper.DocumentTitle);
        //    //Column Description
        //    extrinsicObjectType.Description = this.createInternationalStringType(string.Empty, string.Empty, "Comment- Annual Physical");
        //    ProvideAndRegisterDocumentSetRequest.SubmitObjectsRequest.RegistryObjectList = new SAMLAssertion.HarrisStore.ExtrinsicObjectType[1];
        //    ProvideAndRegisterDocumentSetRequest.SubmitObjectsRequest.RegistryObjectList[0] = extrinsicObjectType;
        //    return ProvideAndRegisterDocumentSetRequest;
        //}

        //private SAMLAssertion.HarrisStore.SlotType1 createSlotType(string CDAConstants, params string[] DocumentCreationDate)
        //{
        //    SAMLAssertion.HarrisStore.SlotType1 SlotType = new SAMLAssertion.HarrisStore.SlotType1();
        //    SlotType.name = CDAConstants;
        //    SlotType.ValueList = new SAMLAssertion.HarrisStore.ValueListType();
        //    SlotType.ValueList.Value = DocumentCreationDate;
        //    return SlotType;
        //}

        //private SAMLAssertion.HarrisStore.SlotType1 createSlot(string name, string value)
        //{
        //    SAMLAssertion.HarrisStore.SlotType1 slot = new SAMLAssertion.HarrisStore.SlotType1();
        //    slot.name = name;
        //    SAMLAssertion.HarrisStore.ValueListType valList = new SAMLAssertion.HarrisStore.ValueListType();
        //    valList.Value = new string[1];
        //    valList.Value[0] = value;
        //    slot.ValueList = valList;
        //    return slot;
        //}

        //private SAMLAssertion.HarrisStore.ExternalIdentifierType getHomeCommunityIdExternalIdentifier(string sHomeCommunityId)
        //{
        //    SAMLAssertion.HarrisStore.ExternalIdentifierType oExtIdTypePatForReg = this.createExternalIdentifier(
        //                                                                            CDAConstants.EXTERNAL_IDENTIFICATION_SCHEMA_REGISTRYOBJECT,
        //                                                                            CDAConstants.PROVIDE_REGISTER_SUBMISSION_SET_SOURCE_ID_UUID,
        //                                                                            CDAConstants.EXTERNAL_OBJECT_IDENTIFIER_TYPE,
        //                                                                            sHomeCommunityId,
        //                                                                            CDAConstants.CHARACTER_SET,
        //                                                                            CDAConstants.LANGUAGE_CODE_ENGLISH,
        //                                                                            CDAConstants.PROVIDE_REGISTER_SLOT_NAME_SUBMISSION_SET_SOURCE_ID);

        //    return oExtIdTypePatForReg;
        //}

        //private SAMLAssertion.HarrisStore.ExternalIdentifierType createExternalIdentifier(
        //                                                         string regObject,
        //                                                         string identScheme,
        //                                                         string objType,
        //                                                         string value,
        //                                                         string nameCharSet,
        //                                                         string nameLang,
        //                                                         string nameVal)
        //{
        //    string id = Guid.NewGuid().ToString();
        //    SAMLAssertion.HarrisStore.ExternalIdentifierType idType = new SAMLAssertion.HarrisStore.ExternalIdentifierType();
        //    idType.id = id;
        //    idType.registryObject = regObject;
        //    idType.identificationScheme = identScheme;
        //    idType.objectType = objType;
        //    idType.value = value;
        //    idType.Name = this.createInternationalStringType(nameCharSet, nameLang, nameVal);
        //    return idType;
        //}

        //private SAMLAssertion.HarrisStore.InternationalStringType createInternationalStringType(string charSet, string language, string value)
        //{
        //    SAMLAssertion.HarrisStore.InternationalStringType intStr = new SAMLAssertion.HarrisStore.InternationalStringType();
        //    SAMLAssertion.HarrisStore.LocalizedStringType locStr = new SAMLAssertion.HarrisStore.LocalizedStringType();
        //    locStr.charset = charSet;
        //    locStr.lang = language;
        //    locStr.value = value;
        //    intStr.LocalizedString = new SAMLAssertion.HarrisStore.LocalizedStringType[1];
        //    intStr.LocalizedString[0] = locStr;
        //    return intStr;
        //}

        //public SAMLAssertion.HarrisStore.NhinTargetCommunityType[] GetNhinComunity(string strHomeCommunityId)
        //{
        //    SAMLAssertion.HarrisStore.NhinTargetCommunityType[] objNhinTargetCommunityTypeArray = new SAMLAssertion.HarrisStore.NhinTargetCommunityType[1];
        //    objNhinTargetCommunityTypeArray[0] = new SAMLAssertion.HarrisStore.NhinTargetCommunityType();
        //    objNhinTargetCommunityTypeArray[0].homeCommunity = new SAMLAssertion.HarrisStore.HomeCommunityType();
        //    objNhinTargetCommunityTypeArray[0].homeCommunity.description = strHomeCommunityId;
        //    objNhinTargetCommunityTypeArray[0].homeCommunity.homeCommunityId = strHomeCommunityId;
        //    objNhinTargetCommunityTypeArray[0].homeCommunity.name = strHomeCommunityId;
        //    return objNhinTargetCommunityTypeArray;
        //}

        //private SAMLAssertion.HarrisStore.ExternalIdentifierType setPatientId(string docUniqueId, string hl7PatientId)
        //{
        //    SAMLAssertion.HarrisStore.ExternalIdentifierType oExtIdTypePat = this.createExternalIdentifier(
        //                                                                     docUniqueId,
        //                                                                     CDAConstants.EBXML_RESPONSE_PATIENTID_IDENTIFICATION_SCHEME,
        //                                                                     CDAConstants.EXTERNAL_OBJECT_IDENTIFIER_TYPE,
        //                                                                     hl7PatientId,
        //                                                                     CDAConstants.CHARACTER_SET,
        //                                                                     CDAConstants.LANGUAGE_CODE_ENGLISH,
        //                                                                     CDAConstants.PROVIDE_REGISTER_SLOT_NAME_PATIENT_ID);
        //    return oExtIdTypePat;
        //}

        //private SAMLAssertion.HarrisStore.ClassificationType createClassification(
        //                                                     string scheme,
        //                                                     string clObject,
        //                                                     string id,
        //                                                     string nodeRep,
        //                                                     string objType,
        //                                                     string slotName,
        //                                                     string slotVal,
        //                                                     string nameCharSet,
        //                                                     string nameLang,
        //                                                     string nameVal)
        //{
        //    SAMLAssertion.HarrisStore.ClassificationType cType = new SAMLAssertion.HarrisStore.ClassificationType();
        //    cType.classificationScheme = scheme;
        //    cType.classifiedObject = clObject;
        //    cType.nodeRepresentation = nodeRep;
        //    if (string.IsNullOrEmpty(id))
        //    {
        //        cType.id = id;
        //    }
        //    else
        //    {
        //        cType.id = Guid.NewGuid().ToString();
        //    }

        //    cType.objectType = objType;
        //    cType.Slot = new SAMLAssertion.HarrisStore.SlotType1[1];
        //    cType.Slot[0] = this.createSlot(slotName, slotVal);
        //    cType.Name = this.createInternationalStringType(nameCharSet, nameLang, nameVal);
        //    return cType;
        //}
        ////FacilityTypeCode
        //private SAMLAssertion.HarrisStore.ClassificationType SetFacilityType(
        //                                                     string docUniqueId,
        //                                                     string nodRep,
        //                                                     string slotValue,
        //                                                     string nameValue,
        //                                                     string langCode)
        //{
        //    SAMLAssertion.HarrisStore.ClassificationType facilityTypeCode = this.createClassification(
        //                                                                    CDAConstants.PROVIDE_REGISTER_FACILITY_TYPE_UUID,
        //                                                                    docUniqueId,
        //                                                                    string.Empty,
        //                                                                    nodRep,
        //                                                                    CDAConstants.CLASSIFICATION_REGISTRY_OBJECT,
        //                                                                    CDAConstants.CLASSIFICATION_SCHEMA_CDNAME,
        //                                                                    slotValue,
        //                                                                    CDAConstants.CHARACTER_SET,
        //                                                                    langCode,
        //                                                                    nameValue);
        //    return facilityTypeCode;
        //}
        ////FormatCode
        //private SAMLAssertion.HarrisStore.ClassificationType setFormatCode(string docUniqueId)
        //{
        //    string sFormatCode = CDAConstants.METADATA_FORMAT_CODE_XACML;
        //    SAMLAssertion.HarrisStore.ClassificationType formatCode = this.createClassification(
        //                                                              CDAConstants.CLASSIFICATION_SCHEMA_IDENTIFIER_CDAR2,
        //                                                              docUniqueId,
        //                                                              string.Empty,
        //                                                              "CDAR2/IHE 1.0",
        //                                                              CDAConstants.CLASSIFICATION_REGISTRY_OBJECT,
        //                                                              CDAConstants.CLASSIFICATION_SCHEMA_CDNAME,
        //                                                              "Connect-a-thon formatCodes",
        //                                                              CDAConstants.CHARACTER_SET,
        //                                                              CDAConstants.LANGUAGE_CODE_ENGLISH,
        //                                                              "CDAR2/IHE 1.0");

        //    return formatCode;
        //}
        ////classificationPracticeSetting
        //private SAMLAssertion.HarrisStore.ClassificationType SetPracticeSetting(string docUniqueId)
        //{
        //    string sFormatCode = CDAConstants.METADATA_FORMAT_CODE_XACML;
        //    SAMLAssertion.HarrisStore.ClassificationType classificationPracticeSetting = null;
        //    classificationPracticeSetting = this.createClassification(
        //                                    CDAConstants.PROVIDE_REGISTER_PRACTICE_SETTING_CD_UUID,
        //                                    docUniqueId,
        //                                    string.Empty,
        //                                    "General Medicine",
        //                                    CDAConstants.CLASSIFICATION_REGISTRY_OBJECT,
        //                                    CDAConstants.CLASSIFICATION_SCHEMA_CDNAME,
        //                                    "Connect-a-thon practiceSettingCodes",
        //                                    CDAConstants.CHARACTER_SET,
        //                                    CDAConstants.LANGUAGE_CODE_ENGLISH,
        //                                    "General Medicine");
        //    return classificationPracticeSetting;
        //}
        ////TypeCode
        //private SAMLAssertion.HarrisStore.ClassificationType SetTypeCode(
        //                                                     string docUniqueId,
        //                                                     string nodRep,
        //                                                     string slotValue,
        //                                                     string nameValue,
        //                                                     string langCode)
        //{
        //    SAMLAssertion.HarrisStore.ClassificationType classificationTypeCode = null;
        //    classificationTypeCode = this.createClassification(
        //                             CDAConstants.CLASSIFICATION_SCHEMA_IDENTIFIER_TYPE_CODE,
        //                             docUniqueId,
        //                             string.Empty,
        //                             nodRep,
        //                             CDAConstants.CLASSIFICATION_REGISTRY_OBJECT,
        //                             CDAConstants.CLASSIFICATION_SCHEMA_CDNAME,
        //                             slotValue,
        //                             CDAConstants.CHARACTER_SET,
        //                             langCode,
        //                             nameValue);
        //    return classificationTypeCode;
        //}
        ////ClassCode
        //private SAMLAssertion.HarrisStore.ClassificationType setClassCode(
        //                                                     string docUniqueId,
        //                                                     string nodRep,
        //                                                     string slotValue,
        //                                                     string nameValue,
        //                                                     string langCode)
        //{
        //    SAMLAssertion.HarrisStore.ClassificationType classificationClassCode = null;
        //    classificationClassCode = this.createClassification(
        //                            CDAConstants.XDS_CLASS_CODE_SCHEMA_UUID,
        //                            docUniqueId,
        //                            string.Empty,
        //                            nodRep,
        //                            CDAConstants.CLASSIFICATION_REGISTRY_OBJECT,
        //                            CDAConstants.CLASSIFICATION_SCHEMA_CDNAME,
        //                            slotValue,
        //                            CDAConstants.CHARACTER_SET,
        //                            langCode,
        //                            nameValue);
        //    return classificationClassCode;
        //}

        //private SAMLAssertion.HarrisStore.ClassificationType setConfidentialityCode(
        //                                                     string docUniqueId,
        //                                                     string nodRep,
        //                                                     string slotValue,
        //                                                     string nameValue,
        //                                                     string langCode)
        //{
        //    if (string.IsNullOrEmpty(slotValue))
        //    {
        //        slotValue = CDAConstants.CONFIDENTIALITY_CODE_SYSTEM;
        //    }

        //    if (string.IsNullOrEmpty(nameValue))
        //    {
        //        switch (nodRep)
        //        {
        //            case "N":
        //                nameValue = CDAConstants.CONFIDENTIALITY_CODE_NORMAL_DISPLAY_NAME;
        //                break;
        //            case "R":
        //                nameValue = CDAConstants.CONFIDENTIALITY_CODE_RESTRICTED_DISPLAY_NAME;
        //                break;
        //            case "V":
        //                nameValue = CDAConstants.CONFIDENTIALITY_CODE_VERY_RESTRICTED_DISPLAY_NAME;
        //                break;
        //            default:
        //                break;
        //        }
        //    }

        //    SAMLAssertion.HarrisStore.ClassificationType confidentialityCode = this.createClassification(
        //                        CDAConstants.PROVIDE_REGISTER_CONFIDENTIALITY_CODE_UUID,
        //                        docUniqueId,
        //                        string.Empty,
        //                        nodRep,
        //                        CDAConstants.CLASSIFICATION_REGISTRY_OBJECT,
        //                        CDAConstants.CLASSIFICATION_SCHEMA_CDNAME,
        //                        slotValue,
        //                        CDAConstants.CHARACTER_SET,
        //                        langCode,
        //                        nameValue);

        //    return confidentialityCode;
        //}

        //public byte[] ReadByteArrayFromFile(string fileName)
        //{
        //    byte[] buff = null;
        //    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        //    BinaryReader br = new BinaryReader(fs);
        //    long numBytes = new FileInfo(fileName).Length;
        //    buff = br.ReadBytes((int)numBytes);
        //    return buff;
        //}

        //private SAMLAssertion.HarrisStore.ExternalIdentifierType setDocumentUniqueId(string docUniqueId)
        //{
        //    SAMLAssertion.HarrisStore.ExternalIdentifierType oExtIdTypePat = this.createExternalIdentifier(
        //                    docUniqueId,
        //                   CDAConstants.DOCUMENT_ID_IDENT_SCHEME,
        //                   CDAConstants.EXTERNAL_OBJECT_IDENTIFIER_TYPE,
        //                   docUniqueId,
        //                   CDAConstants.CHARACTER_SET,
        //                   CDAConstants.LANGUAGE_CODE_ENGLISH,
        //                   CDAConstants.PROVIDE_REGISTER_SLOT_NAME_DOCUMENT_ID);
        //    return oExtIdTypePat;
        //}

        //private string CreateXACMLPolicy(string strDocFileName, string ruleStart, string ruleEnd)
        //{
        //    //string strDirectoryName = Server.MapPath("~") + "\\Upload";
        //    //string strDirectoryName = HostingEnvironment.MapPath("~") + "\\Upload";
        //    string strDirectoryName = "~\\Upload";
        //    //string strDirectoryName = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Upload";
        //    // check folder exists
        //    if (!Directory.Exists(strDirectoryName))
        //    {
        //        Directory.CreateDirectory(strDirectoryName);
        //    }

        //    string strFileNameWithPath = strDirectoryName + "\\" + strDocFileName + "XACML.XML";
        //    XACMLClass objXACMLClass = new XACMLClass();
        //    objXACMLClass.CreateXACMLPolicy("urn:Policy0001", "Access consent policy", string.Empty, "Rule0001", ""
        //                                    + " " + "Can use" + " " + strDocFileName + " " + "for" + " Treatment", "provider1@firstgenesis.com", string.Empty, string.Empty,
        //                                    strDocFileName, "Treatment", strFileNameWithPath, ruleStart, ruleEnd);
        //    return strFileNameWithPath;
        //}

        public string hl7EncodePatientId(string patientId, string homeCommunityId)
        {
            string encodedPatientId = null;
            string sLocalHomeCommunityId = homeCommunityId;

            if (homeCommunityId.StartsWith("urn:oid:"))
            {
                sLocalHomeCommunityId = sLocalHomeCommunityId.Substring(0, "urn:oid:".Length);
            }

            if (patientId != null)
            {
                encodedPatientId = "'" + patientId + "^^^&" + sLocalHomeCommunityId + "&ISO" + "'";
            }

            return encodedPatientId;
        }

        private string dateFormatter(string date)
        {
            string correctDate = string.Empty;
            if (date.Length == 10)
            {
                string year = date.Substring(6, 4);
                string month = date.Substring(0, 2);
                string date1 = date.Substring(3, 2);
                correctDate = year + month + date1;
            }

            return correctDate;
        }


        /// <summary>
        /// This method will return the Patient id from encoded string
        /// </summary>
        /// <param name="value">encoded string contains Community</param>
        /// <returns>PatientId</returns>
        public string DecodeHl7PatientId(string value)
        {
            string patientId = string.Empty;
            string[] arrayValue = value.Split('&');
            if (arrayValue.Length > 1)
            {
                patientId = arrayValue.GetValue(0).ToString().Replace("^", "");
            }

            return patientId;
        }


        /// <summary>
        /// This method will return the community id from encoded string
        /// </summary>
        /// <param name="value">encoded string contains Community</param>
        /// <returns>PatientId</returns>
        public string DecodeHl7CommunityId(string value)
        {
            string CommunityId = string.Empty;
            string[] arrayValue = value.Split('&');
            if (arrayValue.Length > 1)
            {
                CommunityId = arrayValue.GetValue(1).ToString();
            }

            return CommunityId;
        }

        ///// <summary>
        ///// This method will return the list of document after parsing the response of gateway
        ///// </summary>
        ///// <param name="adhocQueryResponse"></param>
        ///// <returns></returns>
        private List<MobiusDocument> ParseRespondingGateway_CrossGatewayQuery(AdhocQueryResponse adhocQueryResponse)
        {
            List<MobiusDocument> documents = new List<MobiusDocument>();
            MobiusDocument document = null;
            ExtrinsicObjectType[] extrinsicObjectTypeList = null;
            DocumentQuery.SlotType1[] slotTypeList = null;
            ExternalIdentifierType[] externalIdentifierType = null;
            string slotTypeValue = String.Empty;

            extrinsicObjectTypeList = adhocQueryResponse.RegistryObjectList;

            if (extrinsicObjectTypeList != null)
            {
                foreach (ExtrinsicObjectType extrinsicObjectType in extrinsicObjectTypeList)
                {
                    document = new MobiusDocument();

                    slotTypeList = extrinsicObjectType.Slot;
                    if (slotTypeList != null)
                    {
                        foreach (DocumentQuery.SlotType1 slotType in slotTypeList)
                        {
                            if (!String.IsNullOrEmpty(slotType.name))
                            {
                                slotTypeValue = Convert.ToString(slotType.ValueList.Value[0]);
                                switch (slotType.name.ToUpper())
                                {
                                    case "CREATIONTIME":
                                        document.CreatedOn = slotTypeValue;
                                        break;
                                    case "REPOSITORYUNIQUEID":
                                        document.RepositoryUniqueId = slotTypeValue;
                                        break;
                                    case "SOURCEPATIENTID":
                                        document.SourcePatientId = this.DecodeHl7PatientId(slotTypeValue);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }

                    externalIdentifierType = extrinsicObjectType.ExternalIdentifier;
                    // Getting values in extrinsic object type
                    if (externalIdentifierType != null)
                    {
                        if (externalIdentifierType.Length > 0)
                        {
                            document.DocumentUniqueId = externalIdentifierType[0].value;
                        }

                        if (externalIdentifierType.Length > 1)
                        {
                            document.SourceCommunityId = this.DecodeHl7CommunityId(externalIdentifierType[1].value);
                            document.DataSource = document.SourceCommunityId == MobiusAppSettingReader.LocalHomeCommunityID ? DocumentSource.LocalGateway.ToString() : DocumentSource.Remote.ToString();
                        }
                    }

                    if (extrinsicObjectType.Name != null && extrinsicObjectType.Name.LocalizedString.Length > 0)
                    {
                        document.DocumentTitle = extrinsicObjectType.Name.LocalizedString[0].value;
                    }

                    if (extrinsicObjectType.Classification != null && extrinsicObjectType.Classification.Length > 0)
                    {
                        if (extrinsicObjectType.Classification[0].Slot.Length > 0)
                        {
                            document.Author = Convert.ToString(extrinsicObjectType.Classification[0].Slot[0].ValueList.Value[0]);
                        }
                    }

                    documents.Add(document);
                }
            }

            return documents;
        }

        //private SAMLAssertion.HarrisStore.ClassificationType createAuthorClassification(
        //                                                     string schema,
        //                                                     string clObject,
        //                                                     string id,
        //                                                     string nodeRep,
        //                                                     string objType,
        //                                                     string slotName,
        //                                                     string slotVal,
        //                                                     string nameCharSet,
        //                                                     string nameLang,
        //                                                     string nameVal,
        //                                                     string[] author)
        //{

        //    SAMLAssertion.HarrisStore.ClassificationType cType = new SAMLAssertion.HarrisStore.ClassificationType();
        //    cType.classificationScheme = schema;
        //    //cType.classifiedObject = clObject;
        //    // cType.nodeRepresentation = nodeRep;
        //    if (string.IsNullOrEmpty(id))
        //    {
        //        cType.id = id;
        //    }
        //    else
        //    {
        //        cType.id = Guid.NewGuid().ToString();
        //    }
        //    // cType.objectType = objType;
        //    cType.Slot = new SAMLAssertion.HarrisStore.SlotType1[author.Length];
        //    //authorPerson
        //    cType.Slot[0] = new SAMLAssertion.HarrisStore.SlotType1();
        //    cType.Slot[0].name = "authorPerson";
        //    cType.Slot[0].ValueList = new SAMLAssertion.HarrisStore.ValueListType();
        //    cType.Slot[0].ValueList.Value = new string[1];
        //    cType.Slot[0].ValueList.Value[0] = author[0];
        //    //authorInstitution
        //    cType.Slot[1] = new SAMLAssertion.HarrisStore.SlotType1();
        //    cType.Slot[1].name = "authorInstitution";
        //    cType.Slot[1].ValueList = new SAMLAssertion.HarrisStore.ValueListType();
        //    cType.Slot[1].ValueList.Value = new string[1];
        //    cType.Slot[1].ValueList.Value[0] = author[1];
        //    //authorRole
        //    cType.Slot[2] = new SAMLAssertion.HarrisStore.SlotType1();
        //    cType.Slot[2].name = "authorRole";
        //    cType.Slot[2].ValueList = new SAMLAssertion.HarrisStore.ValueListType();
        //    cType.Slot[2].ValueList.Value = new string[1];
        //    cType.Slot[2].ValueList.Value[0] = author[2];
        //    //authorSpecialty
        //    cType.Slot[3] = new SAMLAssertion.HarrisStore.SlotType1();
        //    cType.Slot[3].name = "authorSpecialty";
        //    cType.Slot[3].ValueList = new SAMLAssertion.HarrisStore.ValueListType();
        //    cType.Slot[3].ValueList.Value = new string[1];
        //    // TO DO  : Remove hard coding of C- 32 document of Randall Jones.
        //    cType.Slot[3].ValueList.Value[0] = author[3];
        //    return cType;
        //}


        //private SAMLAssertion.HarrisStore.ClassificationType setAuthorClassCode(string docUniqueId, string[] author)
        //{
        //    SAMLAssertion.HarrisStore.ClassificationType oClassificationClassCode = null;
        //    oClassificationClassCode = this.createAuthorClassification(
        //                               CDAConstants.CLASSIFICATION_SCHEMA_AUTHOR_IDENTIFIER,
        //                               docUniqueId,
        //                               string.Empty,
        //                               CDAConstants.METADATA_CLASS_CODE,
        //                               CDAConstants.CLASSIFICATION_REGISTRY_OBJECT,
        //                               CDAConstants.CLASSIFICATION_SCHEMA_CDNAME,
        //                               CDAConstants.CODE_SYSTEM_LOINC_OID,
        //                               CDAConstants.CHARACTER_SET,
        //                               CDAConstants.LANGUAGE_CODE_ENGLISH,
        //                               CDAConstants.METADATA_CLASS_CODE_DISPLAY_NAME,
        //                               author);
        //    return oClassificationClassCode;

        //}

        #endregion Private helper methods



    }
}
