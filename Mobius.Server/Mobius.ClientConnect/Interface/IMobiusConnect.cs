using System;
using System.Collections.Generic;
using Mobius.Server.MobiusHISEService;
using Mobius.Entity;
using Mobius.CoreLibrary;


namespace Mobius.Client.Interface
{
    interface IMobiusConnect
    {

        Result GetDocumentMetadata(string patientId, List<MobiusNHINCommunity> NHINCommunities, DocumentQuery.AssertionType assertion, out List<MobiusDocument> patientDocuments);

        //Result RetrieveDocument(string DocId, string remoteCommunityId, string RepositoryUniqueId, out MobiusDocument document);        
        Result RetrieveDocument(string DocId, string remoteCommunityId, string RepositoryUniqueId, RetrieveDocument.AssertionType assertion, out MobiusDocument document);


        Result SearchPatient(Demographics demographics, List<MobiusNHINCommunity> NHINCommunities, PatientDiscovery.AssertionType assertionType, out List<Patient> patients);

        //Result UploadDocument(string homeCommunityId, string documentId, byte[] documentBytes,byte[] xacmlBytes, string patientId);
        Result UploadDocument(string homeCommunityId, string documentId, byte[] documentBytes, byte[] XACMLdocumentBytes, string patientId, SAMLAssertion.HarrisStore.AssertionType assertion);


        Result UploadDocument(string homeComunityid, string documentId, byte[] documentBytes, byte[] xacmlDocumentBytes, string xacmlDocumentId, string ruleCreatedDate, string ruleStartDate, string ruleEndDate, string remotePatientId, string remoteCommunityId, SAMLAssertion.HarrisStore.AssertionType assertion);

        Result GetPHISource(string assigningAuthorityId, string patientId, out List<RemotePatientIdentifier> patientIdentifiers);
    }
}
