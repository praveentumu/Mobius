using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using FirstGenesis.Mobius.Server.MobiusHISEService.XACML;
using SAMLAssertion;
using PatientDiscovery;
using DocumentQuery;
using RetrieveDocument;
using FirstGenesis.Mobius.Common;
using System.Data;
using FirstGenesis.Mobius.Server.AdminModule;
using System.Configuration;

namespace FirstGenesis.Mobius.Server.MobiusHISEService
{
  public class Mobius : IDisposable
  {

    #region coded values
    string homeCommunityID = "2.16.840.1.113883.3.1605";
    //string remoteCommunityID = "2.16.840.1.113883.3.348.2.1";
    #endregion

    #region Patient Discovery

    public Person[] SearchPatient(Person Demographics)
    {

      Person[] patient;

      EntityPatientDiscovery patientDiscovery = new EntityPatientDiscovery();

      // passing gender and instance intializing block.

      RespondingGateway_PRPA_IN201305UV02RequestType reqType = new RespondingGateway_PRPA_IN201305UV02RequestType();
      RespondingGateway_PRPA_IN201305UV02RequestType[] req = new RespondingGateway_PRPA_IN201305UV02RequestType[4];
      PRPA_MT201306UV02LivingSubjectAdministrativeGender[] genderArray = new PRPA_MT201306UV02LivingSubjectAdministrativeGender[1];
      PRPA_MT201306UV02LivingSubjectAdministrativeGender gender = new PRPA_MT201306UV02LivingSubjectAdministrativeGender();
      PRPA_IN201305UV02 prpa = new PRPA_IN201305UV02();
      PRPA_IN201306UV02MFMI_MT700711UV01ControlActProcess controlactProc = new PRPA_IN201306UV02MFMI_MT700711UV01ControlActProcess();
      PRPA_IN201305UV02QUQI_MT021001UV01ControlActProcess controlactNew = new PRPA_IN201305UV02QUQI_MT021001UV01ControlActProcess();
      PRPA_MT201306UV02ParameterList paramlist = new PRPA_MT201306UV02ParameterList();
      PRPA_MT201306UV02ParameterList[] paramlistArray = new PRPA_MT201306UV02ParameterList[1];
      PRPA_MT201306UV02QueryByParameter queryParam = new PRPA_MT201306UV02QueryByParameter();
      PRPA_MT201306UV02QueryByParameter[] queryParamArray = new PRPA_MT201306UV02QueryByParameter[1];
      PRPA_MT201306UV02LivingSubjectAdministrativeGender genderList = new PRPA_MT201306UV02LivingSubjectAdministrativeGender();
      PRPA_MT201306UV02LivingSubjectAdministrativeGender[] genderListArray = new PRPA_MT201306UV02LivingSubjectAdministrativeGender[1];

      //
      prpa.id = new II();
      prpa.id.root = "2.16.840.1.113883.3.1605";
      prpa.id.extension = "-5a3e95b1:11d1fa33d45:-7f9b";

      prpa.creationTime = new TS_explicit();
      prpa.creationTime.value = "20091116084800";

      prpa.interactionId = new II();
      prpa.interactionId.root = "2.16.840.1.113883.1.6";
      prpa.interactionId.extension = "PRPA_IN201305UV02";

      prpa.processingCode = new CS();
      prpa.processingCode.code = "T";

      prpa.processingModeCode = new CS();
      prpa.processingModeCode.code = "T";

      prpa.acceptAckCode = new CS();
      prpa.acceptAckCode.code = "AL";

      // reciever code
      prpa.receiver = new MCCI_MT000100UV01Receiver[1] { new MCCI_MT000100UV01Receiver() };
      prpa.receiver[0].typeCode = CommunicationFunctionType.RCV;

      prpa.receiver[0].device = new MCCI_MT000100UV01Device();
      prpa.receiver[0].device.classCode = EntityClassDevice.DEV;
      prpa.receiver[0].device.determinerCode = "INSTANCE";
      prpa.receiver[0].device.id = new II[1] { new II() }; ;
      prpa.receiver[0].device.id[0].root = "1.2.345.678.999";
      prpa.receiver[0].device.asAgent = new MCCI_MT000100UV01Agent();
      prpa.receiver[0].device.asAgent.classCode = "AGNT";
      prpa.receiver[0].device.asAgent.representedOrganization = new MCCI_MT000100UV01Organization();
      prpa.receiver[0].device.asAgent.representedOrganization.classCode = "ORG";
      prpa.receiver[0].device.asAgent.representedOrganization.determinerCode = "INSTANCE";
      prpa.receiver[0].device.asAgent.representedOrganization.id = new II[1] { new II() };

      if (Demographics.LocalMPIID == "")
      {// if local patient id is not provided , then pass remotehomecommunity id in the reciever code.
        prpa.receiver[0].device.asAgent.representedOrganization.id[0].root = "2.16.840.1.113883.3.1605";
      }
      else
      {
        prpa.receiver[0].device.asAgent.representedOrganization.id[0].root = "2.16.840.1.113883.3.348.2.1";
      }

      // Sender
      prpa.sender = new MCCI_MT000100UV01Sender();
      prpa.sender.typeCode = CommunicationFunctionType.SND;
      prpa.sender.device = new MCCI_MT000100UV01Device();
      prpa.sender.device.classCode = EntityClassDevice.DEV;
      prpa.sender.device.determinerCode = "INSTANCE";
      prpa.sender.device.id = new II[1] { new II() };
      prpa.sender.device.id[0].root = "1.2.345.678.999";
      prpa.sender.device.asAgent = new MCCI_MT000100UV01Agent();
      prpa.sender.device.asAgent.classCode = "AGNT";
      prpa.sender.device.asAgent.representedOrganization = new MCCI_MT000100UV01Organization();
      prpa.sender.device.asAgent.representedOrganization.classCode = "ORG";
      prpa.sender.device.asAgent.representedOrganization.determinerCode = "INSTANCE";
      prpa.sender.device.asAgent.representedOrganization.id = new II[1] { new II() };
      prpa.sender.device.asAgent.representedOrganization.id[0].root = "2.16.840.1.113883.3.1605";
      //prpa.sender.device.asAgent.representedOrganization.id[0].root = "2.16.840.1.113883.3.1605";

      // controlActProcess element
      prpa.controlActProcess = new PRPA_IN201305UV02QUQI_MT021001UV01ControlActProcess();
      prpa.controlActProcess.classCode = ActClassControlAct.CACT;
      prpa.controlActProcess.moodCode = x_ActMoodIntentEvent.EVN;
      prpa.controlActProcess.code = new CD();
      prpa.controlActProcess.code.code = "PRPA_TE201305UV02";
      prpa.controlActProcess.code.codeSystem = "2.16.840.1.113883.1.6";

      // controlActProcess/authorOrPerformer element
      prpa.controlActProcess.authorOrPerformer = new QUQI_MT021001UV01AuthorOrPerformer[1] { new QUQI_MT021001UV01AuthorOrPerformer() };
      prpa.controlActProcess.authorOrPerformer[0].typeCode = x_ParticipationAuthorPerformer.AUT;

      COCT_MT090300UV01AssignedDevice assignedDevice = new COCT_MT090300UV01AssignedDevice();
      assignedDevice.id = new II[1] { new II() };
      assignedDevice.id[0].root = "2.16.840.1.113883.3.1605";
      prpa.controlActProcess.authorOrPerformer[0].Item = assignedDevice;

      // controlActProcess/queryByParameter element
      prpa.controlActProcess.queryByParameter = new PRPA_MT201306UV02QueryByParameter();
      prpa.controlActProcess.queryByParameter.queryId = new II();
      prpa.controlActProcess.queryByParameter.queryId.root = "2.16.840.1.113883.3.1605";
      prpa.controlActProcess.queryByParameter.queryId.extension = "-abd3453dcd24wkkks545";
      prpa.controlActProcess.queryByParameter.statusCode = new CS();
      prpa.controlActProcess.queryByParameter.statusCode.code = "new";
      prpa.controlActProcess.queryByParameter.responseModalityCode = new CS();
      prpa.controlActProcess.queryByParameter.responseModalityCode.code = "R";
      prpa.controlActProcess.queryByParameter.responsePriorityCode = new CS();
      prpa.controlActProcess.queryByParameter.responsePriorityCode.code = "I";

      // controlActProcess/queryByParameter/parameterList element
      prpa.controlActProcess.queryByParameter.parameterList = new PRPA_MT201306UV02ParameterList();
      prpa.controlActProcess.queryByParameter.parameterList.livingSubjectAdministrativeGender = new PRPA_MT201306UV02LivingSubjectAdministrativeGender[1] { new PRPA_MT201306UV02LivingSubjectAdministrativeGender() };
      prpa.controlActProcess.queryByParameter.parameterList.livingSubjectAdministrativeGender[0].value = new CE[1] { new CE() };
      prpa.controlActProcess.queryByParameter.parameterList.livingSubjectAdministrativeGender[0].value[0].code = Demographics.Gender;
      prpa.controlActProcess.queryByParameter.parameterList.livingSubjectAdministrativeGender[0].semanticsText = new ST();

      prpa.controlActProcess.queryByParameter.parameterList.livingSubjectAdministrativeGender[0].semanticsText.representation = BinaryDataEncoding.TXT;
      prpa.controlActProcess.queryByParameter.parameterList.livingSubjectAdministrativeGender[0].semanticsText.Text = new string[] { "LivingSubject.administrativeGender" };

      prpa.controlActProcess.queryByParameter.parameterList.livingSubjectBirthTime = new PRPA_MT201306UV02LivingSubjectBirthTime[1] { new PRPA_MT201306UV02LivingSubjectBirthTime() };
      prpa.controlActProcess.queryByParameter.parameterList.livingSubjectBirthTime[0].value = new IVL_TS_explicit[1] { new IVL_TS_explicit() };
      prpa.controlActProcess.queryByParameter.parameterList.livingSubjectBirthTime[0].value[0].value = dateFormatter(Demographics.DOB);
      prpa.controlActProcess.queryByParameter.parameterList.livingSubjectBirthTime[0].semanticsText = new ST();
      prpa.controlActProcess.queryByParameter.parameterList.livingSubjectBirthTime[0].semanticsText.representation = BinaryDataEncoding.TXT;
      prpa.controlActProcess.queryByParameter.parameterList.livingSubjectBirthTime[0].semanticsText.Text = new string[] { "LivingSubject.birthTime" };

      prpa.controlActProcess.queryByParameter.parameterList.livingSubjectName = new PRPA_MT201306UV02LivingSubjectName[1] { new PRPA_MT201306UV02LivingSubjectName() };
      prpa.controlActProcess.queryByParameter.parameterList.livingSubjectName[0].value = new EN_explicit[1] { new EN_explicit() };
      prpa.controlActProcess.queryByParameter.parameterList.livingSubjectName[0].value[0].Items = new ENXP_explicit[2] { new ENXP_explicit(), new ENXP_explicit() };

      en_explicitfamily family = new en_explicitfamily();
      family.partType = "FAM";
      family.Text = new string[] { Demographics.Family };
      prpa.controlActProcess.queryByParameter.parameterList.livingSubjectName[0].value[0].Items[0] = family;

      en_explicitgiven given = new en_explicitgiven();
      given.partType = "GIV";
      given.Text = new string[] { Demographics.Given };
      prpa.controlActProcess.queryByParameter.parameterList.livingSubjectName[0].value[0].Items[1] = given;

      prpa.controlActProcess.queryByParameter.parameterList.livingSubjectId = new PRPA_MT201306UV02LivingSubjectId[1] { new PRPA_MT201306UV02LivingSubjectId() };
      prpa.controlActProcess.queryByParameter.parameterList.livingSubjectId[0].value = new II[1] { new II() };
      prpa.controlActProcess.queryByParameter.parameterList.livingSubjectId[0].value[0].root = "2.16.840.1.113883.3.1605";
      prpa.controlActProcess.queryByParameter.parameterList.livingSubjectId[0].value[0].extension = Demographics.LocalMPIID;//1111
      prpa.controlActProcess.queryByParameter.parameterList.livingSubjectId[0].semanticsText = new ST();
      prpa.controlActProcess.queryByParameter.parameterList.livingSubjectId[0].semanticsText.representation = BinaryDataEncoding.TXT;

      // NHIN target communtity
      PatientDiscovery.NhinTargetCommunityType nhin = new PatientDiscovery.NhinTargetCommunityType();
      PatientDiscovery.HomeCommunityType homeCommunity = new PatientDiscovery.HomeCommunityType();

      if (Demographics.LocalMPIID == "")
      {   //Changes related to patient id not mandatory.
        homeCommunity.homeCommunityId = "2.16.840.1.113883.3.1605";
      }
      else
      {
        homeCommunity.homeCommunityId = "2.16.840.1.113883.3.348.2.1";
      }

      nhin.homeCommunity = homeCommunity;

      //
      reqType.PRPA_IN201305UV02 = prpa;
      reqType.NhinTargetCommunities = new PatientDiscovery.NhinTargetCommunityType[] { nhin };

      PatientDiscovery.Community_PRPA_IN201306UV02ResponseType[] response = patientDiscovery.RespondingGateway_PRPA_IN201305UV02(reqType);
      patient = new Person[1];
      patient[0] = new Person();

      if (response[0] != null && response[0].PRPA_IN201306UV02 != null && response[0].PRPA_IN201306UV02.controlActProcess != null && response[0].PRPA_IN201306UV02.controlActProcess.subject != null)
      {
        if (response[0].PRPA_IN201306UV02.controlActProcess.subject[0].registrationEvent != null && response[0].PRPA_IN201306UV02.controlActProcess.subject[0].registrationEvent.subject1 != null && response[0].PRPA_IN201306UV02.controlActProcess.subject[0].registrationEvent.subject1.patient.id[0] != null)
        {
          patient[0].LocalMPIID = response[0].PRPA_IN201306UV02.controlActProcess.subject[0].registrationEvent.subject1.patient.id[0].extension;
        }
        else
        {
          // error code.
        }
        if (response[0] != null && response[0].PRPA_IN201306UV02 != null && response[0].PRPA_IN201306UV02.sender != null && response[0].PRPA_IN201306UV02.sender.device != null && response[0].PRPA_IN201306UV02.sender.device.asAgent != null && response[0].PRPA_IN201306UV02.sender.device.asAgent.representedOrganization.id[0].root != null)
        {
          patient[0].FacilityID = response[0].PRPA_IN201306UV02.sender.device.asAgent.representedOrganization.id[0].root;
        }
        PRPA_MT201310UV02Person patientPerson = response[0].PRPA_IN201306UV02.controlActProcess.subject[0].registrationEvent.subject1.patient.Item as PRPA_MT201310UV02Person;
        foreach (ENXP_explicit item in patientPerson.name[0].Items)
        {
          if (item.partType.Equals("FAM"))
            patient[0].Family = item.Text[0];
          if (item.partType.Equals("GIV"))
            patient[0].Given = item.Text[0];
        }
        if (patientPerson.administrativeGenderCode.code != null)
          patient[0].Gender = patientPerson.administrativeGenderCode.code;
        if (patientPerson.birthTime.value != null)
          patient[0].DOB = patientPerson.birthTime.value;
      }

      return patient;
    }

    #endregion

    #region Document Query

    public document[] GetDocuments(string patientId, string CommunityIDSearch)
    {

      document[] doc2 = new document[1];
      doc2[0] = new document();

      try
      {
        RespondingGateway_CrossGatewayQueryRequestType respondingGatewayCrossGatewayQueryInRequest = new RespondingGateway_CrossGatewayQueryRequestType();
        // Setting NHIN target values


        DocumentQuery.NhinTargetCommunityType[] objNhinTargetCommunityTypeArray = new DocumentQuery.NhinTargetCommunityType[1];
        objNhinTargetCommunityTypeArray[0] = new DocumentQuery.NhinTargetCommunityType();
        objNhinTargetCommunityTypeArray[0].homeCommunity = new DocumentQuery.HomeCommunityType();
        objNhinTargetCommunityTypeArray[0].homeCommunity.description = "Home Community";
        objNhinTargetCommunityTypeArray[0].homeCommunity.homeCommunityId = homeCommunityID;
        objNhinTargetCommunityTypeArray[0].homeCommunity.name = "Home Community";
        // NHIN settings ends

        // Setting the response option 
        ResponseOptionType objResponseOptionType = new ResponseOptionType();
        objResponseOptionType.returnType = ResponseOptionTypeReturnType.LeafClass; ;
        objResponseOptionType.returnComposedObjects = true; // change one it was false earlier

        // adhocquerytype
        AdhocQueryType objAdhocQueryType = new AdhocQueryType();
        objAdhocQueryType.id = "urn:uuid:14d4debf-8f97-4251-9a74-a90016b0af0d";

        AdhocQueryRequest objAdhocQueryRequest = new AdhocQueryRequest();
        objAdhocQueryRequest.ResponseOption = objResponseOptionType;

        DocumentQuery.SlotType1[] objSlotType1 = new DocumentQuery.SlotType1[2];
        objSlotType1[0] = new DocumentQuery.SlotType1();
        objSlotType1[0].name = "$XDSDocumentEntryPatientId";//"creationTime";
        objSlotType1[0].ValueList = new DocumentQuery.ValueListType();
        objSlotType1[0].ValueList.Value = new string[1];
        objSlotType1[0].ValueList.Value[0] = hl7EncodePatientId(patientId, homeCommunityID);
        objSlotType1[1] = new DocumentQuery.SlotType1();
        objSlotType1[1].name = "$XDSDocumentEntryStatus";
        objSlotType1[1].ValueList = new DocumentQuery.ValueListType();
        objSlotType1[1].ValueList.Value = new string[1];
        objSlotType1[1].ValueList.Value[0] = "('urn:oasis:names:tc:ebxml-regrep:StatusType:Approved')";// "en-us";
        objAdhocQueryType.Slot = objSlotType1;
        objAdhocQueryRequest.AdhocQuery = objAdhocQueryType;

        AdhocQueryRequest oRequest = objAdhocQueryRequest;
        respondingGatewayCrossGatewayQueryInRequest.NhinTargetCommunities = objNhinTargetCommunityTypeArray;
        respondingGatewayCrossGatewayQueryInRequest.AdhocQueryRequest = oRequest;
        EntityDocQuery objEntityDocQuery = new EntityDocQuery();

        AdhocQueryResponse objAdhocQueryResponse = objEntityDocQuery.RespondingGateway_CrossGatewayQuery(respondingGatewayCrossGatewayQueryInRequest);

        // Parsing the response.
        string state = objAdhocQueryResponse.status;
        if (objAdhocQueryResponse.status == "urn:oasis:names:tc:ebxml-regrep:ResponseStatusType:Success")
        {
          ExtrinsicObjectType[] ex;
          ex = objAdhocQueryResponse.RegistryObjectList;
          DocumentQuery.SlotType1[] stotArr;
          ExternalIdentifierType[] exID;

          stotArr = ex[0].Slot;
          exID = ex[0].ExternalIdentifier;


          if (stotArr != null)
          {
            foreach (DocumentQuery.SlotType1 st in stotArr)
            {
              if (st.name == "creationTime" && st.name != null)
              {

                doc2[0].CreatedDate = st.ValueList.Value[0].ToString();
                System.Diagnostics.EventLog myLog3 = new System.Diagnostics.EventLog();
                myLog3.Source = "DocQuerydate";

              }
              if (st.name == "repositoryUniqueId" && st.name != null)
              {
                doc2[0].RepositoryUniqueId = st.ValueList.Value[0].ToString();
              }
              if (st.name == "sourcePatientId" && st.name != null)
              {
                doc2[0].SourcePatientId = st.ValueList.Value[0].ToString();
              }
            }
          }
          // Getting values in extrinsic object type
          if (exID != null && exID[0].value != null)
          {
            doc2[0].DocumentUniqueID = exID[0].value.ToString();
          }
          if (exID != null && exID[1].value != null)
          {
            doc2[0].DocTargetCommID = exID[1].value.ToString();
            //string val = "99990069^^^&2.16.840.1.113883.3.348.2.1&ISO";
            //string val = doc2[0].DocTargetCommID.Substring(13, 27);
            //doc2[0].DocTargetCommID = val;
          }
        }
        else
        {
          // pass the data contract for errorcode.
        }
        return doc2;
      }
      catch (Exception ex)
      {

        throw ex;
      }
    }

    #endregion

    #region Document Retrieve
    public document GetDocument(string DocumentID)
    {
        document Doc = new document();
        FileHandler fh = new FileHandler();
        byte[] docByte = null;
        try
        {
            //determine wheather the document source is local or remote
            DataSet ds = new DataSet();
            ds = GetDocumentMetaData(DocumentID);

            if (ds.Tables[0].Rows[0]["DataSource"].ToString() == "Local")
            {
                string filepathLocation = ds.Tables[0].Rows[0]["Location"].ToString();
                bool bdocExists = fh.CheckDocumentExists(DocumentID, filepathLocation);
                if (bdocExists)
                {
                    docByte = fh.LoadDocument(DocumentID, filepathLocation);
                    MemoryStream mS = new MemoryStream(docByte);
                    Doc.ByteData = docByte;
                }
                else if (!bdocExists)
                {
                    Doc = null;
                }
            }

            else if (ds.Tables[0].Rows[0]["DataSource"].ToString() == "Remote")
            {
                //to check if reposed
                bool breposed = Convert.ToBoolean(ds.Tables[0].Rows[0]["Reposed"]);
                if (breposed)
                {
                    string filepathLocation = ds.Tables[0].Rows[0]["Location"].ToString();
                    bool bdocExists = fh.CheckDocumentExists(DocumentID, filepathLocation);
                    if (bdocExists)
                    {

                        docByte = fh.LoadDocument(DocumentID, filepathLocation);
                        MemoryStream mS = new MemoryStream(docByte);
                        Doc.ByteData = docByte;
                    }
                    else if (!bdocExists)
                    {
                        Doc = null;
                    }
                }
                else
                {

                    string RemoteCommunityID = ds.Tables[0].Rows[0]["SourceCommunityID"].ToString();
                    Doc = RetrieveDocument(DocumentID, RemoteCommunityID, "1");
                    string strfoldername = Guid.NewGuid().ToString();
                    string saveLocation = ConfigurationManager.AppSettings["RetrievePath"].ToString() + strfoldername + "\\" + DocumentID + ".xml";
                    bool bdocSavedsuccessfull = fh.SaveDocument(Doc.ByteData, saveLocation);
                    if (bdocSavedsuccessfull)
                    {
                        string DbPathlocation = ConfigurationManager.AppSettings["RetrievePath"].ToString() + "\\" + strfoldername;
                        bool bUpdateDocumentMetadata = UpdateDocumentMetadata(DocumentID, DbPathlocation);
                        if (bUpdateDocumentMetadata)
                        {
                            docByte = fh.LoadDocument(DocumentID, DbPathlocation);
                            Doc.ByteData = docByte;
                        }
                    }
                }
            }
        }

        catch (Exception ex)
        {

        }
        return Doc;
    }

    public DataSet GetDocumentMetaData(string DocumentID)
    {
        DataSet ds = new DataSet();
        AdminServiceManagement _common = new FirstGenesis.Mobius.Server.AdminModule.AdminServiceManagement();
        ds = _common.GetDocumentMetaData(DocumentID);
        return ds;
    }
    private bool UpdateDocumentMetadata(string DocumentID, string Location)
    {
        bool bUpdated = false;
        AdminServiceManagement _common = new FirstGenesis.Mobius.Server.AdminModule.AdminServiceManagement();
        bUpdated = _common.UpdateDocumentMetadata(DocumentID, Location);
        return bUpdated;
    }
    public document RetrieveDocument(string DocId, string remoteCommunityId, string RepositoryUniqueId)
    {
      RespondingGateway_CrossGatewayRetrieveRequestType objRetrieveRequest = new RespondingGateway_CrossGatewayRetrieveRequestType();
      RetrieveDocumentSetRequestTypeDocumentRequest[] objRetrieveDoc = new RetrieveDocumentSetRequestTypeDocumentRequest[1];
      objRetrieveDoc[0] = new RetrieveDocumentSetRequestTypeDocumentRequest();
      objRetrieveDoc[0].DocumentUniqueId = DocId;
      objRetrieveDoc[0].HomeCommunityId = remoteCommunityId;
      objRetrieveDoc[0].RepositoryUniqueId = RepositoryUniqueId;
      objRetrieveRequest.RetrieveDocumentSetRequest = objRetrieveDoc;
      EntityDocRetrieve objEntityDocRetrieve = new EntityDocRetrieve();
      RetrieveDocumentSetResponseType objRetrieveDocumentSetResponseType = new RetrieveDocumentSetResponseType();
      objRetrieveDocumentSetResponseType = objEntityDocRetrieve.RespondingGateway_CrossGatewayRetrieve(objRetrieveRequest);
      document doc1 = new document();
      //if (objRetrieveDocumentSetResponseType.DocumentResponse[0].Document != null)
      if (objRetrieveDocumentSetResponseType != null && objRetrieveDocumentSetResponseType.DocumentResponse != null)
      {
        doc1.ByteData = objRetrieveDocumentSetResponseType.DocumentResponse[0].Document;
        //doc1.DocumentID = DocId;
      }

      //byte[] buff = ReadByteArrayFromFile("C:\\MobiusClientService\\HITSP_C32v2.5_Elizabeth_Smith.xml");
      //doc1.ByteData = buff;

      //doc1.DocumentID = DocId;
      return doc1;
    }

    #endregion Document Retrieve

    #region Document Upload

    public bool UploadDocument(string HomeCommunityId, string strDocumentId, byte[] docByte, string RuleCreateDate, string RuleStartDate, string RuleEndDate, string patientId)
    {
      string XACMLDocIdPrefix = "1.123403.";
      string C32DocIdPrefix = "1.123402.";
      Random randomNumber = new Random();
      string strUploadDocumentId = C32DocIdPrefix + randomNumber.Next().ToString();
      string strXACMLDocumentId = XACMLDocIdPrefix + randomNumber.Next().ToString();
      string policyFilePath = CreateXACMLPolicy(strXACMLDocumentId, RuleStartDate, RuleEndDate);
      // XACML policy starts
      SAMLAssertionForDocSubmit objSAMLAssertionForDocSubmit = new SAMLAssertionForDocSubmit();
      SAMLAssertion.HarrisStore.EntityXDR_Service objEntityXDR_Service = new SAMLAssertion.HarrisStore.EntityXDR_Service();
      SAMLAssertion.HarrisStore.RespondingGateway_ProvideAndRegisterDocumentSetRequestType objRespondingGateway_ProvideAndRegisterDocumentSetRequestType = new SAMLAssertion.HarrisStore.RespondingGateway_ProvideAndRegisterDocumentSetRequestType();
      // Document submission starts 
      objRespondingGateway_ProvideAndRegisterDocumentSetRequestType.nhinTargetCommunities = objSAMLAssertionForDocSubmit.GetNhinComunity(HomeCommunityId);
      objRespondingGateway_ProvideAndRegisterDocumentSetRequestType.ProvideAndRegisterDocumentSetRequest = GetProvideAndRegisterDocumentSetRequest(HomeCommunityId, strDocumentId, docByte, RuleCreateDate, RuleStartDate, RuleEndDate, patientId, false);
      SAMLAssertion.HarrisStore.RegistryResponseType objRegistryResponseType = objEntityXDR_Service.ProvideAndRegisterDocumentSetb(objRespondingGateway_ProvideAndRegisterDocumentSetRequestType);
      // Document submission ends

      // XACML submission starts

      objRespondingGateway_ProvideAndRegisterDocumentSetRequestType.ProvideAndRegisterDocumentSetRequest = objSAMLAssertionForDocSubmit.GetProvideAndRegisterDocumentSetRequest(HomeCommunityId, strXACMLDocumentId, policyFilePath, Convert.ToDateTime(RuleCreateDate), Convert.ToDateTime(RuleStartDate), Convert.ToDateTime(RuleEndDate), patientId, true);
      SAMLAssertion.HarrisStore.RegistryResponseType objRegistryResponseTypeXACML = objEntityXDR_Service.ProvideAndRegisterDocumentSetb(objRespondingGateway_ProvideAndRegisterDocumentSetRequestType);

      // XACML submission ends
      return true;
    }

    #endregion Document Upload

    #region Private helper methods

    public SAMLAssertion.HarrisStore.ProvideAndRegisterDocumentSetRequestType GetProvideAndRegisterDocumentSetRequest(string homecomunityid, string sDocUniqueId, byte[] docByte, string creationTime, string serviceStartTime, string serviceStopTime, string sPatientId, bool policy)
    {
      SAMLAssertion.HarrisStore.ProvideAndRegisterDocumentSetRequestTypeDocument[] oDoc = new SAMLAssertion.HarrisStore.ProvideAndRegisterDocumentSetRequestTypeDocument[1];
      SAMLAssertionForDocSubmit objSAMLAssertionForDocSubmit = new SAMLAssertionForDocSubmit();
      oDoc[0] = new SAMLAssertion.HarrisStore.ProvideAndRegisterDocumentSetRequestTypeDocument();
      oDoc[0].id = sDocUniqueId;//"1.123402.11112";//Guid.NewGuid().ToString(); 
      oDoc[0].Value = docByte; //ReadByteArrayFromFile(docPath);
      SAMLAssertion.HarrisStore.ProvideAndRegisterDocumentSetRequestType ProvideAndRegisterDocumentSetRequest = new SAMLAssertion.HarrisStore.ProvideAndRegisterDocumentSetRequestType();
      ProvideAndRegisterDocumentSetRequest.SubmitObjectsRequest = new SAMLAssertion.HarrisStore.SubmitObjectsRequest();
      ProvideAndRegisterDocumentSetRequest.SubmitObjectsRequest.id = "123";
      ProvideAndRegisterDocumentSetRequest.SubmitObjectsRequest.comment = "comme";
      SAMLAssertion.HarrisStore.ExtrinsicObjectType objE = new SAMLAssertion.HarrisStore.ExtrinsicObjectType();//ExtrinsicObjectType();
      objE.ExternalIdentifier = new SAMLAssertion.HarrisStore.ExternalIdentifierType[2];
      objE.ExternalIdentifier[0] = setPatientId(sDocUniqueId, sPatientId);
      objE.ExternalIdentifier[1] = setDocumentUniqueId(sDocUniqueId);
      MemoryStream memoryStream = new MemoryStream(docByte);

      XmlDocument XmlDoc = new XmlDocument();

      XmlDoc.Load(memoryStream);

      string PatientFirstName = (((XmlDoc).DocumentElement).ParentNode.ChildNodes[4].ChildNodes[12]).ChildNodes[0].ChildNodes[3].ChildNodes[0]["given"].InnerText;
      string PatientLastName = (((XmlDoc).DocumentElement).ParentNode.ChildNodes[4].ChildNodes[12]).ChildNodes[0].ChildNodes[3].ChildNodes[0]["family"].InnerText;
      string PatientDOB = (((XmlDoc).DocumentElement).ParentNode.ChildNodes[4].ChildNodes[12]).ChildNodes[0].ChildNodes[3].ChildNodes[2].Attributes["value"].Value;
      string PatientGender = (((XmlDoc).DocumentElement).ParentNode.ChildNodes[4].ChildNodes[12]).ChildNodes[0].ChildNodes[3].ChildNodes[1].Attributes["code"].Value;
      string PatientStreetAddress = (((XmlDoc).DocumentElement).ParentNode.ChildNodes[4].ChildNodes[12]).ChildNodes[0].ChildNodes[1]["streetAddressLine"].InnerText;
      string PatientCity = (((XmlDoc).DocumentElement).ParentNode.ChildNodes[4].ChildNodes[12]).ChildNodes[0].ChildNodes[1]["city"].InnerText;
      string PatientState = (((XmlDoc).DocumentElement).ParentNode.ChildNodes[4].ChildNodes[12]).ChildNodes[0].ChildNodes[1]["state"].InnerText;
      string PatientPostalCode = (((XmlDoc).DocumentElement).ParentNode.ChildNodes[4].ChildNodes[12]).ChildNodes[0].ChildNodes[1]["postalCode"].InnerText;
      string hl7PatientID = hl7EncodePatientId(sPatientId, homecomunityid);
      string DocumentCreationDate = (((XmlDoc).DocumentElement).ParentNode.ChildNodes[4].ChildNodes[9]).Attributes["value"].Value.Substring(0, 8);

      // set mime type
      objE.mimeType = CDAConstants.PROVIDE_REGISTER_MIME_TYPE;
      if (policy)
      {
        objE.Classification = new SAMLAssertion.HarrisStore.ClassificationType[3];
        objE.Classification[0] = setClassCode(sDocUniqueId);
        objE.Classification[1] = setFormatCode(sDocUniqueId);
        objE.Classification[2] = setConfidentialityCode(sDocUniqueId);
      }

      objE.id = oDoc[0].id;
      objE.objectType = CDAConstants.PROVIDE_REGISTER_OBJECT_TYPE;
      SAMLAssertion.HarrisStore.SlotType1[] objSlotType1 = new SAMLAssertion.HarrisStore.SlotType1[6];
      objSlotType1[0] = new SAMLAssertion.HarrisStore.SlotType1();
      objSlotType1[0].name = CDAConstants.SLOT_NAME_CREATION_TIME;//"creationTime";
      objSlotType1[0].ValueList = new SAMLAssertion.HarrisStore.ValueListType();
      objSlotType1[0].ValueList.Value = new string[1];
      objSlotType1[0].ValueList.Value[0] = DocumentCreationDate;//DateTime.Now.ToString("yyyyMMdd");
      // "20051224";//DateTime.UtcNow.ToFileTime().ToString(); ToUniversalTime().ToString();//"20051224";// DateTime.UtcNow.ToString();//"20051224";//System.DateTime.Now.Year.ToString() + "0" + System.DateTime.Now.Month.ToString() + "0" + System.DateTime.Now.Day.ToString();

      // put langauge code;

      objSlotType1[1] = new SAMLAssertion.HarrisStore.SlotType1();
      objSlotType1[1].name = "languageCode";
      objSlotType1[1].ValueList = new SAMLAssertion.HarrisStore.ValueListType();
      objSlotType1[1].ValueList.Value = new string[1];
      objSlotType1[1].ValueList.Value[0] = CDAConstants.LANGUAGE_CODE_ENGLISH;// "en-us";
      // put service Start Time;
      objSlotType1[2] = new SAMLAssertion.HarrisStore.SlotType1();
      objSlotType1[2].name = CDAConstants.SLOT_NAME_SERVICE_START_TIME;// "servieStartTime";
      objSlotType1[2].ValueList = new SAMLAssertion.HarrisStore.ValueListType();
      objSlotType1[2].ValueList.Value = new string[1];
      objSlotType1[2].ValueList.Value[0] = DateTime.Now.ToString("yyyyMMdd HH:MM:ss");//"20051224";// DateTime.UtcNow.ToString();//"200412230800";//System.DateTime.Now.Year.ToString() + "0" + System.DateTime.Now.Month.ToString() + "0" + System.DateTime.Now.Day.ToString();
      objSlotType1[3] = new SAMLAssertion.HarrisStore.SlotType1();
      objSlotType1[3].name = "servieStopTime";
      objSlotType1[3].ValueList = new SAMLAssertion.HarrisStore.ValueListType();
      objSlotType1[3].ValueList.Value = new string[1];
      objSlotType1[3].ValueList.Value[0] = DateTime.Now.ToString("yyyyMMdd HH:MM:ss"); //;// DateTime.UtcNow.ToString();// "200412230801";//System.DateTime.Now.Year.ToString() + "0" + System.DateTime.Now.Month.ToString() + "0" + System.DateTime.Now.Day.ToString();
      objSlotType1[4] = new SAMLAssertion.HarrisStore.SlotType1();
      objSlotType1[4].name = "sourcePatientId";
      objSlotType1[4].ValueList = new SAMLAssertion.HarrisStore.ValueListType();
      objSlotType1[4].ValueList.Value = new string[1];
      objSlotType1[4].ValueList.Value[0] = sPatientId;
      // 

      objSlotType1[5] = new SAMLAssertion.HarrisStore.SlotType1();
      objSlotType1[5].name = "sourcePatientInfo";
      objSlotType1[5].ValueList = new SAMLAssertion.HarrisStore.ValueListType();
      objSlotType1[5].ValueList.Value = new string[5];
      // objSlotType1[5].ValueList.Value[0] = "PID-3|ST-1000^^^&1.3.6.1.4.1.21367.2003.3.9&ISO";
      objSlotType1[5].ValueList.Value[0] = "PID-3|" + hl7PatientID;
      objSlotType1[5].ValueList.Value[1] = "PID-5|" + PatientFirstName + "^" + PatientLastName + "^^^";
      objSlotType1[5].ValueList.Value[2] = "PID-7|" + PatientDOB;
      objSlotType1[5].ValueList.Value[3] = "PID-8|" + PatientGender;
      objSlotType1[5].ValueList.Value[4] = "PID-11|" + PatientStreetAddress + "^^" + PatientCity + "^" + PatientState + "^" + PatientPostalCode;
      objE.Slot = objSlotType1;
      objE.status = CDAConstants.PROVIDE_REGISTER_STATUS_APPROVED;
      //objE[0].Name=

      ProvideAndRegisterDocumentSetRequest.SubmitObjectsRequest.RegistryObjectList = new SAMLAssertion.HarrisStore.IdentifiableType[4];
      ProvideAndRegisterDocumentSetRequest.SubmitObjectsRequest.RegistryObjectList[0] = objE;//objIType;// new IdentifiableType[1];
      SAMLAssertion.HarrisStore.RegistryPackageType oRegistryPackage = new SAMLAssertion.HarrisStore.RegistryPackageType();
      oRegistryPackage.ExternalIdentifier = new SAMLAssertion.HarrisStore.ExternalIdentifierType[4];
      oRegistryPackage.objectType = CDAConstants.XDS_REGISTRY_REGISTRY_PACKAGE_TYPE;
      oRegistryPackage.id = CDAConstants.EXTERNAL_IDENTIFICATION_SCHEMA_REGISTRYOBJECT;
      oRegistryPackage.ExternalIdentifier[0] = createExternalIdentifier(CDAConstants.EXTERNAL_IDENTIFICATION_SCHEMA_REGISTRYOBJECT, CDAConstants.EXTERNAL_IDENTIFICATION_SCHEMA_PATIENTID,
                                                                        CDAConstants.EXTERNAL_OBJECT_IDENTIFIER_TYPE, sPatientId,
                                                                        CDAConstants.CHARACTER_SET,
                                                                        CDAConstants.LANGUAGE_CODE_ENGLISH,
                                                                        CDAConstants.PROVIDE_REGISTER_SLOT_NAME_DOC_SUBMISSION_SET_PATIENT_ID);
      oRegistryPackage.ExternalIdentifier[1] = createExternalIdentifier(CDAConstants.EXTERNAL_IDENTIFICATION_SCHEMA_REGISTRYOBJECT,
                                                                        CDAConstants.EXTERNAL_IDENTIFICATION_SCHEMA_UNIQUEID,
                                                                        CDAConstants.EXTERNAL_OBJECT_IDENTIFIER_TYPE, "E013",
                                                                        CDAConstants.CHARACTER_SET,
                                                                        CDAConstants.LANGUAGE_CODE_ENGLISH,
                                                                        CDAConstants.PROVIDE_REGISTER_SLOT_NAME_DOC_SUBMISSION_SET_DOCUMENT_ID);
      oRegistryPackage.ExternalIdentifier[2] = getHomeCommunityIdExternalIdentifier(homecomunityid);
      // add registry package;
      ProvideAndRegisterDocumentSetRequest.SubmitObjectsRequest.RegistryObjectList[1] = oRegistryPackage;
      SAMLAssertion.HarrisStore.AssociationType1 oAssociation = new SAMLAssertion.HarrisStore.AssociationType1();
      oAssociation.associationType = CDAConstants.XDS_REGISTRY_ASSOCIATION_TYPE;
      oAssociation.sourceObject = CDAConstants.EXTERNAL_IDENTIFICATION_SCHEMA_REGISTRYOBJECT;
      oAssociation.targetObject = sDocUniqueId;
      oAssociation.id = Guid.NewGuid().ToString();// (UUID.randomUUID().toString());
      oAssociation.objectType = CDAConstants.XDS_REGISTRY_ASSOCIATION_OBJECT_TYPE;
      oAssociation.Slot = new SAMLAssertion.HarrisStore.SlotType1[1];
      oAssociation.Slot[0] = createSlot("SubmissionSetStatus", "Original");
      // add registry association;
      ProvideAndRegisterDocumentSetRequest.SubmitObjectsRequest.RegistryObjectList[2] = oRegistryPackage;
      SAMLAssertion.HarrisStore.ClassificationType oClassificationSumissionSet = new SAMLAssertion.HarrisStore.ClassificationType();
      oClassificationSumissionSet.classificationNode = CDAConstants.PROVIDE_REGISTER_SUBMISSION_SET_CLASSIFICATION_UUID;
      oClassificationSumissionSet.classifiedObject = CDAConstants.EXTERNAL_IDENTIFICATION_SCHEMA_REGISTRYOBJECT;
      oClassificationSumissionSet.objectType = CDAConstants.CLASSIFICATION_REGISTRY_OBJECT;
      oClassificationSumissionSet.id = Guid.NewGuid().ToString();//(UUID.randomUUID().toString());
      // add registry association;
      ProvideAndRegisterDocumentSetRequest.SubmitObjectsRequest.RegistryObjectList[3] = oClassificationSumissionSet;
      ProvideAndRegisterDocumentSetRequest.Document = oDoc;
      //XmlSerializer serializer = new XmlSerializer(typeof(RespondingGateway_ProvideAndRegisterDocumentSetRequestType));
      //TextWriter writer = new StreamWriter(Server.MapPath("\\SubmissionRequest.xml"));
      //serializer.Serialize(writer, objRegisterDocument);
      //writer.Close();

      //RegistryResponseType objRegistryResponseType = objEntityXDR_Service.ProvideAndRegisterDocumentSetb(objRegisterDocument);

      // To print the response status

      //Response.Write(objRegistryResponseType.status);



      return ProvideAndRegisterDocumentSetRequest;

      //document[] doc = new document[5];

      //return doc;



    }
    private SAMLAssertion.HarrisStore.SlotType1 createSlot(String name, String value)
    {

      SAMLAssertion.HarrisStore.SlotType1 slot = new SAMLAssertion.HarrisStore.SlotType1();

      slot.name = name;

      SAMLAssertion.HarrisStore.ValueListType valList = new SAMLAssertion.HarrisStore.ValueListType();

      valList.Value = new string[1];

      valList.Value[0] = value;

      slot.ValueList = valList; //setValueListvalList);

      return slot;

    }
    private SAMLAssertion.HarrisStore.ExternalIdentifierType getHomeCommunityIdExternalIdentifier(String sHomeCommunityId)
    {

      SAMLAssertion.HarrisStore.ExternalIdentifierType oExtIdTypePatForReg = createExternalIdentifier(CDAConstants.EXTERNAL_IDENTIFICATION_SCHEMA_REGISTRYOBJECT,

          CDAConstants.PROVIDE_REGISTER_SUBMISSION_SET_SOURCE_ID_UUID,

          CDAConstants.EXTERNAL_OBJECT_IDENTIFIER_TYPE,

          sHomeCommunityId,

          CDAConstants.CHARACTER_SET,

          CDAConstants.LANGUAGE_CODE_ENGLISH,

          CDAConstants.PROVIDE_REGISTER_SLOT_NAME_SUBMISSION_SET_SOURCE_ID);



      return oExtIdTypePatForReg;

    }
    private SAMLAssertion.HarrisStore.ExternalIdentifierType createExternalIdentifier(String regObject, String identScheme, String objType, String value, String nameCharSet, String nameLang, String nameVal)
    {

      String id = Guid.NewGuid().ToString();// "id03";// UUID.randomUUID().toString();

      SAMLAssertion.HarrisStore.ExternalIdentifierType idType = new SAMLAssertion.HarrisStore.ExternalIdentifierType();// createExternalIdentifierType();

      idType.id = id;

      idType.registryObject = regObject;

      idType.identificationScheme = identScheme;

      idType.objectType = objType;

      idType.value = value;

      idType.Name = createInternationalStringType(nameCharSet, nameLang, nameVal);

      return idType;

    }
    private SAMLAssertion.HarrisStore.InternationalStringType createInternationalStringType(String charSet, String language, String value)
    {

      SAMLAssertion.HarrisStore.InternationalStringType intStr = new SAMLAssertion.HarrisStore.InternationalStringType();

      SAMLAssertion.HarrisStore.LocalizedStringType locStr = new SAMLAssertion.HarrisStore.LocalizedStringType();

      locStr.charset = charSet;

      locStr.lang = language;

      locStr.value = value;

      intStr.LocalizedString = new SAMLAssertion.HarrisStore.LocalizedStringType[1];

      intStr.LocalizedString[0] = locStr;

      return intStr;

    }
    public SAMLAssertion.HarrisStore.NhinTargetCommunityType[] GetNhinComunity(string strHomeCommunityId)
    {

      SAMLAssertion.HarrisStore.NhinTargetCommunityType[] objNhinTargetCommunityTypeArray = new SAMLAssertion.HarrisStore.NhinTargetCommunityType[1];

      objNhinTargetCommunityTypeArray[0] = new SAMLAssertion.HarrisStore.NhinTargetCommunityType();

      objNhinTargetCommunityTypeArray[0].homeCommunity = new SAMLAssertion.HarrisStore.HomeCommunityType();

      objNhinTargetCommunityTypeArray[0].homeCommunity.description = strHomeCommunityId;

      objNhinTargetCommunityTypeArray[0].homeCommunity.homeCommunityId = strHomeCommunityId;

      objNhinTargetCommunityTypeArray[0].homeCommunity.name = strHomeCommunityId;

      return objNhinTargetCommunityTypeArray;

    }
    private SAMLAssertion.HarrisStore.ExternalIdentifierType setPatientId(String sDocUniqueId, String hl7PatientId)
    {

      SAMLAssertion.HarrisStore.ExternalIdentifierType oExtIdTypePat = createExternalIdentifier(

              sDocUniqueId,

              CDAConstants.EBXML_RESPONSE_PATIENTID_IDENTIFICATION_SCHEME,

              CDAConstants.EXTERNAL_OBJECT_IDENTIFIER_TYPE,

              hl7PatientId,

              CDAConstants.CHARACTER_SET,

              CDAConstants.LANGUAGE_CODE_ENGLISH,

              CDAConstants.PROVIDE_REGISTER_SLOT_NAME_PATIENT_ID);



      return oExtIdTypePat;

    }
    private SAMLAssertion.HarrisStore.ClassificationType setConfidentialityCode(String sDocUniqueId)
    {

      String sConfidentialityCode = "R";

      String sConfidentialityCodeScheme = "2.16.840.1.113883.5.25";

      String sConfidentialityCodeDisplayName = "Restricted";



      SAMLAssertion.HarrisStore.ClassificationType oClassificationConfd = createClassification

(CDAConstants.PROVIDE_REGISTER_CONFIDENTIALITY_CODE_UUID,

              sDocUniqueId,

              "",

              sConfidentialityCode,

              CDAConstants.CLASSIFICATION_REGISTRY_OBJECT,

              CDAConstants.CLASSIFICATION_SCHEMA_CDNAME,

              sConfidentialityCodeScheme,

              CDAConstants.CHARACTER_SET,

              CDAConstants.LANGUAGE_CODE_ENGLISH,

              sConfidentialityCodeDisplayName);



      return oClassificationConfd;



    }
    private SAMLAssertion.HarrisStore.ClassificationType createClassification(String scheme, String clObject, String id, String nodeRep, String objType, String slotName, String slotVal, String nameCharSet, String nameLang, String nameVal)
    {

      SAMLAssertion.HarrisStore.ClassificationType cType = new SAMLAssertion.HarrisStore.ClassificationType();

      cType.classificationScheme = scheme;

      cType.classifiedObject = clObject;

      cType.nodeRepresentation = nodeRep;

      if (id != null && !id.Equals(""))
      {

        cType.id = id;

      }

      else
      {

        cType.id = Guid.NewGuid().ToString();

      }



      cType.objectType = objType;

      cType.Slot = new SAMLAssertion.HarrisStore.SlotType1[1];

      cType.Slot[0] = createSlot(slotName, slotVal);

      cType.Name = createInternationalStringType(nameCharSet, nameLang, nameVal);



      return cType;

    }
    private SAMLAssertion.HarrisStore.ClassificationType setFormatCode(String sDocUniqueId)
    {

      String sFormatCode = CDAConstants.METADATA_FORMAT_CODE_XACML;
      SAMLAssertion.HarrisStore.ClassificationType oClassificationClassCode = createClassification(

              CDAConstants.CLASSIFICATION_SCHEMA_IDENTIFIER_FORMAT_CODE,

              sDocUniqueId,

              "",

              sFormatCode,

              CDAConstants.CLASSIFICATION_REGISTRY_OBJECT,

              CDAConstants.CLASSIFICATION_SCHEMA_CDNAME,

              CDAConstants.METADATA_FORMAT_CODE_SYSTEM,

              CDAConstants.CHARACTER_SET,

              CDAConstants.LANGUAGE_CODE_ENGLISH,

              "");



      return oClassificationClassCode;



    }
    private SAMLAssertion.HarrisStore.ClassificationType setClassCode(String sDocUniqueId)
    {

      SAMLAssertion.HarrisStore.ClassificationType oClassificationClassCode = null;



      oClassificationClassCode = createClassification(

          CDAConstants.XDS_CLASS_CODE_SCHEMA_UUID,

              sDocUniqueId,

              "",

              CDAConstants.METADATA_CLASS_CODE,

              CDAConstants.CLASSIFICATION_REGISTRY_OBJECT,

              CDAConstants.CLASSIFICATION_SCHEMA_CDNAME,

              CDAConstants.CODE_SYSTEM_LOINC_OID,

              CDAConstants.CHARACTER_SET,

              CDAConstants.LANGUAGE_CODE_ENGLISH,

              CDAConstants.METADATA_CLASS_CODE_DISPLAY_NAME);



      return oClassificationClassCode;

    }
    public byte[] ReadByteArrayFromFile(string fileName)
    {

      byte[] buff = null;

      FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);

      BinaryReader br = new BinaryReader(fs);

      long numBytes = new FileInfo(fileName).Length;

      buff = br.ReadBytes((int)numBytes);

      return buff;

    }
    private SAMLAssertion.HarrisStore.ExternalIdentifierType setDocumentUniqueId(String sDocUniqueId)
    {

      SAMLAssertion.HarrisStore.ExternalIdentifierType oExtIdTypePat = createExternalIdentifier(sDocUniqueId,

              CDAConstants.DOCUMENT_ID_IDENT_SCHEME,

              CDAConstants.EXTERNAL_OBJECT_IDENTIFIER_TYPE,

              sDocUniqueId,

              CDAConstants.CHARACTER_SET,

              CDAConstants.LANGUAGE_CODE_ENGLISH,

              CDAConstants.PROVIDE_REGISTER_SLOT_NAME_DOCUMENT_ID);



      return oExtIdTypePat;

    }
    private string CreateXACMLPolicy(string strDocFileName, string ruleStart, string ruleEnd)
    {
      //string strDirectoryName = Server.MapPath("~") + "\\Upload";
      //string strDirectoryName = HostingEnvironment.MapPath("~") + "\\Upload";

      string strDirectoryName = "~\\Upload";
      //string strDirectoryName = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Upload";



      // check folder exists
      if (!Directory.Exists(strDirectoryName))
        Directory.CreateDirectory(strDirectoryName);

      string strFileNameWithPath = strDirectoryName + "\\" + strDocFileName + "XACML.XML";
      XACMLClass objXACMLClass = new XACMLClass();
      objXACMLClass.CreateXACMLPolicy("urn:Policy0001", "Access consent policy", string.Empty, "Rule0001", ""

+ " " + "Can use" + " " + strDocFileName + " " + "for" + " Treatment", "provider1@firstgenesis.com", "", "",

strDocFileName, "Treatment", strFileNameWithPath, ruleStart, ruleEnd);
      return strFileNameWithPath;
    }
    public string hl7EncodePatientId(String patientId, String homeCommunityId)
    {

      String sLocalHomeCommunityId = homeCommunityId;

      if (homeCommunityId.StartsWith("urn:oid:"))
      {

        sLocalHomeCommunityId = sLocalHomeCommunityId.Substring(0, "urn:oid:".Length);

      }



      String encodedPatientId = null;



      if (patientId != null)
      {

        encodedPatientId = "'" + patientId + "^^^&" + sLocalHomeCommunityId + "&ISO" + "'";

      }

      return encodedPatientId;

    }

    private string dateFormatter(string date)
    {
      string correctDate = "";
      if (date.Length == 10)
      {
        string year = date.Substring(6, 4);
        string month = date.Substring(0, 2);
        string date1 = date.Substring(3, 2);
        correctDate = year + month + date1;
      }
      return correctDate;
    }
    #endregion Private helper methods

    void IDisposable.Dispose()
    {
      //TODO ,once the structure is clear as to what all managed resources need to be cleared.
      //throw new NotImplementedException();
    }







  }
}
