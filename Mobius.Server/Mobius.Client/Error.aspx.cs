using System;
using FirstGenesis.UI;

public partial class Error : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string message = "Authentication failed.";
        lblerrorMessage.Font.Bold = true;
        if (Convert.ToString(Request.QueryString["Logout"]) == "1")
        {
            lblerrorMessage.Text = "You are successfully logged out from Mobius HISE.";
            return;
        }

        if (Convert.ToString(Request.QueryString["Logout"]) == "2")
        {
            lblerrorMessage.Text = "Your session has expired. ";
            return;
        }
        
        if (GlobalSessions.SessionItem(SessionItem.ErrorMessage) != null && !string.IsNullOrEmpty(GlobalSessions.SessionItem(SessionItem.ErrorMessage).ToString()))
        {
            message = GlobalSessions.SessionItem(SessionItem.ErrorMessage).ToString();            
        }
        
        lblerrorMessage.Text = message;
    }
   
}