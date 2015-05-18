using System.Data;
using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Mobius.CoreLibrary;
using Mobius.Entity;
using System.Collections.Generic;
using MobiusServiceLibrary;
using System.IO;
using System.Linq;
using PatientDiscovery;
using PolicyEngine;
using System.Xml;
using FirstGenesis.Mobius.Server.DataAccessLayer;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace MobiusHISE.UnitTest
{


    /// <summary>
    ///This is a test class for MobiusHISETest and is intended
    ///to contain all MobiusHISETest Unit Tests
    ///</summary>
    [TestClass()]
    public class MobiusHISETest
    {


        private TestContext testContextInstance;
        private string _patientId = "99990099";
        private string PatientId { get { return _patientId; } }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }


        /// <summary>
        ///A test for GetFacilities
        ///</summary>
        [TestMethod()]
        public void GetFacilitiesTest()
        {
            Mobius.Server.MobiusHISEService.MobiusHISE target = new Mobius.Server.MobiusHISEService.MobiusHISE(); // TODO: Initialize to an appropriate value
            string eicGuid = string.Empty; // TODO: Initialize to an appropriate value
            GetMasterDataResponse expected = null; // TODO: Initialize to an appropriate value
            GetMasterDataResponse actual;
            GetMasterDataRequest getMasterDataRequest = new GetMasterDataRequest();
            getMasterDataRequest.MasterCollection = MasterCollection.City;
            actual = target.GetMasterData(getMasterDataRequest);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SearchPatient when local MPI is not provided in the expected object.
        ///</summary>
        [TestMethod()]
        public void SearchPatientWithoutLocalMPITest()
        {
            Mobius.Server.MobiusHISEService.MobiusHISE target = new Mobius.Server.MobiusHISEService.MobiusHISE(); // TODO: Initialize to an appropriate value
            SearchPatientRequest SearchPatientRequest = new SearchPatientRequest();
            SearchPatientRequest.Demographics = new MobiusServiceLibrary.Demographics();
            SearchPatientResponse expected = new SearchPatientResponse();
            SearchPatientResponse actual = new SearchPatientResponse();

            Community nhinCommunties = new Community();
            List<Community> Communities = new List<Community>();
            nhinCommunties.CommunityIdentifier = "2.16.840.1.113883.3.1605";
            Communities.Add(nhinCommunties);

            SearchPatientRequest.Demographics.GivenName = "Randall";
            SearchPatientRequest.Demographics.FamilyName = "Jones";
            SearchPatientRequest.Demographics.DOB = "19820703";// "07/03/1982";
            SearchPatientRequest.Demographics.Gender =  Gender.Male;
            SearchPatientRequest.Demographics.LocalMPIID = ""; // "990099";
            SearchPatientRequest.NHINCommunities = Communities;


            MobiusServiceLibrary.Patient patientDetails = new MobiusServiceLibrary.Patient();
            patientDetails.GivenName.Add("Randall");
            patientDetails.FamilyName.Add("Jones");
            patientDetails.DOB = "19820703";// "07/03/1982";

            patientDetails.Gender = (Gender)Enum.Parse(typeof(Gender), "M", true);
            patientDetails.LocalMPIID = "";// "990099";
            expected.Patients.Add(patientDetails);
            expected.Result.IsSuccess = true;
            actual = target.SearchPatient(SearchPatientRequest);

            //Assert.AreEqual(expected, actual);
            Assert.AreEqual(expected.Patients[0].FamilyName, actual.Patients[0].FamilyName, "Returned Family name Matched.");
            Assert.AreEqual(expected.Patients[0].GivenName, actual.Patients[0].GivenName, "Returned Given name Matched.");
            Assert.AreEqual(expected.Patients[0].DOB, actual.Patients[0].DOB, "Returned DOB Matched.");
            Assert.AreEqual(expected.Patients[0].Gender, actual.Patients[0].Gender, "Returned Gender Matched.");
            Assert.AreNotEqual(expected.Patients[0].LocalMPIID, actual.Patients[0].LocalMPIID, "Returned LocalMPIID Does not Matched.");
            

        }




        /// <summary>
        ///A test for GetNhinCommunity
        ///</summary>
        [TestMethod()]
        public void GetNhinCommunityTest()
        {
            Mobius.Server.MobiusHISEService.MobiusHISE target = new Mobius.Server.MobiusHISEService.MobiusHISE(); // TODO: Initialize to an appropriate value
            GetNhinCommunityResponse expected = null;
            GetNhinCommunityResponse actual;
            actual = target.GetNhinCommunity();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void GetAvailableValidationsTest()
        {
            Mobius.Server.MobiusHISEService.MobiusHISE target = new Mobius.Server.MobiusHISEService.MobiusHISE(); // TODO: Initialize to an appropriate value
            GetAvailableValidationsResponse actual = null;
            actual = target.GetAvailableValidations();
            Assert.IsTrue(actual.AvailableValidations.Count>0);
        }

        [TestMethod()]
        public void ValidateDocumentTest()
        { 
            Mobius.Server.MobiusHISEService.MobiusHISE target = new Mobius.Server.MobiusHISEService.MobiusHISE(); // TODO: Initialize to an appropriate value
            ValidateDocumentRequest request = new ValidateDocumentRequest();
            ValidateDocumentResponse response = null;
            byte[] documentBytes = null;

            request.SpecificationId = "c32_v_2_5_c83_2_0";
            request.ValidationType = NISTValidationType.ALL;
            string docPath;
            //docPath = @"\\10.131.10.151\First Genesis Documents\e724be11-e94f-451b-8824-df39238f94ce\32cee169-a525-4862-8b26-4d09702fd8b0.xml";
            //docPath = @"\\10.131.10.151\First Genesis Documents\TestC32Docs\HITSP_C32v2.5_Rev4_11Sections_Entries_MinimalErrors.xml";
            docPath = @"\\10.131.10.151\First Genesis Documents\TestC32Docs\InvalidC32.xml";

            FileStream fs = new FileStream(docPath, FileMode.Open, FileAccess.Read);
            documentBytes = new byte[fs.Length];
            fs.Read(documentBytes, 0, documentBytes.Length);
            fs.Flush();
            fs.Close();

            UTF8Encoding Encoder = new UTF8Encoding();
            string fileContent = Encoder.GetString(documentBytes);
           
            request.Document = fileContent;

            response = target.ValidateDocument(request);
            if (response.Result.IsSuccess)
            {
                switch (request.ValidationType)
                {
                    //case NISTValidationType.ERRORS:
                    //    Assert.IsTrue(response.ValidationResult.issue.Count == 13);
                    //    break;
                    //case NISTValidationType.WARNING:
                    //    Assert.IsTrue(response.ValidationResult.issue.Count == 10);
                    //    break;
                    //case NISTValidationType.ALL:
                    //    Assert.IsTrue(response.ValidationResult.issue.Count == 23);
                    //    break;
                    default:
                        Assert.IsTrue(response.ValidationResult.issue.Count > 0);
                        break;
                }
            }
            else
                Assert.IsTrue(false);

        }

        #region DocumentMetadata

        /// <summary>
        ///A test for GetDocuments
        ///</summary>
        [TestMethod()]
        public void GetDocumentMetadataValidationTest()
        {
            string patientId = "";
            string communityId = MobiusAppSettingReader.LocalHomeCommunityID;
            //List<NHINCommunity> NHINCommunities = new List<NHINCommunity>();

            List<Community> NHINCommunities = new List<Community>();




            Mobius.Server.MobiusHISEService.MobiusHISE target = new Mobius.Server.MobiusHISEService.MobiusHISE(); // TODO: Initialize to an appropriate value
            GetDocumentMetadataResponse expected = new GetDocumentMetadataResponse();
            expected.Result = new Result();
            expected.Result.SetError(ErrorCode.Document_PatientId_Missing);
            GetDocumentMetadataResponse actual;
            GetDocumentMetadataRequest request = new GetDocumentMetadataRequest();
            request.patientId = patientId;
            Community nhinCommunity;
            nhinCommunity = new Community();
            nhinCommunity.CommunityIdentifier = communityId;
            request.communities.Add(nhinCommunity);
            actual = target.GetDocumentMetadata(request);

            //Error check for missing patient ID
            Assert.AreEqual(expected.Result.ErrorCode, actual.Result.ErrorCode, "Call fail for missing PatientId");

            //Error check for missing Community
            patientId = "99990099";
            communityId = "";
            request = new GetDocumentMetadataRequest();
            request.patientId = patientId;
            nhinCommunity = new Community();
            nhinCommunity.CommunityIdentifier = communityId;
            request.communities.Add(nhinCommunity);
            actual = target.GetDocumentMetadata(request);
            expected.Result = new Result();
            expected.Result.SetError(ErrorCode.Document_communityId_Missing);
            Assert.AreEqual(expected.Result.ErrorCode, actual.Result.ErrorCode, "Call fail for missing CommunityId");
        }

        /// <summary>
        ///A test for GetDocuments
        ///</summary>
        [TestMethod()]
        public void GetDocumentMetadataFromMobiusDBTest()
        {
            string patientId = "";
            string communityId = MobiusAppSettingReader.LocalHomeCommunityID;

            List<Community> NHINCommunities = new List<Community>();
            Community nhinCommunity;
            nhinCommunity = new Community();
            nhinCommunity.CommunityIdentifier = communityId;
            NHINCommunities.Add(nhinCommunity);

            Mobius.Server.MobiusHISEService.MobiusHISE target = new Mobius.Server.MobiusHISEService.MobiusHISE(); // TODO: Initialize to an appropriate value
            GetDocumentMetadataResponse expected = new GetDocumentMetadataResponse();
            GetDocumentMetadataResponse actual;

            //valid Call to gateway
            GetNhinCommunityResponse NhinCommunityResponse = target.GetNhinCommunity();

            if (NhinCommunityResponse.Result.IsSuccess)
            {

                patientId = "99990099";
                communityId = "";

                GetDocumentMetadataRequest request = new GetDocumentMetadataRequest();
                request.patientId = patientId;
                nhinCommunity = new Community();
                nhinCommunity.CommunityIdentifier = communityId;
                request.communities.Add(nhinCommunity);
                actual = target.GetDocumentMetadata(request);
                expected.Result = new Result();
                expected.Result.IsSuccess = true; ;
                Assert.AreEqual(expected.Result.IsSuccess, actual.Result.IsSuccess, "Call is Success");
            }
            else
            {
                Assert.Fail(NhinCommunityResponse.Result.ErrorMessage);
            }
        }



        /// <summary>
        ///A test for GetDocuments
        ///</summary>
        [TestMethod()]
        public void GetDocumentMetadataAllTest()
        {
            string patientId = "";
            string communityId = MobiusAppSettingReader.LocalHomeCommunityID;
            List<Community> NHINCommunities = new List<Community>();

            Mobius.Server.MobiusHISEService.MobiusHISE target = new Mobius.Server.MobiusHISEService.MobiusHISE(); // TODO: Initialize to an appropriate value
            GetDocumentMetadataResponse expected = new GetDocumentMetadataResponse();
            expected.Result = new Result();
            expected.Result.SetError(ErrorCode.Document_PatientId_Missing);
            GetDocumentMetadataResponse actual;

            //valid Call to gateway
            //Add Home community to the list, to fetch data from Mobius DB
            Community HomeCommunity = new Community();
            HomeCommunity.CommunityIdentifier = MobiusAppSettingReader.LocalHomeCommunityID;
            NHINCommunities.Add(HomeCommunity);

            //Add Remote community to the list, to fetch data from Remote community
            Community RemoteCommunity = new Community();
            RemoteCommunity.CommunityIdentifier = "2.16.840.1.113883.3.1605";

            NHINCommunities.Add(RemoteCommunity);

            patientId = "99990099";
            communityId = "";

            GetDocumentMetadataRequest request = new GetDocumentMetadataRequest();
            request.patientId = patientId;
            Community nhinCommunity = new Community();
            nhinCommunity.CommunityIdentifier = communityId;
            request.communities.Add(nhinCommunity);
            actual = target.GetDocumentMetadata(request);
            expected.Result = new Result();
            expected.Result.IsSuccess = true; ;
            Assert.IsTrue(actual.Documents.Count > 0, "Documents returned.");
            //Assert.AreEqual(expected.Result.IsSuccess, actual.Result.IsSuccess, "Call is Success");


        }
        #endregion

        /// <summary>
        ///A test for GetDocument
        ///</summary>
        [TestMethod()]
        public void GetDocumentTest()
        {
            Mobius.Server.MobiusHISEService.MobiusHISE target = new Mobius.Server.MobiusHISEService.MobiusHISE(); // TODO: Initialize to an appropriate value
            GetDocumentResponse actual;
            GetDocumentResponse expected = new GetDocumentResponse();
            GetDocumentRequest request = new GetDocumentRequest();
            request.subjectRole = "Public health officer";
            request.purpose = "PUBLICHEALTH";
            request.patientId = "ZNV9GVPQ"; // Randall Jones
            request.documentId = "202ac9fb-8119-4c3c-9be2-461424bd69e0";
            expected = new GetDocumentResponse();
            expected.Result.IsSuccess = true;
            //valid Call to gateway
            actual = target.GetDocument(request);
            //Assert.AreEqual(expected.Result.IsSuccess, actual.Result.IsSuccess, "Call fail for missing documentId");

        }

        /// <summary>
        ///A test for UploadDocument
        ///</summary>
        [TestMethod()]
        public void UploadDocumentTest()
        {
            Mobius.Server.MobiusHISEService.MobiusHISE target = new Mobius.Server.MobiusHISEService.MobiusHISE(); // TODO: Initialize to an appropriate value
            string communityId = string.Empty;
            string documentId = string.Empty;
            byte[] documentBytes = null;
            string ruleCreatedDate = string.Empty;
            string ruleStartDate = string.Empty;
            string ruleEndDate = string.Empty;
            string patientId = string.Empty;
            string uploadedBy = string.Empty;
            string repositoryId = string.Empty;
            string facilityId = string.Empty;
            bool expected = false;
            bool actual;



            communityId = string.Empty;
            documentId = Guid.NewGuid().ToString();
            documentBytes = null;
            ruleCreatedDate = System.DateTime.Now.ToShortDateString(); ;
            ruleStartDate = System.DateTime.Now.ToShortDateString();
            ruleEndDate = System.DateTime.Now.AddDays(4).ToShortDateString();
            patientId = "1";
            uploadedBy = Guid.NewGuid().ToString();
            repositoryId = "1";
            facilityId = "1";
            expected = true;
            UploadDocumentResponse uploadDocumentResponse = new UploadDocumentResponse();
            uploadDocumentResponse.Result = new Result();
            string docPath = @"\\10.131.10.151\First Genesis Documents\Upload\HITSP_C32v2.5_Randall Jones.xml";
            FileStream fs = new FileStream(docPath, FileMode.Open, FileAccess.Read);
            documentBytes = new byte[fs.Length];
            fs.Read(documentBytes, 0, documentBytes.Length);
            fs.Flush();
            fs.Close();

            UploadDocumentRequest uploadDocumentRequest = new UploadDocumentRequest();
            uploadDocumentRequest.CommunityId = MobiusAppSettingReader.LocalHomeCommunityID;
            uploadDocumentRequest.DocumentId = documentId;
            uploadDocumentRequest.DocumentBytes = documentBytes;
            uploadDocumentRequest.XACMLBytes = documentBytes;
            uploadDocumentRequest.PatientId = patientId;
            uploadDocumentRequest.UploadedBy = uploadedBy;
            uploadDocumentRequest.RepositoryId = repositoryId;
            

            uploadDocumentResponse = target.UploadDocument(uploadDocumentRequest);

            Assert.AreEqual(expected, uploadDocumentResponse.Result.IsSuccess);
        }



        #region RemoteServer Call

        /// <summary>
        ///A test for SearchPatient when local MPI is provided in the expected object.
        ///</summary>
        [TestMethod()]
        public void SearchPatientTest_GateWayCall()
        {
            Mobius.Server.MobiusHISEService.MobiusHISE target = new Mobius.Server.MobiusHISEService.MobiusHISE();
            SearchPatientRequest SearchPatientRequest = new SearchPatientRequest();
            SearchPatientRequest.Demographics = new MobiusServiceLibrary.Demographics();
            SearchPatientResponse expected = new SearchPatientResponse();
            SearchPatientResponse actual = new SearchPatientResponse();

            GetNhinCommunityResponse NhinCommunityResponse;//= target.GetNhinCommunity();


            NhinCommunityResponse = new GetNhinCommunityResponse();
            List<Community> Communities = new List<Community>();
            Community oNHINCommunity = new Community();
            oNHINCommunity.CommunityIdentifier = MobiusAppSettingReader.LocalHomeCommunityID;
            Communities.Add(oNHINCommunity);





            if (!NhinCommunityResponse.Result.IsSuccess)
            {


                SearchPatientRequest.Demographics.GivenName = "Randall";
                SearchPatientRequest.Demographics.FamilyName = "Jones";
                SearchPatientRequest.Demographics.DOB = "19820704";//"19820703";// "07/03/1982";
                SearchPatientRequest.Demographics.Gender = Gender.Female;
                SearchPatientRequest.Demographics.LocalMPIID = "99990099";
                SearchPatientRequest.NHINCommunities = Communities;


                MobiusServiceLibrary.Patient patientDetails = new MobiusServiceLibrary.Patient();
                patientDetails.GivenName.Add("Randall");
                patientDetails.FamilyName.Add("Jones");
                patientDetails.DOB = "19820704";// "07/03/1982";
                patientDetails.Gender = (Gender)Enum.Parse(typeof(Gender), "M", true);
                patientDetails.LocalMPIID = "99990099";// "990099";
                expected.Patients.Add(patientDetails);
                expected.Result.IsSuccess = true;
                actual = target.SearchPatient(SearchPatientRequest);

                if (actual.Result.IsSuccess)
                {
                    //Assert.AreEqual(expected, actual);
                    Assert.AreEqual(expected.Patients[0].FamilyName, actual.Patients[0].FamilyName, "Returned Family name Matched.");
                    Assert.AreEqual(expected.Patients[0].GivenName, actual.Patients[0].GivenName, "Returned Given name Matched.");
                    Assert.AreEqual(expected.Patients[0].DOB, actual.Patients[0].DOB, "Returned DOB Matched.");
                    Assert.AreEqual(expected.Patients[0].Gender, actual.Patients[0].Gender, "Returned Gender Matched.");
                    Assert.AreNotEqual(expected.Patients[00].LocalMPIID, actual.Patients[0].LocalMPIID, "Returned LocalMPIID Does not Matched.");
                    
                }
                else
                {
                    Assert.Fail(actual.Result.ErrorMessage);
                }
            }
            else
            {
                Assert.Fail(actual.Result.ErrorMessage);
            }
        }

        /// <summary>
        ///A test for GetDocument
        ///</summary>
        [TestMethod()]
        public void GetDocumentTest_GateWayCall()
        {
            Mobius.Server.MobiusHISEService.MobiusHISE target = new Mobius.Server.MobiusHISEService.MobiusHISE(); // TODO: Initialize to an appropriate value
            string documentId = string.Empty;
            GetDocumentResponse actual;
            GetDocumentResponse expected = new GetDocumentResponse();
            List<Community> Communities = new List<Community>();
            Community community = null;
            GetNhinCommunityResponse NhinCommunityResponse = target.GetNhinCommunity();

            GetDocumentRequest getDocumentRequest = new GetDocumentRequest();
            getDocumentRequest.documentId = "c0db92c1-8390-4a2a-934b-9a8522017f98";
            getDocumentRequest.subjectRole = "Audiologist";
            actual = target.GetDocument(getDocumentRequest);

            if (NhinCommunityResponse.Result.IsSuccess)
            {
                GetDocumentMetadataRequest request = new GetDocumentMetadataRequest();
                request.patientId = this.PatientId;
                Community nhinCommunity;
                nhinCommunity = new Community();

                foreach (NHINCommunity nHINCommunity in NhinCommunityResponse.Communities)
                {
                    community = new Community();
                    community.CommunityIdentifier = nHINCommunity.CommunityDescription;
                    request.communities.Add(community);
                }



                GetDocumentMetadataResponse documentMetadataResponse = target.GetDocumentMetadata(request);
                //GetDocumentMetadataResponse documentMetadataResponse =  documentMetadataResponse.Documents.Add(XmlSerializerHelper.DeserializeObject(XmlSerializerHelper.SerializeObject(target.GetDocumentMetadata(request)), typeof(Document)) as Document);                        

                if (documentMetadataResponse.Result.IsSuccess)
                {

                    foreach (Document item in documentMetadataResponse.Documents)
                    {
                        getDocumentRequest = new GetDocumentRequest();
                        getDocumentRequest.documentId = item.DocumentUniqueId;
                        getDocumentRequest.subjectRole = "Audiologist";
                        actual = target.GetDocument(getDocumentRequest);

                        actual = target.GetDocument(getDocumentRequest);
                        expected.Result.SetError(ErrorCode.Document_PatientId_Missing);
                        expected = new GetDocumentResponse();
                        expected.Result.IsSuccess = true;
                        actual = target.GetDocument(getDocumentRequest);
                        Assert.AreEqual(expected.Result.IsSuccess, actual.Result.IsSuccess, "Call fail for missing documentId");
                    }
                }

            }
        }

        [TestMethod()]
        public void UploadDocumentTest_GateWayCall()
        {
            Mobius.Server.MobiusHISEService.MobiusHISE target = new Mobius.Server.MobiusHISEService.MobiusHISE(); // TODO: Initialize to an appropriate value
            string communityId = string.Empty;
            string documentId = string.Empty;
            byte[] documentBytes = null;
            string ruleCreatedDate = string.Empty;
            string ruleStartDate = string.Empty;
            string ruleEndDate = string.Empty;
            string patientId = string.Empty;
            string uploadedBy = string.Empty;
            string repositoryId = string.Empty;
            string facilityId = string.Empty;
            bool expected = false;
            bool actual;



            communityId = string.Empty;
            documentId = Guid.NewGuid().ToString();
            documentBytes = null;
            ruleCreatedDate = System.DateTime.Now.ToShortDateString(); ;
            ruleStartDate = System.DateTime.Now.ToShortDateString();
            ruleEndDate = System.DateTime.Now.AddDays(4).ToShortDateString();
            patientId = "1";
            uploadedBy = Guid.NewGuid().ToString();
            repositoryId = "1";
            facilityId = "1";
            expected = true;
            UploadDocumentResponse uploadDocumentResponse = new UploadDocumentResponse();
            uploadDocumentResponse.Result = new Result();

            string docPath = @"\\10.131.10.151\First Genesis Documents\Upload\HITSP_C32v2.5_Randall Jones.xml";
            FileStream fs = new FileStream(docPath, FileMode.Open, FileAccess.Read);
            documentBytes = new byte[fs.Length];
            fs.Read(documentBytes, 0, documentBytes.Length);
            fs.Flush();
            fs.Close();

            UploadDocumentRequest uploadDocumentRequest = new UploadDocumentRequest();
            uploadDocumentRequest.CommunityId = MobiusAppSettingReader.LocalHomeCommunityID;
            uploadDocumentRequest.DocumentId = documentId;
            uploadDocumentRequest.DocumentBytes = documentBytes;
            uploadDocumentRequest.XACMLBytes = documentBytes;
            uploadDocumentRequest.PatientId = patientId;
            uploadDocumentRequest.UploadedBy = uploadedBy;
            uploadDocumentRequest.RepositoryId = repositoryId;
            


            uploadDocumentResponse = target.UploadDocument(uploadDocumentRequest);
            Assert.AreEqual(expected, uploadDocumentResponse.Result.IsSuccess);
        }

        /// <summary>
        ///A test for GetDocuments
        ///</summary>
        [TestMethod()]
        public void GetDocumentMetadataTest_GateWayCall()
        {
            string patientId = "";
            string communityId = MobiusAppSettingReader.LocalHomeCommunityID;
            List<NHINCommunity> NHINCommunities = new List<NHINCommunity>();

            Mobius.Server.MobiusHISEService.MobiusHISE target = new Mobius.Server.MobiusHISEService.MobiusHISE(); // TODO: Initialize to an appropriate value
            GetDocumentMetadataResponse expected = new GetDocumentMetadataResponse();
            expected.Result = new Result();
            expected.Result.SetError(ErrorCode.Document_PatientId_Missing);
            GetDocumentMetadataResponse actual;
            GetDocumentMetadataRequest request = new GetDocumentMetadataRequest();
            //valid Call to gateway - Remote community
            GetNhinCommunityResponse NhinCommunityResponse = target.GetNhinCommunity();

            List<Community> Communities = new List<Community>();
            Community community = null;
            foreach (NHINCommunity nHINCommunity in NhinCommunityResponse.Communities)
            {
                community = new Community();
                community.CommunityIdentifier = nHINCommunity.CommunityDescription;
                Communities.Add(community);
            }

            if (NhinCommunityResponse.Result.IsSuccess)
            {
                IEnumerable<MobiusServiceLibrary.Community> oo = Communities.Where(t => t.CommunityIdentifier == "2.2");
                patientId = this.PatientId;
                communityId = "";
                request.patientId = this.PatientId;
                foreach (MobiusServiceLibrary.Community comm in oo)
                {
                    request.communities.Add(community);
                }
                actual = target.GetDocumentMetadata(request);
                expected.Result = new Result();
                expected.Result.IsSuccess = true; ;
                Assert.AreEqual(expected.Result.IsSuccess, actual.Result.IsSuccess, "Call is Success");
            }
            else
            {
                Assert.Fail(String.IsNullOrEmpty(NhinCommunityResponse.Result.ErrorMessage) ? NhinCommunityResponse.Result.ErrorCode.ToString() : NhinCommunityResponse.Result.ErrorMessage);
            }
        }


        #endregion RemoteServer Call

        #region "AddProviderTest"
        /// <summary>
        ///A test for AddProvider
        ///</summary>
        [TestMethod()]
        public void AddProviderTest()
        {
            Mobius.Server.MobiusHISEService.MobiusHISE target = new Mobius.Server.MobiusHISEService.MobiusHISE(); // TODO: Initialize to an appropriate value
            AddProviderRequest registerProviderRequest = null; // TODO: Initialize to an appropriate value
            AddProviderResponse expected = null; // TODO: Initialize to an appropriate value
            AddProviderResponse actual;
            registerProviderRequest = new AddProviderRequest();
            registerProviderRequest.Provider = new MobiusServiceLibrary.Provider();


            registerProviderRequest.Provider.City = new MobiusServiceLibrary.City();
            registerProviderRequest.Provider.City.CityName = "EAST HOMER";
            registerProviderRequest.Provider.City.State = new MobiusServiceLibrary.State();
            registerProviderRequest.Provider.City.State.StateName = "NY";
            registerProviderRequest.Provider.City.State.Country = new MobiusServiceLibrary.Country();
            registerProviderRequest.Provider.City.State.Country.CountryName = "US";
            registerProviderRequest.Provider.Email = "test.test@test.com";
            registerProviderRequest.Provider.FirstName = "FirstName";
            registerProviderRequest.Provider.Gender = Gender.Male;
            registerProviderRequest.Provider.IndividualProvider = true;
            registerProviderRequest.Provider.Language = new MobiusServiceLibrary.Language();
            registerProviderRequest.Provider.Language.LanguageId = 1;
            registerProviderRequest.Provider.LastName = "LastName";
            registerProviderRequest.Provider.MedicalRecordsDeliveryEmailAddress = "test.test@test.com";
            registerProviderRequest.Provider.MiddleName = "MiddleName";
            registerProviderRequest.Provider.PostalCode = "12345";
            registerProviderRequest.Provider.ProviderType = ProviderType.Clinics;
            registerProviderRequest.Provider.Status = Status.Active;
            registerProviderRequest.Provider.StreetName = "StreetName";
            registerProviderRequest.Provider.StreetNumber = "12345";

            registerProviderRequest.CSR = "MIIBnTCCAQYCAQAwXTELMAkGA1UEBhMCU0cxETAPBgNVBAoTCE0yQ3J5cHRvMRIwEAYDVQQDEwlsb2NhbGhvc3QxJzA" +
                                          "lBgkqhkiG9w0BCQEWGGFkbWluQHNlcnZlci5leGFtcGxlLmRvbTCBnzANBgkqhkiG9w0BAQEFAAOBjQAwgYkCgYEAr1nYY1Qr" +
                                          "ll1ruB/FqlCRrr5nvupdIN+3wF7q915tvEQoc74bnu6b8IbbGRMhzdzmvQ4SzFfVEAuMMuTHeybPq5th7YDrTNizKKxOBnqE2KY" +
                                           "uX9X22A1Kh49soJJFg6kPb9MUgiZBiMlvtb7K3CHfgw5WagWnLl8Lb+ccvKZZl+8CAwEAAaAAMA0GCSqGSIb3DQEBBAUAA4GBAHpoRp" +
                                           "5YS55CZpy+wdigQEwjL/wSluvo+WjtpvP0YoBMJu4VMKeZi405R7o8oEwiPdlrrliKNknFmHKIaCKTLRcU59ScA6ADEIWUzqmUzP5Cs6jr" +
                                           "SRo3NKfg1bd09D1K9rsQkRc9Urv9mRBIsredGnYECNeRaK5R1yzpOowninXC";


            actual = target.AddProvider(registerProviderRequest);
            if (actual.Result.IsSuccess)
            {
                Assert.IsTrue(actual.Result.IsSuccess, actual.Result.ErrorMessage.ToString());
            }
            else
            {
                Assert.IsTrue(true, actual.Result.ErrorMessage.ToString());
            }
        }
        #endregion

        #region AddPatientTest
        /// <summary>
        ///A test for AddrPatient
        ///</summary>
        [TestMethod()]
        public void AddPatientTest()
        {
            Mobius.Server.MobiusHISEService.MobiusHISE target = new Mobius.Server.MobiusHISEService.MobiusHISE(); // TODO: Initialize to an appropriate value
            AddPatientRequest registerPatientRequest = null; // TODO: Initialize to an appropriate value
            AddPatientResponse expected = null; // TODO: Initialize to an appropriate value
            AddPatientResponse actual;

            expected = new AddPatientResponse();
            registerPatientRequest = new AddPatientRequest();
            registerPatientRequest.Patient = new MobiusServiceLibrary.Patient();

            //Address
            registerPatientRequest.Patient.PatientAddress = new List<MobiusServiceLibrary.Address>();
            MobiusServiceLibrary.Address address = new MobiusServiceLibrary.Address();
            address.AddressLine1 = "testAddressLine1";
            address.AddressLine2 = "testAddressLine2";
            address.City = new MobiusServiceLibrary.City();
            address.City.CityName = "EAST HOMER";
            address.City.State = new MobiusServiceLibrary.State();
            address.City.State.StateName = "NY";
            address.City.State.Country = new MobiusServiceLibrary.Country();
            address.City.State.Country.CountryName = "US";
            address.Zip = "12345";
            registerPatientRequest.Patient.PatientAddress.Add(address);

            registerPatientRequest.Patient.CommunityId = "2.16.840.1.113883.3.1605";
            registerPatientRequest.Patient.CSR = "MIIBnTCCAQYCAQAwXTELMAkGA1UEBhMCU0cxETAPBgNVBAoTCE0yQ3J5cHRvMRIwEAYDVQQDEwlsb2NhbGhvc3QxJzA" +
                                                "lBgkqhkiG9w0BCQEWGGFkbWluQHNlcnZlci5leGFtcGxlLmRvbTCBnzANBgkqhkiG9w0BAQEFAAOBjQAwgYkCgYEAr1nYY1Qr" +
                                                 "ll1ruB/FqlCRrr5nvupdIN+3wF7q915tvEQoc74bnu6b8IbbGRMhzdzmvQ4SzFfVEAuMMuTHeybPq5th7YDrTNizKKxOBnqE2KY" +
                                                 "uX9X22A1Kh49soJJFg6kPb9MUgiZBiMlvtb7K3CHfgw5WagWnLl8Lb+ccvKZZl+8CAwEAAaAAMA0GCSqGSIb3DQEBBAUAA4GBAHpoRp" +
                                                  "5YS55CZpy+wdigQEwjL/wSluvo+WjtpvP0YoBMJu4VMKeZi405R7o8oEwiPdlrrliKNknFmHKIaCKTLRcU59ScA6ADEIWUzqmUzP5Cs6jr" +
                                                  "SRo3NKfg1bd09D1K9rsQkRc9Urv9mRBIsredGnYECNeRaK5R1yzpOowninXC";

            registerPatientRequest.Patient.GivenName = new List<string>();
            registerPatientRequest.Patient.GivenName.Add("Randall");
            registerPatientRequest.Patient.FamilyName.Add("Jones");
            registerPatientRequest.Patient.DOB = "07/18/2012";// "07/03/1982";
            registerPatientRequest.Patient.Gender = (Gender)Enum.Parse(typeof(Gender), "M", true);


            registerPatientRequest.Patient.EmailAddress = "Randall.Jones@gmail.com";
            registerPatientRequest.Patient.Telephones = new List<MobiusServiceLibrary.Telephone>();
            MobiusServiceLibrary.Telephone telephone = new MobiusServiceLibrary.Telephone();
            telephone.Extensionnumber = "";
            telephone.Number = "";
            telephone.Type = "";
            registerPatientRequest.Patient.Telephones.Add(telephone);
            actual = target.AddPatient(registerPatientRequest);
            if (actual.Result.IsSuccess)
            {
                Assert.IsTrue(actual.Result.IsSuccess, actual.Result.ErrorMessage.ToString());
            }
            else
            {
                Assert.IsTrue(true, actual.Result.ErrorMessage.ToString());
            }
        }

        #endregion

        /// <summary>
        ///A test for GetPatientDetails
        ///</summary>
        [TestMethod()]
        public void GetPatientDetailsTest()
        {
            Mobius.Server.MobiusHISEService.MobiusHISE target = new Mobius.Server.MobiusHISEService.MobiusHISE(); // TODO: Initialize to an appropriate value
            GetPatientDetailsResponse expected = null; // TODO: Initialize to an appropriate value
            GetPatientDetailsResponse actual;
            GetPatientDetailsRequest request = new GetPatientDetailsRequest();
            request.MPIID = "1";
            actual = target.GetPatientDetails(request);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        #region"GetLocalityByZipCode"

        ///A test for GetLocalityByZipCode
        ///</summary>
        [TestMethod()]
        public void GetLocalityByZipCodeTest()
        {
            Mobius.Server.MobiusHISEService.MobiusHISE target = new Mobius.Server.MobiusHISEService.MobiusHISE(); // TODO: Initialize to an appropriate value
            string zipCode = "13056"; // TODO: Initialize to an appropriate value
            GetLocalityByZipCodeResponse actual;
            actual = target.GetLocalityByZipCode(zipCode);
            if (actual.Result.IsSuccess)
            {
                Assert.IsTrue(actual.Result.IsSuccess, actual.Result.ErrorMessage.ToString());
            }
            else
            {
                Assert.IsTrue(true, actual.Result.ErrorMessage.ToString());
            }
        }
        #endregion

        /// <summary>
        ///A test for FindCandidates
        ///</summary>
        [TestMethod()]
        public void FindCandidatesTest()
        {
            List<string> FamilyName = new List<string>();
            List<string> GivenName = new List<string>();
            Mobius.Server.MobiusHISEService.MobiusHISE target = new Mobius.Server.MobiusHISEService.MobiusHISE();
            RespondingGateway_PRPA_IN201305UV02RequestType reqType = null; // TODO: Initialize to an appropriate value
            Community_PRPA_IN201306UV02ResponseType expected = null; // TODO: Initialize to an appropriate value
            Community_PRPA_IN201306UV02ResponseType actual;

            EntityPatientDiscovery patientDiscovery = new EntityPatientDiscovery();
            // passing gender and instance intializing block.
            reqType = new RespondingGateway_PRPA_IN201305UV02RequestType();
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
            prpa.sender.device.asAgent.representedOrganization.id[0].root = "2.2";// "2.16.840.1.113883.3.1605";
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
            prpa.controlActProcess.queryByParameter.parameterList.livingSubjectAdministrativeGender[0].value[0].code = "M";
            prpa.controlActProcess.queryByParameter.parameterList.livingSubjectAdministrativeGender[0].semanticsText = new ST();

            prpa.controlActProcess.queryByParameter.parameterList.livingSubjectAdministrativeGender[0].semanticsText.representation = BinaryDataEncoding.TXT;
            prpa.controlActProcess.queryByParameter.parameterList.livingSubjectAdministrativeGender[0].semanticsText.Text = new string[] { "LivingSubject.administrativeGender" };

            prpa.controlActProcess.queryByParameter.parameterList.livingSubjectBirthTime = new PRPA_MT201306UV02LivingSubjectBirthTime[1] { new PRPA_MT201306UV02LivingSubjectBirthTime() };
            prpa.controlActProcess.queryByParameter.parameterList.livingSubjectBirthTime[0].value = new IVL_TS_explicit[1] { new IVL_TS_explicit() };
            prpa.controlActProcess.queryByParameter.parameterList.livingSubjectBirthTime[0].value[0].value = "19840704";
            prpa.controlActProcess.queryByParameter.parameterList.livingSubjectBirthTime[0].semanticsText = new ST();
            prpa.controlActProcess.queryByParameter.parameterList.livingSubjectBirthTime[0].semanticsText.representation = BinaryDataEncoding.TXT;
            prpa.controlActProcess.queryByParameter.parameterList.livingSubjectBirthTime[0].semanticsText.Text = new string[] { "LivingSubject.birthTime" };

            prpa.controlActProcess.queryByParameter.parameterList.livingSubjectName = new PRPA_MT201306UV02LivingSubjectName[1] { new PRPA_MT201306UV02LivingSubjectName() };
            prpa.controlActProcess.queryByParameter.parameterList.livingSubjectName[0].value = new EN_explicit[1] { new EN_explicit() };
            prpa.controlActProcess.queryByParameter.parameterList.livingSubjectName[0].value[0].Items = new ENXP_explicit[2] { new ENXP_explicit(), new ENXP_explicit() };
            FamilyName.Add("jones");
            en_explicitfamily family = new en_explicitfamily();
            family.partType = "FAM";
            family.Text = new string[1];
            family.Text = FamilyName.ToArray();
            prpa.controlActProcess.queryByParameter.parameterList.livingSubjectName[0].value[0].Items[0] = family;

            GivenName.Add("randall");
            en_explicitgiven given = new en_explicitgiven();
            given.partType = "GIV";
            given.Text = new string[1];
            given.Text = GivenName.ToArray();

            prpa.controlActProcess.queryByParameter.parameterList.livingSubjectName[0].value[0].Items[1] = given;

            prpa.controlActProcess.queryByParameter.parameterList.livingSubjectId = new PRPA_MT201306UV02LivingSubjectId[1] { new PRPA_MT201306UV02LivingSubjectId() };
            prpa.controlActProcess.queryByParameter.parameterList.livingSubjectId[0].value = new II[1] { new II() };
            prpa.controlActProcess.queryByParameter.parameterList.livingSubjectId[0].value[0].root = "2.16.840.1.113883.3.1605";
            prpa.controlActProcess.queryByParameter.parameterList.livingSubjectId[0].value[0].extension = "990099";//1111
            prpa.controlActProcess.queryByParameter.parameterList.livingSubjectId[0].semanticsText = new ST();
            prpa.controlActProcess.queryByParameter.parameterList.livingSubjectId[0].semanticsText.representation = BinaryDataEncoding.TXT;

            //Add NHIN target community collection in NhinTargetCommunityType array                  
            //create object of NhinTargetCommunities at index
            List<NhinTargetCommunityType> NhinTargetCommunity = new List<NhinTargetCommunityType>();
            NhinTargetCommunityType NhinTargetCommunityType = new PatientDiscovery.NhinTargetCommunityType();
            PatientDiscovery.HomeCommunityType homeCommunity = new PatientDiscovery.HomeCommunityType();
            homeCommunity.homeCommunityId = "2.16.840.1.113883.3.1605";
            NhinTargetCommunityType.homeCommunity = homeCommunity;
            // object of NhinTargetCommunity at index
            NhinTargetCommunity.Add(NhinTargetCommunityType);
            reqType.PRPA_IN201305UV02 = prpa;
            //assgin the NhinTargetCommunities to request
            reqType.NhinTargetCommunities = NhinTargetCommunity.ToArray();
            // actual = target.FindCandidates(reqType);
            // if (actual != null)
            // {
            //    Assert.IsTrue(true);
            // }
        }

        /// <summary>
        ///A test for checkPolicy
        ///</summary>
        [TestMethod()]
        public void checkPolicyTest()
        {
            List<XmlElement> xmlElements1 = new List<XmlElement>();
            List<XmlElement> xmlElements2 = new List<XmlElement>();
            List<XmlElement> xmlElements3 = new List<XmlElement>();
            XmlElement xmlElement = null;
            Mobius.Server.MobiusHISEService.MobiusHISE target = new Mobius.Server.MobiusHISEService.MobiusHISE(); // TODO: Initialize to an appropriate value
            CheckPolicyRequestType checkPolicyRequestType = new CheckPolicyRequestType(); // TODO: Initialize to an appropriate value
            PolicyEngine.AssertionType assertion = null; // TODO: Initialize to an appropriate value
            // CheckPolicyResponse actual;

            checkPolicyRequestType.request = new RequestType();
            checkPolicyRequestType.request.Subject = new SubjectType[1] { new SubjectType() };

            AttributeType attributeType1 = new AttributeType();
            attributeType1.AttributeId = "urn:oasis:names:tc:xacml:1.0:subject:subject-id";
            XmlDocument XmlDocument1 = new System.Xml.XmlDocument();
            xmlElement = XmlDocument1.CreateElement("subject-id");
            xmlElement.InnerText = "99990099";
            xmlElements1.Add(xmlElement);

            AttributeType attributeType2 = new AttributeType();
            attributeType2.AttributeId = "urn:oasis:names:tc:xacml:2.0:subject:role";
            XmlDocument XmlDocument2 = new System.Xml.XmlDocument();
            xmlElement = XmlDocument2.CreateElement("role");
            xmlElement.InnerText = "Audiologist";
            xmlElements2.Add(xmlElement);

            AttributeType attributeType3 = new AttributeType();
            attributeType3.AttributeId = "urn:oasis:names:tc:xacml:1.0:subject:purposeofuse";
            XmlDocument XmlDocument3 = new System.Xml.XmlDocument();
            xmlElement = XmlDocument3.CreateElement("purposeofuse");
            xmlElement.InnerText = "TREATMENT";
            xmlElements3.Add(xmlElement);


            attributeType1.AttributeValue = xmlElements1.ToArray();
            attributeType2.AttributeValue = xmlElements2.ToArray();
            attributeType3.AttributeValue = xmlElements3.ToArray();

            checkPolicyRequestType.request.Subject[0].Attribute = new AttributeType[3] { new AttributeType(), new AttributeType(), new AttributeType() };

            checkPolicyRequestType.request.Subject[0].Attribute[0] = attributeType1;
            checkPolicyRequestType.request.Subject[0].Attribute[1] = attributeType2;
            checkPolicyRequestType.request.Subject[0].Attribute[2] = attributeType3;
            // actual = target.CheckPolicy(checkPolicyRequestType, assertion);
            // if (actual != null)
            // {
            //     Assert.IsTrue(true);
            // }

        }


        /// <summary>
        ///  This method is used to get deserialized response objecte stored in byte form in database
        ///  This method is for future reference only.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="eventTypeId"></param>
        /// <returns></returns>
        [TestMethod()]
        public void GetAuditEventData()
        {
            int eventId = 12;
            DbCommand dbCommand;
            DataTable dteventActionData = null;
            DataAccessManager dataAccessManager = DataAccessManager.GetInstance;
            dbCommand = dataAccessManager.GetStoredProcCommand("GetAuditData");
            dataAccessManager.AddInParameter(dbCommand, "@EventId", DbType.Int32, eventId);
            dteventActionData = dataAccessManager.ExecuteDataSet(dbCommand).Tables[0];
            byte[] byteArray = (byte[])dteventActionData.Rows[0]["Message"];
            if (byteArray.Length > 0)
            {
                Mobius.Entity.Patient responseObject = Deserialize(byteArray);
                Assert.IsNotNull(responseObject);
            }
            else
            {
                Assert.Fail("Response object not found in database.");
            }

        }
        /// <summary>
        /// Deserialize response object
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        private Mobius.Entity.Patient Deserialize(byte[] byteArray)
        {
            using (MemoryStream memorystream = new MemoryStream(byteArray))
            {
                BinaryFormatter bfd = new BinaryFormatter();
                Mobius.Entity.Patient deserializedObject = bfd.Deserialize(memorystream) as Mobius.Entity.Patient;
                return deserializedObject;
            }
        }


    }
}
