#region Namespaces
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using C32Utility;
using FirstGenesis.Mobius.Server.MobiusHISEService.XACML;
using FirstGenesis.UI.Base;
using Mobius.CoreLibrary;
using MobiusServiceLibrary;
using FirstGenesis.UI;
using Mobius.Entity;
using System.Linq.Expressions;
using System.Linq;
using Mobius.FileSystem;
using MobiusServiceUtility;

#endregion

public partial class SharePatientDocument : BaseClass
{
    #region Variables
    string documentId = string.Empty;
    string providerValue = string.Empty;
    byte[] documentBytes = null;
    byte[] XACMLBytes = null;
    string strUploadPath = string.Empty;
    string strRetrievePath = string.Empty;
    string strSharedPath = string.Empty;
    string strUserID = string.Empty;
    string strMode = string.Empty;
    string strUserGUID = string.Empty;
    MobiusClient objHISEProxy = new MobiusClient();
    string docTitle = string.Empty;
    string location = string.Empty;
    private static string userRole = string.Empty;
    private static CDAHelper CDAHelper;
    private const string DOCUMENT_SHARED = "Document shared successfully.";
    private const string DEFAULT_PAGE = "Default.aspx";
    private const string COMMUNITY_DESCRIPTION = "CommunityDescription";
    private const string COMMUNITY_IDENTIFIER = "CommunityIdentifier";
    protected const string INFORMATION_SOURCE = "Information Source";
    protected const string LANGUAGE_SPOKEN = "Language Spoken";
    protected const string PERSON_INFORMATION = "Person Information";
    protected const string SUPPORT = "Support";
    protected const string SUMMARY_PURPOSE = "Summary Purpose";
    protected const string CHECK_ALL = "Check All";

    GetMasterDataResponse getMasterDataResponse = null;
    GetMasterDataRequest getMasterDataRequest = null;
    SoapHandler soapHandler;
    SoapProperties soapProperties = new SoapProperties();
    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            soapHandler = new SoapHandler(Convert.ToString(GlobalSessions.SessionItem(SessionItem.SerialNumber)));

            if (!string.IsNullOrEmpty(Request.QueryString["DocumentID"]))
            {
                documentId = Request.QueryString["DocumentID"];
            }
            else
            {
                Response.Redirect(DEFAULT_PAGE, false);
            }

            if (Request.QueryString["MODE"] != null)
            {
                strMode = Request.QueryString["MODE"].ToString();
            }
            if (!Page.IsPostBack)
            {
                trProvideRole.Visible = false;
                trProvideEmail.Visible = true;
                getMasterDataResponse = new GetMasterDataResponse();
                getMasterDataRequest = new GetMasterDataRequest();
                getMasterDataRequest.MasterCollection = MasterCollection.UserRole;
                getMasterDataRequest.dependedValue = 0;
                getMasterDataResponse = objHISEProxy.GetMasterData(getMasterDataRequest);
                if (getMasterDataResponse.Result.IsSuccess)
                {
                    FillDropDown(ddlProviderRole, getMasterDataResponse.MasterDataCollection);

                }
                getMasterDataResponse = new GetMasterDataResponse();
                getMasterDataRequest = new GetMasterDataRequest();
                getMasterDataRequest.MasterCollection = MasterCollection.PurposeOfUse;
                getMasterDataRequest.dependedValue = 0;
                getMasterDataResponse = objHISEProxy.GetMasterData(getMasterDataRequest);
                if (getMasterDataResponse.Result.IsSuccess)
                {
                    FillDropDown(ddlPurposeForUse, getMasterDataResponse.MasterDataCollection);

                }
                getMasterDataResponse = new GetMasterDataResponse();
                FillNHINCommunities();

                GetDocumentMetadataDocumentIDRequest getDocumentMetadataDocumentIDRequest = new GetDocumentMetadataDocumentIDRequest();
                getDocumentMetadataDocumentIDRequest.documentID = documentId;
                // Encryption
                soapHandler.RequestEncryption(getDocumentMetadataDocumentIDRequest, out soapProperties);
                getDocumentMetadataDocumentIDRequest.SoapProperties = soapProperties;
                GetDocumentMetadataResponse documentMetadataResponse = objProxy.GetDocumentMetaDataByDocumentId(getDocumentMetadataDocumentIDRequest);
                if (documentMetadataResponse.Result.IsSuccess)
                {
                    if (soapHandler.ResponseDecryption(documentMetadataResponse.SoapProperties, documentMetadataResponse))
                    {
                        if (documentMetadataResponse.Result.IsSuccess)
                        {
                            BindSharedWithGrid(documentId);
                            if (documentMetadataResponse.Documents.Count() > 0)
                            {
                                BindDocumentGrid(documentMetadataResponse.Documents);

                                docTitle = documentMetadataResponse.Documents[0].DocumentTitle;
                                location = Path.Combine(MobiusAppSettingReader.DocumentPath, documentMetadataResponse.Documents[0].Location);
                                ViewState["Location"] = location;
                                if (!string.IsNullOrEmpty(docTitle))
                                {
                                    ViewState["docTitle"] = docTitle;
                                }
                            }
                        }

                        SetControlValues(documentId, docTitle);
                    }
                    else
                    {
                        lblmessage.Text = INVALID_RESPONSE_DATA;
                        return;
                    }
                }
            }
            if (string.IsNullOrEmpty(lblPatientName.Text) && GlobalSessions.SessionItem(SessionItem.SelectedPatientName) != null)
            {
                lblPatientName.Text = string.Join(" ", (List<string>)GlobalSessions.SessionItem(SessionItem.SelectedPatientName));

            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    #region Document Metadata Grid operation
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dsDocument"></param>
    private void BindDocumentGrid(List<Document> documents)
    {
        for (int i = 0; i < documents.Count; i++)
        {
            string date = Convert.ToString(DateTime.ParseExact(documents[i].CreatedOn.ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("MM/dd/yyyy"));
            documents[i].CreatedOn = date;
        }

        gridDocument.DataSource = documents;
        gridDocument.DataBind();
    }

    #endregion

    #region SetControlValues
    /// <summary>
    /// 
    /// </summary>
    /// <param name="documentId"></param>
    /// <param name="docTitle"></param>
    private void SetControlValues(string documentId, string docTitle)
    {
        string strFileName = "";
        strFileName = documentId + ".xml";
        txtDocumentId.Text = documentId;
        //txtDocumentTitle.Text = docTitle;
        txtDocumentTitle.Text = ViewState["docTitle"].ToString();
        userRole = GlobalSessions.SessionItem(SessionItem.UserRole) != null ? GlobalSessions.SessionItem(SessionItem.UserRole) as string : string.Empty;

        MobiusServiceLibrary.Patient patient = GlobalSessions.SessionItem(SessionItem.SelectedPatient) as MobiusServiceLibrary.Patient;
        if (patient != null)
        {

            lblgender.Text = patient.Gender.ToString();// == "M" ? "Male" : "Female";
            DateTime myDate = ConvertDate(patient.DOB, "yyyyMMdd");
            lblDOB.Text = myDate.ToShortDateString();

           string name = "";
            if (patient.Prefix!=null && patient.Prefix.Count>0 &&  !string.IsNullOrWhiteSpace(patient.Prefix.First()))
                name = patient.Prefix.First() + " ";
            if (patient.GivenName != null && patient.GivenName.Count > 0 && !string.IsNullOrWhiteSpace(patient.GivenName.First()))
                name = name + patient.GivenName.First() + " ";
            if (patient.MiddleName != null && patient.MiddleName.Count > 0 && !string.IsNullOrWhiteSpace(patient.MiddleName.First()))
                name = name + patient.MiddleName.First() + " ";
            if (patient.FamilyName != null && patient.FamilyName.Count > 0 && !string.IsNullOrWhiteSpace(patient.FamilyName.First()))
                name = name + patient.FamilyName.First() + " ";
            if (patient.Suffix != null && patient.Suffix.Count > 0 && !string.IsNullOrWhiteSpace(patient.Suffix.First()))
                name = name + patient.Suffix.First();

            lblPatientName.Text = name;


        }
        else if (userRole == Helper.Patient)
        {
            UserInformation userInfo = (MobiusServiceLibrary.UserInformation)(GlobalSessions.SessionItem(SessionItem.UserInformation));
            if (userInfo.Name != null)
            {
                string name = "";
                if (!string.IsNullOrWhiteSpace(userInfo.Name.Prefix))
                    name = userInfo.Name.Prefix + " ";
                if (!string.IsNullOrWhiteSpace(userInfo.Name.GivenName))
                    name = name + userInfo.Name.GivenName + " ";
                if (!string.IsNullOrWhiteSpace(userInfo.Name.MiddleName))
                    name = name + userInfo.Name.MiddleName + " ";
                if (!string.IsNullOrWhiteSpace(userInfo.Name.FamilyName))
                    name = name + userInfo.Name.FamilyName + " ";
                if (!string.IsNullOrWhiteSpace(userInfo.Name.Suffix))
                    name = name + userInfo.Name.Suffix + " ";

                lblPatientName.Text = name;

            }

            trGender.Visible = false;
            trDOB.Visible = false;
        }


        //Commented
        //lbAvalSections.DataSource = c32data.GetC32Sections(Path.Combine(location, strFileName));
        //Ends commented

        //Added to not showing mandatory list in available sections
        List<string> listWithoutMandatorySections = new List<string>();
        List<string> listMandatorySections = new List<string>();

        CDAHelper = new CDAHelper(FileHandler.LoadDocument(documentId, ViewState["Location"].ToString()));

        if (string.IsNullOrEmpty(lblgender.Text) && CDAHelper.PatientGender != null)
        {
            lblgender.Text = CDAHelper.PatientGender == "M" ? "Male" : CDAHelper.PatientGender == "F" ? "Female" : "";
            trGender.Visible = true;
        }
        if (string.IsNullOrEmpty(lblDOB.Text) && CDAHelper.PatientDOB != null)
        {
            lblDOB.Text = ConvertDate(CDAHelper.PatientDOB, "yyyyMMdd").ToShortDateString();
            trDOB.Visible = true;
        }

        if(CDAHelper.Sections != null && CDAHelper.Sections.Count > 0)
        {
            //List of mendatory sections whose Template Ids are available
            List<string> listMendatorySectionTemplateIds = new List<string>
                                                                 {
                                                                 "2.16.840.1.113883.3.88.11.83.2",  //Language Spoken
                                                                 "2.16.840.1.113883.3.88.11.83.3",//Support
                                                                 "2.16.840.1.113883.10.20.1.13"//Summary Purpose
                                                                 };

            foreach (var section in CDAHelper.Sections)
            {
                var templateList = section.section.templateId.Select(t => t.root).ToList(); //List of template ids for each section. section can have CCD/HL7/HIE template Id
                var commonList = templateList.Intersect(listMendatorySectionTemplateIds).ToList();  //If any of the template id of the section will be similar to the List of Mendatory section Ids, it will be added to CommonList
                string sectionTitle = string.Join("", section.section.title.Text); //Title of the section
                if (commonList.Count > 0)
                    listMandatorySections.Add(sectionTitle);
                else if (sectionTitle == INFORMATION_SOURCE || sectionTitle == SUPPORT)// || sectionTitle == LANGUAGE_SPOKEN || sectionTitle == PERSON_INFORMATION || sectionTitle == SUMMARY_PURPOSE)
                    listMandatorySections.Add(sectionTitle);
                else
                    listWithoutMandatorySections.Add(sectionTitle);

            }
            //Left side list i.e. List of all non-mendatory  sections present in the document
            lbAvalSections.DataSource = listWithoutMandatorySections;
            lbAvalSections.DataBind();
            lbAvalSections.Items.Insert(0, CHECK_ALL);

            //Right side list i.e. List of all mendatory sections present in the document
            lbSelectedsections.DataSource = listMandatorySections;
            lbSelectedsections.DataBind();
            lbSelectedsections.Items.Insert(0, CHECK_ALL);

        }
       
    }
    #endregion

    #region gridSharedWith operations
    protected void gridSharedWith_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gridSharedWith.PageIndex = e.NewPageIndex;
            BindSharedWithGrid(documentId);
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }
    protected void gridSharedWith_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }
    protected void gridSharedWith_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[4].Visible = true;
                e.Row.Cells[4].Text = "Provider";
            }


            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = true;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;

                e.Row.Cells[7].Visible = true;

            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    protected void gridDocument_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }
    protected void gridSharedWith_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Delete")
            {
                int iSelectedIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow selRow = gridSharedWith.Rows[iSelectedIndex];
                string strOriginalDocumentID = selRow.Cells[1].Text;
                string strProviderEmail = selRow.Cells[5].Text;
                // objProxy.RemoveSharing(strOriginalDocumentID, strProviderEmail, out  errorCode);
                BindSharedWithGrid(documentId);
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }
    #endregion

    #region Removesections
    /// <summary>
    /// 
    /// </summary>
    private void RemoveCommonSections()
    {
        foreach (ListItem selectedItem in lbSelectedsections.Items)
        {
            foreach (ListItem availableItem in lbAvalSections.Items)
            {
                if (availableItem.Text == selectedItem.Text)
                {
                    lbAvalSections.Items.Remove(availableItem);
                    break;
                }
            }
        }
    }
    #endregion

    #region ResetConrols
    /// <summary>
    /// Resets the controls.
    /// </summary>
    private void ResetControls()
    {
        txtRuleStartDate.Text = "";
        txtRuleEndDate.Text = "";
        ddlProviderRole.SelectedIndex = 0;
        txtProviderEmail.Text = "";
        ddlPurposeForUse.SelectedIndex = 0;
        lbSelectedsections.Items.Clear();
        lbAvalSections.Items.Clear();
        ddlCommunities.SelectedIndex = -1;
    }
    #endregion

    # region Create XACML Policy
    /// <summary>
    /// Creates the XACML policy.
    /// </summary>
    /// <param name="lstResource">Name of the strdoc files.</param>
    private byte[] CreateXACMLPolicy(List<string> lstResource, List<string> lstSubject)
    {
        string fileNameWithPath = string.Empty;
        string xacmlFileName = Guid.NewGuid().ToString();

        xacmlFileName = xacmlFileName + "XACML.xml";

        fileNameWithPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache), xacmlFileName);



        XACMLClass objXACMLClass = new XACMLClass();

        XACMLBytes = objXACMLClass.CreateXACMLPolicy("urn:Policy0001", "Access consent policy", string.Empty, "Rule0001", String.Join(", ", lstSubject) + " " + "can use" + " " + String.Join(", ", lstResource) + " " + "for" + " " + ddlPurposeForUse.SelectedItem.Text, lstSubject, "", "", lstResource, ddlPurposeForUse.SelectedItem.Text, fileNameWithPath, txtRuleStartDate.Text, txtRuleEndDate.Text);

        return XACMLBytes;
    }
    #endregion

    #region lbSelectedsections_SelectedIndexChanged event
    /// <summary>
    /// Handles the Click event of the btnShare control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void lbSelectedsections_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool blnAllSelected = true;
        foreach (ListItem item in lbSelectedsections.Items)
        {
            if (!item.Selected)
            {
                blnAllSelected = false;
                break;
            }
        }
        if (blnAllSelected)
            chkRemoveAll.Checked = true;
        else
            chkRemoveAll.Checked = false;
    }
    #endregion

    #region Available sections
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbAvalSections_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool blnAllSelected = true;
        foreach (ListItem item in lbAvalSections.Items)
        {
            if (!item.Selected)
            {
                blnAllSelected = false;
                break;
            }
        }
        if (blnAllSelected)
            chkSelectAll.Checked = true;
        else
            chkSelectAll.Checked = false;
    }
    #endregion

    

    #region Move Right
    /// <summary>
    /// Handles the Click event of the imgbtnMoveRight control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Web.UI.ImageClickEventArgs"/> instance containing the event data.</param>
    protected void imgbtnMoveRight_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            if (lbAvalSections.SelectedIndex > -1)
            {
                string AssetIDList = "";
                foreach (ListItem lstAsset in lbAvalSections.Items)
                {
                    if (lstAsset.Selected && lbSelectedsections.Items.FindByValue(lstAsset.Value) == null)
                    {
                        lbSelectedsections.Items.Add(lstAsset);
                        AssetIDList += AssetIDList.Length > 0 ? "," + lstAsset.Value : lstAsset.Value;
                    }
                }

                string[] arrItemsToRemove = AssetIDList.Split(new char[] { ',' });
                for (int i = 0; i < arrItemsToRemove.Length; i++)
                {
                    ListItem lstSelectedItem = lbAvalSections.Items.FindByValue(arrItemsToRemove[i].ToString());
                    if (lstSelectedItem != null)
                    {
                        lbAvalSections.Items.Remove(lstSelectedItem);
                    }
                }

                lbAvalSections.ClearSelection();
                lbSelectedsections.ClearSelection();
                chkSelectAll.Checked = false;
            }
        }
        catch (Exception ex)
        {
            string Msg = ex.Message.Replace("\r\n", "").Replace("\n", "");
            Response.Redirect("..\\ErrorPage.aspx?ErrorNo=" + ex.GetHashCode() + "&ErrorDesc=" + Msg, false);
        }
    }

    #endregion

    #region MoveLeft
    /// <summary>
    /// Handles the Click event of the imgbtnMoveLeft control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Web.UI.ImageClickEventArgs"/> instance containing the event data.</param>
    protected void imgbtnMoveLeft_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (lbSelectedsections.SelectedIndex > -1)
            {
                string AssetIDList = "";
                foreach (ListItem lstAsset in lbSelectedsections.Items)
                {
                    //Condition added for not to showing mandatory sections in available listbox
                    if (!(lstAsset.ToString() == INFORMATION_SOURCE || lstAsset.ToString() == CHECK_ALL || lstAsset.ToString() == LANGUAGE_SPOKEN || lstAsset.ToString() == PERSON_INFORMATION || lstAsset.ToString() == SUPPORT || lstAsset.ToString() == SUMMARY_PURPOSE))
                    {

                        if (lstAsset.Selected && lbAvalSections.Items.FindByValue(lstAsset.Value) == null)
                        {
                            lbAvalSections.Items.Add(lstAsset);
                            AssetIDList += AssetIDList.Length > 0 ? "," + lstAsset.Value : lstAsset.Value;
                        }
                    }
                }

                string[] arrItemsToRemove = AssetIDList.Split(new char[] { ',' });
                for (int i = 0; i < arrItemsToRemove.Length; i++)
                {
                    ListItem lstSelectedItem = lbSelectedsections.Items.FindByValue(arrItemsToRemove[i].ToString());

                    if (lstSelectedItem != null)
                    {
                        lbSelectedsections.Items.Remove(lstSelectedItem);
                    }
                }
                lbAvalSections.ClearSelection();
                lbSelectedsections.ClearSelection();
                chkRemoveAll.Checked = false;
            }
        }
        catch (Exception ex)
        {
            string Msg = ex.Message.Replace("\r\n", "").Replace("\n", "");
            Response.Redirect("..\\ErrorPage.aspx?ErrorNo=" + ex.GetHashCode() + "&ErrorDesc=" + Msg, false);
        }
    }
    #endregion

    #region Removeall
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkRemoveAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkRemoveAll.Checked)
        {
            foreach (ListItem item in lbSelectedsections.Items)
            {
                item.Selected = true;
            }
        }
        else
        {
            lbSelectedsections.ClearSelection();
        }
    }
    #endregion

    #region selectall
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSelectAll.Checked)
        {
            foreach (ListItem item in lbAvalSections.Items)
            {
                item.Selected = true;
            }
        }
        else
        {
            lbAvalSections.ClearSelection();
        }
    }
    #endregion

    #region Reset
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.RawUrl);
    }
    #endregion

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("DocumentList.aspx", false);
        //Ends addition
    }


    #region ShareDocument
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShare_Click(object sender, EventArgs e)
    {

        string targetC32Document = string.Empty;

        GlobalSessions.SessionAdd(SessionItem.ProviderEMail, txtProviderEmail.ToString());
        ShareDocumentRequest getShareDocumentRequest = new ShareDocumentRequest();
        ShareDocumentResponse getDocumentsResponse = new ShareDocumentResponse();

        try
        {
            if (Convert.ToInt32(ddlProviderRole.SelectedValue) > 0)
            {
                providerValue = ddlProviderRole.SelectedItem.Text;
            }
            else
            {
                providerValue = txtProviderEmail.Text;
            }

            DateTime dt1 = Convert.ToDateTime(txtRuleStartDate.Text);
            DateTime dt2 = Convert.ToDateTime(txtRuleEndDate.Text);
            TimeSpan ts = dt1 - dt2;
            int days = ts.Days;
            if (days >= 0)
            {
                ScriptManager.RegisterStartupScript(this, typeof(SharePatientDocument), "Share", "alert('Rule Start Date can not be greater than equal to Rule End Date');", true);
                return;
            }


            List<string> lstResource = new List<string>();
            lstResource.Add(documentId);
            //lstResource.Add("db25fc0e-23b0-4ae2-8158-cbed124ab75cXACML");  //temp name ..will be changed later

            List<string> lstSubject = new List<string>();
            lstSubject.Add(providerValue);
            lstSubject.Add(Session["UserEmailAddress"].ToString());

            // Get the XACML policy file byte data from policyFilePath and pass in UploadDocumentNew()Method 
            XACMLBytes = CreateXACMLPolicy(lstResource, lstSubject);



            List<string> itemsToShare = new List<string>();
            foreach (ListItem lstAsset in lbSelectedsections.Items)
            {
                if (lstAsset.Selected && lstAsset.ToString() != CHECK_ALL)
                    itemsToShare.Add(lstAsset.Text);
            }
            //check if no section has been selected then return
            if (itemsToShare.Count==0)
            {
                lblmessage.Text = Helper.GetErrorMessage(ErrorCode.C32Sections_Missing);
                return;
            }
            Result result = CDAHelper.SharedSectionDocument(itemsToShare, out documentBytes);

            //Service call for share document
            getShareDocumentRequest.docByteData = documentBytes;
            getShareDocumentRequest.XACMLbyteData = XACMLBytes;
            getShareDocumentRequest.patientId = GlobalSessions.SessionItem(SessionItem.MPIID).ToString();
            getShareDocumentRequest.homeCommunityId = MobiusAppSettingReader.LocalHomeCommunityID;
            getShareDocumentRequest.sourceRepositryId = Helper.SourceRepositoryId;
            getShareDocumentRequest.subject = GlobalSessions.SessionItem(SessionItem.UserRole) as string;
            getShareDocumentRequest.OriginalDocumentID = txtDocumentId.Text;
           

            List<MobiusServiceLibrary.PatientIdentifier> NHINCommunities = (List<MobiusServiceLibrary.PatientIdentifier>)GlobalSessions.SessionItem(SessionItem.PHISources);

            getShareDocumentRequest.RemoteCommunityId = ddlCommunities.SelectedValue;

            var patient = NHINCommunities.Where(t => t.CommunityIdentifier == getShareDocumentRequest.RemoteCommunityId).FirstOrDefault();

            if (patient != null)
            {
                getShareDocumentRequest.RemotePatientId = patient.PatientId;
            }

            List<NHINCommunity> community = (List<NHINCommunity>)GlobalSessions.SessionItem(SessionItem.CommunityList);
            NHINCommunity homeNHINCommunity = community.Where(t => t.IsHomeCommunity).FirstOrDefault();
            //Verify PurposeOfUse
            PurposeOfUse purposeOfUse;
            Enum.TryParse(ddlPurposeForUse.SelectedItem.Text, out purposeOfUse);

            AssertionHelper assertion = new AssertionHelper();
            getShareDocumentRequest.Assertion = assertion.CreateAssertion(AssertionMode.Default, AssertionAction.DocumentSubmission,
           purposeOfUse,
            base.GetUserInformation(homeNHINCommunity), homeNHINCommunity);



            soapHandler.RequestEncryption(getShareDocumentRequest, out soapProperties);
            getShareDocumentRequest.SoapProperties = soapProperties;
            getDocumentsResponse = objProxy.ShareDocument(getShareDocumentRequest);
            if (getDocumentsResponse.Result.IsSuccess)
            {
                if (!soapHandler.ResponseDecryption(getDocumentsResponse.SoapProperties, getDocumentsResponse))
                {

                    lblmessage.Text = INVALID_RESPONSE_DATA;
                    return;
                }
                else
                {
                    BindSharedWithGrid(documentId);
                    lblmessage.Text = DOCUMENT_SHARED;
                }
            }
            else
            {
                lblmessage.Text = getDocumentsResponse.Result.ErrorMessage;
                return;
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }

    }
    #endregion

    #region BindSharedWithGrid
    /// <summary>
    /// 
    /// </summary>
    /// <param name="documentId"></param>
    private void BindSharedWithGrid(string documentId)
    {

        try
        {
            DataSet dsProvider = new DataSet();
            if (dsProvider.Tables.Count > 0)
            {
                if (dsProvider.Tables[0].Rows.Count > 0)
                {
                    gridSharedWith.DataSource = dsProvider.Tables[0];
                    gridSharedWith.DataBind();
                }
            }

        }
        catch
        { }
    }
    #endregion

    #region Role and email Visibility
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rdbrolecheckedChanged(object sender, EventArgs e)
    {
        if (rbtlst.Items[0].Selected == true)
        {
            trProvideRole.Visible = false;
            trProvideEmail.Visible = true;
        }
        else
        {
            trProvideRole.Visible = true;
            trProvideEmail.Visible = false;
        }
    }
    #endregion

    /// <summary>
    /// list of NHIN communities is passed as parameter.
    /// </summary>
    /// <param name="NHINCommunities">NHINCommunities which is generic list of type NHINCommunity </param>
    private void FillNHINCommunities()
    {
        List<MobiusServiceLibrary.PatientIdentifier> NHINCommunities = (List<MobiusServiceLibrary.PatientIdentifier>)GlobalSessions.SessionItem(SessionItem.PHISources);

        ddlCommunities.DataSource = NHINCommunities;
        ddlCommunities.DataTextField = COMMUNITY_DESCRIPTION;
        ddlCommunities.DataValueField = COMMUNITY_IDENTIFIER;
        ddlCommunities.DataBind();
        ListItem selectCommunity = new ListItem("--Select--", "-1");
        ddlCommunities.Items.Insert(0, selectCommunity);

    }



    protected void ViewPatientDocument(object sender, EventArgs e)
    {
        try
        {

            string strDocumentId = string.Empty;
            string localMPIID = string.Empty;

            MobiusServiceLibrary.Patient patientInfo = (MobiusServiceLibrary.Patient)GlobalSessions.SessionItem(SessionItem.SelectedPatient);
            strDocumentId = Request.QueryString["DocumentID"];
            localMPIID = GlobalSessions.SessionItem(SessionItem.MPIID).ToString();

            GetDocumentResponse getDocumentResponse = new GetDocumentResponse();
            GetDocumentRequest getDocumentRequest = new GetDocumentRequest();
            getDocumentRequest.patientId = localMPIID;
            getDocumentRequest.documentId = strDocumentId;
            getDocumentRequest.purpose = ddlPurposeForUse.SelectedItem.Text;
            getDocumentRequest.subjectRole = GlobalSessions.SessionItem(SessionItem.UserRole) != null ? GlobalSessions.SessionItem(SessionItem.UserRole) as string : string.Empty; ;
            getDocumentRequest.subjectEmailID = this.EmailAddress;

            string selectedPurpose = ddlPurposeForUse.SelectedItem.Text;
            PurposeOfUse PurposeOfUse;
            Enum.TryParse(selectedPurpose, out PurposeOfUse);
            if (PurposeOfUse.GetHashCode() == 0 && userRole == Helper.Patient)
                PurposeOfUse = PurposeOfUse.REQUEST;
            else if (PurposeOfUse.GetHashCode() == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShareDocument", "alert('Please select purpose.');", true);
                return;
            }

            List<NHINCommunity> nhinCommunitiesSession = (List<NHINCommunity>)GlobalSessions.SessionItem(SessionItem.CommunityList);

            if (nhinCommunitiesSession == null)
                nhinCommunitiesSession = GetNhinCommunities();

            NHINCommunity homeNHINCommunity = nhinCommunitiesSession.Where(t => t.IsHomeCommunity).FirstOrDefault();
            AssertionHelper assertionHelper = new AssertionHelper();
            getDocumentRequest.Assertion = assertionHelper.CreateAssertion(AssertionMode.Default, AssertionAction.DocumentRetrieve,
                PurposeOfUse, base.GetUserInformation(homeNHINCommunity), homeNHINCommunity);


            this.soapHandler = new SoapHandler();
            this.soapProperties = new SoapProperties();
            this.soapHandler.RequestEncryption(getDocumentRequest, out this.soapProperties);


            getDocumentRequest.SoapProperties = this.soapProperties;
            getDocumentResponse = this.objProxy.GetDocument(getDocumentRequest);
            if (getDocumentResponse.Result.IsSuccess)
            {
                if (this.soapHandler.ResponseDecryption(getDocumentResponse.SoapProperties, getDocumentResponse))
                {
                    Document docData = getDocumentResponse.Document;
                    if (docData.DocumentBytes != null)
                    {
                        GlobalSessions.SessionAdd(SessionItem.XMLDOC, docData.DocumentBytes);
                        ScriptManager.RegisterStartupScript(this, typeof(SharePatientDocument), "ViewDocument", "openpopupInFullScreen('ViewDoDDocument.aspx','Document');", true);
                    }
                    else if (docData.DocumentBytes == null)
                    {
                        lblmessage.Text = getDocumentResponse.Result.ErrorMessage;
                    }
                }
                else
                {
                    lblmessage.Text = INVALID_RESPONSE_DATA;
                    return;
                }
            }
            else
            {
                lblmessage.Text = getDocumentResponse.Result.ErrorMessage;
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    private List<NHINCommunity> GetNhinCommunities()
    {
        SoapHandler soapHandler = new SoapHandler();
        GetNhinCommunityResponse nhinCommunityResponse = objProxy.GetNhinCommunity();
        if (nhinCommunityResponse.Result.IsSuccess)
        {
            if (soapHandler.ResponseDecryption(nhinCommunityResponse.SoapProperties, nhinCommunityResponse))
            {
                if (nhinCommunityResponse.Communities != null)
                {
                    List<NHINCommunity> nhinCommunities = new List<NHINCommunity>(nhinCommunityResponse.Communities);
                    GlobalSessions.SessionAdd(SessionItem.CommunityList, nhinCommunities);
                    return nhinCommunities;
                }
            }
        }
        return new List<NHINCommunity>();
    }

    private DateTime ConvertDate(string date, string pattern)
    {
        DateTime retval = DateTime.MinValue;
        if (System.DateTime.TryParseExact(date, pattern, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out retval))
            return retval;
        // Could not convert date..     
        return DateTime.MinValue;
    }


}
