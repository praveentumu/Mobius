// <copyright file="DocumentList.cs" company="R Systems">
// Copyright (c) R Systems International Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using FirstGenesis.UI;
using FirstGenesis.UI.Base;
using Mobius.CoreLibrary;
using Mobius.Entity;
using MobiusServiceLibrary;
using MobiusServiceUtility;
using System.Linq;
using System.Web;


public partial class DocumentList : BaseClass
{
    #region Variables
    SoapHandler soapHandler;
    MobiusClient objHISEProxy = new MobiusClient();
    SoapProperties soapProperties = new SoapProperties();
    GetMasterDataResponse getMasterDataResponse = null;
    GetMasterDataResponse getMasterDataResponseEmergency = null;
    private const string REFRESH = "REFRESH";
    private const string NO_DOCUMENT_FOUND = "No Document found.";
    private const string NO_PATIENT_SELECTED = "No Patient Selected.";
    #endregion
    /// <summary>
    /// Gets or sets a value indicating whether	grid has blank data row or not.
    /// </summary>
    public bool HasBlankRow { get; set; }


    /// <summary>
    /// This method is called to handle every page load event of this page.
    /// </summary>
    /// <param name="sender">grid object</param>
    /// <param name="e">grid event object</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            soapHandler = new SoapHandler(Convert.ToString(GlobalSessions.SessionItem(SessionItem.SerialNumber)));
            if ((GlobalSessions.SessionItem(SessionItem.ViewError) != null) && (GlobalSessions.SessionItem(SessionItem.ViewError).ToString() != string.Empty))
            {
                lblErrorMsg.Text = GlobalSessions.SessionItem(SessionItem.ViewError).ToString();
                GlobalSessions.SessionAdd(SessionItem.ViewError, string.Empty);
            }

            if (!Page.IsPostBack && Request.QueryString["MODE"] == REFRESH)
            {
                // Get Uploaded Document MetaData
                GetDocumentMetadataDocumentIDRequest getDocumentMetadataRequest = new GetDocumentMetadataDocumentIDRequest();
                getDocumentMetadataRequest.documentID = GlobalSessions.SessionItem(SessionItem.DocumentId).ToString();
                soapHandler.RequestEncryption(getDocumentMetadataRequest, out soapProperties);
                getDocumentMetadataRequest.SoapProperties = soapProperties;
                GetDocumentMetadataResponse getDocumentMetadataResponse = this.objProxy.GetDocumentMetaDataByDocumentId(getDocumentMetadataRequest);
                if (soapHandler.ResponseDecryption(getDocumentMetadataResponse.SoapProperties, getDocumentMetadataResponse))
                {
                    gridDocument.DataSource = getDocumentMetadataResponse.Documents;
                    gridDocument.DataBind();
                    FillNHINCommunities((List<MobiusServiceLibrary.PatientIdentifier>)GlobalSessions.SessionItem(SessionItem.PHISources));

                }
                else
                {
                    // Empty Grid bind
                    gridDocument.DataSource = this.BindBlankGrid();
                    gridDocument.DataBind();
                    lblErrorMsg.Text = INVALID_RESPONSE_DATA;
                    return;
                }
            }

            if (!Page.IsPostBack && Request.QueryString["MODE"] != REFRESH)
            {
                // Empty Grid bind
                gridDocument.DataSource = this.BindBlankGrid();
                gridDocument.DataBind();

                // List Box Population.
                PHISourceResponse patientCorrelationResponse = new PHISourceResponse();
                PHISourceRequest patientCorrelationRequest = new PHISourceRequest();
                patientCorrelationRequest.PatientId = GlobalSessions.SessionItem(SessionItem.MPIID).ToString();
                patientCorrelationRequest.AssigningCommunityId = MobiusAppSettingReader.LocalHomeCommunityID;
                patientCorrelationResponse = this.objProxy.GetPHISource(patientCorrelationRequest);

                if (soapHandler.ResponseDecryption(patientCorrelationResponse.SoapProperties, patientCorrelationResponse))
                {
                    if (patientCorrelationResponse.PatientIdentifiers != null)
                    {
                        GlobalSessions.SessionAdd(SessionItem.PHISources, patientCorrelationResponse.PatientIdentifiers);
                        this.FillNHINCommunities(patientCorrelationResponse.PatientIdentifiers);
                    }
                    else
                    {
                        lblErrorMsg.Text = patientCorrelationResponse.Result.ErrorMessage;
                    }
                }
                else
                {
                    lblErrorMsg.Text = INVALID_RESPONSE_DATA;
                    return;
                }

            }

            if (GlobalSessions.SessionItem(SessionItem.SelectedPatientName) != null)
            {
                lblPatientName.Text = string.Join(" ", (List<string>)GlobalSessions.SessionItem(SessionItem.SelectedPatientName)); ;
            }
            //if Page is loading first time and Emergency Reason data is null 
            if (!Page.IsPostBack && getMasterDataResponseEmergency == null)
                getMasterData(MasterCollection.EmergencyReason);

            lblEmergencyAccessTime.Text = MobiusAppSettingReader.EmergencyOverriddenTime.ToString();

        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }


    /// <summary>
    /// This  method will be called at  GetDocuments buttons's click.
    /// </summary>
    /// <param name="sender">grid event object</param>
    /// <param name="e">grid object</param>
    protected void BtnGetDocuments_Click(object sender, EventArgs e)
    {
        try
        {
            string patientID = string.Empty;
            lblErrorMsg.Text = string.Empty;
            GetDocumentMetadataResponse getDocumentsResponse = new GetDocumentMetadataResponse();
            GetDocumentMetadataRequest getDocumentMetadataRequest = new GetDocumentMetadataRequest();
            List<Community> nhinCommunities = new List<Community>();
            Community nhinCommunity;
            getDocumentMetadataRequest.communities = new List<Community>();

            for (int i = 1; i < lbCommunities.Items.Count; i++)
            {
                if (lbCommunities.Items[i].Selected)
                {
                    nhinCommunity = new Community();
                    nhinCommunity.CommunityIdentifier = lbCommunities.Items[i].Value;
                    getDocumentMetadataRequest.communities.Add(nhinCommunity);
                }
            }

            if (!string.IsNullOrWhiteSpace((string)GlobalSessions.SessionItem(SessionItem.MPIID)))
            {
                patientID = (string)GlobalSessions.SessionItem(SessionItem.MPIID);
            }

            if (patientID != string.Empty)
            {
                getDocumentMetadataRequest.patientId = patientID;
                getDocumentMetadataRequest.GetLocallyAvailable = chkLocalDocuments.Checked;
                AssertionHelper assertionHelper = new AssertionHelper();

                List<NHINCommunity> community = (List<NHINCommunity>)GlobalSessions.SessionItem(SessionItem.CommunityList);
                NHINCommunity homeNHINCommunity = community.Where(t => t.IsHomeCommunity).FirstOrDefault();
                getDocumentMetadataRequest.Assertion = assertionHelper.CreateAssertion(AssertionMode.Default, AssertionAction.DocumentQuery,
                    PurposeOfUse.TREATMENT, base.GetUserInformation(homeNHINCommunity), homeNHINCommunity);


                soapHandler.RequestEncryption(getDocumentMetadataRequest, out soapProperties);
                getDocumentMetadataRequest.SoapProperties = soapProperties;
                getDocumentsResponse = this.objProxy.GetDocumentMetadata(getDocumentMetadataRequest);
                if (getDocumentsResponse.Result.IsSuccess)
                {
                    if (soapHandler.ResponseDecryption(getDocumentsResponse.SoapProperties, getDocumentsResponse))
                    {

                        getMasterData(MasterCollection.PurposeOfUse);
                        // Populate records in the grid.
                        if (getDocumentsResponse.Result.IsSuccess && getDocumentsResponse.Documents.Count > 0)
                        {
                            GlobalSessions.SessionAdd(SessionItem.DocInfo, getDocumentsResponse.Documents);

                            gridDocument.DataSource = getDocumentsResponse.Documents; // this.GetDocumentFromDOD(docList);
                            gridDocument.DataBind();
                        }
                        else
                        {
                            lblErrorMsg.Text = NO_DOCUMENT_FOUND;
                        }

                    }
                    else
                    {
                        lblErrorMsg.Text = INVALID_RESPONSE_DATA;
                        return;
                    }
                }
                else
                {
                    gridDocument.DataSource = this.BindBlankGrid();
                    gridDocument.DataBind();
                    lblErrorMsg.Text = getDocumentsResponse.Result.ErrorMessage;
                    return;
                }
            }
            else
            {
                lblErrorMsg.Text = NO_PATIENT_SELECTED;
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }



    /// <summary>
    /// Handles the RowDataBound event of the gridDocument control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> instance containing the event data.</param>
    protected void GridDocument_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                gridDocument.Columns[5].Visible = false;

                if (GlobalSessions.SessionItem(SessionItem.UserType).ToString() == "Patient")
                    gridDocument.Columns[7].Visible = gridDocument.Columns[8].Visible = gridDocument.Columns[11].Visible = false;

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (!this.HasBlankRow)
                {
                    ImageButton ibtnEdit = (ImageButton)e.Row.FindControl("ibtnEdit");
                    ImageButton ibtnCustomEdit = (ImageButton)e.Row.FindControl("ibtnCustomEdit");
                    ImageButton ibtnView = (ImageButton)e.Row.FindControl("ibtnView");
                    ImageButton ibtnPolicy = (ImageButton)e.Row.FindControl("ibtnPolicy");
                    DropDownList ddlPurposeForView = (DropDownList)e.Row.FindControl("ddlPurposeForView");

                    ibtnEdit.Visible = true;
                    ibtnCustomEdit.Visible = true;
                    ibtnView.Visible = true;
                    ddlPurposeForView.Visible = true;
                    ibtnPolicy.Visible = true;

                    if (ibtnPolicy != null)
                    {
                        ibtnPolicy.CommandName = "Policy";
                        ibtnPolicy.CommandArgument = e.Row.RowIndex.ToString();
                    }

                    if (ibtnEdit != null)
                    {
                        ibtnEdit.CommandName = "Refer";
                        ibtnEdit.CommandArgument = e.Row.RowIndex.ToString();
                    }

                    if (ibtnCustomEdit != null)
                    {
                        ibtnCustomEdit.CommandName = "CustomEdit";
                        ibtnCustomEdit.CommandArgument = e.Row.RowIndex.ToString();
                    }

                    if (ibtnView != null)
                    {
                        ibtnView.CommandName = "View";
                        ibtnView.CommandArgument = e.Row.RowIndex.ToString();
                    }
                    if (!string.IsNullOrWhiteSpace(e.Row.Cells[3].Text) && e.Row.Cells[3].Text != "&nbsp;")
                    {
                        DateTime dateTime;
                        string[] format = { "yyyyMMddHHmmss", "yyyyMMddHHmmsszzz" };
                        string date = e.Row.Cells[3].Text;
                        if (DateTime.TryParseExact(date, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dateTime))
                        {
                            date = Convert.ToString(dateTime);
                            e.Row.Cells[3].Text = date;
                        }



                    }

                    if (ddlPurposeForView != null)
                        fillPurposeForView(ddlPurposeForView);

                    if (((Document)(e.Row.DataItem)).DataSource == DocumentSource.Local.ToString())
                    {
                        ibtnEdit.Visible = ibtnCustomEdit.Visible = true;
                    }
                    else
                    {
                        ibtnCustomEdit.Visible = false;
                    }

                    if (((Document)(e.Row.DataItem)).XACMLDocumentId != "")
                    {
                        ibtnPolicy.Visible = true;
                    }
                    else
                    {
                        ibtnPolicy.Visible = false;
                    }


                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    private void fillPurposeForView(DropDownList ddlPurposeForView)
    {
        if (getMasterDataResponse == null)
        {
            getMasterData(MasterCollection.PurposeOfUse);
        }
        FillDropDown(ddlPurposeForView, getMasterDataResponse.MasterDataCollection);
    }


    /// <summary>
    /// This method will do the row editing.
    /// </summary>
    /// <param name="sender">grid object</param>
    /// <param name="e">grid event object</param>
    protected void GridDocument_RowEditing(object sender, GridViewEditEventArgs e)
    {
    }

    /// <summary>
    /// This method is called when page index is changed.
    /// </summary>
    /// <param name="sender">grid object</param>
    /// <param name="e">grid event object</param>
    protected void GridDocument_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            lblErrorMsg.Text = string.Empty;
            if (GlobalSessions.SessionItem(SessionItem.DocInfo) != null)
            {
                gridDocument.DataSource = (List<Document>)GlobalSessions.SessionItem(SessionItem.DocInfo);
                gridDocument.PageIndex = e.NewPageIndex;
                gridDocument.DataBind();
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    /// <summary>
    /// This method is called when page index is changed. 
    /// </summary>
    /// <param name="sender">grid object</param>
    /// <param name="e">grid event object.</param>
    protected void GridDocument_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            hdnErrorMessage.Value = "";
            GlobalSessions.SessionAdd(SessionItem.SelectedValues, null);
            if (e.CommandName == "Refer")
            {
                int iselectedIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow selRow = gridDocument.Rows[iselectedIndex];
                string selectedPatientID = GlobalSessions.SessionItem(SessionItem.MPIID) != null ? (!string.IsNullOrEmpty(GlobalSessions.SessionItem(SessionItem.MPIID).ToString()) ? GlobalSessions.SessionItem(SessionItem.MPIID).ToString() : null) : null;

                string strDocumentId = ((Label)selRow.FindControl("lblOriginaldocumentID")).Text;

                GetDocumentResponse getDocumentResponse = new GetDocumentResponse();
                GetDocumentRequest getDocumentRequest = new GetDocumentRequest();
                getDocumentRequest.patientId = selectedPatientID;
                getDocumentRequest.documentId = strDocumentId;
                getDocumentRequest.purpose = "TREATMENT";
                getDocumentRequest.subjectRole = Helper.Patient;
                getDocumentRequest.subjectEmailID = this.EmailAddress;

                List<NHINCommunity> community = (List<NHINCommunity>)GlobalSessions.SessionItem(SessionItem.CommunityList);
                NHINCommunity homeNHINCommunity = community.Where(t => t.IsHomeCommunity).FirstOrDefault();

                AssertionHelper assertion = new AssertionHelper();
                getDocumentRequest.Assertion = assertion.CreateAssertion(AssertionMode.Default, AssertionAction.DocumentQuery,
                PurposeOfUse.TREATMENT, base.GetUserInformation(homeNHINCommunity), homeNHINCommunity);


                // Following code has been added as a HACK to complete the Patient Referral demo, as retrieving and consent policy setting 
                // currently works on provider role and NOT on emailID, while Refer Patient cycle runs on email.
                soapHandler.RequestEncryption(getDocumentRequest, out soapProperties);
                getDocumentRequest.SoapProperties = soapProperties;


                getDocumentResponse = this.objProxy.GetDocument(getDocumentRequest);
                if (getDocumentResponse.Result.IsSuccess)
                {
                    if (soapHandler.ResponseDecryption(getDocumentResponse.SoapProperties, getDocumentResponse))
                    {
                        Document docData = getDocumentResponse.Document;
                        if (docData.DocumentBytes == null)
                        {
                            hdnErrorMessage.Value = getDocumentResponse.Result.ErrorMessage;
                        }
                        else
                        {
                            Response.Redirect("ReferPatient.aspx?DocumentID=" + strDocumentId,false);
                        }
                    }
                    else
                    {
                        lblErrorMsg.Text = INVALID_RESPONSE_DATA;
                        return;
                    }
                }
                else
                {
                    lblErrorMsg.Text = getDocumentResponse.Result.ErrorMessage;
                }

            }

            if (e.CommandName == "CustomEdit")
            {


                int iselectedIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow selRow = gridDocument.Rows[iselectedIndex];
                string selectedPatientID = GlobalSessions.SessionItem(SessionItem.MPIID) != null ? (!string.IsNullOrEmpty(GlobalSessions.SessionItem(SessionItem.MPIID).ToString()) ? GlobalSessions.SessionItem(SessionItem.MPIID).ToString() : null) : null;

                string strDocumentId = ((Label)selRow.FindControl("lblOriginaldocumentID")).Text;

                GetDocumentResponse getDocumentResponse = new GetDocumentResponse();
                GetDocumentRequest getDocumentRequest = new GetDocumentRequest();
                getDocumentRequest.patientId = selectedPatientID;
                getDocumentRequest.documentId = strDocumentId;
                getDocumentRequest.purpose = "TREATMENT";
                getDocumentRequest.subjectRole = Helper.Patient;
                getDocumentRequest.subjectEmailID = this.EmailAddress;
                
                List<NHINCommunity> community = (List<NHINCommunity>)GlobalSessions.SessionItem(SessionItem.CommunityList);
                NHINCommunity homeNHINCommunity = community.Where(t => t.IsHomeCommunity).FirstOrDefault();

                AssertionHelper assertion = new AssertionHelper();
                getDocumentRequest.Assertion = assertion.CreateAssertion(AssertionMode.Default, AssertionAction.DocumentQuery,
                PurposeOfUse.TREATMENT, base.GetUserInformation(homeNHINCommunity), homeNHINCommunity);


                // Following code has been added as a HACK to complete the Patient Referral demo, as retrieving and consent policy setting 
                // currently works on provider role and NOT on emailID, while Refer Patient cycle runs on email.
                soapHandler.RequestEncryption(getDocumentRequest, out soapProperties);
                getDocumentRequest.SoapProperties = soapProperties;


                getDocumentResponse = this.objProxy.GetDocument(getDocumentRequest);
                if (getDocumentResponse.Result.IsSuccess)
                {
                    if (soapHandler.ResponseDecryption(getDocumentResponse.SoapProperties, getDocumentResponse))
                    {
                        Document docData = getDocumentResponse.Document;
                        if (docData.DocumentBytes == null)
                        {
                            hdnErrorMessage.Value = getDocumentResponse.Result.ErrorMessage;
                        }
                        else
                        {
                            Response.Redirect("SharePatientDocument.aspx?DocumentID=" + strDocumentId, false);
                        }
                    }
                    else
                    {
                        lblErrorMsg.Text = INVALID_RESPONSE_DATA;
                        return;
                    }
                }
                else
                {
                    lblErrorMsg.Text = getDocumentResponse.Result.ErrorMessage;
                }


            }

            if (e.CommandName == "View")
            {
                int iselectedIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow selRow = gridDocument.Rows[iselectedIndex];
                string strDocumentId = ((Label)selRow.FindControl("lblOriginaldocumentID")).Text;
                int selectedPurposeValue = Convert.ToInt32(((DropDownList)selRow.FindControl("ddlPurposeForView")).SelectedValue);
                string subject = GlobalSessions.SessionItem(SessionItem.UserRole) != null ? GlobalSessions.SessionItem(SessionItem.UserRole) as string : string.Empty;
                if (selectedPurposeValue == 0 && subject != Helper.Patient)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(DocumentList), "DocumentList", "alert('Please select purpose.');", true);
                    return;
                }

                string selectedPurpose = ((DropDownList)selRow.FindControl("ddlPurposeForView")).SelectedItem.Text.ToString();
                string selectedPatientID = GlobalSessions.SessionItem(SessionItem.MPIID) != null ? (!string.IsNullOrEmpty(GlobalSessions.SessionItem(SessionItem.MPIID).ToString()) ? GlobalSessions.SessionItem(SessionItem.MPIID).ToString() : null) : null;


                GetDocumentResponse getDocumentResponse = new GetDocumentResponse();
                GetDocumentRequest getDocumentRequest = new GetDocumentRequest();
                getDocumentRequest.patientId = selectedPatientID;
                getDocumentRequest.documentId = strDocumentId;
                getDocumentRequest.purpose = selectedPurpose;
                getDocumentRequest.subjectRole = subject;
                getDocumentRequest.subjectEmailID = this.EmailAddress;
                getDocumentRequest.LocalData = chkLocalDocuments.Checked;

                PurposeOfUse PurposeOfUse;
                Enum.TryParse(selectedPurpose, out PurposeOfUse);
                if (PurposeOfUse.GetHashCode() == 0)
                    PurposeOfUse = PurposeOfUse.REQUEST;

                if (PurposeOfUse == Mobius.CoreLibrary.PurposeOfUse.EMERGENCY)
                {
                    EmergencyAccess emergencyAccess = new EmergencyAccess();
                    emergencyAccess.Status = true;
                    getDocumentRequest.EmergencyAccess = emergencyAccess;
                }

                List<NHINCommunity> community = (List<NHINCommunity>)GlobalSessions.SessionItem(SessionItem.CommunityList);

                NHINCommunity homeNHINCommunity = community.Where(t => t.IsHomeCommunity).FirstOrDefault();

                AssertionHelper assertion = new AssertionHelper();
                getDocumentRequest.Assertion = assertion.CreateAssertion(AssertionMode.Default, AssertionAction.DocumentRetrieve,
                PurposeOfUse, base.GetUserInformation(homeNHINCommunity), homeNHINCommunity);

                soapHandler.RequestEncryption(getDocumentRequest, out soapProperties);
                getDocumentRequest.SoapProperties = soapProperties;

                getDocumentResponse = this.objProxy.GetDocument(getDocumentRequest);
                if (getDocumentResponse.Result.IsSuccess)
                {
                    if (soapHandler.ResponseDecryption(getDocumentResponse.SoapProperties, getDocumentResponse))
                    {
                        Document docData = getDocumentResponse.Document;

                        if (docData.DocumentBytes != null)
                        {
                            GlobalSessions.SessionAdd(SessionItem.XMLDOC, docData.DocumentBytes);
                            ScriptManager.RegisterStartupScript(this, typeof(DocumentList), "ViewDocument", "HideSearchIcon();openpopupInFullScreen('ViewDoDDocument.aspx','Document');", true);

                        }
                        else if (docData.DocumentBytes == null)
                        {
                            hdnErrorMessage.Value = getDocumentResponse.Result.ErrorMessage;
                        }
                    }
                    else
                    {
                        lblErrorMsg.Text = INVALID_RESPONSE_DATA;
                        return;
                    }
                }
                else
                {

                    GlobalSessions.SessionAdd(SessionItem.DocumentId, strDocumentId);
                    if (getDocumentResponse.Result.ErrorCode == Mobius.CoreLibrary.ErrorCode.Emergency_Override_Case)
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "EmergencyDialoge", "OpenEmergencyDialoge()", true);
                    else
                        hdnErrorMessage.Value = Helper.GetErrorMessage(getDocumentResponse.Result.ErrorCode);

                }
            }

            //if request for view policy file
            if (e.CommandName == "Policy")
            {
                string selectedPurpose = "";
                int iselectedIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow selRow = gridDocument.Rows[iselectedIndex];
                string strDocumentId = ((Label)selRow.FindControl("lblOriginaldocumentID")).Text;
                int selectedPurposeValue = Convert.ToInt32(((DropDownList)selRow.FindControl("ddlPurposeForView")).SelectedValue);
                string subject = GlobalSessions.SessionItem(SessionItem.UserRole) != null ? GlobalSessions.SessionItem(SessionItem.UserRole) as string : string.Empty;
                if (selectedPurposeValue == 0 && subject != Helper.Patient)
                {
                    //if no purpose is selected then Set purpose anything to avoid the validation of controller.
                    selectedPurpose = "TREATMENT";
                }
                else
                {
                    selectedPurpose = ((DropDownList)selRow.FindControl("ddlPurposeForView")).SelectedItem.Text.ToString();
                }
                string selectedPatientID = GlobalSessions.SessionItem(SessionItem.MPIID) != null ? (!string.IsNullOrEmpty(GlobalSessions.SessionItem(SessionItem.MPIID).ToString()) ? GlobalSessions.SessionItem(SessionItem.MPIID).ToString() : null) : null;

                GetDocumentResponse getDocumentResponse = new GetDocumentResponse();
                GetDocumentRequest getDocumentRequest = new GetDocumentRequest();
                getDocumentRequest.patientId = selectedPatientID;
                getDocumentRequest.documentId = strDocumentId;
                getDocumentRequest.purpose = selectedPurpose;
                getDocumentRequest.subjectRole = subject;
                getDocumentRequest.subjectEmailID = this.EmailAddress;
                getDocumentRequest.LocalData = chkLocalDocuments.Checked;


                PurposeOfUse PurposeOfUse;
                Enum.TryParse(selectedPurpose, out PurposeOfUse);
                if (PurposeOfUse.GetHashCode() == 0)
                    PurposeOfUse = PurposeOfUse.REQUEST;

                List<NHINCommunity> community = (List<NHINCommunity>)GlobalSessions.SessionItem(SessionItem.CommunityList); ;

                NHINCommunity homeNHINCommunity = community.Where(t => t.IsHomeCommunity).FirstOrDefault();

                AssertionHelper assertion = new AssertionHelper();
                getDocumentRequest.Assertion = assertion.CreateAssertion(AssertionMode.Default, AssertionAction.DocumentRetrieve,
                PurposeOfUse, base.GetUserInformation(homeNHINCommunity), homeNHINCommunity);

                soapHandler.RequestEncryption(getDocumentRequest, out soapProperties);
                getDocumentRequest.SoapProperties = soapProperties;

                getDocumentResponse = this.objProxy.GetDocument(getDocumentRequest);
                //if 
                if (getDocumentResponse.Result.IsSuccess || (getDocumentResponse.Document.XACMLBytes != null))
                {
                    if (soapHandler.ResponseDecryption(getDocumentResponse.SoapProperties, getDocumentResponse))
                    {
                        Document docData = getDocumentResponse.Document;
                        if (docData.XACMLBytes != null)
                        {
                            GlobalSessions.SessionAdd(SessionItem.XACMLBytes, docData.XACMLBytes);
                            ScriptManager.RegisterStartupScript(this, typeof(DocumentList), "ViewDocument", "window.open('ViewFile.aspx')", true);

                        }
                        else
                        {
                            hdnErrorMessage.Value = Helper.GetErrorMessage(ErrorCode.FileNotFound);
                        }
                    }
                    else
                    {
                        hdnErrorMessage.Value = Helper.GetErrorMessage(ErrorCode.FileNotFound);
                        return;
                    }
                }
                else
                {
                    hdnErrorMessage.Value = Helper.GetErrorMessage(ErrorCode.FileNotFound);
                }
            }


        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    /// <summary>
    /// This method fills the NHIN communities.
    /// </summary>
    /// <param name="NHINCommunities">List of NHINCommunities.</param>
    private void FillNHINCommunities(List<MobiusServiceLibrary.PatientIdentifier> NHINCommunities)
    {
        if (NHINCommunities == null)
        {
            NHINCommunities = new List<MobiusServiceLibrary.PatientIdentifier>();
        }


        MobiusServiceLibrary.PatientIdentifier NHINCommunity = new MobiusServiceLibrary.PatientIdentifier();
        if (NHINCommunities.Find(delegate(MobiusServiceLibrary.PatientIdentifier e) { return e.CommunityDescription == "Check All"; }) == null)
        {
            NHINCommunity.CommunityDescription = "Check All";
        }
        else
        {
            int index = NHINCommunities.FindIndex(s => s.CommunityDescription == "Check All");
            if (index >= 0)
            {
                NHINCommunities.RemoveAt(index);
                NHINCommunity.CommunityDescription = "Check All";
            }
        }
        NHINCommunity.CommunityIdentifier = "-1";
        NHINCommunities.Insert(0, NHINCommunity);
        lbCommunities.DataSource = NHINCommunities;
        lbCommunities.DataTextField = "CommunityDescription";
        lbCommunities.DataValueField = "CommunityIdentifier";
        lbCommunities.DataBind();
        NHINCommunities.RemoveAt(0);

    }

    /// <summary>
    /// Method to get the data for dropdowns
    /// </summary>
    /// <param name="dropdown"></param>
    private void getMasterData(MasterCollection dropdown)
    {
        GetMasterDataRequest getMasterDataRequest = new GetMasterDataRequest();
        getMasterDataRequest.MasterCollection = dropdown;
        getMasterDataRequest.dependedValue = 0;

        if (dropdown == MasterCollection.PurposeOfUse)
        {
            // Fill the purpose of use dropdown list.
            getMasterDataResponse = new GetMasterDataResponse();
            getMasterDataResponse = objHISEProxy.GetMasterData(getMasterDataRequest);
        }
        else if (dropdown == MasterCollection.EmergencyReason)
        {
            // Fill the purpose of use dropdown list.
            getMasterDataResponseEmergency = new GetMasterDataResponse();
            getMasterDataResponseEmergency = objHISEProxy.GetMasterData(getMasterDataRequest);
            FillDropDown(ddlEmergencyReasons, getMasterDataResponseEmergency.MasterDataCollection);
        }
    }


    private List<Document> BindBlankGrid()
    {
        List<Document> documents = new List<Document>();
        Document document = new Document();
        document.DocumentTitle = document.DocumentUniqueId = "";
        documents.Add(document);
        this.HasBlankRow = true;
        return documents;
    }

    protected void btnSubmitEmergency_Click(object sender, EventArgs e)
    {
        try
        {


            string selectedPatientID = GlobalSessions.SessionItem(SessionItem.MPIID) != null ? (!string.IsNullOrEmpty(GlobalSessions.SessionItem(SessionItem.MPIID).ToString()) ? GlobalSessions.SessionItem(SessionItem.MPIID).ToString() : null) : null;

            GetDocumentRequest getDocumentRequest = new GetDocumentRequest();
            getDocumentRequest.patientId = selectedPatientID;
            getDocumentRequest.documentId = GlobalSessions.SessionItem(SessionItem.DocumentId).ToString();
            getDocumentRequest.purpose = "EMERGENCY";
            getDocumentRequest.subjectRole = GlobalSessions.SessionItem(SessionItem.UserRole) != null ? GlobalSessions.SessionItem(SessionItem.UserRole) as string : string.Empty;
            getDocumentRequest.subjectEmailID = this.EmailAddress;

            PurposeOfUse purposeOfUse;
            Enum.TryParse("EMERGENCY", out purposeOfUse);
            if (purposeOfUse.GetHashCode() == 0)
                purposeOfUse = PurposeOfUse.REQUEST;
            List<NHINCommunity> community = (List<NHINCommunity>)GlobalSessions.SessionItem(SessionItem.CommunityList);
            NHINCommunity homeNHINCommunity = community.Where(t => t.IsHomeCommunity).FirstOrDefault();
            AssertionHelper assertion = new AssertionHelper();
            getDocumentRequest.Assertion = assertion.CreateAssertion(AssertionMode.Default, AssertionAction.DocumentRetrieve,
            purposeOfUse, base.GetUserInformation(homeNHINCommunity), homeNHINCommunity);

            EmergencyAccess emergency = new EmergencyAccess();

            emergency.OverrideReason = (OverrideReason)Enum.Parse(typeof(OverrideReason), ddlEmergencyReasons.SelectedValue, true);
            emergency.Description = txtDescription.Text;
            emergency.OverrideDate = DateTime.Now;
            emergency.OverriddenBy = this.EmailAddress;
            emergency.Status = true;

            getDocumentRequest.EmergencyAccess = emergency;


            MobiusSecuredClient objProxy = new MobiusSecuredClient();
            GetDocumentResponse getDocumentResponse = this.objProxy.GetDocument(getDocumentRequest);
            if (getDocumentResponse.Result.IsSuccess)
            {

                Document docData = getDocumentResponse.Document;
                if (docData.DocumentBytes != null)
                {
                    GlobalSessions.SessionAdd(SessionItem.XMLDOC, docData.DocumentBytes);
                    ScriptManager.RegisterStartupScript(this, typeof(DocumentList), "ViewDocument", "openpopupInFullScreen('ViewDoDDocument.aspx','Document');ResetEmergencyDialog();", true);
                }
                else if (docData.DocumentBytes == null)
                {
                    lblErrorMsg.Text = Helper.GetErrorMessage(getDocumentResponse.Result.ErrorCode);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "EmergencyDialoge", "LightenMasterPage();ResetEmergencyDialog();", true);
                }

            }
            else
            {
                lblErrorMsg.Text = Helper.GetErrorMessage(getDocumentResponse.Result.ErrorCode);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "EmergencyDialoge", "LightenMasterPage();ResetEmergencyDialog();", true);
            }
        }

        catch (Exception ex)
        {
            lblErrorMsg.Text = Helper.GetErrorMessage(ErrorCode.Error_in_emergency_access);
            ExceptionHelper.HandleException(page: Page, ex: ex);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "EmergencyDialoge", "LightenMasterPage();ResetEmergencyDialog();", true);
        }
    }

}

