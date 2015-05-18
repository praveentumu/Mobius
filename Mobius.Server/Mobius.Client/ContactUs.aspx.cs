using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstGenesis.UI;
using Mobius.CoreLibrary;

public partial class ContactUs : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrorMsg.Text = string.Empty;
            if (!string.IsNullOrEmpty(GlobalSessions.SessionItem(SessionItem.UserEmailAddress).ToString()))
            {
                string Username = GlobalSessions.SessionItem(SessionItem.UserEmailAddress).ToString();
                IEmailTemplate Emailtemplate = new ExpiredCertificate(Username);
                EmailHelper.SendMail(EmailConfiguration.SmtpHost, EmailConfiguration.SmtpPort,
                EmailConfiguration.SmtpUserName, EmailConfiguration.SmtpPassword, EmailConfiguration.SmtpSSL,
                Username,
                EmailConfiguration.SmtpUserName, Emailtemplate.Subject, Emailtemplate.Emailbody);
                lblErrorMsg.Text = "Mail has been sent to system administrator. Admin will contact you soon.";
            }

        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = ex.Message.ToString();
            ExceptionHelper.HandleException(Page, ex: ex, IsPopup: true);
        }
    }
}