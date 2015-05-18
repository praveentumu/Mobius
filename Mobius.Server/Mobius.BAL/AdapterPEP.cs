using System;
using System.Collections.Generic;
using AdapterPEP;
using FirstGenesis.Mobius.Server.MobiusHISEService.XACML;
using System.Xml;
using Mobius.Entity;
using Mobius.DAL;
using Mobius.CoreLibrary;
using System.Linq;
using System.Linq.Expressions;

namespace Mobius.BAL
{
    public class PEP : IDisposable
    {


        #region properties
        private MobiusDAL _MobiusDAL = null;
        private Result _Result = null;
        private CheckPolicyResponseType _CheckPolicyResponseType = null;

        private string PatientID
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
                if (_Result != null) _Result = new Result();
                _Result = value;
            }
        }

        private MobiusDAL MobiusDAL
        {
            get
            {
                return _MobiusDAL != null ? _MobiusDAL : _MobiusDAL = new MobiusDAL();
            }
        }

        internal CheckPolicyResponseType CheckPolicyResponseType
        {
            get
            {
                if (_CheckPolicyResponseType == null)
                {
                    _CheckPolicyResponseType = new CheckPolicyResponseType();
                    CreateResponse(DecisionType.Deny);
                }

                return _CheckPolicyResponseType;
            }
            set
            {
                if (_CheckPolicyResponseType != null)
                {
                    _CheckPolicyResponseType = new CheckPolicyResponseType();
                    CreateResponse(DecisionType.Deny);
                }
                _CheckPolicyResponseType = value;
            }
        }

        #endregion properties


        /*
         * Outbound -> Outbound to initiating gateway (example Mobius Gateway) 
         *             sends patient information to another gateway (responding gateway example: DoD Gateway).
         * Inbound ->  Inbound to initiating gateway (Example DoD Gateway) receives patient information from 
         *             another gateway (responding gateway example Mobius server).
         * ---------------------------------------------------------------------------------------------------------            
         */
        /// <summary>
        /// Process Policy Engine Request
        /// </summary>
        /// <param name="checkPolicyRequest"></param>
        /// <returns></returns>
        internal Result ProcessPolicyEngineRequest(CheckPolicyRequestType checkPolicyRequest)
        {

            string action = string.Empty;
            action = this.getPolicyAction(checkPolicyRequest);

            //////create response for success;
            //CreateResponse(DecisionType.Permit);


            switch (action)
            {
                case "PATIENTDISCOVERYOUT":
                case "PATIENTDISCOVERYIN":
                    this.patientDiscoveryOut(checkPolicyRequest);
                    this.Result.IsSuccess = true;
                    break;
                case "DOCUMENTQUERYOUT":
                case "DOCUMENTQUERYIN":
                    this.documentQuery(checkPolicyRequest);
                    this.Result.IsSuccess = true;
                    break;
                case "DOCUMENTRETRIEVEIN":
                case "DOCUMENTRETRIEVEOUT":
                    this.documentRetrive(checkPolicyRequest);
                    this.Result.IsSuccess = true;
                    break;
                case "XDROUT":
                case "XDRIN":
                    this.documentSubmission(checkPolicyRequest);
                    this.Result.IsSuccess = true;
                    break;
                default:
                    CreateResponse(DecisionType.NotApplicable);
                    this.Result.IsSuccess = true;
                    break;
            }

            return this.Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkPolicyRequest"></param>
        private void patientDiscoveryOut(CheckPolicyRequestType checkPolicyRequest)
        {

            string patientId = this.getPatientId(checkPolicyRequest);
            // Local MPIID is provided              
            if (getPatientOptInStatus(patientId))
            {
                //create response for success;
                CreateResponse(DecisionType.Permit);
            }

        }


        /// <summary>
        ///   PatientDiscoveryOut (Outbound)-> 
        ///   Before sending the patient information Patient consent should be verified. 
        ///   Local Patient ID is available for this transaction to verify Policy.
        /// PatientDiscoveryIn (Inbound)
        /// Before responding to initiating gateway you should verify the consent.
        /// Perform a search for Patient and look for consent information. 
        /// Here you have demographic data like Patient first name, last name, DoB, Gender 
        /// if you have more patients with similar data all you need to do is Deny. 
        /// For now we are not supporting this for multiple patients.        
        /// </summary>
        /// <param name="checkPolicyRequest"></param>
        private void patientDiscovery(CheckPolicyRequestType checkPolicyRequest)
        {
            string patientId = string.Empty;
            patientId = searchPatient(checkPolicyRequest);
            // Local MPIID is provided              
            if (getPatientOptInStatus(patientId))
            {
                //create response for success;
                CreateResponse(DecisionType.Permit);
            }
        }


        /// <summary>
        /// DocumentQueryOut  (Outbound)
        /// You are requesting other community for Patient document information
        /// and you will anyways have your local patient Id as part of request. 
        /// Use this to check the Patient Consent.
        /// DocumentQueryIn (Inbound) 
        /// Before disclosing the patient document meta-data we wanted to ensure what type of documents
        /// and document information we can share. 
        /// You will have a local patient Id for the patient to verify Patient Consent as part of this request. 
        /// This is pulled from correlations table.
        /// </summary>
        /// <param name="checkPolicyRequest"></param>
        private void documentQuery(CheckPolicyRequestType checkPolicyRequest)
        {
            // Assume patient Id is given
            //Considering patient OptIn status as consent. 
            string patientId = string.Empty;
            //Assuming Local MPIID is provided  
            patientId = this.getPatientId(checkPolicyRequest);
            if (getPatientOptInStatus(patientId))
            {
                //create response for success;
                CreateResponse(DecisionType.Permit);
            }
        }

        /// <summary>
        /// XDRIN (Inbound) -> 
        ///
        /// XDROUT (Outbound) ->
        ///
        /// </summary>
        /// <param name="checkPolicyRequest"></param>
        private void documentSubmission(CheckPolicyRequestType checkPolicyRequest)
        {
            // Assume patient Id is given
            //Considering patient OptIn status as consent. 
            string patientId = string.Empty;
            int iResult = 0;
            string documentId = string.Empty;
            //Assuming Local MPIID is provided  
            patientId = this.getPatientId(checkPolicyRequest);
            if (getPatientOptInStatus(patientId))
            {
                CreateResponse(DecisionType.Permit);

            }
        }


        /// <summary>
        /// DocumentRetriveIn (Inbound) -> 
        /// Similar to DocumentQuery only change here is you will get the DocumentId instead of Patient Id.
        /// Look for the local document Id in policy request and then look for Patient ID to use.
        /// DocumentRetriveOut (Outbound) ->
        /// Similar to DocumentQuery only change here is you will get DocumentID instead of Patient Id.
        /// Look for the local document Id in Policy Request and then look for patient associated with the document.
        /// </summary>
        /// <param name="checkPolicyRequest"></param>
        private void documentRetrive(CheckPolicyRequestType checkPolicyRequest)
        {
            // Assume patient Id is given
            //Considering patient OptIn status as consent. 
            string patientId = string.Empty;
            int iResult = 0;
            string documentId = string.Empty;
            ////Assuming Local MPIID is provided        
            //get the document Id from request and then verify the record exists in database for this patient or not.
            documentId = this.getDocumentId(checkPolicyRequest);
            MobiusDocument document = new MobiusDocument();
            this.Result = this.MobiusDAL.GetDocumentMetaData(documentId, out document);
            if (!checkCallIsPatient(checkPolicyRequest, document.SourcePatientId))
            {

                if (Result.IsSuccess && document != null && getPatientOptInStatus(document.SourcePatientId))
                {
                    XACMLClass XACMLClass = this.ParseRequest(checkPolicyRequest);
                    DocumentRequest documentRequest = new DocumentRequest();
                    documentRequest.patientId = document.SourcePatientId;
                    documentRequest.subjectRole = XACMLClass.Subject.First();
                    documentRequest.purpose = XACMLClass.PurposeofUse;

                    if (this.MobiusDAL.HasAccessPermission(documentRequest, out iResult).IsSuccess)
                    {
                        CreateResponse(DecisionType.Permit);
                    }
                }
            }
            else
            {
                CreateResponse(DecisionType.Permit);
            }
        }

        private bool checkCallIsPatient(CheckPolicyRequestType checkPolicyRequest, string patientId)
        {
            if (checkPolicyRequest.assertion != null
                && checkPolicyRequest.assertion.userInfo != null
                && checkPolicyRequest.assertion.userInfo.roleCoded != null
                && !string.IsNullOrEmpty(checkPolicyRequest.assertion.userInfo.roleCoded.code)
                && checkPolicyRequest.assertion.uniquePatientId != null
                && checkPolicyRequest.assertion.uniquePatientId.Length > 0
                && PatientExists(checkPolicyRequest.assertion.uniquePatientId, patientId))
            {


                if (UserRole.Patient.GetHashCode().ToString().Equals(checkPolicyRequest.assertion.userInfo.roleCoded.code))
                {
                    return true;
                }
            }
            return false;
        }

        private bool PatientExists(string[] hl7patientIds, string patientId)
        {
            foreach (var item in hl7patientIds)
            {
                if (DecodeHl7PatientId(item).Equals(patientId))
                    return true;
            }
            return false;
        }

        #region Response
        /// <summary>
        ///  This method will create response based on decision type
        /// </summary>
        /// <param name="categoryID">categoryID int</param>
        /// <returns>return CheckPolicyResponseType class object</returns>
        private void CreateResponse(DecisionType decisionType)
        {
            ResultType resultType = null;
            try
            {
                resultType = new ResultType();
                resultType.Decision = decisionType;
                this.CheckPolicyResponseType.response = new ResultType[1] { resultType };

            }
            catch (Exception)
            {
                resultType.Decision = DecisionType.Deny;
                this.CheckPolicyResponseType.response[0] = resultType;
            }
        }

        #endregion Response

        #region Helper Methods - Call Database
        /// <summary>
        /// This method will search the patient based on demographics
        /// </summary>
        /// <param name="checkPolicyRequest"></param>
        /// <returns></returns>
        private string searchPatient(CheckPolicyRequestType checkPolicyRequest)
        {
            string patientId = string.Empty;
            List<Patient> patientList = null;
            //GetPatient demographic data
            Demographics demographics = this.getPatientDemographics(checkPolicyRequest);
            //Search Patient 
            this.Result = this.MobiusDAL.SearchPatient(demographics, out patientList);

            if (this.Result.IsSuccess)
            {
                if (patientList != null && patientList.Count == 1)
                {
                    patientId = patientList[0].LocalMPIID;
                }
            }
            return patientId;
        }

        /// <summary>
        /// This method will call the db to verify the status of patient Opt-In state.
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        private bool getPatientOptInStatus(string patientId)
        {
            bool status = false;
            if (!string.IsNullOrEmpty(patientId))
            {
                this.Result = this.MobiusDAL.GetOptInStatus(patientId, out status);
            }
            return status;
        }

        #endregion Helper Methods - Call Database

        #region Helper Methods - Read request properties


        /// <summary>
        /// Parse Policy Request Type
        /// </summary>
        /// <param name="checkPolicyRequest">checkPolicyRequest object</param>
        /// <param name="patientID"></param>
        /// <returns>return XACMLClass class object</returns>
        private XACMLClass ParseRequest(CheckPolicyRequestType checkPolicyRequest)
        {
            XACMLClass XACMLClass = null;
            this.PatientID = string.Empty;
            string organizationid = string.Empty;

            if (checkPolicyRequest.request != null && checkPolicyRequest.request.Subject != null && checkPolicyRequest.request.Subject.Length > 0)
            {
                SubjectType subject = (SubjectType)checkPolicyRequest.request.Subject[0];
                XACMLClass = new XACMLClass();
                AttributeType[] Attributes = subject.Attribute;

                foreach (AttributeType attributeType in Attributes)
                {
                    if (attributeType.AttributeId.Contains("subject:organization-id"))
                    {
                        if (attributeType.AttributeValue != null)
                        {
                            foreach (XmlElement xmlElement in attributeType.AttributeValue)
                            {
                                organizationid = xmlElement.ChildNodes[0].InnerText.Trim();
                                break;
                            }
                        }
                    }

                    if (attributeType.AttributeId.Contains("subject:role"))
                    {
                        if (attributeType.AttributeValue != null)
                        {
                            foreach (XmlElement xmlElement in attributeType.AttributeValue)
                            {
                                //XACMLClass.Subject = xmlElement.ChildNodes[0].InnerText.Trim();
                                //break;

                                XACMLClass.Subject = new List<string>();
                                for (int count = 0; count < xmlElement.ChildNodes.Count; count++)
                                {
                                    XACMLClass.Subject.Add(xmlElement.ChildNodes[count].InnerText.Trim());
                                }
                                break;
                            }
                        }
                    }


                    if (attributeType.AttributeId.Contains("subject:purposeofuse"))
                    {
                        if (attributeType.AttributeValue != null)
                        {
                            foreach (XmlElement xmlElement in attributeType.AttributeValue)
                            {
                                XACMLClass.PurposeofUse = xmlElement.ChildNodes[0].InnerText.Trim();
                                break;
                            }
                        }
                    }
                }
            }

            return XACMLClass;
        }



        /// <summary>
        /// Thos method get the document Id from request
        /// </summary>
        /// <param name="checkPolicyRequest"></param>
        /// <returns></returns>
        private string getDocumentId(CheckPolicyRequestType checkPolicyRequest)
        {
            string documentId = string.Empty;
            //Get PatientId
            if (checkPolicyRequest.request != null && checkPolicyRequest.request.Resource != null && checkPolicyRequest.request.Resource.Length > 0)
            {
                AttributeType[] Attributes = checkPolicyRequest.request.Resource[0].Attribute;

                foreach (AttributeType attributeType in Attributes)
                {
                    if (attributeType.AttributeId.Contains("document-id"))
                    {
                        if (attributeType.AttributeValue != null && attributeType.AttributeValue.Length > 0)
                        {
                            return documentId = attributeType.AttributeValue[0].InnerText;
                        }
                    }
                }
            }

            return documentId;
        }

        /// <summary>
        /// Get the patient demographics from request
        /// </summary>
        /// <param name="checkPolicyRequest"></param>
        /// <returns></returns>
        private Demographics getPatientDemographics(CheckPolicyRequestType checkPolicyRequest)
        {
            Demographics demographics = new Demographics();
            demographics.DOB = this.ParseDate(checkPolicyRequest.assertion.dateOfBirth);
            demographics.FamilyName = checkPolicyRequest.assertion.personName.familyName;
            demographics.GivenName = checkPolicyRequest.assertion.personName.givenName;

            if (checkPolicyRequest.assertion.phoneNumber != null)
            {
                Telephone Telephone = new Entity.Telephone();
                Telephone.Number = checkPolicyRequest.assertion.phoneNumber.localNumber;
                demographics.Telephones.Add(Telephone);
            }
            return demographics;
        }


        /// <summary>
        /// This method will extract the patient Id from the request
        /// </summary>
        /// <param name="checkPolicyRequest">Patient Id</param>
        private string getPatientId(CheckPolicyRequestType checkPolicyRequest)
        {
            string patientId = string.Empty;

            //Get PatientId
            if (checkPolicyRequest.request != null && checkPolicyRequest.request.Resource != null && checkPolicyRequest.request.Resource.Length > 0)
            {
                AttributeType[] Attributes = checkPolicyRequest.request.Resource[0].Attribute;

                foreach (AttributeType attributeType in Attributes)
                {

                    if (attributeType.AttributeId.Contains("resource-id"))
                    {

                        foreach (XmlElement xmlElement in attributeType.AttributeValue)
                        {
                            foreach (XmlNode innerNode in xmlElement.ChildNodes)
                            {
                                return patientId = innerNode.Value;

                            }
                        }
                    }

                    if (attributeType.AttributeId.Contains("subject-id") && attributeType.DataType.Contains("hl7"))
                    {
                        if (attributeType.AttributeValue != null)
                        {
                            foreach (XmlElement xmlElement in attributeType.AttributeValue)
                            {
                                foreach (XmlNode innerNode in xmlElement.ChildNodes)
                                {
                                    foreach (XmlAttribute attribute in innerNode.Attributes)
                                    {
                                        if (attribute.Name.Equals("extension"))
                                        {
                                            return patientId = attribute.Value;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }//-PatientId
            return patientId;
        }

        /// <summary>
        /// This will get the policy engine action based action workflow processed
        /// </summary>
        /// <param name="checkPolicyRequest"></param>
        private string getPolicyAction(CheckPolicyRequestType checkPolicyRequest)
        {
            string action = string.Empty;
            if (checkPolicyRequest.request != null && checkPolicyRequest.request.Item != null
                  && checkPolicyRequest.request.Item is ActionType1)
            {
                ActionType1 actionType1 = checkPolicyRequest.request.Item as ActionType1;

                if (actionType1.Attribute != null && actionType1.Attribute.Length > 0)
                {
                    foreach (var item in actionType1.Attribute)
                    {
                        if (item.AttributeId.Contains("action-id"))
                        {
                            if (item.AttributeValue != null && item.AttributeValue.Length > 0)
                            {
                                return action = item.AttributeValue[0].InnerText.ToUpper();
                            }
                        }

                    }
                }
            }
            return action;
        }

        private string DecodeHl7PatientId(string value)
        {
            string patientId = string.Empty;
            string[] arrayValue = value.Split('&');
            if (arrayValue.Length > 1)
            {
                patientId = arrayValue.GetValue(0).ToString().Replace("^", "").Replace("'", "");

            }

            return patientId;
        }

        private string ParseDate(string date)
        {
            DateTime retval = DateTime.MinValue;
            if (!DateTime.TryParse(date, out retval))
            {
                if (System.DateTime.TryParseExact(date, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out retval))
                    return retval.ToShortDateString();
            }
            // Could not convert date..                    
            return retval.ToShortDateString();

        }
        #endregion Helper Methods - Read request properties

        #region Dispose
        public void Dispose()
        {
            _MobiusDAL = null;
        }
        #endregion Dispose
    }
}
