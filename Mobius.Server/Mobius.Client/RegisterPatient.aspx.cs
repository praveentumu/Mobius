using System;
using System.Web.UI;
using WebText = System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using FirstGenesis.UI.Base;
using Ediable_Repeater;
using System.Web.UI.WebControls;
using MobiusServiceLibrary;
using Mobius.CoreLibrary;
using System.Web.Services;
using FirstGenesis.UI;
using MobiusServiceUtility;

public partial class RegisterPatient : BaseClass
{
    #region Variables
    MobiusClient HISEProxy = new MobiusClient();
    string MPID = string.Empty;
    SoapHandler soapHandler;
    private const string FIRSTNAME_LASTNAME = "Please provide both first and last name";
    private const string FIRST_NAME = "Please provide first name";
    private const string LAST_NAME = "Please provide last name";
    private const string ZIP_CODE = "Please provide valid zip code";
    private const string POSTAL_CODE = "Please provide postalcode.";
    private const string INVALID_CSR = "Invalid CSR request input(s).";
    private const string TELEPHONE_EXTENSION_NUMBER = "Please provide telephone number";
    private const string TELEPHONE_NUMBER = "Please provide telephone number";
    private const string EXTENSION_NUMBER = "Please provide extension number";
    private const string TeleStatus_Active = "Active";
    private const string TeleStatus_InActive = "InActive";
    
    #endregion

    #region Properties
  
    private int EditIndexName
    {
        get { return (int)ViewState["EditIndexName"]; }
        set { ViewState["EditIndexName"] = value; }
    }

    private int EditIndexTelephone
    {
        get { return (int)ViewState["EditIndexTelephone"]; }

        set { ViewState["EditIndexTelephone"] = value; }
    }

    private int EditIndexAddress
    {
        get { return (int)ViewState["EditIndexAddress"]; }
        set { ViewState["EditIndexAddress"] = value; }
    }

    #endregion

    #region Methods
    // Function call on page load.
    /// <summary>
    /// Function called at page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            soapHandler = new SoapHandler(Convert.ToString(GlobalSessions.SessionItem(SessionItem.SerialNumber)));
            if (Page.IsPostBack)
            {
                txtSSN.Attributes["value"] = txtSSN.Text;
            }
            if (!Page.IsPostBack)
            {
                Data data = new Data();
                data.Names.Clear();
                rptrName.DataSource = data.Names;
                rptrName.DataBind();

                data.Address.Clear();
                rptr_Address.DataSource = data.Address;
                rptr_Address.DataBind();

                data.Telephone.Clear();
                rptrTelephone.DataSource = data.Telephone;
                rptrTelephone.DataBind();
                    EditIndexName = -1;
                    EditIndexTelephone = -1;
                    EditIndexAddress = -1;
                    btnUpdate.Enabled = false;
                    LoadData();
                    txtSSN.Text = "";
                    BindGenderList();
                    //ddlSex.DataBind();
                    txtPassword.Visible = true;
                    //********************************************************************PT
                    ddlSex.DataSource = Enum.GetValues(typeof(Gender));
                    ddlSex.DataBind();
                    ddlSex.SelectedIndex = 0;
                    //********************************************************************ENDS   
            }
            lblErrorMsg.Text = "";
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    protected void CancelRegister(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(Request.QueryString["MPIID"]))
            {
                MPID = Request.QueryString["MPIID"];
                Response.Redirect("RegisterPatient.aspx?MPIID=" + MPID, false);
            }
            else
            {
                Response.Redirect("RegisterPatient.aspx", false);
            }

        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    protected void OnAddName(object sender, EventArgs e)
    {
        // Get the textboxes using the button as the starting point
        WebText.TextBox firstName = ((System.Web.UI.Control)sender).Parent.FindControl("NewFirstName") as WebText.TextBox;
        WebText.TextBox middleName = ((System.Web.UI.Control)sender).Parent.FindControl("NewMiddleName") as WebText.TextBox;
        string middlename = !string.IsNullOrEmpty(middleName.Text.ToString()) ? middleName.Text.ToString() : string.Empty;
        WebText.TextBox lastName = ((System.Web.UI.Control)sender).Parent.FindControl("NewLastName") as WebText.TextBox;

        WebText.TextBox PrefixName = ((System.Web.UI.Control)sender).Parent.FindControl("txtPrefixName") as WebText.TextBox;
        WebText.TextBox SuffixName = ((System.Web.UI.Control)sender).Parent.FindControl("txtSuffixName") as WebText.TextBox;
        Data data = new Data();
        BindNameDetails(data);
        // No point in adding anything if empty
        if (!string.IsNullOrEmpty(firstName.Text) && !string.IsNullOrEmpty(lastName.Text))
        {
            // Add a new name and rebind the repeater
           
            Names name = new Names();
            name.IDName = 0;
            name.FirstName = firstName.Text;
            name.MiddleName = middlename;
            name.LastName = lastName.Text;
            name.PrefixName = PrefixName.Text;
            name.SuffixName= SuffixName.Text;

            if (name != null)
            {
                data.Names.Add(name);
            }

            
        }
        else
        {
            string message = string.Empty;
            if (string.IsNullOrWhiteSpace(firstName.Text) && string.IsNullOrWhiteSpace(lastName.Text))
            {
                message = FIRSTNAME_LASTNAME;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(firstName.Text))
                {
                    message = FIRST_NAME;
                }
                else
                {
                    message = LAST_NAME;
                }

            }
            var script = "alert('" + message + "');";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "ShowError", script, true);
        }
        rptrName.DataSource = data.Names;
        rptrName.DataBind();
    }

    protected void rptrName_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item ||
             e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.ItemIndex == EditIndexName)
                {
                    // Find the placeholder
                    PlaceHolder p = e.Item.FindControl("firstNameEditPlaceholder") as PlaceHolder;

                    // Create textBox and assign the current value of the data item
                    TextBox t = new TextBox();
                    t.ID = "firstNameEdit";
                    t.Text = ((Names)e.Item.DataItem).FirstName;
                    // Add the textbox to the placeholder
                    p.Controls.Add(t);
                    // Get the existing label and hide it
                    Label l = e.Item.FindControl("firstName") as Label;
                    l.Visible = false;



                    p = e.Item.FindControl("middleNameEditPlaceholder") as PlaceHolder;
                    t = new TextBox();
                    t.ID = "middleNameEdit";
                    t.Text = ((Names)e.Item.DataItem).MiddleName;
                    p.Controls.Add(t);
                    l = e.Item.FindControl("middleName") as Label;
                    l.Visible = false;



                    p = e.Item.FindControl("lastNameEditPlaceholder") as PlaceHolder;
                    t = new TextBox();
                    t.ID = "lastNameEdit";
                    t.Text = ((Names)e.Item.DataItem).LastName;
                    p.Controls.Add(t);
                    l = e.Item.FindControl("lastName") as Label;
                    l.Visible = false;



                    p = e.Item.FindControl("SuffixNamePlaceHolder") as PlaceHolder;
                    t = new TextBox();
                    t.ID = "suffixNameEdit";
                    t.Text = ((Names)e.Item.DataItem).SuffixName;
                    p.Controls.Add(t);
                    l = e.Item.FindControl("suffixName") as Label;
                    l.Visible = false;


                    p = e.Item.FindControl("PrefixNameEditPlaceholder") as PlaceHolder;
                    t = new TextBox();
                    t.ID = "prefixNameEdit";
                    t.Text = ((Names)e.Item.DataItem).PrefixName;
                    p.Controls.Add(t);
                    l = e.Item.FindControl("prefixName") as Label;
                    l.Visible = false;




                    // Make hidden fields visible
                    HtmlInputHidden h = e.Item.FindControl("firstNameHidden") as HtmlInputHidden;
                    h.Visible = true;
                    //middle
                    h = e.Item.FindControl("middleNameHidden") as HtmlInputHidden;
                    h.Visible = true;

                    h = e.Item.FindControl("lastNameHidden") as HtmlInputHidden;
                    h.Visible = true;


                    h = e.Item.FindControl("PrefixNameHidden") as HtmlInputHidden;
                    h.Visible = true;

                    h = e.Item.FindControl("SuffixNameHidden") as HtmlInputHidden;
                    h.Visible = true;

                    // Remove the edit button from display
                    ImageButton b = e.Item.FindControl("Edit") as ImageButton;
                    b.Visible = false;

                    // Re use the delete button
                    b = e.Item.FindControl("Delete") as ImageButton;
                    b.CommandName = "save";
                    b.OnClientClick = "OnSave(this)";
                    b.ImageUrl = "~/images/base_floppydisk_32.png";
                    b.ToolTip = "Save";
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    protected void rptrName_ItemCommand(object sender, RepeaterCommandEventArgs e)
    {
        Data data = new Data();
        BindNameDetails(data);
        try
        {
            if (data.Names.Count > e.Item.ItemIndex)
            {
                if (e.CommandName == "delete")
                {
                    if (data.Names[e.Item.ItemIndex] != null)
                        data.Names.RemoveAt(e.Item.ItemIndex);
                    EditIndexName = -1;
                }
                else if (e.CommandName == "edit")
                {
                    EditIndexName = e.Item.ItemIndex;
                }
                else if (e.CommandName == "save")
                {
                    HtmlInputHidden t = e.Item.FindControl("firstNameHidden") as HtmlInputHidden;
                    data.Names[e.Item.ItemIndex].FirstName = t.Value;

                    t = e.Item.FindControl("middleNameHidden") as HtmlInputHidden;
                    data.Names[e.Item.ItemIndex].MiddleName = t.Value;


                    t = e.Item.FindControl("lastNameHidden") as HtmlInputHidden;
                    data.Names[e.Item.ItemIndex].LastName = t.Value;

                    t = e.Item.FindControl("PrefixNameHidden") as HtmlInputHidden;
                    data.Names[e.Item.ItemIndex].PrefixName = t.Value;

                    t = e.Item.FindControl("SuffixNameHidden") as HtmlInputHidden;
                    data.Names[e.Item.ItemIndex].SuffixName = t.Value;

                    EditIndexName = -1;
                }

                rptrName.DataSource = data.Names;
                rptrName.DataBind();
            }
            else
                ExceptionHelper.HandleException(page: Page, displayMessage: "Record Not Found");
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    public string GetTelephoneStatus(string val)
    {
        return val.ToString().Equals("True") ? TeleStatus_Active : TeleStatus_InActive;
    }
    protected void OnAddTelephone(object sender, EventArgs e)
    {

        // Get the textboxes using the button as the starting point
        WebText.DropDownList NewTelephoneType = ((System.Web.UI.Control)sender).Parent.FindControl("NewTelephoneType") as WebText.DropDownList;
        WebText.TextBox NewTelephoneExtension = ((System.Web.UI.Control)sender).Parent.FindControl("NewTelephoneExtension") as WebText.TextBox;
        WebText.TextBox NewTelephoneData = ((System.Web.UI.Control)sender).Parent.FindControl("NewTelephoneData") as WebText.TextBox;
        WebText.DropDownList NewTelephoneStatus = ((System.Web.UI.Control)sender).Parent.FindControl("NewTelephoneStatus") as WebText.DropDownList;
        Data data = new Data();
        BindTelephoneDetails(data);
        //  No point in adding anything if empty
        if (!string.IsNullOrEmpty(NewTelephoneData.Text))
        {
            // Add a new name and rebind the repeater
          
            MobiusServiceLibrary.Telephone tele = new MobiusServiceLibrary.Telephone();
            tele.Id = 0;
            tele.Type = NewTelephoneType.SelectedItem.Text;
            tele.Extensionnumber = NewTelephoneExtension.Text;
            tele.Number = NewTelephoneData.Text;
            tele.Status = NewTelephoneStatus.SelectedItem.Text == "Active" ? true : false; ;
            if (tele != null)
            {
                data.Telephone.Add(tele);
            }
           
        }
        else
        {
            string message = string.Empty;
            if (string.IsNullOrWhiteSpace(NewTelephoneData.Text) )
            {
                message = TELEPHONE_EXTENSION_NUMBER;
            }           
            var script = "alert('" + message + "');";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "ShowError", script, true);
        }
        rptrTelephone.DataSource = data.Telephone;
        rptrTelephone.DataBind();
    }

    protected void rptrTelephone_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //to handle
                if (e.Item.ItemIndex == EditIndexTelephone)
                {
                    // Find the placeholder
                    PlaceHolder p = e.Item.FindControl("TelephoneTypeEditPlaceholder") as PlaceHolder;

                    // Create textBox and assign the current value of the data item
                    DropDownList d = new DropDownList();
                    d.ID = "TelephoneTypeEdit";
                    d.DataSource = GetTypes();
                    d.SelectedIndex = ((MobiusServiceLibrary.Telephone)e.Item.DataItem).Type == "Work" ? 0 : ((MobiusServiceLibrary.Telephone)e.Item.DataItem).Type == "Home" ? 1 : 2;
                    d.DataBind();
                    d.Width = 150;
                    // Add the textbox to the placeholder
                    p.Controls.Add(d);
                    // Get the existing label and hide it
                    Label l = e.Item.FindControl("TelephoneType") as Label;
                    l.Visible = false;


                    p = e.Item.FindControl("TelephoneExtensionEditPlaceholder") as PlaceHolder;
                    TextBox t = new TextBox();
                    t.ID = "TelephoneExtensionEdit";
                    t.Text = ((MobiusServiceLibrary.Telephone)e.Item.DataItem).Extensionnumber;
                    p.Controls.Add(t);
                    l = e.Item.FindControl("TelephoneExtension") as Label;
                    l.Visible = false;

                    p = e.Item.FindControl("TelephoneDataEditPlaceholder") as PlaceHolder;
                    t = new TextBox();
                    t.ID = "TelephoneDataEdit";
                    t.Text = ((MobiusServiceLibrary.Telephone)e.Item.DataItem).Number;
                    p.Controls.Add(t);
                    l = e.Item.FindControl("TelephoneData") as Label;
                    l.Visible = false;

                    p = e.Item.FindControl("TelephoneStatusEditPlaceholder") as PlaceHolder;
                    d = new DropDownList();
                    d.ID = "TelephoneStatusEdit";
                    d.DataSource = TelephoneStatus();
                    d.SelectedIndex = ((MobiusServiceLibrary.Telephone)e.Item.DataItem).Status == true ? 0 : 1;
                    d.DataBind();
                    d.Width = 150;
                    // Add the textbox to the placeholder
                    p.Controls.Add(d);
                    // Get the existing label and hide it
                    l = e.Item.FindControl("TelephoneStatus") as Label;
                    l.Visible = false;


                    // Make hidden fields visible
                    HtmlInputHidden h = e.Item.FindControl("TelephoneTypeHidden") as HtmlInputHidden;
                    h.Visible = true;

                    h = e.Item.FindControl("TelephoneExtensionHidden") as HtmlInputHidden;
                    h.Visible = true;

                    h = e.Item.FindControl("TelephoneDataHidden") as HtmlInputHidden;
                    h.Visible = true;

                    h = e.Item.FindControl("TelephoneStatusHidden") as HtmlInputHidden;
                    h.Visible = true;
                    // Remove the edit button from display
                    ImageButton b = e.Item.FindControl("EditT") as ImageButton;
                    b.Visible = false;

                    // Re use the delete button
                    b = e.Item.FindControl("DeleteT") as ImageButton;
                    b.CommandName = "saveT";
                    b.OnClientClick = "OnSaveTelephone(this)";
                    b.ImageUrl = "~/images/base_floppydisk_32.png";
                    b.ToolTip = "Save";
                }
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList NewTelephoneType = e.Item.FindControl("NewTelephoneType") as DropDownList;
                if (NewTelephoneType != null)
                {
                    NewTelephoneType.DataSource = GetTypes();
                    NewTelephoneType.DataBind();
                }
                DropDownList NewTelephoneStatus = e.Item.FindControl("NewTelephoneStatus") as DropDownList;
                if (NewTelephoneStatus != null)
                {
                    NewTelephoneStatus.DataSource = TelephoneStatus();
                    NewTelephoneStatus.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    protected void rptrTelephone_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

        Data data = new Data();
        BindTelephoneDetails(data);
        try
        {
            if (data.Telephone.Count > e.Item.ItemIndex)
            {
                if (e.CommandName == "deleteT")
                {
                    if (data.Telephone[e.Item.ItemIndex] != null)
                        data.Telephone.RemoveAt(e.Item.ItemIndex);
                    EditIndexTelephone = -1;
                }
                else if (e.CommandName == "editT")
                {
                    EditIndexTelephone = e.Item.ItemIndex;
                }
                else if (e.CommandName == "saveT")
                {
                    HtmlInputHidden t = e.Item.FindControl("TelephoneTypeHidden") as HtmlInputHidden;
                    data.Telephone[e.Item.ItemIndex].Type = t.Value;

                    t = e.Item.FindControl("TelephoneExtensionHidden") as HtmlInputHidden;
                    data.Telephone[e.Item.ItemIndex].Extensionnumber = t.Value;

                    t = e.Item.FindControl("TelephoneDataHidden") as HtmlInputHidden;
                    data.Telephone[e.Item.ItemIndex].Number = t.Value;

                    t = e.Item.FindControl("TelephoneStatusHidden") as HtmlInputHidden;
                    data.Telephone[e.Item.ItemIndex].Status = t.Value == "Active" ? true : false;

                    EditIndexTelephone = -1;
                }

                rptrTelephone.DataSource = data.Telephone;
                rptrTelephone.DataBind();
            }
            else
                ExceptionHelper.HandleException(page: Page, displayMessage: "Record Not Found");
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    protected void OnAddAddress(object sender, EventArgs e)
    {
        string message = string.Empty;
        //GetLocalityByZipCodeResponse getLocalityByZipCodeResponse = new GetLocalityByZipCodeResponse() ;
        // Get the textboxes using the button as the starting point
        try
        {
            WebText.TextBox NewAddressLine1 = ((System.Web.UI.Control)sender).Parent.FindControl("NewAddressLine1") as WebText.TextBox;
            WebText.TextBox NewAddressLine2 = ((System.Web.UI.Control)sender).Parent.FindControl("NewAddressLine2") as WebText.TextBox;
            WebText.TextBox NewCountry = ((System.Web.UI.Control)sender).Parent.FindControl("NewCountry") as WebText.TextBox;
            WebText.TextBox NewState = ((System.Web.UI.Control)sender).Parent.FindControl("NewState") as WebText.TextBox;
            WebText.TextBox NewCity = ((System.Web.UI.Control)sender).Parent.FindControl("NewCity") as WebText.TextBox;
            WebText.TextBox NewZip = ((System.Web.UI.Control)sender).Parent.FindControl("NewZip") as WebText.TextBox;
            WebText.DropDownList NewStatus = ((System.Web.UI.Control)sender).Parent.FindControl("NewStatus") as WebText.DropDownList;
            Data data = new Data();
            BindAddressDetails(data);
            //  No point in adding anything if empty
            if (!string.IsNullOrEmpty(NewZip.Text))
            {
                if (NewZip.Text.Length < 5)
                {
                    message = ZIP_CODE;
                    var script = "alert('" + message + "');";
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "ShowError", script, true);
                    return;
                }

                // Add a new name and rebind the repeater
              
                MobiusServiceLibrary.Address address = new MobiusServiceLibrary.Address();
                address.Id = 0;
                address.AddressLine1 = NewAddressLine1.Text;
                address.AddressLine2 = NewAddressLine2.Text;
                address.Zip = NewZip.Text;

                address.City = new City();
                address.City.CityName = NewCity.Text;

                address.City.State = new State();
                address.City.State.StateName = NewState.Text;

                address.City.State.Country = new Country();
                address.City.State.Country.CountryName = NewCountry.Text;


                address.Action = ActionType.Insert;
                //}            
                //}            
                //address.Status = NewStatus.SelectedItem.Text;
                address.AddressStatus = (AddressStatus)Enum.Parse(typeof(AddressStatus), NewStatus.SelectedValue, true);

                if (address != null)
                {
                    data.Address.Add(address);
                }
          
            }
            else
            {

                if (string.IsNullOrWhiteSpace(NewZip.Text))
                {
                    message = POSTAL_CODE;
                }

                var script = "alert('" + message + "');";
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "ShowError", script, true);
            }

            txtPassword.Attributes.Add("value", txtPassword.Text);
            rptr_Address.DataSource = data.Address;
            rptr_Address.DataBind();
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    protected void rptr_Address_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //to handle
                if (e.Item.ItemIndex == EditIndexAddress)
                {
                    Data data = new Data();
                    MobiusServiceLibrary.Address adres = new MobiusServiceLibrary.Address();
                    adres = data.Address[e.Item.ItemIndex];

                    PlaceHolder p = e.Item.FindControl("AddressLine1EditPlaceholder") as PlaceHolder;

                    TextBox t = new TextBox();
                    t.ID = "AddressLine1Edit";
                    t.Text = ((MobiusServiceLibrary.Address)e.Item.DataItem).AddressLine1;
                    t.Width = Unit.Pixel(400);
                    // Add the textbox to the placeholder
                    p.Controls.Add(t);
                    // Get the existing label and hide it
                    Label l = e.Item.FindControl("AddressLine1") as Label;
                    l.Visible = false;


                    p = e.Item.FindControl("AddressLine2EditPlaceholder") as PlaceHolder;
                    t = new TextBox();
                    t.ID = "AddressLine2Edit";
                    t.Text = ((MobiusServiceLibrary.Address)e.Item.DataItem).AddressLine2;
                    p.Controls.Add(t);
                    l = e.Item.FindControl("AddressLine2") as Label;
                    l.Visible = false;


                    p = e.Item.FindControl("CountryEditPlaceholder") as PlaceHolder;
                    t = new TextBox();
                    t.ID = "CountryEdit";
                    t.Width = Unit.Pixel(50); ;
                    t.Enabled = false;
                    t.Text = ((MobiusServiceLibrary.Address)e.Item.DataItem).City.State.Country.CountryName;

                    p.Controls.Add(t);
                    l = e.Item.FindControl("Country") as Label;
                    l.Visible = false;


                    p = e.Item.FindControl("StateEditPlaceholder") as PlaceHolder;
                    t = new TextBox();
                    t.ID = "StateEdit";
                    t.Width = Unit.Pixel(50); 
                    t.Enabled = false;
                    t.Text = ((MobiusServiceLibrary.Address)e.Item.DataItem).City.State.StateName;

                    p.Controls.Add(t);
                    l = e.Item.FindControl("State") as Label;
                    l.Visible = false;


                    p = e.Item.FindControl("CityEditPlaceholder") as PlaceHolder;
                    t = new TextBox();
                    t.ID = "CityEdit";
                    t.Width = 120;
                    t.Enabled = false;
                    t.Text = ((MobiusServiceLibrary.Address)e.Item.DataItem).City.CityName;

                    p.Controls.Add(t);
                    l = e.Item.FindControl("City") as Label;
                    l.Visible = false;


                    p = e.Item.FindControl("ZipEditPlaceholder") as PlaceHolder;
                    t = new TextBox();
                    t.ID = "ZipEdit";
                    t.Text = ((MobiusServiceLibrary.Address)e.Item.DataItem).Zip;
                    t.Width =Unit.Pixel(50);
                    t.Attributes.Add("onchange", "GetLocalityByZipCode(this);");
                    p.Controls.Add(t);
                    l = e.Item.FindControl("Zip") as Label;
                    l.Visible = false;


                    p = e.Item.FindControl("StatusEditPlaceholder") as PlaceHolder;
                    DropDownList d = new DropDownList();
                    d.ID = "StatusEdit";
                    d.Width = Unit.Pixel(100); 
                    List<ListItem> status = new List<ListItem>();
                    status.Add(new ListItem("Primary", "1"));
                    status.Add(new ListItem("Secondary ", "2"));
                    status.Add(new ListItem("Inactive", "3"));
                    d.DataSource = status;
                    d.DataBind();
                    //d.SelectedIndex = 0;
                    p.Controls.Add(d);
                    l = e.Item.FindControl("Status") as Label;
                    l.Visible = false;

                    // Make hidden fields visible
                    HtmlInputHidden h = e.Item.FindControl("AddressLine1eHidden") as HtmlInputHidden;
                    h.Visible = true;

                    h = e.Item.FindControl("AddressLine2eHidden") as HtmlInputHidden;
                    h.Visible = true;

                    h = e.Item.FindControl("CountryHidden") as HtmlInputHidden;
                    h.Visible = true;

                    h = e.Item.FindControl("StateHidden") as HtmlInputHidden;
                    h.Visible = true;

                    h = e.Item.FindControl("CityHidden") as HtmlInputHidden;
                    h.Visible = true;

                    h = e.Item.FindControl("ZipHidden") as HtmlInputHidden;
                    h.Visible = true;

                    h = e.Item.FindControl("StatusHidden") as HtmlInputHidden;
                    h.Visible = true;

                    // Remove the edit button from display
                    ImageButton b = e.Item.FindControl("EditA") as ImageButton;

                    b.Visible = false;

                    // Re use the delete button
                    b = e.Item.FindControl("DeleteA") as ImageButton;
                    b.CommandName = "saveA";
                    b.OnClientClick = "OnSaveAddress(this)";
                    b.ImageUrl = "~/images/base_floppydisk_32.png";
                    b.ToolTip = "Save";
                }
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList NewStatus = e.Item.FindControl("NewStatus") as DropDownList;
                if (NewStatus != null)
                {
                    NewStatus.DataSource = Enum.GetValues(typeof(AddressStatus));
                    NewStatus.DataBind();
                    NewStatus.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    protected void rptr_Address_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        Data data = new Data();
        BindAddressDetails(data);
        try
        {
            if (data.Address.Count > e.Item.ItemIndex)
            {
                if (e.CommandName == "deleteA")
                {
                    if (data.Names[e.Item.ItemIndex] != null)
                        data.Address.RemoveAt(e.Item.ItemIndex);
                    EditIndexAddress = -1;
                }

                else if (e.CommandName == "editA")
                {
                    EditIndexAddress = e.Item.ItemIndex;
                }
                else if (e.CommandName == "saveA")
                {
                    HtmlInputHidden t = e.Item.FindControl("AddressLine1eHidden") as HtmlInputHidden;
                    data.Address[e.Item.ItemIndex].AddressLine1 = t.Value;

                    t = e.Item.FindControl("AddressLine2eHidden") as HtmlInputHidden;
                    data.Address[e.Item.ItemIndex].AddressLine2 = t.Value;

                    t = e.Item.FindControl("CountryHidden") as HtmlInputHidden;
                    data.Address[e.Item.ItemIndex].City.State.Country.CountryName = t.Value;

                    t = e.Item.FindControl("StateHidden") as HtmlInputHidden;
                    data.Address[e.Item.ItemIndex].City.State.StateName = t.Value;

                    t = e.Item.FindControl("CityHidden") as HtmlInputHidden;
                    data.Address[e.Item.ItemIndex].City.CityName = t.Value;

                    t = e.Item.FindControl("ZipHidden") as HtmlInputHidden;
                    data.Address[e.Item.ItemIndex].Zip = t.Value;

                    t = e.Item.FindControl("StatusHidden") as HtmlInputHidden;
                    data.Address[e.Item.ItemIndex].AddressStatus = (AddressStatus)Enum.Parse(typeof(AddressStatus), t.Value, true);

                    EditIndexAddress = -1;
                }

                rptr_Address.DataSource = data.Address;
                rptr_Address.DataBind();
            }
            else
                ExceptionHelper.HandleException(page: Page, displayMessage: "Record Not Found");
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    protected void btnRegister_Click(object sender, EventArgs e)
    {
        Data data = new Data();
        AddPatientResponse patientResponse = null;
        AddPatientRequest patientRequest = null;
        string serialNumber = string.Empty;
        string emailAddress = string.Empty;


        patientRequest = new AddPatientRequest();
        patientRequest.Patient = new MobiusServiceLibrary.Patient();

        try
        {

            patientRequest.Patient.EmailAddress = txtUserEmail.Text;
            patientRequest.Patient.CommunityId = MobiusAppSettingReader.LocalHomeCommunityID;
            patientRequest.Patient.CSR = txtCSR.Text;
            patientRequest.Patient.Password = txtPassword.Text;

            patientRequest.Patient.Telephones = data.Telephone;
            data.Names.ForEach(x =>
            {
                if (!string.IsNullOrEmpty(x.FirstName))
                    patientRequest.Patient.GivenName.Add(x.FirstName);
                else
                    patientRequest.Patient.GivenName.Add(string.Empty);

                if (patientRequest.Patient.MiddleName == null)
                    patientRequest.Patient.MiddleName = new List<string>();

                if (!string.IsNullOrEmpty(x.MiddleName))
                    patientRequest.Patient.MiddleName.Add(x.MiddleName);
                else
                    patientRequest.Patient.MiddleName.Add(string.Empty);

                if (patientRequest.Patient.Prefix == null)
                    patientRequest.Patient.Prefix = new List<string>();

                if (!string.IsNullOrEmpty(x.PrefixName))
                    patientRequest.Patient.Prefix.Add(x.PrefixName);
                else
                    patientRequest.Patient.Prefix.Add(string.Empty);

                if (patientRequest.Patient.Suffix == null)
                    patientRequest.Patient.Suffix = new List<string>();

                if (!string.IsNullOrEmpty(x.SuffixName))
                    patientRequest.Patient.Suffix.Add(x.SuffixName);
                else
                    patientRequest.Patient.Suffix.Add(string.Empty);

                if (patientRequest.Patient.FamilyName == null)
                    patientRequest.Patient.FamilyName = new List<string>();


                if (!string.IsNullOrEmpty(x.LastName))
                    patientRequest.Patient.FamilyName.Add(x.LastName);
                else
                    patientRequest.Patient.FamilyName.Add(string.Empty);

                if (x.IDName >= 0)
                    patientRequest.Patient.IDNames.Add(x.IDName);
                else
                    patientRequest.Patient.IDNames.Add(0);

                if (patientRequest.Patient.Action == null)
                    patientRequest.Patient.Action = new List<ActionType>();

              
                    patientRequest.Patient.Action.Add(ActionType.Insert);
            });

            patientRequest.Patient.MothersMaidenName = new Name();
            patientRequest.Patient.MothersMaidenName.Prefix= txtMotherPrefix.Text;
            patientRequest.Patient.MothersMaidenName.GivenName = txtMotherGivenName.Text;
            patientRequest.Patient.MothersMaidenName.MiddleName = txtMotherMiddleName.Text;
            patientRequest.Patient.MothersMaidenName.FamilyName = txtMotherFamilyName.Text;
            patientRequest.Patient.MothersMaidenName.Suffix = txtMotherSuffix.Text;

            patientRequest.Patient.SSN = txtSSN.Text;
            patientRequest.Patient.DOB = txtDOB.Text;

            patientRequest.Patient.Gender = (Mobius.CoreLibrary.Gender)Enum.Parse(typeof(Mobius.CoreLibrary.Gender), ddlSex.SelectedValue, true);
            patientRequest.Patient.IDNames = null;

            patientRequest.Patient.PatientAddress = data.Address;

            patientRequest.Patient.BirthPlaceAddress = txtBirthPlacestreetAddress.Text;
            patientRequest.Patient.BirthPlaceCity = txtBirthPlaceCity.Text;
            patientRequest.Patient.BirthPlaceState = txtBirthPlaceState.Text;
            patientRequest.Patient.BirthPlaceCountry = txtBirthPlaceCountry.Text;
            patientRequest.Patient.BirthPlaceZip = txtBirthPlaceZip.Text;            

            patientResponse = HISEProxy.AddPatient(patientRequest);
            lblErrorMsg.Text = patientResponse.Result.ErrorMessage;
            lblErrorMsg.Visible = true;
            if (patientResponse.Result.IsSuccess)
            {
                btnRegister.Enabled = false;
                AuthenticateUserRequest request = new AuthenticateUserRequest();
                AuthenticateUserResponse response = new AuthenticateUserResponse();
                
                request.EmailAddress = txtUserEmail.Text;
                request.Password = txtPassword.Text;
                request.UserType = (UserType)Enum.Parse(typeof(UserType), UserType.Patient.ToString(), true);
                response = HISEProxy.AuthenticateUser(request);
                serialNumber = response.CertificateSerialNumber;
                response = null;
                if (!string.IsNullOrWhiteSpace(serialNumber))
                {
                    GlobalSessions.SessionAdd(SessionItem.SerialNumber, serialNumber);

                    this.InstallClientCertificate(patientResponse.PKCS7Response);
                    Response.Redirect("Default.aspx", false);
                }

            }
        }

        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page,ex:ex, displayMessage: "Patient registration failed");

        }

    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
 
    }

    protected void btnGenerateCSR_Click(object sender, EventArgs e)
    {
        try
        {
            string OrganizationName = MobiusAppSettingReader.CertificateOrgName;
            string organizationalUnit = MobiusAppSettingReader.CertificateOrganizationalUnit;
            Data data = new Data();
            string patientFirstName = string.Empty;
            string cityName = string.Empty;
            string stateName = string.Empty;
            string countryName = string.Empty;
            string patientName = string.Empty;
            int count = 0;
            foreach (Ediable_Repeater.Names givenName in data.Names)
            {
                if (count == 0)
                {
                    patientName = givenName.FirstName + givenName.LastName;
                }
                patientFirstName = givenName.FirstName;
                count += 1;
                break;
            }

            foreach (MobiusServiceLibrary.Address address in data.Address)
            {
                cityName = address.City.CityName;
                stateName = address.City.CityName;
                countryName = address.City.State.Country.CountryName;
                break;
            }

            if (!string.IsNullOrWhiteSpace(patientFirstName) && !string.IsNullOrWhiteSpace(txtUserEmail.Text)
                   && !string.IsNullOrWhiteSpace(cityName) && !string.IsNullOrWhiteSpace(countryName) && !string.IsNullOrWhiteSpace(stateName))
            {
                lblErrorMsg.Text = "";
                txtCSR.Text = this.GenerateCSR(txtUserEmail.Text, patientName, organizationalUnit, OrganizationName, cityName, stateName, countryName);
            }
            else
            {
                lblErrorMsg.Text = INVALID_CSR;
                lblErrorMsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page,ex:ex, displayMessage: "Unable to generate CSR.");
        }
    }

    [WebMethod]
    public static string GetLocality(string Zipcode)
    {   //13056
        string cityname = string.Empty;
        string statename = string.Empty;
        string CountryName = string.Empty;
        GetLocalityByZipCodeResponse getLocalityByZipCodeResponse = new GetLocalityByZipCodeResponse();
        MobiusClient HISEProxy = new MobiusClient();
        getLocalityByZipCodeResponse = HISEProxy.GetLocalityByZipCode(Zipcode);

        if (getLocalityByZipCodeResponse.Result.IsSuccess)
        {
            cityname = getLocalityByZipCodeResponse.City.CityName;
            statename = getLocalityByZipCodeResponse.City.State.StateName;
            CountryName = getLocalityByZipCodeResponse.City.State.Country.CountryName;
        }

        return cityname + "," + statename + "," + CountryName;
    }

    #endregion

    #region Private Methods
    private void BindGenderList()
    {
        ddlSex.Items.Clear();
        foreach (Mobius.CoreLibrary.Gender Gender in Enum.GetValues(typeof(Mobius.CoreLibrary.Gender)))
        {
            string name = Enum.GetName(typeof(Mobius.CoreLibrary.Gender), Gender);
            ddlSex.Items.Add(name);
        }

    }

    private void EditPatient(Patient patient)
    {
         BindGenderList();
      //  ddlSex.DataBind();
        //ddlSex.SelectedItem.Text = patient.Gender.ToString();
        ddlSex.SelectedIndex = ((Mobius.CoreLibrary.Gender)Enum.Parse(typeof(Mobius.CoreLibrary.Gender), patient.Gender.ToString(), true)).GetHashCode();
        txtSSN.Attributes["value"] = patient.SSN;
        txtDOB.Text = patient.DOB;

        txtUserEmail.Text = patient.EmailAddress;
        hdnPatientID.Value = patient.PatientId;
        txtMotherPrefix.Text = patient.MothersMaidenName.Prefix;
        txtMotherGivenName.Text = patient.MothersMaidenName.GivenName;
        txtMotherMiddleName.Text = patient.MothersMaidenName.MiddleName;
        txtMotherFamilyName.Text = patient.MothersMaidenName.FamilyName;
        txtMotherSuffix.Text = patient.MothersMaidenName.Suffix;
 
        hdnMPIID.Value = patient.LocalMPIID;
        Data data = new Data();
        data.Names = new List<Names>();
        Names name = null;
        int i = 0;
        foreach (string givenName in patient.GivenName)
        {
            name = new Names();
            name.IDName = patient.IDNames.ToArray()[i];
            name.FirstName = givenName;
            name.MiddleName = patient.MiddleName.ToArray()[i];
            name.LastName = patient.FamilyName.ToArray()[i];
            data.Names.Add(name);
            i++;
        }
        EditIndexName = -1;
        rptrName.DataSource = data.Names;
        rptrName.DataBind();

        EditIndexTelephone = -1;
        rptrTelephone.DataSource = data.Telephone;
        rptrTelephone.DataBind();

        EditIndexAddress = -1;
        rptr_Address.DataSource = data.Address;
        rptr_Address.DataBind();
    }

    /// <summary>
    /// This method initializes all repeater and other controls.
    /// </summary>
    private void LoadData()
    {
        Data data = new Data();
        rptrName.DataSource = data.Names;
        rptrName.DataBind();

        rptrTelephone.DataSource = data.Telephone;
        rptrTelephone.DataBind();

        rptr_Address.DataSource = data.Address;
        rptr_Address.DataBind();
    }

    private void BindAddressDetails(Data data)
    {

        if (EditIndexName != -1)
        {
            EditIndexName = -1;
            rptrName.DataSource = data.Names;
            rptrName.DataBind();

        }
        else if (EditIndexTelephone != -1)
        {
            EditIndexTelephone = -1;
            rptrTelephone.DataSource = data.Telephone;
            rptrTelephone.DataBind();

        }

    }

    private void BindNameDetails(Data data)
    {
        if (EditIndexAddress != -1)
        {
            EditIndexAddress = -1;
            rptr_Address.DataSource = data.Address;
            rptr_Address.DataBind();

        }
        else if (EditIndexTelephone != -1)
        {
            EditIndexTelephone = -1;
            rptrTelephone.DataSource = data.Telephone;
            rptrTelephone.DataBind();

        }
    }

    private void BindTelephoneDetails(Data data)
    {
        if (EditIndexAddress != -1)
        {
            EditIndexAddress = -1;
            rptr_Address.DataSource = data.Address;
            rptr_Address.DataBind();

        }
        else if (EditIndexName != -1)
        {
            EditIndexName = -1;
            rptrName.DataSource = data.Names;
            rptrName.DataBind();

        }
    }

    private List<ListItem> GetTypes()
    {
        List<ListItem> types = new List<ListItem>();
        types.Add(new ListItem("Work", "1"));
        types.Add(new ListItem("Home", "2"));
        types.Add(new ListItem("Mobile", "3"));
        return types;
    }

    private List<ListItem> TelephoneStatus()
    {
        List<ListItem> telePhoneStatus = new List<ListItem>();
        telePhoneStatus.Add(new ListItem("Active", "1"));
        telePhoneStatus.Add(new ListItem("InActive", "0"));
        return telePhoneStatus;
    }

    #endregion

}
