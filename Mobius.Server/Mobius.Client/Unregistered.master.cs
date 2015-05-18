using System;

public partial class Unregistered : System.Web.UI.MasterPage
{
    public string pageHeading;
    public string pageName;
    public string sFacilityType;
    public string userName;
    protected void Page_Load(object sender, EventArgs e)
    {
        string URL = Request.Url.AbsolutePath;
        System.IO.FileInfo oInfo = new System.IO.FileInfo(URL);
        pageName = oInfo.Name;
        pageHeading = pageName.Replace(".aspx", "");

        if (pageName == "RegisterPatient.aspx")
        {
            pageHeading = "Register Patient";
            LblHeading.Text = pageHeading;
        }
        if (pageName == "RegisterProvider.aspx")
        {
            pageHeading = "Register Provider";
            LblHeading.Text = pageHeading;
        }

    }
}
