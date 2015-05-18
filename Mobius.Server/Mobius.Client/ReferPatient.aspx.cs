#region Namespaces
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml;
using C32Utility;
using FirstGenesis.Mobius.Server.MobiusHISEService.XACML;
using FirstGenesis.UI;
using FirstGenesis.UI.Base;
using Mobius.CoreLibrary;
using Mobius.Entity;
using Mobius.FileSystem;
using MobiusServiceLibrary;
using MobiusEntity = Mobius.Entity;
using MobiusServiceUtility;
#endregion

public partial class ReferPatient : BaseClass
{
    #region Variables
    MobiusClient _MobiusClient = null;
    private MobiusClient MobiusClient
    {
        get
        {
            return _MobiusClient != null ? _MobiusClient : _MobiusClient = new MobiusClient();
        }
    }
   
    private MobiusServiceLibrary.PatientReferral PatientReferral
    {
        get;
        set;
    }
    protected const string INFORMATION_SOURCE = "Information Source";
    protected const string LANGUAGE_SPOKEN = "Language Spoken";
    protected const string PERSON_INFORMATION = "Person Information";
    protected const string SUPPORT = "Support";
    protected const string SUMMARY_PURPOSE = "Summary Purpose";
    protected const string CHECK_ALL = "Check All";

    private static CDAHelper CDAHelper;
    string documentId = string.Empty;
    string patientReferralID = string.Empty;
    string providerValue = string.Empty;
    byte[] documentBytes = null;
    byte[] XACMLBytes = null;
    string localMPIID = string.Empty;
    SoapHandler SoapHandler = new SoapHandler();
    SoapProperties SoapProperties = new SoapProperties();
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //if Document Id is not provided then redirect to login Page
            if (!string.IsNullOrEmpty(Request.QueryString["DocumentID"]))
            {
                documentId = Request.QueryString["DocumentID"];
            }
            else
            {
                Response.Redirect("login.aspx");
            }

            patientReferralID = Request.QueryString["PatientReferralID"];

            //Added for not to showing mandatory sections in available list box and showing in selected list box in disable state.
            if (!IsPostBack)
            {
                GlobalSessions.SessionRemove(SessionItem.GetPatientReferralResponse);
                if (Request.UrlReferrer != null)
                    hdnUrl.Value = Request.UrlReferrer.ToString();
                else
                    hdnUrl.Value = "ManageReferral.aspx";

                GetAndSetDocumentForReferal();
            }

        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);

        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.RawUrl);
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(hdnUrl.Value))
            Response.Redirect(hdnUrl.Value, false);
        else
            Response.Redirect("ManageReferral.aspx", false);


    }

    protected void lnkViewOutComeDocument_Click(object sender, EventArgs e)
    {
        try
        {
            ViewPatientDocument(((LinkButton)sender).CommandArgument);
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }

    }

    private void ViewPatientDocument(string mode)
    {
        this.PatientReferral = (MobiusServiceLibrary.PatientReferral)GlobalSessions.SessionItem(SessionItem.GetPatientReferralResponse);
        string strDocumentId = string.Empty;

        if (this.PatientReferral != null)
        {
            if (mode == "OutComeDocument")
                strDocumentId = PatientReferral.OutcomeDocumentID;
            else
                strDocumentId = PatientReferral.DocumentId;

            localMPIID = PatientReferral.Patient.LocalMPIID;
        }
        else
        {
            MobiusServiceLibrary.Patient patientInfo = (MobiusServiceLibrary.Patient)GlobalSessions.SessionItem(SessionItem.SelectedPatient);
            strDocumentId = Request.QueryString["DocumentID"];
            localMPIID = patientInfo.LocalMPIID;
        }

        GetDocumentResponse getDocumentResponse = new GetDocumentResponse();
        GetDocumentRequest getDocumentRequest = new GetDocumentRequest();
        getDocumentRequest.patientId = localMPIID;
        getDocumentRequest.documentId = strDocumentId;
        getDocumentRequest.purpose = ddlPurposeForUse.SelectedItem.Text;
        getDocumentRequest.subjectRole = GlobalSessions.SessionItem(SessionItem.UserRole) != null ? GlobalSessions.SessionItem(SessionItem.UserRole) as string : string.Empty; ;
        getDocumentRequest.subjectEmailID = this.EmailAddress;

        PurposeOfUse PurposeOfUse;
        Enum.TryParse(ddlPurposeForUse.SelectedItem.Text, out PurposeOfUse);

        List<NHINCommunity> nhinCommunitiesSession = (List<NHINCommunity>)GlobalSessions.SessionItem(SessionItem.CommunityList);

        if (nhinCommunitiesSession == null)
            nhinCommunitiesSession = GetNhinCommunities();

        NHINCommunity homeNHINCommunity = nhinCommunitiesSession.Where(t => t.IsHomeCommunity).FirstOrDefault();
        AssertionHelper assertionHelper = new AssertionHelper();
        getDocumentRequest.Assertion = assertionHelper.CreateAssertion(AssertionMode.Default, AssertionAction.DocumentRetrieve,
            PurposeOfUse, base.GetUserInformation(homeNHINCommunity), homeNHINCommunity);


        this.SoapHandler = new SoapHandler();
        this.SoapProperties = new SoapProperties();
        this.SoapHandler.RequestEncryption(getDocumentRequest, out this.SoapProperties);


        getDocumentRequest.SoapProperties = this.SoapProperties;
        getDocumentResponse = this.objProxy.GetDocument(getDocumentRequest);
        if (getDocumentResponse.Result.IsSuccess)
        {
            if (this.SoapHandler.ResponseDecryption(getDocumentResponse.SoapProperties, getDocumentResponse))
            {
                Document docData = getDocumentResponse.Document;
                if (docData.DocumentBytes != null)
                {
                    GlobalSessions.SessionAdd(SessionItem.XMLDOC, docData.DocumentBytes);
                    ScriptManager.RegisterStartupScript(this, typeof(ReferPatient), "ViewDocument", "openpopupInFullScreen('ViewDoDDocument.aspx','Document');", true);
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

    #region Document Metadata Grid operation
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dsDocument"></param>
    private void BindDocumentGrid(List<Document> documents)
    {
        for (int i = 0; i < documents.Count; i++)
        {
            string date;
            DateTime dateTime;
            string[] format = { "yyyyMMddHHmmss", "yyyyMMddHHmmsszzz" };

            date = documents[i].CreatedOn.ToString();
            //Follwing code commented for a datetime value which was also returning the Time Zone, which was crashing the implementation.
            //    date = Convert.ToString(DateTime.ParseExact(date, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("MM/dd/yyyy"));
            if (DateTime.TryParseExact(date, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dateTime))
            {
                date = Convert.ToString(dateTime);
                documents[i].CreatedOn = date;
            }
        }
        //Bind Document Grid
        gridDocument.DataSource = documents;
        gridDocument.DataBind();


    }
    #endregion


    #region ResetConrols
    /// <summary>
    /// Resets the controls.
    /// </summary>
    private void ResetControls()
    {
        txtRuleStartDate.Text = "";
        txtProviderEmail.Text = "";
        ddlPurposeForUse.SelectedIndex = 0;
        lbSelectedsections.Items.Clear();
        lbAvalSections.Items.Clear();
    }
    #endregion

    # region Create XACML Policy
    /// <summary>
    /// Creates the XACML policy.
    /// </summary>
    /// <param name="strdocFileName">Name of the strdoc file.</param>
    private byte[] CreateXACMLPolicy(List<string> lstResource,List<string> lstSubject )
    {
        string fileNameWithPath = string.Empty;
        string xacmlFileName = Guid.NewGuid().ToString();

        xacmlFileName = xacmlFileName + "XACML.xml";

        fileNameWithPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache), xacmlFileName);

        XACMLClass objXACMLClass = new XACMLClass();
        string ruleStartDate = string.Empty;
        string ruleEndDate = string.Empty;
        ruleStartDate = Convert.ToString(System.DateTime.Now);
        ruleEndDate = Convert.ToString(System.DateTime.Now.AddYears(2));

        

        XACMLBytes = objXACMLClass.CreateXACMLPolicy("urn:Policy0001", "Access consent policy", string.Empty, "Rule0001", 
            String.Join(", ", lstSubject) + " " + "can use" + " " + String.Join(", ", lstResource) + " " + "for" + " " + ddlPurposeForUse.SelectedItem.Text, 
            lstSubject, "", "", lstResource, ddlPurposeForUse.SelectedItem.Text, fileNameWithPath, ruleStartDate, ruleEndDate);

        return XACMLBytes;
    }
    #endregion

    protected void lbSelectedsections_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }


    #region Drop Down Section Move

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
            ExceptionHelper.HandleException(page: Page, ex: ex);
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
                    //Condition added for not to showing mandatory items in lbAvalSections list box
                    if (!(lstAsset.ToString() == INFORMATION_SOURCE || lstAsset.ToString() == LANGUAGE_SPOKEN ||
                        lstAsset.ToString() == PERSON_INFORMATION|| lstAsset.ToString() == SUPPORT|| lstAsset.ToString() == SUMMARY_PURPOSE))
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
            ExceptionHelper.HandleException(page: Page, ex: ex);
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
            lbSelectedsections.ClearSelection();
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
            lbAvalSections.ClearSelection();
    }
    #endregion

    #endregion Drop Down Section Move

    #region Referralpatient
    /// <summary>
    /// This method will create the patient referral 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnReferral_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtProviderEmail.Text != GlobalSessions.SessionItem(SessionItem.UserEmailAddress).ToString())
            {
                #region variables
                string targetC32Document = string.Empty;
                MobiusServiceLibrary.Patient _objPatientInfo = new MobiusServiceLibrary.Patient();
                MobiusEntity.Demographics _objDemograPhics = new MobiusEntity.Demographics();
                CreateReferralRequest _objReferralRequest = new CreateReferralRequest();
                PatientReferralResponse getReferralResponse = new PatientReferralResponse();

                #endregion

                #region Validation
                providerValue = txtProviderEmail.Text;
                // DateTime dReferralDate = Convert.ToDateTime(txtRuleStartDate.Text);
                _objPatientInfo = (MobiusServiceLibrary.Patient)GlobalSessions.SessionItem(SessionItem.SelectedPatient);

                #endregion
                documentId = Guid.NewGuid().ToString();
                _objReferralRequest.PatientReferred = new MobiusServiceLibrary.CreatePatientReferral();
                _objReferralRequest.PatientReferred.DocumentId = documentId;
                _objReferralRequest.PatientReferred.Patient = new MobiusServiceLibrary.Patient();
                _objReferralRequest.PatientReferred.Patient.Telephones = new List<MobiusServiceLibrary.Telephone>();
                _objReferralRequest.PatientReferred.Patient.PatientAddress = new List<MobiusServiceLibrary.Address>();

                if (_objPatientInfo != null)
                {
                    _objReferralRequest.PatientReferred.Patient.GivenName = _objPatientInfo.GivenName;
                    _objReferralRequest.PatientReferred.Patient.FamilyName = _objPatientInfo.FamilyName;
                    _objReferralRequest.PatientReferred.Patient.Gender = _objPatientInfo.Gender;// == "M" ? "Male" : "Female";
                    _objReferralRequest.PatientReferred.Patient.DOB = ConvertDate(_objPatientInfo.DOB, "yyyyMMdd").ToShortDateString();
                    _objReferralRequest.PatientReferred.Patient.LocalMPIID = _objPatientInfo.LocalMPIID;
                }

                _objReferralRequest.PatientReferred.ReferralSummary = lstcomment.Text;
                _objReferralRequest.PatientReferred.ReferredToEmail = txtProviderEmail.Text;
                _objReferralRequest.PatientReferred.ReferralAccomplishedOn = txtRuleStartDate.Text;
                // _objReferralRequest.PatientReferred.ReferredByEmail = MobiusAppSettingReader.EmailFromAddress;
                _objReferralRequest.PatientReferred.ReferredByEmail = Session["UserEmailAddress"].ToString();
                _objReferralRequest.PatientReferred.PurposeOfUse = (PurposeOfUse)Enum.Parse(typeof(PurposeOfUse), ddlPurposeForUse.SelectedValue, true);
                if (GlobalSessions.SessionItem(SessionItem.UserRole) != null)
                {
                    _objReferralRequest.PatientReferred.Subject = GlobalSessions.SessionItem(SessionItem.UserRole) as string;
                }

                //List of all the files/resource which are being uploaded/created
                List<string> lstResource = new List<string>();
                lstResource.Add(documentId);
                //lstResource.Add("db25fc0e-23b0-4ae2-8158-cbed124ab75cXACML");  //temp name ..will be changed later

                //List of the Subjects/Users who will be given the permission for newly created documents 
                List<string> lstSubject = new List<string>();
                lstSubject.Add(providerValue);
                lstSubject.Add(Session["UserEmailAddress"].ToString());

                //Get the XACML policy file byte data from policyFilePath and pass in UploadDocumentNew()Method 
                XACMLBytes = CreateXACMLPolicy(lstResource, lstSubject);

                List<string> itemsToShare = new List<string>();
                foreach (ListItem lstAsset in lbSelectedsections.Items)
                {
                        if (lstAsset.Selected &&  lstAsset.ToString() != CHECK_ALL)
                                itemsToShare.Add(lstAsset.Text);
                }
                

                //check if no section has been selected then return
                if (itemsToShare.Count==0)
                {
                    lblmessage.Text = Helper.GetErrorMessage(ErrorCode.C32Sections_Missing);
                    return;
                }

                Result result = CDAHelper.SharedSectionDocument(itemsToShare, out documentBytes);

                if (result.IsSuccess)
                {
                    _objReferralRequest.PatientReferred.DocumentBytes = documentBytes;
                    _objReferralRequest.PatientReferred.XACMLBytes = XACMLBytes;
                    _objReferralRequest.PatientReferred.CommunityId = MobiusAppSettingReader.LocalHomeCommunityID;
                    _objReferralRequest.PatientReferred.OriginalDocumentID = hdnDocumentId.Value;

                    List<NHINCommunity> nhinCommunitiesSession = (List<NHINCommunity>)GlobalSessions.SessionItem(SessionItem.CommunityList);
                    if (nhinCommunitiesSession == null)
                        nhinCommunitiesSession = GetNhinCommunities();

                    NHINCommunity homeNHINCommunity = nhinCommunitiesSession.Where(t => t.IsHomeCommunity).FirstOrDefault();
                    AssertionHelper assertionHelper = new AssertionHelper();
                    _objReferralRequest.PatientReferred.Assertion = assertionHelper.CreateAssertion(AssertionMode.Default, AssertionAction.DocumentSubmission,
                        PurposeOfUse.TREATMENT, base.GetUserInformation(homeNHINCommunity), homeNHINCommunity);


                    // Request Encryption
                    this.SoapHandler.RequestEncryption(_objReferralRequest, out this.SoapProperties);
                    _objReferralRequest.SoapProperties = this.SoapProperties;
                    getReferralResponse = objProxy.CreateReferral(_objReferralRequest);
                    if (getReferralResponse.Result.IsSuccess)
                    {
                        if (this.SoapHandler.ResponseDecryption(getReferralResponse.SoapProperties, getReferralResponse))
                        {
                            btnShare.Enabled = false;
                            btnReset.Enabled = false;

                            // lblmessage.Text= Helper.GetErrorMessage(ErrorCode.Patient_Refferred_Successfully);
                            lblmessage.Text = getReferralResponse.Result.ErrorMessage;


                          
                        }
                        else
                        {
                            lblmessage.Text = Helper.GetErrorMessage(ErrorCode.Invalid_Response_Data);
                        }
                    }
                    else
                    {

                        lblmessage.Text = getReferralResponse.Result.ErrorMessage;
                        return;

                    }
                }
                else
                {
                    lblmessage.Text = result.ErrorMessage;
                    return;
                }
            }
            else
            {
                lblmessage.Text = Helper.GetErrorMessage(ErrorCode.Provider_Cannot_Make_A_Referral_To_Himself);
                return;
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    #endregion

    protected void btnAcknowledge_Click(object sender, EventArgs e)
    {
        try
        {
            XmlDocument doc = new XmlDocument();
            string OutcomeDocumentID = string.Empty;
            string srcC32Document = string.Empty;
            bool UploadDocumentResult = false;
            PatientReferralResponse getReferralResponse = new PatientReferralResponse();
            MobiusServiceLibrary.PatientReferral PatientReferral = null;
            if (GlobalSessions.SessionItem(SessionItem.GetPatientReferralResponse) != null)
            {
                PatientReferral = (MobiusServiceLibrary.PatientReferral)GlobalSessions.SessionItem(SessionItem.GetPatientReferralResponse);
            }
            if (!PatientReferral.AcknowledgementStatus)
            {
                AcceptReferralRequest acceptReferralRequest = new AcceptReferralRequest();
                acceptReferralRequest.AcceptPatientReferred = new MobiusServiceLibrary.AcceptReferral();

                if (PatientReferral.Patient != null)
                {
                    acceptReferralRequest.AcceptPatientReferred.Patient = new MobiusServiceLibrary.Patient();
                    acceptReferralRequest.AcceptPatientReferred.Id = PatientReferral.Id;
                    acceptReferralRequest.AcceptPatientReferred.ReferredByEmail = PatientReferral.ReferredByEmail;
                    acceptReferralRequest.AcceptPatientReferred.ReferredToEmail = PatientReferral.ReferredToEmail;
                    acceptReferralRequest.AcceptPatientReferred.ReferralOn = PatientReferral.ReferralOn;
                    acceptReferralRequest.AcceptPatientReferred.DocumentId = PatientReferral.DocumentId;
                    acceptReferralRequest.AcceptPatientReferred.Patient = new MobiusServiceLibrary.Patient();
                    acceptReferralRequest.AcceptPatientReferred.Patient.DOB = PatientReferral.Patient.DOB;
                    acceptReferralRequest.AcceptPatientReferred.Patient.Gender = PatientReferral.Patient.Gender;
                    acceptReferralRequest.AcceptPatientReferred.Patient.LocalMPIID = PatientReferral.Patient.LocalMPIID;
                    acceptReferralRequest.AcceptPatientReferred.Patient.FamilyName = PatientReferral.Patient.FamilyName;
                    acceptReferralRequest.AcceptPatientReferred.Patient.GivenName = PatientReferral.Patient.GivenName;
                }

                if (rdStatuso1.Selected)
                {
                    acceptReferralRequest.AcceptPatientReferred.AcknowledgementStatus = true;
                }
                else
                {
                    acceptReferralRequest.AcceptPatientReferred.AcknowledgementStatus = false;
                }
                acceptReferralRequest.AcceptPatientReferred.PatientAppointmentDate = txtRuleEndDate.Text;
                acceptReferralRequest.AcceptPatientReferred.DispatcherSummary = txtdispatcherComment.Text;
                acceptReferralRequest.AcceptPatientReferred.ReferralAccomplishedOn = Convert.ToDateTime(txtRuleStartDate.Text).ToString("MM/dd/yyyy");

                acceptReferralRequest.AcceptPatientReferred.Patient.Telephones = new List<MobiusServiceLibrary.Telephone>();
                acceptReferralRequest.AcceptPatientReferred.Patient.PatientAddress = new List<MobiusServiceLibrary.Address>();

                SoapHandler.RequestEncryption(acceptReferralRequest, out SoapProperties);
                acceptReferralRequest.SoapProperties = SoapProperties;
                getReferralResponse = objProxy.AcknowledgeReferral(acceptReferralRequest);
                if (getReferralResponse.Result.IsSuccess)
                {
                    if (SoapHandler.ResponseDecryption(getReferralResponse.SoapProperties, getReferralResponse))
                    {
                        txtReadOnlyAppointmentDate.Text = txtRuleEndDate.Text;
                        //trReferralCompleted.Visible = true;
                        btnAcknowledge.Enabled = false;
                        btnReset.Enabled = false;
                        lblmessage.Text = getReferralResponse.Result.ErrorMessage;
                        PatientReferral.AcknowledgementStatus = true;
                        PatientReferral.PatientAppointmentDate = txtRuleEndDate.Text;
                        //GlobalSessions.SessionAdd(SessionItem.GetPatientReferralResponse, PatientReferral);
                    }
                    else
                    {
                        lblmessage.Text = Helper.GetErrorMessage(ErrorCode.Invalid_Response_Data);
                        return;
                    }
                }
                else
                {
                    lblmessage.Text = getReferralResponse.Result.ErrorMessage;
                    return;
                }
            }

            else
            {
                PatientReferralCompletedRequest referralCompletedRequest = new PatientReferralCompletedRequest();
                referralCompletedRequest.ReferralCompleted = new MobiusServiceLibrary.PatientReferralCompleted();

                referralCompletedRequest.ReferralCompleted.DispatcherSummary = txtdispatcherComment.Text;
                referralCompletedRequest.ReferralCompleted.PatientAppointmentDate = chkreschdule.Checked ? txtRuleEndDate.Text : txtReadOnlyAppointmentDate.Text;
                referralCompletedRequest.ReferralCompleted.ReferralAccomplishedOn = Convert.ToDateTime(txtRuleStartDate.Text).ToString("MM/dd/yyyy");
                referralCompletedRequest.ReferralCompleted.ReferralCompleted = PatientReferral.ReferralCompleted;
                referralCompletedRequest.ReferralCompleted.ReferralOn = PatientReferral.ReferralOn;
                referralCompletedRequest.ReferralCompleted.OutcomeDocumentID = OutcomeDocumentID;
                referralCompletedRequest.ReferralCompleted.Id = PatientReferral.Id;
                referralCompletedRequest.ReferralCompleted.ReferredByEmail = PatientReferral.ReferredByEmail;
                referralCompletedRequest.ReferralCompleted.ReferredToEmail = PatientReferral.ReferredToEmail;
                referralCompletedRequest.ReferralCompleted.DocumentId = PatientReferral.DocumentId;
                referralCompletedRequest.ReferralCompleted.Patient = new MobiusServiceLibrary.Patient();
                referralCompletedRequest.ReferralCompleted.Patient.DOB = PatientReferral.Patient.DOB;
                referralCompletedRequest.ReferralCompleted.Patient.Gender = PatientReferral.Patient.Gender;
                referralCompletedRequest.ReferralCompleted.Patient.LocalMPIID = PatientReferral.Patient.LocalMPIID;
                referralCompletedRequest.ReferralCompleted.Patient.FamilyName = PatientReferral.Patient.FamilyName;
                referralCompletedRequest.ReferralCompleted.Patient.GivenName = PatientReferral.Patient.GivenName;
                referralCompletedRequest.ReferralCompleted.AcknowledgementStatus = PatientReferral.AcknowledgementStatus;

                if (option2.Selected)
                {
                    if (FileUpload.HasFile)
                    {
                        Result result = this.ValidateDocumentForPatient(FileUpload.FileBytes, PatientReferral.Patient);
                        if (!result.IsSuccess)
                        {
                            lblmessage.Text = result.ErrorMessage;
                            return;
                        }
                        else
                        {
                            OutcomeDocumentID = Guid.NewGuid().ToString();
                            referralCompletedRequest.ReferralCompleted.OutcomeDocumentID = OutcomeDocumentID;
                            referralCompletedRequest.ReferralCompleted.DocumentBytes = FileUpload.FileBytes;
                            providerValue = PatientReferral.ReferredByEmail;

                            List<string> lstResource = new List<string>();
                            lstResource.Add(OutcomeDocumentID);
                            //lstResource.Add("db25fc0e-23b0-4ae2-8158-cbed124ab75cXACML");  //temp name ..will be changed later

                            List<string> lstSubject = new List<string>();
                            lstSubject.Add(providerValue);
                            lstSubject.Add(Session["UserEmailAddress"].ToString());

                            referralCompletedRequest.ReferralCompleted.XACMLBytes = CreateXACMLPolicy(lstResource, lstSubject);
                            referralCompletedRequest.ReferralCompleted.ReferralCompleted = true;
                            UploadDocumentResult = true;

                            List<NHINCommunity> nhinCommunitiesSession = (List<NHINCommunity>)GlobalSessions.SessionItem(SessionItem.CommunityList);
                            NHINCommunity homeNHINCommunity = nhinCommunitiesSession.Where(t => t.IsHomeCommunity).FirstOrDefault();
                            AssertionHelper assertionHelper = new AssertionHelper();
                            referralCompletedRequest.ReferralCompleted.Assertion = assertionHelper.CreateAssertion(AssertionMode.Default, AssertionAction.DocumentSubmission,
                                PurposeOfUse.TREATMENT, base.GetUserInformation(homeNHINCommunity), homeNHINCommunity);


                        }
                    }
                    else
                    {
                        lblmessage.Text = Helper.GetErrorMessage(ErrorCode.Referral_Outcome_Document_Missing);
                        return;
                    }


                }
                else
                {
                    referralCompletedRequest.ReferralCompleted.ReferralCompleted = false;
                    UploadDocumentResult = true;
                }

                if (UploadDocumentResult)
                {
                    referralCompletedRequest.ReferralCompleted.Patient.Telephones = new List<MobiusServiceLibrary.Telephone>();
                    referralCompletedRequest.ReferralCompleted.Patient.PatientAddress = new List<MobiusServiceLibrary.Address>();

                    SoapHandler.RequestEncryption(referralCompletedRequest, out SoapProperties);
                    referralCompletedRequest.SoapProperties = SoapProperties;

                    if (chkreschdule.Checked)
                    {
                        trHideAppointmentDate.Attributes.Add("style", "display:none");
                        trAppointmentDate.Attributes.Add("style", "display:auto");
                    }
                    else
                    {
                        trHideAppointmentDate.Attributes.Add("style", "display:auto");
                        trAppointmentDate.Attributes.Add("style", "display:none");
                    }

                    getReferralResponse = objProxy.CompletePatientReferral(referralCompletedRequest);
                    if (getReferralResponse.Result.IsSuccess)
                    {
                        if (SoapHandler.ResponseDecryption(getReferralResponse.SoapProperties, getReferralResponse))
                        {
                            documentId = referralCompletedRequest.ReferralCompleted.DocumentId;

                            btnAcknowledge.Enabled = false;
                            btnReset.Enabled = false;

                            if (chkreschdule.Checked)
                                txtReadOnlyAppointmentDate.Text = txtRuleEndDate.Text;

                            trAppointmentDate.Visible = false;
                            trHideAppointmentDate.Visible = true;
                            trReferralCompleted.Visible = false;
                            trReschedule.Visible = false;



                            if (option2.Selected)
                            {
                                lblmessage.Text = Helper.GetErrorMessage(ErrorCode.Referral_Completed);

                            }
                            else if (chkreschdule.Checked)
                            {
                                lblmessage.Text = getReferralResponse.Result.ErrorMessage;

                            }
                        }
                        else
                        {
                            lblmessage.Text = Helper.GetErrorMessage(ErrorCode.Invalid_Response_Data);
                            return;
                        }

                    }
                    else
                    {
                        lblmessage.Text = getReferralResponse.Result.ErrorMessage;
                        return;
                    }
                }
            }

        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);

        }
    }



    #region convertdate
    private string dateFormatter(string date)
    {
        string correctDate = string.Empty;
        if (date.Length == 10)
        {
            string year = date.Substring(6, 4);
            string month = date.Substring(0, 4);
            string date1 = date.Substring(3, 2);
            correctDate = year + month + date1;
        }

        return correctDate;
    }
    #endregion


    #region Helper

    /// <summary>
    /// This method will load the master data 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    private GetMasterDataResponse GetMasterData(MasterCollection type, int value)
    {
        GetMasterDataRequest getMasterDataRequest = new GetMasterDataRequest();
        getMasterDataRequest.MasterCollection = type;
        getMasterDataRequest.dependedValue = value;
        return this.MobiusClient.GetMasterData(getMasterDataRequest);
    }

    #region SetControlValues
    /// <summary>
    /// 
    /// </summary>
    /// <param name="documentId"></param>
    /// <param name="docTitle"></param>
    private void SetControlValues(string documentId, string docTitle)
    {
        string strFileName = "";
        MobiusServiceLibrary.Patient _objPatientInfo = new MobiusServiceLibrary.Patient();
        strFileName = documentId + ".xml";
        hdnDocumentId.Value = documentId;

        List<string> listWithoutMandatorySections = new List<string>();
        List<string> listMandatorySections = new List<string>();
        CDAHelper= new CDAHelper(FileHandler.LoadDocument(documentId, hdnLocation.Value));

        if (CDAHelper.Sections != null && CDAHelper.Sections.Count > 0)
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


    private DateTime ConvertDate(string date, string pattern)
    {
        DateTime retval = DateTime.MinValue;
        if (System.DateTime.TryParseExact(date, pattern, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out retval))
            return retval;
        // Could not convert date..     
        return DateTime.MinValue;
    }

    private void GetAndSetDocumentForReferal()
    {
        GetMasterDataResponse getMasterDataResponse = null;
        trProvideEmail.Visible = true;
        //Fill the purpose of use dropdown list.
        getMasterDataResponse = null;
        getMasterDataResponse = GetMasterData(MasterCollection.PurposeOfUse, 1);
        if (getMasterDataResponse.Result.IsSuccess)
        {
            FillDropDown(ddlPurposeForUse, getMasterDataResponse.MasterDataCollection);
            ddlPurposeForUse.SelectedIndex = 1;
            ddlPurposeForUse.Enabled = false;
        }
        //Load Document Metadata 
        GetDocumentMetadataDocumentIDRequest getDocumentMetadataRequest = new GetDocumentMetadataDocumentIDRequest();
        getDocumentMetadataRequest.documentID = documentId;
        SoapHandler.RequestEncryption(getDocumentMetadataRequest, out SoapProperties);
        getDocumentMetadataRequest.SoapProperties = SoapProperties;
        GetDocumentMetadataResponse getDocumentMetadataResponse = objProxy.GetDocumentMetaDataByDocumentId(getDocumentMetadataRequest);

        if (getDocumentMetadataResponse.Result.IsSuccess)
        {
            if (SoapHandler.ResponseDecryption(getDocumentMetadataResponse.SoapProperties, getDocumentMetadataResponse))
            {
                if (getDocumentMetadataResponse.Documents.Count > 0)
                {
                    BindDocumentGrid(getDocumentMetadataResponse.Documents);
                    hdnLocation.Value = Path.Combine(MobiusAppSettingReader.DocumentPath, getDocumentMetadataResponse.Documents[0].Location);

                }
            }
            else
            {
                lblmessage.Text = Helper.GetErrorMessage(ErrorCode.Invalid_Response_Data);

                return;
            }
        }

        if (!string.IsNullOrEmpty(patientReferralID))
        {
            txtRuleStartDate.Enabled = true;
            txtProviderEmail.Enabled = false;
            ddlPurposeForUse.Enabled = false;
            txtRuleStartDate.Enabled = false;
            imgCalRuleStartDate.Visible = false;
            lbAvalSections.Enabled = false;
            lbSelectedsections.Enabled = false;
            imgbtnMoveLeft.Enabled = false;
            chkRemoveAll.Enabled = false;
            lstcomment.Enabled = false;
            btnShare.Enabled = false;
            chkSelectAll.Enabled = false;
            imgbtnMoveRight.Enabled = false;
            btnShare.Visible = false;
            btnAcknowledge.Visible = true;
            btnAcknowledge.Enabled = true;
            trAppointmentDate.Visible = true;
            trHideAppointmentDate.Visible = true;
            trdispatcherscomments.Visible = true;
            trReferralAcknowledgement.Visible = true;
            trSections.Visible = false;
            //
            getPatientReferralDetails(Convert.ToInt32(patientReferralID));

        }
        else
        {
            if (GlobalSessions.SessionItem(SessionItem.SelectedPatient) == null)
            {
                //
                //Get Patient Object based on Document ID
                if (Request.QueryString["Serial"].ToString() != null && !string.IsNullOrEmpty(Request.QueryString["Serial"].ToString()))
                {

                    GetPatientDetailsResponse getPatientDetailsResponse = new GetPatientDetailsResponse();
                    GetPatientDetailsbyDocumentIdRequest getPatientDetails = new GetPatientDetailsbyDocumentIdRequest();
                    getPatientDetails.DocumentID = documentId;
                    SoapHandler.RequestEncryption(getPatientDetails, out SoapProperties);
                    getPatientDetails.SoapProperties = SoapProperties;
                    getPatientDetailsResponse = objProxy.GetPatientInformationByDocumentID(getPatientDetails);
                    if (getPatientDetailsResponse.Result.IsSuccess)
                    {
                        if (SoapHandler.ResponseDecryption(getPatientDetailsResponse.SoapProperties, getPatientDetailsResponse))
                        {
                            if (getPatientDetailsResponse.Patient != null)
                            {
                                GlobalSessions.SessionAdd(SessionItem.SelectedPatient, getPatientDetailsResponse.Patient);
                            }
                        }

                    }
                }
            }


            if (GlobalSessions.SessionItem(SessionItem.SelectedPatient) != null)
            {
                MobiusServiceLibrary.Patient patient = GlobalSessions.SessionItem(SessionItem.SelectedPatient) as MobiusServiceLibrary.Patient;

                lblgender.Text = patient.Gender.ToString();// == "M" ? "Male" : "Female";
                DateTime myDate = ConvertDate(patient.DOB, "yyyyMMdd");
                lblDOB.Text = myDate.ToShortDateString();
                //
                SetControlValues(documentId, getDocumentMetadataResponse.Documents[0].DocumentTitle);
                //
                trAppointmentDate.Visible = false;
                trHideAppointmentDate.Visible = false;
                trdispatcherscomments.Visible = false;
                trReferralAcknowledgement.Visible = false;
                trReferralCompleted.Visible = false;
                List<string> PatientName = new List<string>();
                PatientName.Add(patient.GivenName[0].ToString());
                PatientName.Add(patient.FamilyName[0].ToString());
                //Bind Session Item For patient Name Again
                {
                    GlobalSessions.SessionAdd(SessionItem.SelectedPatientName, PatientName);
                }
            }
        }
            if (string.IsNullOrEmpty(lblPatientName.Text) &&  GlobalSessions.SessionItem(SessionItem.SelectedPatientName) != null)
            {
                lblPatientName.Text = string.Join(" ", (List<string>)GlobalSessions.SessionItem(SessionItem.SelectedPatientName)); ;
            }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="patientReferralID"></param>
    private void getPatientReferralDetails(int patientReferralID)
    {
        GetPatientReferralRequest getPatientReferralRequest = new GetPatientReferralRequest();
        getPatientReferralRequest.patientReferralId = patientReferralID;
        SoapHandler.RequestEncryption(getPatientReferralRequest, out SoapProperties);
        getPatientReferralRequest.SoapProperties = SoapProperties;

        //Get Referral from db
        GetPatientReferralResponse getPatientReferralDetail = objProxy.GetPatientReferralDetails(getPatientReferralRequest);
        if (getPatientReferralDetail.Result.IsSuccess)
        {
            if (SoapHandler.ResponseDecryption(getPatientReferralDetail.SoapProperties, getPatientReferralDetail))
            {
                if (getPatientReferralDetail.PatientReferrals.Count != 0)
                {
                    //Add Object to session
                    GlobalSessions.SessionAdd(SessionItem.GetPatientReferralResponse, getPatientReferralDetail.PatientReferrals[0]);
                    this.PatientReferral = getPatientReferralDetail.PatientReferrals[0];
                    setPatientReferralDetails();
                }
            }
            else
            {
                lblmessage.Text = Helper.GetErrorMessage(ErrorCode.Invalid_Response_Data);
                return;
            }
        }
    }

    private void setPatientReferralDetails()
    {
        documentId = this.PatientReferral.DocumentId;
        if (!string.IsNullOrEmpty(this.PatientReferral.ReferralAccomplishedOn))
            txtRuleStartDate.Text = this.PatientReferral.ReferralAccomplishedOn;

        if (!string.IsNullOrEmpty(this.PatientReferral.PatientAppointmentDate))
        {
            txtRuleEndDate.Text = this.PatientReferral.PatientAppointmentDate;
            // hide and show Table row at the time of Not Completed referral.
            var results = this.PatientReferral.PatientAppointmentDate.Split(' ');
            txtReadOnlyAppointmentDate.Text = results[0];
            trAppointmentDate.Attributes.Add("style", "display:none");
            trHideAppointmentDate.Attributes.Add("style", "display:block");
        }

        this.lstcomment.Text = this.PatientReferral.ReferralSummary;

        this.txtdispatcherComment.Text = this.PatientReferral.DispatcherSummary;
        txtProviderEmail.Text = this.PatientReferral.ReferredToEmail;
        if (this.PatientReferral.Patient.GivenName.Count >= 1)
        {
            lblPatientName.Text = this.PatientReferral.Patient.GivenName[0].ToString();
        }
        if (this.PatientReferral.Patient.FamilyName.Count >= 1)
        {
            lblPatientName.Text = lblPatientName.Text + " " + this.PatientReferral.Patient.FamilyName[0].ToString();
        }
        localMPIID = this.PatientReferral.Patient.LocalMPIID;
        providerValue = this.PatientReferral.ReferredByEmail;
        lblgender.Text = this.PatientReferral.Patient.Gender.ToString();
        lblDOB.Text = Convert.ToDateTime(this.PatientReferral.Patient.DOB).ToShortDateString();
        if (this.PatientReferral.PurposeOfUseId != 0)
        {
            ddlPurposeForUse.SelectedValue = this.PatientReferral.PurposeOfUseId.ToString();
        }
        if (!this.PatientReferral.AcknowledgementStatus)
        {
            trReferralCompleted.Visible = false;
            trReferralAcknowledgement.Visible = true;
        }
        else
        {
            trReferralCompleted.Visible = true;
            trReferralAcknowledgement.Visible = false;
            trReschedule.Visible = true;
        }
        trOutComeDocument.Visible = false;
        //Referral Process Completed
        if (this.PatientReferral.ReferralCompleted)
        {
            trReferralCompleted.Visible = false;
            trReferralAcknowledgement.Visible = false;
            txtdispatcherComment.Enabled = false;
            btnAcknowledge.Visible = false;
            btnReset.Visible = false;
            btnShare.Visible = false;
            trReschedule.Visible = false;
            trOutComeDocument.Visible = true;

        }

        if (GlobalSessions.SessionItem(SessionItem.UserEmailAddress).ToString() == this.PatientReferral.ReferredByEmail)
        {
            btnAcknowledge.Visible = false;
            btnReset.Visible = false;
            btnShare.Visible = false;
            if (!this.PatientReferral.AcknowledgementStatus)
            {
                rdStatuso1.Selected = false;
                trAppointmentDate.Attributes.Add("style", "display:none");
                trHideAppointmentDate.Attributes.Add("style", "display:block");
            }
            else
            {
                trReschedule.Visible = false;
            }
            trReferralCompleted.Visible = false;



        }

    }




    #endregion Helper

}
