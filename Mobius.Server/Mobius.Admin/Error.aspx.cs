using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstGenesis.UI;

public partial class Error : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //ErrorMessage.Text="Authentication failed.";

        string authmessage = string.Empty;
        ErrorMessage.Font.Bold = true;
        if (Convert.ToString(Request.QueryString["Logout"]) == "1")
        {
            ErrorMessage.Text = "You are successfully logged out from Mobius HISE";
            return;
        }
        if (Convert.ToString(Request.QueryString["Logout"]) == "2")
        {
            ErrorMessage.Text = "Your session has expired. ";
            return;
        }
        if (GlobalSessions.SessionItem(SessionItem.ViewError) == null || string.IsNullOrEmpty(GlobalSessions.SessionItem(SessionItem.ViewError).ToString()))
        {
            authmessage = "Authentication failed.";
            ErrorMessage.Text = authmessage;
        }
        else
        {
            ErrorMessage.Text = GlobalSessions.SessionItem(SessionItem.ViewError).ToString();
        }
    }
}