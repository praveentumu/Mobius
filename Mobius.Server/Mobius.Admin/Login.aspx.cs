using System;
using System.Web.UI;
using FirstGenesis.UI;
using Mobius.Entity;
using Mobius.BAL;
using System.Collections.Generic;
using Mobius.CoreLibrary;
using System.Security.Cryptography.X509Certificates;
public partial class Login : System.Web.UI.Page
{


    private string SerialNumber = string.Empty;
    private const string LOGIN_FAILED = "Login failed.";
    private const string DEFAULT_PAGE = "Default.aspx";
    private const string REGISTER_PATIENT_PAGE = "RegisterPatient.aspx";
   
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            GlobalSessions.SessionRemoveAll();
            lblmessage.Text = "";
            if (Page.IsValid)
            {
          
                    if (this.GetuserInformation().IsSuccess)
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

        Response.Redirect(REGISTER_PATIENT_PAGE, false);

    }


    /// <summary>
    /// GetuserInformation
    /// </summary>
    private Result GetuserInformation()
    {
        AdminDetails adminDetail = null;
        List<AdminDetails> adminDetails = null;
        Result result = null;
        try
        {
            MobiusBAL bal = new MobiusBAL();
            result = new Result();
            adminDetail = new AdminDetails();
            adminDetail.UserName = txtMail.Text;
            adminDetail.Password = Helper.EncryptData(txtPassword.Text);
            result = bal.GetAdminDetails(adminDetail, out adminDetails);
            if (adminDetails != null && adminDetails.Count > 0)
            {
                GlobalSessions.SessionAdd(SessionItem.UserID, adminDetails[0].ID);
                GlobalSessions.SessionAdd(SessionItem.UserEmailAddress, txtMail.Text);
            }
            if (!result.IsSuccess)
            {
                lblmessage.Text = LOGIN_FAILED;

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return result;
    }


  

   

}
