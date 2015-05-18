using System;
using MobiusServiceLibrary;
using Mobius.CoreLibrary;
using FirstGenesis.UI;
using FirstGenesis.UI.Base;
using Mobius.Entity;

public partial class ActivateUser : BaseClass
{
    private const string INVALID_CSR = "Invalid CSR.";
    private const string MESSAGE = "This account is already activated.";
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMessage.Text = "";

        btnActivateUser.Attributes.Add("onclick", "return ShowSearhIcon();");
    }
    protected void ActivateUser_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValid)
            {
                Result result = this.CheckLogin();

                if (!result.IsSuccess && result.ErrorCode == ErrorCode.Inactive_Account)
                {

                    MobiusClient _ObjClient = new MobiusClient();
                    string organizationalUnit = MobiusAppSettingReader.CertificateOrganizationalUnit;
                    string OrganizationName = MobiusAppSettingReader.CertificateOrgName;
                    ActivateUserRequest activateUserRequest = new ActivateUserRequest();
                    ActivateUserResponse activateUserResponse = new ActivateUserResponse();
                    GetCSRRequest getCSRRequest = new GetCSRRequest();
                    GetCSRResponse getCSRResponse = new GetCSRResponse();

                    getCSRRequest.EmailAddress = txtMail.Text;
                    getCSRRequest.UserType = (UserType)Enum.Parse(typeof(UserType), rbtProvider.Checked ? UserType.Provider.ToString() : UserType.Patient.ToString(), true);
                    getCSRResponse = _ObjClient.GetCSRDetails(getCSRRequest);
                    if (getCSRResponse.Result.IsSuccess)
                    {
                        if (!getCSRResponse.IsIndividualProvider)
                        {

                            if (!string.IsNullOrWhiteSpace(getCSRResponse.OrganizationName) && !string.IsNullOrWhiteSpace(getCSRResponse.EmailAddress)
                                && !string.IsNullOrWhiteSpace(getCSRResponse.City) && !string.IsNullOrWhiteSpace(getCSRResponse.State) && !string.IsNullOrWhiteSpace(getCSRResponse.Country))
                            {
                                activateUserRequest.CSR = this.GenerateCSR(getCSRResponse.EmailAddress, getCSRResponse.OrganizationName, organizationalUnit, OrganizationName, getCSRResponse.City, getCSRResponse.State, getCSRResponse.Country);
                            }
                            else
                            {
                                lblMessage.Text = INVALID_CSR;

                            }

                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(getCSRResponse.GivenName) && !string.IsNullOrWhiteSpace(getCSRResponse.EmailAddress)
                                && !string.IsNullOrWhiteSpace(getCSRResponse.City) && !string.IsNullOrWhiteSpace(getCSRResponse.State) && !string.IsNullOrWhiteSpace(getCSRResponse.Country))
                            {
                                activateUserRequest.CSR = this.GenerateCSR(getCSRResponse.EmailAddress, getCSRResponse.GivenName + getCSRResponse.FamilyName, organizationalUnit, OrganizationName, getCSRResponse.City, getCSRResponse.State, getCSRResponse.Country);
                            }
                            else
                            {
                                lblMessage.Text = INVALID_CSR;
                            }

                        }
                    }
                    if (!string.IsNullOrEmpty(activateUserRequest.CSR))
                    {
                        activateUserRequest.EmailAddress = txtMail.Text;
                        activateUserRequest.UserType = (UserType)Enum.Parse(typeof(UserType), rbtPatient.Checked ? UserType.Patient.ToString() : UserType.Provider.ToString(), true);
                        activateUserResponse = _ObjClient.ActivateUser(activateUserRequest);
                        if (activateUserResponse.Result.IsSuccess)
                        {
                            if (this.InstallClientCertificate(activateUserResponse.PKCS7Response))
                            {
                                lblMessage.Text = activateUserResponse.Result.ErrorMessage;
                                btnActivateUser.Visible = false;
                            }
                        }
                        else
                        {
                            lblMessage.Text = activateUserResponse.Result.ErrorMessage.ToString();
                        }
                    }
                    else
                    {
                        lblMessage.Text = INVALID_CSR;
                    }

                }
                else
                {
                    if (result.ErrorCode == ErrorCode.LoginFail)
                    {
                        lblMessage.Text = result.ErrorMessage;
                    }
                    else
                    {
                        lblMessage.Text = MESSAGE;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page,ex:ex, displayMessage: "Unable to generate CSR.");
        }
    }

    private Result CheckLogin()
    {
        MobiusClient objProxy = new MobiusClient();
        AuthenticateUserResponse response = new AuthenticateUserResponse();
        AuthenticateUserRequest request = new AuthenticateUserRequest();
        request.EmailAddress = txtMail.Text;
        request.Password = txtPassword.Text;
        request.UserType = (UserType)Enum.Parse(typeof(UserType), rbtPatient.Checked ? UserType.Patient.ToString() : UserType.Provider.ToString(), true);
        response = objProxy.AuthenticateUser(request);
        return response.Result;
    }



}
