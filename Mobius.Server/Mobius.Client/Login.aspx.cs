using System;
using System.Web.UI;
using FirstGenesis.UI;
using Mobius.CoreLibrary;
using MobiusServiceLibrary;
public partial class Login : System.Web.UI.Page
{

    private string SerialNumber = string.Empty;
    private const string LOGIN_FAILED = "Login failed.";
    private const string DEFAULT_PAGE = "Default.aspx";
    private const string REGISTER_PROVIDER_PAGE = "RegisterProvider.aspx";
    private const string REGISTER_PATIENT_PAGE = "RegisterPatient.aspx";
    protected MobiusClient objProxy = null;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            GlobalSessions.SessionRemoveAll();

            if (Request.QueryString["token"] != null && Request.QueryString["patientId"] != null && Request.QueryString["Serial"] != null)
            {
                GlobalSessions.SessionAdd(SessionItem.SerialNumber, Request.QueryString["Serial"]);

                Response.Redirect("ViewShareDocument.aspx?token=" + Request.QueryString["token"].ToString()
                                              + "&patientId=" + Request.QueryString["patientId"].ToString(), true);
            }

            if (Request.QueryString["DocumentID"] != null && Request.QueryString["PatientReferralID"] != null && Request.QueryString["Serial"] != null)
            {

                GlobalSessions.SessionAdd(SessionItem.SerialNumber, Request.QueryString["Serial"]);

                Response.Redirect("ReferPatient.aspx?DocumentID=" + Request.QueryString["DocumentID"].ToString()
                                   + "&PatientReferralID=" + Request.QueryString["PatientReferralID"].ToString(), true);
            }

            if (Request.QueryString["DocumentID"] != null && Request.QueryString["Serial"] != null)
            {
                GlobalSessions.SessionAdd(SessionItem.SerialNumber, Request.QueryString["Serial"]);

                Response.Redirect("ReferPatient.aspx?DocumentID=" + Request.QueryString["DocumentID"].ToString()
                                     + "&Serial=" + Request.QueryString["Serial"].ToString(), true);
            }

            if (Request.QueryString["IsExpired"] != null && Request.QueryString["Email"]!=null)
            {
                GlobalSessions.SessionAdd(SessionItem.UserEmailAddress, Request.QueryString["Email"].ToString());
                string strScript=@"<script>";
                strScript+="var caption = 'Contact Us';";
                strScript+="var url = 'ContactUs.aspx';";
                strScript+=" open_popup('CENTER', 'CENTER', url, caption, 320, 170, false);";
                strScript += "</script>";
                ClientScript.RegisterClientScriptBlock(Page.GetType(), "ContactUs", strScript);
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }

    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {

            lblmessage.Text = "";

            if (Page.IsValid)
            {

                if (!string.IsNullOrWhiteSpace(this.GetSerialNumber()))
                {
                    Response.Redirect(DEFAULT_PAGE, false);
                }
            }
        }
        catch (Exception ex)
        {
            lblmessage.Text = LOGIN_FAILED;
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }

    }
    protected void btnRegister_Click(object sender, EventArgs e)
    {
        if (rbtPatient.Checked)
        {
            Response.Redirect(REGISTER_PATIENT_PAGE, false);
        }
        else
        {
            Response.Redirect(REGISTER_PROVIDER_PAGE, false);
        }
    }


    private string GetSerialNumber()
    {
        objProxy = new MobiusClient();
        string certificateSerialNumber = string.Empty;
        AuthenticateUserResponse response = new AuthenticateUserResponse();
        AuthenticateUserRequest request = new AuthenticateUserRequest();
        request.EmailAddress = txtMail.Text;
        //TBD
        GlobalSessions.SessionAdd(SessionItem.UserEmailAddress, txtMail.Text);
        request.Password = txtPassword.Text;
        request.UserType = (UserType)Enum.Parse(typeof(UserType), rbtPatient.Checked ? UserType.Patient.ToString() : UserType.Provider.ToString(), true);
        response = objProxy.AuthenticateUser(request);
        if (!response.Result.IsSuccess)
        {
            lblmessage.Text = response.Result.ErrorMessage;
            return string.Empty;
        }
        GlobalSessions.SessionAdd(SessionItem.SerialNumber, response.CertificateSerialNumber);
        objProxy = null;
        return response.CertificateSerialNumber;

    }
}