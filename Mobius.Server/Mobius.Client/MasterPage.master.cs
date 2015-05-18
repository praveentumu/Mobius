using System;
using FirstGenesis.UI;
using System.IO;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Web.UI;
using System.Security.Cryptography;
using MobiusServiceLibrary;
using Mobius.CoreLibrary;

public partial class MasterPage : System.Web.UI.MasterPage
{
    #region Variables
    protected string pageName = string.Empty;
    protected string pageHeading = string.Empty;
    protected string userName = string.Empty;
    protected string userType = string.Empty;
    protected bool IsCertificateExpired { get; set; }
    protected DateTime ValidTill { get; set; }
    
    private const string DOCUMENT_LIST_PAGE = "DocumentList.aspx";
    private const string DEFAULT_PAGE = "Default.aspx";
    private const string SEARCH_USER_PAGE = "SearchUser.aspx";
    private const string MANAGE_REFERRAL_PAGE = "ManageReferral.aspx";
    private const string REFER_PATIENT_PAGE = "ReferPatient.aspx";
    private const string ERROR_PAGE = "Error.aspx";
    private const string LOGIN_PAGE = "Login.aspx";
    private const string Adcance_Search_Patient = "AdvanceSearchPatient.aspx";
    private const string EMERGENCY_OVERRIDE_DETAILS_PAGE = "EmergencyOverrideDetails.aspx";
    private const string VIEW_EMERGENCY_OVERRIDE_PAGE = "ViewEmergencyOverride.aspx";

    private const string DOCUMENT_LIST_PAGE_HEADING = "Document List";
    private const string EDIT_USER_PAGE_HEADING = "Edit User";
    private const string SEARCH_PATIENT_PAGE_HEADING = "Search Patient";
    private const string MANAGE_REFERRAL_PAGE_HEADING = "Manage Referral";
    private const string REFER_PATIENT_PAGE_HEADING = "Refer Patient";
    private const string HOME_PAGE_HEADING = "Home";
    private const string USER_TYPE_PATIENT = "Patient";
    private const string USER_TYPE_PROVIDER = "Provider";
    private const string MANAGE_CONSENT = "Manageconsent";
    private const string MANAGE_CONSENT_PAGE_HEADING = "Manage Consent";
    private const string MANAGE_CONSENT_POLICY = "Manageconsentpolicy";
    private const string MANAGE_CONSENT_PAGE_POLICY_HEADING = "Manage Consent Policy";
    private const string UPDATE_PATIENT_DETAILS = "UpdatePatientDetails";
    private const string UPDATE_PATIENT_DETAILS_HEADING = "Update Patient Details";
    private const string Adcance_Search_Patient_Heading = "Advance Search Patient";
    private const string EMERGENCY_OVERRIDE_DETAILS_HEADING= "Emergency Override Details";
    private const string VIEW_EMERGENCY_OVERRIDE_HEADING = "View Emergency Override";

    public readonly int NOTIFICATION_DURATION = MobiusAppSettingReader.UserUpgradationNotificationGap;
    #endregion

    // Function call on page load.
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            
            if (GlobalSessions.SessionItem(SessionItem.SerialNumber) != null &&
                GlobalSessions.SessionItem(SessionItem.UserName) != null &&
                GlobalSessions.SessionItem(SessionItem.UserType) != null &&
                GlobalSessions.SessionItem(SessionItem.ValidTill) != null)
            {
                userName = GlobalSessions.SessionItem(SessionItem.UserName).ToString();
                userType = GlobalSessions.SessionItem(SessionItem.UserType).ToString();
                ValidTill = (DateTime)GlobalSessions.SessionItem(SessionItem.ValidTill);
                hlkChangePassword.Visible = true;
            }

            SetPageLinks();
        }

        catch (System.Threading.ThreadAbortException Thread)
        {
            ExceptionHelper.HandleException(Page, ex: Thread, title: ExceptionHelper.ErrorTitle.Error);
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(Page, ex: ex, title: ExceptionHelper.ErrorTitle.Error);
        }


    }

    protected void Page_Init(object sender, System.EventArgs e)
    {
        try
        {
            if ((string)GlobalSessions.SessionItem(SessionItem.SerialNumber) == null)
            {
                GlobalSessions.SessionRemoveAll();
                Session.Abandon();
                Response.Redirect(LOGIN_PAGE);
            }
        }
        catch (System.Threading.ThreadAbortException)
        { }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(Page, ex: ex, title: ExceptionHelper.ErrorTitle.Error);
        }
    }

    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        try
        {

            if (GlobalSessions.SessionItem(SessionItem.Token) != null)
            {
                GlobalSessions.SessionAdd(SessionItem.Token, null);
            }
            GlobalSessions.SessionRemoveAll();
            Session.Abandon();
            Response.Redirect(LOGIN_PAGE + "?Logout=1");
        }
        catch (System.Threading.ThreadAbortException)
        { }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(Page, ex: ex, title: ExceptionHelper.ErrorTitle.Error);

        }
    }

    private void SetPageLinks()
    {
        TextInfo textInfo = new CultureInfo("en-Us", false).TextInfo;
        string URL = Request.Url.AbsolutePath;
        System.IO.FileInfo oInfo = new System.IO.FileInfo(URL);
        pageName = oInfo.Name;
        pageHeading = textInfo.ToTitleCase(Path.GetFileNameWithoutExtension(Request.Url.AbsolutePath).Replace("User", " User"));

        //Set Page headings/title 
        if (GlobalSessions.SessionItem(SessionItem.LinkUserGUID) != null)
        {

            if ((string)GlobalSessions.SessionItem(SessionItem.LinkUserGUID) + "" != "")
            {
                pageHeading = EDIT_USER_PAGE_HEADING;
                GlobalSessions.SessionAdd(SessionItem.LinkUserGUID, "");
            }
        }
        else
        {
            if (pageName == DOCUMENT_LIST_PAGE)
            {
                LblHeading.Text = DOCUMENT_LIST_PAGE_HEADING;
            }
            else if (pageName == Adcance_Search_Patient)
            {
                LblHeading.Text = Adcance_Search_Patient_Heading;
            }
            else if (pageName == DEFAULT_PAGE)
            {
                LblHeading.Text = HOME_PAGE_HEADING;
            }
            else if (pageHeading.ToUpper() == MANAGE_CONSENT.ToUpper())
            {
                LblHeading.Text = MANAGE_CONSENT_PAGE_HEADING;
            }
            else if (pageHeading.ToUpper() == MANAGE_CONSENT_POLICY.ToUpper())
            {
                LblHeading.Text = MANAGE_CONSENT_PAGE_POLICY_HEADING;
            }
            else if (pageHeading.ToUpper() == UPDATE_PATIENT_DETAILS.ToUpper())
            {
                LblHeading.Text = UPDATE_PATIENT_DETAILS_HEADING;
            }
            else if (pageName == SEARCH_USER_PAGE)
            {
                //hlkSearchUser.CssClass = "bluetext:hover";
                LblHeading.Text = SEARCH_PATIENT_PAGE_HEADING;
            }
            else if (pageName == MANAGE_REFERRAL_PAGE)
            {
                //hlkManageReferral.CssClass = "bluetext:hover";
                LblHeading.Text = MANAGE_REFERRAL_PAGE_HEADING;
            }
            else if (pageName == REFER_PATIENT_PAGE)
            {
                LblHeading.Text = REFER_PATIENT_PAGE_HEADING;
            }
            else if (pageName == EMERGENCY_OVERRIDE_DETAILS_PAGE)
            {
                LblHeading.Text = EMERGENCY_OVERRIDE_DETAILS_HEADING;
            }
            else if (pageName == VIEW_EMERGENCY_OVERRIDE_PAGE)
            {
                LblHeading.Text = VIEW_EMERGENCY_OVERRIDE_HEADING;
            }

            else
                LblHeading.Text = pageHeading;
        }
        //Set Visibility of appropriate Links as per user profile/type
        if (userType == USER_TYPE_PATIENT)
        {
            hlkSearchUser.Visible = false;
            hlkAdvanceSearchPatient.Visible = false;
            hlkdocumentlist.Visible = true;
            hlkManageConsentPolicy.Visible = true;
            hlkUpdatePatientDetails.Visible = true;
            hlkViewEmergencyOverride.Visible = true;
        }
        else if (userType == USER_TYPE_PROVIDER)
        {
            hlkSearchUser.Visible = true;
            hlkAdvanceSearchPatient.Visible = true;
            hlkManageReferral.Visible = true;
        }
        else
        {
            hlkSearchUser.Visible = false;
            hlkAdvanceSearchPatient.Visible = false;
            hlkManageReferral.Visible = false;
            hlkdocumentlist.Visible = false;
            hlkManageConsentPolicy.Visible = false;
            hlkUpdatePatientDetails.Visible = false;
            hypUserPreferences.Visible = false;

            pageHeading = HOME_PAGE_HEADING;
            LblHeading.Text = pageHeading;
        }

        
        
    }

}
