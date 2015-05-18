using System;
using FirstGenesis.UI.Base;
using Mobius.CoreLibrary;
using MobiusServiceLibrary;
using FirstGenesis.UI;
using MobiusServiceUtility;

public partial class ChangePassword : BaseClass
{
    #region Variables
    private const string PASS_CHANGED_SUCCESS = "Password changed successfully.";
    private const string PASS_CHANGED_FAIL = "Password change failed.";
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected void btnChangePassword_Click(object sender, EventArgs e)
    {
        lblErrorMsg.Text = string.Empty;
        try
        {
            if (IsValid)
            {
                SoapHandler soapHandler = new SoapHandler(Convert.ToString(GlobalSessions.SessionItem(SessionItem.SerialNumber)));
                SoapProperties soapProperties = new SoapProperties();
                ChangePasswordRequest changePasswordRequest = new ChangePasswordRequest();
                changePasswordRequest.EmailAddress = GlobalSessions.SessionItem(SessionItem.UserEmailAddress).ToString();
                changePasswordRequest.OldPassword = txtOldPassword.Text;
                changePasswordRequest.NewPassword = txtNewPassword.Text;

                changePasswordRequest.UserType = (Mobius.CoreLibrary.UserType)Enum.Parse(typeof(UserType), GlobalSessions.SessionItem(SessionItem.UserType).ToString(), true);
                soapHandler.RequestEncryption(changePasswordRequest, out soapProperties);
                changePasswordRequest.SoapProperties = soapProperties;
                ChangePasswordResponse changePasswordResponse = new ChangePasswordResponse();
                changePasswordResponse = objProxy.ChangePassword(changePasswordRequest);
                if (changePasswordResponse.Result.IsSuccess)
                {
                    lblErrorMsg.Text = PASS_CHANGED_SUCCESS;
                    Button1.Visible = false;
                }
                else
                {
                    lblErrorMsg.Text = changePasswordResponse.Result.ErrorMessage;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(Page, ex: ex, IsPopup: true);
        }

    }
}