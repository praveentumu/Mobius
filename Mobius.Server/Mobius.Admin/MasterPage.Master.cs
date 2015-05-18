using System;
using FirstGenesis.UI;
using System.IO;
using System.Globalization;
using Mobius.CoreLibrary;

public partial class MasterPage : System.Web.UI.MasterPage
{
    public string pageName;
    public string pageHeading;
    public string sfid;
    public string sFacilityType;
    public string userName;
    public string facilityName;
    public string userType;
    private const string MANAGE_CONSENT_PAGE = "ManageConsent.aspx";
    private const string MANAGE_CONSENT_POLICY_PAGE = "ManageConsentPolicy.aspx";
    private const string ERROR_PAGE = "Error.aspx";
    private const string LOGIN_PAGE = "Login.aspx";
    private const string MANAGE_CONSENT = "Manage Consent";
    private const string DEFAULT = "Default";
    private const string HOME = "Home";
    private const string CURRENT_PAGE = "Current Page:";
    private const string MANAGECOMMUNITY = "Manage Communities";
    private const string IMPORT_COMMUNITY = "Import Communities";
    private const string MANAGE_EMERGENCY_OVERRIDE = "Manage Emergency Override";
    private const string EMERGENCY_OVERRIDE_DETAILS = "Emergency Override Details";
    protected DateTime ValidTill { get; set; }
    public readonly int NOTIFICATION_DURATION = MobiusAppSettingReader.ServiceNotificationGap;

    //Function call on page load.
    protected void Page_Load(object sender, EventArgs e)
    {
        SetPageLinks();
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
            Response.Redirect(LOGIN_PAGE + "?Logout=1", false);
        }
        catch (System.Threading.ThreadAbortException)
        {

        }
        catch (Exception)
        {
            //nothing to do
        }
    }

    protected void Page_Init(object sender, System.EventArgs e)
    {
        try
        {
            if (GlobalSessions.SessionItem(SessionItem.UserID) == null)
            {
                try
                {
                    GlobalSessions.SessionRemoveAll();
                    Session.Abandon();
                    Response.Redirect(LOGIN_PAGE, true);
                }
                catch (Exception)
                {
                    //nothing to do
                }
            }

        }
        catch (Exception ex)
        { }

    }

    private void SetPageLinks()
    {
        TextInfo textInfo = new CultureInfo("en-Us", false).TextInfo;
        pageHeading = textInfo.ToTitleCase(Path.GetFileNameWithoutExtension(Request.Url.AbsolutePath).Replace("User", " User"));


        if (GlobalSessions.SessionItem(SessionItem.ValidTill) != null)
        {
            ValidTill = (DateTime)GlobalSessions.SessionItem(SessionItem.ValidTill);
        }

        //Set Page LInks
        if (pageHeading == DEFAULT)
        {
            pageHeading = HOME;
            LblHeading.Text = CURRENT_PAGE + " " + pageHeading;
        }
        else if (pageHeading == "Activatedeactivate User")
        {
            pageHeading = "Activate/Deactivate User";
            LblHeading.Text = CURRENT_PAGE + " " + pageHeading;
        }
        else if (pageHeading == "Changeemailaddress")
        {
            pageHeading = "Account Settings";
            LblHeading.Text = CURRENT_PAGE + " " + pageHeading;
        }
        else if (pageHeading == "ChangePassword")
        {
            pageHeading = "Change Password";
            LblHeading.Text = CURRENT_PAGE + " " + pageHeading;
        }
        else if (pageHeading == "Managecommunities")
        {
            pageHeading = MANAGECOMMUNITY;
            LblHeading.Text = CURRENT_PAGE + " " + pageHeading;
        }
        else if (pageHeading == "Importcommunities")
        {
            pageHeading = IMPORT_COMMUNITY;
            LblHeading.Text = CURRENT_PAGE + " " + pageHeading;
        }
        else if (pageHeading == "Manageemergencyoverride")
        {
            pageHeading = MANAGE_EMERGENCY_OVERRIDE;
            LblHeading.Text = CURRENT_PAGE + " " + pageHeading;
        }
        else if (pageHeading == "Emergencyoverridedetails")
        {
            pageHeading = EMERGENCY_OVERRIDE_DETAILS;
            LblHeading.Text = CURRENT_PAGE + " " + pageHeading;
        }
        else
        {
            LblHeading.Text = CURRENT_PAGE + " " + pageHeading;
        }

    }

}
