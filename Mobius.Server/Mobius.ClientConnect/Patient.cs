

namespace Mobius.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Mobius.Client.Interface;
    using Mobius.CoreLibrary;
    using Mobius.Entity;
    using MobiusCorrelation;
    using PatientDiscovery;

    public partial class MobiusConnect : IMobiusConnect
    {





        #region Patient Discovery

        private Result _result;

        private string HL7PatientId
        {
            get;
            set;
        }

        public string PatientAssigningAuthorityId
        {
            get { return MobiusAppSettingReader.LocalHomeCommunityID; }
        }

        public Result Result
        {
            get
            {
                return _result != null ? _result : _result = new Result();
            }
            set { _result = value; }
        }


        public Result SearchPatient(Demographics demographics, List<MobiusNHINCommunity> NHINCommunities, PatientDiscovery.AssertionType assertionType, out List<Patient> patients)
        {
            patients = new List<Patient>();
            try
            {
                //
                PatientDiscovery.NhinTargetCommunityType[] NhinTargetCommunities = null;
                PatientDiscovery.NhinTargetCommunityType NhinTargetCommunity = null;
                PatientDiscovery.HomeCommunityType homeCommunity = null;
                int index = 0;

                this.Result.IsSuccess = true;

                IEnumerable<MobiusNHINCommunity> NHINCommunitiesHomeFilter = NHINCommunities.Where(t => t.IsHomeCommunity == true);

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
                prpa.id.root = this.PatientAssigningAuthorityId;
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


                // if local patient id is not provided , then pass remotehomecommunity id in the reciever code.
                if (NHINCommunitiesHomeFilter.Count() > 0)
                    prpa.receiver[0].device.asAgent.representedOrganization.id[0].root = NHINCommunitiesHomeFilter.FirstOrDefault().CommunityIdentifier;
                //String.IsNullOrEmpty(demographics.LocalMPIID) ? "2.16.840.1.113883.3.1605" : "2.16.840.1.113883.3.348.2.1";

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
                prpa.sender.device.asAgent.representedOrganization.id[0].root = this.PatientAssigningAuthorityId;
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
                assignedDevice.id[0].root = this.PatientAssigningAuthorityId;
                prpa.controlActProcess.authorOrPerformer[0].Item = assignedDevice;

                // controlActProcess/queryByParameter element
                prpa.controlActProcess.queryByParameter = new PRPA_MT201306UV02QueryByParameter();
                prpa.controlActProcess.queryByParameter.queryId = new II();
                prpa.controlActProcess.queryByParameter.queryId.root = this.PatientAssigningAuthorityId;
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
                switch (demographics.Gender)
                {
                    case Gender.Male:
                        prpa.controlActProcess.queryByParameter.parameterList.livingSubjectAdministrativeGender[0].value[0].code = "M";
                        break;
                    case Gender.Female:
                        prpa.controlActProcess.queryByParameter.parameterList.livingSubjectAdministrativeGender[0].value[0].code = "F";
                        break;
                    case Gender.Unspecified:
                        prpa.controlActProcess.queryByParameter.parameterList.livingSubjectAdministrativeGender[0].value[0].code = "UN";
                        break;
                    default:
                        break;
                }


                prpa.controlActProcess.queryByParameter.parameterList.livingSubjectAdministrativeGender[0].semanticsText = new ST();

                prpa.controlActProcess.queryByParameter.parameterList.livingSubjectAdministrativeGender[0].semanticsText.representation = BinaryDataEncoding.TXT;
                prpa.controlActProcess.queryByParameter.parameterList.livingSubjectAdministrativeGender[0].semanticsText.Text = new string[] { "LivingSubject.administrativeGender" };

                prpa.controlActProcess.queryByParameter.parameterList.livingSubjectBirthTime = new PRPA_MT201306UV02LivingSubjectBirthTime[1] { new PRPA_MT201306UV02LivingSubjectBirthTime() };
                prpa.controlActProcess.queryByParameter.parameterList.livingSubjectBirthTime[0].value = new IVL_TS_explicit[1] { new IVL_TS_explicit() };
                prpa.controlActProcess.queryByParameter.parameterList.livingSubjectBirthTime[0].value[0].value = dateFormatter(demographics.DOB);
                prpa.controlActProcess.queryByParameter.parameterList.livingSubjectBirthTime[0].semanticsText = new ST();
                prpa.controlActProcess.queryByParameter.parameterList.livingSubjectBirthTime[0].semanticsText.representation = BinaryDataEncoding.TXT;
                prpa.controlActProcess.queryByParameter.parameterList.livingSubjectBirthTime[0].semanticsText.Text = new string[] { "LivingSubject.birthTime" };

                prpa.controlActProcess.queryByParameter.parameterList.livingSubjectName = new PRPA_MT201306UV02LivingSubjectName[1] { new PRPA_MT201306UV02LivingSubjectName() };
                prpa.controlActProcess.queryByParameter.parameterList.livingSubjectName[0].value = new EN_explicit[1] { new EN_explicit() };

                List<ENXP_explicit> ENXP_explicit = new List<PatientDiscovery.ENXP_explicit>();

                //Add Family Name
                ENXP_explicit.Add(this.addFamilyName(new string[] { demographics.FamilyName }));
                //Add GivenName              
                ENXP_explicit.Add(this.addGivenName(new string[] { demographics.GivenName }));
                //Add Middle Name
                if (!string.IsNullOrEmpty(demographics.MiddleName))
                {
                    ENXP_explicit.Add(this.addMiddleName(new string[] { demographics.MiddleName }));
                }

                //Add Prifix
                if (!string.IsNullOrEmpty(demographics.Prefix))
                {
                    ENXP_explicit.Add(this.addPrefixName(new string[] { demographics.Prefix }));
                }
                //Add Suffix
                if (!string.IsNullOrEmpty(demographics.Suffix))
                {
                    ENXP_explicit.Add(this.addSuffixName(new string[] { demographics.Suffix }));
                }
                prpa.controlActProcess.queryByParameter.parameterList.livingSubjectName[0].value[0].Items = ENXP_explicit.ToArray();
                prpa.controlActProcess.queryByParameter.parameterList.livingSubjectId = new PRPA_MT201306UV02LivingSubjectId[1] { new PRPA_MT201306UV02LivingSubjectId() };


                List<II> IIList = new List<II>();
                II II = new II();
                II.root = this.PatientAssigningAuthorityId;
                II.extension = demographics.LocalMPIID;
                IIList.Add(II);

                //this.HL7PatientId = this.SAMLAssertionHelper.GetHL7EncodePatientId(patientId, documentSubmissionComunityId)
                //Add SSN
                if (!string.IsNullOrEmpty(demographics.SSN))
                {
                    II = new II();
                    II.root = "2.16.840.1.113883.4.1";
                    II.extension = demographics.SSN;
                    IIList.Add(II);
                }

                prpa.controlActProcess.queryByParameter.parameterList.livingSubjectId[0].value = IIList.ToArray();

                //creating Telephone Number Mapping                
                if (demographics.Telephones != null && demographics.Telephones.Count > 0)
                {
                    PRPA_MT201306UV02PatientTelecom PatientTelecom = new PRPA_MT201306UV02PatientTelecom();
                    List<TEL_explicit> telphoneNumbers = new List<TEL_explicit>();
                    TEL_explicit telphoneNumber = null;
                    foreach (var item in demographics.Telephones)
                    {
                        if (!string.IsNullOrEmpty(item.Number))
                        {
                            telphoneNumber = new TEL_explicit();
                            telphoneNumber.value = "tel:" + item.Number;
                            telphoneNumbers.Add(telphoneNumber);
                            PatientTelecom.value = telphoneNumbers.ToArray();
                        }
                    }

                    //Add Telephone object to request
                    prpa.controlActProcess.queryByParameter.parameterList.patientTelecom = new PRPA_MT201306UV02PatientTelecom[1] { PatientTelecom };

                }

                //mothersMaidenName
                if (demographics.MothersMaidenName != null)
                {
                    List<PRPA_MT201306UV02MothersMaidenName> MothersMaidenNames = new List<PRPA_MT201306UV02MothersMaidenName>();
                    PRPA_MT201306UV02MothersMaidenName MothersMaidenName = new PRPA_MT201306UV02MothersMaidenName();

                    ENXP_explicit = new List<PatientDiscovery.ENXP_explicit>();
                    //Add Family Name
                    ENXP_explicit.Add(this.addFamilyName(new string[] { demographics.MothersMaidenName.FamilyName }));
                    //Add GivenName                 
                    ENXP_explicit.Add(this.addGivenName(new string[] { demographics.MothersMaidenName.GivenName }));
                    //Add Middle Name
                    if (!string.IsNullOrEmpty(demographics.MothersMaidenName.MiddleName))
                    {
                        ENXP_explicit.Add(this.addMiddleName(new string[] { demographics.MiddleName }));
                    }

                    //Add Prifix
                    if (!string.IsNullOrEmpty(demographics.MothersMaidenName.Prefix))
                    {
                        ENXP_explicit.Add(this.addPrefixName(new string[] { demographics.Prefix }));
                    }
                    //Add Suffix
                    if (!string.IsNullOrEmpty(demographics.MothersMaidenName.Suffix))
                    {
                        ENXP_explicit.Add(this.addSuffixName(new string[] { demographics.Suffix }));
                    }

                    PN_explicit PN_explicit = new PatientDiscovery.PN_explicit();
                    PN_explicit.Items = ENXP_explicit.ToArray();

                    MothersMaidenName.value = new PN_explicit[] { PN_explicit };

                    MothersMaidenNames.Add(MothersMaidenName);
                    prpa.controlActProcess.queryByParameter.parameterList.mothersMaidenName = MothersMaidenNames.ToArray();
                }


                //Creating address
                if (demographics.Addresses != null && demographics.Addresses.Count > 0)
                {
                    List<PRPA_MT201306UV02PatientAddress> Addresses = new List<PRPA_MT201306UV02PatientAddress>();
                    PRPA_MT201306UV02PatientAddress patientAddress = null;
                    foreach (var item in demographics.Addresses)
                    {
                        patientAddress = new PRPA_MT201306UV02PatientAddress();
                        patientAddress.value = new AD_explicit[1];
                        patientAddress.value[0] = new AD_explicit();
                        patientAddress.value[0].Items = addAdddress(item).ToArray();
                        Addresses.Add(patientAddress);
                    }
                    prpa.controlActProcess.queryByParameter.parameterList.patientAddress = Addresses.ToArray();
                }

                //Creating birth address
                if (demographics.BirthPlaceAddress != null && demographics.BirthPlaceAddress.Count > 0)
                {
                    List<PRPA_MT201306UV02LivingSubjectBirthPlaceAddress> birthPlaceAddresses = new List<PRPA_MT201306UV02LivingSubjectBirthPlaceAddress>();
                    PRPA_MT201306UV02LivingSubjectBirthPlaceAddress birthAddress = null;
                    foreach (var item in demographics.BirthPlaceAddress)
                    {
                        birthAddress = new PRPA_MT201306UV02LivingSubjectBirthPlaceAddress();
                        birthAddress.value = new AD_explicit[1];
                        birthAddress.value[0] = new AD_explicit();
                        birthAddress.value[0].Items = addAdddress(item).ToArray();
                        birthPlaceAddresses.Add(birthAddress);
                    }
                    prpa.controlActProcess.queryByParameter.parameterList.livingSubjectBirthPlaceAddress = birthPlaceAddresses.ToArray();
                }

                prpa.controlActProcess.queryByParameter.parameterList.livingSubjectId[0].semanticsText = new ST();
                prpa.controlActProcess.queryByParameter.parameterList.livingSubjectId[0].semanticsText.representation = BinaryDataEncoding.TXT;

                // NHIN target communtity
                //Add NHIN target community collection in NhinTargetCommunityType array                  
                NhinTargetCommunities = new PatientDiscovery.NhinTargetCommunityType[NHINCommunities.Count];
                foreach (MobiusNHINCommunity NHINCommunity in NHINCommunities)
                {
                    //create object of NhinTargetCommunities at index
                    NhinTargetCommunities[index] = new PatientDiscovery.NhinTargetCommunityType();

                    NhinTargetCommunity = new PatientDiscovery.NhinTargetCommunityType();
                    homeCommunity = new PatientDiscovery.HomeCommunityType();
                    homeCommunity.homeCommunityId = NHINCommunity.CommunityIdentifier;
                    homeCommunity.description = NHINCommunity.CommunityDescription;
                    homeCommunity.name = NHINCommunity.CommunityName;
                    NhinTargetCommunity.homeCommunity = homeCommunity;

                    // object of NhinTargetCommunity at index
                    NhinTargetCommunities[index] = NhinTargetCommunity;

                    //increase index
                    index++;
                }

                //DeceasedTime
                if (!string.IsNullOrEmpty(demographics.DeceasedTime))
                {
                    PRPA_MT201306UV02LivingSubjectDeceasedTime deceasedTime = new PRPA_MT201306UV02LivingSubjectDeceasedTime();
                    IVL_TS_explicit deceased_IVL_TS_explicit = new IVL_TS_explicit();
                    deceased_IVL_TS_explicit.value = dateFormatter(demographics.DeceasedTime);
                    deceasedTime.value = new IVL_TS_explicit[] { deceased_IVL_TS_explicit };

                    deceasedTime.semanticsText = new ST();
                    deceasedTime.semanticsText.representation = BinaryDataEncoding.TXT;
                    deceasedTime.semanticsText.Text = new string[] { "LivingSubject.deceasedTime" };
                    prpa.controlActProcess.queryByParameter.parameterList.livingSubjectDeceasedTime = new PRPA_MT201306UV02LivingSubjectDeceasedTime[] { deceasedTime };

                }
                reqType.PRPA_IN201305UV02 = prpa;
                //assgin the NhinTargetCommunities to request
                reqType.NhinTargetCommunities = NhinTargetCommunities;
                reqType.assertion = assertionType;

                Community_PRPA_IN201306UV02ResponseType[] response = patientDiscovery.RespondingGateway_PRPA_IN201305UV02(reqType);

                if (response == null || response.Length == 0)
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.SearchPatient_GateWayResponse_Error);
                    return this.Result;
                }

                // Parse the Response from GateWay
                patients = ParseRespondingGateway_PRPA_IN201305UV02(response);
                if (patients.Count != 0)
                {
                    foreach (var item in patients)
                    {
                        item.LocalMPIID = demographics.LocalMPIID;
                    }

                }
                else
                {
                    //Check Time error code
                    if (response[0].PRPA_IN201306UV02.controlActProcess != null)
                    {
                        if (response[0].PRPA_IN201306UV02.controlActProcess.reasonOf != null
                            && response[0].PRPA_IN201306UV02.controlActProcess.reasonOf.Length > 0
                            && response[0].PRPA_IN201306UV02.controlActProcess.reasonOf[0].detectedIssueEvent != null)
                        {
                            if (response[0].PRPA_IN201306UV02.controlActProcess.reasonOf[0].detectedIssueEvent.mitigatedBy != null
                            && response[0].PRPA_IN201306UV02.controlActProcess.reasonOf[0].detectedIssueEvent.mitigatedBy.Length > 0
                            && response[0].PRPA_IN201306UV02.controlActProcess.reasonOf[0].detectedIssueEvent.mitigatedBy[0].detectedIssueManagement != null
                            && response[0].PRPA_IN201306UV02.controlActProcess.reasonOf[0].detectedIssueEvent.mitigatedBy[0].detectedIssueManagement.text != null
                            && response[0].PRPA_IN201306UV02.controlActProcess.reasonOf[0].detectedIssueEvent.mitigatedBy[0].detectedIssueManagement.text.Text != null
                            && response[0].PRPA_IN201306UV02.controlActProcess.reasonOf[0].detectedIssueEvent.mitigatedBy[0].detectedIssueManagement.text.Text.Length > 0)
                            {

                                this.Result.IsSuccess = false;
                                this.Result.SetError(ErrorCode.SearchPatient_GateWayResponse_Error, string.Join(" ", response[0].PRPA_IN201306UV02.controlActProcess.reasonOf[0].detectedIssueEvent.mitigatedBy[0].detectedIssueManagement.text.Text));
                                return this.Result;
                            }
                            else if (response[0].PRPA_IN201306UV02.controlActProcess.reasonOf[0].detectedIssueEvent.code != null
                                && !string.IsNullOrEmpty(response[0].PRPA_IN201306UV02.controlActProcess.reasonOf[0].detectedIssueEvent.code.displayName))
                            {
                                this.Result.IsSuccess = false;
                                this.Result.SetError(ErrorCode.SearchPatient_GateWayResponse_Error, string.Join(" ", response[0].PRPA_IN201306UV02.controlActProcess.reasonOf[0].detectedIssueEvent.code.displayName));
                                return this.Result;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return this.Result;
        }



        #region Helper






        private List<ADXP_explicit> addAdddress(Address item)
        {
            List<ADXP_explicit> ADXP_explicit = new List<ADXP_explicit>();
            if (!string.IsNullOrEmpty(item.AddressLine1) || !string.IsNullOrEmpty(item.AddressLine2))
            {
                adxp_explicitstreetAddressLine streetAddressLine = new adxp_explicitstreetAddressLine();
                streetAddressLine = new adxp_explicitstreetAddressLine();
                streetAddressLine.Text = new string[] { ((string.IsNullOrEmpty(item.AddressLine1) ?"" :item.AddressLine1 + " ") 
                                        + (string.IsNullOrEmpty(item.AddressLine2) ?"" : item.AddressLine2))};
                streetAddressLine.partType = "SAL";
                ADXP_explicit.Add(streetAddressLine);
            }

            if (!string.IsNullOrEmpty(item.Zip))
            {
                adxp_explicitpostalCode postalCode = new adxp_explicitpostalCode();
                postalCode.partType = "ZIP";
                postalCode.Text = new string[] { item.Zip };
                ADXP_explicit.Add(postalCode);
            }

            if (item.City != null && !string.IsNullOrEmpty(item.City.CityName))
            {
                adxp_explicitcity city = new adxp_explicitcity();
                city.partType = "CTY";
                city.Text = new string[] { item.City.CityName };
                ADXP_explicit.Add(city);
            }

            if (item.City != null && item.City.State != null && !string.IsNullOrEmpty(item.City.State.StateName))
            {
                adxp_explicitstate state = new adxp_explicitstate();
                state.partType = "STA";
                state.Text = new string[] { item.City.State.StateName };
                ADXP_explicit.Add(state);
            }

            if (item.City != null && item.City.State != null && !string.IsNullOrEmpty(item.City.State.Country.CountryName))
            {
                adxp_explicitcountry country = new adxp_explicitcountry();
                country.partType = "CNT";
                country.Text = new string[] { item.City.State.Country.CountryName };
                ADXP_explicit.Add(country);
            }

            return ADXP_explicit;
        }

        private en_explicitfamily addFamilyName(string[] familyName)
        {
            en_explicitfamily family = new en_explicitfamily();
            family.partType = "FAM";
            family.Text = new string[1];
            family.Text = familyName;
            return family;
        }

        private en_explicitprefix addPrefixName(string[] prefixName)
        {
            en_explicitprefix prefix = new en_explicitprefix();
            prefix.partType = "PFX";
            prefix.Text = new string[1];
            prefix.Text = prefixName;
            return prefix;
        }

        private en_explicitsuffix addSuffixName(string[] suffixName)
        {
            en_explicitsuffix suffix = new en_explicitsuffix();
            suffix.partType = "SFX";
            suffix.Text = new string[1];
            suffix.Text = suffixName;
            return suffix;
        }

        private en_explicitgiven addMiddleName(string[] middleName)
        {
            en_explicitgiven middle = new en_explicitgiven();
            middle.partType = "MID";
            middle.Text = new string[1];
            middle.Text = middleName;
            return middle;
        }

        private en_explicitgiven addGivenName(string[] givenName)
        {
            en_explicitgiven given = null;
            given = new en_explicitgiven();
            given.partType = "GIV";
            given.Text = new string[1];
            given.Text = givenName;
            return given;
        }

        private List<Patient> ParseRespondingGateway_PRPA_IN201305UV02(Community_PRPA_IN201306UV02ResponseType[] gateWayResponse)
        {

            Patient patient = null;
            List<Patient> patients = new List<Patient>();

            foreach (Community_PRPA_IN201306UV02ResponseType item in gateWayResponse)
            {

                if (item != null && item.PRPA_IN201306UV02 != null && item.PRPA_IN201306UV02.controlActProcess != null && item.PRPA_IN201306UV02.controlActProcess.subject != null)
                {

                    if (item.PRPA_IN201306UV02.controlActProcess.subject[0].registrationEvent != null && item.PRPA_IN201306UV02.controlActProcess.subject[0].registrationEvent.subject1 != null && item.PRPA_IN201306UV02.controlActProcess.subject[0].registrationEvent.subject1.patient.id[0] != null)
                    {
                        patient = new Patient();

                        // Following object returns RemoteMPIID, so Shouldn't be assigned to LocalMPIID field.                       
                        if (item.PRPA_IN201306UV02.controlActProcess.subject[0].registrationEvent.subject1.patient.id[0].root == MobiusAppSettingReader.LocalHomeCommunityID)
                        {
                            patient.RemoteMPIID = item.PRPA_IN201306UV02.controlActProcess.subject[0].registrationEvent.subject1.patient.id[0].extension;
                            patient.CommunityId = item.PRPA_IN201306UV02.controlActProcess.subject[0].registrationEvent.subject1.patient.id[0].root;
                        }
                        else
                        {
                            patient.CommunityId = item.PRPA_IN201306UV02.controlActProcess.subject[0].registrationEvent.subject1.patient.id[0].root;
                            patient.RemoteMPIID = item.PRPA_IN201306UV02.controlActProcess.subject[0].registrationEvent.subject1.patient.id[0].extension;
                        }

                        PRPA_MT201310UV02Person patientPerson = item.PRPA_IN201306UV02.controlActProcess.subject[0].registrationEvent.subject1.patient.Item as PRPA_MT201310UV02Person;
                        bool IsMiddleName = false;
                        foreach (ENXP_explicit name in patientPerson.name[0].Items)
                        {
                            //if (ENXP_explicit.partType.Equals("FAM"))
                            //    patient.FamilyName.Add(ENXP_explicit.Text[0]);
                            //if (ENXP_explicit.partType.Equals("GIV"))
                            //    patient.GivenName.Add(ENXP_explicit.Text[0]);

                            //Family Name
                            if (name.Text != null && name.Text.Length > 0)
                            {
                                if (name is en_explicitfamily)
                                {
                                    patient.FamilyName.AddRange(name.Text);
                                    continue;
                                }
                                //Middle Name
                                else if (name is en_explicitgiven && IsMiddleName)
                                {
                                    patient.MiddleName.AddRange(name.Text);
                                    continue;
                                }
                                //Given Name
                                else if (name is en_explicitgiven)
                                {
                                    patient.GivenName.AddRange(name.Text);
                                    IsMiddleName = true;
                                    continue;
                                }

                                  //Given Name
                                else if (name is en_explicitsuffix)
                                {
                                    patient.Suffix.AddRange(name.Text);
                                    continue;
                                }
                                //Given Name
                                else if (name is en_explicitprefix)
                                {
                                    patient.Prefix.AddRange(name.Text);
                                    continue;
                                }
                            }
                        }
                        if (!String.IsNullOrEmpty(patientPerson.administrativeGenderCode.code))
                        {
                            //patient.Gender =(Gender)Enum.Parse(typeof(Gender), patientPerson.administrativeGenderCode.code,true);
                            switch (patientPerson.administrativeGenderCode.code.ToString().ToUpper())
                            {
                                case "M":
                                case "MALE":
                                    patient.Gender = Gender.Male;
                                    break;
                                case "F":
                                case "FEMALE":
                                    patient.Gender = Gender.Female;
                                    break;
                                case "UN":
                                case "UNSPECIFIED":
                                    patient.Gender = Gender.Unspecified;
                                    break;
                                default:
                                    patient.Gender = Gender.Unspecified;
                                    break;
                            }

                        }
                        if (!String.IsNullOrEmpty(patientPerson.birthTime.value))
                        {
                            patient.DOB = patientPerson.birthTime.value;
                        }

                        patients.Add(patient);
                    }

                }
            }
            return patients;
        }

        #endregion Helper
        #endregion

        #region Patient Correlation
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assigningAuthorityId"></param>
        /// <param name="patientId"></param>
        /// <param name="patientIdentifiers"></param>
        /// <returns></returns>
        public Result GetPHISource(string assigningAuthorityId, string patientId, out List<RemotePatientIdentifier> patientIdentifiers)
        {
            patientIdentifiers = null;
            try
            {
                MobiusPatientCorrelation mobiusPatientCorrelation = new MobiusPatientCorrelation();
                qualifiedPatientIdentifier[] PatientIdentifier = mobiusPatientCorrelation.getCorrelatedPatients(assigningAuthorityId, patientId);
                patientIdentifiers = new List<RemotePatientIdentifier>();
                if (PatientIdentifier != null && PatientIdentifier.Count() > 0)
                {
                    RemotePatientIdentifier patientIdentifier = null;
                    foreach (qualifiedPatientIdentifier item in PatientIdentifier)
                    {
                        patientIdentifier = new RemotePatientIdentifier();
                        patientIdentifier.PatientId = item.patientId;
                        patientIdentifier.CommunityIdentifier = item.assigningAuthorityId;
                        patientIdentifiers.Add(patientIdentifier);
                    }
                }
                patientIdentifiers.Distinct();

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



    }
}
