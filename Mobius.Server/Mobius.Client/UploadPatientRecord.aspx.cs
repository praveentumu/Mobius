using System;
using System.Linq;
using System.Web.UI;
using C32Utility;
using FirstGenesis.UI;
using FirstGenesis.UI.Base;
using Mobius.CoreLibrary;
using MobiusServiceLibrary;
using Mobius.Entity;
using MobiusServiceUtility;
using System.Collections.Generic;



public partial class UploadPatientRecord : BaseClass
{
    SoapHandler soapHandler;
    SoapProperties soapProperties = new SoapProperties();

    
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            soapHandler = new SoapHandler(Convert.ToString(GlobalSessions.SessionItem(SessionItem.SerialNumber)));
            //Check the ContentType of uploaded document. 
            if (fileC32Document.HasFile
                && (fileC32Document.PostedFile.ContentLength > 0) && fileC32Document.PostedFile.ContentType == "text/xml")
            {

                //Load Patient information from server
                MobiusServiceLibrary.Patient patient = GetPatient();
                if (patient == null)
                {
                    RefreshBasePage();
                    return;
                }

                byte[] c32DocumentBytes = fileC32Document.FileBytes;

                Result result = this.ValidateDocumentForPatient(fileC32Document.FileBytes, patient);

                if (!result.IsSuccess)
                {
                    DisplayMessage(result.ErrorMessage);
                }
                else
                {

                    UploadDocumentRequest uploadDocumentRequest = new UploadDocumentRequest();
                    UploadDocumentResponse uploadDocumentResponse = new UploadDocumentResponse();
                    SoapProperties soapProperties = new SoapProperties();

                    uploadDocumentRequest.CommunityId = MobiusAppSettingReader.LocalHomeCommunityID;
                    uploadDocumentRequest.DocumentId = Guid.NewGuid().ToString(); ;
                    uploadDocumentRequest.DocumentBytes = c32DocumentBytes;
                    uploadDocumentRequest.XACMLBytes = null;
                    uploadDocumentRequest.PatientId = patient.LocalMPIID;
                    uploadDocumentRequest.UploadedBy = "";
                    uploadDocumentRequest.RepositoryId = Helper.SourceRepositoryId;


                    List<NHINCommunity> nhinCommunitiesSession = (List<NHINCommunity>)GlobalSessions.SessionItem(SessionItem.CommunityList);
                    NHINCommunity homeNHINCommunity = nhinCommunitiesSession.Where(t => t.IsHomeCommunity).FirstOrDefault();
                    AssertionHelper assertionHelper = new AssertionHelper();
                    uploadDocumentRequest.Assertion = assertionHelper.CreateAssertion(AssertionMode.Default, AssertionAction.DocumentSubmission,
                        PurposeOfUse.TREATMENT, base.GetUserInformation(homeNHINCommunity), homeNHINCommunity);

                    soapHandler.RequestEncryption(uploadDocumentRequest, out soapProperties);
                    uploadDocumentRequest.SoapProperties = soapProperties;
                    //upload document on local server
                    uploadDocumentResponse = objProxy.UploadDocument(uploadDocumentRequest);
                    if (uploadDocumentResponse.Result.IsSuccess)
                    {
                        if (soapHandler.ResponseDecryption(uploadDocumentResponse.SoapProperties, uploadDocumentResponse))
                        {
                            GlobalSessions.SessionAdd(SessionItem.DocumentId, uploadDocumentRequest.DocumentId);
                            RefreshBasePage(true);
                        }
                        else
                        {
                            DisplayMessage(uploadDocumentResponse.Result.ErrorMessage);
                        }
                    }
                    else
                    {
                        DisplayMessage(uploadDocumentResponse.Result.ErrorMessage);
                    }
                }

            }
            else
            {
                DisplayMessage("Invalid document.");
            }
        }
        catch (Exception ex)
        {
            GlobalSessions.SessionAdd(SessionItem.ViewError, ex.Message);
            if (!string.IsNullOrEmpty(ex.Message) && ex.Message.Contains("C32"))
                DisplayMessage(ex.Message);
            else
                DisplayMessage(ex.Message);              
        }
    }

    private MobiusServiceLibrary.Patient GetPatient()
    {
        MobiusServiceLibrary.Patient patient = null;

        if (!string.IsNullOrEmpty(hdnPatient.Value))
        {
            patient = XmlSerializerHelper.DeserializeObject(hdnPatient.Value, typeof(MobiusServiceLibrary.Patient)) as MobiusServiceLibrary.Patient;

        }

        if (GlobalSessions.SessionItem(SessionItem.MPIID) != null && patient == null)
        {
            GetPatientDetailsRequest getPatientDetailsRequest = new GetPatientDetailsRequest();
            GetPatientDetailsResponse patientResponse = null;
            getPatientDetailsRequest.MPIID = GlobalSessions.SessionItem(SessionItem.MPIID).ToString();
            if (!string.IsNullOrWhiteSpace(getPatientDetailsRequest.MPIID))
            {
                soapHandler.RequestEncryption(getPatientDetailsRequest, out soapProperties);
                getPatientDetailsRequest.SoapProperties = soapProperties;
                patientResponse = objProxy.GetPatientDetails(getPatientDetailsRequest);
                if (patientResponse.Result.IsSuccess)
                {
                    if (!soapHandler.ResponseDecryption(patientResponse.SoapProperties, patientResponse))
                    {
                        DisplayMessage("Invalid response data.");
                    }
                    else
                    {
                        patient = patientResponse.Patient;
                        hdnPatient.Value = XmlSerializerHelper.SerializeObject(patient);
                    }
                }
                else
                {
                    DisplayMessage(patientResponse.Result.ErrorMessage);
                }
            }
        }

        return patient;
    }

    private void DisplayMessage(string message)
    {
        lblmessage.Text = message;
    }

    private void RefreshBasePage(bool displayAlert = false)
    {
        string strScript = "<script>";
        if (displayAlert)
            strScript += "alert('Document Uploaded Successfully.');";
        strScript += "window.opener.location='DocumentList.aspx?MODE=REFRESH';";
        strScript += "window.close();";
        strScript += "</script>";
        ClientScript.RegisterClientScriptBlock(Page.GetType(), "Upload", strScript);
    }

}
