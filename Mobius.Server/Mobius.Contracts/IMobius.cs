#region namespaces list
using System.Data;
using System.ServiceModel;
using FirstGenesis.Mobius.Common;
using System.Collections.Generic;
#endregion

namespace FirstGenesis.Mobius.Server.MobiusHISEService
{
    #region ServiceContract
    [ServiceContract]
    public interface IMobius
    {
        //
        [OperationContract]
        Person[] SearchPatient(Person patientInfo);
        [OperationContract]
        ErrorCode GetAuth(string userName, string hassPass, out FirstGenesis.Mobius.Common.ErrorCode errorCode);
        [OperationContract]
        string GetUserGUIDByUserID(string UserID, out FirstGenesis.Mobius.Common.ErrorCode errorCode);
        [OperationContract]
        string GetFacilityName(string token, int FacilityId, out FirstGenesis.Mobius.Common.ErrorCode errorCode);
        [OperationContract]
        FacilityInfo GetCurrentFacilityInfo(string token, out FirstGenesis.Mobius.Common.ErrorCode errorCode);
        [OperationContract]
        string Login(string userId, string passwordHash, int facilityId, out FirstGenesis.Mobius.Common.ErrorCode errorCode);
        [OperationContract]
        DataSet GetUsersGroup(string userId, out FirstGenesis.Mobius.Common.ErrorCode errorCode);
        [OperationContract]
        Person PatientCorrelationRequest(string mobiusId, string facilityId);
        [OperationContract]
        document[] GetDocuments(string MPIId, string homeCommunityId);
        [OperationContract]
        document RetrieveDocument(string DocId, string HomeCommunityId, string RepositoryUniqueId);
        [OperationContract]
        bool UploadDocument(string HomeCommunityId, string strDocumentId, byte[] docByte, string RuleCreateDate, string RuleStartDate, string RuleEndDate, string patientId);
        [OperationContract]
        DataSet GetFacilities(int dropDownType, int userTypeId, int facilityId, string eicGuid);
        [OperationContract]
        bool SaveC32DocumentMetaData(string OriginalDocumentID, string PatientId, string UploadedBy, int DocumentType, string sTitle, string sAuthor, string sCreatedDate, string DocumentSource, string SourceCommunityID, string SourceRepositryID, string FacilityID, out  FirstGenesis.Mobius.Common.ErrorCode errorCode, bool Reposed, string filePath);
        [OperationContract]
        DataSet GetDocumentByPatientId(string PatientId, out ErrorCode errorCode);
        [OperationContract]
        NHINCommunity GetNhinCommunity();
        //int getFacilityID(string userGUID);
        [OperationContract]
        DataSet GetCommunitiesForMPIID(string MPIID);
        [OperationContract]
        DataSet GetUploadedDocumentMetaData(string MPIID, string DocID);
        [OperationContract]
        document GetDocument(string DocumentID);
        [OperationContract]
        DataSet GetCommonData(int dropDownType, int userTypeId, int facilityId, string eicGuid);

    }
    #endregion

    #region commented method stubs
    //Person[] findPatientAhaltaID(string demoInfo);
    //bool UpdateMPI(string patientInfo);
    //bool findPatientClinicalDoc(string uniqueIdentifier);
    //string FetchC32Doc(string DocID, string []intersectionofConsent);
    //bool statusMessgUploadC32(string docid, string alhtaid);
    #endregion
}
