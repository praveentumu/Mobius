using System;
using Mobius.CoreLibrary;
using FirstGenesis.UI;
using Mobius.BAL;
using Mobius.Entity;
using System.Collections.Generic;

public partial class ChangePassword : System.Web.UI.Page
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
        string message = string.Empty;
        Result result = null;
        try
        {
            AdminDetails adminDetails = new AdminDetails();
            result = new Result();
            result.IsSuccess = true;
            if (string.IsNullOrEmpty(txtNewPassword.Text))
            {
                result.IsSuccess = false;
                result.SetError(ErrorCode.New_Password_Missing);
            }
            if (string.IsNullOrEmpty(txtOldPassword.Text))
            {
                result.IsSuccess = false;
                result.SetError(ErrorCode.Old_Password_Missing);
            }

            if (txtOldPassword.Text == txtNewPassword.Text)
            {
                result.IsSuccess = false;
                result.SetError(ErrorCode.OldPassword_And_NewPassword_Equal);
            }
            if (!Validator.ValidatePassword(txtNewPassword.Text, out message))
            {
                result.IsSuccess = false;
                result.SetError(ErrorCode.Invalid_Password);
            }
            if (!this.GetuserInformation(txtOldPassword.Text).IsSuccess)
            {
                result.IsSuccess = false;
                result.SetError(ErrorCode.Incorrect_Old_Password);
            }
            if (result.IsSuccess)
            {
                if (GlobalSessions.SessionItem(SessionItem.UserID) != null)
                {
                    adminDetails.ID = (int)GlobalSessions.SessionItem(SessionItem.UserID);
                    // EncryptData password
                    adminDetails.Password = Helper.EncryptData(txtNewPassword.Text);

                    if (this.UpdateDetails(adminDetails).IsSuccess)
                    {
                        lblErrorMsg.Text = PASS_CHANGED_SUCCESS;
                        Button1.Visible = false;
                        btnclose.Visible = true;
                    }
                    else
                    {
                        lblErrorMsg.Text = PASS_CHANGED_FAIL;
                    }
                }
            }
            else
            {
                lblErrorMsg.Text = result.ErrorMessage;
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(Page, ex: ex, IsPopup: true);
        }

    }

    /// <summary>
    /// UpdateDetails
    /// </summary>
    /// <param name="adminDetails"></param>
    /// <returns></returns>
    private Result UpdateDetails(AdminDetails adminDetails)
    {
        Result result = null;
        try
        {
            MobiusBAL bal = new MobiusBAL();
            result = new Result();
            result = bal.UpdateAdminDetails(adminDetails);
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return result;
    }

    /// <summary>
    /// for validate old password
    /// </summary>
    private Result GetuserInformation(string oldpassword)
    {
        AdminDetails adminDetail = null;
        List<AdminDetails> adminDetails = null;
        Result result = null;
        try
        {
            MobiusBAL bal = new MobiusBAL();
            result = new Result();
            adminDetail = new AdminDetails();
            if (GlobalSessions.SessionItem(SessionItem.UserEmailAddress) != null && !string.IsNullOrEmpty(oldpassword))
            {
                adminDetail.UserName = (string)GlobalSessions.SessionItem(SessionItem.UserEmailAddress);

                adminDetail.Password = Helper.EncryptData(oldpassword);
                result = bal.GetAdminDetails(adminDetail, out adminDetails);
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
        return result;
    }
}