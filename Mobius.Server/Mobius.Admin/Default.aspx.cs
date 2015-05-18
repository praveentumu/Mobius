#region Using Directives
using System;
using System.Security.Cryptography.X509Certificates;
using Mobius.CoreLibrary;
using FirstGenesis.UI;

#endregion


public partial class _Default : System.Web.UI.Page
{
    #region Properties
    X509Certificate2Collection certCollection = null;
    X509Store store = null;
    private const string IssuerName = "MobiusCA";
    #endregion

    #region Page Load event
    protected void Page_Load(object sender, EventArgs e)
    {
        bool isVerified = false;
        if (GlobalSessions.SessionItem(SessionItem.CertificateValidityVerified) != null)
            isVerified = (bool)GlobalSessions.SessionItem(SessionItem.CertificateValidityVerified);
        if (!isVerified)
        {
            CheckCertificateValidity();
            GlobalSessions.SessionAdd(SessionItem.CertificateValidityVerified, true);
        }
    }
    #endregion

    

    #region HelperMethods

    private int CountRemainingDays(X509Certificate2 Certificate)
    {
        return (int)(Certificate.NotAfter - DateTime.Today).TotalDays;
    }

    private bool IsExpiredCertificate(X509Certificate2 Certificate)
    {
        if (Certificate.NotAfter <= DateTime.Today)
            return true;
        else
            return false;

    }

    private void CheckCertificateValidity()
    {

        //Nothing to do
        X509Certificate2 ServerCert = null;
        try
        {
            store = new X509Store();
            store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadWrite);
            certCollection = store.Certificates.Find(X509FindType.FindByIssuerName, IssuerName, true);
            if (certCollection != null && certCollection.Count > 0)
            {
                if (certCollection.Count > 1)
                {

                    //return false if number of certifcates are more than one 
                }
                ServerCert = certCollection[0];
                if (IsExpiredCertificate(ServerCert))  //check if certificate is expired
                {
                    IEmailTemplate Emailtemplate = new ExpiredServerCertificate();
                    EmailHelper.SendMail(EmailConfiguration.SmtpHost, EmailConfiguration.SmtpPort,
                    EmailConfiguration.SmtpUserName, EmailConfiguration.SmtpPassword, EmailConfiguration.SmtpSSL,
                     EmailConfiguration.SmtpUserName,
                    EmailConfiguration.SmtpUserName, Emailtemplate.Subject, Emailtemplate.Emailbody);
                    return;
                }
                int DaysLeft = CountRemainingDays(ServerCert);
                GlobalSessions.SessionAdd(SessionItem.ValidTill, ServerCert.NotAfter);
                //return true if number of days left for expiration are less than  ServiceNotificationGap in cofing file 
                if (DaysLeft < MobiusAppSettingReader.ServiceNotificationGap) 
                {
                    IEmailTemplate Emailtemplate = new RenewServerCertificate(ServerCert.NotAfter, DaysLeft);
                    EmailHelper.SendMail(EmailConfiguration.SmtpHost, EmailConfiguration.SmtpPort,
                    EmailConfiguration.SmtpUserName, EmailConfiguration.SmtpPassword, EmailConfiguration.SmtpSSL,
                    EmailConfiguration.SmtpUserName,
                    EmailConfiguration.SmtpUserName, Emailtemplate.Subject, Emailtemplate.Emailbody);
                    return;
                }

            }
        }

        catch (Exception ex)
        {

        }


    }
  
    #endregion
}