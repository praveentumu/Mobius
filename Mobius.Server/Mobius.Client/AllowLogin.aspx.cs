using System;
using FirstGenesis.UI;
using Mobius.Entity;
using System.Security.Cryptography.X509Certificates;
using Mobius.CoreLibrary;
using System.IO;
using MobiusServiceLibrary;

public partial class AllowLogin : System.Web.UI.Page
{
    private const string LOGIN_FAILED = "Invalid credentials.";
    MobiusClient objProxy = null;
    protected void Page_Load(object sender, EventArgs e)
    {

        lblMessage.Text = "";
    }
    protected void ImportPFXCertificate_Click(object sender, EventArgs e)
    {
        string certificate = string.Empty;
        try
        {
            if (IsValid)
            {

                objProxy = new MobiusClient();
                GetPFXCertificateResponse getPFXCertificateResponse = new GetPFXCertificateResponse();
                GetPFXCertificateRequest getPFXCertificateRequest = new GetPFXCertificateRequest();
                Result result = new Result();

                getPFXCertificateRequest.UserType = (UserType)Enum.Parse(typeof(UserType), rbtPatient.Checked ? UserType.Patient.ToString() : UserType.Provider.ToString(), true);
                getPFXCertificateRequest.EmailAddress = txtMail.Text;
                getPFXCertificateResponse = objProxy.GetPFXCertificate(getPFXCertificateRequest);

                if (getPFXCertificateResponse.Result.IsSuccess)
                {
                    result = this.ImportUserCertificate(getPFXCertificateResponse);

                    if (result.IsSuccess)
                    {
                        if (GlobalSessions.SessionItem(SessionItem.Token) != null)
                        {
                            GlobalSessions.SessionAdd(SessionItem.Token, null);
                        }
                        GlobalSessions.SessionRemoveAll();
                        Session.Abandon();

                        lblMessage.Text = "Account access enabled.";

                        ImportPFXCertificate.Visible = false;
                    }
                    else
                    {
                        lblMessage.Text = result.ErrorMessage;
                    }

                }
                else
                {
                    lblMessage.Text = getPFXCertificateResponse.Result.ErrorMessage;
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write(" <script language='javascript' type='text/javascript'>alert('" + ex.Message.ToString() + "');  top.location.href = 'login.aspx' </script>");
        }
    }

    #region ImportUserCertificate
    /// <summary>
    /// Import PFX Certificate
    /// </summary>
    /// <param name="getPFXCertificateResponse"></param>
    /// <returns></returns>
    private Result ImportUserCertificate(GetPFXCertificateResponse getPFXCertificateResponse)
    {
        X509Store store = null;
        X509Certificate2 certificate = null;
        Result result = new Result();
        try
        {
            if (!string.IsNullOrEmpty(getPFXCertificateResponse.PFXCertificate) && txtPassword.Text.Trim() != "")
            {
                certificate = new X509Certificate2(Convert.FromBase64String(getPFXCertificateResponse.PFXCertificate), txtPassword.Text.Trim(), X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
                store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadWrite);
                store.Add(certificate);
                result.IsSuccess = true;
            }
            else
            {
                result.ErrorMessage = LOGIN_FAILED;
                result.IsSuccess = false;
            }
        }
        catch (Exception ex)
        {
            if (!string.IsNullOrWhiteSpace(ex.Message))
            {
                if (ex.Message.Contains("The specified network password is not correct"))
                {
                    result.SetError(ErrorCode.UnknownException, "The specified security password is not correct.");
                    result.IsSuccess = false;
                }
            }
            else
            {
                result.SetError(ErrorCode.UnknownException, ex.Message);
                result.IsSuccess = false;
            }
        }
        finally
        {
            if (store != null)
            {
                store.Close();
            }
        }
        return result;
    }
    #endregion


}