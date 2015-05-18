using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstGenesis.UI.Base;
using MobiusServiceLibrary;
using Mobius.CoreLibrary;
using System.Web.Services;
using FirstGenesis.UI;

public partial class RegisterProvider : BaseClass
{
    #region Variables
    MobiusClient HISEProxy = new MobiusClient();
    DataSet dsType = new DataSet();
    DataSet dsCity = new DataSet();
    DataSet dsCountry = new DataSet();
    DataSet dsLanguage = new DataSet();
    DataSet dsSpeciality = new DataSet();
    DataSet dsState = new DataSet();
    DataSet dsStatus = new DataSet();
    DataSet dsGender = new DataSet();
    GetMasterDataResponse getMasterDataResponse = null;
    GetMasterDataRequest getMasterDataRequest = null;
    private const string INVALID_CSR = "Invalid CSR.";
    private const string SELECT_LANGUAGE = "Please select language.";
    private const string SELECT_PROVIDER_TYPE = "Please select provider type.";
    private const string SELECT_STATUS = "Please select status.";
    #endregion

    #region events
    protected void Page_Load(object sender, EventArgs e)
    {
        txtPostalCode.Attributes.Add("onkeyup", "GetLocalityByZipCode();");

        if (!Page.IsPostBack)
        {
            txtCity.Attributes.Add("readonly", "readonly");
            txtState.Attributes.Add("readonly", "readonly");
            txtCountry.Attributes.Add("readonly", "readonly");
            txtCSR.Attributes.Add("readonly", "readonly");
            ShowHideControl(true);

            getMasterDataResponse = new GetMasterDataResponse();
            getMasterDataRequest = new GetMasterDataRequest();
            getMasterDataRequest.MasterCollection = MasterCollection.Language;
            getMasterDataRequest.dependedValue = 0;
            getMasterDataResponse = HISEProxy.GetMasterData(getMasterDataRequest);

            if (getMasterDataResponse.Result.IsSuccess)
            {
                FillDropDown(ddlLanguage, getMasterDataResponse.MasterDataCollection);

            }

            getMasterDataResponse = new GetMasterDataResponse();
            getMasterDataRequest = new GetMasterDataRequest();
            getMasterDataRequest.MasterCollection = MasterCollection.Specialty;
            getMasterDataRequest.dependedValue = 0;
            getMasterDataResponse = HISEProxy.GetMasterData(getMasterDataRequest);

            if (getMasterDataResponse.Result.IsSuccess)
            {
                FillListBox(lbxSpeciality, getMasterDataResponse.MasterDataCollection);

            }

            getMasterDataResponse = new GetMasterDataResponse();
            getMasterDataRequest = new GetMasterDataRequest();
            getMasterDataRequest.MasterCollection = MasterCollection.Status;
            getMasterDataRequest.dependedValue = 0;
            getMasterDataResponse = HISEProxy.GetMasterData(getMasterDataRequest);

            ddlGender.DataSource = GenderList();
            ddlGender.DataBind();

            if (rbtlstProvider.Items[0].Selected == true)
            {
                ddlProviderStatus.DataSource = ActiveInactiveList();
                ddlProviderStatus.DataBind();
            }
            else if (rbtlstProvider.Items[1].Selected == true)
            {
                ddlProviderStatus.DataSource = StatusList();
                ddlProviderStatus.DataBind();
            }
        }

    }

    protected void rdbprovidercheckedChanged(object sender, EventArgs e)
    {
        if (rbtlstProvider.Items[0].Selected == true)
        {
            ddlProviderStatus.DataSource = ActiveInactiveList();
            ddlProviderStatus.DataBind();
            lblErrorMsg.Text = string.Empty;
            ShowHideControl(true);
        }
        else if (rbtlstProvider.Items[1].Selected == true)
        {
            ddlProviderStatus.DataSource = StatusList();
            ddlProviderStatus.DataBind();
            lblErrorMsg.Text = string.Empty;
            ShowHideControl(false);
        }
    }

    protected void btnGenerateCSR_Click(object sender, EventArgs e)
    {
        try
        {
            string organizationalUnit = MobiusAppSettingReader.CertificateOrganizationalUnit;
            string OrganizationName = MobiusAppSettingReader.CertificateOrgName;
            if (rbtlstProvider.Items[0].Selected == true)
            {

                if (!string.IsNullOrWhiteSpace(txtOrganizationName.Text) && !string.IsNullOrWhiteSpace(txtdelievryEmail.Text)
                    && !string.IsNullOrWhiteSpace(txtCity.Text) && !string.IsNullOrWhiteSpace(txtCountry.Text) && !string.IsNullOrWhiteSpace(txtState.Text))
                {
                    lblErrorMsg.Text = "";
                    txtCSR.Text = this.GenerateCSR(txtdelievryEmail.Text, txtOrganizationName.Text, organizationalUnit, OrganizationName, txtCity.Text, txtState.Text, txtCountry.Text);
                }
                else
                {
                    lblErrorMsg.Text = INVALID_CSR;
                    ShowHideControl(true);
                }
                txtOrgPassword.Attributes.Add("value", txtOrgPassword.Text);
            }
            else if (rbtlstProvider.Items[1].Selected == true)
            {
                if (!string.IsNullOrWhiteSpace(txtFirstName.Text) && !string.IsNullOrWhiteSpace(txtdelievryEmail.Text)
                    && !string.IsNullOrWhiteSpace(txtCity.Text) && !string.IsNullOrWhiteSpace(txtCountry.Text) && !string.IsNullOrWhiteSpace(txtState.Text))
                {
                    lblErrorMsg.Text = "";
                    txtCSR.Text = this.GenerateCSR(txtdelievryEmail.Text, txtFirstName.Text + txtLastName.Text, organizationalUnit, OrganizationName, txtCity.Text, txtState.Text, txtCountry.Text);


                }
                else
                {
                    lblErrorMsg.Text = INVALID_CSR;
                    ShowHideControl(false);
                }
                txtPassword.Attributes.Add("value", txtPassword.Text);
            }

        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page,ex:ex, displayMessage: "Unable to generate CSR.");
        }



    }

    protected void btnRegisterProvider_Click(object sender, EventArgs e)
    {

        string serialNumber = string.Empty;
        string password = string.Empty;
        string emailAddress = string.Empty;
        AddProviderResponse providerResponse = null;
        AddProviderRequest providerRequest = null;
        List<MobiusServiceLibrary.Specialty> selectedSpecialities = new List<MobiusServiceLibrary.Specialty>();
        MobiusServiceLibrary.Specialty specialty = null;
        MobiusServiceLibrary.City city = new MobiusServiceLibrary.City();
        MobiusServiceLibrary.State state = new MobiusServiceLibrary.State();
        MobiusServiceLibrary.Country country = new MobiusServiceLibrary.Country();
        MobiusServiceLibrary.Language language = new MobiusServiceLibrary.Language();
        providerRequest = new AddProviderRequest();
        providerRequest.Provider = new MobiusServiceLibrary.Provider();
        foreach (ListItem item in lbxSpeciality.Items)
        {

            if (item.Selected == true)
            {
                specialty = new MobiusServiceLibrary.Specialty();
                specialty.SpecialtyName = item.Text;
                selectedSpecialities.Add(specialty);
            }
        }
        try
        {
            city.CityName = txtCity.Text;
            state.StateName = txtState.Text;
            country.CountryName = txtCountry.Text;
            language.LanguageId = Convert.ToInt32(ddlLanguage.SelectedValue);

            if (ddlLanguage.SelectedValue == "0")
            {
                lblErrorMsg.Text = SELECT_LANGUAGE;
                return;
            }
            if (ddlProviderType.SelectedValue == "0")
            {
                lblErrorMsg.Text = SELECT_PROVIDER_TYPE;
                return;
            }
            if (ddlProviderStatus.SelectedValue == "0")
            {
                lblErrorMsg.Text = SELECT_STATUS;
                return;
            }

            providerRequest.Provider.ProviderType = (ProviderType)Enum.Parse(typeof(ProviderType), ddlProviderType.SelectedValue, true);
            providerRequest.Provider.Status = (Status)Enum.Parse(typeof(Status), ddlProviderStatus.SelectedValue, true);
            providerRequest.Provider.ContactNumber = txtContact.Text;
            providerRequest.Provider.ElectronicServiceURI = txtEURI.Text;
            providerRequest.Provider.MedicalRecordsDeliveryEmailAddress = txtdelievryEmail.Text;
            providerRequest.Provider.StreetNumber = txtStreetNumber.Text;
            providerRequest.Provider.StreetName = txtStreetName.Text;
            providerRequest.Provider.City = city;
            providerRequest.Provider.City.State = state;
            providerRequest.Provider.PostalCode = txtPostalCode.Text;
            providerRequest.Provider.City.State.Country = country;
            providerRequest.Provider.Language = language;
            providerRequest.Provider.Identifier = txtIdentifier.Text;


            providerRequest.Provider.Specialty = selectedSpecialities;

            if (rbtlstProvider.Items[0].Selected == true)
            {
                providerRequest.Provider.IndividualProvider = false;
                providerRequest.Provider.OrganizationName = txtOrganizationName.Text;
                providerRequest.Provider.Password = txtOrgPassword.Text;
                password = txtOrgPassword.Text;
            }
            else if (rbtlstProvider.Items[1].Selected == true)
            {
                providerRequest.Provider.IndividualProvider = true;
                providerRequest.Provider.FirstName = txtFirstName.Text;
                providerRequest.Provider.MiddleName = txtMiddleName.Text;
                providerRequest.Provider.LastName = txtLastName.Text;
                providerRequest.Provider.Gender = (Gender)Enum.Parse(typeof(Gender), ddlGender.SelectedValue, true);
                providerRequest.Provider.Email = txtIndividualEmail.Text;
                providerRequest.Provider.Password = txtPassword.Text;
                password = txtPassword.Text;
            }

            //To do 
            providerRequest.CSR = txtCSR.Text;
            providerResponse = HISEProxy.AddProvider(providerRequest);
            if (providerResponse.Result.IsSuccess)
            {
                AuthenticateUserRequest request = new AuthenticateUserRequest();
                AuthenticateUserResponse response = new AuthenticateUserResponse();
                if (providerRequest.Provider.IndividualProvider)
                {
                    request.EmailAddress = txtdelievryEmail.Text;
                    request.Password = txtPassword.Text;
                    Reset(false);
                }
                else
                {
                    request.EmailAddress = txtdelievryEmail.Text;
                    request.Password = txtOrgPassword.Text;
                    Reset(true);

                }
                request.UserType = (UserType)Enum.Parse(typeof(UserType), UserType.Provider.ToString(), true);
                response = HISEProxy.AuthenticateUser(request);
                serialNumber = response.CertificateSerialNumber;
                response = null;
                if (!string.IsNullOrWhiteSpace(serialNumber))
                {
                    GlobalSessions.SessionAdd(SessionItem.SerialNumber, serialNumber);
                    this.InstallClientCertificate(providerResponse.PKCS7Response);
                    
                    //System.Threading.Thread.Sleep(1000);    //1 sec. delay
                    if (Request.QueryString["DocumentID"] != null && Request.QueryString["PatientReferralID"] != null)
                    {
                        Response.Redirect("ReferPatient.aspx?DocumentID=" + Request.QueryString["DocumentID"].ToString()
                                 + "&PatientReferralID=" + Request.QueryString["PatientReferralID"].ToString(), false);
                    }
                    else
                    {
                        Response.Redirect("Default.aspx", false);
                    }

                }
            }
            else
            {
                lblErrorMsg.Text = providerResponse.Result.ErrorMessage;

            }

        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page,ex:ex, displayMessage: "Provider registration failed.");

        }
    }

    protected void CancelRegister(object sender, EventArgs e)
    {
        lblErrorMsg.Text = string.Empty;

        txtOrgPassword.Attributes["value"] = "";
        txtPassword.Attributes["value"] = "";

        if (rbtlstProvider.Items[0].Selected == true)
        {
            Reset(true);
        }
        else if (rbtlstProvider.Items[1].Selected == true)
        {
            Reset(false);
        }
        return;
    }

    [WebMethod]
    public static string GetLocality(string Zipcode)
    {
        // 13058
        string cityName = string.Empty;
        string stateName = string.Empty;
        string CountryName = string.Empty;

        GetLocalityByZipCodeResponse getLocalityByZipCodeResponse = new GetLocalityByZipCodeResponse();
        MobiusClient HISEProxy = new MobiusClient();
        getLocalityByZipCodeResponse = HISEProxy.GetLocalityByZipCode(Zipcode);
        if (getLocalityByZipCodeResponse.Result.IsSuccess)
        {
            if (getLocalityByZipCodeResponse.City != null)
            {
                cityName = getLocalityByZipCodeResponse.City.CityName;
                stateName = getLocalityByZipCodeResponse.City.State.StateName;
                CountryName = getLocalityByZipCodeResponse.City.State.Country.CountryName;

            }
        }

        return cityName + "," + stateName + "," + CountryName;
    }
    #endregion

    #region Private Methods
    private void Reset(bool isOrganizationalProvider)
    {
        txtContact.Text = string.Empty;
        txtEURI.Text = string.Empty;
        txtdelievryEmail.Text = string.Empty;
        txtStreetName.Text = string.Empty;
        txtStreetNumber.Text = string.Empty;
        txtCity.Text = string.Empty;
        txtState.Text = string.Empty;
        txtPostalCode.Text = string.Empty;
        txtCountry.Text = string.Empty;
        ddlLanguage.SelectedIndex = 0;
        lbxSpeciality.ClearSelection();
        txtIdentifier.Text = string.Empty;
        ddlProviderType.SelectedIndex = 0;
        txtCSR.Text = string.Empty;
        if (isOrganizationalProvider)
        {
            txtOrganizationName.Text = string.Empty;
        }
        else
        {
            ddlGender.SelectedIndex = 0;
            txtIndividualEmail.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtMiddleName.Text = string.Empty;
            txtLastName.Text = string.Empty;
        }
    }

    private void ShowHideControl(bool isOrganizationalProvider)
    {

        if (isOrganizationalProvider)
        {
            rowCommonControls.Visible = true;
            rowOrganization.Visible = true;
            rowIndiviual.Visible = false;
            getMasterDataResponse = new GetMasterDataResponse();
            getMasterDataRequest = new GetMasterDataRequest();
            getMasterDataRequest.MasterCollection = MasterCollection.OrganizationType;
            getMasterDataRequest.dependedValue = 0;
            getMasterDataResponse = HISEProxy.GetMasterData(getMasterDataRequest);

            if (getMasterDataResponse.Result.IsSuccess)
            {
                FillDropDown(ddlProviderType, getMasterDataResponse.MasterDataCollection);

            }
        }
        else
        {
            rowCommonControls.Visible = true;
            rowOrganization.Visible = false;
            rowIndiviual.Visible = true;
            getMasterDataResponse = new GetMasterDataResponse();
            getMasterDataRequest = new GetMasterDataRequest();
            getMasterDataRequest.MasterCollection = MasterCollection.Provider_IndividualType;
            getMasterDataRequest.dependedValue = 0;
            getMasterDataResponse = HISEProxy.GetMasterData(getMasterDataRequest);

            if (getMasterDataResponse.Result.IsSuccess)
            {
                FillDropDown(ddlProviderType, getMasterDataResponse.MasterDataCollection);

            }

        }
    }

    private List<ListItem> GenderList()
    {
        List<ListItem> genderList = new List<ListItem>();
        genderList.Add(new ListItem("Male", "1"));
        genderList.Add(new ListItem("Female", "2"));
        genderList.Add(new ListItem("Unspecified", "3"));
        return genderList;
    }

    private List<ListItem> ActiveInactiveList()
    {
        List<ListItem> activeInactiveList = new List<ListItem>();
        activeInactiveList.Add(new ListItem("Active", "1"));
        activeInactiveList.Add(new ListItem("Inactive", "2"));
        return activeInactiveList;
    }

    private List<ListItem> StatusList()
    {
        List<ListItem> statusList = new List<ListItem>();
        statusList.Add(new ListItem("Active", "1"));
        statusList.Add(new ListItem("Inactive", "2"));
        statusList.Add(new ListItem("Retired", "3"));
        statusList.Add(new ListItem("Deceased", "4"));
        return statusList;
    }
    #endregion
}