
namespace Mobius.BAL
{
    #region Namespace
    using System;
    using System.Collections.Generic;
    using PatientDiscovery;
    using Mobius.Entity;
    using Mobius.CoreLibrary;
    #endregion

    public class AdapterComponentBuildMessage
    {
        #region Readonly from AdapterComponent specification
        private string _CommunityId = "";
        private string _ReciverCommunityId = "";
        private readonly string _HL7Constants_Sender_Determiner_Code = "INSTANCE";
        private readonly string _TimeFomat = "yyyyMMddHHmmss";
        private readonly string _InteractionIdExtension = "PRPA_IN201306UV02";
        private readonly string _ProcessingCodeCode = "T";
        private readonly string _ProcessingModeCodeCode = "I";
        private readonly string _AckCodeCode = "NE";
        private readonly string _TypeCodeCode = "AA";
        private readonly string _CodeCode = "PRPA_TE201306UV";
        private readonly string _AssignedDeviceClassCode = "ASSIGNED";
        private readonly string _RespCodeCodeNF = "NF";
        private readonly string _RespCodeCodeOK = "OK";
        private readonly string _RespCodeCodeQE = "QE";

        private readonly string _SubjectTypeCode = "SUBJ";
        private readonly string _RegEventMoodCode = "EVN";
        private readonly string _RegEventClassCode = "REG";
        private readonly string _IdNullFlavor = "NA";
        private readonly string _StatusCodeCode = "active";//createPatient
        private readonly string _ResultTypeCode = "CST";
        private readonly string _AssignedEntityClassCode = "ASSIGNED";
        private readonly string _CeCode = "NotHealthDataLocator";
        private readonly string _CeCodeSystem = "1.3.6.1.4.1.19376.1.2.27.2";
        private readonly string _SubjectPatientClassCode = "PAT";
        //string statusCodeCode = "active";
        private readonly string _PersonClassCode = "PSN";
        private readonly string _TEL_explicitUse = "WP";
        private readonly string _QueryMatchMoodCode = "EVN";
        private readonly string _QueryMatchClassCode = "CASE";
        private readonly string _QueryMatchcodeCode = "IHE_PDQ";
        private readonly string _IntValueValue = "100";
        private readonly string _OtherIdsClassCode = "SD";
        private readonly string _SSNRoot = "2.16.840.1.113883.4.1";
        private readonly string _ScopingOrgClassCode = "ORG";
        private readonly string _ScopingOrgDeterminerCode = "INSTANCE";
        private readonly string _OrgDeterminerCode = "INSTANCE";
        private readonly string _OrgClassCode = "ORG";
        private readonly string _SenderOrgClassCode = "DEV";
        private readonly string _AgentClassCode = "DEV";
        private readonly string _ReceiverDeviceDeterminerCode = "INSTANCE";
        private readonly string _ReceiverOrgClassCode = "DEV";
        private readonly string _ReceiverOrgDeterminerCode = "INSTANCE";

        #endregion

        #region internal Method
        /// <summary>
        /// This method will build the object based on the returned patient record.
        /// </summary>
        /// <param name="patients"></param>
        /// <param name="pRPA_IN201305UV02"></param>
        /// <returns></returns>
        internal PRPA_IN201306UV02 BuildMessageFromMpiPatient(List<Patient> patients, PatientDiscovery.PRPA_IN201305UV02 pRPA_IN201305UV02)
        {
            PRPA_IN201306UV02 PRPA_IN201306UV02 = new PRPA_IN201306UV02();

            // Set up the creation time string
            string timestamp = DateTime.UtcNow.ToString(_TimeFomat);

            // Set the receiver and sender
            PRPA_IN201306UV02.receiver = new MCCI_MT000300UV01Receiver[1] { CreateReceiver(pRPA_IN201305UV02.sender) };
            if (pRPA_IN201305UV02.receiver.Length > 0)
                PRPA_IN201306UV02.sender = CreateSender(pRPA_IN201305UV02.receiver[0]);

            II id = new II();
            id.root = this._CommunityId;
            id.extension = this.GenerateMessageId();
            PRPA_IN201306UV02.id = id;


            TS_explicit creationTime = new TS_explicit();
            creationTime.value = timestamp;
            PRPA_IN201306UV02.creationTime = creationTime;

            II interactionId = new II();
            interactionId.root = _CommunityId;// "2.16.840.1.113883.1.6";
            interactionId.extension = _InteractionIdExtension;
            PRPA_IN201306UV02.interactionId = interactionId;


            CS processingCode = new CS();
            processingCode.code = _ProcessingCodeCode;
            PRPA_IN201306UV02.processingCode = processingCode;

            CS processingModeCode = new CS();
            processingModeCode.code = _ProcessingModeCodeCode;
            PRPA_IN201306UV02.processingModeCode = processingModeCode;

            CS ackCode = new CS();
            ackCode.code = _AckCodeCode;
            PRPA_IN201306UV02.acceptAckCode = ackCode;

            PRPA_IN201306UV02.acknowledgement = new MCCI_MT000300UV01Acknowledgement[1] { this.CreateAck(pRPA_IN201305UV02) };

          
            PRPA_IN201306UV02.controlActProcess = CreateControlActProcess(patients, pRPA_IN201305UV02);           

            return PRPA_IN201306UV02;
        }

        /// <summary>
        /// This method specifically creates object to hanlde error conditions.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        internal PRPA_IN201306UV02 BuildMessageForErrorConditions(Result result, PatientDiscovery.PRPA_IN201305UV02 pRPA_IN201305UV02)
        {
            PRPA_IN201306UV02 PRPA_IN201306UV02 = new PRPA_IN201306UV02();
            PRPA_IN201306UV02 = BuildMessageFromMpiPatient(null, pRPA_IN201305UV02);
            

            MFMI_MT700711UV01Reason reason = new MFMI_MT700711UV01Reason();

            reason.detectedIssueEvent = new MCAI_MT900001UV01DetectedIssueEvent();
            reason.detectedIssueEvent.classCode = "ALRT";
            reason.detectedIssueEvent.moodCode = "EVN";
            reason.detectedIssueEvent.code = new CD();
            reason.detectedIssueEvent.code.code = "ActAdministrativeDetectedIssueCode";
            reason.detectedIssueEvent.code.codeSystem = "2.16.840.1.113883.5.4";
            List<MCAI_MT900001UV01SourceOf> MCAI_MT900001UV01SourceOfList = new List<PatientDiscovery.MCAI_MT900001UV01SourceOf>();
            
            //reason.detectedIssueEvent.triggerFor = new MCAI_MT900001UV01Requires[1];
            //reason.detectedIssueEvent.triggerFor[0] = new MCAI_MT900001UV01Requires();
            //reason.detectedIssueEvent.triggerFor[0].typeCode = "TRIG";
            //reason.detectedIssueEvent.triggerFor[0].actOrderRequired = new MCAI_MT900001UV01ActOrderRequired();
            //reason.detectedIssueEvent.triggerFor[0].actOrderRequired.classCode = "ACT";
            //reason.detectedIssueEvent.triggerFor[0].actOrderRequired.moodCode =  "RQO";
            //reason.detectedIssueEvent.triggerFor[0].actOrderRequired.code = new CE();

            MCAI_MT900001UV01SourceOf MCAI_MT900001UV01SourceOf = null;

            //Handling for requesting patient address to uniquely identify a patient.
            if (result.ErrorCode == ErrorCode.PatientAdressRequested)
            {
                 MCAI_MT900001UV01SourceOf = new PatientDiscovery.MCAI_MT900001UV01SourceOf();
                MCAI_MT900001UV01SourceOf.detectedIssueManagement = new MCAI_MT900001UV01DetectedIssueManagement();
                MCAI_MT900001UV01SourceOf.detectedIssueManagement.classCode = "ACT";
                MCAI_MT900001UV01SourceOf.detectedIssueManagement.moodCode = x_ActMoodDefEvn.EVN;
                MCAI_MT900001UV01SourceOf.detectedIssueManagement.code = new CD();         

                
                MCAI_MT900001UV01SourceOf.detectedIssueManagement.code.code = result.ErrorCode.ToString();
                MCAI_MT900001UV01SourceOf.detectedIssueManagement.code.codeSystem = "1.3.6.1.4.1.19376.1.2.27.1";
                MCAI_MT900001UV01SourceOf.detectedIssueManagement.code.displayName = result.ErrorMessage ;
                MCAI_MT900001UV01SourceOf.detectedIssueManagement.text = new ED_explicit();
                MCAI_MT900001UV01SourceOf.detectedIssueManagement.text.Text = new string[] { result.ErrorMessage };
                
                MCAI_MT900001UV01SourceOfList.Add(MCAI_MT900001UV01SourceOf);
            }
            //Handling for requesting patient telecom to uniquely identify a patient.
            else if (result.ErrorCode == ErrorCode.PatientTelecomRequested)
            {
                MCAI_MT900001UV01SourceOf = new PatientDiscovery.MCAI_MT900001UV01SourceOf();
                MCAI_MT900001UV01SourceOf.detectedIssueManagement = new MCAI_MT900001UV01DetectedIssueManagement();
                MCAI_MT900001UV01SourceOf.detectedIssueManagement.classCode = "ACT";
                MCAI_MT900001UV01SourceOf.detectedIssueManagement.moodCode = x_ActMoodDefEvn.EVN;
                MCAI_MT900001UV01SourceOf.detectedIssueManagement.code = new CD();

                MCAI_MT900001UV01SourceOf.detectedIssueManagement.code.code = result.ErrorCode.ToString();
                MCAI_MT900001UV01SourceOf.detectedIssueManagement.code.codeSystem = "1.3.6.1.4.1.19376.1.2.27.1";
                MCAI_MT900001UV01SourceOf.detectedIssueManagement.code.displayName = result.ErrorMessage;
                MCAI_MT900001UV01SourceOf.detectedIssueManagement.text = new ED_explicit();
                MCAI_MT900001UV01SourceOf.detectedIssueManagement.text.Text = new string[] { result.ErrorMessage };
                MCAI_MT900001UV01SourceOfList.Add(MCAI_MT900001UV01SourceOf);
            }


            reason.detectedIssueEvent.mitigatedBy = MCAI_MT900001UV01SourceOfList.ToArray();
            PRPA_IN201306UV02.controlActProcess.reasonOf = new MFMI_MT700711UV01Reason[] { reason };            
            return PRPA_IN201306UV02;
        }
        #endregion internal Method

        #region Helper Methods
        /// <summary>
        ///   Create MCCI_MT000300UV01Acknowledgement acknowledgement
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private MCCI_MT000300UV01Acknowledgement CreateAck(PRPA_IN201305UV02 query)
        {
            MCCI_MT000300UV01Acknowledgement ack = new MCCI_MT000300UV01Acknowledgement();
            ack.typeId = query.interactionId;
            CS typeCode = new CS();
            typeCode.code = _TypeCodeCode;
            ack.typeCode = typeCode;
            ack.targetMessage = new MCCI_MT000300UV01TargetMessage();
            II id = new II();
            id.root = this._CommunityId;
            id.extension = this.GenerateMessageId();
            ack.targetMessage.id = id;

            return ack;
        }

        /// <summary>
        /// create createControlActProcess
        /// </summary>
        /// <param name="patients"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        private PRPA_IN201306UV02MFMI_MT700711UV01ControlActProcess CreateControlActProcess(List<Patient> patients, PRPA_IN201305UV02 query)
        {
            PRPA_IN201306UV02MFMI_MT700711UV01ControlActProcess ControlActProcess = new PRPA_IN201306UV02MFMI_MT700711UV01ControlActProcess();

            ControlActProcess.moodCode = x_ActMoodIntentEvent.EVN;
            ControlActProcess.classCode = ActClassControlAct.CACT;


            CD code = new CD();
            code.code = _CodeCode;
            code.codeSystem = _CommunityId;//"2.16.840.1.113883.1.6";
            ControlActProcess.code = code;

            if (patients != null)
            {
                List<PRPA_IN201306UV02MFMI_MT700711UV01Subject1> subject1List = new List<PRPA_IN201306UV02MFMI_MT700711UV01Subject1>();
                foreach (Patient item in patients)
                {
                    subject1List.Add(this.CreateSubject(item, query));
                }

                ControlActProcess.subject = subject1List.ToArray();
                ControlActProcess.queryAck = CreateQueryAck(query, _RespCodeCodeOK);
            }
            else
            {
                ControlActProcess.queryAck = CreateQueryAck(query, _RespCodeCodeNF);
            }
            ControlActProcess.authorOrPerformer = this.CreateAuthorOrPerformer().ToArray();


            // Add in query parameters
            if (query.controlActProcess != null &&
            query.controlActProcess.queryByParameter != null)
            {
                ControlActProcess.queryByParameter = query.controlActProcess.queryByParameter;
            }



            // Set original QueryByParameter in response
            ControlActProcess.queryByParameter = query.controlActProcess.queryByParameter;

            return ControlActProcess;
        }

        /// <summary>
        /// Create CreateAuthorOrPerformer
        /// </summary>
        /// <returns></returns>
        private List<MFMI_MT700711UV01AuthorOrPerformer> CreateAuthorOrPerformer()
        {

            List<MFMI_MT700711UV01AuthorOrPerformer> authorOrPerformers = new List<MFMI_MT700711UV01AuthorOrPerformer>();
            MFMI_MT700711UV01AuthorOrPerformer authorOrPerformer = new MFMI_MT700711UV01AuthorOrPerformer();
            authorOrPerformer.typeCode = x_ParticipationAuthorPerformer.AUT;
            COCT_MT090300UV01AssignedDevice assignedDevice = new COCT_MT090300UV01AssignedDevice();
            assignedDevice.classCode = _AssignedDeviceClassCode;
            II id = new II();
            id.root = this._ReciverCommunityId;
            assignedDevice.id = new II[1] { id };
            authorOrPerformer.Item = assignedDevice;
            authorOrPerformers.Add(authorOrPerformer);
            return authorOrPerformers;
        }

        /// <summary>
        /// Create Query acknowledgement
        /// </summary>
        /// <param name="query"></param>
        /// <param name="Code">code</param>
        /// <returns></returns>
        /// The queryResponseCode element indicates at a high level the results of performing the query. 
        /// It may have the value 'OK' to indicate that the query was performed and has results.
        /// It may have the value 'NF' to indicate that the query was performed, but no results were located.
        /// Finally, it may have the value 'QE' to indicate that an error was detected in the incoming query message.
        private MFMI_MT700711UV01QueryAck CreateQueryAck(PRPA_IN201305UV02 query, string code)
        {
            MFMI_MT700711UV01QueryAck result = new MFMI_MT700711UV01QueryAck();

            if (query != null &&
                    query.controlActProcess != null &&
                    query.controlActProcess.queryByParameter != null &&
                    query.controlActProcess.queryByParameter.queryId != null)
            {
                result.queryId = query.controlActProcess.queryByParameter.queryId;
            }



            CS respCode = new CS();
            respCode.code = code; //OK  

            result.queryResponseCode = respCode;
            return result;
        }

        /// <summary>
        /// create Subject
        /// </summary>
        /// <param name="patient"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        private PRPA_IN201306UV02MFMI_MT700711UV01Subject1 CreateSubject(Patient patient, PRPA_IN201305UV02 query)
        {
            PRPA_IN201306UV02MFMI_MT700711UV01Subject1 subject = new PRPA_IN201306UV02MFMI_MT700711UV01Subject1();
            subject.typeCode = _SubjectTypeCode;
            subject.registrationEvent = this.CreateRegEvent(patient, query);
            return subject;
        }

        /// <summary>
        /// create Register Event
        /// </summary>
        /// <param name="patient"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        private PRPA_IN201306UV02MFMI_MT700711UV01RegistrationEvent CreateRegEvent(Patient patient, PRPA_IN201305UV02 query)
        {
            PRPA_IN201306UV02MFMI_MT700711UV01RegistrationEvent regEvent = new PRPA_IN201306UV02MFMI_MT700711UV01RegistrationEvent();
            regEvent.moodCode = _RegEventMoodCode;
            regEvent.classCode = _RegEventClassCode;
            II id = new II();
            id.nullFlavor = _IdNullFlavor;
            regEvent.id = new II[1] { id };
            CS statusCode = new CS();
            statusCode.code = _StatusCodeCode;
            regEvent.statusCode = statusCode;
            regEvent.subject1 = this.CreateSubject1(patient, query);
            regEvent.custodian = this.CreateCustodian(patient);
            return regEvent;

        }

        /// <summary>
        /// create Custodian
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        private MFMI_MT700711UV01Custodian CreateCustodian(Patient patient)
        {
            MFMI_MT700711UV01Custodian result = new MFMI_MT700711UV01Custodian();
            result.typeCode = _ResultTypeCode;
            result.assignedEntity = this.CreateAssignEntity();
            return result;
        }

        /// <summary>
        /// create Assign Entity
        /// </summary>
        /// <returns></returns>
        private COCT_MT090003UV01AssignedEntity CreateAssignEntity()
        {
            COCT_MT090003UV01AssignedEntity assignedEntity = new COCT_MT090003UV01AssignedEntity();
            assignedEntity.classCode = _AssignedEntityClassCode; //(HL7Constants.ASSIGNED_DEVICE_CLASS_CODE);
            II id = new II();
            id.root = this._CommunityId;
            assignedEntity.id = new II[1] { id };
            CE ce = new CE();
            ce.code = _CeCode;
            ce.codeSystem = _CeCodeSystem;
            assignedEntity.code = ce;
            return assignedEntity;
        }

        /// <summary>
        /// createSubject1
        /// </summary>
        /// <param name="patient"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        private PRPA_IN201306UV02MFMI_MT700711UV01Subject2 CreateSubject1(Patient patient, PRPA_IN201305UV02 query)
        {
            PRPA_IN201306UV02MFMI_MT700711UV01Subject2 subject = new PRPA_IN201306UV02MFMI_MT700711UV01Subject2();
            subject.typeCode = ParticipationTargetSubject.SBJ;
            // Add in patient
            subject.patient = this.CreatePatient(patient, query);
            return subject;
        }

        /// <summary>
        /// create Patient
        /// </summary>
        /// <param name="patient"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        private PRPA_MT201310UV02Patient CreatePatient(Patient patient, PRPA_IN201305UV02 query)
        {
            PRPA_MT201310UV02Patient subjectPatient = new PRPA_MT201310UV02Patient();
            subjectPatient.classCode = _SubjectPatientClassCode;
            CS statusCode = new CS();
            statusCode.code = _StatusCodeCode;
            subjectPatient.statusCode = statusCode;
            // Add in patient id 
            subjectPatient.id = this.CreateSubjectId(patient).ToArray();//new II[1] { this.CreateSubjectId(patient) };

            // Add in patient person
            subjectPatient.Item = CreatePatientPerson(patient, query);
            // Add in provider organization
            subjectPatient.providerOrganization = this.createProviderOrg(patient);
            // Add in query match observation
            subjectPatient.subjectOf1 = new PRPA_MT201310UV02Subject[1] { this.CreateSubjectOf1() };
            return subjectPatient;
        }

        /// <summary>
        /// create Patient Person
        /// </summary>
        /// <param name="patient"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        private PRPA_MT201310UV02Person CreatePatientPerson(Patient patient, PRPA_IN201305UV02 query)
        {
            PRPA_MT201310UV02Person person = new PRPA_MT201310UV02Person();
            // Set classCode
            person.classCode = _PersonClassCode;
            // Set the Subject Gender       
            person.administrativeGenderCode = this.CreateGender(patient.Gender);
            // Set the Subject Name        
            if (patient.GivenName.Count > 0)
            {
                List<PN_explicit> names = new List<PN_explicit>();
                for (var index = 0; index < patient.GivenName.Count; index++)
                {
                    names.Add(this.CreatePNExplicit(patient.GivenName[index], patient.MiddleName[index], patient.FamilyName[index], patient.Suffix[index], patient.Prefix[index]));
                }
                person.name = names.ToArray();
            }

            // Set the Birth Time            
            person.birthTime = this.CreateBirthTime(patient.DOB);
            person.birthPlace = this.CreateBirthPlace(patient);

            // Set the Deceased Time
            person.deceasedTime = this.CreateDeceasedTime(patient.DeceasedTime);
            // Set the Address
            person.addr = this.CreateAddress(patient).ToArray();
            // Set phone number
            person.telecom = this.CreateTELExplicit(patient).ToArray();
            // Set the SSN
            if (!string.IsNullOrEmpty(patient.SSN))
            {
                person.asOtherIDs = new PRPA_MT201310UV02OtherIDs[1] { this.CreateOtherIds(patient.SSN) };
            }

            return person;
        }



        /// <summary>
        /// create TELphone number Explicit
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        /// Refernce http://tools.ietf.org/html/rfc3966        
        private List<TEL_explicit> CreateTELExplicit(Patient patient)
        {
            List<TEL_explicit> telephoneNumbers = new List<TEL_explicit>();
            TEL_explicit TEL_explicit = null;
            if (patient.Telephone != null && patient.Telephone.Count > 0)
            {
                foreach (var item in patient.Telephone)
                {
                    TEL_explicit = new TEL_explicit();
                    /// RFC 3966 code for telephone standards we are adding "tel:" as prefix to number 
                    TEL_explicit.value = "TEL:" + item.Number;
                    telephoneNumbers.Add(TEL_explicit);
                }
            }
            else
            {
                TEL_explicit = new TEL_explicit();
                TEL_explicit.value = "";
                TEL_explicit.use = new string[1] { _TEL_explicitUse };
                telephoneNumbers.Add(TEL_explicit);
            }
            return telephoneNumbers;

        }

        private PRPA_MT201310UV02BirthPlace CreateBirthPlace(Patient patient)
        {
            PRPA_MT201310UV02BirthPlace BirthPlace = new PRPA_MT201310UV02BirthPlace();
            AD_explicit address = new AD_explicit();
            List<ADXP_explicit> bithAddress = new List<ADXP_explicit>();

            if (!string.IsNullOrEmpty(patient.BirthPlaceAddress))
            {
                adxp_explicitstreetAddressLine street = new adxp_explicitstreetAddressLine();
                street.Text = new string[1] { patient.BirthPlaceAddress };
                bithAddress.Add(street);
            }

            if (!string.IsNullOrEmpty(patient.BirthPlaceCity))
            {
                adxp_explicitcity city = new adxp_explicitcity();
                city.Text = new string[1] { patient.BirthPlaceCity };
                bithAddress.Add(city);

            }


            if (!string.IsNullOrEmpty(patient.BirthPlaceState))
            {
                adxp_explicitstate state = new adxp_explicitstate();
                state.Text = new string[1] { patient.BirthPlaceState };
                bithAddress.Add(state);

            }


            if (!string.IsNullOrEmpty(patient.BirthPlaceCountry))
            {
                adxp_explicitcountry country = new adxp_explicitcountry();
                country.Text = new string[1] { patient.BirthPlaceCountry };
                bithAddress.Add(country);

            }

            if (!string.IsNullOrEmpty(patient.BirthPlaceZip))
            {
                adxp_explicitpostalCode zip = new adxp_explicitpostalCode();
                zip.Text = new string[1] { patient.BirthPlaceZip };
                bithAddress.Add(zip);

            }

            address.Items = bithAddress.ToArray();
            BirthPlace.addr = address;
            //BirthPlace.addr.Items
            return BirthPlace;

        }

        /// <summary>
        /// create Address
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        private List<AD_explicit> CreateAddress(Patient patient)
        {
            List<AD_explicit> addresslist = new List<AD_explicit>();
            AD_explicit address = null;

            foreach (var item in patient.PatientAddress)
            {
                address = new AD_explicit();
                List<ADXP_explicit> ADXP_explicit = new List<PatientDiscovery.ADXP_explicit>();


                if (!string.IsNullOrEmpty(item.AddressLine1))
                {
                    adxp_explicitstreetAddressLine street = new adxp_explicitstreetAddressLine();
                    street.Text = new string[1] { item.AddressLine1 };
                    ADXP_explicit.Add(street);
                }

                if (!string.IsNullOrEmpty(item.AddressLine2))
                {
                    adxp_explicitstreetAddressLine street = new adxp_explicitstreetAddressLine();
                    street.Text = new string[1] { item.AddressLine2 };
                    ADXP_explicit.Add(street);
                }

                if (item.City != null && !string.IsNullOrEmpty(item.City.CityName))
                {
                    adxp_explicitcity city = new adxp_explicitcity();
                    city.Text = new string[1] { item.City.CityName };
                    ADXP_explicit.Add(city);

                    if (item.City.State != null && !string.IsNullOrEmpty(item.City.State.StateName))
                    {
                        adxp_explicitstate state = new adxp_explicitstate();
                        state.Text = new string[1] { item.City.State.StateName };
                        ADXP_explicit.Add(state);
                    }

                    if (item.City.State != null && item.City.State.Country != null && !string.IsNullOrEmpty(item.City.State.Country.CountryName))
                    {
                        adxp_explicitcountry country = new adxp_explicitcountry();
                        country.Text = new string[1] { item.City.State.Country.CountryName };
                        ADXP_explicit.Add(country);
                    }
                }

                if (!string.IsNullOrEmpty(item.Zip))
                {
                    adxp_explicitpostalCode zip = new adxp_explicitpostalCode();
                    zip.Text = new string[1] { item.Zip };
                    ADXP_explicit.Add(zip);
                }

                address.Items = ADXP_explicit.ToArray();
                addresslist.Add(address);
            }

            return addresslist;
        }


        /// <summary>
        /// create Other Ids like ssn
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private PRPA_MT201310UV02OtherIDs CreateOtherIds(string ssnValue)
        {
            PRPA_MT201310UV02OtherIDs otherIds = new PRPA_MT201310UV02OtherIDs();
            otherIds.classCode = _OtherIdsClassCode;
            // Set the SSN            
            II ssn = new II();
            ssn.extension = ssnValue;
            ssn.root = _SSNRoot;
            otherIds.id = new II[1] { ssn };

            COCT_MT150002UV01Organization scopingOrg = new COCT_MT150002UV01Organization();
            scopingOrg.classCode = _ScopingOrgClassCode;
            scopingOrg.determinerCode = _ScopingOrgDeterminerCode;
            II orgId = new II();
            orgId.root = ssn.root;
            scopingOrg.id = new II[1] { orgId };
            otherIds.scopingOrganization = scopingOrg;
            return otherIds;
        }

        /// <summary>
        /// createBirthTime
        /// </summary>
        /// <param name="dateOfbirth"></param>
        /// <returns></returns>
        private TS_explicit CreateBirthTime(string dateOfbirth)
        {

            TS_explicit bday = new TS_explicit();

            if (dateOfbirth != null &&
              dateOfbirth.Length > 0)
            {
                bday.value = dateOfbirth;
                ST text = new ST();
            }
            return bday;
        }


        /// <summary>
        /// CreateDeceasedTime
        /// </summary>
        /// <param name="dateOfbirth"></param>
        /// <returns></returns>
        private TS_explicit CreateDeceasedTime(string deceasedTime)
        {

            TS_explicit deceasedday = new TS_explicit();

            if (deceasedTime != null &&
              deceasedTime.Length > 0)
            {
                deceasedday.value = deceasedTime;
                ST text = new ST();
            }
            return deceasedday;
        }

        /// <summary>
        /// CreatePNExplicit
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        private PN_explicit CreatePNExplicit(string firstName, string middleName, string lastName, string suffixName, string prefixName)
        {

            List<ENXP_explicit> patientName = new List<PatientDiscovery.ENXP_explicit>();
            PN_explicit pnName = new PN_explicit();

            if (!string.IsNullOrEmpty(lastName))
            {
                en_explicitfamily familyName = new en_explicitfamily();
                familyName.Text = new string[1] { lastName };
                patientName.Add(familyName);
            }

            if (!string.IsNullOrEmpty(firstName))
            {
                en_explicitgiven givenName = new en_explicitgiven();
                givenName.Text = new string[1] { firstName };
                patientName.Add(givenName);
            }

            if (!string.IsNullOrEmpty(middleName))
            {
                en_explicitgiven middle = new en_explicitgiven();
                middle.Text = new string[1] { middleName };
                patientName.Add(middle);
            }

            if (!string.IsNullOrEmpty(suffixName))
            {
                en_explicitsuffix suffix = new en_explicitsuffix();
                suffix.Text = new string[1] { suffixName };
                patientName.Add(suffix);
            }

            if (!string.IsNullOrEmpty(suffixName))
            {
                en_explicitprefix prefix = new en_explicitprefix();
                prefix.Text = new string[1] { prefixName };
                patientName.Add(prefix);
            }

            pnName.Items = patientName.ToArray();


            return pnName;
        }


        /// <summary>
        /// createGender
        /// </summary>
        /// <param name="gender"></param>
        /// <returns></returns>
        private CE CreateGender(Gender gender)
        {
            CE genderCode = new CE();
            switch (gender)
            {
                case Gender.Male:
                    genderCode.code = "M";
                    break;
                case Gender.Female:
                    genderCode.code = "F";
                    break;
                case Gender.Unspecified:
                    genderCode.code = "UN";
                    break;

            }

            return genderCode;
        }

        /// <summary>
        /// create SubjectOf1
        /// </summary>
        /// <returns></returns>
        private PRPA_MT201310UV02Subject CreateSubjectOf1()
        {
            PRPA_MT201310UV02Subject subject = new PRPA_MT201310UV02Subject();
            subject.queryMatchObservation = this.CreateQueryMatch();
            return subject;
        }

        /// <summary>
        /// create Query Match
        /// </summary>
        /// <returns></returns>
        private PRPA_MT201310UV02QueryMatchObservation CreateQueryMatch()
        {
            PRPA_MT201310UV02QueryMatchObservation queryMatch = new PRPA_MT201310UV02QueryMatchObservation();
            queryMatch.moodCode = _QueryMatchMoodCode;
            queryMatch.classCode = _QueryMatchClassCode;
            CD code = new CD();
            code.code = _QueryMatchcodeCode;
            queryMatch.code = code;
            INT intValue = new INT();
            intValue.value = _IntValueValue;
            queryMatch.value = intValue;
            return queryMatch;
        }

        /// <summary>
        /// create Provider Organization 
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        private COCT_MT150003UV03Organization createProviderOrg(Patient patient)
        {
            COCT_MT150003UV03Organization org = new COCT_MT150003UV03Organization();
            org.determinerCode = _OrgDeterminerCode;
            org.classCode = _OrgClassCode;
            II id = new II();
            id.root = patient.CommunityId;
            org.id = new II[1] { id };
            org.contactParty = new COCT_MT150003UV03ContactParty[1];
            return org;
        }

        /// <summary>
        /// createSubjectId
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        private List<II> CreateSubjectId(Patient patient)
        {
            List<II> IDs = new List<II>();
            II id = new II();
            id.root = patient.CommunityId;
            id.extension = patient.LocalMPIID;
            IDs.Add(id);
            return IDs;
        }



        /// <summary>
        /// create Sender
        /// </summary>
        /// <param name="queryReceiver"></param>
        /// <returns></returns>
        private MCCI_MT000300UV01Sender CreateSender(MCCI_MT000100UV01Receiver queryReceiver)
        {
            MCCI_MT000300UV01Sender sender = new MCCI_MT000300UV01Sender();
            string app = null;
            string oid = null;

            sender.typeCode = CommunicationFunctionType.SND;

            if (queryReceiver.device != null &&
              queryReceiver.device.asAgent != null &&
              queryReceiver.device.asAgent.representedOrganization != null &&
              queryReceiver.device.asAgent.representedOrganization.typeId != null &&
              !string.IsNullOrEmpty(queryReceiver.device.asAgent.representedOrganization.typeId.root))
            {

                this._CommunityId = oid = queryReceiver.device.asAgent.representedOrganization.typeId.root;

            }
            if (queryReceiver.device != null &&
                   queryReceiver.device.id != null &&
                   queryReceiver.device.id.Length > 0)
            {
                app = queryReceiver.device.id[0].root;
            }
            else
            {
                app = this._CommunityId;
            }

            MCCI_MT000300UV01Device senderDevice = new MCCI_MT000300UV01Device();
            senderDevice.determinerCode = _HL7Constants_Sender_Determiner_Code; //"HL7Constants.SENDER_DETERMINER_CODE";
            senderDevice.classCode = EntityClassDevice.DEV;
            senderDevice.id = new II[1] { this.IIFactory(app) };
            MCCI_MT000300UV01Agent agent = new MCCI_MT000300UV01Agent();
            MCCI_MT000300UV01Organization org = new MCCI_MT000300UV01Organization();
            org.classCode = _SenderOrgClassCode;// "HL7Constants.ORG_CLASS_CODE";
            org.determinerCode = _HL7Constants_Sender_Determiner_Code;// "HL7Constants.SENDER_DETERMINER_CODE";
            org.id = new II[1] { this.IIFactory(oid) };
            agent.representedOrganization = org;
            agent.classCode = _AgentClassCode;//"HL7Constants.AGENT_CLASS_CODE";
            senderDevice.asAgent = agent;
            sender.device = senderDevice;
            return sender;
        }


        /// <summary>
        /// create Receiver         
        /// </summary>
        /// <param name="querySender"></param>
        /// <returns></returns>
        private MCCI_MT000300UV01Receiver CreateReceiver(MCCI_MT000100UV01Sender querySender)
        {
            MCCI_MT000300UV01Receiver receiver = new MCCI_MT000300UV01Receiver();
            string app = null;
            string oid = null;

            receiver.typeCode = CommunicationFunctionType.RCV;

            if (querySender.device != null && querySender.device.id != null &&
                querySender.device.id.Length > 0)
            {

                app = querySender.device.id[0].root;
            }
            else
            {
                app = this._CommunityId;
            }

            if (querySender.device != null && querySender.device.asAgent != null &&
                querySender.device.asAgent.representedOrganization != null &&
                querySender.device.asAgent.representedOrganization.typeId != null
                && !string.IsNullOrEmpty(querySender.device.asAgent.representedOrganization.typeId.root))
            {
                oid = querySender.device.asAgent.representedOrganization.typeId.root;
            }

            this._ReciverCommunityId = oid;
            MCCI_MT000300UV01Device receiverDevice = new MCCI_MT000300UV01Device();
            receiverDevice.determinerCode = _ReceiverDeviceDeterminerCode;
            receiverDevice.classCode = EntityClassDevice.DEV;
            receiverDevice.id = new II[1] { this.IIFactory(app) };


            MCCI_MT000300UV01Agent agent = new MCCI_MT000300UV01Agent();
            MCCI_MT000300UV01Organization org = new MCCI_MT000300UV01Organization();


            org.classCode = _ReceiverOrgClassCode; //"HL7Constants.ORG_CLASS_CODE";
            org.determinerCode = _ReceiverOrgDeterminerCode;// "HL7Constants.RECEIVER_DETERMINER_CODE"; //HL7Constants.RECEIVER_DETERMINER_CODE";        
            org.id = new II[1] { this.IIFactory(oid) };

            agent.representedOrganization = org;
            agent.classCode = _AgentClassCode;// "HL7Constants.AGENT_CLASS_CODE";
            receiverDevice.asAgent = agent;
            receiver.device = receiverDevice;
            return receiver;
        }

        /// <summary>
        /// HL7DataTransformHelper 
        /// </summary>
        /// <param name="localDeviceId"></param>
        /// <param name="patientId"></param>
        /// <returns></returns>
        private II IIFactory(string root)
        {
            return this.IIFactory(root, null, null);
        }

        /// <summary>
        /// IIFactory
        /// </summary>
        /// <param name="root"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        private II IIFactory(string root, string extension)
        {
            return this.IIFactory(root, extension, null);
        }

        /// <summary>
        /// IIFactory
        /// </summary>
        /// <param name="root"></param>
        /// <param name="extension"></param>
        /// <param name="assigningAuthorityName"></param>
        /// <returns></returns>
        private II IIFactory(string root, string extension, string assigningAuthorityName)
        {
            II ii = new II();
            if (!string.IsNullOrEmpty(root))
            {
                ii.root = root;
            }
            if (!string.IsNullOrEmpty(extension))
            {

                ii.extension = extension;
            }
            if (!string.IsNullOrEmpty(assigningAuthorityName))
            {
                ii.assigningAuthorityName = assigningAuthorityName;
            }
            return ii;
        }


        /// <summary>
        /// TODO need to check
        /// </summary>
        /// <returns></returns>
        private string GenerateMessageId()
        {
            Random _r = new Random();

            //TODO Need to change message id after discussion 
            return _r.Next().ToString();
        }

        #endregion Helper Methods


    }
}
