using System;
using MobiusServiceLibrary;
using Mobius.CoreLibrary;
using FirstGenesis.UI;

public partial class ForgotPassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMessage.Text = "";
    }
    protected void Button1_Click1(object sender, EventArgs e)
    {
        try
        {
            if (IsValid)
            {
                lblMessage.Text = "";
                MobiusClient objProxy = new MobiusClient();

                ForgotPasswordRequest forgotPasswordRequest = new ForgotPasswordRequest();
                ForgotPasswordResponse forgotPasswordResponse = null;
                forgotPasswordRequest.EmailAddress = txtEmailAddress.Text;
                forgotPasswordRequest.UserType = rbtPatient.Checked ? UserType.Patient : UserType.Provider;
                forgotPasswordResponse = objProxy.ForgotPassword(forgotPasswordRequest);
                if (forgotPasswordResponse.Result.IsSuccess)
                {
                    lblMessage.Text = forgotPasswordResponse.Result.ErrorMessage;
                    Button1.Visible = false;
                }
                else
                {
                    lblMessage.Text = forgotPasswordResponse.Result.ErrorMessage;
                }
            }

        }
        catch (Exception ex)
        {

            ExceptionHelper.HandleException(Page, ex: ex, IsPopup: true);
        }
    }
}