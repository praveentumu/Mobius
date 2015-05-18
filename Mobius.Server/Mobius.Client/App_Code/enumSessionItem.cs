
namespace FirstGenesis.UI
{
    /// <summary>
    /// 
    /// </summary>
    public enum SessionItem
    {
        UserInformation,
        UserType,//MasterPage,baseclass,documentlist,masterpage2.master.aspx,
        UserName,//MasterPage,baseclass,masterpage.master.cs,masterpage2.master.aspx
        LinkUserGUID,//MasterPage
        Token,//MasterPage,searchuser,ViewC32document,
        UserRole,//MatserPage.master.aspx,
        ViewError,//basecalss,
        MPIID,//baseclass,
        UserEmailAddress,//baseclass
        IsOptIn,//baseclass
        DocumentId,//documentlist,UploadPatientRecord.aspx


        CommunityList,//documentlist
        SelectedPatientName,//documentlist
        SelectedValues,//documentlist
        XMLDOC,//documentlist
        PatientGender,//Uploadpatientrecord
        PatientDOB,//Uploadpatientrecord
        GetPatientReferralResponse,//referpatient
        SelectedPatient,//referpatient
        ProviderEMail,//sharepatientdocumnent
        SearchData,//searchuser
        FacilityType,//searchuser
        ErrorMessage,//Exception Helper.cs,Error.aspx
        MyReferrals,//ManageReferral
        ReferralsByMe,//ManageReferral
        DocInfo,//DocumentList
        SerialNumber,//SerialNumber
        // Manage Consent session Id
        PatientConsentId,//PatientConsentId
        PageIndex,// PageIndex
        IsNew, // IsNew        
        PHISources,//DocumentList,ShareDocument
        ValidTill,
        RenewalNotificationSent, //Default page
        XACMLBytes
    }
}