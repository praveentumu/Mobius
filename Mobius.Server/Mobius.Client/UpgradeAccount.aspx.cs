using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstGenesis.UI.Base;
using MobiusServiceLibrary;
using FirstGenesis.UI;
using Mobius.CoreLibrary;
using System.IO;

public partial class UpgradeAccount : BaseClass
{

    MobiusSecuredClient _ObjClient = null;
    UpgradeUserRequest UpgradeUserRequest = null;
    UpgradeUserResponse UpgrdeUserResponse = null;
    private const string INVALID_CSR = "Invalid CSR request input(s).";
    private const string MESSAGE = "This account is already activated.";
    protected void Page_Load(object sender, EventArgs e)
    {
        ShowHideControls();
        //btnActivateUser.Attributes.Add("onclick", "return ShowSearhIcon();"
    }


    protected void ShowHideControls()
    {
        lblErrorMsg.Text = string.Empty;
        btnUpgrade.Visible = true;
        btnclose.Visible = true;
        txtPassword.Visible = true;
        if (GlobalSessions.SessionItem(SessionItem.ValidTill) != null)
        {
            DateTime validtill = (DateTime)GlobalSessions.SessionItem(SessionItem.ValidTill);
            if ((validtill - DateTime.Now).TotalDays > MobiusAppSettingReader.UserUpgradationNotificationGap)
            {
                btnUpgrade.Visible = false;
                btnclose.Visible = false;
                txtPassword.Visible = false;
                lblSecurity.Visible = false;
                lblErrorMsg.Text = string.Format(Helper.GetErrorMessage(ErrorCode.Account_Upgradation_Error_Message),MobiusAppSettingReader.UserUpgradationNotificationGap);
            }
        }
    }


    protected void btnUpgrade_Click(object sender, EventArgs e)
    {
        try
        {
            _ObjClient = new MobiusSecuredClient();
            UpgradeUserRequest = new UpgradeUserRequest();
            UpgrdeUserResponse = new UpgradeUserResponse();
            UserInformation UserInfo = null;
            if (GlobalSessions.SessionItem(SessionItem.UserInformation) != null)
            {
                UserInfo = (UserInformation)GlobalSessions.SessionItem(SessionItem.UserInformation);
                UpgradeUserRequest.EmailAddress = UserInfo.EmailAddress;
                UpgradeUserRequest.UserType = UserInfo.UserType;
                UpgradeUserRequest.Password = txtPassword.Text;
            }


            string PKCS7Request = this.GenerateCSRFromUserDetails();
            if (!string.IsNullOrEmpty(PKCS7Request))
            {
                UpgradeUserRequest.PKCS7Request = PKCS7Request;

                UpgrdeUserResponse = this.objProxy.UpgradeUser(UpgradeUserRequest);
                if (UpgrdeUserResponse.Result.IsSuccess)
                {
                    if (!string.IsNullOrEmpty(UpgrdeUserResponse.NewSerialNumber))
                    {
                        GlobalSessions.SessionAdd(SessionItem.SerialNumber, UpgrdeUserResponse.NewSerialNumber);
                        if (this.InstallClientCertificate(UpgrdeUserResponse.PKCS7Response))
                        {
                            if (this.DeleteExistingCertificate())
                            {
                                lblErrorMsg.Text = string.Format(Helper.GetErrorMessage(ErrorCode.Account_Upgraded_Successfully), DateTime.Now.AddYears(1).ToLocalTime());
                            }
                            else
                            {
                                lblErrorMsg.Text = Helper.GetErrorMessage(ErrorCode.Certificate_Deletion_Failed);
                            }
                        }
                        else
                        {
                            lblErrorMsg.Text = Helper.GetErrorMessage(ErrorCode.Account_Upgradation_Failed);
                        }
                    }
                    else
                        lblErrorMsg.Text = Helper.GetErrorMessage(ErrorCode.Account_Upgradation_Failed);
                }
                else
                    lblErrorMsg.Text = UpgrdeUserResponse.Result.ErrorMessage;
            }
            else
            {
                lblErrorMsg.Text = Helper.GetErrorMessage(ErrorCode.Account_Upgradation_Failed);
            }
        }
        catch (Exception ex)
        {
            Helper.LogError(ex.Message);
            lblErrorMsg.Text = Helper.GetErrorMessage(ErrorCode.Account_Upgradation_Failed) +ex.Message;

        }
    }

    protected string GenerateCSRFromUserDetails()
    {
        MobiusClient _ObjClient = new MobiusClient();
        string organizationalUnit = MobiusAppSettingReader.CertificateOrganizationalUnit;
        string OrganizationName = MobiusAppSettingReader.CertificateOrgName;
        string PKCS7Request = string.Empty;
        GetCSRRequest getCSRRequest = new GetCSRRequest();
        GetCSRResponse getCSRResponse = new GetCSRResponse();
        MobiusServiceLibrary.UserInformation UserInfo=null;

        if (GlobalSessions.SessionItem(SessionItem.UserInformation) != null)
            UserInfo = (MobiusServiceLibrary.UserInformation)GlobalSessions.SessionItem(SessionItem.UserInformation);

        if (GlobalSessions.SessionItem(SessionItem.UserType) != null)
            getCSRRequest.UserType =(UserType)GlobalSessions.SessionItem(SessionItem.UserType);

        getCSRRequest.EmailAddress = UserInfo.EmailAddress;

        getCSRResponse = _ObjClient.GetCSRDetails(getCSRRequest);

        if (getCSRResponse.Result.IsSuccess)
        {
            if (!getCSRResponse.IsIndividualProvider)
            {

                if (!string.IsNullOrWhiteSpace(getCSRResponse.OrganizationName) && !string.IsNullOrWhiteSpace(getCSRResponse.EmailAddress)
                    && !string.IsNullOrWhiteSpace(getCSRResponse.City) && !string.IsNullOrWhiteSpace(getCSRResponse.State) && !string.IsNullOrWhiteSpace(getCSRResponse.Country))
                {
                    PKCS7Request = this.GenerateCSR(getCSRResponse.EmailAddress, getCSRResponse.OrganizationName, organizationalUnit, OrganizationName, getCSRResponse.City, getCSRResponse.State, getCSRResponse.Country);
                }
                else
                {
                    lblErrorMsg.Text = INVALID_CSR;

                }

            }
            else
            {
                if (!string.IsNullOrWhiteSpace(getCSRResponse.GivenName) && !string.IsNullOrWhiteSpace(getCSRResponse.EmailAddress)
                    && !string.IsNullOrWhiteSpace(getCSRResponse.City) && !string.IsNullOrWhiteSpace(getCSRResponse.State) && !string.IsNullOrWhiteSpace(getCSRResponse.Country))
                {
                   PKCS7Request = this.GenerateCSR(getCSRResponse.EmailAddress, getCSRResponse.GivenName + getCSRResponse.FamilyName, organizationalUnit, OrganizationName, getCSRResponse.City, getCSRResponse.State, getCSRResponse.Country);
                }
                else
                {
                    lblErrorMsg.Text = INVALID_CSR;
                }

            }
        }

        return PKCS7Request;
    }
}