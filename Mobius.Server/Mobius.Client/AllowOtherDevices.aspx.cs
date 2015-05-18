using System;
using FirstGenesis.UI;
using System.IO;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Web.UI;
using System.Security.Cryptography;
using MobiusServiceLibrary;
using Mobius.CoreLibrary;
using MobiusServiceUtility;

public partial class AllowOtherDevices : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblErrorMsg.Text = string.Empty;
        btnPwd.Attributes.Add("onClick", "return hasWhiteSpace('"+txtPassword.ClientID+"');");
        txtPassword.Focus();
        
    }
    protected void btnPwd_Click(object sender, EventArgs e)
    {
        X509Store store = null;
        X509Certificate2Collection certCollection = null;
        X509Certificate2 certificate = null;
        AddPFXCertificateRequest addPFXCertificateRequest = null;
        AddPFXCertificateResponse addPFXCertificateResponse = null;
        MobiusSecuredClient SecuredClient = null;
        SoapHandler soapHandler = null;
        SoapProperties soapProperties = null;
       
        try
        {

            string userEmailAddress = string.Empty;
            string serialNumber = string.Empty;
            addPFXCertificateRequest = new AddPFXCertificateRequest();
            addPFXCertificateResponse = new AddPFXCertificateResponse();
            SecuredClient = new MobiusSecuredClient();
            soapHandler = new SoapHandler(Convert.ToString(GlobalSessions.SessionItem(SessionItem.SerialNumber)));
            soapProperties = new SoapProperties();
            if (Page.IsValid)
            {
                if (GlobalSessions.SessionItem(SessionItem.SerialNumber) != null)
                {

                    userEmailAddress = (string)GlobalSessions.SessionItem(SessionItem.UserEmailAddress);
                    serialNumber = (string)GlobalSessions.SessionItem(SessionItem.SerialNumber);
                    store = new X509Store(StoreLocation.CurrentUser);
                    store.Open(OpenFlags.MaxAllowed);
                    certCollection = store.Certificates.Find(X509FindType.FindBySerialNumber, serialNumber, true);
                    certificate = new X509Certificate2(certCollection[0]);
                    if (certificate != null)
                    {
                        SecuredClient.ClientCredentials.ClientCertificate.Certificate = certificate;
                    }
                    byte[] certData = certificate.Export(X509ContentType.Pfx, txtPassword.Text);

                    addPFXCertificateRequest.UserType = (UserType)GlobalSessions.SessionItem(SessionItem.UserType);
                    addPFXCertificateRequest.EmailAddress = userEmailAddress;
                    addPFXCertificateRequest.Certificate = Convert.ToBase64String(certData);
                    soapHandler.RequestEncryption(addPFXCertificateRequest, out soapProperties);
                    addPFXCertificateRequest.SoapProperties = soapProperties;
                    addPFXCertificateResponse = SecuredClient.AddPFXCertificate(addPFXCertificateRequest);
                    if (!addPFXCertificateResponse.Result.IsSuccess)
                    {
                        string message = Helper.GetErrorMessage(ErrorCode.Certificate_Export_Failed);
                        var script = "alert('" + message + "');";
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "CertError", script, true);
                    }
                    else
                    {
                        lblErrorMsg.Text = Helper.GetErrorMessage(ErrorCode.Successful_Export_Certificate_Message);
                        btnPwd.Visible = false;
                    }
                }
            }
        }
        catch (CryptographicException ex)
        {
            string message = Helper.GetErrorMessage(ErrorCode.Certificate_Export_Failed);
            var script = "alert('" + message + "');";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CertError", script, true);
        }
        catch (Exception ex)
        {
            string message = Helper.GetErrorMessage(ErrorCode.Certificate_Export_Failed);
            var script = "alert('" + message + "');";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CertError", script, true);
        }
        finally
        {
            store = null;
            certCollection = null;
            certificate = null;
        }

    }
}