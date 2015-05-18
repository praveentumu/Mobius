#region Using Directives
using System;
using FirstGenesis.UI.Base;
using FirstGenesis.UI;
using Mobius.CoreLibrary;

#endregion

public partial class Default : BaseClass
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (GlobalSessions.SessionItem(SessionItem.UserName) != null &&
                       GlobalSessions.SessionItem(SessionItem.ValidTill) != null &&
                       GlobalSessions.SessionItem(SessionItem.UserEmailAddress) != null)
            {
                string userName = GlobalSessions.SessionItem(SessionItem.UserName).ToString();
                string UserEmailAddres = GlobalSessions.SessionItem(SessionItem.UserEmailAddress).ToString();
                DateTime ValidTill = (DateTime)GlobalSessions.SessionItem(SessionItem.ValidTill);
                int DaysToExpire = Convert.ToInt32((ValidTill - DateTime.Today).TotalDays);
                bool RenewalNotificationSent = false;
                if (GlobalSessions.SessionItem(SessionItem.RenewalNotificationSent) != null)
                {
                    RenewalNotificationSent = (bool)GlobalSessions.SessionItem(SessionItem.RenewalNotificationSent);
                }
                //Check Validity of account, if it is less than equal NOTIFICATION_DURATION (Base class) in days, send a mail to the user.
                IEmailTemplate EmailTemplate = new RenewCertificate(userName, ValidTill, DaysToExpire);
                if (!RenewalNotificationSent)
                {
                    if (DaysToExpire <= MobiusAppSettingReader.UserUpgradationNotificationGap)
                    {
                        EmailHelper.SendMail(EmailConfiguration.SmtpHost, EmailConfiguration.SmtpPort,
                            EmailConfiguration.SmtpUserName, EmailConfiguration.SmtpPassword, EmailConfiguration.SmtpSSL,
                            EmailConfiguration.SmtpUserName,
                            UserEmailAddres, EmailTemplate.Subject, EmailTemplate.Emailbody);
                        GlobalSessions.SessionAdd(SessionItem.RenewalNotificationSent, true);
                    }
                }
            }
        }

    }

}

